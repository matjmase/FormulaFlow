using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.Models.Base
{
    public class OwnerEntityModel : BaseIdEntityModel
    {
        public string OwnerUserId { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }
    }
}
