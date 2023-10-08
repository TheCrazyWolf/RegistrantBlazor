using Microsoft.AspNetCore.Mvc;
using RegistrantApplication.Server.Database;
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
        public IActionResult Get()
        {
            var data = _ef.Contragents.Where(x=> x.IsDeleted == false).ToList();
            return Ok(data);
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
