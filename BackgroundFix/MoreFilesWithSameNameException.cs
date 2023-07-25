using System;
using System.Runtime.Serialization;

namespace BackgroundFix
{
    [Serializable]
    internal class MoreFilesWithSameNameException : Exception
    {
        private string[] filesFound;
            public string[] FilesFound {
            get { return filesFound; }
            }

        public MoreFilesWithSameNameException()
        {
        }

        public MoreFilesWithSameNameException(string message) : base(message)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="helper">Files found</param>
        public MoreFilesWithSameNameException(string message, string[] helper) : base(message)
        {
            this.filesFound = helper;
        }

        public MoreFilesWithSameNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MoreFilesWithSameNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}