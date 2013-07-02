using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class FreezeOnMatchCustomizationTest
    {
        [Fact]
        public void SutShouldBeCustomization()
        {
            // Fixture setup
            var dummyType = typeof(object);
            // Exercise system
            var sut = new FreezeOnMatchCustomization(dummyType);
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithTargetTypeShouldSetCorrespondingProperty()
        {
            // Fixture setup
            var targetType = typeof(object);
            // Exercise system
            var sut = new FreezeOnMatchCustomization(targetType);
            // Verify outcome
            Assert.Equal(targetType, sut.TargetType);
        }

        [Fact]
        public void InitializeWithTargetTypeShouldSetMatchByPropertyToExactType()
        {
            // Fixture setup
            // Exercise system
            var sut = new FreezeOnMatchCustomization(typeof(string));
            // Verify outcome
            Assert.Equal(Matching.ExactType, sut.MatchBy);
        }

        [Fact]
        public void InitializeWithTargetTypeAndMatchByShouldSetCorrespondingProperties()
        {
            // Fixture setup
            var targetType = typeof(object);
            var matchBy = Matching.BaseType;
            // Exercise system
            var sut = new FreezeOnMatchCustomization(targetType, matchBy);
            // Verify outcome
            Assert.Equal(targetType, sut.TargetType);
            Assert.Equal(matchBy, sut.MatchBy);
        }

        [Fact]
        public void InitializeWithNullTargetTypeShouldThrowArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FreezeOnMatchCustomization(null));
            // Teardown
        }

        [Fact]
        public void CustomizeWithNullFixtureShouldThrowArgumentNullException()
        {
            // Fixture setup
            var sut = new FreezeOnMatchCustomization(typeof(object));
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(bool))]
        public void FreezeByMatchingExactTypeShouldReturnSameSpecimenForThatType(Type targetType)
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new FreezeOnMatchCustomization(targetType, Matching.ExactType);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.Equal(context.Resolve(targetType), context.Resolve(targetType));
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingBaseTypeShouldReturnSameSpecimenForBaseType()
        {
            // Fixture setup
            var targetType = typeof(ConcreteType);
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(targetType, Matching.ExactType);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var baseSpecimen = fixture.Create<AbstractType>();
            var specimen = fixture.Create<ConcreteType>();
            Assert.Equal(baseSpecimen, specimen);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingBaseTypeShouldReturnSameSpecimenForAncestorType()
        {
            // Fixture setup
            var targetType = typeof(ConcreteType);
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(targetType, Matching.ExactType);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var ancestorSpecimen = fixture.Create<object>();
            var specimen = fixture.Create<ConcreteType>();
            Assert.Equal(ancestorSpecimen, specimen);
            // Teardown
        }
    }
}
