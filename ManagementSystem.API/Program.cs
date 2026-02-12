using System.Text;
using ManagementSystem.Application.Students;
using ManagementSystem.Infrastructure.Persistence; // Vérifie que ce namespace correspond à ton DbContext
using ManagementSystem.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ManagementSystem.Application; // Assure-toi que c'est le namespace exact défini dans ton fichier DependencyInjection.cs
using ManagementSystem.Application.Common.Interfaces;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IStudentService, StudentService>();
// Ajoute tous les services définis dans ton projet Application
builder.Services.AddScoped<IStudentRepository, StudentRepository>();


// --- 1. CONFIGURATION DU DBCONTEXT ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<ManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 2. CONFIGURATION DU CORS (Pour React) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173") // Port par défaut de Vite
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// --- 3. CONFIGURATION DE L'AUTHENTIFICATION JWT ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
    };
});

// --- 4. CONFIGURATION DE SWAGGER ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Management System API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez 'Bearer' [espace] puis votre jeton.\nExemple: \"Bearer eyJhbGci...\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
// Ajoute tous les services définis dans ton projet Application
builder.Services.AddApplication();
//Config QuestPDF
QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// --- 5. CONFIGURATION DU PIPELINE HTTP (L'ORDRE EST CRUCIAL) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Management System API V1");
        c.RoutePrefix = string.Empty; // Swagger à la racine
    });
}

app.UseHttpsRedirection();

// 1. Activer le CORS avant l'Auth
app.UseCors("AllowReactApp");

// 2. Activer l'Authentification (Qui es-tu ?)
app.UseAuthentication();

// 3. Activer l'Autorisation (As-tu le droit ?)
app.UseAuthorization();

app.MapControllers();

app.Run();