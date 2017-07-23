using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaLibrary.Models
{
    public class AuthorizationDTO
    {
        public string AccessToken { get; set; }
        public bool IsAdmin { get; set; }
    }
}
