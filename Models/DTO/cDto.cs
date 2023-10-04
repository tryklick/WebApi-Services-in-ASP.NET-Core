using System;
using System;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection.Emit;
using System.Xml.Linq;
using DbModels;

namespace Models.DTO
{

    public  class SightseeingCUDto
    {
        public virtual Guid SightseeingId { get; set; }


        public virtual string Name { get; set; }
        public string Description { get; set; }
        public virtual string Sightnames { get; set; }

        public virtual Guid? DestinationId { get; set; } = null;

        public virtual List<Guid> CommentId { get; set; } = new List<Guid>();

        public SightseeingCUDto() { }

        public SightseeingCUDto(Sightseeing org)
        {
            SightseeingId = org.SightseeingId;
            this.Description = org.Description;
            this.Sightnames = org.Sightnames;

            DestinationId = org?.Destinations.DestinationId;
            CommentId = org.Comments?.Select(i => i.CommentId).ToList();

        }


    }
    public class CommentCUDto
    {
        public virtual Guid CommentId { get; set; }

        public string Text { get; set; }
        public virtual List<Guid> UserId { get; set; } = new List<Guid>();
        public virtual List<Guid> SightseeingId { get; set; } = new List<Guid>();

        public CommentCUDto(Comment org)
        {
            CommentId = org.CommentId;
            this.Text = org.Text;

            UserId = org.Users?.Select(i => i.UserId).ToList();
            SightseeingId = org.Sightseeings?.Select(i => i.SightseeingId).ToList();
        }
    }

    public class DestinationCUDto
    {
        public Guid DestinationId { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public virtual string StreetAddress { get; set; }
        public virtual int ZipCode { get; set; }
        public virtual List<Guid> SightseeingId { get; set; } = new List<Guid>();



        public DestinationCUDto(Destination org)
        {
            this.DestinationId = org.DestinationId;
            this.City = org.City;
            this.Country = org.Country;
            this.StreetAddress = org.StreetAddress;
            this.ZipCode = org.ZipCode;

            SightseeingId = org.Sightseeings?.Select(i => i.SightseeingId).ToList();

        }

        public class UserCUDto
        {
            public Guid UserId { get; set; }

            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public virtual List<Guid> CommentId { get; set; } = new List<Guid>();


            public UserCUDto(User org)
            {
                this.UserName = org.UserName;
                this.Email = org.Email;
                this.Password = org.Password;
                this.UserId = org.UserId;

                CommentId = org.Comments?.Select(i => i.CommentId).ToList();
            }


        }


    }

}   