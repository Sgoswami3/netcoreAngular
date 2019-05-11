using System;
using System.Collections.Generic;
using DatingApp.API.Models;

namespace DatingApp.API.Dtos
{
    public class userForDetailedDto
    {
        public int Id { get; set; }
        public string username { get; set; }
   
        public string gender { get; set; }
        public int Age { get; set; }
        public string knownas { get; set; }
        public DateTime Created { get; set; }
        public DateTime lastactive { get; set; }
        public string introduction { get; set; }
        public string lookingfor { get; set; }
        public string interest { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string photourl { get; set; }
        public ICollection<photosForDetailsDtos> Photos { get; set; }
    }
}
