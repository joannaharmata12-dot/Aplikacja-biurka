using DeskBooking.Domain.Entities;
using DeskBooking.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeskBooking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetAll()
        => Ok(await _unitOfWork.Repository<Room>().GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetById(int id)
    {
        var item = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Room item)
    {
        await _unitOfWork.Repository<Room>().AddAsync(item);
        await _unitOfWork.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Room item)
    {
        if (id != item.Id) return BadRequest();

        _unitOfWork.Repository<Room>().Update(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
        if (item == null) return NotFound();

        _unitOfWork.Repository<Room>().Delete(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}
