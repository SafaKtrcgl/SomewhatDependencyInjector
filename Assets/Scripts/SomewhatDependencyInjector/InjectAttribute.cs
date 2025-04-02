using System;

namespace SomewhatDependencyInjector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectAttribute : Attribute
    {
    }
}
