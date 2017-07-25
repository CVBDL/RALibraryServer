﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace RaLibrary.Utils
{
    public class Jwt
    {
        public static readonly string TOKEN_TYPE = "Bearer";
        public static readonly char JWT_SEPARATOR = '.';

        public static Jwt GetJwtFromRequestHeader(HttpRequestMessage req)
        {
            var authorization = req.Headers.Authorization;

            // It should attach authentication info in HTTP `Authorization` header.
            if (authorization == null)
            {
                return null;
            }

            // It only accepts a bearer type token.
            if (!string.Equals(authorization.Scheme, TOKEN_TYPE, StringComparison.OrdinalIgnoreCase))
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

        private string jwt = null;
        private JwtPayload jwtPayload = null;
           
        public Jwt(string jwt)
        {
            if (!string.IsNullOrWhiteSpace(jwt))
            {
                this.jwt = jwt;
            }
        }

        public string RawJwt
        {
            get
            {
                return jwt;
            }
        }

        public JwtPayload Payload
        {
            get
            {
                if (jwt == null)
                {
                    return null;
                }

                if (jwtPayload != null)
                {
                    return jwtPayload;
                }
                
                try
                {
                    var payloadBase64String = jwt.Split(JWT_SEPARATOR)[1];
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

        /// <summary>
        /// Simply validate a JWT consist of three parts separated by dots.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(jwt))
            {
                return false;
            }

            try
            {
                string[] parts = jwt.Split(JWT_SEPARATOR);
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
