using System;
namespace musicMash.Exceptions
{
    public class ArtistDoesNotExistException : Exception
    {
        public ArtistDoesNotExistException()
        {
        }

        public ArtistDoesNotExistException(string message) : base(message)
        {
        }
    }
}
