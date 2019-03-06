using System;
using System.Collections.Generic;

namespace technical.API.Models.Yodlee
{
    public class AccountData
    {
        public BalanceData AvailableCash { get; set; }

        public string IncludeInNetWorth { get; set; }

        public string AccountName { get; set; }

        public BalanceData CurrentBalance { get; set; }

        public string AccountType { get; set; }

        public string IsManual { get; set; }

        public string DisplayedName { get; set; }

        public string AccountNumber { get; set; }

        public BalanceData AvailableBalance { get; set; }

        public string AccountStatus { get; set; }

        public string LastUpdated { get; set; }

        public string IsAsset { get; set; }

        public string CreatedDate { get; set; }   // DateTime would work as well

        public BalanceData Balance { get; set; }

        public string AggregationSource { get; set; }

        public string ProviderId { get; set; }

        public int ProviderAccountId { get; set; }

        public string CONTAINER { get; set; }

        public int id { get; set; }

        public string UserClassification { get; set; }

        public List<DatasetDataAcc> Dataset { get; set; }

        public string ProviderName { get; set; }
    }
}