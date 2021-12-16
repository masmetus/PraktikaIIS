using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ho4yZa4et.Models
{
    public class dta
    {
        public Guid id { get; set; }
        public String action { get; set; }
        public String datte { get; set; }
        public String email { get; set; }
        public int ls { get; set; }
        public int itog { get; set; }
        public String name { get; set; }
        public String companyINN { get; set; }

        public String number { get; set; }


        public String status { get; set; }

        public dta(string email, int ls, int itog, string name, string status, string companyINN, string number)
        {
            id = Guid.NewGuid();
            action = "REG";
            this.datte = DateTime.Now.ToString();
            this.email = email;
            this.ls = ls;
            this.itog = itog;
            this.name = name;
            this.status = status;
            this.companyINN = companyINN;
            this.number = number;
        }

        public dta()
        {
        }
    }
}