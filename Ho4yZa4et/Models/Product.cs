using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ho4yZa4et.Models
{
    public class Product
    {
        public long id;
        public String name;
        public String descrip;
        public double price;

        public Product(long id, string name, string descrip, double price)
        {
            this.id = id;
            this.name = name;
            this.descrip = descrip;
            this.price = price;
        }

        public Product()
        {
        }
    }
}