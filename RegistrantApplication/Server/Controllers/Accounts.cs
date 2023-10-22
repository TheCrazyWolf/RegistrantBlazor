using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.Base;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.Accounts;
using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Drivers;

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
        public async Task<IActionResult> GetAccountDetails([FromHeader] string? token)
        {
            if (!IsValidateToken(token).Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            if (_session == null)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            AccountDetails details = new AccountDetails()
            {
                Family = _session.Account.Family.ToUpper(),
                Name = _session.Account.Name.ToUpper(),
                Patronymic = _session.Account.Patronymic?.ToUpper(),
                IsEmployee = _session.Account.IsEmployee
            };

            return Ok(details);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromHeader] string? token, [FromBody] Account account, IFormFile[] files)
        {
            if (!IsValidateToken(token).Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            account.Family = account.Family.ToUpper();
            account.Name = account.Name.ToUpper();
            if (!string.IsNullOrEmpty(account.Patronymic))
                account.Patronymic = account.Patronymic.ToUpper();
            if (!string.IsNullOrEmpty(account.PasswordHash))
                account.PasswordHash = Security.GetMd5(account.PasswordHash).Result;
            account.IsDeleted = false;

            if (account.Documents == null)
                account.Documents = new List<Document>();
            foreach (var file in files)
            {
                var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
                // создаем папку для хранения файлов
                Directory.CreateDirectory(uploadPath);
                
                // путь к папке uploads
                string fullPath = $"{uploadPath}/{file.FileName}";
            
                // сохраняем файл в папку uploads
                using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
                {
                    await file.CopyToAsync(fileStream);
                }
                
                /*Document doc = new Document();
                doc.Name = file.FileName;
                doc.Data = System.IO.File.ReadAllBytes(fullPath);*/
                
                /*account.Documents.Add(doc);*/
            }
            

            await _ef.AddAsync(account);
            await _ef.SaveChangesAsync();

            return Ok();
        }
        
        [HttpGet("GetById")]
        public IActionResult GetById([FromHeader] string token, long idDriver)
        {
            if (!IsValidateToken(token).Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

            var currentDriver = _ef.Accounts
                .Include(x => x.Documents)
                .Include(x => x.Autos)
                .FirstOrDefault(x => x.IdAccount == idDriver);

            if (currentDriver == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);

            return Ok(currentDriver);
        }
        
        [HttpGet("Get")]
        public IActionResult Get([FromHeader] string? token, string? search, long page, bool showEmployee, bool showDeleted)
        {
            if (!IsValidateToken(token).Result)
                return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
            
            if (page < 0)
                return BadRequest(ConfigMsg.PaginationError);

            long totalRecords = _ef.Accounts
                .Where(x => x.IsDeleted == showDeleted && x.IsEmployee== showEmployee).Count();

            long totalPages = totalRecords / ConfigServer.RecordsByPage;

            if (page > totalPages)
                return BadRequest(ConfigMsg.PaginationError);

            List<Account> data;

            if (string.IsNullOrEmpty(search))
            {
                data = _ef.Accounts
                     .OrderBy(x => x.IdAccount)
                     .Where(x => x.IsDeleted == showDeleted && x.IsEmployee == showEmployee)
                     .Skip((int)(page * ConfigServer.RecordsByPage))
                     .Take((int)ConfigServer.RecordsByPage)
                     .ToList();
                totalRecords = _ef.Accounts.Where(x => x.IsDeleted == showDeleted && x.IsEmployee == showEmployee).Count();
            }
            else
            {
                data = _ef.Accounts
                    .OrderBy(x => x.IdAccount)
                    .Where(x => (x.IsDeleted == showDeleted && x.IsEmployee == showEmployee) 
                                && x.Family.ToUpper().Contains(search.ToUpper()))
                    .Skip((int)(page * ConfigServer.RecordsByPage))
                    .Take((int)ConfigServer.RecordsByPage)
                    .ToList();

                totalRecords = 
                    _ef.Accounts.Count(x => (x.IsDeleted == showDeleted && x.IsEmployee == showEmployee) && x.Family.ToUpper()
                        .Contains(search.ToUpper()));
                totalPages = totalRecords / ConfigServer.RecordsByPage;
            }

            IViewAPI view = new ViewAccounts()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Accounts = data,
                MaxRecordsOnPageConst = ConfigServer.RecordsByPage
            };

            return Ok(view);
        }
 
    }
}
