namespace CheckLiveInsta
{
    public class ConsoleExtension
    {
        private static readonly object _locker = new();

        public static void WriteError(string message)
        {
            lock (_locker)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
                Console.WriteLine(">===================<");
            }
        }

        public static void WriteSuccess(string message)
        {
            lock (_locker)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ResetColor();
                Console.WriteLine(">===================<");
            }
        }

        public static void WriteInfo(string message)
        {
            lock (_locker)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message);
                Console.ResetColor();
                Console.WriteLine(">===================<");
            }
        }

        public static void WriteInfo(int account, int total, int live, int die, int error)
        {
            lock (_locker)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Total account: {account}");
                Console.WriteLine($"Total scanned: {total}");
                Console.WriteLine($"Live: {live}");
                Console.WriteLine($"Die: {die}");
                Console.WriteLine($"Error: {error}");
                Console.ResetColor();
            }
        }
    }
}
