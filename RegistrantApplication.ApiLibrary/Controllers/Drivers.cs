using RegistrantApplication.ApiLibrary.Controllers.Base;
using RegistrantApplication.Shared.Drivers;
using RestSharp;

namespace RegistrantApplication.ApiLibrary.Controllers;

public class Drivers : BaseController
{
    public Drivers(string urlConnection) : base(urlConnection)
    {
    }
    
    public async Task<RestResponse> Get(string? searchTitle, long page, bool showDeleted)
    {
        var request = new RestRequest(resource: "Drivers/Get", method: Method.Get);
        request.AddParameter("search", searchTitle);
        request.AddParameter("page", page);
        request.AddParameter("showDeleted", showDeleted);

        return await _client.ExecuteAsync(request);
    }

    public async Task<RestResponse> Get(long idDriver)
    {
        var request = new RestRequest(resource: "Driver/GetById", method: Method.Get);
        request.AddParameter("idDriver", idDriver);
        return await _client.ExecuteAsync(request);
    }
}