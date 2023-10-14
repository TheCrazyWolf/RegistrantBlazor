using Microsoft.AspNetCore.Mvc;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.Contragents;

namespace RegistrantApplication.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Contragents : BaseController
    {
        public Contragents(ILogger<BaseController> logger, LiteContext ef) : base(logger, ef)
        {
        }

        [HttpGet("Get")]
        public IActionResult Get(string? search, int page, bool showDeleted)
        {
            if (page < 0 )
                return BadRequest("Страница отрицательная");

            const int recordsByPage = 10;

            long totalRecords = _ef.Contragents.Where(x => x.IsDeleted == showDeleted).Count();

            long totalPages = totalRecords / recordsByPage;

            if (page > totalPages)
                return BadRequest("Страница за пределами количество страниц)");

            var data = _ef.Contragents
                .OrderBy(x => x.IdContragent)
                .Where(x=> x.IsDeleted == showDeleted)
                .Skip(page * recordsByPage)
                .Take(recordsByPage)
                .ToList();

            ViewContragents view = new ViewContragents()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Contragents = data,
                MaxRecordsOnPageConst = recordsByPage
            };

            return Ok(view);
        }
        
        [HttpPost("Create")]
        public IActionResult Create([FromBody] Contragent contragent)
        {
            contragent.DateTimeCreated = DateTime.Now;
            _ef.Add(contragent);
            _ef.SaveChanges();
            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] Contragent contragent)
        {
            var found = _ef.Contragents
                .FirstOrDefault(x=> x.IdContragent == contragent.IdContragent);

            if (found == null)
                return NotFound("Db not found");

            found.Title = contragent.Title;
            _ef.Update(found);
            _ef.SaveChanges();
            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromBody] long[] ids) 
        {
            foreach (var item in ids)
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
