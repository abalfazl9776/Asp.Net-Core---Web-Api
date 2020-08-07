namespace Common
{
    public class SiteSettings
    {
        public JwtSettings JwtSettings { get; set; }
        public IdentitySettings IdentitySettings { get; set; }
        public InitialUserSettings InitialUserSettings { get; set; }
        public MashhadHostSmsSettings MashhadHostSmsSettings { get; set; }
        public KavenegarSmsSettings KavenegarSmsSettings { get; set; }
    }

    public class IdentitySettings
    {
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonAlphanumeric { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool RequireUniqueEmail { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
        public int DefaultLockoutTimeSpanMinutes { get; set; }
        public bool AllowedForNewUsers { get; set; }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string EncryptKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int NotBeforeMinutes { get; set; }
        public int ExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }

    public class InitialUserSettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    
    public class MashhadHostSmsSettings
    {
        public string Sender { get; set; }
    }

    public class KavenegarSmsSettings
    {
        public string Sender { get; set; }
    }
}
