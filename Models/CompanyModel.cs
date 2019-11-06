using System;
using System.Collections.Generic;

namespace GenericExcelReader.Models
{
    public class CompanyModel
    {
        public string ID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Name { get; set; }
        public string LegalForm { get; set; }
        //TODO rename this property
        //public AddressModel ComplexAddress { get; set; }
        public string Address { get; set; }
        public IList<string> Administrators { get; set; }
        public IList<string> Founders { get; set; }
        public IList<ShareModel> Shares { get; set; }
        public IList<int> LicensedActivities { get; set; }
        public IList<int> UnlicensedActivities { get; set; }
        public bool Liquidated { get; set; }
    }
}
