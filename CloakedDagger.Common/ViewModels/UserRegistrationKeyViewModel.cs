using System;

namespace CloakedDagger.Common.ViewModels;

public class UserRegistrationKeyViewModel
{
    public Guid Id { get; set; }
    
    public string Key { get; set; }
    
    public int UsesRemaining { get; set; }
    
    public DateTime CreateDate { get; set; }
    
}