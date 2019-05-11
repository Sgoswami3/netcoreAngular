using System;
namespace DatingApp.API.Dtos
{
    public class photosForDetailsDtos
    {
        public int Id { get; set; }
        public string url { get; set; }
        public string Description { get; set; }
        public DateTime dateAdded { get; set; }
        public bool IsMain { get; set; }
    }
}
