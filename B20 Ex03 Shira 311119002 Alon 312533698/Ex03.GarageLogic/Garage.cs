using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    class Garage
    {
        readonly Dictionary<string, Customer> r_Vehicles = new Dictionary<string, Customer>();
        
        public bool AddNewVehicle(string io_CustomerName, string io_CustomerPhoneNumber, eSupportVehicles io_Choice, string io_SerialNumber)
        {
            bool isAdded = false;

            if (r_Vehicles.ContainsKey(io_SerialNumber))
            {
                r_Vehicles[io_SerialNumber].VehicleStatus = eServiceStatus.InRepair;
            }
            else
            {
                Vehicle newVehicle = VehicleObjectCreator.CreateVehicle(io_Choice, io_SerialNumber);
                Customer newCustomer = new Customer(io_CustomerName, io_CustomerPhoneNumber, eServiceStatus.InRepair, newVehicle);
                r_Vehicles.Add(io_SerialNumber, newCustomer);
                isAdded = true;
            }

            return isAdded;
        } 
        
        public StringBuilder VehicleStringByFilter(bool i_InRepair, bool i_Fixed, bool i_Paid)
        {
            int index = 1;
            StringBuilder vehicleList = new StringBuilder();

            foreach (KeyValuePair<string, Customer> entry in r_Vehicles)
            {
                if (i_InRepair && entry.Value.VehicleStatus.Equals(eServiceStatus.InRepair))
                {
                    vehicleList.Append(string.Format("{0}. {1}{2}", index++, entry.Key, Environment.NewLine));
                }

                if (i_Fixed && entry.Value.VehicleStatus.Equals(eServiceStatus.Fixed))
                {                
                    vehicleList.Append(string.Format("{0}. {1}{2}", index++, entry.Key, Environment.NewLine));
                }
                if (i_Paid && entry.Value.VehicleStatus.Equals(eServiceStatus.Paid))
                {
                    vehicleList.Append(string.Format("{0}. {1}{2}", index++, entry.Key, Environment.NewLine));
                }
            }

            return vehicleList;
        }

        public bool ChangeServiceStatus(string i_SerialNumber, eServiceStatus i_NewStatus)
        {
            bool isChanged = false;

            if (r_Vehicles.ContainsKey(i_SerialNumber))
            {
                r_Vehicles[i_SerialNumber].VehicleStatus = i_NewStatus;
                isChanged = true;
            }

            return isChanged;
        }

        public bool InflateWheels(string i_SerialNumber)
        {
            bool isInflated = false;
            float amountToAdd = 0f;

            if (r_Vehicles.ContainsKey(i_SerialNumber))
            {
                Wheel[] currentWheels = r_Vehicles[i_SerialNumber].Vehicle.Wheels;

                for (int i = 0; i <= currentWheels.Length; i++) 
                {
                    amountToAdd = currentWheels[i].MaxAirPressure - currentWheels[i].CurrentAirPressure;
                    currentWheels[i].InflatingAirPressure(amountToAdd);
                }

                isInflated = true;
            }

            return isInflated;
        }

        public bool FillGasTank(string i_SerialNumber, eFuelType io_FuelType, float io_AmountToAdd)
        {
            bool isFilled = false;

            if (r_Vehicles.ContainsKey(i_SerialNumber))
            {
                if (r_Vehicles[i_SerialNumber].Vehicle.EnergyType is Fuel)
                {
                    isFilled = (r_Vehicles[i_SerialNumber].Vehicle.EnergyType as Fuel).FillTank(io_AmountToAdd, io_FuelType);
                }
            }

            return isFilled;
        }

        public bool FillCharge(string i_SerialNumber, , float io_MinutesToAdd)
        {
            bool isFilled = false;

            if (r_Vehicles.ContainsKey(i_SerialNumber))
            {
                if (r_Vehicles[i_SerialNumber].Vehicle.EnergyType is Electric)
                {
                    isFilled = (r_Vehicles[i_SerialNumber].Vehicle.EnergyType as Electric).ChargeBattery(io_MinutesToAdd / 60f);
                }
            }

            return isFilled;
        }
    }
}