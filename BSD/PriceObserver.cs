using BSD.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSD
{
    public class PriceObserver : IObserver
    {
        private string observerName;

        public PriceObserver(string name)
        {
            observerName = name;
        }

        public void Update(Item item)
        {
            Console.WriteLine($"Observer {observerName}: Item {item.Name} price updated to {item.MainPrice}");
        }
    }
}
