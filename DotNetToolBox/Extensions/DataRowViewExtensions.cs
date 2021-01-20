using System;
using System.Data;
using JetBrains.Annotations;

namespace DotNetToolBox.Extensions
{
    public static class DataRowViewExtensions
    {
        public static T TryGetValue<T>(this DataRowView row, [NotNull] string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            return row.Row.TryGetValue<T>(columnName);
        }

        public static T GetValue<T>(this DataRowView row, [NotNull] string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            return row.Row.TryGetValue<T>(columnName);
        }

        public static decimal GetDecimal(this DataRowView row, [NotNull] string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            return row.Row.TryGetValue<decimal>(columnName);
        }

        public static int GetInt(this DataRowView row, [NotNull] string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            return row.Row.TryGetValue<int>(columnName);
        }

        public static string GetString(this DataRowView row, [NotNull] string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            return row.Row.TryGetValue<string>(columnName);
        }

        public static byte GetByte(this DataRowView row, [NotNull] string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            return row.Row.TryGetValue<byte>(columnName);
        }

        public static bool GetBool(this DataRowView row, [NotNull] string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            return row.Row.TryGetValue<bool>(columnName);
        }
    }
}
