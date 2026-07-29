using FormulaFlow.Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.Models
{
    public class StockSymbol : BaseIdEntityModel
    {
        public string Symbol { get; set; }
    }
}
