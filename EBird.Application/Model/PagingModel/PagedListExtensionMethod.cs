using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.PagingModel
{
    public static class PagedListExtensionMethod
    {
        public static void MapMetaData<TDestination, TSource>(this PagedList<TDestination> destination, PagedList<TSource> source)
        {
            destination.TotalCount = source.TotalCount;
            destination.PageSize = source.PageSize;
            destination.CurrentPage = source.CurrentPage;
            destination.TotalPages = source.TotalPages;

        }
    }
}
