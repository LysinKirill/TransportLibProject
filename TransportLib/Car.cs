using System;
namespace TransportLib
{
    /// <summary>
    /// Класс, представляющий автомобиль 
    /// </summary>
    public class Car:Transport
    {
        //  Свойства
        public override double FuelMileage
        {
            get => _fuelMileage;
            set
            {
                if (value > 30)
                    throw new InvalidVehicleException("Расход топлива на 100 км для автомобиля не может превышать 30 литров;");
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
                if (value < 10)
                    throw new InvalidVehicleException("Объём бензобака автомобиля не может быть меньше 10 литров;");
                base.FuelTankCapacity = value;
            }
        }
        
        //  Конструктор
        public Car(int fuelTankCapacity, double fuelMileage, string name) : base(fuelTankCapacity, fuelMileage, name)
        {
        }
        
        /// <summary>
        /// Метод, используемый для фильтрации автомобилей по критерию
        /// </summary>
        /// <param name="filterValue">критерий фильтра</param>
        /// <returns>логическое значение, показывающее, удовлетворяет ли данный мотоцикл переданному критерию</returns>
        public override bool Filter(double filterValue)
        {

            // Если произошёл сбой
            if (IsMalfunction(10))
            {
                Console.WriteLine($"Автомобиль {Name} дал сбой. Результаты вычисления расхода топлива изменились");
                return (_fuelMileage * Math.Log(100)) * 10   < filterValue;
            }
            
            // Если не произошёл сбой
            return _fuelMileage * 10 < filterValue;
            
            //  В задании ОЧЕНЬ непонятно написано насчёт сбоя, который может давать автомобиль
            //  В данной программе реализация сбоя подразумевает, что при одних и тех же входных данных программа может возвращать разный результат,
            //  т.к. расчёт расхода на 1000 километров будет давать разный результат в зависимости от того, произошёл сбой или нет
        }

        /// <summary>
        /// Индексатор, возвращающий количество дозаправок, необходимых для того чтобы проехать заданное количество километров
        /// </summary>
        /// <param name="n">Расстояние, которое нужно проехать (в километрах)</param>
        public override int this[int n]
        {
            get
            {
                if (n < 0)
                    throw new ArgumentException("Число километров должно быть неотрицательным;");
                if (n == 0)
                    return 0;
                double additionalMileage = 1;
                if (IsMalfunction(10) && n > 1)
                {
                    additionalMileage = Math.Log(n);
                    Console.WriteLine($"Автомобиль {Name} дал сбой. Результаты вычисления расхода топлива изменились");
                }
                


                double liters = ((FuelMileage * additionalMileage) * n/100d);
                return (int)Math.Ceiling(liters / FuelTankCapacity);
            }
        }
        


        /// <summary>
        /// Метод, "определяющий" произошёл ли сбой
        /// </summary>
        /// <param name="chance">Вероятность сбоя в процентах</param>
        /// <returns>Логическое значение, показывающее, произошёл ли сбой</returns>
        private bool IsMalfunction(int chance)
        {
            if (chance < 0)
                chance = 0;
            if (chance > 100)
                chance = 100;
            Random rand = new Random();
            return (rand.NextDouble() <= chance / 100d);
        }

        public override string ToString()
        {
            return
                $"Car {Name} {FuelTankCapacity} {FuelMileage}";
        }
    }
}