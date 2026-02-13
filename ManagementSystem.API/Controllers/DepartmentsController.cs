using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Application.Departments.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
    {
        var departments = await _departmentService.GetAllAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> GetById(Guid id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department == null) return NotFound();
        
        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateDepartmentRequest request)
    {
        var id = await _departmentService.CreateAsync(request.Name);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentRequest request)
    {
        await _departmentService.UpdateAsync(id, request.Name);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _departmentService.DeleteAsync(id);
        return NoContent();
    }
}

// Petits records pour valider les entrées API
public record CreateDepartmentRequest(string Name);
public record UpdateDepartmentRequest(string Name);