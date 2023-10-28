using Mapster;
using RegistrantApplication.Shared.API.AccountsDto;
using RegistrantApplication.Shared.API.AutoDto;
using RegistrantApplication.Shared.API.FilesDto;
using RegistrantApplication.Shared.API.OrdersDto;
using RegistrantApplication.Shared.API.Security;
using RegistrantApplication.Shared.Database.Accounts;
using RegistrantApplication.Shared.Database.Orders;

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

        config.NewConfig<Auto, DtoAuto>()
            .Map(x => x.Title, z => z.Title)
            .Map(x => x.AutoNumber, z => z.AutoNumber)
            .Map(x => x.IdAuto, z => z.IdAuto)
            .Map(x => x.IdAccount, z => z.Account!.IdAccount);

        config.NewConfig<Document, DtoDocumentAccount>()
            .Map(x => x.IdDocument, z => z.IdDocument)
            .Map(x => x.Title, z => z.Title)
            .Map(x => x.Authority, z => z.Authority)
            .Map(x => x.IdAccount, z => z.Account!.IdAccount)
            .Map(x => x.Serial, z => z.Serial)
            .Map(x => x.Number, z => z.Number)
            .Map(x => x.DateOfIssue, z => z.DateOfIssue)
            .Map(x=> x.IdFile, y=> y.FileDocument!.IdFile);

        config.NewConfig<Order, DtoOrderCreate>()
            .Map(x => x.Account, z => z.Account!.IdAccount)
            .Map(x => x.Auto, z => z.Auto!.IdAuto)
            .Map(x => x.Contragent, z => z.Contragent!.IdContragent)
            .Map(x => x.DateTimePlannedArrive, z => z.DateTimePlannedArrive)
            .Map(x => x.DateTimeRegistration, z => z.DateTimeRegistration)
            .Map(x => x.DateTimeArrived, z => z.DateTimeArrived)
            .Map(x => x.DateTimeStartOrder, z => z.DateTimeArrived)
            .Map(x => x.DateTimeEndOrder, z => z.DateTimeEndOrder)
            .Map(x => x.DateTimeLeft, z => z.DateTimeEndOrder);
    }
}