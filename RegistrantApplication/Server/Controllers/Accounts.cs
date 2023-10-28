using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API.AccountsDto;
using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Database.Accounts;
using ModelTransfer = RegistrantApplication.Server.Controllers.BaseAPI.ModelTransfer;

namespace RegistrantApplication.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Accounts : BaseApiController
    {
        public Accounts(ILogger<BaseApiController> logger, LiteContext ef, IMapper mapper) : base(logger, ef, mapper)
        {
        }

        /// <summary>
        /// Получить список аккаунтов по странично
        /// </summary>
        /// <param name="token">Валидный токен</param>
        /// <param name="search">Поисковой запрос</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="showEmployee">Показать сотрудников</param>
        /// <param name="showDeleted">Показать удаленные</param>
        /// <returns></returns>
        [HttpGet("Get")]
        public IActionResult Get([FromHeader] string token, string? search, long page, bool showEmployee,
            bool showDeleted)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanViewAccounts)
                return StatusCode(403, ConfigMsg.NotAllowed);

            if (page < 0)
                return BadRequest(ConfigMsg.PaginationError);

            long totalRecords = _ef.Accounts
                .Count(x => x.IsDeleted == showDeleted && x.IsEmployee == showEmployee);

            long totalPages = totalRecords / ConfigSrv.RecordsByPage;

            if (page > totalPages)
                return BadRequest(ConfigMsg.PaginationError);

            List<Account> data;

            if (string.IsNullOrEmpty(search))
            {
                data = _ef.Accounts
                    .OrderBy(x => x.IdAccount)
                    .Where(x => x.IsDeleted == showDeleted && x.IsEmployee == showEmployee)
                    .Skip((int)(page * ConfigSrv.RecordsByPage))
                    .Take((int)ConfigSrv.RecordsByPage)
                    .ToList();
                totalRecords = _ef.Accounts.Count(x => x.IsDeleted == showDeleted && x.IsEmployee == showEmployee);
            }
            else
            {
                data = _ef.Accounts
                    .OrderBy(x => x.IdAccount)
                    .Where(x => (x.IsDeleted == showDeleted && x.IsEmployee == showEmployee)
                                && x.Family.ToUpper().Contains(search.ToUpper()))
                    .Skip((int)(page * ConfigSrv.RecordsByPage))
                    .Take((int)ConfigSrv.RecordsByPage)
                    .ToList();

                totalRecords =
                    _ef.Accounts.Count(x => (x.IsDeleted == showDeleted && x.IsEmployee == showEmployee) && x.Family
                        .ToUpper()
                        .Contains(search.ToUpper()));
                totalPages = totalRecords / ConfigSrv.RecordsByPage;
            }

            DtoDtoViewAccounts dtoDtoView = new DtoDtoViewAccounts()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Accounts = data.Adapt<List<DtoAccountView>>(),
                MaxRecordsOnPageConst = ConfigSrv.RecordsByPage
            };
            
            return Ok(dtoDtoView);
        }
        
        /// <summary>
        /// Получить информацию о конкретном аккаунте
        /// </summary>
        /// <param name="token">Валидный токен</param>
        /// <param name="idAccount">ID акканута</param>
        /// <returns></returns>
        [HttpGet("GetDetails")]
        public IActionResult GetDetails([FromHeader] string token, long idAccount)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (idAccount == 0)
                return Ok(session.Account.Adapt<DtoAccountView>());

            if (session != null && !session.Account.AccountRole.CanViewAccounts)
                return StatusCode(403, ConfigMsg.NotAllowed);

            var currentAccount = _ef.Accounts
                .FirstOrDefault(x => x.IdAccount == idAccount);

            if (currentAccount == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);

            var account = currentAccount.Adapt<DtoAccountView>();

            return Ok(account);
        }

        /// <summary>
        /// Создает новый аккаунт
        /// </summary>
        /// <param name="token">Валидный токен</param>
        /// <param name="form">Аккаунт</param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromHeader] string token, [FromBody] DtoAccountCreate form)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanCreateAccounts)
                return StatusCode(403, ConfigMsg.NotAllowed);

            if (_ef.Accounts.Any(x =>
                    x.PhoneNumber == ModelTransfer.ValidationNumber(form.PhoneNumber) && x.IsDeleted == false))
                return BadRequest("Этот объект уже существует");

            var newAccount = form.Adapt<Account>();
            newAccount.PasswordHash = await ModelTransfer.GetMd5(form.PasswordHash);
            newAccount.PhoneNumber = ModelTransfer.ValidationNumber(form.PhoneNumber);
            
            await _ef.AddAsync(newAccount);
            await _ef.SaveChangesAsync();

            return Ok(newAccount.Adapt<DtoAccountView>());
        }
        
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromHeader] string token, [FromBody] DtoAccountCreate form)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanEditAccount)
                return StatusCode(403, ConfigMsg.NotAllowed);

            var foundAccount = await _ef.Accounts
                .Include(x => x.AccountRole)
                .FirstOrDefaultAsync(x => x.IdAccount == form.IdAccount);

            if (foundAccount == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);

            if (foundAccount.PhoneNumber != ModelTransfer.ValidationNumber(form.PhoneNumber))
                if (_ef.Accounts.Any(x =>
                        x.PhoneNumber == ModelTransfer.ValidationNumber(form.PhoneNumber) && x.IsDeleted == false))
                    return BadRequest("Этот объект уже существует");

            if (!string.IsNullOrEmpty(form.PasswordHash))
                foundAccount.PasswordHash = await ModelTransfer.GetMd5(form.PasswordHash);

            if ((foundAccount.AccountRole == null) || form.IdAccountRole != foundAccount.AccountRole.IdRole)
                foundAccount.AccountRole = await _ef.AccountRoles.FirstOrDefaultAsync(x =>x.IdRole== form.IdAccountRole);
            
            foundAccount.Adapt(form);
            _ef.Update(foundAccount);
            await _ef.SaveChangesAsync();

            return Ok(foundAccount.Adapt<DtoAccountView>());
        }
        
        /// <summary>
        /// Удаление аккаунтов
        /// </summary>
        /// <param name="token">Валидный токен</param>
        /// <param name="idsAccount">Массив ID аккаунтов</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromHeader] string token, [FromBody] long[] idsAccount)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (!session.Account.AccountRole.CanDeleteAccounts)
                return StatusCode(403, ConfigMsg.NotAllowed);

            foreach (var accountId in idsAccount)
            {
                var foundAccount = await _ef.Accounts.FirstOrDefaultAsync(x => x.IdAccount == accountId);
                if (foundAccount == null)
                    continue;
                foundAccount.IsDeleted = true;
                _ef.Update(foundAccount);
            }

            await _ef.SaveChangesAsync();
            return Ok();
        }
        
    }
}