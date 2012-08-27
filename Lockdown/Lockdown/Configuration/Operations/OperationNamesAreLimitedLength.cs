namespace Lockdown.Configuration.Operations
{
    public class OperationNamesAreLimitedLength : IConstrainOperationIdentifiers
    {
        private readonly int _maxLength;

        public OperationNamesAreLimitedLength(int maxLength)
        {
            _maxLength = maxLength;
        }

        public void Check(string name)
        {
            if (name.Length > _maxLength)
            {
                throw new AzmanConstraintViolation("Operation names are limited to 64 chars");
            }
        }
    }
}