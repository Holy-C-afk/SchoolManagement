using ManagementSystem.API.Models.Requests;
using ManagementSystem.Application.Teachers;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Application.Teachers.DTOs;
using ManagementSystem.API.Contracts.Teachers;
using ManagementSystem.API.Contracts.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly IPdfService _pdfService;

    public TeachersController(ITeacherService teacherService, IPdfService pdfService)
    {
        _teacherService = teacherService;
        _pdfService = pdfService;
    }

    // POST: api/teachers
    [HttpPost]
    public async Task<IActionResult> Create(CreateTeacherRequest request)
    {
        // On suppose que ta commande prend le nom et l'ID du département
        var id = await _teacherService.CreateAsync(request.FullName, request.DepartmentId);

        return Ok(new { id, message = "Enseignant créé avec succès !" });
    }

    // GET: api/teachers/paged
    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest("pageNumber et pageSize doivent être > 0");

        var (items, totalCount) = await _teacherService.GetPagedAsync(pageNumber, pageSize, search);

        return Ok(new
        {
            items,
            totalCount,
            pageNumber,
            pageSize,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }

    // PUT: api/teachers/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateRequestTeacher request)
    {
        // Logique de mise à jour via le service
        await _teacherService.UpdateAsync(id, request.FullName, request.DepartmentId);
        return NoContent();
    }

    // DELETE: api/teachers/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _teacherService.DeleteAsync(id);
        return NoContent();
    }

    // GET: api/teachers/export/pdf
    [HttpGet("export/pdf")]
    public async Task<IActionResult> ExportPdf([FromQuery] string? search = null)
    {
        // 1️⃣ Récupérer les professeurs filtrés (tout sur une page virtuelle)
        var (teachers, _) = await _teacherService.GetPagedAsync(1, int.MaxValue, search);

        // 2️⃣ Générer le PDF via le service (avec le tableau classique à bordures)
        var pdfBytes = _pdfService.GenerateTeachersPdf(teachers);

        // 3️⃣ Retourner le fichier
        return File(pdfBytes, "application/pdf", $"Liste_Enseignants_{DateTime.Now:yyyyMMdd}.pdf");
    }
}