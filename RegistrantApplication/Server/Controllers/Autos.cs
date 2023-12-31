﻿using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API.AutoDto;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Autos : BaseApiController
{
    public Autos(ILogger<BaseApiController> logger, LiteContext ef, IMapper mapper) : base(logger, ef, mapper)
    {
    }
    
    /// <summary>
    /// Получение списка машин закрепленных за учетной записью пользователя
    /// </summary>
    /// <param name="token">Действующий токен</param>
    /// <param name="idAccount">ID аккаунта</param>
    /// <param name="showDeleted">Показывать удаленных</param>
    /// <returns>Возращает коллекцию машин</returns>
    [HttpGet("Get")]
    public async Task<IActionResult> Get([FromHeader] string token, long idAccount, bool showDeleted)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (!session!.Account.AccountRole!.CanViewAutos)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var autoList =  _ef.AccountsAutos
            .Include(x => x.Account)
            .Where(x => x.Account != null && x.Account.IdAccount == idAccount && x.IsDeleted == showDeleted)
            .ToList();

        if (autoList! == null!)
            return NotFound(ConfigMsg.ValidationElementNotFound);
        
        return Ok(autoList.Adapt<List<DtoAuto>>());
    }

    
    /// <summary>
    /// Добавление машин за учетной записью
    /// </summary>
    /// <param name="token">Действующий токен</param>
    /// <param name="form">Модель машины</param>
    /// <returns>200 в случае успешного сохранения</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, [FromBody] DtoAuto form)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (!session!.Account.AccountRole!.CanCreateAutos)
            return StatusCode(403, ConfigMsg.NotAllowed);
        
        var account = await _ef.Accounts
            .FirstOrDefaultAsync(x => x.IdAccount == form.IdAccount);

        if (account == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        var newAuto = form.Adapt<Auto>();
        newAuto.Account = await _ef.Accounts.FirstOrDefaultAsync(x => x.IdAccount == form.IdAccount);
        await _ef.AddAsync(newAuto);
        await _ef.SaveChangesAsync();
        
        return Ok(newAuto.Adapt<DtoAuto>());
    }
    
    /// <summary>
    /// Обновление данных о машине
    /// </summary>
    /// <param name="token">Действующий токен</param>
    /// <param name="form">Модель машины с сохранением прошлого ID</param>
    /// <returns>200 в случае успешного сохранения</returns>
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromHeader] string token, [FromBody] DtoAuto form)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (!session!.Account.AccountRole!.CanEditAutos)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundAuto = await _ef.AccountsAutos
            .Include(x=> x.Account)
            .FirstOrDefaultAsync(x => x.IdAuto == form.IdAuto);

        if (foundAuto == null)
            return NoContent();

        foundAuto.Adapt(form);
        
        if(foundAuto.Account!.IdAccount != form.IdAccount)
            foundAuto.Account = await _ef.Accounts.FirstOrDefaultAsync(x => x.IdAccount == form.IdAccount);
        
        _ef.Update(foundAuto);
        await _ef.SaveChangesAsync();
        return Ok(foundAuto.Adapt<DtoAuto>());
    }
    
    /// <summary>
    /// Удаление/Скрытие машины
    /// </summary>
    /// <param name="token">Действующий токен</param>
    /// <param name="idAutos">ID представленных в массиве</param>
    /// <returns>200 в случае успешного удаления</returns>
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] string token, [FromBody] long[] idAutos)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (!session!.Account.AccountRole!.CanDeleteAutos)
            return StatusCode(403, ConfigMsg.NotAllowed);

        foreach (var item in idAutos)
        {
            var foundAuto = await _ef.AccountsAutos
                .FirstOrDefaultAsync(x => x.IdAuto == item);
            
            if(foundAuto == null)
                continue;

            foundAuto.IsDeleted = true;
            _ef.Update(foundAuto);
            await _ef.SaveChangesAsync();
        }

        return Ok();
    }
    
}