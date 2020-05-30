using System.Collections.Generic;

namespace DesignPatternSingleton
{
    public interface IActionHistory
    {
         void AddAction(string action);
         void Save();
         string RetriveLastAction();
         List<string> RetriveAllActions();
    }
}