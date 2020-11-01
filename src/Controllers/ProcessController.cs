using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using akhr.ir.Models;
using akhr.ir.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using MaxMind.GeoIP2;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Caching.Memory;

namespace akhr.ir.Controllers
{
    [Route("")]
    public class ProcessController : Controller
    {
        #region Property
        IProcessService srv;
        IHostingEnvironment env;
        IMemoryCache cache;
        const int cacheDuration = 180;
        #endregion

        #region Constructor
        public ProcessController(IProcessService _srv, IHostingEnvironment _env, IMemoryCache _cache)
        {
            srv = _srv;
            env = _env;
            cache = _cache;
        }
        #endregion

        #region GET

        [HttpGet]
        public ActionResult Get()
        {
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            DtoShortLinkDetail dtoDetail = new DtoShortLinkDetail();
            var dto = await srv.Get(id);
            if (dto == null || dto.ID == 0 || !dto.IsPublish)
            {
                return NotFound();
            }
            dtoDetail.ShortLinkID = dto.ID;
            try
            {
                #region GET BROWSER INFO
                var ua = Request.Headers["User-Agent"];
                UserAgent.UserAgent userAgent = new UserAgent.UserAgent(ua);
                dtoDetail.OS = userAgent.OS.Name;
                dtoDetail.Browser = userAgent.Browser.Name;
                #endregion

                #region GET IP
                string ip;
                ip = SplitCsv(GetHeaderValueAs<string>("X-Forwarded-For")).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(ip) && HttpContext?.Connection?.RemoteIpAddress != null)
                    ip = HttpContext.Connection.RemoteIpAddress.ToString();

                if (string.IsNullOrWhiteSpace(ip))
                    ip = GetHeaderValueAs<string>("REMOTE_ADDR");
                #endregion

                #region GEO LOCATION
                string token = string.Format("ip-{0}", ip);
                if (cache.TryGetValue(token, out string country))
                {
                    dtoDetail.Country = country;
                }
                else
                {
                    using (var reader = new DatabaseReader(env.ContentRootPath + "\\wwwroot\\GeoLite2-Country.mmdb", MaxMind.Db.FileAccessMode.Memory))
                    {
                        var obj = reader.Country(ip);
                        dtoDetail.Country = obj.Country.Name;
                        cache.Set(token, obj.Country.Name, DateTime.Now.AddMinutes(cacheDuration));
                    }
                }

                if (dtoDetail.Country == null)
                    dtoDetail.Country = ip;
                #endregion
            }
            catch
            {
            }
            await srv.Save(dtoDetail);
            return Redirect(dto.OriginLink);
        } 
        #endregion

        #region Utils
        public T GetHeaderValueAs<T>(string headerName)
        {

            if (HttpContext?.Request?.Headers?.TryGetValue(headerName, out StringValues values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!String.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        public List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
        #endregion

    }
}
