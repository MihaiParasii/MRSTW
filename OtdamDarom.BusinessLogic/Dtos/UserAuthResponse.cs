namespace OtdamDarom.BusinessLogic.Dtos
{
    public class UserAuthResponse
    {
        public int Id { get; set; }
        public bool IsSuccess { get; set; }
        public string StatusMessage { get; set; }
        public string UserRole { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}