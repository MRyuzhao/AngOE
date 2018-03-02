using AngOE.Entities;
using AngOE.Repository.IRepository;

namespace AngOE.Repository.Impl
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}