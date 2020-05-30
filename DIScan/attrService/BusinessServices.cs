using System;
using Microsoft.Extensions.DependencyInjection;

namespace attrService
{

    public interface IMyBusinessService
    {
        string GetValue();
    }

    public class MyBusinessService : IMyBusinessService
    {
        private string guid = Guid.NewGuid().ToString();
        public string GetValue()
        {
            return guid;
        }
    }

}