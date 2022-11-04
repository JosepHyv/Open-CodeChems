using OpenCodeChems.DataAccess;
using OpenCodeChems.BusinessLogic;
namespace OpenCodeChems.Service.Services
{
    
    public class UsersRepository
    {
        private readonly ILogger _logger;
        private UserManagement management = new UserManagement();

        public UsersRepository(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UsersRepository>();
        }

        public bool RegisterNewUser(string username, string name, string password, string email, string nickname)
        {
            User newUser = new User(username, password, name, email);
            bool status = this.management.RegisterUser(newUser, nickname);
            if (!status)
            {
                _logger.LogError($"{username} or {email} already exist");
            }
            return status;
        }

        public bool LoginPlayer(string username, string password)
        {
            bool status = this.management.Login(username, password);
            if (!status)
            {
                _logger.LogError("cannot log in");
            }

            return status;
        }
        
        
    }    
}

