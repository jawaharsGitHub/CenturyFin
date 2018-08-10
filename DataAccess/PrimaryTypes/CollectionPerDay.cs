namespace DataAccess.PrimaryTypes
{
    public class CollectionPerDay : BaseClass<CollectionPerDay>
    {

        public string Date { get; set; }

        public int ActualCollection { get; set; }

        public int ExpectedCollection { get; set; }
    }
}
