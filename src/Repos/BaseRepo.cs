using ScissorLink.Data;

namespace ScissorLink.Repos
{
    public abstract class BaseRepo
    {
        protected readonly ScissorLinkDbContext _context;

        protected BaseRepo(ScissorLinkDbContext context)
        {
            _context = context;
        }
    }
}
