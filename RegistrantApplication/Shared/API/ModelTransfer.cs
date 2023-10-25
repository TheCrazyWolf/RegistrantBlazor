using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Shared.API;

public static class ModelTransfer
{
    /*
    public static Account FromForm(Account? entityAccount, FormAccount form)
    {
        if (entityAccount == null)
            entityAccount = new Account();
        
        entityAccount.Family = form.Family.ToUpper();
        entityAccount.Name = form.Name.ToUpper();
        entityAccount.Patronymic = form.Patronymic?.ToUpper();
        entityAccount.PhoneNumber = string.IsNullOrEmpty(form.PhoneNumber) ? null : ValidationNumber(form.PhoneNumber);
        entityAccount.IsEmployee = form.IsEmployee;
        entityAccount.IsDeleted = form.IsDeleted;
        
        return entityAccount;
    }

    public static Account FromForm(Account? entityAccount, FormAccount form, AccountRole role)
    {
        if (entityAccount == null)
            entityAccount = new Account();
        
        entityAccount.Family = form.Family.ToUpper();
        entityAccount.Name = form.Name.ToUpper();
        entityAccount.Patronymic = form.Patronymic?.ToUpper();
        entityAccount.PhoneNumber = string.IsNullOrEmpty(form.PhoneNumber) ? null : ValidationNumber(form.PhoneNumber);
        entityAccount.IsEmployee = form.IsEmployee;
        entityAccount.IsDeleted = form.IsDeleted;
        
        return entityAccount;
    }
    
    public static GetAccount FromDB(Account account)
    {
        var view = new GetAccount()
        {
            
            IdAccount = account.IdAccount,
            IdAccountRole = account.AccountRole?.IdRole,
            Family = account.Family,
            Name = account.Family,
            Patronymic = account.Patronymic?.ToString(),
            PhoneNumber = account.PhoneNumber?.ToString(),
            IsEmployee = account.IsEmployee,
            IsDeleted = account.IsDeleted
        };

        return view;
    }
    */
    
    
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