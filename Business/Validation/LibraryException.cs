using System;

namespace Business.Validation
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Ожидание>")]
    public class LibraryException : Exception
    {
        public LibraryException(string message)
        : base(message)
        { }
    }
}
