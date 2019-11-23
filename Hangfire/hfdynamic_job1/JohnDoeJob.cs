using System;
using System.Linq;

namespace hfdynamic_job1
{
    public class JohnDoeJob
    {
        public JohnDoeJob()
        {
            
        }
        public void ExecuteJob (int test)
        {
            var range = Enumerable.Range(0, test);
            var obj = Newtonsoft.Json.JsonConvert.SerializeObject(range);
            Console.WriteLine($"Hello from plugin {test} {obj}");
        }
    }
}
