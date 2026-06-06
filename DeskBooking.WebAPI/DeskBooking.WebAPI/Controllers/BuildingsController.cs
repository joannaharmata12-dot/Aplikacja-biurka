using DeskBooking.Domain.Entities;
using DeskBooking.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeskBooking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuildingsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public BuildingsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Building>>> GetAll()
        => Ok(await _unitOfWork.Repository<Building>().GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Building>> GetById(int id)
    {
        var item = await _unitOfWork.Repository<Building>().GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Building item)
    {
        await _unitOfWork.Repository<Building>().AddAsync(item);
        await _unitOfWork.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Building item)
    {
        if (id != item.Id) return BadRequest();

        _unitOfWork.Repository<Building>().Update(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _unitOfWork.Repository<Building>().GetByIdAsync(id);
        if (item == null) return NotFound();

        _unitOfWork.Repository<Building>().Delete(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}
