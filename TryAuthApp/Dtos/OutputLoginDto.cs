namespace TryAuthApp.Dtos
{
    public class OutputLoginDto
    {
        public string IdUser { get; set; }
        public string Name { get; set; }
        public string Profil { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }

    }
}
