using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class FreezeOnMatchCustomization : ICustomization
    {
        private readonly Type targetType;
        private readonly Matching matchBy;

        public FreezeOnMatchCustomization(Type targetType, Matching matchBy = Matching.ExactType)
        {
            Require.IsNotNull(targetType, "targetType");

            this.targetType = targetType;
            this.matchBy = matchBy;
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

            this.MapFixedSpecimenToTargetType(fixture);
        }

        private void MapFixedSpecimenToTargetType(IFixture fixture)
        {
            var specimen = fixture.Create(this.targetType);
            fixture.Customizations.Insert(0, new FixedBuilder(specimen));
        }
    }
}
