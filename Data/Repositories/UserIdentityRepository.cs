using Microsoft.AspNetCore.Identity;


namespace PizzaWebsite.Data.Repositories
{
    public interface IUserIdentityRepository
    {

    }

    public class UserIdentityRepository : IUserIdentityRepository
    {
        private readonly UserIdentityDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserIdentityRepository(UserIdentityDbContext context, 
                                        UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
    }
}
