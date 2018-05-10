namespace HomeTask12
{
    using HtmlDownLoader;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using HtmlDownLoader.Constraints;

    internal class Program
    {
        private const string DefaultUrl = "https://www.google.kz/";
        private static readonly string DefaultPath = Path.Combine(Environment.CurrentDirectory, "Sites");

        static void Main(string[] args)
        {
            var url = AskUrl();
            var path = AskPath();
            var detph = AskDetph();
            var extentions = new List<string> {"jpg", "jpeg", "png", "bmp", "css", "js"};
            var downloader = new DownloadCore(url, path, detph, DomainWidth.Unlimited, extentions);
            try
            {
                downloader.Process();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            Console.ReadLine();
        }

        private static string AskUrl()
        {
            Console.Write("Enter url please: ");
            var url = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(url))
                url = DefaultUrl;
            Console.WriteLine(url);
            return url;
        }

        private static string AskPath()
        {
            Console.Write("Enter please path to copy whole content: ");
            var path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
                path = DefaultPath;
            Console.WriteLine(path);
            return path;
        }

        private static int AskDetph()
        {
            Console.Write("Enter please depth of nested links: ");
            var depth = 0;
            try
            {
                depth = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(depth);
            return depth;
        }
    }
}
