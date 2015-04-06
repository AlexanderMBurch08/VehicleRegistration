using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VehicleRegistration
{
    [Serializable]
    public class Owner
    {
        List<OwnerVehicle> ownedV = new List<OwnerVehicle>();
        string ssn, first, last, address, birthDate;
        public Owner(string ssn, string first, string last, string address, string birthDate)
        {
            this.ssn = ssn;
            this.first = first;
            this.last = last;
            this.address = address;
            this.birthDate = birthDate;
        }
        public List<OwnerVehicle> getVehics
        {
            get
            {
                return ownedV;
            }
        }
        public string getSsn
        {
            get
            {
                return ssn;
            }
        }
        public void Add(OwnerVehicle vehicle)
        {
            ownedV.Add(vehicle);
        }
        public void Remove(OwnerVehicle vehicle)
        {
            ownedV.Remove(vehicle);
        }
        public override string ToString()
        {
            return ("SSN: " + ssn + ", Name:  " + first + "  " + last + "   , Address: " + address + "   , BirthDate: " + birthDate);
        }

    }
}
