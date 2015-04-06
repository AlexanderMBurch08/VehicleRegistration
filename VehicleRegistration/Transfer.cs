using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VehicleRegistration
{
    [Serializable]
    public class Transfer
    {
        string date, license, price, endDate;
        Owner owner;
        Dealer dealer;
        public Transfer(string date, string license, string price, Owner owner)
        {
            this.date = date;
            this.license = license;//vehicle class?
            this.price = price;
            this.owner = owner;
        }
        public Transfer(string date, string price, Dealer dealer)
        {
            this.dealer = dealer;
            this.date = date;
            this.price = price;
        }
        public string getLicense
        {
            get
            {
                return license;
            }
        }
        public string getDate
        {
            get
            {
                return date;
            }
        }
        public string getPrice
        {
            get
            {
                return price;
            }
        }
        public void setDate(string endDate)
        {
            this.endDate = endDate;
        }
        public Dealer getDealer
        {
            get
            {
                return dealer;
            }
        }
        public Owner getOwner
        {
            get
            {
                return owner;
            }
        }
        public override string ToString()
        {
            return ("From:"+date+  "  To:"+endDate+",   Price:$" + price);
        }
    }
}
