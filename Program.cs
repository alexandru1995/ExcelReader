using GenericExcelReader.Models;
using System;

namespace GenericExcelReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new BulkDataIndexer<CompanyModel>(new CompaniesReader());
            
            test.IndexData(new BulkDataIndexerSettings
            {
                Password = "b86c137aa62f5a639a664d947fa77e02",
                Index = "open-data-companies-3/_doc",
                ExcelPath = @"E:\OpenData\ExcelProcessor2\Excel\Company_21.10.2019.xlsx",
                ServiceUri = new Uri("https://test-logs.gov.md"),
                UserName = "opendata"
            });

        }
    }
}
