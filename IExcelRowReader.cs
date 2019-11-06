using ExcelDataReader;

namespace GenericExcelReader
{
    public interface IExcelRowReader<out T>
    {
        T ReadRow(IExcelDataReader reader);
    }
}
