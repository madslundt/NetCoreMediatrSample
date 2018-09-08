using DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Src.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DatabaseContext _db;

        public UserHelper(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<bool> DoesUserExistById(Guid id)
        {
            var result = await _db.Users.AnyAsync(user => user.Id == id).ConfigureAwait(false);
            
            return result;
        }

        public async Task<bool> DoesUserExistByEmail(string email)
        {
            var temp = await _db.Users.ToListAsync();
            var result = await _db.Users.AnyAsync(user => user.Email == email).ConfigureAwait(false);

            return result;
        }
    }
}
