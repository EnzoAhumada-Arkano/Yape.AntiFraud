namespace Domain;

public static class Constants
{
	public enum TranstactionStatus
	{
        pending = 0,
        approved = 1,
        rejected = 2,
    }

    public struct TrasanctionValidateStatus
    {
        public const string TransactionValid = "Transaction is valid";
        public const string TransactionInvalid = "Transaction is invalid";
    }
}
