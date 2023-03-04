using System.Globalization;
using System.Net;
using System.Text;
using AutoMapper;
using EBird.Application.AppConfig;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Application.Utilities;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace EBird.Application.Services;
public class VnPayService : IPaymentService
{
    public const string VERSION = "2.1.0";
    private IMapper _mapper;
    private SortedList<string, string> _requestData = new SortedList<string, string>();
    private SortedList<string, string> _responseData = new SortedList<string, string>();

    private readonly VnpayConfig _config;
    private readonly IGenericRepository<PaymentEntity> _paymentRepository;
    private readonly IGenericRepository<VipRegistrationEntity> _vipRegistrationRepository;

    public VnPayService(IMapper mapper, IOptions<VnpayConfig> config, IGenericRepository<PaymentEntity> paymentRepository, IGenericRepository<VipRegistrationEntity> vipRegistrationRepository)
    {
        _mapper = mapper;
        _config = config.Value;
        _paymentRepository = paymentRepository;
        _vipRegistrationRepository = vipRegistrationRepository;
    }
    

    #region Request
    public void AddRequestData(string key, string value)
    {
        if (!String.IsNullOrEmpty(key))
        {
            _requestData.Add(key, value);
        }
    }
    public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
    {
        StringBuilder data = new StringBuilder();
        foreach (KeyValuePair<string, string> kv in _requestData)
        {
            if (!String.IsNullOrEmpty(kv.Value))
            {
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
            }
        }
        string queryString = data.ToString();
        baseUrl += "?" + queryString;
        String signData = queryString;
        if (signData.Length > 0)
        {
            signData = signData.Remove(data.Length - 1, 1);
        }
        string vnp_SecureHash = PaymentUtil.HmacSHA512(vnp_HashSecret, signData);
        baseUrl += "vnp_SecureHash=" + vnp_SecureHash;
        return baseUrl;
    }

    #endregion 

    #region Response
    public void AddResponseData(string key, string value)
    {
        if (!String.IsNullOrEmpty(value))
        {
            _responseData.Add(key, value);
        }
    }

    public string GetResponseData(string key)
    {
        string retValue;
        if (_responseData.TryGetValue(key, out retValue))
        {
            return retValue;
        }
        else
        {
            return string.Empty;
        }
    }
    public bool ValidateSignature(string inputHash, string secretKey)
    {
        string rspRaw = GetResponseData();
        string myChecksum = PaymentUtil.HmacSHA512(secretKey, rspRaw);
        return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }
    private string GetResponseData()
    {
        StringBuilder data = new StringBuilder();
        if (_responseData.ContainsKey("vnp_SecureHashType"))
        {
            _responseData.Remove("vnp_SecureHashType");
        }

        if (_responseData.ContainsKey("vnp_SecureHash"))
        {
            _responseData.Remove("vnp_SecureHash");
        }

        foreach (KeyValuePair<string, string> kv in _responseData)
        {
            if (!String.IsNullOrEmpty(kv.Value))
            {
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
            }
        }

        //remove last '&'
        if (data.Length > 0)
        {
            data.Remove(data.Length - 1, 1);
        }

        return data.ToString();
    }

