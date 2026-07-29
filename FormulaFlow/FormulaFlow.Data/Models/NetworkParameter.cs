using FormulaFlow.Data.Enum;
using FormulaFlow.Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.Models
{
    public class NetworkParameter : BaseIdEntityModel
    {
        public int Order { get; set; }
        public NetworkParameterType Type { get; set; }
        public required string Value { get; set; }

        public Guid NetworkCardId { get; set; }
        public virtual NetworkCard NetworkCard { get; set; }

    }
}
