using System;

namespace DesignPatternSingleton
{
    class Program
    {
        static void Main(string[] args)
        {
            IActionHistory v3 = ActionHistory_V3.Instance;
            v3.AddAction("hello 1");

            Console.WriteLine(v3.RetriveLastAction());
        }
    }
}
