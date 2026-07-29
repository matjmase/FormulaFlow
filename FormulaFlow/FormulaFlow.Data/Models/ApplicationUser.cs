using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<NetworkCanvas> NetworkCanvas { get; set; }
    }
}
