using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.NoSql.Models.Base
{
    public class NoSqlBaseIdEntityModel
    {
        public Guid Id { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? UpdatedByUserId { get; set; }
    }
}
