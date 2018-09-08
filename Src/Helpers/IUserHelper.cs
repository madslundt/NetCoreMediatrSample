using System;
using System.Threading.Tasks;

namespace Src.Helpers
{
    public interface IUserHelper
    {
        Task<bool> DoesUserExistById(Guid id);

        Task<bool> DoesUserExistByEmail(string email);
    }
}
