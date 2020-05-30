using System;

namespace attrService
{
    public interface ITransient { }

    public interface IScoped { }

    public interface ISingleton { }

    public interface IMyTransientService
    {
        string GetValue();
    }
    public interface ITransientService
    {
        string GetValue();
    }

    public interface IScopedService
    {
        string GetValue();
    }

    public interface ISingletonService
    {
        string GetValue();
    }


    public class MyTransientService : IMyTransientService, ITransient
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid + " bb";
        }
    }

    public class TransientService : ITransientService, ITransient
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid;
        }
    }


    public class ScopedService : IScopedService, IScoped
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid;
        }
    }

    public class SingletonService : ISingletonService, ISingleton
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid;
        }
    }
}