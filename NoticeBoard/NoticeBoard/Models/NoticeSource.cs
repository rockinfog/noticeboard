using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoticeBoard.Models
{
    public class NoticeSource
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Domain { get; set; }

        public string NoticePath { get; set; }

        public bool Active { get; set; }

        public string RegexRule { get; set; }

    }
}