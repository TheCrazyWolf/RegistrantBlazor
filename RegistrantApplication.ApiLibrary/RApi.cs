using RegistrantApplication.ApiLibrary.Controllers;

namespace RegistrantApplication.ApiLibrary
{
    public class RApi
    {
        public Contragents Contragents { get; private set; }
        public Drivers Drivers { get; private set; }
        public RApi(string urlConnection)
        {
            Contragents = new Contragents(urlConnection);
            Drivers = new Drivers(urlConnection);
        }
    }
}