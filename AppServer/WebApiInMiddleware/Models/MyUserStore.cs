using System;
using System.Data.Entity;
using System.Threading.Tasks;
using WebApiInMiddleware.Models;

namespace WebApiInMiddleware.Models
{
    public class MyUserStore
    {
        ApplicationDbContext _db;

        public MyUserStore(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task AddUserSync(AuthModel user, string password)
        {
            if (await UserExists(user))
            {
                throw new Exception("A User with that Email address already exists");
            }
            MyPasswordHasher hasher = new MyPasswordHasher();
            user.PasswordHash = hasher.CreateHash(password).ToString();
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<AuthModel> FindByEmailAsync(string email)
        {
            AuthModel user = await
                          _db.Users
                             .Include(c => c.Claims)
                             .FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<AuthModel> FindByIdAsync(string userId)
        {
            AuthModel user = await
                          _db.Users
                             .FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<bool> UserExists(AuthModel user)
        {
            return await _db.Users.AnyAsync(u => u.Id == user.Id || u.Email == user.Email);
        }

        public async Task AddClaimAsync(string userId, MyUserClaim claim)
        {
            AuthModel user = await FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            user.Claims.Add(claim);
            await _db.SaveChangesAsync();
        }

        public bool PasswordIsValid(AuthModel user, string password)
        {
            MyPasswordHasher hasher = new MyPasswordHasher();
            string hash = hasher.CreateHash(password);
            return hash.Equals(user.PasswordHash);

        }
    }
}
