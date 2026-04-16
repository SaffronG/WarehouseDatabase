using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public sealed class JwtSettings
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationHours { get; set; } = 8;
}

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthService(AppDbContext context, IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<AppUser> RegisterAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters long.", nameof(password));

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
        if (existing is not null)
            throw new InvalidOperationException("A user with that email already exists.");

        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        var user = new AppUser
        {
            Email = normalizedEmail,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Email and password are required.");

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
        if (user is null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return GenerateJwtToken(user);
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var rng = RandomNumberGenerator.Create();
        passwordSalt = new byte[16];
        rng.GetBytes(passwordSalt);
        passwordHash = Rfc2898DeriveBytes.Pbkdf2(password, passwordSalt, 150_000, HashAlgorithmName.SHA256, 32);
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        var computedHash = Rfc2898DeriveBytes.Pbkdf2(password, storedSalt, 150_000, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }

    private string GenerateJwtToken(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
