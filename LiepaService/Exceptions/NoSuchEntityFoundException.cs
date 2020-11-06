using System;
using System.Runtime.Serialization;

namespace LiepaService.Exceptions {
    public class NoSuchEntityFoundException : Exception
    {
        public int SearchedIndex {get; private set;}

        public NoSuchEntityFoundException(int searchedIndex)
        {
            SearchedIndex = searchedIndex;
        }

        public NoSuchEntityFoundException(int searchedIndex, string message) : base(message)
        {
            SearchedIndex = searchedIndex;
        }

        public NoSuchEntityFoundException(int searchedIndex, string message, Exception innerException) : base(message, innerException)
        {
            SearchedIndex = searchedIndex;
        }

        protected NoSuchEntityFoundException(int searchedIndex, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            SearchedIndex = searchedIndex;
        }
    }
}