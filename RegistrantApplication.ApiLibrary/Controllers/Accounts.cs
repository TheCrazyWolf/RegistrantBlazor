using RegistrantApplication.ApiLibrary.Controllers.Base;
using RegistrantApplication.Shared.Drivers;
using RestSharp;

namespace RegistrantApplication.ApiLibrary.Controllers;

public class Accounts : BaseController
{
    public Accounts(string urlConnection) : base(urlConnection)
    {
        _urlController = "Accounts";
    }

    /// <summary>
    /// Создание новой учетной записи
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="account">Модель аккаунта</param>
    /// <param name="paths">Пути к файлам для загрузки документов к профилю</param>
    /// <returns></returns>
    public async Task<RestResponse> Create(string? token, Account account, string[]? paths)
    {
        var request = new RestRequest(resource: $"{_urlController}/Create", method: Method.Post);
        request.AddHeader("Token", token);
        request.AddBody(account);

        if (paths != null)
        {
            foreach (var path in paths)
            {
                request.AddFile("content", path);
            }
        }

        return await _client.ExecuteAsync(request);
    }
}