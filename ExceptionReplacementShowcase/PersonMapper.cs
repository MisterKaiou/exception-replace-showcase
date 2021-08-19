using System;
using ExceptionReplacementShowcase.Core;
using ExceptionReplacementShowcase.ResultRelated;

namespace ExceptionReplacementShowcase
{
    public static class PersonMapper
    {
        public static Person MapToPersonWithoutResult(this PersonDTO dto)
        {
            try
            {
                var firstname = new Name(dto.Firstname);
                var lastname = new Name(dto.Lastname);
                var age = new Age(dto.Age);

                return new Person(firstname, lastname, age);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }

        public static Result<Person> MapToPersonWithResult(this PersonDTO dto)
        {
            try
            {
                var firstname = new Name(dto.Firstname);
                var lastname = new Name(dto.Lastname);
                var age = new Age(dto.Age);

                return Result.Ok(new Person(firstname, lastname, age));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Fail<Person>($"{e.Message}");
            }
        }
    }
}
