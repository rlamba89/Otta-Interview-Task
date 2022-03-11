using System.Data;

namespace Otta.Utility
{
    public interface IReadCsv
    {
        DataTable GetFileData(string fileName);
    }
}