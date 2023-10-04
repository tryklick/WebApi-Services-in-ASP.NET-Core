using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;

using Configuration;
using Models;
using System.Net;
using System.Text.Json.Serialization;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using Models.DTO;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.Design;
using static Models.DTO.DestinationCUDto;

namespace DbModels
{
    public class Sightseeing //ISeed<SightseeingDbM>
    {
        [Key]       // for EFC Code first
        public Guid SightseeingId { get; set; }
        public bool Seeded { get; set; } = true;


        [Required]
        public  string Description { get; set; }

        [Required]
        public string Sightnames { get; set; }

        public Guid? DestinationId { get; set; }

        public List<Comment> Comments { get; set; }
        public Destination Destinations { get; set; }

        public Sightseeing() { }

        public Sightseeing(SightseeingCUDto _dto)
        {
            SightseeingId = Guid.NewGuid();
            Sightnames = _dto.Sightnames;
            Description = _dto.Description;
            Seeded = false;

        }

        public Sightseeing Seed(csSeedGenerator sgen)
        {
            var sightseeing = new Sightseeing
            {
                SightseeingId = Guid.NewGuid(),
                Description = sgen.SightDescription,
                Sightnames = sgen.SightName,
                Seeded = true

            // sightseeing.Comments = new List<Comment>(); // Skapa en tom lista för Comments
            //sightseeing.Destinations = new Destination(); // Skapa en tom instans för Destinations

        };
            return sightseeing;

        }
    }

        public class Comment
        {

            [Key]       // for EFC Code first
            public Guid CommentId { get; set; }
            public bool Seeded { get; set; } = true;

            [Required]
            public string Text { get; set; }

            public  List<User> Users { get; set; }
            public  List<Sightseeing> Sightseeings { get; set; }

            public Comment() { }

            public Comment(CommentCUDto _dto)
            {
                CommentId = Guid.NewGuid();
                Text = _dto.Text;
                Seeded = false;
            }


        #region randomly seed this instance
        public virtual Comment Seed(csSeedGenerator sgen)
        {
            Seeded = true;
            CommentId = Guid.NewGuid();

            var _text = sgen.Comment;
            Text = _text.Text;

            return this;
        }
        #endregion
    }
        public class Destination
        {

            [Key]       // for EFC Code first
            public Guid DestinationId { get; set; }

            [Required]
            public string StreetAddress { get; set; }
            [Required]
            public int ZipCode { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string Country { get; set; }

            public List<Sightseeing> Sightseeings { get; set; }

            #region randomly seed this instance
            public bool Seeded { get; set; } = false;
            public Destination() { }

            public Destination (DestinationCUDto _dto)
            {
                DestinationId = _dto.DestinationId;
                City = _dto.City;
                Country = _dto.Country;
                StreetAddress = _dto.StreetAddress;
                ZipCode = _dto.ZipCode;
            }


        public virtual Destination Seed(csSeedGenerator sgen)
            {
                Seeded = true;
                DestinationId = Guid.NewGuid();

                Country = sgen.Country;
                StreetAddress = sgen.Country;
                ZipCode = sgen.ZipCode;
                City = sgen.Country;

                return this;
            }

        #endregion
    }
        public class User
        {

            [Key]   
            public  Guid UserId { get; set; }

            [Required]
            public  string UserName { get; set; }
            public string Email { get; set; }
            public virtual string Password { get; set; }

            public virtual List<Comment> Comments { get; set; }

            public User () { }
            public User(UserCUDto _dto)
            {
              UserName = _dto.UserName;
              Email = _dto.Email;
              Password = _dto.Password;
              UserId = _dto.UserId;
            }

        #region randomly seed this instance
        public bool Seeded { get; set; } = false;

            public virtual User Seed(csSeedGenerator sgen)
            {
                Seeded = true;
                UserId = Guid.NewGuid();
                UserName = sgen.FullName;
                Email = sgen.Email();
                return this;
            }
            #endregion
        }
    
}