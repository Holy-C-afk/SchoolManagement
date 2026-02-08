namespace ManagementSystem.API.Contracts.Auth;

// Les noms doivent être EXACTEMENT Username et Password (attention aux majuscules)
public record RegisterRequest(string Username, string Password);