using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Accounts;
using RegistrantApplication.Shared.Drivers;

namespace RegistrantApplication.Server.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly LiteContext _ef;
        protected Session? _session;
        
        public BaseController(ILogger<BaseController> logger, LiteContext ef)
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

            _session = await _ef.Sessions
                .Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Token == tokenString && x.DateTimeSessionExpired >= DateTime.Now);

            if (_session == null)
                return false;
            
            return true;
        }
        
        
    }
}
