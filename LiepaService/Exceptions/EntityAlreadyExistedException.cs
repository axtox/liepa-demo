using System;
using System.Runtime.Serialization;

namespace LiepaService.Exceptions {
    public class EntityAlreadyExistedException : Exception {
        
        public int SearchedIndex {get; private set;}

        public EntityAlreadyExistedException(int searchedIndex)
        {
            SearchedIndex = searchedIndex;
        }

        public EntityAlreadyExistedException(int searchedIndex, string message) : base(message)
        {
            SearchedIndex = searchedIndex;
        }

        public EntityAlreadyExistedException(int searchedIndex, string message, Exception innerException) : base(message, innerException)
        {
            SearchedIndex = searchedIndex;
        }

        protected EntityAlreadyExistedException(int searchedIndex, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            SearchedIndex = searchedIndex;
        }
    }
}