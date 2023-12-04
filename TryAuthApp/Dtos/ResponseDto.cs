namespace TryAuthApp.Dtos
{
    public class ResponseDto
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<object>? Errors { get; set; }
        public Object? Result { get; set; }
    }
}
