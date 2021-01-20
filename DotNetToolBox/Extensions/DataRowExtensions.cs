using System;
using System.Data;
using JetBrains.Annotations;

namespace DotNetToolBox.Extensions
{
    public static class DataRowExtensions
    {
        public static T TryGetValue<T>(this DataRow row, [NotNull] string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            try
            {
                if (!row.Table.Columns.Contains(columnName))
                {
                    return default;
                }

                var value = row[columnName];
                if (Convert.IsDBNull(value))
                {
                    return default;
                }

                return (T) value;
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static T GetValue<T>(this DataRow row, [NotNull] string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (!row.Table.Columns.Contains(columnName)) throw new ArgumentOutOfRangeException(nameof(columnName));

            try
            {
                var value = row[columnName];
                if (Convert.IsDBNull(value)) return default;
                return (T) value;
            }
            catch (InvalidCastException)
            {
                throw;
            }
            catch (Exception)
            {
                return default;
            }
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
