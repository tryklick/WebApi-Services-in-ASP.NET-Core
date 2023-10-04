using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using DbContext;
using DbModels;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Services;
using static Models.DTO.DestinationCUDto;
using Microsoft.VisualBasic;

namespace ResWebApi
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReseController : Controller
    {
        IReseServices _service;
        ILogger<ReseController> _logger;

        [HttpGet()]
        [ActionName("Seed")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Seed(string count)
        {
            try
            {
                int _count = int.Parse(count);

                int cnt = await _service.Seed(_count);
                return Ok(cnt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }

        }

        [HttpGet()]
        [ActionName("RemoveSeed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Remove()
        {
            try
            {
                int _count = await _service.Remove();
                return Ok(_count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }

        }

        /************************************************************************************************************************/
        /************************************************************************************************************************/

        [HttpGet()]
        [ActionName("Read ")]
        [ProducesResponseType(200, Type = typeof(List<Sightseeing>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadSightseeing(string flat = "true")
        {
           
                bool _flat = bool.Parse(flat);
                var _list = await _service.ReadSightseeing(_flat);

                return Ok(_list);
        

        }

        [HttpGet()]
        [ActionName("Read Sightseeing With Null Comments")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Sightseeing>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadSightseeingWithNullCommentsAsync(string seeded = "true")
        {
            bool _seeded = true;
            if (!bool.TryParse(seeded, out _seeded))
            {
                return BadRequest("seeded format error");
            }


            var items = await _service.ReadSightseeingWithNullCommentsAsync( _seeded);
            return Ok(items);
             }  


        [HttpGet()]
        [ActionName("ReadItemDto Sightseeing ")]
        [ProducesResponseType(200, Type = typeof(Sightseeing))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemSightseeing(string id)
        {
            try
            {
                var _id = Guid.Parse(id);
                var sightseeing = await _service.ReadItemSightseeing(_id, false);

                if (sightseeing == null)
                {
                    return BadRequest($"Item with id {id} does not exist");
                }

                
                return Ok(sightseeing);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet()]
        [ActionName("DeleteItem")]
        [ProducesResponseType(200, Type = typeof(Sightseeing))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteItem(string id)
        {
            try
            {
                var _id = Guid.Parse(id);
                using (var db = csMainDbContext.DbContext("sysadmin"))
                {
                    var sightseeing = await db.Sightseeings
                                      .FirstOrDefaultAsync(sightseeing => sightseeing.SightseeingId == _id);
                    if (sightseeing == null)
                    {
                        return BadRequest($"Item with id {id} does not exist");
                    }

                    db.Sightseeings.Remove(sightseeing);

                    await db.SaveChangesAsync();
                    return Ok(sightseeing);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [ActionName("UpdateItem Sightseeing")]
        [ProducesResponseType(200, Type = typeof(Sightseeing))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItemSightseeing(string id, [FromBody] SightseeingCUDto item)
        {
            try
            {
                var _id = Guid.Parse(id);

                if (item.SightseeingId != _id)
                    throw new Exception("Id mismatch");

                var sightseeing = await _service.UpdateItemSightseeing(item);

                return Ok(sightseeing);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        [ActionName("CreateItem Sightseeing")]
        [ProducesResponseType(200, Type = typeof(Sightseeing))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateItemSightseeing([FromBody] SightseeingCUDto item)
        {
            try
            {
                var sightseeing = await _service.CreateItemSightseeing(item);

                return Ok(sightseeing);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /************************************************************************************************************************/
        /************************************************************************************************************************/

        //GET: api/user/read
        [HttpGet()]
        [ActionName("ReadUser")]
        [ProducesResponseType(200, Type = typeof(List<User>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadUser(string flat = "true")
        {
            try
            {
                bool _flat = bool.Parse(flat);
                var _list = await _service.ReadUser(_flat);

                return Ok(_list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/user/readitem
        [HttpGet()]
        [ActionName("ReadItemUser")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemUser(string id, string flat = "false")
        {
            try
            {
                var _id = Guid.Parse(id);
                bool _flat = bool.Parse(flat);
                var user = await _service.ReadItemUser(_id, _flat);

                if (user == null)
                {
                    return BadRequest($"User with id {id} does not exist");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/user/readitemdto
        [HttpGet()]
        [ActionName("ReadItemUserDto")]
        [ProducesResponseType(200, Type = typeof(UserCUDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemUserDto(string id)
        {
            try
            {
                var _id = Guid.Parse(id);
                var user = await _service.ReadItemUser(_id, false);

                if (user == null)
                {
                    return BadRequest($"User with id {id} does not exist");
                }

                var dto = new UserCUDto(user);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        [ActionName("CreateItem User")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> CreateItemUser([FromBody] UserCUDto item)
        {
            try
            {
                var user = await _service.CreateItemUser(item);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public ReseController(IReseServices service, ILogger<ReseController> logger)
        {
            _service = service;
            _logger = logger;
        }


        [HttpPut("{id}")]
        [ActionName("UpdateItemUser")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItemUser(string id, [FromBody] UserCUDto item)
        {
            try
            {
                var _id = Guid.Parse(id);

                if (item.UserId != _id)
                    throw new Exception("Id mismatch");

                var user = await _service.UpdateItemUser(item);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /************************************************************************************************************************/
        /************************************************************************************************************************/



        [HttpGet()]
        [ActionName("ReadDestination")]
        [ProducesResponseType(200, Type = typeof(List<Destination>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadDestination(string flat = "true")
        {
            try
            {
                bool _flat = bool.Parse(flat);
                var _list = await _service.ReadDestination(_flat);

                return Ok(_list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadItemDestination")]
        [ProducesResponseType(200, Type = typeof(Destination))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDestination(string id, string flat = "false")
        {
            try
            {
                var _id = Guid.Parse(id);
                bool _flat = bool.Parse(flat);
                var destination = await _service.ReadItemDestination(_id, _flat);

                if (destination == null)
                {
                    return BadRequest($"Item with id {id} does not exist");
                }

                return Ok(destination);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Controlleråtgärder för Destination
        [HttpGet()]
        [ActionName("RemoveDestination")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> RemoveDestination()
        {
            try
            {
                int _count = await _service.RemoveDestination();
                return Ok(_count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }




        [HttpPut("{id}")]
        [ActionName("UpdateItem Destination")]
        [ProducesResponseType(200, Type = typeof(Destination))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItemDestination(string id, [FromBody] DestinationCUDto item)
        {
            try
            {
                var _id = Guid.Parse(id);

                if (item.DestinationId != _id)
                    throw new Exception("Id mismatch");

                var destination = await _service.UpdateItemDestination(item);

                return Ok(destination);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost()]
        [ActionName("CreateItem Destination")]
        [ProducesResponseType(200, Type = typeof(Destination))]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> CreateItemDestination([FromBody] DestinationCUDto item)
        {
            try
            {
                var destination = await _service.CreateItemDestination(item);

                return Ok(destination);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /************************************************************************************************************************/
        /************************************************************************************************************************/




        //GET: api/comment/read
        [HttpGet()]
        [ActionName("Read Comment")]
        [ProducesResponseType(200, Type = typeof(List<Comment>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadComment(string flat = "true")
        {
            try
            {
                bool _flat = bool.Parse(flat);
                var _list = await _service.ReadComment(_flat);

                return Ok(_list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/comment/readitem
        [HttpGet()]
        [ActionName("ReadItem Comment")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemComment(string id, string flat = "false")
        {
            try
            {
                var _id = Guid.Parse(id);
                bool _flat = bool.Parse(flat);
                var comment = await _service.ReadItemComment(_id, _flat);

                if (comment == null)
                {
                    return BadRequest($"Comment with id {id} does not exist");
                }

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //GET: api/comment/removeseed
        [HttpGet()]
        [ActionName("Remove Comment Seed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> RemoveCommentSeed()
        {
            try
            {
                int _count = await _service.RemoveCommentSeed();
                return Ok(_count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }


        [HttpPut("{id}")]
        [ActionName("UpdateItem Comment")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItemComment(string id, [FromBody] CommentCUDto item)
        {
            try
            {
                var _id = Guid.Parse(id);

                if (item.CommentId != _id)
                    throw new Exception("Id mismatch");

                var comment = await _service.UpdateItemComment(item);

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       


        [HttpPost()]
        [ActionName("CreateItem Comment")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> CreateItemComment([FromBody] CommentCUDto item)
        {
            try
            {
                var comment = await _service.CreateItemComment(item);

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /************************************************************************************************************************/
        /************************************************************************************************************************/
    }
}


