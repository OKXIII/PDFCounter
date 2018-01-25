using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using System.Drawing.Imaging;
//using Ghostscript.NET.Rasterizer.GhostscriptRasterizer;

namespace PDFCounter
{
    class Program
    {
        static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("PDFCounter ver 1.1");
            Console.WriteLine("PDFCounter [диск:][путь] [/P] [/S]");
            Console.WriteLine("/P\tОтображать количество страниц в файле.");
            Console.WriteLine("/S\tОтображать размер файла.");
        }

       
        static int Main(string[] args)
        {
            //Анализ командной строки
            int countArgs = args.Count();
            int ret = 0;
            bool pP = false;
            bool pS = false;
            string dir;
            if (countArgs==0) 
            {
                Console.WriteLine("Ошибка в синтаксисе команды. Наберите PDFCounter /? для получения справки.");
                Console.WriteLine("Укажите каталог содержащий *.PDF файлы. ");
                PrintHelp();
                return -1;
            }
            dir=args[0].ToString();
            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Укажите каталог содержащий *.PDF файлы. ");
                PrintHelp();
                return -1;
            }
            for (int i = 1; i < countArgs; i++)
            {
                if (args[i] == "/?") { PrintHelp(); return 0; }
                if (args[i] == "/P" || args[i] == "/p") pP = true;
                if (args[i] == "/S" || args[i] == "/s") pS = true;

            }

            var localGhostscriptDll = Path.Combine(Environment.CurrentDirectory, "gsdll32.dll");
            var localDllInfo = new GhostscriptVersionInfo(localGhostscriptDll);
            GhostscriptRasterizer rasterizer = new GhostscriptRasterizer();

            int pagecount=0;
            //Получение списка файлов PDF в каталоге
            string[] files = Directory.GetFiles(dir, "*.pdf");
              //Получение информации о PDF файле (размер, количество страниц)
            foreach (string filename in files)
            {
                Trace.WriteLine("Обработка файла: " + filename);
                //Сохранение информации в результирующем файле
                Console.Write(filename + "\t");
                if (pP)
                {
                    rasterizer.Open(filename, localDllInfo, false);
                    pagecount = rasterizer.PageCount;
                    Console.Write(pagecount + "\t");
                }
                if (pS)
                {
                    FileInfo f = new FileInfo(filename);
                    Console.Write(f.Length);
                }
                ret++;
                Console.WriteLine();
            }
            Console.WriteLine("Обработано файлов " + ret);
            return ret;
        }
    }
}
