using DeskBooking.Domain.Entities;
using DeskBooking.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeskBooking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DesksController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DesksController> _logger;

    public DesksController(IUnitOfWork unitOfWork, ILogger<DesksController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Desk>>> GetAll()
    {
        _logger.LogInformation("Pobrano listę biurek.");
        var items = await _unitOfWork.Repository<Desk>().GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Desk>> GetById(int id)
    {
        var item = await _unitOfWork.Repository<Desk>().GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Desk item)
    {
        await _unitOfWork.Repository<Desk>().AddAsync(item);
        await _unitOfWork.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Desk item)
    {
        if (id != item.Id) return BadRequest();

        _unitOfWork.Repository<Desk>().Update(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _unitOfWork.Repository<Desk>().GetByIdAsync(id);
        if (item == null) return NotFound();

        _unitOfWork.Repository<Desk>().Delete(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}