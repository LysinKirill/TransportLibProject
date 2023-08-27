using System;

namespace TransportLib
{
    /// <summary>
    /// абстрактный класс транспортных средств
    /// </summary>
    public abstract class Transport
    {
        protected double _fuelMileage;
        private string _name;
        private int _fuelTankCapacity;
        
        //  Свойства
        public string Name
        {
            get { return _name;}
            private set
            {
                if (!CheckName(value))
                    throw new ArgumentException("Допускается последовательность только из букв латинского алфавита для имени. Имя должно содержать хотя бы один символ");
                _name = value;
            }
        }
        
        public virtual int FuelTankCapacity { get; protected set; }

        public abstract double FuelMileage { get; set; }

        //  Конструктор
        protected Transport(int fuelTankCapacity, double fuelMileage, string name)
        {
            Name = name;
            FuelTankCapacity = fuelTankCapacity;
            FuelMileage = fuelMileage;

        }

        /// <summary>
        /// Метод, проверяющий корректность переданного имени
        /// </summary>
        /// <param name="name">имя для проверки</param>
        private bool CheckName(string name)
        {
            if (name.Length == 0)
                return false;
            foreach (char x in name)
            {
                if (!((x >= 'A' && x <= 'Z') || (x >= 'a' && x <= 'z') || (x >= '0' && x <= '9')))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Индексатор, возвращающий количество дозаправок, необходимых для того чтобы проехать заданное количество километров
        /// </summary>
        /// <param name="n">Расстояние, которое нужно проехать (в километрах)</param>
        public virtual int this[int n]
        {
            get
            {
                if (n < 0)
                    throw new ArgumentException("Число километров должно быть неотрицательным");
                double liters = (FuelMileage * n / 100d);
                return (int)Math.Ceiling(liters / FuelTankCapacity);
            }
        }

        /// <summary>
        /// абстрактный метод, используемый для фильтрации транспортных средств
        /// </summary>
        /// <param name="filterValue">значение, по которому происходит фильтрация</param>
        public abstract bool Filter(double filterValue);
        public override string ToString()
        {
            return $"TransportLib {_name} {_fuelTankCapacity} {FuelMileage}";
        }
    }
}