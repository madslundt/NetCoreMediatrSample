using DataModel.Models.Users;

namespace Api.Dtos.UserTaskDtos;

public class UpdateUserTaskBodyDto
{
    public UserId AssignToUserId { get; init; } = null!;
}