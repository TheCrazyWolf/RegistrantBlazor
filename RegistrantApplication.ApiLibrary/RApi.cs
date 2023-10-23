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
        /// Методы для с Аккаунтами
        /// </summary>
        public Accounts Accounts { get; private set; }
        /// <summary>
        /// Методы для работы с Авторизацией
        /// </summary>
        public Security Security { get; private set; }
        public RApi(string urlConnection)
        {
            Contragents = new Contragents(urlConnection);
            Accounts = new Accounts(urlConnection);
            Security = new Security(urlConnection);
        }
    }
}