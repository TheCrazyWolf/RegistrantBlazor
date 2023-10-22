using Microsoft.AspNetCore.Mvc;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.Base;
using RegistrantApplication.Server.Controllers.BaseAPI;
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
        
        /// <summary>
        /// Создание нового контрагента
        /// </summary>
        /// <param name="token">Действующий токен</param>
        /// <param name="contragent">Модель контрагента</param>
        /// <returns>200 - в случае успешного создания</returns>
        [HttpPost("Create")]
        public IActionResult Create([FromHeader] string? token, [FromBody] Contragent contragent)
        {
            if (!IsValidateToken(token).Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
            
            if (string.IsNullOrEmpty(contragent.Title))
                return BadRequest(ConfigMsg.ValidationTextEmpty);

            if (_ef.Contragents.Any(x => x.Title.ToUpper() == contragent.Title.ToUpper() && x.IsDeleted == false))
                return BadRequest(ConfigMsg.ValidationElementtExist);
            
            contragent.DateTimeCreated = DateTime.Now;
            _ef.Add(MyValidator.GetModel(contragent));
            _ef.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Обновление модели
        /// </summary>
        /// <param name="token">Действующий токен</param>
        /// <param name="contragent">Модель контрагента с сохранением ID</param>
        /// <returns>200 - в случае успешного создания</returns>
        [HttpPut("Update")]
        public IActionResult Update([FromHeader] string? token, [FromBody] Contragent contragent)
        {
            if (!IsValidateToken(token).Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
            
            var found = _ef.Contragents
                .FirstOrDefault(x=> x.IdContragent == contragent.IdContragent);

            if (found == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);

            if (string.IsNullOrEmpty(contragent.Title))
                return BadRequest(ConfigMsg.ValidationTextEmpty);
            
            found.IsDeleted = contragent.IsDeleted;
            _ef.Update(MyValidator.GetModel(found));
            _ef.SaveChanges();
            return Ok();
        }
        
        /// <summary>
        /// Получение списка контрагентов
        /// </summary>
        /// <param name="token">Действующий токен</param>
        /// <param name="search">Поисковой параметры</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="showDeleted">Показывать удаленных?</param>
        /// <returns>200 - в случае успешного создания</returns>
        [HttpGet("Get")]
        public IActionResult Get([FromHeader] string? token, string? search, long page, bool showDeleted)
        {
            if (!IsValidateToken(token).Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
            
            if (page < 0 )
                return BadRequest(ConfigMsg.PaginationError);

            long totalRecords = _ef.Contragents.Count(x => x.IsDeleted == showDeleted);

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

                totalRecords = _ef.Contragents.Count(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper()
                    .Contains(search.ToUpper()));
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

        /// <summary>
        /// Удаление/Скрытие контрагентов
        /// </summary>
        /// <param name="token">Действующий токен</param>
        /// <param name="idsContragents">ID контрагентов представленны в виде массива</param>
        /// <returns>200 - в случае успешного создания</returns>
        [HttpDelete("Delete")]
        public IActionResult Delete([FromHeader] string? token, [FromBody] long[] idsContragents) 
        {
            if (!IsValidateToken(token).Result)
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
