using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class FreezeOnMatchCustomization : ICustomization
    {
        private readonly Type targetType;
        private readonly string identifier;
        private readonly Matching matchBy;
        private readonly List<Type> matchingTypes;

        public FreezeOnMatchCustomization(
            Type targetType,
            string identifier = null,
            Matching matchBy = Matching.ExactType)
        {
            Require.IsNotNull(targetType, "targetType");

            this.targetType = targetType;
            this.identifier = identifier;
            this.matchBy = matchBy;
            this.matchingTypes = new List<Type>();
        }

        public Type TargetType
        {
            get { return this.targetType; }
        }

        public Matching MatchBy
        {
            get { return this.matchBy; }
        }

        public void Customize(IFixture fixture)
        {
            Require.IsNotNull(fixture, "fixture");

            var specimen = FreezeTargetType(fixture);
            var types = MatchSpecimenByType(specimen);
            var builder = new CompositeSpecimenBuilder(types);

            fixture.Customizations.Insert(0, builder);
        }

        private ISpecimenBuilder FreezeTargetType(IFixture fixture)
        {
            var specimen = fixture.Create(this.targetType);
            return new FixedBuilder(specimen);
        }

        private IEnumerable<ISpecimenBuilder> MatchSpecimenByType(ISpecimenBuilder specimen)
        {
            MatchByExactType();
            MatchByBaseType();
            MatchByImplementedInterfaces();
            return MapSpecimenToTypeFilters(specimen);
        }

        private void MatchByExactType()
        {
            if (this.matchBy.HasFlag(Matching.ExactType))
            {
                matchingTypes.Add(this.targetType);
            }
        }

        private void MatchByBaseType()
        {
            if (this.matchBy.HasFlag(Matching.BaseType))
            {
                matchingTypes.Add(this.targetType.BaseType ?? typeof(object));
            }
        }

        private void MatchByImplementedInterfaces()
        {
            if (this.matchBy.HasFlag(Matching.ImplementedInterfaces))
            {
                matchingTypes.AddRange(this.targetType.GetInterfaces());
            }
        }

        private IEnumerable<FilteringSpecimenBuilder> MapSpecimenToTypeFilters(ISpecimenBuilder specimen)
        {
            return from type in matchingTypes
                   select SpecimenBuilderNodeFactory.CreateTypedNode(
                       type,
                       specimen);
        }
    }
}
