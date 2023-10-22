using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.Base;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
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


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Account account, IFormFile[] files)
        {
            
            if (!IsValidateToken().Result)
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
        public IActionResult GetById(long idDriver)
        {

            var currentDriver = _ef.Accounts
                .Include(x => x.Documents)
                .Include(x => x.Autos)
                .FirstOrDefault(x => x.IdAccount == idDriver);

            if (currentDriver == null)
                return NotFound(ConfigMsg.ValidationElementNotFound);

            return Ok(currentDriver);
        }
        
        [HttpGet("Get")]
        public IActionResult Get(string? search, long page, bool showEmployee, bool showDeleted)
        {
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


        [NonAction]
        public static string ValidationNumber(string inpute)
        {
            if (string.IsNullOrEmpty(inpute))
                return inpute;
            
            inpute = inpute.Replace(" ", string.Empty)
                .Replace("+", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("-",string.Empty);

            while (inpute[0].ToString() == "8" || inpute[0].ToString() == "7")
            {
                inpute = inpute.Substring(1);
            }

            return inpute;
        }

    }
}
