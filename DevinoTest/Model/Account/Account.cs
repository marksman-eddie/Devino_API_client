using System;
namespace DevinoTest.Model.Account
{
    public class Account
    {
        public int companyId { get; set; }
        public string accountType { get; set; }
        public double balance { get; set; }
        public double credit { get; set; }
        public double reserveSms { get; set; }
        public double reserveViber { get; set; }
        public double reserve { get; set; }
        public double currencyId { get; set; }
        public bool isBlocked { get; set; }
        public double disableThreshold { get; set; }
        public double notifyThreshold { get; set; }
    }
}
