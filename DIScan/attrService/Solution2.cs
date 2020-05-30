using System;
namespace attrService
{
    public class TransientAttribute : Attribute
    {
        public TransientAttribute()
        {
        }
    }

    public class ScopedAttribute : Attribute
    {
        public ScopedAttribute()
        {
        }
    }

    public class SingletonAttribute : Attribute
    {
        public SingletonAttribute()
        {
        }
    }

    [Transient]
    public class TransientAttrService : ITransientService
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid + " by Attribute";
        }
    }

    [Scoped]
    public class ScopedAttrService : IScopedService
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid + " by Attribute";
        }
    }

    [Singleton]
    public class SingletonAttrService : ISingletonService
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid + " by Attribute";
        }
    }

}