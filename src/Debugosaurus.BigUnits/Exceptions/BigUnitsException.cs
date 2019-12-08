using System;

namespace Debugosaurus.BigUnits.Exceptions
{
    public sealed class BigUnitsException : Exception
    {
        public BigUnitsException(
            string message,
            params (object key, object value)[] data) : base(message)
        {
            foreach (var (key, value) in data)
            {
                Data.Add(
                    key,
                    value);
            }
        }
    }
}