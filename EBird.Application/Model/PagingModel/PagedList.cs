using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.PagingModel
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            
            TotalPages = (int) Math.Ceiling(count / (double)pageSize);
            
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> ToPageList(IQueryable<T> queryList, int pageNumber, int pageSize)
        {
            int count = queryList.Count();
            
            var items =  await queryList
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
