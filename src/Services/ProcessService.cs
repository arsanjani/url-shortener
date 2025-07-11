using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using akhr.ir.Models;
using akhr.ir.Repos.Interface;
using akhr.ir.Services.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace akhr.ir.Services;

public class ProcessService : IProcessService
{
    #region Constructor
    private readonly IProcessRepo rep;
    private readonly IMemoryCache cache;
    private const int CacheDuration = 10;
    
    public ProcessService(IProcessRepo rep, IMemoryCache cache)
    {
        this.rep = rep;
        this.cache = cache;
    }
    #endregion

    #region Implement Methods
    public async Task<DtoShortLink?> Get(string token)
    {
        string key = $"ShortLink-{token}";
        if (cache.TryGetValue(key, out DtoShortLink? result))
        {
            return result;
        }
        
        result = await rep.Get(token);
        if (result != null)
        {
            cache.Set(key, result, DateTime.Now.AddMinutes(CacheDuration));
        }
        return result;
    }

    public async Task<bool> Save(DtoShortLinkDetail dto)
    {
        return await rep.Save(dto);
    }
    #endregion
}
