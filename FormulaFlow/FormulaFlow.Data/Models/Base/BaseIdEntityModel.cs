using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.Models.Base
{
    public abstract class BaseIdEntityModel
    {
        public Guid Id { get; set; }

        public string? CreatedByUserId { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }

        public string? UpdatedByUserId { get; set; }
        public virtual ApplicationUser UpdatedByUser { get; set; }
    }
}
