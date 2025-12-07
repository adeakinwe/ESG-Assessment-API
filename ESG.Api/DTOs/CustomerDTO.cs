namespace ESG.API.DTOs
{
    public class CustomerForCreationDTO
    {
        public int customerId { get; set; }
        public string customerCode { get; set; } = null!;
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string gender { get; set; } = null!;
        public int sector { get; set; }
        public string address { get; set; } = null!;
        public int createdBy { get; set; } = 1;
        public DateTime dateTimeCreated { get; set; } = DateTime.Now;
    }

    public class CustomerForReturnDTO
    {
        public string customerCode { get; set; } = null!;
        public string customerName { get; set; } = null!;
        public string gender { get; set; } = null!;
        public string sectorName { get; set; } = null!;
        public string address { get; set; } = null!;
    }
}