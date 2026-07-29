using FormulaFlow.Data.Enum;
using FormulaFlow.Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.Models
{
    public class NetworkCard : BaseIdEntityModel
    {
        public string Name { get; set; }

        public int Top { get; set; }
        public int Left { get; set; }

        public NetworkCardType NetworkType { get; set; }

        public Guid NetworkCanvasId { get; set; }
        public virtual NetworkCanvas NetworkCanvas { get; set; }


        public virtual ICollection<NetworkParameter>? Parameters { get; set; }

        public virtual ICollection<NetworkCardToNetworkCard>? Parents { get; set; }

        public virtual ICollection<NetworkCardToNetworkCard>? Children { get; set; }
    }
}
