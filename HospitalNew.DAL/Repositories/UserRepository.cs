using HospitalNew.DAL.Data;
using HospitalNew.DAL.Interfaces;
using HospitalNew.DAL.Models;

namespace HospitalNew.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public User IsValid(string name)
        {
            var user = _context.Users.SingleOrDefault(x => x.Name == name);
            return user;
        }
    }
}
