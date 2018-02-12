using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoticeBoard.Models
{
    public class NBUser
    {
        public int Id { get; set; }

        public string OpenId { get; set; }

        public string NickName { get; set; }


        public DateTime CreateDate { get; set; }
    }
}