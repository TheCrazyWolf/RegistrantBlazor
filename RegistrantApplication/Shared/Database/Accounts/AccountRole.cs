using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Database.Accounts;

public class AccountRole
{
    [Key]
    public long IdRole { get; set; }
    public required string Title { get; set; }
    public bool IsDefault { get; set; }
    
    public bool CanLogin { get; set; }
    
    public bool CanViewAccount { get; set; }
    public bool CanCreateAccount { get; set; }
    public bool CanEditAccount { get; set; }
    public bool CanChangeRole { get; set; }
    public bool CanDeleteAccount { get; set; }

    public bool CanViewAuto { get; set; }
    public bool CanCreateAuto { get; set; }
    public bool CanEditAuto{ get; set; }
    public bool CanDeleteAuto{ get; set; }
    
    public bool CanViewDocument { get; set; }
    public bool CanCreateDocument { get; set; }
    public bool CanEditDocument { get; set; }
    public bool CanDeleteDocument { get; set; }
    
    public bool CanViewContragent { get; set; }
    public bool CanCreateContragent { get; set; }
    public bool CanEditContragent { get; set; }
    public bool CanDeleteContragent { get; set; }
    
    public bool CanViewOrder { get; set; }
    public bool CanCreateOrder { get; set; }
    public bool CanEditOrder { get; set; }
    public bool CanDeleteOrder { get; set; }
    
    public bool CanViewOrderDetails { get; set; }
    public bool CanCreateOrderDetails  { get; set; }
    public bool CanEditOrderDetails  { get; set; }
    public bool CanDeleteOrderDetails  { get; set; }
    
 
}