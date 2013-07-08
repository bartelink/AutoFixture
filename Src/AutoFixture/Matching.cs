using System;

namespace Ploeh.AutoFixture
{
    [Flags]
    public enum Matching
    {
        ExactType = 0,
        BaseType = 1,
        ImplementedInterfaces = 2,
        PropertyName = 4
    }
}
