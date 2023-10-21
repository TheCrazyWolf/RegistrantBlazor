using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Drivers;

namespace RegistrantApplication.Server.Controllers
{
    public class Accounts : BaseController
    {
        public Accounts(ILogger<BaseController> logger, LiteContext ef) : base(logger, ef)
        {
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Account account, IFormFile[] files)
        {
            
            if (!IsValidateToken().Result)
                return Unauthorized("Требуется авторизация");

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
                
                Document doc = new Document();
                doc.Name = file.FileName;
                doc.Data = System.IO.File.ReadAllBytes(fullPath);
                
                account.Documents.Add(doc);
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
                return NotFound("Водитель не найден");

            return Ok(currentDriver);
        }
        
        [HttpGet("Get")]
        public IActionResult Get(string? search, long page, bool showDeleted)
        {
            if (page < 0)
                return BadRequest("Страница отрицательная");

            const long recordsByPage = 10;

            long totalRecords = _ef.Contragents
                .Where(x => x.IsDeleted == showDeleted).Count();

            long totalPages = totalRecords / recordsByPage;

            if (page > totalPages)
                return BadRequest("Страница за пределами количество страниц)");

            List<Account> data;

            if (string.IsNullOrEmpty(search))
            {
                data = _ef.Accounts
                     .OrderBy(x => x.IdAccount)
                     .Where(x => x.IsDeleted == showDeleted)
                     .Skip((int)(page * recordsByPage))
                     .Take((int)recordsByPage)
                     .ToList();
                totalRecords = _ef.Contragents.Where(x => x.IsDeleted == showDeleted).Count();
            }
            else
            {
                data = _ef.Accounts
                    .OrderBy(x => x.IdAccount)
                    .Where(x => (x.IsDeleted == showDeleted) && x.Family.ToUpper().Contains(search.ToUpper()))
                    .Skip((int)(page * recordsByPage))
                    .Take((int)recordsByPage)
                    .ToList();

                totalRecords = 
                    _ef.Contragents.Count(x => (x.IsDeleted == showDeleted) && x.Title.ToUpper()
                        .Contains(search.ToUpper()));
                totalPages = totalRecords / recordsByPage;
            }

            IViewAPI view = new ViewDrivers()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = page,
                Accounts = data,
                MaxRecordsOnPageConst = recordsByPage
            };

            return Ok(view);
        }

    }
}
