using Microsoft.AspNetCore.Identity;
using System;

namespace DataModel.Models
{
    public class User : IdentityUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public UserStatus Status { get; set; } = UserStatus.WaitingConfirmation;
        public UserStatusRef UserStatusRef { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
