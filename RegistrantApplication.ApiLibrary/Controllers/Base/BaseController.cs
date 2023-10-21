using RestSharp;

namespace RegistrantApplication.ApiLibrary.Controllers.Base
{
    public class BaseController
    {
        protected RestClient _client;
        protected string? _urlController;
        
        public BaseController(string urlConnection)
        {
            _client = new RestClient(urlConnection);
        }


    }
}
