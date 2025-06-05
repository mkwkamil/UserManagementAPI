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
        user.Id = _nextId++;
        Users.Add(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(Users);

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        return user is not null ? Ok(user) : NotFound();
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, User updatedUser)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user is null) return NotFound();
        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        return Ok(user);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user is null) return NotFound();
        Users.Remove(user);
        return NoContent();
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}