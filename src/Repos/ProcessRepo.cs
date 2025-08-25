using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScissorLink.Models;
using ScissorLink.Repos.Interface;
using ScissorLink.Data;

namespace ScissorLink.Repos 
{
    public class ProcessRepo : BaseRepo, IProcessRepo
    {
        #region Constructor
        public ProcessRepo(ScissorLinkDbContext context) : base(context)
        {
        }
        #endregion

        #region Implement Methods
        public async Task<DtoShortLink?> Get(string token)
        {
            return await _context.ShortLinks
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<bool> Save(DtoShortLinkDetail dto)
        {
            try
            {
                _context.ShortLinkDetails.Add(dto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<DtoShortLink>> GetAll()
        {
            return await _context.ShortLinks
                .Include(x => x.Details)
                .OrderByDescending(x => x.CreateAdminDate)
                .ToListAsync();
        }

        public async Task<DtoShortLink?> GetById(int id)
        {
            return await _context.ShortLinks
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<DtoShortLink> Create(DtoShortLink shortLink)
        {
            _context.ShortLinks.Add(shortLink);
            await _context.SaveChangesAsync();
            return shortLink;
        }

        public async Task<DtoShortLink?> Update(DtoShortLink shortLink)
        {
            try
            {
                _context.ShortLinks.Update(shortLink);
                await _context.SaveChangesAsync();
                return shortLink;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var shortLink = await _context.ShortLinks.FindAsync(id);
                if (shortLink == null) return false;

                _context.ShortLinks.Remove(shortLink);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> TokenExists(string token)
        {
            return await _context.ShortLinks.AnyAsync(x => x.Token == token);
        }
        #endregion
    }
}
