using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScissorLink.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            ViewBag.PageTitle = $"Oops! {code}";
            ViewBag.Title = "!این راه به جایی نمیرسد";
            ViewBag.SubTitle = "شرمنده، به نظر می‌رسد مشکلی پدید آمده باشد. صفحه درخواستی پیدا نشد که نشد";
            return View("Views/404.cshtml");
        }
    }
}
