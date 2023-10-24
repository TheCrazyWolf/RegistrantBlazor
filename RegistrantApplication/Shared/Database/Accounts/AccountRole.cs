using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Database.Accounts;

public class AccountRole
{
    [Key]
    public long IdRole { get; set; }
    public required string Title { get; set; }
    public bool IsDefault { get; set; }
    
    public bool CanLogin { get; set; }
    
    public bool CanViewAccounts { get; set; }
    public bool CanCreateAccounts { get; set; }
    public bool CanEditAccount { get; set; }
    public bool CanChangeRoles { get; set; }
    public bool CanDeleteAccounts { get; set; }
    
    public bool CanViewRoles { get; set; }
    public bool CanCreateRoles { get; set; }
    public bool CanEditRoles { get; set; }
    public bool CanDeleteRoles { get; set; }
    
    public bool CanViewAutos { get; set; }
    public bool CanCreateAutos { get; set; }
    public bool CanEditAutos { get; set; }
    public bool CanDeleteAutos { get; set; }
    
    public bool CanViewDocuments { get; set; }
    public bool CanCreateDocuments { get; set; }
    public bool CanEditDocuments { get; set; }
    public bool CanDeleteDocuments { get; set; }
    
    public bool CanViewContragents { get; set; }
    public bool CanCreateContragents { get; set; }
    public bool CanEditContragents { get; set; }
    public bool CanDeleteContragents { get; set; }
    
    public bool CanViewOrders { get; set; }
    public bool CanCreateOrders { get; set; }
    public bool CanEditOrders { get; set; }
    public bool CanDeleteOrders { get; set; }
    
    public bool CanViewOrderDetails { get; set; }
    public bool CanCreateOrderDetails  { get; set; }
    public bool CanEditOrderDetails  { get; set; }
    public bool CanDeleteOrderDetails  { get; set; }
    
    public bool CanViewLogs { get; set; }
 
}