using CsvHelper;
using CsvHelper.Configuration;
using System.Data;
using System.Globalization;
using System.IO;

namespace Otta.Utility
{
    public class ReadCsv : IReadCsv
    {
        public DataTable GetFileData(string path)
        {
            var dt = new DataTable();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            }))
            {
                using var dr = new CsvDataReader(csv);
                dt.Load(dr);
            }
            return dt;
        }
    }
}
