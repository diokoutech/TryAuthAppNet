namespace TryAuthApp.Dtos
{
    public class TokenResponse
    {
        public string TokenString { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
