using RegistrantApplication.Shared.Contragents;
using RegistrantApplication.Shared.Drivers;

namespace RegistrantApplication.Server.Controllers.BaseAPI;

public static class MyValidator
{
    public static Auto GetModel(Auto auto)
    {
        auto.Title = auto.Title?.ToUpper();
        auto.AutoNumber = auto.AutoNumber.ToUpper();
        auto.IsDeleted = auto.IsDeleted;

        return auto;
    }

    public static Contragent GetModel(Contragent contragent)
    {
        contragent.Title = contragent.Title.ToUpper();
        return contragent;
    }

    public static Document GetModel(Document document)
    {
        document.Authority = document.Authority?.ToUpper();
        document.Number = document.Number?.ToUpper();
        document.Serial = document.Serial?.ToUpper();
        return document;
    }


    public static string ValidationNumber(string number)
    {
        number = number.ToUpper();
        if (string.IsNullOrEmpty(number))
            return number;

        number = number.Replace(" ", string.Empty)
            .Replace("+", string.Empty)
            .Replace("(", string.Empty)
            .Replace(")", string.Empty)
            .Replace("-", string.Empty);

        while (number[0].ToString() == "8" || number[0].ToString() == "7")
        {
            number = number.Substring(1);
        }

        return number;
    }
}