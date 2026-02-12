
using ManagementSystem.API.Models.Requests;
using ManagementSystem.Application.Students;
using ManagementSystem.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IPdfService _pdfService;

    public StudentsController(IStudentService studentService, IPdfService pdfService)
    {
        _studentService = studentService;
        _pdfService = pdfService;
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

    // GET: api/students
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAllAsync();

        // On normalise la forme JSON pour le front (id, fullName, birthDate)
        var response = students.Select(s => new
        {
            id = s.id,
            fullName = s.fullName,
            birthDate = s.dateOfBirth
        });

        return Ok(response);
    }
    //[Authorize]
    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null)
    {
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest("pageNumber et pageSize doivent être > 0");

        var (items, totalCount) = await _studentService.GetPagedAsync(pageNumber, pageSize, search);


        return Ok(new
        {
            items,
            totalCount,
            pageNumber,
            pageSize,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
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
    [HttpGet("export/pdf")]
    public async Task<IActionResult> ExportPdf([FromQuery] string? search = null)
    {
        // 1️⃣ Récupérer les étudiants filtrés
        var (students, _) = await _studentService.GetPagedAsync(1, int.MaxValue, search);

        // 2️⃣ Générer PDF
        var pdfBytes = _pdfService.GenerateStudentsPdf(students);

        // 3️⃣ Retourner fichier
        return File(pdfBytes, "application/pdf", "Etudiants.pdf");
}

}
