using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Accounts;

namespace RegistrantApplication.Server.Controllers.Base
{
    public class BaseApiController : ControllerBase
    {
        protected readonly ILogger<BaseApiController> _logger;
        protected readonly LiteContext _ef;
        protected Session? _session;
        
        public BaseApiController(ILogger<BaseApiController> logger, LiteContext ef)
        {
            _logger = logger;
            _ef = ef;
        }
        
        
        /// <summary>
        /// Валидация текущего токена
        /// </summary>
        /// <param name="token">Токен для проверки валидации</param>
        /// <returns>Булевое знание - Валиден/Нет</returns>
        protected async Task<bool> IsValidateToken()
        {
            Request.Headers.TryGetValue("Token", out var values);
            string tokenString = values.ToString();
            
            if (string.IsNullOrEmpty(tokenString))
                return false;

            _session = await _ef.AccountsSessions
                .Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Token == tokenString && x.DateTimeSessionExpired >= DateTime.Now);

            if (_session == null)
                return false;
            
            return true;
        }
        
        /// <summary>
        /// Валидация текущего токена
        /// </summary>
        /// <param name="token">Токен для проверки валидации</param>
        /// <returns>Булевое знание - Валиден/Нет</returns>
        protected async Task<bool> IsValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            _session = await _ef.AccountsSessions
                .Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Token == token && x.DateTimeSessionExpired >= DateTime.Now);

            if (_session == null)
                return false;
            
            return true;
        }
    }
}
