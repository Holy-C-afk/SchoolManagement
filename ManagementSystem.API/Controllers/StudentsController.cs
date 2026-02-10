
using ManagementSystem.API.Models.Requests;
using ManagementSystem.Application.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // POST: api/students
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentRequest request)
    {
        if (!DateOnly.TryParse(request.BirthDate, out var birthDate))
        {
            return BadRequest("BirthDate invalide. Format attendu: YYYY-MM-DD");
        }

        var command = new CreateStudentCommand(
            $"{request.FirstName} {request.LastName}",
            birthDate
        );

        var id = await _studentService.CreateAsync(command);

        return Ok(new { id, message = "Étudiant créé avec succès !" });
    }

    // GET: api/students?search=...
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var students = await _studentService.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            students = students
                .Where(s => !string.IsNullOrEmpty(s.fullName) &&
                            s.fullName.ToLower().Contains(term))
                .ToList();
        }

        // On normalise la forme JSON pour le front (id, fullName, birthDate)
        var response = students.Select(s => new
        {
            id = s.id,
            fullName = s.fullName,
            birthDate = s.dateOfBirth
        });

        return Ok(response);
    }
    //PUT: api/students/{id}
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateStudentRequest request)
    {
        var command = new UpdateStudentCommand(
            id,
            request.FirstName,
            request.LastName,
            request.BirthDate
        );

        await _studentService.UpdateAsync(id, command);
        return NoContent();
    }

    // DELETE: api/students/{id}
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _studentService.DeleteAsync(id);
        return NoContent();
    }

    // POST: api/students/bulk
    [Authorize]
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate([FromBody] IEnumerable<CreateStudentRequest> requests)
    {
        var commands = new List<CreateStudentCommand>();

        foreach (var r in requests)
        {
            if (!DateOnly.TryParse(r.BirthDate, out var birthDate))
            {
                return BadRequest($"BirthDate invalide pour l'étudiant {r.FirstName} {r.LastName}. Format attendu: YYYY-MM-DD");
            }

            commands.Add(new CreateStudentCommand(
                $"{r.FirstName} {r.LastName}",
                birthDate
            ));
        }

        var ids = await _studentService.BulkCreateAsync(commands);

        return Ok(new { ids });
    }
}
