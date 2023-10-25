using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Database.Admin;
using ModelTransfer = RegistrantApplication.Server.Controllers.BaseAPI.ModelTransfer;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Events : BaseApiController
{
    public Events(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
    {
    }

    /// <summary>
    /// Создает событие о том, что произошло в этом мире
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="event">Модель лога</param>
    /// <returns>Возращает 200 - в случае успешного сохранения лога</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, [FromBody] Event @event)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        @event.Account = session.Account;
        @event = ModelTransfer.GetModel(@event);
        Ef.Add(@event);
        await Ef.SaveChangesAsync();
        return Ok();
    }
    
    /// <summary>
    /// Возращает список логов постранично
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="page">Номер страницы</param>
    /// <returns>Возращает IViewAPI с коллекцией логов</returns>
    [HttpGet("GetEvents")]
    public async Task<IActionResult> GetEvents([FromHeader] string token, long page)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if (session != null && !session.Account.AccountRole.CanViewLogs)
            return StatusCode(403, ConfigMsg.NotAllowed);

        long totalRecords = Ef.Events.Count();
        var totalPages = totalRecords / ConfigSrv.RecordsByPage;
        
        if (page < 0 && page > totalPages )
            return BadRequest(ConfigMsg.PaginationError);

        var events = Ef.Events
            .Include(x=> x.Account)
            .OrderByDescending(x => x.IdEvent)
            .Skip((int)(page * ConfigSrv.RecordsByPage))
            .Take((int)ConfigSrv.RecordsByPage)
            .ToList();

        IViewAPI result = new ViewEvents()
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            MaxRecordsOnPageConst = ConfigSrv.RecordsByPage,
            Events = events
        };

        return Ok(result);
    }
    
    
}