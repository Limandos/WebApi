using System;

namespace WebApi.Exceptions
{
    public class NotFoundException : ArgumentException
    {
        public string Value { get; set; }

        public NotFoundException(string message, string value) : base(message)
        {
            Value = value;
        }
    }
}