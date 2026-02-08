
using ManagementSystem.API.Models.Requests;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;
using ManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly ManagementDbContext _context;

    public StudentsController(ManagementDbContext context)
    {
        _context = context;
    }

    // POST: api/students
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentRequest request)
    {
        // Création de l'entité avec son ID typé (Value Object)
        var student = new Student(
            $"{request.FirstName} {request.LastName}",
            request.BirthDate
        );

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return Ok(new { id = student.Id.Value, message = "Étudiant créé avec succès !" });
    }

    // GET: api/students
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _context.Students.ToListAsync();
        return Ok(students);
    }
}