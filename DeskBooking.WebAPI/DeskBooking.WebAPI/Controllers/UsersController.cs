using DeskBooking.Domain.Entities;
using DeskBooking.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeskBooking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
        => Ok(await _unitOfWork.Repository<User>().GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var item = await _unitOfWork.Repository<User>().GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(User item)
    {
        await _unitOfWork.Repository<User>().AddAsync(item);
        await _unitOfWork.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User item)
    {
        if (id != item.Id) return BadRequest();

        _unitOfWork.Repository<User>().Update(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _unitOfWork.Repository<User>().GetByIdAsync(id);
        if (item == null) return NotFound();

        _unitOfWork.Repository<User>().Delete(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}
