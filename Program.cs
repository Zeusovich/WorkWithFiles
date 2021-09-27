using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Homework_06
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            /// Домашнее задание
            ///
            /// Группа начинающих программистов решила поучаствовать в хакатоне с целью демонстрации
            /// своих навыков. 
            /// 
            /// Немного подумав они вспомнили, что не так давно на занятиях по математике
            /// они проходили тему "свойства делимости целых чисел". На этом занятии преподаватель показывал
            /// пример с использованием фактов делимости. 
            /// Пример заключался в следующем: 
            /// Написав на доске все числа от 1 до N, N = 50, преподаватель разделил числа на несколько групп
            /// так, что если одно число делится на другое, то эти числа попадают в разные руппы. 
            /// В результате этого разбиения получилось M групп, для N = 50, M = 6
            /// 
            /// N = 50
            /// Группы получились такими: 
            /// 
            /// Группа 1: 1
            /// Группа 2: 2 3 5 117  13 17 19 23 29 31 37 41 43 47
            /// Группа 3: 4 6 9 10 14 15 21 22 25 26 33 34 35 38 39 46 49
            /// Группа 4: 8 12 18 20 27 28 30 42 44 45 50
            /// Группа 5: 16 24 36 40
            /// Группа 6: 32 48
            /// 
            /// M = 6
            /// 
            /// ===========
            /// 
            /// N = 10
            /// Группы получились такими: 
            /// 
            /// Группа 1: 1
            /// Группа 2: 2 7 9
            /// Группа 3: 3 4 10
            /// Группа 4: 5 6 8
            /// 
            /// M = 4
            /// 
            /// Участники хакатона решили эту задачу, добавив в неё следующие возможности:
            /// 1. Программа считыват из файла (путь к которому можно указать) некоторое N, 
            ///    для которого нужно подсчитать количество групп
            ///    Программа работает с числами N не превосходящими 1 000 000 000
            ///   
            /// 2. В ней есть два режима работы:
            ///   2.1. Первый - в консоли показывается только количество групп, т е значение M
            ///   2.2. Второй - программа получает заполненные группы и записывает их в файл используя один из
            ///                 вариантов работы с файлами
            ///            
            /// 3. После выполения пунктов 2.1 или 2.2 в консоли отображается время, за которое был выдан результат 
            ///    в секундах и миллисекундах
            /// 
            /// 4. После выполнения пунта 2.2 программа предлагает заархивировать данные и если пользователь соглашается -
            /// делает это.
            /// 
            /// Попробуйте составить конкуренцию начинающим программистам и решить предложенную задачу
            /// (добавление новых возможностей не возбраняется)
            ///
            /// * При выполнении текущего задания, необходимо документировать код 
            ///   Как пометками, так и xml документацией
            ///   В обязательном порядке создать несколько собственных методов*/

            int N ;
            DateTime date = new DateTime();
            
            //получаем кол-во групп
            static int GetM(int Number)
            {
                int k = 1;
                int M = 0;
                
                while (k<=Number)
                {
                    M++;
                    k *= 2;
                }

                return M;
            }

            //массив чисел от 1 до N
            static void GetMas(int A)
            {
                int[] mas = new int[A];
                int flag = 0;
                string text = "";
                for (int i = A/2; i < A; i++)
                {
                    mas[i] = i + 1;
                    string group = Convert.ToString(mas[i]);
                    text = text + " " + group;
                }

                int groupNumber = GetM(A);
                text = "Группа "+ groupNumber + ":" + text;
                using (StreamWriter streamWriter = new StreamWriter(@"E:\file.txt", true))
                {
                    streamWriter.Write(text); // запись строки в файл
                    streamWriter.WriteLine();
                }
            }
            
            using (FileStream fileStream = File.OpenRead(@"E:\file.txt"))
            {
                  byte[] array = new byte[fileStream.Length]; 
                  // преобразуем строку в байты
                  fileStream.Read(array, 0, array.Length);       // считываем данные

                  string text = Encoding.Default.GetString(array);    // декодируем байты в строку
                  
                  N = Convert.ToInt32(text);
                  
            }
            Console.WriteLine($"Number : {N}");


            Console.WriteLine(" Выберите режим работы ");
            int oneOfCase = Convert.ToInt32(Console.ReadLine());
            
            if (N<=1_000_000_000)
            {
                switch (oneOfCase)
                {
                    case 1: 
                        date = DateTime.Now;
                        Console.WriteLine($" Кол-во групп {GetM(N)}");
                        TimeSpan timeSpan1 = DateTime.Now.Subtract(date);
                        Console.WriteLine($"timeSpan.TotalMilliseconds = {timeSpan1.TotalMilliseconds}");
                        break;
                    
                    case 2:
                        date = DateTime.Now;
                        Sort(N);
                        TimeSpan timeSpan2 = DateTime.Now.Subtract(date);
                        Console.WriteLine($"timeSpan.TotalMilliseconds = {timeSpan2.TotalMilliseconds}");
                        break;
                    
                    case 3:
                        Sort(N);
                        string source = (@"E:\file.txt");
                        string compressed = (@"E:\file.zip");

                        using (FileStream ss = new FileStream(source, FileMode.OpenOrCreate))
                        {
                            using (FileStream ts = File.Create(compressed))   // поток для записи сжатого файла
                            {
                                // поток архивации
                                using (GZipStream cs = new GZipStream(ts, CompressionMode.Compress))
                                {
                                    ss.CopyTo(cs); // копируем байты из одного потока в другой
                                    Console.WriteLine("Сжатие файла {0} завершено. Было: {1}  стало: {2}.",
                                        source,
                                        ss.Length,
                                        ts.Length);
                                }
                            }
                        }
                        break;
                    }
            }
            
            // заполняем группы
            static void Sort(int A)
            {
                int k = GetM(A);

                while (k>0)
                {
                    GetMas(A);
                    k--;
                    A = A / 2;
                }
            }
        }
    }
}
