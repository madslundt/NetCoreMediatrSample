using System;
using Microsoft.AspNetCore.Identity;

namespace DataModel.Models
{
    public class User : IdentityUser<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override string Email { get; set; }

        public UserStatus Status { get; set; } = UserStatus.WaitingConfirmation;
        public UserStatusRef UserStatusRef { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
