using System;

namespace Debugosaurus.BigUnits.Exceptions
{
    public class BigUnitsException : Exception
    {
        public BigUnitsException(
            string message,
            params (object key, object value)[] data) : base(message) 
        {
            foreach(var dataItem in data)
            {
                Data.Add(
                    dataItem.key,
                    dataItem.value);
            }
        }
    }
}