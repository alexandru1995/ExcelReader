using Elasticsearch.Net;
using ExcelDataReader;
using GenericExcelReader.Models;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GenericExcelReader
{
    public class BulkDataIndexer<T> where T : class
    {
        static BulkDataIndexer()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private readonly IExcelRowReader<T> _excelRowReader;
        public BulkDataIndexer(IExcelRowReader<T> excelRowReader)
        {
            _excelRowReader = excelRowReader;
        }

        public void IndexData(BulkDataIndexerSettings indexerSettings)
        {

            var pool = new SingleNodeConnectionPool(indexerSettings.ServiceUri);

            var connectionConfiguration = new ConnectionSettings(pool, (builtin, settings) =>
                new JsonNetSerializer(
                    builtin, settings, () => new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    },
                    resolver =>
                    {
                        resolver.NamingStrategy = new DefaultNamingStrategy();
                    }));
            connectionConfiguration.DefaultDisableIdInference()
                .BasicAuthentication(indexerSettings.UserName, indexerSettings.Password)
                .EnableHttpCompression()
                .ServerCertificateValidationCallback((context, certificate, chain, errors) => true)
                .RequestTimeout(TimeSpan.FromMinutes(2));

            var client = new ElasticClient(connectionConfiguration);

            client.BulkAll(ReadRows(indexerSettings.ExcelPath,indexerSettings.SkipHeader), b => b
                    .Index(indexerSettings.Index)
                    .BackOffTime(indexerSettings.BackOffTime)
                    .BackOffRetries(2)
                    .RefreshOnCompleted()
                    .MaxDegreeOfParallelism(indexerSettings.MaxDegreeOfParallelism)
                    .Size(1000))
                .Wait(indexerSettings.MaximumRunTime, next =>
                {
                    Console.WriteLine($"Inserted page: {next.Page} {next.Items.Count}");
                });
        }

        private IEnumerable<T> ReadRows(string path, bool skipHeader)
        {
            using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            if (skipHeader)
            {
                reader.Read();
            }
            while (reader.Read())
            {
                yield return _excelRowReader.ReadRow(reader);
            }
        }
    }
}
