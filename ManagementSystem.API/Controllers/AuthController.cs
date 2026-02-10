using ManagementSystem.API.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ManagementSystem.Infrastructure.Persistence; // Chemin vers ton DbContext
using ManagementSystem.Domain.Entities;          // Chemin vers ton entité User
            
// On définit des alias pour éviter l'erreur d'ambiguïté avec Microsoft.AspNetCore.Identity
using LoginRequest = ManagementSystem.API.DTOs.LoginRequest;
using RegisterRequest = ManagementSystem.API.Contracts.Auth.RegisterRequest;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ManagementDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(ManagementDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // 1. Vérifier si l'utilisateur existe déjà en BD
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            return BadRequest("Cet utilisateur existe déjà.");

        // 2. Hasher le mot de passe
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        // 3. Créer l'utilisateur dans la BD (Utilise ton entité Domain.User)
        var user = new User(request.Username, passwordHash, "Admin");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return Ok(new { message = "Utilisateur créé avec succès dans la base de données !" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 1. Chercher l'utilisateur réel dans ta table Users
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        
        // 2. Comparer le hash stocké avec le mot de passe saisi
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Identifiants incorrects.");

        // 3. Générer le Token pour cet utilisateur spécifique
        var token = GenerateToken(user);
        // on renvoie "token" en camelCase pour correspondre au front React
        return Ok(new { token });
    }

    private string GenerateToken(User user)
    {
        // On récupère la clé secrète depuis appsettings.json
        var secretKey = _config["JwtSettings:Secret"];
        if (string.IsNullOrEmpty(secretKey)) 
            throw new InvalidOperationException("La clé JWT 'Secret' est manquante dans appsettings.json");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        
        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: new[] { 
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Ajoute l'ID de l'user dans le token
            },
            expires: DateTime.Now.AddHours(2),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}