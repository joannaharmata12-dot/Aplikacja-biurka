using DeskBooking.Domain.Entities;
using DeskBooking.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeskBooking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FloorsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public FloorsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Floor>>> GetAll()
        => Ok(await _unitOfWork.Repository<Floor>().GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Floor>> GetById(int id)
    {
        var item = await _unitOfWork.Repository<Floor>().GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Floor item)
    {
        await _unitOfWork.Repository<Floor>().AddAsync(item);
        await _unitOfWork.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Floor item)
    {
        if (id != item.Id) return BadRequest();

        _unitOfWork.Repository<Floor>().Update(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _unitOfWork.Repository<Floor>().GetByIdAsync(id);
        if (item == null) return NotFound();

        _unitOfWork.Repository<Floor>().Delete(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}
