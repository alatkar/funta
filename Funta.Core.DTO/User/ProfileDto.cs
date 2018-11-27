using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.DTO.User
{
    public class ProfileDto
    {
        public Guid UserId { get; set; }
        public string Mobile { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string City { get; set; }
        public DateTime? LastActivity { get; set; }
        public string SerialNumber { get; set; }
    }
}
