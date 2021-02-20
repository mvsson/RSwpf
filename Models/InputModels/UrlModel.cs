using System;

namespace RateShopperWPF.Models.InputModels
{
    class UrlModel
    {
        public readonly string ParentLink;
        public readonly string Link;
        public readonly DateTime Date;
        public UrlModel(string parentLink, string link, DateTime date )
        {
            ParentLink = parentLink;
            Link = link;
            Date = date;
        }
    }
}
