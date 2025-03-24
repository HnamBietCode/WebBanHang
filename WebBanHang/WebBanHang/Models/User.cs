namespace WebBanHang.Models
{
    // Models/User.cs
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
