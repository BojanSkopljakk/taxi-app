﻿namespace AuthenticationService.Models
{
    public class UpdateProfileDto
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
