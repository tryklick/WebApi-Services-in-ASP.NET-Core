using System;
using System.Xml.Linq;
using DbContext;
using DbModels;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using static Models.DTO.DestinationCUDto;

namespace Services
{
    public class ReseServices : IReseServices
    {
        public async Task<int> Seed(int NrOfitems)
        {
            var sg = new csSeedGenerator();

            var _sightseeing = new List<Sightseeing>();
            var _user = new List<User>();
            for (int i = 0; i < 100; i++)
            {
                var _u = new User().Seed(sg);
                _user.Add(_u);
            }

            for (int i = 0; i < NrOfitems; i++)
            {
                var sightseeing = new Sightseeing().Seed(sg);
                var destination = new Destination().Seed(sg);
                sightseeing.Destinations = destination;
                
                if (sg.Bool)
                {
                    sightseeing.Comments = new List<Comment>();
                    for (int a = 0; a < sg.Next(0, 20); a++)
                    {
                        var _comment = new Comment().Seed(sg);
                        _comment.Users = new List<User>();

                        for (int u = 0; u < sg.Next(0,5); u++)
                        {
                            var _u = _user[sg.Next(0, _user.Count)];
                            _comment.Users.Add(_u);
                        }

                        sightseeing.Comments.Add(_comment);
                       
                       
                    }

                }
                
                
                _sightseeing.Add(sightseeing);
            }

            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                foreach (var item in _sightseeing)
                {
                    db.Sightseeings.Add(item);
                }

                await db.SaveChangesAsync();

                int cnt = await db.Sightseeings.CountAsync();
                return cnt;
            }
        }

