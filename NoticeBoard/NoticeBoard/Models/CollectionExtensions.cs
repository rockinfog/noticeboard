using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoticeBoard.Models
{
    public static class CollectionExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> collection, int page, int pageSize, int rowCount)
        {
            if (collection == null) throw new System.ArgumentNullException("collection");
            return new PagedList<T>()
            {
                Results = collection.ToList(),
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    PageCount = (int)Math.Ceiling((double)rowCount / pageSize),
                    RowCount = rowCount
                }
            };
        }
    }
}