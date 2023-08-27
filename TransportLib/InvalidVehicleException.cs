using System;
namespace TransportLib
{
    /// <summary>
    /// Исключение, возникающее при попытке создания транспорта с неправильными характеристиками
    /// </summary>
    public class InvalidVehicleException : Exception
    {
        public InvalidVehicleException(string message) : base(message)
        {
        }

        public InvalidVehicleException() : base("Invalid vehicle")
        {
        }
    }
}