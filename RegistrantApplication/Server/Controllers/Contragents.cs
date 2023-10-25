using Microsoft.AspNetCore.Mvc;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Database.Contragents;
using ModelTransfer = RegistrantApplication.Server.Controllers.BaseAPI.ModelTransfer;

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
        public IActionResult Create([FromHeader] string token, [FromBody] Contragent contragent)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanCreateContragents)
                return StatusCode(403, ConfigMsg.NotAllowed);
            
            if (string.IsNullOrEmpty(contragent.Title))
                return BadRequest(ConfigMsg.ValidationTextEmpty);

            if (Ef.Contragents.Any(x => x.Title.ToUpper() == contragent.Title.ToUpper() && x.IsDeleted == false))
                return BadRequest(ConfigMsg.ValidationElementtExist);
            
            contragent.DateTimeCreated = DateTime.Now;
            contragent = ModelTransfer.GetModel(contragent);

            Ef.Add(contragent);
            Ef.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Обновление модели
        /// </summary>
        /// <param name="token">Действующий токен</param>
        /// <param name="contragent">Модель контрагента с сохранением ID</param>
        /// <returns>200 - в случае успешного создания</returns>
        [HttpPut("Update")]
        public IActionResult Update([FromHeader] string token, [FromBody] Contragent contragent)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanEditContragents)
                return StatusCode(403, ConfigMsg.NotAllowed);
            
            var found = Ef.Contragents
                .FirstOrDefault(x=> x.IdContragent == contragent.IdContragent);

            if (found == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);

            if (string.IsNullOrEmpty(contragent.Title))
                return BadRequest(ConfigMsg.ValidationTextEmpty);
            
            found.IsDeleted = contragent.IsDeleted;
            Ef.Update(ModelTransfer.GetModel(found));
            Ef.SaveChanges();
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
        public IActionResult Get([FromHeader] string token, string? search, long page, bool showDeleted)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanViewContragents)
                return StatusCode(403, ConfigMsg.NotAllowed);
            
            if (page < 0 )
                return BadRequest(ConfigMsg.PaginationError);

            long totalRecords = Ef.Contragents.Count(x => x.IsDeleted == showDeleted);

            long totalPages = totalRecords / ConfigSrv.RecordsByPage;

            if (page > totalPages)
                return BadRequest(ConfigMsg.PaginationError);
            
            List<Contragent> data;

            if (string.IsNullOrEmpty(search))
            {
               data = Ef.Contragents
                    .OrderBy(x => x.Title)
                    .Where(x => x.IsDeleted == showDeleted)
                    .Skip((int)(page * ConfigSrv.RecordsByPage))
                    .Take((int)ConfigSrv.RecordsByPage)
                    .ToList();
                totalRecords = Ef.Contragents.Where(x => x.IsDeleted == showDeleted).Count();
            }
            else
            {
                data = Ef.Contragents
                    .OrderBy(x => x.Title)
                    .Where(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper().Contains(search.ToUpper()))
                    .Skip((int)(page * ConfigSrv.RecordsByPage))
                    .Take((int)ConfigSrv.RecordsByPage)
                    .ToList();

                totalRecords = Ef.Contragents.Count(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper()
                    .Contains(search.ToUpper()));
                totalPages = totalRecords / ConfigSrv.RecordsByPage;
            }

            IViewAPI view = new ViewContragents()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Contragents = data,
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
                var found = Ef.Contragents.FirstOrDefault(x => x.IdContragent == item);
                if (found == null)
                    continue;
                found.IsDeleted = true;
                Ef.Update(found);
                Ef.SaveChanges();
            }

            return Ok();
        }

    }
}
