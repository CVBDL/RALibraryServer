namespace RaLibrary.Utilities
{
    public class JwtPayload
    {
        /// <summary>
        /// Token issuer.
        /// </summary>
        public string Iss { get; set; }

        /// <summary>
        /// Issued At.
        /// </summary>
        public long Iat { get; set; }

        /// <summary>
        /// Expiration Time.
        /// </summary>
        public long Exp { get; set; }

        /// <summary>
        /// Audience. The username of domain account.
        /// </summary>
        public string Aud { get; set; }

        /// <summary>
        /// User email address read from Active Directory.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User name read from Active Directory.
        /// </summary>
        public string Name { get; set; }
    }
}
