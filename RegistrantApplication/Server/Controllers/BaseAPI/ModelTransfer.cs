using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Database.Accounts;
using RegistrantApplication.Shared.Database.Contragents;

namespace RegistrantApplication.Server.Controllers.BaseAPI;

public static class ModelTransfer
{
    /*
    public static async Task<Account> FromFormCreate(Account? entityAccount, FormAccount form, AccountRole role, LiteContext ef)
    {
        if (entityAccount == null)
            entityAccount = new Account();
        
        entityAccount.Family = form.Family.ToUpper();
        entityAccount.Name = form.Name.ToUpper();
        entityAccount.Patronymic = form.Patronymic?.ToUpper();
        entityAccount.PhoneNumber = string.IsNullOrEmpty(form.PhoneNumber) ? null : ValidationNumber(form.PhoneNumber);
        entityAccount.IsEmployee = form.IsEmployee;

        if (role.CanChangeRoles)
            entityAccount.AccountRole = await ef.AccountRoles.FirstOrDefaultAsync(x => x.IdRole == form.IdAccountRole);
        else
            entityAccount.AccountRole = await ef.AccountRoles.FirstOrDefaultAsync(x => x.IsDefault == true);

        entityAccount.PasswordHash = string.IsNullOrEmpty(form.PasswordHash)
            ? null
            : await ModelTransfer.GetMd5(form.PasswordHash);
        entityAccount.IsDeleted = false;
        
        return entityAccount;
    }

    public static async Task<Account> FromFormUpdate(Account? entityAccount, FormAccount form, AccountRole role, LiteContext ef)
    {
        if (entityAccount == null)
            entityAccount = new Account();
        
        entityAccount.Family = form.Family.ToUpper();
        entityAccount.Name = form.Name.ToUpper();
        entityAccount.Patronymic = form.Patronymic?.ToUpper();
        entityAccount.PhoneNumber = string.IsNullOrEmpty(form.PhoneNumber) ? null : ValidationNumber(form.PhoneNumber);
        
        if (entityAccount.AccountRole.CanChangeRoles)
            entityAccount.AccountRole =
                await ef.AccountRoles.FirstOrDefaultAsync(x => x.IdRole == form.IdAccountRole);
        else
            entityAccount.AccountRole = await ef.AccountRoles.FirstOrDefaultAsync(x => x.IsDefault == true);
        
        if (!string.IsNullOrEmpty(form.PasswordHash))
            entityAccount.PasswordHash = string.IsNullOrEmpty(form.PasswordHash)
                ? null
                : await ModelTransfer.GetMd5(form.PasswordHash);
        
        entityAccount.IsEmployee = form.IsEmployee;
        
        if(entityAccount.IsDeleted |= form.IsDeleted)
            if (entityAccount.AccountRole.CanDeleteAccounts)
                entityAccount.IsDeleted = form.IsDeleted;
        
        return entityAccount;
    }


    public static async Task<Auto> FromFormCreate(Auto? entityAuto, FormAuto form, AccountRole role, LiteContext ef)
    {
        if (entityAuto == null)
            entityAuto = new Auto();

        entityAuto.AutoNumber = form.AutoNumber.ToString();
        entityAuto.Title = form.Title.ToString();
        entityAuto.IsDeleted = false;
        
        if (role.CanChangeRoles)
            entityAuto.Account =
                await ef.Accounts.FirstOrDefaultAsync(x => x.IdAccount == form.IdAccount);

        return entityAuto;

    }
    
    public static async Task<Auto> FromFormUpdate(Auto? entityAuto, FormAuto form, AccountRole role, LiteContext ef)
    {
        if (entityAuto == null)
            entityAuto = new Auto();

        entityAuto.AutoNumber = form.AutoNumber?.ToUpper();
        entityAuto.Title = form.Title.ToString();

        if (role.CanDeleteAutos)
            entityAuto.IsDeleted = form.IsDeleted;

        if (!string.IsNullOrEmpty(form.IdAccount.ToString()))
            entityAuto.Account = await ef.Accounts.FirstOrDefaultAsync(x => x.IdAccount == form.IdAccount);
        
        return entityAuto;

    }
    
    
    public static async Task<Contragents> FromFormCreate(Contragent contragent, )
    
    
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