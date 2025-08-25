using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScissorLink.Models;

namespace ScissorLink.Repos.Interface
{
    public interface IProcessRepo
    {
        Task<DtoShortLink?> Get(string token);
        Task<bool> Save(DtoShortLinkDetail dto);
        Task<List<DtoShortLink>> GetAll();
        Task<DtoShortLink?> GetById(int id);
        Task<DtoShortLink> Create(DtoShortLink shortLink);
        Task<DtoShortLink?> Update(DtoShortLink shortLink);
        Task<bool> Delete(int id);
        Task<bool> TokenExists(string token);
    }
}
