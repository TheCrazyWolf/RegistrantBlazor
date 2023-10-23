using RegistrantApplication.ApiLibrary.Controllers.Base;
using RegistrantApplication.Shared.Database.Contragents;
using RestSharp;

namespace RegistrantApplication.ApiLibrary.Controllers
{
    public class Contragents : BaseController
    {
        public Contragents(string urlConnection) : base(urlConnection)
        {
            _urlController = "Contragents";
        }

        /// <summary>
        /// Создание контрагента на сервере
        /// </summary>
        /// <param name="token">Текущий валидный токен</param>
        /// <param name="contragent">Модель контрагента</param>
        /// <returns></returns>
        public async Task<RestResponse> Create(string? token, Contragent? contragent)
        {
            var request = new RestRequest(resource: $"{_urlController}/Create", method: Method.Post);
            request.AddBody(contragent);
            request.AddHeader("Token", token);

            return await _client.ExecuteAsync(request);
        }

        /// <summary>
        /// Обновление контрагента на сервере
        /// </summary>
        /// <param name="token">Текущий валидный токен</param>
        /// <param name="contragent">Новая модель контрагента, ID не должен изменится!</param>
        /// <returns></returns>
        public async Task<RestResponse> Update(string token, Contragent contragent)
        {
            var request = new RestRequest(resource: $"{_urlController}/Update", method: Method.Put);
            request.AddBody(contragent);
            request.AddHeader("Token", token);
            
            return await _client.ExecuteAsync(request);
        }

        /// <summary>
        /// Установка флажка "Удален" контрагентами
        /// </summary>
        /// <param name="token">Текущий валидный токен</param>
        /// <param name="idsContragents">Массив ИД контрагентов</param>
        /// <returns></returns>
        public async Task<RestResponse> Delete(string token, long[] idsContragents)
        {
            var request = new RestRequest(resource: $"{_urlController}/Delete", method: Method.Delete);
            request.AddBody(idsContragents);
            request.AddHeader("Token", token);
            return await _client.ExecuteAsync(request);
        }

        /// <summary>
        /// Получить список постранично контрагентов
        /// </summary>
        /// <param name="token">Текущий валидный токен</param>
        /// <param name="search">Поисковый запрос</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="showDeleted">Показать удаленных</param>
        /// <returns></returns>
        public async Task<RestResponse> Get(string token, string? search, long page, bool showDeleted)
        {
            var request = new RestRequest(resource: $"{_urlController}/Get", method: Method.Get);
            request.AddParameter(name: "search", value: search);
            request.AddParameter(name: "page", value: page);
            request.AddParameter(name: "showDeleted", value: showDeleted);
            request.AddHeader("Token", token);

            return await _client.ExecuteAsync(request);
        }
    }
}
