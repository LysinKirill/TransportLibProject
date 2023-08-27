using System;

namespace TransportLib
{
    /// <summary>
    /// Класс, представляющий мотоцикл
    /// </summary>
    public class Motorcycle:Transport
    {
        // Свойства
        public override double FuelMileage
        {
            get => _fuelMileage;
            set
            {
                if (value > 15)
                    throw new InvalidVehicleException("Расход топлива на 100 км для мотоцикла не может превышать 15 литров;");
                if(value <= 0) 
                    throw new InvalidVehicleException("Расход топлива на 100 км должен быть положительным числом;");
                _fuelMileage = value;
            }
        }

        public override int FuelTankCapacity
        {
            get => base.FuelTankCapacity;
            protected set
            {
                if (value < 5)
                    throw new InvalidVehicleException("Объём бензобака мотоцикла не может быть меньше 5 литров;");
                base.FuelTankCapacity = value;
            }
        }

        //  Конструктор
        public Motorcycle(int fuelTankCapacity, double fuelMileage, string name):base(fuelTankCapacity, fuelMileage, name)
        {
        }

        /// <summary>
        /// Метод, используемый для фильтрации мотоциклов по критерию
        /// </summary>
        /// <param name="filterValue">критерий фильтра</param>
        /// <returns>логическое значение, показывающее, удовлетворяет ли данный мотоцикл переданному критерию</returns>
        public override bool Filter(double filterValue)
        {
            
            return _fuelMileage * 10 < filterValue;
        }
        
        public override int this[int n]
        {
            get
            {
                if (n < 0)
                    throw new ArgumentException("Число километров, проезжаемых мотоциклом, быть неотрицательным");
                double liters = (FuelMileage * n / 100d);
                return (int)Math.Ceiling(liters / FuelTankCapacity);
            }
        }

        
        public override string ToString()
        {
            return
                $"Motorcycle {Name} {FuelTankCapacity} {FuelMileage}";
        }
    }
}