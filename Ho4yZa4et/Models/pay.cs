using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ho4yZa4et.Models
{
    public class payment
    {
        public string id { get; set; }
        public string status { get; set; }

        public payment(string id, string status)
        {
            this.id = id;
            this.status = status;

        }

        public payment()
        {
        }
    }
}