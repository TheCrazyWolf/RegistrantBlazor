﻿using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Drivers;

namespace RegistrantApplication.Shared.API;

public class ViewDrivers : ViewBase
{
    public List<Account> Accounts { get; set; }
}