using System;
using System.Collections.Generic;
using System.Text;

namespace BSD.Interface
{
    public interface IObserver
    {
        void Update(Item item);
    }
}
