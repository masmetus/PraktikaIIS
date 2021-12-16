using Ho4yZa4et.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Ho4yZa4et.Controllers
{
    public class HomeController : Controller
    {
        List<Product> products = new List<Product> { new Product(1, "Айфон", "Айфон шмайфон даааа", 22800), new Product(2, "Не айфон", "Каво", 11400), new Product(3, "Сяоми", "Топ за свои деньги", 111400) };

        private static string GET(string url, string Data)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(url + "?" + Data);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;


        }

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(dta dta) {

            dta.id = Guid.NewGuid();
            dta.action = "REG";
            dta.datte = DateTime.Now.ToString();
            GET("https://pay.pay-ok.org/demo/", "REQ={\"PAY_ID\":\"" + dta.id + "\",\"PAY_ACTION\":\"" + dta.action + "\",\"PAY_DATE\":\"" + dta.datte + "\",\"PAY_EMAIL\":\"" + dta.email + "\",\"PAY_LS\":\"" + dta.ls + "\",\"PAY_ITOG\":\"" + dta.itog + "\",\"PAY_NAME\":\"" + dta.name + "\"}");
            return RedirectToAction("status/" + dta.id);
        }


        //Ну чо, попарсим?
        public ActionResult status(string id) 
        {
            dta data = new dta();
            String status = GET("https://pay.pay-ok.org/demo/", "REQ={\"PAY_ID\":\"" + id + "\",\"PAY_ACTION\":\"GET_INFO\"}");
            int firstIndex = status.IndexOf("OD_STATUS")+ 12;
            int secondIndex = status.IndexOf("OD_PARAMS")- 3;
            data.status = status.Substring(firstIndex, secondIndex - firstIndex);
            firstIndex = status.IndexOf("processedAt") + 16;
            secondIndex = status.IndexOf("fp") - 5;
            data.datte = status.Substring(firstIndex, secondIndex - firstIndex);
            firstIndex = status.IndexOf("lsc") + 6;
            secondIndex = status.IndexOf("contacts") - 3;
            data.ls = Int32.Parse(status.Substring(firstIndex, secondIndex - firstIndex));
            firstIndex = status.IndexOf("PAY_NAME") + 17;
            secondIndex = status.IndexOf("type") - 8;
            data.name = status.Substring(firstIndex, secondIndex - firstIndex);
            firstIndex = status.IndexOf("companyINN") + 15;
            secondIndex = status.IndexOf("documentNumber") - 5;
            data.companyINN = status.Substring(firstIndex, secondIndex - firstIndex);
            firstIndex = status.IndexOf("documentNumber") + 17;
            secondIndex = status.IndexOf("shiftNumber") - 3;
            data.number = status.Substring(firstIndex, secondIndex - firstIndex);
            return View(data);
        }


        public ActionResult buy() {
            return View(products);
        }

        public ActionResult BUY1()
        {
            return View("BUY1");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BUY1([Bind(Exclude ="id")] int id, Product product) {

            string idProduct = "";
            foreach (Product p in products) 
            {
                String URL = "";
                String URL1 = "";
                if (p.id == id) 
                {
                    URL = GET("https://pay.pay-ok.org/demo/", "REQ={\"PAY_ACTION\": \"REG_PAYMENT\",\"PAY_ITOG\":\"" + p.price + "\",\"PAY_NAME\":\"Оплата товара: " + p.name + "\"}");
                    int firstIndex = URL.IndexOf("PAY_URL") + 10;
                    int secondIndex = URL.IndexOf("PAY_ID") - 3;
                    int thirdIndex = URL.IndexOf("PAY_ID") + 9;
                    int fourthIndex = URL.Length - 2;
                    idProduct = URL.Substring(thirdIndex, fourthIndex - thirdIndex);
                    URL1 = URL.Substring(firstIndex, secondIndex - firstIndex);
                    Process.Start(URL1);
                    Thread.Sleep(4);                  
                    return RedirectToAction("paystatus/" + idProduct);               
                }
            }
            return View("Payment");

        }

        public ActionResult paystatus(string id) 
        {
            dta dta = new dta();
            String URL = "";
            String URL1 = "";
            URL = GET("https://pay.pay-ok.org/demo/", "REQ={\"PAY_ACTION\": \"GET_PAYMENT_INFO\",\"PAY_ID\":\"" + id + "\"}");
            int firstIndex = URL.IndexOf("status") + 9;
            int secondIndex = URL.IndexOf("startAmount") - 3;
            URL1 = URL.Substring(firstIndex, secondIndex - firstIndex);
            if (URL1.Equals("1"))
            {
                dta.id = Guid.NewGuid();
                dta.action = "REG";
                dta.datte = DateTime.Now.ToString();
                dta.email = "Kavo@mail.ua";
                firstIndex = URL.IndexOf("с") + 2;
                secondIndex = URL.Length - 4;
                
                dta.ls = 43;
                firstIndex = URL.IndexOf("paidAmount") + 13;
                secondIndex = URL.IndexOf("order_id") - 3;
                dta.itog = Int32.Parse(URL.Substring(firstIndex, secondIndex - firstIndex));

                firstIndex = URL.IndexOf("PAY_NAME") + 11;
                secondIndex = URL.Length - 4;
                dta.name = URL.Substring(firstIndex, secondIndex - firstIndex);
                GET("https://pay.pay-ok.org/demo/", "REQ={\"PAY_ID\":\"" + dta.id + "\",\"PAY_ACTION\":\"" + dta.action + "\",\"PAY_DATE\":\"" + dta.datte + "\",\"PAY_EMAIL\":\"" + dta.email + "\",\"PAY_LS\":\"" + dta.ls + "\",\"PAY_ITOG\":\"" + dta.itog + "\",\"PAY_NAME\":\"" + dta.name + "\"}");
                return RedirectToAction("status/" + dta.id);
            }

            else
                return View("payWait");
        }
    }
}
