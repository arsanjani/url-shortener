using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScissorLink.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ErrorController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            // Set the response status code
            Response.StatusCode = code;

            // Serve the React app's index.html, which will handle the error routing client-side
            var indexPath = Path.Combine(_webHostEnvironment.WebRootPath, "index.html");

            if (System.IO.File.Exists(indexPath))
            {
                return PhysicalFile(indexPath, "text/html");
            }

            // Fallback if index.html doesn't exist
            return Content($"Error {code}: The page you requested could not be found.", "text/plain");
        }
    }
}
