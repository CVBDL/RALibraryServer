using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace RaLibrary.Utilities
{
    public class Jwt
    {
        #region Fields

        public static readonly string s_tokenType = "Bearer";
        public static readonly char s_jwtSeparator = '.';

        private string _jwt;
        private JwtPayload _jwtPayload;

        #endregion Fields

        #region Constructors

        public Jwt(string jwt)
        {
            if (!string.IsNullOrWhiteSpace(jwt))
            {
                _jwt = jwt;
            }
        }

        #endregion Constructors

        #region Properties

        public string RawJwt
        {
            get
            {
                return _jwt;
            }
        }

        public JwtPayload Payload
        {
            get
            {
                if (_jwt == null)
                {
                    return null;
                }

                if (_jwtPayload != null)
                {
                    return _jwtPayload;
                }

                try
                {
                    var payloadBase64String = _jwt.Split(s_jwtSeparator)[1];
                    int mod4 = payloadBase64String.Length % 4;
                    if (mod4 > 0)
                    {
                        payloadBase64String += new string('=', 4 - mod4);
                    }
                    var payloadJsonByte = Convert.FromBase64String(payloadBase64String);
                    var payloadJsonString = Encoding.UTF8.GetString(payloadJsonByte);

                    return JsonConvert.DeserializeObject<JwtPayload>(payloadJsonString);
                }
                catch
                {
                    return null;
                }
            }
        }

        #endregion Properties

        /// <summary>
        /// Parse JWT from HTTP request "Authorization" header.
        /// </summary>
        /// <param name="req">HTTP request object.</param>
        /// <returns></returns>
        public static Jwt GetJwtFromRequestHeader(HttpRequestMessage req)
        {
            var authorization = req.Headers.Authorization;

            // It should attach authentication info in HTTP `Authorization` header.
            if (authorization == null)
            {
                return null;
            }

            // It only accepts a bearer type token.
            if (!string.Equals(authorization.Scheme, s_tokenType, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            // It should attach a token value.
            if (authorization.Parameter == null)
            {
                return null;
            }

            return new Jwt(authorization.Parameter);
        }

        /// <summary>
        /// Simply validate a JWT consist of three parts separated by dots.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(_jwt))
            {
                return false;
            }

            try
            {
                string[] parts = _jwt.Split(s_jwtSeparator);
                if (parts.Length != 3)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
