namespace Mango.Services.AuthAPI.Data.DTO
{
    public class LoginRequestDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
