using Microsoft.AspNetCore.Mvc;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.View;
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
        public IActionResult Get(string? search, long page, bool showDeleted)
        {
            if (!IsValidateToken().Result)
                return Unauthorized("Требуется авторизация");
            
            if (page < 0 )
                return BadRequest("Страница отрицательная");

            const long recordsByPage = 10;

            long totalRecords = _ef.Contragents.Where(x => x.IsDeleted == showDeleted).Count();

            long totalPages = totalRecords / recordsByPage;

            if (page > totalPages)
                return BadRequest("Страница за пределами количество страниц)");
            
            List<Contragent> data;

            if (string.IsNullOrEmpty(search))
            {
               data = _ef.Contragents
                    .OrderBy(x => x.Title)
                    .Where(x => x.IsDeleted == showDeleted)
                    .Skip((int)(page * recordsByPage))
                    .Take((int)recordsByPage)
                    .ToList();
                totalRecords = _ef.Contragents.Where(x => x.IsDeleted == showDeleted).Count();
            }
            else
            {
                data = _ef.Contragents
                    .OrderBy(x => x.Title)
                    .Where(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper().Contains(search.ToUpper()))
                    .Skip((int)(page * recordsByPage))
                    .Take((int)recordsByPage)
                    .ToList();

                totalRecords = _ef.Contragents.Where(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper().Contains(search.ToUpper())).Count();
                totalPages = totalRecords / recordsByPage;
            }

            IViewAPI view = new ViewContragents()
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
            if (!IsValidateToken().Result)
                return Unauthorized("Требуется авторизация");
            
            if (string.IsNullOrEmpty(contragent.Title))
                return BadRequest("Название контрагента не может быть пустым");

            if (_ef.Contragents.Any(x => x.Title.ToUpper() == contragent.Title.ToUpper() && x.IsDeleted == false))
                return BadRequest("Такой контрагент уже существует");
            
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
                return Unauthorized("Требуется авторизация");
            
            var found = _ef.Contragents
                .FirstOrDefault(x=> x.IdContragent == contragent.IdContragent);

            if (found == null)
                return NotFound("Элемент не найден");

            if (string.IsNullOrEmpty(contragent.Title))
                return BadRequest("Новое имя агента не должно быть пустым");

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
                return Unauthorized("Требуется авторизация");
            
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
