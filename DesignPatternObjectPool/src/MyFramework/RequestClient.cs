using System;

namespace MyFramework
{
    internal class RequestClient : Client
    {
        public override void Connect()
        {
            Console.WriteLine("Connecting to something with RequestClient...");
        }
    }
}