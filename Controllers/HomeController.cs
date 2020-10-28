using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Security.Cryptography;


namespace RandomCode.Controllers
{
    public class HomeController : Controller
    {

        private static Random rand = new Random();
        public static string RandomCode(int length)
        {
            // const string randjunk = "12345678910QWERTYUIOPASDFGHJKLZXCVBNM";
            // return new string(Enumerable.Repeat(randjunk, 14).Select(s => s[rand.Next(s.Length)]).ToArray());

            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }
            return res.ToString();
        }



        [HttpGet("")]
        public IActionResult Index(string code, int Ccount)   
        {   
            HttpContext.Session.SetInt32("genCount", Ccount);
            int? genCount = HttpContext.Session.GetInt32("genCount");                            
            ViewBag.code = code;
            ViewBag.genCount = genCount;
            return View();
        }



        [HttpGet("getcode")]
        public IActionResult NewCode()
        {
            string code = RandomCode(14);
            int? CCount = HttpContext.Session.GetInt32("genCount");
            CCount++;
            return RedirectToAction("Index", new{code = code, CCount});
        }



        [HttpGet("clear")]
        public IActionResult Clear()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }        
    }
}


