using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Work15
{
    class Program
    {
        static void Main(string[] args)
        {
            string num = "Pause!!";
            TimerCallback time = new TimerCallback(Pause);
            Timer timer = new Timer(time, num, 0, 30000);
            Console.WriteLine();

            Console.WriteLine("\0Информация о процессе:");

            try
            {
                foreach (Process process in Process.GetProcesses())
                {
                    Console.WriteLine($"\0Id:\0{process.Id};\0имя:\0{process.ProcessName};\0времечко:\0{process.StartTime};\0приоритет:\0{process.PriorityClass}");
                }
            }
            

            catch(System.ComponentModel.Win32Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine();


            Console.WriteLine("\0Информация о домене:");
            AppDomain domain = AppDomain.CurrentDomain;

            Console.WriteLine($"\0Имя:\0{domain.FriendlyName};\0сборки:\0{domain.GetAssemblies()};\0детали конфигурации:\0{domain.SetupInformation}");

            AppDomain secondDomain = AppDomain.CreateDomain("\0Создание второго домена:");
            secondDomain.AssemblyLoad += Load;
            secondDomain.DomainUnload += UnLoad;

            Console.WriteLine($"\0Созданный домен:\0{secondDomain.FriendlyName}");
            //secondDomain.Load(new AssemblyName("System.Data"));

            AppDomain.Unload(secondDomain);//выгрузка
            Console.WriteLine();


            Console.WriteLine("\0Работа с задачей расчёта:");
            Console.Write("\0Введите число, до которого будет идти счёт:\0");
            int n = Convert.ToInt32(Console.ReadLine());

            Thread thread = new Thread(new ParameterizedThreadStart(Output));
            thread.Start(n);

            Console.WriteLine($"\0Статус потока:\0{thread.ThreadState}");
            Thread.Sleep(200);

            thread.Join();
            Console.WriteLine($"\0Статус потока:\0{thread.ThreadState}");
            Console.WriteLine();

            Console.WriteLine("\0Работа с двумя потоками:");

            Console.WriteLine("\0Раздельный вывод чисел:");
            Thread firstThread = new Thread(Count1);
            Thread secondThread = new Thread(Count2);

            firstThread.Priority = ThreadPriority.Highest;

            firstThread.Start(n);
            secondThread.Start(n);

            firstThread.Join();
            secondThread.Join();
            Console.WriteLine();

            Console.WriteLine("\0Переменный вывод чисел:");
            for(int i=1; i<=n;i++)
            {
                if(i%2==0)
                {
                    Thread thirdThread = new Thread(Count3);
                    thirdThread.Start();
                }

                else
                {
                    Thread fourthThread = new Thread(Count4);
                    fourthThread.Start();
                }
            }               
               
            
        }

        private static void UnLoad(object sender, EventArgs args)
        {
            Console.WriteLine("\0Домен выгружен!");
        }

        private static void Load(object sender, AssemblyLoadEventArgs args)
        {
            Console.WriteLine("\0Сборка загружена!");
        }

        public static void Output(object number)
        {
            StreamWriter writer = new StreamWriter("F:\\15.txt");

            for(int i=1;i<=(int)number;i++)
            {
                writer.Write($"\0{i}");
                Console.Write($"\0{i}");
                Thread.Sleep(2000);
            }
        }
             
        static string objlocker = "null";

        public static void Count1(object x)
        {
            try
            {
                Monitor.Enter(objlocker);
                StreamWriter writer = new StreamWriter("F:\\15(4).txt", true);

                for(int i=0;i<=5;i+=2)
                {
                    writer.WriteLine(i);
                    Console.WriteLine("Чётное число: " + i);
                    Thread.Sleep(1000);
                }
                
                writer.Close();
            }
            finally
            {
                Monitor.Exit(objlocker);
            }
        }
        public static void Count2(object x)
        {
            try
            {
                Monitor.Enter(objlocker);
                StreamWriter writer = new StreamWriter("F:\\15(4).txt", true);

                for (int i = 1; i <= 5; i+=2)
                {
                    writer.WriteLine(i);
                    Console.WriteLine("Нечётное число: " + i);
                    Thread.Sleep(2000);
                }

                
                writer.Close();
            }

            finally
            {
                Monitor.Exit(objlocker);
            }
        }

        static int i1 = 2; static int i2 = 1;
        public static void Count3(object x)
        {
            try
            {
                Monitor.Enter(objlocker);
                StreamWriter writer = new StreamWriter("F:\\15(4-2).txt", true);
                writer.WriteLine(i1);
                Console.WriteLine("Чётное число: " + i1);
                i1 += 2;
                Thread.Sleep(1000);
                writer.Close();
            }
            finally
            {
                Monitor.Exit(objlocker);
            }
        }
        public static void Count4(object x)
        {
            try
            {
                Monitor.Enter(objlocker);
                StreamWriter writer = new StreamWriter("F:\\15(4-2).txt", true);
                writer.WriteLine(i2);
                Console.WriteLine("Нечётное число: " + i2);
                i2 += 2;
                Thread.Sleep(2000);
                writer.Close();
            }
            finally
            {
                Monitor.Exit(objlocker);
            }
        }

        public static void Pause(object obj)
        {
            string x = (string)obj;

            Console.WriteLine(x);
        }
    }
}
