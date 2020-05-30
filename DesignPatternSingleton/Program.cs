using System;

namespace DesignPatternSingleton
{
    class Program
    {
        static void Main(string[] args)
        {
            IActionHistory v3 = ActionHistory_V3.Instance;
            v3.AddAction("hello 1");
            v3.Save();

            foreach(var item in v3.RetriveAllActions())
                Console.WriteLine(item);
        }
    }
}
