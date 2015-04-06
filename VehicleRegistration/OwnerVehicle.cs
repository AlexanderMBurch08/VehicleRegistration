using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace VehicleRegistration
{
    [Serializable]
    public class OwnerVehicle
    {
        private List<Transfer> History = new List<Transfer>();
        private Transfer currTrans;
        string vin, make, model, year, color, firstDeal, price, license, since;
        public OwnerVehicle(string vin, string make, string model, string year, string color, string firstDeal)
        {
            this.vin = vin;
            this.make = make;
            this.model = model;
            this.year = year;
            this.color = color;
            this.firstDeal = firstDeal;

        }
        public Transfer currTransfer
        {
            get
            {
                return currTrans;
            }
        }
        public List<Transfer> getList
        {
            get
            {
                return History;
            }
        }
        public void Add(Transfer x)
        {
            History.Add(x);
        }
        public string getVin
        {
            get
            {
                return vin;
            }
        }
        public void TransferO2D(Dealer to, string price, string date)
        {
            this.price = price;
            since = date;
            currTrans.setDate(date);
            History.Add(currTrans);
            Transfer x = new Transfer(date, price, to);
            currTrans = x;
        }
        public void TransferO2O(Owner to, string price, string date,string license)
        {

            this.price = price;
            this.license = license;
            since = date;
            //currenttransfer needs to be added now
            currTrans.setDate(date);
            History.Add(currTrans);
            Transfer x = new Transfer(date, license, price, to);
            currTrans = x;
            //History.Add(x);
        }
        public void TransferD2O(Owner to, string price, string date, string license)
        {
            this.price = price;
            this.license = license;
            since = date;
            Transfer x = new Transfer(date, license, price, to);
            currTrans = x;
        }
        public void TransferD2D(Dealer toD, string price, string date)
        {
            this.price = price;
            since = date;
            currTransfer.setDate(date);
            Transfer x = new Transfer(date,price,toD);
            currTrans = x;
        }
        public string PrintOwner()
        {
            return("Since:"+since+"  License Number:"+license+",  Price:$"+price);
        }
        public override string ToString()
        {
            StringBuilder sB = new StringBuilder();
            sB.Append("VIN:" + vin + ",   Make:" + make + ",  Model:" + model + ",   Year:" + year + ",   Color: " + color);
            return sB.ToString();
        }
    }
}
