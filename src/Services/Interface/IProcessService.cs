﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using akhr.ir.Models;

namespace akhr.ir.Services.Interface
{
    public interface IProcessService
    {
        Task<DtoShortLink?> Get(string token);
        Task<bool> Save(DtoShortLinkDetail dto);
    }
}
