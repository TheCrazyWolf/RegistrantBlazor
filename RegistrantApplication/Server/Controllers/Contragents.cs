using Microsoft.AspNetCore.Mvc;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.Base;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Contragents;

namespace RegistrantApplication.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Contragents : BaseApiController
    {
        public Contragents(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
        {
        }

        [HttpGet("Get")]
        public IActionResult Get(string? search, long page, bool showDeleted)
        {
            if (!IsValidateToken().Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
            
            if (page < 0 )
                return BadRequest(ConfigMsg.PaginationError);

            long totalRecords = _ef.Contragents.Where(x => x.IsDeleted == showDeleted).Count();

            long totalPages = totalRecords / ConfigServer.RecordsByPage;

            if (page > totalPages)
                return BadRequest(ConfigMsg.PaginationError);
            
            List<Contragent> data;

            if (string.IsNullOrEmpty(search))
            {
               data = _ef.Contragents
                    .OrderBy(x => x.Title)
                    .Where(x => x.IsDeleted == showDeleted)
                    .Skip((int)(page * ConfigServer.RecordsByPage))
                    .Take((int)ConfigServer.RecordsByPage)
                    .ToList();
                totalRecords = _ef.Contragents.Where(x => x.IsDeleted == showDeleted).Count();
            }
            else
            {
                data = _ef.Contragents
                    .OrderBy(x => x.Title)
                    .Where(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper().Contains(search.ToUpper()))
                    .Skip((int)(page * ConfigServer.RecordsByPage))
                    .Take((int)ConfigServer.RecordsByPage)
                    .ToList();

                totalRecords = _ef.Contragents.Where(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper().Contains(search.ToUpper())).Count();
                totalPages = totalRecords / ConfigServer.RecordsByPage;
            }

            IViewAPI view = new ViewContragents()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Contragents = data,
                MaxRecordsOnPageConst = ConfigServer.RecordsByPage
            };

            return Ok(view);
        }
        
        [HttpPost("Create")]
        public IActionResult Create([FromBody] Contragent contragent)
        {
            if (!IsValidateToken().Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
            
            if (string.IsNullOrEmpty(contragent.Title))
                return BadRequest(ConfigMsg.ValidationTextEmpty);

            if (_ef.Contragents.Any(x => x.Title.ToUpper() == contragent.Title.ToUpper() && x.IsDeleted == false))
                return BadRequest(ConfigMsg.ValidationElementtExist);
            
            contragent.DateTimeCreated = DateTime.Now;
            contragent.Title = contragent.Title.ToUpper();
            _ef.Add(contragent);
            _ef.SaveChanges();
            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] Contragent contragent)
        {
            if (!IsValidateToken().Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
            
            var found = _ef.Contragents
                .FirstOrDefault(x=> x.IdContragent == contragent.IdContragent);

            if (found == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);

            if (string.IsNullOrEmpty(contragent.Title))
                return BadRequest(ConfigMsg.ValidationTextEmpty);

            found.Title = contragent.Title.ToUpper();
            found.IsDeleted = contragent.IsDeleted;
            _ef.Update(found);
            _ef.SaveChanges();
            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromBody] long[] idsContragents) 
        {
            if (!IsValidateToken().Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
            
            foreach (var item in idsContragents)
            {
                var found = _ef.Contragents.FirstOrDefault(x => x.IdContragent == item);
                if (found == null)
                    continue;
                found.IsDeleted = true;
                _ef.Update(found);
                _ef.SaveChanges();
            }

            return Ok();
        }

    }
}
