namespace RealEstate.Contracts.DTOs
{
    public class AuthDto
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}
