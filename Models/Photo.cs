using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string url { get; set; }
        public string Description { get; set; }
        public DateTime dateAdded { get; set; }
        public bool IsMain { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }
    }
}