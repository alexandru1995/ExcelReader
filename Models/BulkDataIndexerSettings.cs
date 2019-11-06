using System;
using Nest;

namespace GenericExcelReader.Models
{
    public class BulkDataIndexerSettings
    {
        public Uri ServiceUri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Index { get; set; }
        public string ExcelPath { get; set; }
        public Time BackOffTime { get; set; } = "30s";
        public TimeSpan MaximumRunTime { get; set; } = TimeSpan.FromMinutes(30);
        public bool SkipHeader { get; set; } = true;
        public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;
    }
}
