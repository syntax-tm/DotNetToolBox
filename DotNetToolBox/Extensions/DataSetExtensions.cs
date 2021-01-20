using System.Collections.Generic;
using System.Data;

namespace DotNetToolBox.Extensions
{
    public static class DataSetExtensions
    {
        /// <summary>
        /// Checks a <see cref="DataSet"/> and returns true if any rows are found in
        /// any <see cref="DataTable"/> in the <see cref="DataTableCollection"/>.
        /// </summary>
        /// <param name="source">The input <see cref="DataSet"/> object.</param>
        /// <returns><see langword="true"/> if any <see cref="DataTable"/> contains at least 1 <see cref="DataRow"/>. <see langword="false"/> if it is empty.</returns>
        public static bool HasRows(this DataSet source)
        {
            if (source == null) return false;
            if (source.Tables.Count == 0) return false;
            foreach (DataTable table in source.Tables)
            {
                if (table.HasRows())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks a <see cref="DataSet"/> and returns true if no rows are found in
        /// any <see cref="DataTable"/> in the <see cref="DataTableCollection"/>.
        /// </summary>
        /// <param name="source">The input <see cref="DataSet"/> object.</param>
        /// <returns><see langword="false"/> if any <see cref="DataTable"/> contains at least 1 <see cref="DataRow"/>. <see langword="true"/> if it is empty.</returns>
        public static bool IsEmpty(this DataSet source)
        {
            if (source == null) return true;
            if (source.Tables.Count == 0) return true;
            foreach (DataTable table in source.Tables)
            {
                if (table.HasRows())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Creates and returns a new <see cref="IList&lt;T&gt;"/> with all of the <see cref="DataRow"/> items found
        /// in the <see cref="source"/> <see cref="DataSet"/>'s <see cref="DataTableCollection"/>.
        /// <see cref="DataTableCollection"/> of <see cref="DataTable"/>.
        /// </summary>
        /// <param name="source">The input <see cref="DataSet"/> object.</param>
        public static IList<DataRow> Rows(this DataSet source)
        {
            if (source == null) return null;
            if (source.Tables.Count == 0) return new List<DataRow>();
            var dataRows = new List<DataRow>();
            foreach (DataTable table in source.Tables)
            {
                if (table.HasRows())
                {
                    dataRows.AddRange(table.Rows());
                }
            }
            return dataRows;
        }

        /// <summary>
        /// Returns the first <see cref="DataRow"/> of the first <see cref="DataTable"/> in the <param name="source"/>
        /// <see cref="DataSet"/>.
        /// </summary>
        /// <param name="source">The input <see cref="DataSet"/> object.</param>
        public static DataRow FirstOrDefaultRow(this DataSet source)
        {
            if (source == null) return null;
            if (source.Tables.Count == 0) return null;
            var firstTable = source.Tables[0];
            if (firstTable.Rows.Count == 0) return null;
            return firstTable.Rows[0];
        }
    }
}
