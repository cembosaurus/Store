using Business.Enums;

namespace Business.Tools
{
    public class ConsoleWriter
    {
        private string? _header;
        private string? _subject;
        private string? _description;
        private TypeOfInfo _type;
        private string? _message;


        public void Message(string header = "", string subject = "", string description = "", TypeOfInfo type = TypeOfInfo.INFO, string message = "")
        {
            _header = header;
            _subject = subject;
            _description = description;
            _type = type;
            _message = message;

            Write();
        }

        public void Text(ConsoleColor backGround1, ConsoleColor foreGround1, string text1, 
            ConsoleColor backGround2 = ConsoleColor.Black, ConsoleColor foreGround2 = ConsoleColor.White, string text2 = "", 
            ConsoleColor backGround3 = ConsoleColor.Black, ConsoleColor foreGround3 = ConsoleColor.White, string text3 = "", 
            ConsoleColor backGround4 = ConsoleColor.Black, ConsoleColor foreGround4 = ConsoleColor.White, string text4 = "", 
            ConsoleColor backGround5 = ConsoleColor.Black, ConsoleColor foreGround5 = ConsoleColor.White, string text5 = "")
        {
            Console.BackgroundColor = backGround1;
            Console.ForegroundColor = foreGround1;
            Console.Write(text1);
            Console.BackgroundColor = backGround2;
            Console.ForegroundColor = foreGround2;
            Console.Write(text2);
            Console.BackgroundColor = backGround3;
            Console.ForegroundColor = foreGround3;
            Console.Write(text3);
            Console.BackgroundColor = backGround4;
            Console.ForegroundColor = foreGround4;
            Console.Write(text4);
            Console.BackgroundColor = backGround5;
            Console.ForegroundColor = foreGround5;
            Console.Write(text5);
            Console.ResetColor();

            Console.WriteLine();
        }



        private void Write()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{_header}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($":");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($" {_subject}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($":");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" {_description}");
            Console.ForegroundColor = _type switch
            {
                TypeOfInfo.FAIL => ConsoleColor.Red,
                TypeOfInfo.SUCCESS => ConsoleColor.DarkYellow,
                TypeOfInfo.INFO => ConsoleColor.Green,
                TypeOfInfo.WARNING => ConsoleColor.DarkMagenta,
                _ => ConsoleColor.White
            };
            Console.Write($" {_type}:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.IsNullOrWhiteSpace(_message) ? "" : $" {_message}");
            Console.ResetColor();
        }

    }
}
