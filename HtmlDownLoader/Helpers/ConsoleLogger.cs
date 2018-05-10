namespace HtmlDownLoader.Helpers
{
    using System;

    public class ConsoleLogger
    {
        private static ConsoleLogger _instance;

        public static ConsoleLogger CreateInstance()
        {
            return _instance ?? (_instance = new ConsoleLogger());
        }

        public void LogError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            SetConsoleToDefaultColor();
        }

        public void LogResult(string resultMessage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(resultMessage);
            SetConsoleToDefaultColor();
        }

        public void LogProcess(string processMessage)
        {
            SetConsoleToDefaultColor();
            Console.WriteLine(processMessage);
        }

        private ConsoleLogger()
        {
            SetConsoleToDefaultColor();
        }

        private void SetConsoleToDefaultColor()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}