using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCodeChems.Server.DataAccess;

namespace OpenCodeChems.Server.BusinessLogic.OpenCodeChems.Server.Contracts
{
    internal interface IUserManagement
    {
        string ComputeSHA256Hash(string text);

        bool RegisterUser(User user, string nickname);

        bool Login(string username, string password);
    }
}
