using RegistrantApplication.ApiLibrary.Controllers.Base;
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
}