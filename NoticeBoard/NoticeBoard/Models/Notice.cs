using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoticeBoard.Models
{
    public class Notice 
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Source { get; set; }

        public DateTime CreateDate { get; set; }


    }


    public class NoticeComparer : IEqualityComparer<Notice>
    {
        public bool Equals(Notice x, Notice y)
        {
            if (x.Title == y.Title )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(Notice obj)
        {
            return 0;
        }
    }
}