using RegistrantApplication.ApiLibrary.Controllers.Base;
using RestSharp;

namespace RegistrantApplication.ApiLibrary.Controllers;

public class Drivers : BaseController
{
    public Drivers(string urlConnection) : base(urlConnection)
    {
        _urlController = "Drivers";
    }
    
    public async Task<RestResponse> Get(string? search, long page, bool showDeleted)
    {
        var request = new RestRequest(resource: $"{_urlController}/Get", method: Method.Get);
        request.AddParameter("search", search);
        request.AddParameter("page", page);
        request.AddParameter("showDeleted", showDeleted);

        return await _client.ExecuteAsync(request);
    }

    public async Task<RestResponse> Get(long idDriver)
    {
        var request = new RestRequest(resource: $"{_urlController}/GetById", method: Method.Get);
        request.AddParameter("idDriver", idDriver);
        return await _client.ExecuteAsync(request);
    }
}