public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; } // Mot de passe crypté
    public string Role { get; private set; } // ex: "Admin", "User"

    public User(string username, string passwordHash, string role)
    {
        Id = Guid.NewGuid();
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
    }
}