    #endregion
    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
    public async Task<string> CreatePayment(CreatePayment payment, string origin)
    {
        _requestData.Clear();
        var vnp_Returnurl = origin + _config.ReturnUrl;
        var vnp_Url = _config.Url;
        var vnp_TmnCode = _config.TmnCode;
        var vnp_HashSecret = _config.HashSecret;
        var entity = _mapper.Map<PaymentEntity>(payment);
        await _paymentRepository.CreateAsync(entity);

        // if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
        // {
        //     lblMessage.Text = "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config";
        //     return;
        // }
        AddRequestData("vnp_Version", VERSION);
        AddRequestData("vnp_Command", "pay");
        AddRequestData("vnp_TmnCode", vnp_TmnCode);
        AddRequestData("vnp_Amount", ((long)entity.Amount * 100).ToString());
        AddRequestData("vnp_CreateDate", entity.CreatedDate.ToString("yyyyMMddHHmmss"));
        AddRequestData("vnp_CurrCode", "VND");
        AddRequestData("vnp_IpAddr", "");
        AddRequestData("vnp_Locale", "vn");
        AddRequestData("vnp_OrderInfo", "Thanh toan don hang: " + entity.Id);
        AddRequestData("vnp_OrderType", "other");
        AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        var txnRef = entity.Id + "|" + "" + payment.LimitMonth+ "|" + DateTime.Now.Millisecond;
        AddRequestData("vnp_TxnRef", txnRef); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
        AddRequestData("vnp_ExpireDate", "20230924083000");
        //AddRequestData("vnp_Bill_AccountId", entity.AccountId.ToString());
        string paymentUrl = CreateRequestUrl(vnp_Url, vnp_HashSecret);
        return paymentUrl;
    }
    public async Task ProcessCallback(Dictionary<string, StringValues> queryData)
    {
        string msg = "";
        int isSuccess = 0;
        if (queryData.Count > 0)
        {
            string vnp_HashSecret = _config.HashSecret;
            var vnpData = queryData;
            _responseData.Clear();
            foreach (string s in vnpData.Keys)
            {
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    AddResponseData(s, vnpData[s]);
                }
            }
            //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
            //vnp_TransactionNo: Ma GD tai he thong VNPAY
            //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
            //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

            Guid paymentId = Guid.Parse(GetResponseData("vnp_TxnRef").Split('|')[0]);
            long vnpTranId = Convert.ToInt64(GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = GetResponseData("vnp_ResponseCode");
            String vnp_SecureHash = queryData["vnp_SecureHash"];
            string vnp_TransactionStatus = GetResponseData("vnp_TransactionStatus");
            long vnp_Amount = Convert.ToInt64(GetResponseData("vnp_Amount")) / 100;
            String bankCode = queryData["vnp_BankCode"];
            bool checkSignature = ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            if (checkSignature)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    var entity = await _paymentRepository.GetByIdActiveAsync(paymentId);
                    if (entity != null)
                    {
                        // entity.AccountId = accountId;
                        entity.Status = PaymentStatus.Paid;
                        await _paymentRepository.UpdateAsync(entity);
                        var limitMonth = int.Parse(GetResponseData("vnp_TxnRef").Split('|')[1]);
                        await CreateVip(entity.AccountId, paymentId, limitMonth);
                    }
                    isSuccess = 1;
                }
                else
                {
                    var entity = await _paymentRepository.GetByIdActiveAsync(paymentId);
                    if (entity != null)
                    {
                        entity.Status = PaymentStatus.Failed;
                        await _paymentRepository.UpdateAsync(entity);
                    }
                    msg = string.Format("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}",
                             paymentId, vnpTranId, vnp_ResponseCode);

                }

            }
            else
            {
                msg = "There is an error during the process!";

            }
        }
        queryData.Add("be_msg", msg);
        queryData.Add("isSuccess", isSuccess.ToString());
    }
    private async Task CreateVip(Guid accountId, Guid paymentId, int LimitMonth)
    {
        var entity = new VipRegistrationEntity
        {
            AccountId = accountId,
            CreatedDate = DateTime.Now,
            ExpiredDate = DateTime.Now.AddMonths(LimitMonth),
            PaymentId = paymentId
        };
        await _vipRegistrationRepository.CreateAsync(entity);
    }

    public async Task<PaymentEntity> GetPaymentById(Guid paymentId)
    {
        var payment = await _paymentRepository.GetByIdActiveAsync(paymentId);
        if(payment == null){
            throw new NotFoundException("Payment not found");
        }
        return payment;
    }

    public async Task<List<PaymentEntity>> GetPayments()
    {
        var payments = await _paymentRepository.GetAllActiveAsync();
        if(payments == null){
            throw new NotFoundException("No payments found");
        }
        return payments;
    }

    public async Task DeletePayment(Guid paymentId)
    {
        await _paymentRepository.DeleteSoftAsync(paymentId);
    }
}