using Mapster;
using RegistrantApplication.Shared.API.AccountsDto;
using RegistrantApplication.Shared.API.FilesDto;
using RegistrantApplication.Shared.API.Security;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Server.Configs;

public class ConfigAdapters : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FileDocument, DtoFileInfo>()
            .Map(output => output.FileName, dest => dest.FileName)
            .Map(output => output.IdFile, dest => dest.IdFile)
            .Map(output => output.SizeFile, dest => dest.DataBytes.Length);


        config.NewConfig<Account, DtoAccountView>()
            .Map(output => output.IdAccount, z => z.IdAccount)
            .Map(output => output.IsEmployee, z => z.IsEmployee)
            .Map(output => output.PhoneNumber, z => z.PhoneNumber)
            .Map(output => output.Family, z => z.Family)
            .Map(output => output.Name, z => z.Name)
            .Map(output => output.Patronymic, z => z.Patronymic)
            .Map(output => output.IdAccountRole, z => z.AccountRole!.IdRole);
        

        config.NewConfig<AccountSession, DtoAccessToken>()
            .Map(x => x.Token, z => z.Token)
            .Map(x => x.DateTimeSessionExpired, z => z.DateTimeSessionExpired)
            .Map(x => x.DateTimeSessionStarted, z => z.DateTimeSessionStarted);
    }
}