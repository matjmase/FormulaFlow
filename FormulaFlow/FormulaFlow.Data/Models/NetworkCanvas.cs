using FormulaFlow.Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.Models
{
    public class NetworkCanvas : OwnerEntityModel
    {
        public string Name { get; set; }
        public double Scale { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }


        public virtual ICollection<NetworkCard>? Cards { get; set; }
    }
}
