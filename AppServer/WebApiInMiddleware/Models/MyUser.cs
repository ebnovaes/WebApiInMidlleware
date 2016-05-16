﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApiInMiddleware.Models
{
    public class MyUser
    {
        public MyUser()
        {
            Id = Guid.NewGuid().ToString();
            Claims = new List<MyUserClaim>();
        }

        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<MyUserClaim> Claims { get; set; }
    }

    public class MyUserClaim
    {
        public MyUserClaim()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class MyPasswordHasher
    {
        public string CreateHash(string password)
        {
            char[] chars = password.ToArray();
            char[] hash = chars.Reverse().ToArray();
            return new string(hash);
        }
    }

}
