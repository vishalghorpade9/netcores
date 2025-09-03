namespace AuthenticationWebApplication.Models
{
    public class LoginResponseDto
    {
        public string JWTToken { get; set; }
        public ShopUser shopUser { get; set; }
    }
}
