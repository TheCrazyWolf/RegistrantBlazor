using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.ContragentsDto;
using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.API.View.Base;
using RegistrantApplication.Shared.Database.Accounts;
using RegistrantApplication.Shared.Database.Contragents;

namespace RegistrantApplication.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Contragents : BaseApiController
    {
        public Contragents(ILogger<BaseApiController> logger, LiteContext ef, IMapper mapper) : base(logger, ef, mapper)
        {
        }
        
        /// <summary>
        /// Создание нового контрагента
        /// </summary>
        /// <param name="token">Действующий токен</param>
        /// <param name="form">Модель контрагента</param>
        /// <returns>200 - в случае успешного создания</returns>
        [HttpPost("Create")]
        public IActionResult Create([FromHeader] string token, [FromBody] DtoContragent form)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanCreateContragents)
                return StatusCode(403, ConfigMsg.NotAllowed);
            
            if (string.IsNullOrEmpty(form.Title))
                return BadRequest(ConfigMsg.ValidationTextEmpty);

            if (_ef.Contragents.Any(x => x.Title.ToUpper() == form.Title.ToUpper() && x.IsDeleted == false))
                return BadRequest(ConfigMsg.ValidationElementtExist);
            
            form.DateTimeCreated = DateTime.Now;
            var contragent = form.Adapt<Contragent>();
            
            _ef.Add(contragent);
            _ef.SaveChanges();
            
            return Ok(contragent.Adapt<DtoContragent>());
        }

        /// <summary>
        /// Обновление модели
        /// </summary>
        /// <param name="token">Действующий токен</param>
        /// <param name="form">Модель контрагента с сохранением ID</param>
        /// <returns>200 - в случае успешного создания</returns>
        [HttpPut("Update")]
        public IActionResult Update([FromHeader] string token, [FromBody] DtoContragent form)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanEditContragents)
                return StatusCode(403, ConfigMsg.NotAllowed);
            
            var found = _ef.Contragents
                .FirstOrDefault(x=> x.IdContragent == form.IdContragent);

            if (found == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);

            if (string.IsNullOrEmpty(form.Title))
                return BadRequest(ConfigMsg.ValidationTextEmpty);

            found.Adapt(form);
            
            _ef.Update(found);
            _ef.SaveChanges();
            return Ok(found);
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
        public IActionResult Get([FromHeader] string token, string? search, long page, bool showDeleted)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanViewContragents)
                return StatusCode(403, ConfigMsg.NotAllowed);
            
            if (page < 0 )
                return BadRequest(ConfigMsg.PaginationError);

            long totalRecords = _ef.Contragents.Count(x => x.IsDeleted == showDeleted);

            long totalPages = totalRecords / ConfigSrv.RecordsByPage;

            if (page > totalPages)
                return BadRequest(ConfigMsg.PaginationError);
            
            List<Contragent> data;

            if (string.IsNullOrEmpty(search))
            {
               data = _ef.Contragents
                    .OrderBy(x => x.Title)
                    .Where(x => x.IsDeleted == showDeleted)
                    .Skip((int)(page * ConfigSrv.RecordsByPage))
                    .Take((int)ConfigSrv.RecordsByPage)
                    .ToList();
                totalRecords = _ef.Contragents.Where(x => x.IsDeleted == showDeleted).Count();
            }
            else
            {
                data = _ef.Contragents
                    .OrderBy(x => x.Title)
                    .Where(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper().Contains(search.ToUpper()))
                    .Skip((int)(page * ConfigSrv.RecordsByPage))
                    .Take((int)ConfigSrv.RecordsByPage)
                    .ToList();

                totalRecords = _ef.Contragents.Count(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper()
                    .Contains(search.ToUpper()));
                totalPages = totalRecords / ConfigSrv.RecordsByPage;
            }

            IViewAPI view = new DtoDtoViewContragents()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Contragents = data.Adapt<List<DtoContragent>>(),
                MaxRecordsOnPageConst = ConfigSrv.RecordsByPage
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
        public IActionResult Delete([FromHeader] string token, [FromBody] long[] idsContragents) 
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanDeleteContragents)
                return StatusCode(403, ConfigMsg.NotAllowed);
            
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
