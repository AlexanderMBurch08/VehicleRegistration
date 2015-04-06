using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VehicleRegistration
{

    [Serializable]
    public class Dealer
    {
        private List<OwnerVehicle> DealerVehicles = new List<OwnerVehicle>();

        string Did, Name, City, State;
        public Dealer(string Did, string Name, string City, string State)
        {
            this.Did = Did;
            this.Name = Name;
            this.City = City;
            this.State = State;
            
        }
        public void AddVehicle(OwnerVehicle NewVehicle)
        {
            DealerVehicles.Add(NewVehicle);
            
        }
        public void RemoveVehicle(OwnerVehicle oldVehicle)
        {
            DealerVehicles.Remove(oldVehicle);
        }
        public List<OwnerVehicle> getVehicles
        {
            get
            {
                return DealerVehicles;
            }
        }
        public string getDid
        {
            get
            {
                return Did;
            }
        }
        public override string ToString()
        {
            StringBuilder sB = new StringBuilder();
            sB.Append("Dealer: "+Did+" "+Name + " ,  " + City + ",  " + State);
            return sB.ToString();
        }
    }
}
