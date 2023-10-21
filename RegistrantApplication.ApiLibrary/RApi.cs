using RegistrantApplication.ApiLibrary.Controllers;

namespace RegistrantApplication.ApiLibrary
{
    public class RApi
    {
        /// <summary>
        /// Методы для работы с контрагентами
        /// </summary>
        public Contragents Contragents { get; private set; }
        /// <summary>
        /// Методы для с Водителями
        /// </summary>
        public Drivers Drivers { get; private set; }
        /// <summary>
        /// Методы для работы с Авторизацией
        /// </summary>
        public Security Security { get; private set; }
        public RApi(string urlConnection)
        {
            Contragents = new Contragents(urlConnection);
            Drivers = new Drivers(urlConnection);
            Security = new Security(urlConnection);
        }
    }
}