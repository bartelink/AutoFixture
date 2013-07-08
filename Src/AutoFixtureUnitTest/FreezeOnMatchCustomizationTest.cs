using System;
using System.Collections;
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
            var matcher = Matching.BaseType;
            // Exercise system
            var sut = new FreezeOnMatchCustomization(targetType, matchBy: matcher);
            // Verify outcome
            Assert.Equal(targetType, sut.TargetType);
            Assert.Equal(matcher, sut.MatchBy);
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
        [InlineData(typeof(object), typeof(object), true)]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(int), typeof(int), true)]
        [InlineData(typeof(object), typeof(bool), false)]
        [InlineData(typeof(int), typeof(string), false)]
        public void FreezeByMatchingExactTypeShouldReturnTheRightSpecimen(
            Type frozenType,
            Type requestedType,
            bool areSameSpecimen)
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                matchBy: Matching.ExactType);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = context.Resolve(frozenType);
            var requested = context.Resolve(requestedType);
            Assert.Equal(
                object.ReferenceEquals(frozen, requested),
                areSameSpecimen);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(ConcreteType), typeof(ConcreteType), true)]
        [InlineData(typeof(ConcreteType), typeof(AbstractType), true)]
        [InlineData(typeof(string), typeof(object), true)]
        [InlineData(typeof(ConcreteType), typeof(object), false)]
        [InlineData(typeof(int), typeof(string), false)]
        public void FreezeByMatchingBaseTypeShouldReturnTheRightSpecimen(
            Type frozenType,
            Type requestedType,
            bool areSameSpecimen)
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                matchBy: Matching.BaseType);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = context.Resolve(frozenType);
            var requested = context.Resolve(requestedType);
            Assert.Equal(
                object.ReferenceEquals(frozen, requested),
                areSameSpecimen);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(ArrayList), typeof(ArrayList), true)]
        [InlineData(typeof(ArrayList), typeof(IEnumerable), true)]
        [InlineData(typeof(ArrayList), typeof(IList), true)]
        [InlineData(typeof(ArrayList), typeof(ICollection), true)]
        public void FreezeByMatchingImplementedInterfacesShouldReturnTheRightSpecimen(
            Type frozenType,
            Type requestedType,
            bool areSameSpecimen)
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                matchBy: Matching.ImplementedInterfaces);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = context.Resolve(frozenType);
            var requested = context.Resolve(requestedType);
            Assert.Equal(
                object.ReferenceEquals(frozen, requested),
                areSameSpecimen);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnSameSpecimenForPropertyOfSameType()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var propertyName = "Property";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                propertyName,
                Matching.PropertyName);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<ConcreteType>();
            var requested = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnSameSpecimenForPropertyOfAssignableType()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var propertyName = "Property";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                propertyName,
                Matching.PropertyName);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<ConcreteType>();
            var requested = fixture.Create<PropertyHolder<AbstractType>>().Property;
            Assert.Same(frozen, requested);
            // Teardown
        }
    }
}
