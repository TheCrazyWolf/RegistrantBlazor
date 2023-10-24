using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Accounts : BaseApiController
    {
        public Accounts(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
        {
        }

        /// <summary>
        /// Получение информации о текущем аккаунте
        /// </summary>
        /// <param name="token">Валидный токен</param>
        /// <returns>Возращает инорфмацию о аккаунте с токенам</returns>
        [HttpGet("GetAccountDetails")]
        public IActionResult GetAccountDetails([FromHeader] string token)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanLogin)
                return StatusCode(403, ConfigMsg.NotAllowed);
            
            return Ok(session.Account);
        }

        /// <summary>
        /// Создает новый аккаунт
        /// </summary>
        /// <param name="token">Валидный токен</param>
        /// <param name="account">Аккаунт</param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromHeader] string token, [FromBody] Account account)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanCreateAccounts)
                return StatusCode(403, ConfigMsg.NotAllowed);

            if (Ef.Accounts.Any(x =>
                    x.PhoneNumber == MyValidator.ValidationNumber(account.PhoneNumber) && x.IsDeleted == false))
                return BadRequest("Этот объект уже существует");

            if(!session.Account.AccountRole.CanChangeRoles)
                account.AccountRole = await Ef.AccountRoles.FirstOrDefaultAsync(x => x.IsDefault == true);
            
            account = MyValidator.GetModel(new Account(), account);
            account.PasswordHash = string.IsNullOrEmpty(account.PasswordHash) ? null : await MyValidator.GetMd5(account.PasswordHash);
            account.IsDeleted = false;
            
            await Ef.AddAsync(account);
            await Ef.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromHeader] string token, [FromBody] Account account)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanEditAccount)
                return StatusCode(403, ConfigMsg.NotAllowed);

            var foundAccount = await Ef.Accounts
                .Include(x=> x.AccountRole)
                .FirstOrDefaultAsync(x => x.IdAccount == account.IdAccount);

            if (foundAccount == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);
            
            if(foundAccount.PhoneNumber != MyValidator.ValidationNumber(account.PhoneNumber))
                if(Ef.Accounts.Any(x=> x.PhoneNumber == MyValidator.ValidationNumber(account.PhoneNumber) && x.IsDeleted == false))
                    return BadRequest("Этот объект уже существует");
            
            foundAccount = MyValidator.GetModel(foundAccount, account);
            if (session != null && !session.Account.AccountRole.CanChangeRoles)
                foundAccount.AccountRole = account.AccountRole;
            
            if(!string.IsNullOrEmpty(account.PasswordHash))
                foundAccount.PasswordHash = string.IsNullOrEmpty(account.PasswordHash) ? null : await MyValidator.GetMd5(account.PasswordHash);

            Ef.Update(foundAccount);
            await Ef.SaveChangesAsync();

            return Ok();
        }
        
        [HttpGet("GetById")]
        public IActionResult GetById([FromHeader] string token, long idDriver)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanViewAccounts)
                return StatusCode(403, ConfigMsg.NotAllowed);

            var currentDriver = Ef.Accounts
                .FirstOrDefault(x => x.IdAccount == idDriver);

            if (currentDriver == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);

            return Ok(currentDriver);
        }
        
        [HttpGet("Get")]
        public IActionResult Get([FromHeader] string token, string? search, long page, bool showEmployee, bool showDeleted)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanViewAccounts)
                return StatusCode(403, ConfigMsg.NotAllowed);
            
            if (page < 0)
                return BadRequest(ConfigMsg.PaginationError);

            long totalRecords = Ef.Accounts
                .Count(x => x.IsDeleted == showDeleted && x.IsEmployee== showEmployee);

            long totalPages = totalRecords / ConfigSrv.RecordsByPage;

            if (page > totalPages)
                return BadRequest(ConfigMsg.PaginationError);

            List<Account> data;

            if (string.IsNullOrEmpty(search))
            {
                data = Ef.Accounts
                     .OrderBy(x => x.IdAccount)
                     .Where(x => x.IsDeleted == showDeleted && x.IsEmployee == showEmployee)
                     .Skip((int)(page * ConfigSrv.RecordsByPage))
                     .Take((int)ConfigSrv.RecordsByPage)
                     .ToList();
                totalRecords = Ef.Accounts.Count(x => x.IsDeleted == showDeleted && x.IsEmployee == showEmployee);
            }
            else
            {
                data = Ef.Accounts
                    .OrderBy(x => x.IdAccount)
                    .Where(x => (x.IsDeleted == showDeleted && x.IsEmployee == showEmployee) 
                                && x.Family.ToUpper().Contains(search.ToUpper()))
                    .Skip((int)(page * ConfigSrv.RecordsByPage))
                    .Take((int)ConfigSrv.RecordsByPage)
                    .ToList();

                totalRecords = 
                    Ef.Accounts.Count(x => (x.IsDeleted == showDeleted && x.IsEmployee == showEmployee) && x.Family.ToUpper()
                        .Contains(search.ToUpper()));
                totalPages = totalRecords / ConfigSrv.RecordsByPage;
            }

            IViewAPI view = new ViewAccounts()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Accounts = data,
                MaxRecordsOnPageConst = ConfigSrv.RecordsByPage
            };

            return Ok(view);
        }
 
    }
}
