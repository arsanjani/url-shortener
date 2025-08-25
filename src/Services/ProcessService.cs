using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScissorLink.Models;
using ScissorLink.Repos.Interface;
using ScissorLink.Services.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace ScissorLink.Services;

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

    public async Task<List<DtoShortLink>> GetAll()
    {
        return await rep.GetAll();
    }

    public async Task<DtoShortLink?> GetById(int id)
    {
        return await rep.GetById(id);
    }

    public async Task<DtoShortLink> Create(DtoShortLink shortLink)
    {
        // Clear cache if needed
        InvalidateCache(shortLink.Token);
        return await rep.Create(shortLink);
    }

    public async Task<DtoShortLink?> Update(DtoShortLink shortLink)
    {
        // Clear cache
        InvalidateCache(shortLink.Token);
        return await rep.Update(shortLink);
    }

    public async Task<bool> Delete(int id)
    {
        var shortLink = await rep.GetById(id);
        if (shortLink != null)
        {
            InvalidateCache(shortLink.Token);
        }
        return await rep.Delete(id);
    }

    public async Task<string> GenerateUniqueToken()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        string token;
        
        do
        {
            token = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        while (await rep.TokenExists(token));
        
        return token;
    }

    private void InvalidateCache(string token)
    {
        string key = $"ShortLink-{token}";
        cache.Remove(key);
    }
    #endregion
}
