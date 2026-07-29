using FormulaFlow.Data.NoSql.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.NoSql.Models
{
    public class StockDataEntry : NoSqlBaseIdEntityModel
    {
        public Guid StockSymbolId { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
    }
}
