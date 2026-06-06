using DeskBooking.Domain.Entities;
using DeskBooking.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeskBooking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhoneBoothsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public PhoneBoothsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PhoneBooth>>> GetAll()
        => Ok(await _unitOfWork.Repository<PhoneBooth>().GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<PhoneBooth>> GetById(int id)
    {
        var item = await _unitOfWork.Repository<PhoneBooth>().GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PhoneBooth item)
    {
        await _unitOfWork.Repository<PhoneBooth>().AddAsync(item);
        await _unitOfWork.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PhoneBooth item)
    {
        if (id != item.Id) return BadRequest();

        _unitOfWork.Repository<PhoneBooth>().Update(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _unitOfWork.Repository<PhoneBooth>().GetByIdAsync(id);
        if (item == null) return NotFound();

        _unitOfWork.Repository<PhoneBooth>().Delete(item);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}
