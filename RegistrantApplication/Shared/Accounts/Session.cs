﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using RegistrantApplication.Shared.Drivers;

namespace RegistrantApplication.Shared.Accounts;

public class Session
{
    [Key]
    public string Token { get; set; }
    public Account Account { get; set; }
    public DateTime DateTimeSessionStarted { get; set; }
    public DateTime DateTimeSessionExpired { get; set; }
    
    public string? FingerPrintIdentity { get; set; }
}