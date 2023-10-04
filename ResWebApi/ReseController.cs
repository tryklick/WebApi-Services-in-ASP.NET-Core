
using DbContext;
using DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
                return BadRequest(ex.Message);
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

        [HttpGet()]
        [ActionName("Read Sightseeing")]
        [ProducesResponseType(200, Type = typeof(List<Sightseeing>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadSightseeing(string flat = "true")
        {
            try
            {
                bool _flat = bool.Parse(flat);
                var _list = await _service.ReadSightseeing(_flat);

                return Ok(_list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet()]
        [ActionName("ReadItem")]
        [ProducesResponseType(200, Type = typeof(Sightseeing))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemSightseeing(string id, string flat = "false")
        {
            try
            {
                var _id = Guid.Parse(id);
                bool _flat = bool.Parse(flat);
                var mg = await _service.ReadItemSightseeing(_id, _flat);

                if (mg == null)
                {
                    return BadRequest($"Item with id {id} does not exist");
                }

                return Ok(mg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadItemDto Sightseeing ")]
        [ProducesResponseType(200, Type = typeof(SightseeingCUDto))]
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

                var dto = new SightseeingCUDto(sightseeing);
                return Ok(dto);
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


        [HttpPost()]
        [ActionName("CreateItem Comment")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> CreateItemNew([FromBody] CommentCUDto item)
        {
            try
            {
                var comment = await _service.CreateItemNew(item);

                return Ok(comment);
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
    }
}
    




