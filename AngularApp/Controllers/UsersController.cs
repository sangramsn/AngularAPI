using AngularApp.Data;
using AngularApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Driver;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public UsersController(MongoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            user.Id = Guid.NewGuid().ToString(); 
            user.Role = "user"; 
            await _context.Users.InsertOneAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }


        [HttpGet()]
        public async Task<ActionResult<User>> Get()
        {
            var users = await _context.Users.Find(_ => true).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            var user = await _context.Users.Find(Builders<User>.Filter.Eq(u => u.Id, id)).FirstOrDefaultAsync();
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var result = await _context.Users.ReplaceOneAsync(Builders<User>.Filter.Eq(u => u.Id, id), user);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return NoContent(); 
            }

            return NotFound(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _context.Users.DeleteOneAsync(Builders<User>.Filter.Eq(u => u.Id, id));

            if (result.DeletedCount > 0)
            {
                return NoContent(); 
            }

            return NotFound(); 
        }




        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and password are required.");
            }

            
            var user = await _context.Users.Find(u => u.Email == loginRequest.Email).FirstOrDefaultAsync();

            
            if (user == null || user.Password != loginRequest.Password) 
            {
                return Unauthorized("Invalid username or password.");
            }
                Random random = new Random();
                int length = 10; 
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                string randomString = new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());


                
                return Ok(new { role = user.Role, token = randomString });
        }

    }


}
