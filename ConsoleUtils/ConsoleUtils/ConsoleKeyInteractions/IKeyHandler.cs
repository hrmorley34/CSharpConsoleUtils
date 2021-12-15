using System;

namespace ConsoleUtils.ConsoleKeyInteractions
{
    [System.Serializable]
    public class NoValueException : Exception
    {
        public NoValueException() { }
        public NoValueException(string message) : base(message) { }
        public NoValueException(string message, System.Exception inner) : base(message, inner) { }
        protected NoValueException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class FinishedException : Exception
    {
        public FinishedException() { }
        public FinishedException(string message) : base(message) { }
        public FinishedException(string message, System.Exception inner) : base(message, inner) { }
        protected FinishedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public interface IKeyHandler<T>
    {
        /// <summary>Handle a key</summary>
        /// <returns><c>Finished()</c></returns>
        /// <exception cref="FinishedException">If no more keys are accepted</exception>
        bool HandleKey(ConsoleKeyInfo key);

        /// <summary>Handle a key</summary>
        /// <returns><c>Finished()</c></returns>
        /// <exception cref="FinishedException">If no more keys are accepted</exception>
        bool HandleKey(char c);

        /// <summary>Whether this can take any more keys</summary>
        bool Finished();

        /// <summary>Get the return value, if finished</summary>
        /// <exception cref="NoValueException">If no value exists yet</exception>
        T GetReturnValue();

        T ReadKeys() => ReadKeysMethod.ReadKeys<T>(this);
    }

    public static class ReadKeysMethod
    {
        /// <summary>Read from the console until a value is recieved</summary>
        /// <returns>The value (<c>GetReturnValue()</c>)</returns>
        public static T ReadKeys<T>(IKeyHandler<T> keyHandler)
        {
            Action GetAndHandle;
            if (Console.IsInputRedirected)
            {
                Func<char> read = () =>
                {
                    int c = Console.Read();
                    if (c == -1) throw new Exception();
                    return (char)c;
                };
                GetAndHandle = () => keyHandler.HandleKey(read());
            }
            else
            {
                GetAndHandle = () => keyHandler.HandleKey(Console.ReadKey(true));
            }
            while (!keyHandler.Finished()) GetAndHandle();
            return keyHandler.GetReturnValue();
        }
    }
}