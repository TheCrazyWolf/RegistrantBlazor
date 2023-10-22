using RegistrantApplication.ApiLibrary.Controllers.Base;
using RestSharp;

namespace RegistrantApplication.ApiLibrary.Controllers;

public class Security : BaseController
{
    public Security(string urlConnection) : base(urlConnection)
    {
        _urlController = "Security";
    }

    /// <summary>
    /// Авторизация на сервере и получение уникального токена
    /// </summary>
    /// <param name="phone">Телефон</param>
    /// <param name="password">Нехешированный пароль</param>
    /// <param name="family">Фамилия</param>
    /// <param name="isEmployee">Авторизоваться как сотрудник</param>
    /// <returns></returns>
    public async Task<RestResponse> GetToken(string phone, string? password, string? family, bool isEmployee, string? fingerprint)
    {
        var request = new RestRequest($"{_urlController}/GetToken", Method.Get);
        request.AddParameter("phone", phone);
        request.AddParameter("password", password);
        request.AddParameter("isEmployee", isEmployee);
        request.AddParameter("FingerPrintIdentity", fingerprint);

        return await _client.ExecuteAsync(request);
    }

    /// <summary>
    /// Принудительная деактивация токенов
    /// </summary>
    /// <param name="arrayTokens">Массив токенов в виде строки</param>
    /// <returns></returns>
    public async Task<RestResponse> PostResetToken(string[] arrayTokens)
    {
        var request = new RestRequest($"{_urlController}/ResetToken", Method.Post);
        request.AddBody(arrayTokens);
        return await _client.ExecuteAsync(request);
    }


}