using System;
using System.Collections;
using System.Collections.Generic;

namespace DatingApp.API.Models
{
    public class User
    {

        public int Id { get; set; }
        public string username { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        public string gender { get; set; }
        public DateTime dateofbirth { get; set; }
        public string knownas { get; set; }
        public DateTime Created { get; set; }
        public DateTime lastactive { get; set; }
        public string introduction { get; set; }
        public string lookingfor { get; set; }
        public string interest { get; set; }
        public string city { get; set; }
        public string country { get; set; }

        public ICollection<Photo> photos { get; set; }

    }
}
