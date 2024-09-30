namespace AngularApp.Models
{
    public class User
    {
        public string? Id { get; set; } // MongoDB unique identifier
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
