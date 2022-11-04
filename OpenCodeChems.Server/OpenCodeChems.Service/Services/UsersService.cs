using System.Threading.Tasks;
using Grpc.Core;
using static OpenCodeChems.Service.Users;

namespace OpenCodeChems.Service.Services
{
    public class UsersService : Users.UsersBase
    {
        private readonly UsersRepository _usersRepository;

        public UsersService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public override Task<StatusPetitionResponse> RegisterNewUsers(RegisterRequest request, ServerCallContext context)
        {
            var newUser = context.GetHttpContext().Items;
            return base.RegisterNewUsers(request, context);
        }

        public override Task<StatusPetitionResponse> LoginPlayer(LoginRequest request, ServerCallContext context)
        {
            return base.LoginPlayer(request, context);
        }
    }
}
