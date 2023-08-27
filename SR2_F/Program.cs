using System;
using System.Text;
using Utilities;

namespace SR2_F
{
    class Program
    {
        public static void Main(string[] args)
        {
            //  Установка набора кодировок для текущей системы
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Menu menu = new Menu();
            bool requestsActive = true;
            
            // Цикл обработки команд пользователя
            while (requestsActive)
            {
                menu.ShowOptions();
                string? command = Console.ReadLine();
                if (!int.TryParse(command, out int commandIndex))
                {
                    Console.WriteLine("Команда должна быть целым числом из следующего набора {0, 1, 2, 3}");
                    continue;
                }
                //  Обработка введённой команды
                switch (commandIndex)
                {
                    case 0:
                        requestsActive = false;
                        break;
                    case 1:
                        try
                        {
                            FileHandler fh = FileHandler.GetFileHandler();
                            //  Можно ускорить работу программы при работе с одним и тем же файлом
                            //  Для этого вместо считывания файла каждый раз при вызове команды обработки можно сохранять сведения об объектах транспорта в List и иcпользовать их в дальнейшем

                            menu.SetFileHandler(fh);
                            menu.ProcessTransportInfo(Encoding.GetEncoding(1251));
                        }
                        catch (NotSupportedException)
                        {
                            Console.WriteLine("Кодировка не поддерживается системой.");
                            throw;
                        }

                        break;
                    case 2:
                        menu.ChangeUnprocessedLines();
                        break;
                    case 3:
                        menu.ChangeInvalidVehicles();
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
                Console.WriteLine("_______________________________________________________\n");
            }
        }
    }
}