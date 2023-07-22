/* 
 * Code From
 * https://gist.github.com/Drarig29/4aa001074826f7da69b5bb73a83ccd39
 */

using System;
using System.Runtime.Serialization;

namespace BackgroundFix
{
    [Serializable]
    internal class NotBackedUpExeption : Exception
    {
        public NotBackedUpExeption()
        {
        }

        public NotBackedUpExeption(string message) : base(message)
        {
        }

        public NotBackedUpExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotBackedUpExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}