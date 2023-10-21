using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Drivers;

namespace RegistrantApplication.Server.Controllers
{
    public class Drivers : BaseController
    {
        public Drivers(ILogger<BaseController> logger, LiteContext ef) : base(logger, ef)
        {
        }


        [HttpGet("GetById")]
        public IActionResult GetById(long idDriver)
        {

            var currentDriver = _ef.Accounts
                .Include(x => x.Documents)
                .Include(x => x.Autos)
                .FirstOrDefault(x => x.IdAccount == idDriver);

            if (currentDriver == null)
                return NotFound("Водитель не найден");

            return Ok(currentDriver);
        }
        
        [HttpGet("Get")]
        public IActionResult Get(string? search, long page, bool showDeleted)
        {
            if (page < 0)
                return BadRequest("Страница отрицательная");

            const long recordsByPage = 10;

            long totalRecords = _ef.Contragents
                .Where(x => x.IsDeleted == showDeleted).Count();

            long totalPages = totalRecords / recordsByPage;

            if (page > totalPages)
                return BadRequest("Страница за пределами количество страниц)");

            List<Account> data;

            if (string.IsNullOrEmpty(search))
            {
                data = _ef.Accounts
                     .OrderBy(x => x.IdAccount)
                     .Where(x => x.IsDeleted == showDeleted)
                     .Skip((int)(page * recordsByPage))
                     .Take((int)recordsByPage)
                     .ToList();
                totalRecords = _ef.Contragents.Where(x => x.IsDeleted == showDeleted).Count();
            }
            else
            {
                data = _ef.Accounts
                    .OrderBy(x => x.IdAccount)
                    .Where(x => (x.IsDeleted == showDeleted) && x.Family.ToUpper().Contains(search.ToUpper()))
                    .Skip((int)(page * recordsByPage))
                    .Take((int)recordsByPage)
                    .ToList();

                totalRecords = 
                    _ef.Contragents.Count(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper()
                        .Contains(search.ToUpper()));
                totalPages = totalRecords / recordsByPage;
            }

            IViewAPI view = new ViewDrivers()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Accounts = data,
                MaxRecordsOnPageConst = recordsByPage
            };

            return Ok(view);
        }

    }
}
