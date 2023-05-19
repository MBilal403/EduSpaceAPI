namespace EduSpaceAPI.Helpers
{
    public class GenerateVerificationCode
    {
        public static string VerificationCode()
        {
            // Generate a random 6-digit code
            Random random = new Random();
            int verificationCode = random.Next(100000, 999999);
            return verificationCode.ToString();
        }
    }
}
