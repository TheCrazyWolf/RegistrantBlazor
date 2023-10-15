using RegistrantApplication.ApiLibrary.Controllers;

namespace RegistrantApplication.ApiLibrary
{
    public class RApi
    {
        public Contragents Contragents { get; set; }
        public RApi(string urlConnection)
        {
            Contragents = new Contragents(urlConnection);
        }
    }
}