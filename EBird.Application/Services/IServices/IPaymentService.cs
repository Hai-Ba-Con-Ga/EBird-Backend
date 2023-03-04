using EBird.Application.Model;
using EBird.Domain.Entities;
using Microsoft.Extensions.Primitives;

namespace EBird.Application.Services.IServices;
public interface IPaymentService
{
    Task<string> CreatePayment(CreatePayment payment, string origin);
    
    Task ProcessCallback(Dictionary<string, StringValues> queryData);

    Task<PaymentEntity> GetPaymentById(Guid paymentId);
    Task<List<PaymentEntity>> GetPayments();

    Task DeletePayment(Guid paymentId);
}