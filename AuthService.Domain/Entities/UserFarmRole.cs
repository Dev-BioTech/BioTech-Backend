namespace AuthService.Domain.Entities;

public class UserFarmRole
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int FarmId { get; set; }
    public Farm Farm { get; set; } = null!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
