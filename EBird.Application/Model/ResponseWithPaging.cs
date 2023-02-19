using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.PagingModel;

namespace Response
{
    public class ResponseWithPaging<T> : Response<T>
    {
        public PagingData? PagingData { get; set; }

        public new static ResponseWithPaging<T> Builder() => new ResponseWithPaging<T>();

        public dynamic SetPagingData(PagingData pagingData)
        {
            PagingData = pagingData;
            return this;
        }
    }
}