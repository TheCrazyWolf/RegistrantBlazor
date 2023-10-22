using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Accounts;

public class AccountRole
{
    [Key]
    public long IdRole { get; set; }
    public required string Title { get; set; }
    
    public bool CanViewDashboardDriver { get; set; }
    
    public bool CanViewDashboardSecurity { get; set; }
    public bool CanEditDashboardSecurity { get; set; }
    
    public bool CanViewDashboard { get; set; }
    public bool CanEditDashboard { get; set; }
    
    public bool CanViewContragents { get; set; }
    public bool CanAddContragents { get; set; }
    public bool CanUpdateContragents { get; set; }
    public bool CanDeleteContragents { get; set; }
    
    public bool CanViewAccounts { get; set; }
    public bool CanCreateAccounts { get; set; }
    public bool CanUpdateAccounts { get; set; }
    public bool CanDeleteAccounts { get; set; }
    
    public bool CanViewOrders { get; set; }
    public bool CanCreateOrders { get; set; }
    public bool CanUpdateOrders { get; set; }
    public bool CanDeleteOrders { get; set; }
}