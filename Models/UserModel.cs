namespace EduSpaceAPI.Models
{
    public class UserModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserRole { get; set; }
        public string? FullName { get; set; }
        public string? VerificationCode { get; set; }
        public byte[]? UserImage { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsVerified { get; set; }
        public string? Address { get; set; }
        public byte[]? Resume { get; set; }
        public string? ImageName { get; set; }
        public string? ResumeName { get; set; }
    }
}
