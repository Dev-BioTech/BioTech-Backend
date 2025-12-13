namespace AuthService.Domain.Entities;

public class UserFarmRole
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int FarmId { get; set; }
    // Farm entity is in another microservice/context, so we only keep the ID here.
    // If we need Farm details, we would fetch them from the FarmService or a shared kernel if strictly necessary,
    // but for Auth, ID is usually enough for the token.

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
