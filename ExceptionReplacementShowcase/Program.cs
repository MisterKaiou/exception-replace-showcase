using System;
using System.Collections.Generic;
using System.Linq;
using ExceptionReplacementShowcase.Core;
using ExceptionReplacementShowcase.ResultRelated;
using FluentAssertions;
using NUnit.Framework;

namespace ExceptionReplacementShowcase
{
    public class ExceptionReplacementShowcase
    {
        [Test]
        public void MappingWithValidDTO_ResultShouldBeTrue()
        {
            var dto = new PersonDTO("Dude", "Guy", 25);

            var person = dto.MapToPersonWithoutResult();
            var personResult = dto.MapToPersonWithResult();

            person.Should().NotBeNull();

            personResult.Should().NotBeNull();
            personResult.Success.Should().BeTrue();
        }

        [Test]
        public void MappingWithInvalidName_ResultShouldBeFailure()
        {
            var dto = new PersonDTO("Dud3", "Guy", 25); //Invalid firstname

            var person = dto.MapToPersonWithoutResult();
            var personResult = dto.MapToPersonWithResult();

            person.Should().BeNull();

            personResult.Should().NotBeNull();
            personResult.Failure.Should().BeTrue();
        }

        [Test]
        public void MappingWithInvalidOnlyList_ResultShouldContainOnlyErrors()
        {
            var dto = new List<PersonDTO>
            {
                new("Dud3", "Guy", 25), //Invalid firstname
                new("Dude", "6uy", 25), //Invalid lastname
                new("Dude", "Guy", 130) //invalid age
            };

            var people = dto.Select(d => d.MapToPersonWithoutResult()).ToList();
            var peopleResultList = dto.Select(d => d.MapToPersonWithResult())
                .ToArray()
                .Then(rs => new PersonBatchMappingResult(rs));

            people.Should().OnlyContain(p => p == null);

            peopleResultList.Should().NotContainNulls();
            peopleResultList.Successes.Should().BeEmpty();
            peopleResultList.Errors.Should().Match(l => l.All(r => r.Failure));
            peopleResultList.Failure.Should().BeTrue();
            peopleResultList.Errors.Should().BeEquivalentTo(peopleResultList.GetValue());
        }

        [Test]
        public void MatchSuccessfulResult_BodyShouldBeEqualToResultValue()
        {
            var dto = new PersonDTO("Dude", "Guy", 25);

            var result = dto.MapToPersonWithResult();

            var match = result.Match(
                WannabeController.Ok,
                WannabeController.BadRequest
            );

            match.Body.Should().NotBeNull();
            match.Body.Should().Be(result.GetValue());
        }

        [Test]
        public void MatchUnsuccessfulResult_BodyShouldBeEqualToErrorResult()
        {
            var dto = new PersonDTO("Dud3", "Guy", 25);
            var expected = new ErrorResult<Person>("A name cannot contain numbers");

            var result = dto.MapToPersonWithResult();
            var match = result.Match(
                WannabeController.Ok,
                WannabeController.BadRequest
            );

            match.Body.Should().Be(expected);
        }

        [Test]
        public void MapSuccessfulResult_FinalResultShouldContainAllModifications()
        {
            var dto = new PersonDTO("Dude", "Guy", 25);
            var expected = new Person(new Name("Sir"), new Name("Guy"), new Age(35));

            var result = dto.MapToPersonWithResult();
            var map = result
                .Map(p => p with { Firstname = expected.Firstname })
                .Map(p => p with { Age = expected.Age });

            map.Success.Should().BeTrue();
            map.GetValue().Should().Be(expected);
        }

        [Test]
        public void MapToUnsuccessfulResult_FinalResultShouldBeErrorResult()
        {
            var dto = new PersonDTO("Dude", "Guy", 130);
            var expected = new ErrorResult<Person>("We have a new world record for oldest person alive!");

            var result = dto.MapToPersonWithResult();
            var map = result
                .Map(p => p with { Firstname = new Name("Other") });

            map.Failure.Should().BeTrue();
            Assert.Throws<InvalidOperationException>(() => map.GetValue());
            map.Should().Be(expected);
        }

        [Test]
        public void MapWithMatch_ShouldProduceBodyWithAllModifications()
        {
            var dto = new PersonDTO("Dude", "Guy", 25);
            var expect = new Person(new Name("Man"), new Name(dto.Lastname), new Age(dto.Age));

            var result = dto.MapToPersonWithResult();
            var match = result
                .Map(p => p with { Firstname = expect.Firstname })
                .Match(
                    WannabeController.Ok,
                    WannabeController.BadRequest
                );

            match.Body.Should().Be(expect);
        }
    }
}
