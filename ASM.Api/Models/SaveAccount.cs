﻿namespace ASM.Api.Models
{
    public class SaveAccount
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public bool? AcceptedTerms { get; set; }
    }
}
