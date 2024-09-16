namespace Qwitter.User.Contract.Auth.Models;

public class RegisterRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? ReferralCode { get; set; }
}