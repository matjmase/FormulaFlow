using FormulaFlow.Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.Models
{
    public class NetworkCardToNetworkCard : BaseIdEntityModel
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public int Order { get; set; }

        public virtual NetworkCard Parent { get; set; }

        public virtual NetworkCard Child { get; set; }
    }
}
