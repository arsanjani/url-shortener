using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using akhr.ir.Models;
using akhr.ir.Repos.Interface;
using akhr.ir.Services.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace akhr.ir.Services
{
    public class ProcessService : IProcessService
    {
        #region Constructor
        IProcessRepo rep;
        IMemoryCache cache;
        const int cacheDuration = 10;
        
        public ProcessService(IProcessRepo _rep, IMemoryCache _cache)
        {
            rep = _rep;
            cache = _cache;
        }
        #endregion

        #region Implement Methods
        public async Task<DtoShortLink> Get(string token)
        {
            string key = string.Format("ShortLink-{0}", token);
            if (cache.TryGetValue(key, out DtoShortLink result))
            {
                return result;
            }
            else
            {
                result = await rep.Get(token);
                cache.Set(key, result, DateTime.Now.AddMinutes(cacheDuration));
                return result;
            }
            
        }

        public async Task<bool> Save(DtoShortLinkDetail dto)
        {
            return await rep.Save(dto);
        }
        #endregion
    }
}
