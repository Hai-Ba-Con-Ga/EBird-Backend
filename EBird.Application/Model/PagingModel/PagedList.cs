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
        public int PageSize { get; set; } = 0;
        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList()
        {
        }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            
            TotalPages = (int) Math.Ceiling(count / (double)pageSize);
            
            this.AddRange(items);
        }

        public async Task LoadData(IQueryable<T> queryList, int pageNumber, int pageSize)
        {
            int count = queryList.Count();
            this.TotalCount = count;
            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;
            this.TotalPages = (int) Math.Ceiling(count / (double) pageSize);

            var items = await queryList
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            this.AddRange(items);
        }

        public async Task LoadData(IQueryable<T> queryList)
        {
            int count = queryList.Count();
            this.TotalCount = count;

            var items = await queryList.ToListAsync();
            this.AddRange(items);
        }
    }
}
