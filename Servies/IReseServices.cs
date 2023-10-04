using System.Net;
using DbModels;
using Microsoft.VisualBasic;
using Models.DTO;
using static Models.DTO.DestinationCUDto;

namespace Services
{
	public interface IReseServices

	{
        public Task<int> Seed(int NrOfitems);
        public Task<int> Remove();

        public Task<List<Sightseeing>> ReadSightseeing(bool flat);
        public Task<Sightseeing> ReadItemSightseeing(Guid id, bool flat);

        public Task<Sightseeing> UpdateItemSightseeing(SightseeingCUDto _src);
        public Task<Sightseeing> CreateItemSightseeing(SightseeingCUDto _src);


        public Task<Comment> CreateItemComment(CommentCUDto _src);
        public Task<Comment> UpdateItemComment(CommentCUDto _src);
        public Task<Comment> ReadItemComment(Guid id, bool flat);
        public Task<List<Comment>> ReadComment(bool flat);
        public Task<int> RemoveCommentSeed();

        public Task<Destination> CreateItemDestination(DestinationCUDto _src);
        public Task<Destination> UpdateItemDestination(DestinationCUDto _src);
        public Task<Destination> ReadItemDestination(Guid id, bool flat);
        public Task<List<Destination>> ReadDestination(bool flat);
        public Task<int> RemoveDestination();


        public Task<User> CreateItemUser(UserCUDto _src);
        public Task<User> UpdateItemUser(UserCUDto _src);
        public Task<User> ReadItemUser(Guid id, bool flat);
        public Task<List<User>> ReadUser(bool flat);
        public Task<int> RemoveUser();
        public Task<List<Sightseeing>> ReadSightseeingWithNullCommentsAsync(bool seeded);


    }
}

