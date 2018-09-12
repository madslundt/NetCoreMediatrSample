namespace DataModel.Models
{
    public class UserStatusRef
    {
        public UserStatus Id { get; private set; }
        public string Name { get; private set; }

        public UserStatusRef()
        {}

        public UserStatusRef(UserStatus userStatus)
        {
            Id = userStatus;
            Name = userStatus.ToString();
        }
    }

    public enum UserStatus
    {
        WaitingConfirmation = 0,
        Active = 1,
        Deactive = 2
    }
}
