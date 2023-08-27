using System;
using System.Collections.Generic;
using TransportLib;
using System.Linq;
using System.Text;


namespace Utilities
{
    /// <summary>
    /// Класс, реализующий пользовательское меню и методы, вызов которых доступен из меню
    /// </summary>
    public class Menu
    {
        private FileHandler _fileHandler;
        private bool _showInvalidVehicles;
        private bool _showUnprocessedLines;

        
        /// <summary>
        /// Вывод доступных пользователю команд
        /// </summary>
        public void ShowOptions()
        {
            Console.WriteLine("0 - Завершение работы программы;\n" +
                              "1 - Обработка файла по введённому имени;\n" +
                              "2 - Задать параметр, определяющий необходимость вывода необработанных строк в консоль;\n" +
                              "3 - Задать параметр, определяющий необходимость вывода в консоль строк, считанные из которых транспортные средства были отфильтрованы программой;\n");
        }

        //  Вместо данного метода можно было сделать свойство
        /// <summary>
        /// Установка обработчика файлов для данного меню
        /// </summary>
        /// <param name="fileHandler">Обработчик файлов, который нужно прикрепить к объекту Menu</param>
        public void SetFileHandler(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        /// <summary>
        /// Метод, производящий обработку файла, прикреплённого к данному меню, фильтрацию полученного транспорта и сохранение результатов обработки в файл.
        /// </summary>
        /// <param name="outputEncoding">Кодировка, в которой будут сохраняться результаты работы в файлы</param>
        public void ProcessTransportInfo(Encoding outputEncoding)
        {
            //  Проверка на пустоту файла
            if (_fileHandler.CheckFile())
            {
                string[] temp;
                double n, m;
                // Ввод значений N и M в консоль
                do
                {
                    Console.WriteLine("Введите значения N и M (вещественные числа) в одной строке через пробел (вещественные числа должны вводиться с учётом региональных настроек платформы):");
                    temp = Console.ReadLine()?.Split();

                } while (temp?.Length != 2 || !double.TryParse(temp[0], out n) ||
                         !double.TryParse(temp[1], out m));
                        
                List<Car> filteredCars;
                List<Motorcycle> filteredMotorcycles;
                if (_fileHandler.ProcessFile(n, m, out filteredCars, out filteredMotorcycles, _showInvalidVehicles, _showUnprocessedLines))
                {
                    //  Альтернативная реализация: пользователь вводит имена файлов в которые нужно сохранять результаты работы программы
                    FileHandler.WriteLines("FilteredCars.txt", filteredCars.Select(x => x.ToString()).ToArray(), outputEncoding);
                    FileHandler.WriteLines("FilteredMotorcycles.txt", filteredMotorcycles.Select(x => x.ToString()).ToArray(), outputEncoding);
                    Console.WriteLine("Данные успешно обработаны. Результаты обработки записаны в файлы FilteredCars.txt и FilteredMotorcycles.txt");
                }
                else
                {
                    Console.WriteLine("Данные не подходят для обработки.");
                }

            }
        }

        /// <summary>
        /// Метод, изменяющий параметр, который отвечает за отображение в консоли строк файла, которые представлены в неправильном формате и не подлежат обработке
        /// </summary>
        public void ChangeUnprocessedLines()
        {
            Console.WriteLine("Введите y/n; y - программа выводит номера строк, которые не подлежат обработке; n - программа не выводит такие строки");
            string input = Console.ReadLine();
            if (input == "y")
            {
                _showUnprocessedLines = true;
                Console.WriteLine("Параметр успешно изменён.");
            }
            else if (input == "n")
            {
                _showUnprocessedLines = false;
                Console.WriteLine("Параметр успешно изменён.");
            }
            else
                Console.WriteLine("Вводимое значение должно быть одним из символов y или n.");
        }

        /// <summary>
        /// Метод, изменяющий параметр, который отвечает за отображение в консоли строк файла, которым соответствуют транспортные средства с недопустимыми параметрами
        /// </summary>
        public void ChangeInvalidVehicles()
        {
            Console.WriteLine("Введите y/n; y - программа выводит номера строк, транспортные средства в которых были отсортированы программой; n - программа не выводит такие строки");
            string ans = Console.ReadLine();
            if (ans == "y")
            {
                _showInvalidVehicles = true;
                Console.WriteLine("Параметр успешно изменён.");
            }
            else if (ans == "n")
            {
                _showInvalidVehicles = false;
                Console.WriteLine("Параметр успешно изменён.");
            }
            else
                Console.WriteLine("Вводимое значение должно быть одним их символов y или n");
        }
    }
}