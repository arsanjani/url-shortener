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
using System.IO;

namespace akhr.ir.Controllers
{
    [Route("")]
    public class ProcessController : Controller
    {
        #region Property
        private readonly IProcessService srv;
        private readonly IWebHostEnvironment env;
        private readonly IMemoryCache cache;
        private const int CacheDuration = 180;
        #endregion

        #region Constructor
        public ProcessController(IProcessService srv, IWebHostEnvironment env, IMemoryCache cache)
        {
            this.srv = srv;
            this.env = env;
            this.cache = cache;
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
            var dtoDetail = new DtoShortLinkDetail();
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
                var userAgent = new UserAgent.UserAgent(ua!);
                dtoDetail.OS = userAgent.OS.Name;
                dtoDetail.Browser = userAgent.Browser.Name;
                #endregion

                #region GET IP
                string? ip = SplitCsv(GetHeaderValueAs<string>("X-Forwarded-For")).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(ip) && HttpContext?.Connection?.RemoteIpAddress != null)
                    ip = HttpContext.Connection.RemoteIpAddress.ToString();

                if (string.IsNullOrWhiteSpace(ip))
                    ip = GetHeaderValueAs<string>("REMOTE_ADDR");
                #endregion

                #region GEO LOCATION
                if (!string.IsNullOrWhiteSpace(ip))
                {
                    string token = $"ip-{ip}";
                    if (cache.TryGetValue(token, out string? country))
                    {
                        dtoDetail.Country = country;
                    }
                    else
                    {
                        var geoDbPath = Path.Combine(env.ContentRootPath, "wwwroot", "GeoLite2-Country.mmdb");
                        using var reader = new DatabaseReader(geoDbPath, MaxMind.Db.FileAccessMode.Memory);
                        var obj = reader.Country(ip);
                        dtoDetail.Country = obj.Country.Name;
                        cache.Set(token, obj.Country.Name, DateTime.Now.AddMinutes(CacheDuration));
                    }

                    dtoDetail.Country ??= ip;
                }
                #endregion
            }
            catch
            {
                // Log exception in production
            }
            await srv.Save(dtoDetail);
            return Redirect(dto.OriginLink);
        } 
        #endregion

        #region Utils
        public T? GetHeaderValueAs<T>(string headerName)
        {
            if (HttpContext?.Request?.Headers?.TryGetValue(headerName, out StringValues values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T?)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default;
        }

        public List<string> SplitCsv(string? csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? new List<string>() : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .Select(s => s.Trim())
                .ToList();
        }
        #endregion
    }
}
