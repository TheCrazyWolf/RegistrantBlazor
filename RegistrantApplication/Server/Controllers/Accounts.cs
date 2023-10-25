using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.Accounts;
using RegistrantApplication.Shared.API.Accounts.Get;
using RegistrantApplication.Shared.API.Accounts.Post;
using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Database.Accounts;
using ModelTransfer = RegistrantApplication.Server.Controllers.BaseAPI.ModelTransfer;


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

            long totalRecords = Ef.Accounts
                .Count(x => x.IsDeleted == showDeleted && x.IsEmployee == showEmployee);

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
                    Ef.Accounts.Count(x => (x.IsDeleted == showDeleted && x.IsEmployee == showEmployee) && x.Family
                        .ToUpper()
                        .Contains(search.ToUpper()));
                totalPages = totalRecords / ConfigSrv.RecordsByPage;
            }

            GetViewAccounts view = new GetViewAccounts()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Accounts = new List<GetAccount>(),
                MaxRecordsOnPageConst = ConfigSrv.RecordsByPage
            };
            data.ForEach(x=> view.Accounts.Add(Shared.API.ModelTransfer.FromDB(x)));
            
            return Ok(view);
        }


        /// <summary>
        /// Получение информации о текущем аккаунте
        /// </summary>
        /// <param name="token">Валидный токен</param>
        /// <returns>Возращает инорфмацию о аккаунте с токенам</returns>
        [HttpGet("GetDetailsFromToken")]
        public IActionResult GetDetailsFromToken([FromHeader] string token)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanLogin)
                return StatusCode(403, ConfigMsg.NotAllowed);

            var account = Shared.API.ModelTransfer.FromDB(session.Account);
            
            return Ok(account);
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

            if (session != null && !session.Account.AccountRole.CanViewAccounts)
                return StatusCode(403, ConfigMsg.NotAllowed);

            var currentAccount = Ef.Accounts
                .FirstOrDefault(x => x.IdAccount == idAccount);

            if (currentAccount == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);
            
            var account = ModelTransfer.FromDB(currentAccount);

            return Ok(account);
        }

        /// <summary>
        /// Создает новый аккаунт
        /// </summary>
        /// <param name="token">Валидный токен</param>
        /// <param name="form">Аккаунт</param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromHeader] string token, [FromBody] FormAccount form)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanCreateAccounts)
                return StatusCode(403, ConfigMsg.NotAllowed);

            if (Ef.Accounts.Any(x =>
                    x.PhoneNumber == ModelTransfer.ValidationNumber(form.PhoneNumber) && x.IsDeleted == false))
                return BadRequest("Этот объект уже существует");

            var newAccount = await ModelTransfer.FromFormCreate(new Account(), form, session.Account.AccountRole, Ef);
            
            await Ef.AddAsync(newAccount);
            await Ef.SaveChangesAsync();

            return Ok();
        }
        
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromHeader] string token, [FromBody] FormAccount form)
        {
            if (!IsValidateToken(token, out var session))
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (session != null && !session.Account.AccountRole.CanEditAccount)
                return StatusCode(403, ConfigMsg.NotAllowed);

            var foundAccount = await Ef.Accounts
                .Include(x => x.AccountRole)
                .FirstOrDefaultAsync(x => x.IdAccount == form.IdAccount);

            if (foundAccount == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);

            if (foundAccount.PhoneNumber != ModelTransfer.ValidationNumber(form.PhoneNumber))
                if (Ef.Accounts.Any(x =>
                        x.PhoneNumber == ModelTransfer.ValidationNumber(form.PhoneNumber) && x.IsDeleted == false))
                    return BadRequest("Этот объект уже существует");

            foundAccount = await ModelTransfer.FromFormUpdate(foundAccount, form, session.Account.AccountRole, Ef);

            Ef.Update(foundAccount);
            await Ef.SaveChangesAsync();

            return Ok();
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
                var foundAccount = await Ef.Accounts.FirstOrDefaultAsync(x => x.IdAccount == accountId);
                if (foundAccount == null)
                    continue;
                foundAccount.IsDeleted = true;
                Ef.Update(foundAccount);
            }

            await Ef.SaveChangesAsync();
            return Ok();
        }
    }
}