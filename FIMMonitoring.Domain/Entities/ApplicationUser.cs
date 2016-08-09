using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FIMMonitoring.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CreatedAt = DateTime.Now;
            IsEnabled = true;
        }

        public ApplicationUser(string username)
            : this()
        {
            UserName = username;
        }

        public string EmailAddress { get; set; }

        public string DisplayName { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsEnabled { get; set; }

    }
}
