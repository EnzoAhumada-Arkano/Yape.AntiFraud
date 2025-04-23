namespace Domain.Models
{
    public class TransactionCreate
    {
        public Guid SourceAccountId { get; set; }
        public Guid DestinationAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public int Value { get; set; }
    }
}
