using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoticeBoard.Models
{
    public class Pagination
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }


    public class PagedList<T>
    {
        public IList<T> Results { get; set; }

        public Pagination Pagination { get; set; }

    }
}