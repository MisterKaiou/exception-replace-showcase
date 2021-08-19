using System;
using System.Linq;

namespace ExceptionReplacementShowcase.Core
{
    public record Person(Name Firstname, Name LastName, Age Age);

    public readonly struct Name
    {
        private readonly string _value;

        public static implicit operator string(Name n) => n._value;

        public override string ToString() => this;

        public Name(string value) => _value = value.Any(char.IsDigit)
            ? throw new ArgumentException("A name cannot contain numbers")
            : value;
    }

    public readonly struct Age
    {
        private readonly int _value;

        public static implicit operator int(Age a) => a._value;

        public override string ToString() => _value.ToString();

        public Age(int value) => _value = value > 123 //Current world record of oldest person alive, as of 2021
            ? throw new ArgumentException("We have a new world record for oldest person alive!")
            : value;
    }
}
