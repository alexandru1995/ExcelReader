using ExcelDataReader;
using GenericExcelReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericExcelReader
{
    public class CompaniesReader : IExcelRowReader<CompanyModel>
    {
        public CompanyModel ReadRow(IExcelDataReader reader)
        {
            return new CompanyModel
            {
                ID = string.IsNullOrEmpty(reader.GetString(0)) ? null : reader.GetString(0).Trim(),
                RegistrationDate = reader.GetDateTime(1),
                Name = string.IsNullOrEmpty(reader.GetString(2)) ? null : reader.GetString(2).Trim(),
                LegalForm = string.IsNullOrEmpty(reader.GetString(3)) ? null : reader.GetString(3).Trim(),
                Address = string.IsNullOrEmpty(reader.GetString(4)) ? null : reader.GetString(4).Trim(),
                Administrators = string.IsNullOrEmpty(reader.GetString(5)) ? null : reader.GetString(5).Split(",").Select(s => s.Trim()).ToList(),
                Founders = (reader.GetValue(6) == null) ? null : GetShares(reader.GetValue(6).ToString()).Select(f => f.Founder).ToList(),
                Shares = (reader.GetValue(6) == null) ? null : GetShares(reader.GetValue(6).ToString()),
                UnlicensedActivities = string.IsNullOrEmpty(reader.GetString(7)) ? null : reader.GetString(7).Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Where(l => l.Trim().All(c => char.IsDigit(c))).Select(l => int.Parse(l)).ToList(),
                LicensedActivities = string.IsNullOrEmpty(reader.GetString(8)) ? null : reader.GetString(8).Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Where(l => l.Trim().All(c => char.IsDigit(c))).Select(l => int.Parse(l)).ToList(),
                Liquidated = (reader.GetString(9) != null)
            };
        }
        public List<ShareModel> GetShares(string stringFounders)
        {
            var founders = new List<ShareModel>();
            foreach (var founder in stringFounders.Split(", "))
            {
                founders.Add(new ShareModel
                {
                    Founder = founder.Split("(").First().Trim(),
                    Share = founder.Contains('(') ?
                        (founder.Split("(").Last().Contains("%") ? (float?)float.Parse(founder.Split("(").Last().Trim().Trim('%', ')').Replace(",", ".")) : null) : null
                });
            }
            return founders;
        }
    }
}
