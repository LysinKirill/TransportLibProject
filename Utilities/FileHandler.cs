using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using TransportLib;

namespace Utilities
{
    /// <summary>
    /// Класс, отвечающий за работу с файлами
    /// </summary>
    public class FileHandler
    {
        private string _path;

        private FileHandler(string path)
        {
            _path = path;
        }


        /// <summary>
        /// Проверка возможности чтения файла по заданному имени
        /// </summary>
        /// <param name="fileName">Имя файла, проверка которого проводится</param>
        private static bool CheckFileName(string fileName)
        {
            if (!File.Exists(fileName))
                return false;
            try
            {
                new FileInfo(fileName);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (PathTooLongException)
            {
                return false;
            }
            catch
            {
                return false;
            }
            if ((fileName.Length < 5) || (fileName.Substring(fileName.Length - 4) != ".txt"))
                return false;
            return true;
        }

        /// <summary>
        /// Метод, проверяющий, содержит ли файл хотя бы одну непустую строку;
        /// </summary>
        public bool CheckFile()
        {
            string[] lines = File.ReadAllLines(_path);
            if (lines.Length == 0)
            {
                Console.WriteLine("Введённый файл пустой, отсутствуют данные для обработки");
                return false;
            }

            //  Пропуск пустых строк
            lines = lines.Where(x => x != string.Empty).ToArray();
            if (lines.Length == 0)
            {
                Console.WriteLine("Введённый файл содержит лишь пустые строки, отсутствуют данные для обработки");
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Метод, в котором производится считывание транспорта из файла и фильтрация транспорта в соответствии с заданными критериями
        /// </summary>
        /// <param name="n">Значение, используемое для фильтрации мотоциклов по расходу топлива</param>
        /// <param name="m">Значение, используемое для фильтрации автомобилей по расходу топлива</param>
        /// <param name="filteredCars">Отфильтрованные по критерию машины</param>
        /// <param name="filteredMotorcycles">Отфильтрованные по критерию мотоциклы</param>
        /// <param name="showInvalidVehicles">Флаг, определяющий необходимость вывода в консоль транспорта с некорректными параметрами</param>
        /// <param name="showUnprocessedLines">Флаг, определяющий необходимость вывода в консоль некорректных строк файла</param>
        /// <returns>Логическое значение, отображающее успешность обработки файла</returns>
        public bool ProcessFile(double n, double m, out List<Car> filteredCars, out List<Motorcycle> filteredMotorcycles, bool showInvalidVehicles, bool showUnprocessedLines)
        {
            List<Transport> list = new List<Transport>();
            filteredCars = new List<Car>();
            filteredMotorcycles = new List<Motorcycle>();
            //  Считывание содержимого файла
            string[] lines = File.ReadAllLines(_path);
            
            bool foundTransport = false;
            for(int i = 0; i < lines.Length; i++)
            {
                
                // Альтернативно можно было не писать блок try-catch и написать с помощью if-ов,
                // но тогда исключения, выбрасываемые при попытке создания объектов транспорта пришлось бы обрабатывать на более высоком уровне
                try
                {
                    string s = lines[i];
                    
                    //  Строки, содержащие неподходящую информацию, пропускаются
                    if (s.Length == 0)
                        continue;
                        
                    string[] args = s.Split();
                    //  Строки, содержащие неправильное количество слов (значений)
                    if (args.Length != 4)
                        throw new ArgumentException("Строка должна содержать 4 разделённых пробелом значения");
                    
                    //  Проверка на возможность излечения целочисленных значений из строк
                    if (!int.TryParse(args[2], out int fuelTankCapacity) || !double.TryParse(args[3], out double fuelMileage))
                        throw new ArgumentException("Третье значение в строке должно быть целым числом, а четвёртое значение в строке должно быть вещественным числом,\n" +
                                                    "записанным с учётом региональных настроек");
                    
                    //  Создание объектов правильного типа и их фильтрация
                    switch (args[0])
                    {

                        case "Car":
                            list.Add(new Car(fuelTankCapacity, fuelMileage, args[1]));
                            //  Фильтрация
                            if (list[list.Count - 1].Filter(m))
                                filteredCars.Add((Car)list[list.Count - 1]);
                            foundTransport = true;
                            break;
                        case "Motorcycle":
                            list.Add(new Motorcycle(fuelTankCapacity, fuelMileage, args[1]));
                            //  Фильтрация
                            if (list[list.Count - 1].Filter(n))
                                filteredMotorcycles.Add((Motorcycle)list[list.Count - 1]);
                            foundTransport = true;
                            break;

                        default:
                            throw new ArgumentException($"Первым словом в строке может быть только Car или Motorcycle. Найдено: {args[0]}");
                    }
                }
                catch (ArgumentException e)
                {
                    if(showUnprocessedLines)
                        Console.WriteLine($"строка №{i + 1} из файла: {e.Message};");
                }
                catch (InvalidVehicleException e)
                {
                    if (showInvalidVehicles)
                        Console.WriteLine($"Строка №{i + 1} из файла: {e.Message}");
                }
            }
            
            //  Проверка, что была найдена хотя бы одна строка, из которой можно считать транспорт
            if (!foundTransport)
            {
                Console.WriteLine("Файл не содержит строк, из которых можно считать транспорт");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Метод, запрашивающий имя файла у пользователя и возвращающий новый объект FileHandler
        /// </summary>
        public static FileHandler GetFileHandler()
        {
            Console.WriteLine("Введите имя файла, содержащего данные:");
            bool flag = false;
            string fileName = string.Empty;
            while (!CheckFileName(fileName))
            {
                if(flag)
                    Console.WriteLine("Файл с данным именем не найден или расширение файла не .txt\n" +
                                      "Повторите ввод имени файла:");
                fileName = Console.ReadLine();
                flag = true;
            }
            return new FileHandler(fileName);
        }

        /// <summary>
        /// Метод, записывающий массив строк в файл по заданному имени
        /// </summary>
        /// <param name="fileName">Имя файла, в который необходимо проводить запись</param>
        /// <param name="lines">Массив строк для записи в файл</param>
        /// <param name="encoding">Кодировка, используемая при записи данных в файл</param>
        public static void WriteLines(string fileName, string[] lines, Encoding encoding)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, false, encoding))
                {
                    foreach (string line in lines)
                        sw.WriteLine(line);
                }
            }
            catch
            {
                Console.WriteLine("Невозможно сохранить данные в файл ");
            }
        }
    }
}