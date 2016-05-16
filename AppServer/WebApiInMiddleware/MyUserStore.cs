using System;
using System.Data.Entity;
using System.Threading.Tasks;
using WebApiInMiddleware.Models;

namespace WebApiInMiddleware
{
    public class MyUserStore
    {
        ApplicationDbContext _db;

        public MyUserStore(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task AddUserSync(MyUser user, string password)
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

        public async Task<MyUser> FindByEmailAsync(string email)
        {
            MyUser user = await
                          _db.Users
                             .Include(c => c.Claims)
                             .FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<MyUser> FindByIdAsync(string userId)
        {
            MyUser user = await
                          _db.Users
                             .FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<bool> UserExists(MyUser user)
        {
            return await _db.Users.AnyAsync(u => u.Id == user.Id || u.Email == user.Email);
        }

        public async Task AddClaimAsync(string userId, MyUserClaim claim)
        {
            MyUser user = await FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            user.Claims.Add(claim);
            await _db.SaveChangesAsync();
        }

        public bool PasswordIsValid(MyUser user, string password)
        {
            MyPasswordHasher hasher = new MyPasswordHasher();
            string hash = hasher.CreateHash(password);
            return hash.Equals(user.PasswordHash);

        }
    }
}
