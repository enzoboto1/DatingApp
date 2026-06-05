public class AppUser
{
    public required string Id { get; set; }
    
    public string? DisplayName { get; set; }

    public string? Email { get; set; }

    public required byte[] PasswordHash { get; set; }

    public required byte[] PasswordSalt { get; set; }
    
}