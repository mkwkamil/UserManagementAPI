using Microsoft.AspNetCore.Mvc;

namespace UserManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private static readonly List<User> Users = new();
    private static int _nextId = 1;

    [HttpPost]
    public IActionResult Create(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
            return BadRequest("Name and email are required.");

        try
        {
            user.Id = _nextId++;
            Users.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            if (Users == null || !Users.Any())
                return NotFound("No users found.");

            return Ok(Users);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, User updatedUser)
    {
        if (string.IsNullOrWhiteSpace(updatedUser.Name) || string.IsNullOrWhiteSpace(updatedUser.Email))
            return BadRequest("Name and email are required.");

        try
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            return Ok(user);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            Users.Remove(user);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}