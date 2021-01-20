using System.Collections.Generic;
using System.Data;

namespace DotNetToolBox.Extensions
{
    public static class DataTableExtensions
    {

        /// <summary>
        /// Checks a <see cref="DataTable"/> to determine whether or not it contains
        /// at least 1 <see cref="DataRow"/>.
        /// </summary>
        /// <param name="table">The input <see cref="DataTable"/> object.</param>
        /// <returns><code>true</code> if the <see cref="DataTable"/> <see cref="table"/> contains at 
        /// least 1 <see cref="DataRow"/>. <code>false</code> otherwise.</returns>
        public static bool HasRows(this DataTable table)
        {
            if (table == null) return false;
            if (table.Rows.Count == 0) return false;
            return true;
        }

        /// <summary>
        /// Creates and returns a new <see cref="IList&lt;T&gt;"/> with the <see cref="DataRow"/> items found
        /// in the <see cref="table"/> <see cref="DataTable"/>.
        /// </summary>
        /// <param name="table">The input <see cref="DataTable"/> object.</param>
        public static IList<DataRow> Rows(this DataTable table)
        {
            if (table == null) return null;
            if (table.Rows.Count == 0) return new List<DataRow>();
            var dataRows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
            {
                dataRows.Add(row);
            }
            return dataRows;
        }

        /// <summary>
        /// Returns the first <see cref="DataRow"/> of the <see cref="DataTable"/>.
        /// </summary>
        /// <param name="source">The input <see cref="DataTable"/> object.</param>
        public static DataRow FirstOrDefaultRow(this DataTable source)
        {
            if (source == null) return null;
            if (source.Rows.Count == 0) return null;
            return source.Rows[0];
        }

    }
}
