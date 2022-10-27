using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCodeChems.DataAccess;

namespace OpenCodeChems.BusinessLogic.Interface
{
    public interface IUserManagement
    {
        bool RegisterUser(User user, string nikname);
        bool Login(string username, string password);
    }

}
