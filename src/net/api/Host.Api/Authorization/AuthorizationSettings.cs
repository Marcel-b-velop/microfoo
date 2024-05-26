namespace com.b_velop.microfe.Authorization
{
    public class AuthorizationSettings
    {
        public string? Authority { get; init; }
        public string? User { get; init; }
        public string? Admin { get; init; }

        public static AuthorizationSettings Read(IConfiguration configuration)
        {
            var authorizationSettings = new AuthorizationSettings();
            configuration.GetSection("Authorization").Bind(authorizationSettings);

            if (string.IsNullOrWhiteSpace(authorizationSettings.Authority))
            {
                throw new InvalidOperationException("Authorization:Authority is not set");
            }

            return authorizationSettings;
        }
    }
}
