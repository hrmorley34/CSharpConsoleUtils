using System;

namespace ConsoleUtils.ConsoleImagery.Exceptions
{
    [Serializable]
    public class MismatchedHeightException : ArgumentException
    {
        public const string MismatchedHeightText = "The heights of the images do not match.";

        public MismatchedHeightException() : base(MismatchedHeightText) { }
        protected MismatchedHeightException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}