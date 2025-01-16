﻿using System;
using FluentAssertions.Equivalency;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FluentAssertions.Json.Specs
{
    [Collection("AssertionOptionsSpecs")]
    public class JsonAssertionOptionsSpecs
    {
        [Fact]
        public void Local_equivalency_options_are_applied_on_top_of_global_equivalency_options()
        {
            using var assertionOptions = new TempDefaultAssertionOptions(e => e
                       .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.1))
                       .WhenTypeIs<double>());

            // Arrange
            var actual = JToken.Parse("{ \"id\": 1.1232 }");
            var expected = JToken.Parse("{ \"id\": 1.1235 }");

            // Act & Assert
            actual.Should().BeEquivalentTo(expected, options => options);
        }

        private sealed class TempDefaultAssertionOptions : IDisposable
        {
            public TempDefaultAssertionOptions(Func<EquivalencyOptions, EquivalencyOptions> config)
            {
                AssertionConfiguration.Current.Equivalency.Modify(config);
            }

            public void Dispose()
            {
                AssertionConfiguration.Current.Equivalency.Modify(_ => new());
            }
        }
    }

    // Due to tests that call AssertionOptions
    [CollectionDefinition("AssertionOptionsSpecs", DisableParallelization = true)]
    public class AssertionOptionsSpecsDefinition { }
}
