namespace DataAccess.ExtendedTypes
{
    public class CreditReport
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public double InterestRate { get; set; }
        public double PercGainPerMonth { get; set; }
        public double InterestPerMonth { get; set; }
        public int DaysTaken { get; set; }
    }
}
