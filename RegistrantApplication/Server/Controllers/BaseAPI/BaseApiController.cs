using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Server.Controllers.BaseAPI
{
    public class BaseApiController : ControllerBase
    {
        protected readonly ILogger<BaseApiController> _logger;
        protected readonly LiteContext Ef;
        
        public BaseApiController(ILogger<BaseApiController> logger, LiteContext ef)
        {
            _logger = logger;
            Ef = ef;
        }
        
        /// <summary>
        /// Валидация текущего токена
        /// </summary>
        /// <param name="token">Токен для проверки валидации</param>
        /// <param name="session"></param>
        /// <returns>Булевое знание - Валиден/Нет</returns>
        protected bool IsValidateToken(string token, out AccountSession? session)
        {
            if (string.IsNullOrEmpty(token))
            {
                session = null;
                return false;
            }

            session =  Ef?.AccountsSessions
                .Include(x => x.Account)
                .Include(x => x.Account.AccountRole)
                .FirstOrDefault(x => x.Token == token && x.DateTimeSessionExpired >= DateTime.Now);

            if (session == null)
                return false;
            
            return true;
        }
    }
}
