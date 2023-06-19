using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace BSD
{
    public class Item
    {
        public Item(string url, decimal expectedPrice, string expectedSize)
        {
            ExpectedPrice = expectedPrice;
            Size = expectedSize;
            UrlLink = url;
            InitDoc(url);

            Name = ItemHtmlDocument.DocumentNode.SelectSingleNode("//h1").InnerText.Trim();
            MainPrice = decimal.Parse(GetTrimedOffCurrency(), NumberStyles.Currency, new CultureInfo("pl-PL"));

        }

        private void InitDoc(string url)
        {
            var _htmlWeb = new HtmlWeb();
            var cookieContainer = new CookieContainer();
            _htmlWeb.PreRequest = request =>
            {
                request.CookieContainer = cookieContainer;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
                return true;
            };
            ItemHtmlDocument = _htmlWeb.Load(url);
        }

        public void ReloadDoc()
        {
            InitDoc(UrlLink);
        }

        public string GetTrimedOffCurrency()
        {
            var priceFromSite = ItemHtmlDocument.DocumentNode.SelectSingleNode("//div[@data-test='product-price']").InnerText.Trim();
            return priceFromSite.Substring(0, priceFromSite.Length - 3);
        }

        public decimal OldPrice { get; set; }
        public decimal MainPrice { get; set; }
        public decimal ExpectedPrice { get; set; }
        public string Name { get; set; }
        public string UrlLink { get; set; }
        public string Size { get; set; }
        public HtmlDocument ItemHtmlDocument { get; set; }
    }
}
