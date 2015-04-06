using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace VehicleRegistration
{
    [Serializable()]
    public partial class VehicleRegistrationSystem : Form
    {
        List<Dealer> AllDealers = new List<Dealer>();
        List<OwnerVehicle> AllVehicles = new List<OwnerVehicle>();
        List<Owner> AllOwners = new List<Owner>();
        public VehicleRegistrationSystem()
        {
            InitializeComponent();
        }

        private void bnDone_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bnRegDealer_Click(object sender, EventArgs e)
        {
            DealerRegistrationDialogbox drd = new DealerRegistrationDialogbox();
            if (drd.ShowDialog() == DialogResult.OK)
            {
                Dealer x = new Dealer(drd.DealerID, drd.DealerName, drd.DealerCity, drd.DealerState);
                AllDealers.Add(x);
            }
        }

        private void bnRegVehicle_Click(object sender, EventArgs e)
        {
            VehicleRegistrationDialogbox vrd = new VehicleRegistrationDialogbox();
            if (vrd.ShowDialog() == DialogResult.OK)
            {
                OwnerVehicle tempVehic = new OwnerVehicle(vrd.VIN, vrd.Make, vrd.Model, vrd.Year, vrd.Color, vrd.InitialDealerID);
                AllVehicles.Add(tempVehic);
                Dealer temp = AllDealers.Find(x=>x.getDid.Equals(vrd.InitialDealerID));
                temp.AddVehicle(tempVehic);
            }
        }

        private void bnRegOwner_Click(object sender, EventArgs e)
        {
            OwnerRegistrationDialogbox ord = new OwnerRegistrationDialogbox();
            if (ord.ShowDialog() == DialogResult.OK)
            {
                Owner x = new Owner(ord.SSN, ord.FirstName, ord.LastName, ord.Address, ord.BirthDate);
                AllOwners.Add(x);
            }
        }

        private void bnDeleteDealer_Click(object sender, EventArgs e)
        {
            // do not implement
        }

        private void bnDeleteVehicle_Click(object sender, EventArgs e)
        {
            // do not implement
        }

        private void bnDeleteOwner_Click(object sender, EventArgs e)
        {
            // do not implement
        }

        private void bnListVehicles_Click(object sender, EventArgs e)
        {
            ListDialog ld = new ListDialog();
            foreach (OwnerVehicle x in AllVehicles)
            {
                ld.AddDisplayItems(x.ToString());
            }
            //ld.AddDisplayItem( );  // add items to print
            ld.ShowDialog();
        }

        private void bnListOwners_Click(object sender, EventArgs e)
        {
            ListDialog ld = new ListDialog();
            foreach (Owner x in AllOwners)
            {
                ld.AddDisplayItems(x.ToString());
            }
            //ld.AddDisplayItems(/* pass object[] */);
            ld.ShowDialog();
        }

        private void bnListDealers_Click(object sender, EventArgs e)
        {
            ListDialog ld = new ListDialog();
            foreach (Dealer x in AllDealers)
            {
                ld.AddDisplayItems(x.ToString());
                List<OwnerVehicle> dV = x.getVehicles;
                if (x.getVehicles.Count != 0)
                {
                    ld.AddDisplayItems("\t" + "Vehicles Owned");
                }
                foreach (OwnerVehicle j in dV)
                {
                    ld.AddDisplayItems("\t\t"+j.ToString());
                }
            }
            ld.ShowDialog();
        }
        private void bnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "VRS Files|*.vrs";
            saveFileDialog.AddExtension = true;
            saveFileDialog.InitialDirectory = Application.StartupPath;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // you can put the following in a try block
                System.IO.FileStream f = new System.IO.FileStream(saveFileDialog.FileName, System.IO.FileMode.Create);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter fo = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                fo.Serialize(f, AllDealers);
                fo.Serialize(f, AllVehicles);
                fo.Serialize(f, AllOwners);
                f.Close();
            }
        }

        private void bnRestore_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VRS Files|*.vrs";
            openFileDialog.InitialDirectory = Application.StartupPath;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // you can put the following in a try block
                System.IO.FileStream f = new System.IO.FileStream(openFileDialog.FileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter fo = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                AllDealers = (List<Dealer>)fo.Deserialize(f);
                AllVehicles = (List<OwnerVehicle>)fo.Deserialize(f);
                AllOwners = (List<Owner>)fo.Deserialize(f);
  
            }
        }

        private void bnTransfer_Click(object sender, EventArgs e)
        {
            string from, to, price, date, license;
            OwnershipTransferDialogbox otd = new OwnershipTransferDialogbox();
            if (otd.ShowDialog() != DialogResult.OK) return;
            OwnerVehicle curreVehicle = AllVehicles.Find(x => x.getVin.Equals(otd.VIN));
            if (otd.TransferFromDealer == true)
            {
                from = otd.FromDealerID;
                Dealer fromD = AllDealers.Find(x => x.getDid.Equals(from));
                fromD.RemoveVehicle(curreVehicle);
                if(otd.TransferToDealer == true)
                {
                    to = otd.ToDealerID;
                    Dealer toD = AllDealers.Find(x => x.getDid.Equals(to));
                    toD.AddVehicle(curreVehicle);
                    price = otd.Price;
                    date = otd.Date;
                    curreVehicle.TransferD2D(toD, price, date);
                }
                else//private Owner Method
                {
                    to = otd.ToSSN;
                    Owner toO = AllOwners.Find(x => x.getSsn.Equals(to));
                    toO.Add(curreVehicle);
                    license = otd.LicenceNumber;
                    price = otd.Price;
                    date = otd.Date;
                    curreVehicle.TransferD2O(toO, price, date, license);
                }
            }
            else//private owner to ?from
            {
                from = otd.FromSSN;
                Owner fromO = AllOwners.Find(x => x.getSsn.Equals(from));
                fromO.Remove(curreVehicle);
                if(otd.TransferToDealer== true)
                {
                    to = otd.ToDealerID;
                    Dealer toD = AllDealers.Find(x => x.getDid.Equals(to));
                    toD.AddVehicle(curreVehicle);
                    price = otd.Price;
                    date = otd.Date;
                    curreVehicle.TransferO2D(toD,price,date);
                }
                else//Private owner method
                {
                    to = otd.ToSSN;
                    Owner toO = AllOwners.Find(x => x.getSsn.Equals(to));
                    toO.Add(curreVehicle);
                    license = otd.LicenceNumber;
                    price = otd.Price;
                    date = otd.Date;
                    curreVehicle.TransferO2O(toO, price,date, license);
                }
            }
        }

        private void bnListOwnedVehicles_Click(object sender, EventArgs e)
        {
            LocateOwnerDialogbox lod = new LocateOwnerDialogbox();
            if (lod.ShowDialog() == DialogResult.OK)
            {
                string ssn = lod.SSN;
                Owner curr = AllOwners.Find(x => x.getSsn.Equals(ssn));
                if (curr == null)
                {
                    return;
                }
                else
                {
                    ListDialog ld = new ListDialog();
                    ld.AddDisplayItems(curr.ToString());
                    ld.AddDisplayItems("Currently Owns:");
                    foreach (OwnerVehicle x in curr.getVehics)
                    {
                        ld.AddDisplayItems(x.ToString()+" "+x.PrintOwner());
                    }
                    ld.ShowDialog();
                }
            }
           
        }

        private void ListOwnerHistory_Click(object sender, EventArgs e)
        {
            LocateVehicleDialogbox lvd = new LocateVehicleDialogbox();
            if (lvd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sB = new StringBuilder();
                ListDialog ld = new ListDialog();
                string vin = lvd.VIN;
                OwnerVehicle currVehicle = AllVehicles.Find(x => x.getVin.Equals(vin));
                List<Transfer> History = currVehicle.getList;
                sB.Append(currVehicle.ToString());
                ld.AddDisplayItems(sB);
                sB = new StringBuilder();
                if (currVehicle.currTransfer.getOwner != null)
                {
                    sB.Append("\t" + currVehicle.currTransfer.getOwner.ToString() + " " + currVehicle.PrintOwner());
                    ld.AddDisplayItems(sB);
                }
                sB = new StringBuilder();
                sB.Append("\t\t" + "History: ");
                ld.AddDisplayItems(sB);
                foreach (Transfer x in History)
                {
                    sB = new StringBuilder();
                    sB.Append("\t\t" + x.getOwner.ToString() + " " + x.ToString());
                    ld.AddDisplayItems(sB);
                }
                ld.ShowDialog();
            }
        }

        private void bnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VRS Info Files|*.inf";
            openFileDialog.InitialDirectory = Application.StartupPath;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                TextReader trs = new StreamReader(openFileDialog.FileName);
                string s;
                List<string> words;
                int stringIndex;
                while (((s = trs.ReadLine()) != null) && (s != ""))
                {
                    words = new List<string>();
                    while (true)
                    {
                        if ((stringIndex = s.IndexOf('"')) == -1) break;
                        s = s.Substring(stringIndex + 1);
                        stringIndex = s.IndexOf('"');
                        words.Add(s.Substring(0, stringIndex));
                        s = s.Substring(stringIndex + 1);
                       
                    }
                    if (words.Count == 0)
                    {
                        break;
                    }
                    if (words[0].Equals("RegisterDealer"))
                    {
                        Dealer addD = new Dealer(words[1], words[2], words[3], words[4]);
                        AllDealers.Add(addD);
                    }
                    else if (words[0].Equals("RegisterVehicle"))
                    {
                        OwnerVehicle addV = new OwnerVehicle(words[1], words[2], words[3], words[4], words[5], words[6]);
                        AllVehicles.Add(addV);
                        Dealer temp = AllDealers.Find(x => x.getDid.Equals(words[6]));
                        temp.AddVehicle(addV);
                    }
                    else if (words[0].Equals("RegisterOwner"))
                    {
                        Owner addO = new Owner(words[1], words[2], words[3], words[4], words[5]);
                        AllOwners.Add(addO);
                    }
                    else if (words[0].Equals("TransferD2D"))
                    {
                        Dealer from = AllDealers.Find(x => x.getDid.Equals(words[1]));
                        Dealer to = AllDealers.Find(x => x.getDid.Equals(words[2]));
                        OwnerVehicle currVehicle = AllVehicles.Find(x => x.getVin.Equals(words[3]));
                        to.AddVehicle(currVehicle);
                        Transfer newTrans = new Transfer(words[4], words[5], to);
                        currVehicle.Add(newTrans);
                        from.RemoveVehicle(currVehicle);
                    }
                    else if (words[0].Equals("TransferD2O"))
                    {
                        Dealer from = AllDealers.Find(x => x.getDid.Equals(words[1]));
                        Owner to = AllOwners.Find(x => x.getSsn.Equals(words[2]));
                        OwnerVehicle currVehicle = AllVehicles.Find(x => x.getVin.Equals(words[3]));
                        to.Add(currVehicle);
                        currVehicle.TransferD2O(to, words[5], words[4], words[6]);
                        from.RemoveVehicle(currVehicle);
                    }
                    else if (words[0].Equals("TransferO2D"))
                    {
                        Owner from = AllOwners.Find(x => x.getSsn.Equals(words[1]));
                        Dealer to = AllDealers.Find(x => x.getDid.Equals(words[2]));
                        OwnerVehicle currVehicle = AllVehicles.Find(x => x.getVin.Equals(words[3]));
                        to.AddVehicle(currVehicle);
                        currVehicle.TransferO2D(to,words[5],words[4]);
                        from.Remove(currVehicle);
                    }
                    else if (words[0].Equals("TransferO2O"))
                    {
                        Owner from = AllOwners.Find(x => x.getSsn.Equals(words[1]));
                        Owner to = AllOwners.Find(x => x.getSsn.Equals(words[2]));
                        OwnerVehicle currVehicle = AllVehicles.Find(x => x.getVin.Equals(words[3]));
                        to.Add(currVehicle);
                        currVehicle.TransferO2O(to, words[5], words[4], words[6]);
                        from.Remove(currVehicle);
                    }
                    else if (words[0].Equals("ListOwnersOfVehicle"))
                    {
                        StringBuilder sB = new StringBuilder();
                        ListDialog ld = new ListDialog();
                        OwnerVehicle vehicle = AllVehicles.Find(x => x.getVin.Equals(words[1]));
                        List<Transfer> History = vehicle.getList;
                        sB.Append(vehicle.ToString());
                        ld.AddDisplayItems(sB);
                        sB = new StringBuilder();
                        if (vehicle.currTransfer.getOwner!=null)
                        {
                            sB.Append("\t" +vehicle.currTransfer.getOwner.ToString()+" "+vehicle.PrintOwner());
                            ld.AddDisplayItems(sB);
                        }
                        sB = new StringBuilder();
                        sB.Append("\t\t"+"History: ");
                        ld.AddDisplayItems(sB);
                        foreach (Transfer x in History)
                        {
                            sB = new StringBuilder();
                            sB.Append("\t\t"+x.getOwner.ToString()+" "+x.ToString());
                            ld.AddDisplayItems(sB);
                        }
                        ld.ShowDialog();
                    }
                    else if (words[0].Equals("ListVehiclesOfOwner"))
                    {
                        StringBuilder sB = new StringBuilder();
                        ListDialog ld = new ListDialog();
                        Owner person = AllOwners.Find(x=>x.getSsn.Equals(words[1]));
                        ld.AddDisplayItems(person.ToString());
                        ld.AddDisplayItems("Currently Owns:");
                        List<OwnerVehicle> temp = person.getVehics;
                        foreach (OwnerVehicle x in temp)
                        {
                            ld.AddDisplayItems("\t"+x.ToString()+" "+x.PrintOwner());
                        }
                        ld.ShowDialog();
                    }
                    else if (words[0].Equals("ListDealers"))
                    {
                        StringBuilder sB = new StringBuilder();
                        ListDialog ld = new ListDialog();
                        foreach (Dealer x in AllDealers)
                        {
                            sB.Append(x.ToString());
                            ld.AddDisplayItems(sB.ToString());
                            List<OwnerVehicle> dV = x.getVehicles;
                            sB = new StringBuilder();
                            if (x.getVehicles.Count != 0)
                            {
                                ld.AddDisplayItems("\t" + "Vehicles Owned");
                            }
                            foreach (OwnerVehicle j in dV)
                            {
                                sB.Append("\t\t").Append(j.ToString());
                                ld.AddDisplayItems(sB.ToString());
                                sB = new StringBuilder();
                            }
                            sB = new StringBuilder();
                        }
                        ld.ShowDialog();
                    }
                }

            }
        }
    }
}
