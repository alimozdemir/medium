using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternSingleton
{
    public class ActionHistory_V3 : IActionHistory
    {
        private static Lazy<ActionHistory_V3> instance = 
                        new Lazy<ActionHistory_V3>(() => new ActionHistory_V3());
        public static ActionHistory_V3 Instance { get; } = instance.Value;

        private Stack<string> _history { get; set; }

        private ActionHistory_V3()
        {
            if (File.Exists("actions.txt"))
            {
                var lines = File.ReadAllLines("actions.txt");

                _history = new Stack<string>(lines.ToList());
            }
            else
                _history =  new Stack<string>();
        }
        public void AddAction(string action)
        {
            _history.Push(action);
        }
        public void Save()
        {
            File.WriteAllLines("actions.txt", _history);
        }
        public string RetriveLastAction()
        {
            return _history.FirstOrDefault();
        }
        public List<string> RetriveAllActions()
        {
            return _history.ToList();
        }
    }
}