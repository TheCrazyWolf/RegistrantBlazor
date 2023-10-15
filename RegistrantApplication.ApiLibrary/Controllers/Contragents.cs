using RegistrantApplication.ApiLibrary.Controllers.Base;
using RegistrantApplication.Shared.Contragents;
using RestSharp;

namespace RegistrantApplication.ApiLibrary.Controllers
{
    public class Contragents : BaseController
    {
        public Contragents(string urlConnection) : base(urlConnection)
        {
        }

        public async Task<RestResponse> Create(Contragent contragent)
        {
            var request = new RestRequest(resource: "Contragents/Create", method: Method.Post);

            request.AddBody(contragent);

            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> Update(Contragent contragent)
        {
            var request = new RestRequest(resource: "Contragents/Update", method: Method.Put);

            request.AddBody(contragent);

            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> Delete(long[] idsContragents)
        {
            var request = new RestRequest(resource: "Contragents/Delete", method: Method.Delete);

            request.AddBody(idsContragents);

            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> Get(string? searchTitle, long pageNumber, bool showDeleted)
        {
            var request = new RestRequest(resource: "Contragents/Get", method: Method.Get);

            request.AddParameter(name: "search", value: searchTitle);
            request.AddParameter(name: "page", value: pageNumber);
            request.AddParameter(name: "showDeleted", value: showDeleted);

            return await _client.ExecuteAsync(request);
        }
    }
}
