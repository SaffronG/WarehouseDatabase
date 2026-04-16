using System;

namespace Backend.Entities;

public partial class AppUser
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
