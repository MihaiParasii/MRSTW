namespace OtdamDarom.BusinessLogic.Dtos
{
    public class UserAuthResponse
    {
        public bool IsSuccess { get; set; }
        public string StatusMessage { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; } 
        public string UserRole { get; set; }
        public string AuthToken { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}