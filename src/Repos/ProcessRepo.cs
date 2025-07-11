using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using akhr.ir.Models;
using akhr.ir.Repos.Interface;
using Dapper;
using Microsoft.Extensions.Configuration;
using Dapper.Contrib.Extensions;

namespace akhr.ir.Repos 
{
    public class ProcessRepo : BaseRepo, IProcessRepo
    {
        #region Constructor
        public ProcessRepo(IConfiguration config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }
        #endregion

        #region Implement Methods
        public async Task<DtoShortLink?> Get(string token)
        {
            if (con == null)
                return null;
            
            return await con.GetAsync<DtoShortLink>(token);
        }

        public async Task<bool> Save(DtoShortLinkDetail dto)
        {
            if (con == null)
                return false;
            
            await con.InsertAsync(dto);
            return true;
        }
        #endregion
    }
}
