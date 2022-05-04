using FluentAssertions;
using FluentValidation.Results;
using Ordering.Application.Exceptions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Ordering.Application.UnitTests.Exceptions
{
    public class ValidationExceptionTest
    {
        [Fact]
        public void DefaultConstructorCreatesAnEmptyErrorDictionary()
        {
            var actual = new ValidationException().Errors;

            actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
        }

        [Fact]
        public void SingleValidationFailureCreatesASingleElementErrorDictionary()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("TotalPrice", "Total price cannot be less than zero")
            };

            var actual = new ValidationException(failures).Errors;

            actual.Keys.Should().BeEquivalentTo(new string[] { "TotalPrice" });
            actual["TotalPrice"].Should().BeEquivalentTo(new string[] { "Total price cannot be less than zero" });
        }

        [Fact]
        public void MulitpleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("TotalPrice", "Total price cannot be less than zero"),
                new ValidationFailure("TotalPrice", "Total price must be greater than zero"),
                new ValidationFailure("UserName", "UserName cannot be empty")
            };

            var actual = new ValidationException(failures).Errors;

            actual.Keys.Should().BeEquivalentTo(new string[] { "TotalPrice", "UserName" });

            actual["TotalPrice"].Should().BeEquivalentTo(new string[]
            {
                "Total price cannot be less than zero",
                "Total price must be greater than zero"
            });

            actual["UserName"].Should().BeEquivalentTo(new string[]
            {
                "UserName cannot be empty"
            });
        }
    }
}
