using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternSingleton
{
    public class ActionHistory_V1 : IActionHistory
    {
        private Stack<string> _history { get; set; }

        public ActionHistory_V1()
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