        public async Task<List<Sightseeing>> ReadSightseeingWithNullCommentsAsync(bool seeded)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var query = db.Sightseeings.AsNoTracking().Where(i => i.Comments.Count() == 0 && seeded == true)
                    .Include(i => i.Comments);
                return await query.ToListAsync();
            }
        }


        public async Task<List<Sightseeing>> ReadSightseeing(bool flat)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                
                if (flat)
                {
                    var _list = db.Sightseeings.AsNoTracking().Include(i => i.Destinations).Include(i => i.Comments).ThenInclude(i => i.Users);
                    return await _list
                        .ToListAsync();
                        
                }
                else
                {
                    var _list = await db.Sightseeings
                        .Include(sightseeing => sightseeing.Comments).ToListAsync();
                    return _list;
                }
            }
        }
        public async Task<int> Remove()
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                db.Destinations.RemoveRange(db.Destinations.Where(d => d.Seeded));
                db.Users.RemoveRange(db.Users.Where(u => u.Seeded));
                db.Comments.RemoveRange(db.Comments.Where(c => c.Seeded));
                db.Sightseeings.RemoveRange(db.Sightseeings.Where(i => i.Seeded));
                await db.SaveChangesAsync();

                int _count = await db.Sightseeings.CountAsync();
                return _count;
            }
        }
        public  async Task<int> RemoveUser()
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                db.Users.RemoveRange(db.Users.Where(user => user.Seeded));
                await db.SaveChangesAsync();

                int _count = await db.Users.CountAsync();
                return _count;
            }
        }

        public async Task<List<User>> ReadUser(bool flat)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                if (flat)
                {
                    var _list = await db.Users.ToListAsync();
                    return _list;
                }
                else
                {
                    var _list = await db.Users.Include(user => user.Comments).ToListAsync();
                    return _list;
                }
            }
        }

        public async Task<User> ReadItemUser(Guid id, bool flat)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                if (flat)
                {
                    var user = await db.Users.FirstOrDefaultAsync(u => u.UserId == id);
                    return user;
                }
                else
                {
                    var user = await db.Users.Include(u => u.Comments)
                        .FirstOrDefaultAsync(u => u.UserId == id);
                    return user;
                }
            }
        }


        public async Task<int> RemoveDestination()
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                db.Destinations.RemoveRange(db.Destinations.Where(dest => dest.Seeded));
                await db.SaveChangesAsync();

                int _count = await db.Destinations.CountAsync();
                return _count;
            }
        }

        public async Task<List<Destination>> ReadDestination(bool flat)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                if (flat)
                {
                    var _list = await db.Destinations.ToListAsync();
                    return _list;
                }
                else
                {
                    var _list = await db.Destinations.Include(dest => dest.Sightseeings).ToListAsync();
                    return _list;
                }
            }
        }

        public async Task<Destination> ReadItemDestination(Guid id, bool flat)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                if (flat)
                {
                    var destination = await db.Destinations.FirstOrDefaultAsync(dest => dest.DestinationId == id);
                    return destination;
                }
                else
                {
                    var destination = await db.Destinations.Include(dest => dest.Sightseeings)
                        .FirstOrDefaultAsync(dest => dest.DestinationId == id);
                    return destination;
                }
            }
        }

        public async Task<int> RemoveCommentSeed()
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                db.Comments.RemoveRange(db.Comments.Where(comment => comment.Seeded));
                await db.SaveChangesAsync();

                int _count = await db.Comments.CountAsync();
                return _count;
            }
        }

        public async Task<List<Comment>> ReadComment(bool flat)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                if (flat)
                {
                    var _list = await db.Comments.ToListAsync();
                    return _list;
                }
                else
                {
                    var _list = await db.Comments
                        .Include(comment => comment.Users)
                        .Include(comment => comment.Sightseeings)
                        .ToListAsync();
                    return _list;
                }
            }
        }

        public async Task<Comment> ReadItemComment(Guid id, bool flat)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                if (flat)
                {
                    var comment = await db.Comments
                        .FirstOrDefaultAsync(comment => comment.CommentId == id);

                    return comment;
                }
                else
                {
                    var comment = await db.Comments
                        .Include(comment => comment.Users)
                        .Include(comment => comment.Sightseeings)
                        .FirstOrDefaultAsync(comment => comment.CommentId == id);

                    return comment;
                }
            }
        }

        public async Task<Sightseeing> ReadItemSightseeing(Guid id, bool flat)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                if (flat)
                {
                    var sightseeing = await db.Sightseeings
                        .FirstOrDefaultAsync(sightseeing => sightseeing.SightseeingId == id);

                    return sightseeing;
                }
                else
                {
                    var sightseeing = await db.Sightseeings.Include(sightseeing => sightseeing.Comments)
                        .FirstOrDefaultAsync(sightseeing => sightseeing.SightseeingId == id);

                    return sightseeing;
                }
            }
        }
        public async Task<Destination> UpdateItemDestination(DestinationCUDto _src)
        {
           using (var db = csMainDbContext.DbContext("sysadmin"))
           {
                    var _query1 = db.Destinations.Where(destination => destination.DestinationId == _src.DestinationId);
                    var _item = await _query1.Include(destination => destination.Sightseeings).FirstOrDefaultAsync();

                    _item.StreetAddress = _src.StreetAddress;
                    _item.ZipCode = _src.ZipCode;
                    _item.City = _src.City;
                    _item.Country = _src.Country;

                    await DestinationCUDto_To_DestinationCUDto_NavProp(db, _src, _item);

                    db.Destinations.Update(_item);
                    await db.SaveChangesAsync();

                    return (_item);
           }
        }
        public async Task<User> UpdateItemUser(UserCUDto _src)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var _query1 = db.Users.Where(user => user.UserId == _src.UserId);
                var _item = await _query1.Include(user => user.Comments).FirstOrDefaultAsync();

                _item.UserName = _src.UserName;
                _item.Email = _src.Email;
                _item.Password = _src.Password;

                await UserCUDto_To_UserCUDto_NavProp(db, _src, _item);

                db.Users.Update(_item);
                await db.SaveChangesAsync();

                return (_item);
            }
        }


        public async Task<Comment> UpdateItemComment(CommentCUDto _src)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var _query1 = db.Comments.Where(comment => comment.CommentId == _src.CommentId);
                var _item = await _query1.Include(comment => comment.Users).FirstOrDefaultAsync();

                _item.Text = _src.Text; 

                await CommentCUDto_To_CommentCUDto_NavProp(db, _src, _item);

                db.Comments.Update(_item);
                await db.SaveChangesAsync();

                return (_item);
            }
        }

        public async Task<Sightseeing> UpdateItemSightseeing(SightseeingCUDto _src)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var _query1 = db.Sightseeings.Where(sightseeing => sightseeing.SightseeingId == _src.SightseeingId);
                var _item = await _query1.Include(sightseeing => sightseeing.Comments).FirstOrDefaultAsync();

                _item.Sightnames = _src.Sightnames;
                _item.Description = _src.Description;



                await SightseeingCUDto_To_SightseeingCUDto_NavProp(db, _src, _item);

                db.Sightseeings.Update(_item);
                await db.SaveChangesAsync();

                return (_item);
            }
        }
        public async Task<User> CreateItemUser(UserCUDto _src)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var _item = new User(_src);

                await UserCUDto_To_UserCUDto_NavProp(db, _src, _item);

                db.Users.Add(_item);  //istf update
                await db.SaveChangesAsync();

                return (_item);
            }
        }
        public async Task UserCUDto_To_UserCUDto_NavProp(csMainDbContext db,
               UserCUDto _src, User _dst)
        {
            List<Comment> _comments = new List<Comment>();
            foreach (var id in _src.CommentId)
            {
                var _comment = await db.Comments.FirstOrDefaultAsync(u => u.CommentId == id);

                if (_comment == null)
                    throw new ArgumentException($"Item id {id} not existing");

                _comments.Add(_comment);
            }

            _dst.Comments = _comments;
        }

        public async Task<Destination> CreateItemDestination(DestinationCUDto _src)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var _item = new Destination(_src);

                await DestinationCUDto_To_DestinationCUDto_NavProp(db, _src, _item);

                db.Destinations.Add(_item);  //istf update
                await db.SaveChangesAsync();

                return (_item);
            }
        }
        public async Task DestinationCUDto_To_DestinationCUDto_NavProp(csMainDbContext db,
               DestinationCUDto _src, Destination _dst)
        {
            List<Sightseeing> _sightseeings = new List<Sightseeing>();
            foreach (var id in _src.SightseeingId)
            {
                var _sightseeing = await db.Sightseeings.FirstOrDefaultAsync(u => u.SightseeingId == id);

                if (_sightseeing == null)
                    throw new ArgumentException($"Item id {id} not existing");

                _sightseeings.Add(_sightseeing);
            }

            _dst.Sightseeings = _sightseeings;
        }

        public async Task<Comment> CreateItemComment(CommentCUDto _src)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var _item = new Comment(_src);

                await CommentCUDto_To_CommentCUDto_NavProp(db, _src, _item);

                db.Comments.Add(_item);  //istf update
                await db.SaveChangesAsync();

                return (_item);
            }
        }
            public async Task CommentCUDto_To_CommentCUDto_NavProp(csMainDbContext db,
                   CommentCUDto _src, Comment _dst)
            {
                List<User> _users = new List<User>();
                foreach (var id in _src.UserId )
                {
                    var _user = await db.Users.FirstOrDefaultAsync(u => u.UserId == id);

                    if (_user == null)
                        throw new ArgumentException($"Item id {id} not existing");

                _users.Add(_user);
                }

                _dst.Users = _users;
            }

        public async Task<Sightseeing> CreateItemSightseeing(SightseeingCUDto _src)
        {
            using (var db = csMainDbContext.DbContext("sysadmin"))
            {
                var _item = new Sightseeing(_src);

                await SightseeingCUDto_To_SightseeingCUDto_NavProp(db, _src, _item);

                db.Sightseeings.Add(_item);  //istf update
                await db.SaveChangesAsync();

                return (_item);
            }
        }
        public async Task SightseeingCUDto_To_SightseeingCUDto_NavProp(csMainDbContext db,
              SightseeingCUDto _src, Sightseeing _dst)
        {
            List<Comment> _comments = new List<Comment>();
            foreach (var id in _src.CommentId)
            {
                var _comment = await db.Comments.FirstOrDefaultAsync(a => a.CommentId == id);

                if (_comment == null)
                    throw new ArgumentException($"Item id {id} not existing");

                _comments.Add(_comment);
            }

            _dst.Comments = _comments;
        }
    }


}

