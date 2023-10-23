using RegistrantApplication.Shared.Database.Accounts;
using RegistrantApplication.Shared.Database.Contragents;
using RegistrantApplication.Shared.Database.Drivers;

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

    public static Account GetModel(Account account)
    {
        account.Family = account.Family.ToUpper();
        account.Name = account.Name.ToUpper();
        account.Patronymic = account.Patronymic?.ToUpper();
        account.PhoneNumber = string.IsNullOrEmpty(account.PhoneNumber) ? null : ValidationNumber(account.PhoneNumber);

        return account;
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
    
    public static async Task<string> GetMd5(string? input)
    {
        if (string.IsNullOrEmpty(input))
            if (input != null)
                return input;

        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            await Task.Delay(0);
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            string data = Convert.ToHexString(hashBytes);
            return data; // .NET 5 +
            // Convert the byte array to hexadecimal string prior to .NET 5
            // StringBuilder sb = new System.Text.StringBuilder();
            // for (int i = 0; i < hashBytes.Length; i++)
            // {
            //     sb.Append(hashBytes[i].ToString("X2"));
            // }
            // return sb.ToString();
        }
    }
    
    public static async Task<string> GetUnqueStringForToken()
    {
        await Task.Delay(0);
        char[] uniqString = (Guid.NewGuid() + Guid.NewGuid().ToString()).Replace("-", string.Empty).ToArray();

        return new string(uniqString).ToString();
    }
    
}