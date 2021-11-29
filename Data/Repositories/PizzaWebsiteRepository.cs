using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Repositories
{
    public interface IPizzaWebsiteRepository
    {
        List<UserData> GetAllUserDatas();
        UserData GetUserDataByUserId(string userId);
        void Update(UserData userData);

        List<Product> GetAllProducts();
        
        bool SaveAll();
    }

    public class PizzaWebsiteRepository : IPizzaWebsiteRepository
    {
        private readonly PizzaWebsiteDbContext _context;
        private readonly ILogger<PizzaWebsiteRepository> _logger;

        public PizzaWebsiteRepository(ILogger<PizzaWebsiteRepository> logger, PizzaWebsiteDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<UserData> GetAllUserDatas()
        {
            try
            {
                _logger.LogInformation("Getting all user data ...");

                return _context.UserDatas.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all user data: {e}");

                return null;
            }
        }

        public UserData GetUserDataByUserId(string userId)
        {
            try
            {
                _logger.LogInformation("Getting user data by user id ...");

                return _context.UserDatas.FirstOrDefault(ud => ud.UserId == userId);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get user data by user id: {e}");

                return null;
            }
        }

        public void Update(UserData userData)
        {
            try
            {
                _logger.LogInformation("Getting user data by user id ...");

                _context.Update(userData);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get user data by user id: {e}");
            }
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Getting all products ...");

                return _context.Products.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all products: {e}");

                return null;
            }
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
