using FormulaFlow.Data.NoSql.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaFlow.Data.NoSql
{
    public class NoSqlFormulaFlowContext
    {
        public LiteDatabase Database { get; private set; }
        private readonly Dictionary<Type, object> _collections = new();

        public ILiteCollection<StockDataEntry> StockDataEntries { get; private set; }


        public NoSqlFormulaFlowContext(string connectionString)
        {
            Database = new LiteDatabase(connectionString);

            StockDataEntries = Database.GetCollection<StockDataEntry>();

            StockDataEntries.EnsureIndex("StockDataIdIndex", stockDataEntry => stockDataEntry.Id, unique: true);
            StockDataEntries.EnsureIndex("StockDataCompIndex", stockDataEntry => new { stockDataEntry.StockSymbolId, stockDataEntry.Date }, unique: true);

            // Store it so GetCollection<T>() returns the same instance
            _collections[typeof(StockDataEntry)] = StockDataEntries;

        }

        public ILiteCollection<T> GetCollection<T>()
        {
            if (_collections.TryGetValue(typeof(T), out var col))
                return (ILiteCollection<T>)col;

            // First time this type is requested — create and cache it
            var newCol = Database.GetCollection<T>();
            _collections[typeof(T)] = newCol;
            return newCol;
        }
    }
}
