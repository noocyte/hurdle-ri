using System;

namespace WebAPI.Backend.Models
{
    public class Incident
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Deadline { get; set; }
    }
}