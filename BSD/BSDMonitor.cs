using BSD.Interface;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Timers;

namespace BSD
{
    public class BSDMonitor
    {
        private List<IObserver> observers;
        private List<Item> items;
        private MessagePublisher messagePublisher;
        private Timer _timer;
        public BSDMonitor(double refreshHours, string accountSid, string authToken, string fromPhoneNumber, string toPhoneNumber)
        {
            _timer = new Timer(TimeSpan.FromSeconds(refreshHours).TotalMilliseconds);
            _timer.Elapsed += _timer_Elapsed;
            messagePublisher = new MessagePublisher(accountSid, authToken, fromPhoneNumber, toPhoneNumber);
            messagePublisher.InitTwilio();
            observers = new List<IObserver>();
            items = new List<Item>();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                foreach(var item in items)
                {
                    item.ReloadDoc();
                    UpdateItemPrice(item.Name, decimal.Parse(item.GetTrimedOffCurrency(), NumberStyles.Currency, new CultureInfo("pl-PL")));
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public void StartMonitoring()
        {
            _timer.Start();
        }

        public void StopMonitoring()
        {
            _timer.Elapsed -= _timer_Elapsed;
            _timer.Stop();
        }

        #region Observer
        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void UpdateItemPrice(string itemName, decimal newPrice)
        {
            var item = items.Find(i => i.Name == itemName);
            if (item == null)
                return;
            
            if(item.MainPrice < newPrice)
            {
                item.OldPrice = item.MainPrice;
                item.MainPrice = newPrice;
                NotifyObservers(item);
            }
        }

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        private void NotifyObservers(Item item)
        {
            foreach (var observer in observers)
            {
                observer.Update(item);
            }
        }
        #endregion



    }
}
