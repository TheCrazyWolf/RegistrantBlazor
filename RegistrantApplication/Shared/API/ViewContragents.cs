﻿using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Database.Contragents;

namespace RegistrantApplication.Shared.API
{
    public class ViewContragents : ViewBase
    {
        public List<Contragent> Contragents { get; set; }
    }
}
