using Serilog;

namespace CheckLiveInsta
{
    public class FileUtil
    {
        private static StreamWriter _liveWriter = null!;
        private static StreamWriter _dieWriter = null!;
        private static StreamWriter _errorWriter = null!;
        private static StreamWriter _generalWriter = null!;

        public static void Init(DateTime session)
        {
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var subFolder = Path.Combine(folder, session.ToString("yyyyMMdd.HHmmss.fff"));
            if (!Directory.Exists(subFolder)) Directory.CreateDirectory(subFolder);

            _liveWriter = new StreamWriter(Path.Combine(subFolder, "live.txt"), true);
            _dieWriter = new StreamWriter(Path.Combine(subFolder, "die.txt"), true);
            _errorWriter = new StreamWriter(Path.Combine(subFolder, "error.txt"), true);
            _generalWriter = new StreamWriter(Path.Combine(subFolder, "general.txt"), true);
        }

        public static void WriteDie(Tuple<string, string> account)
        {
            try
            {
                lock (_dieWriter)
                {
                    _dieWriter.WriteLine(account.Item1 + "|" + account.Item2);
                }
            }
            catch { }
        }

        public static void WriteLive(Tuple<string, string> account)
        {
            try
            {
                lock (_liveWriter)
                {
                    _liveWriter.WriteLine(account.Item1 + "|" + account.Item2);
                }
            }
            catch { }
        }

        public static void WriteGeneral(Tuple<string, string> account, bool? success)
        {
            try
            {
                var info = success switch
                {
                    true => "OK",
                    false => "Die",
                    _ => "Error"
                };
                lock (_generalWriter)
                {
                    _generalWriter.WriteLine(account.Item1 + "|" + account.Item2 + "|" + info);
                }
            }
            catch { }
        }

        public static void WriteError(Tuple<string, string> account)
        {
            try
            {
                lock (_errorWriter)
                {
                    _errorWriter.WriteLine(account.Item1 + "|" + account.Item2);
                }
            }
            catch { }
        }

        public static void Close()
        {
            _dieWriter.Flush();
            _liveWriter.Flush();
            _errorWriter.Flush();
            _generalWriter.Flush();

            _dieWriter.Close();
            _liveWriter.Close();
            _errorWriter.Close();
            _generalWriter.Close();
        }

        public static List<Account> ReadAccount(string path)
        {
            var accounts = new List<Account>();
            try
            {
                using var reader = new StreamReader(path);
                var line = reader.ReadLine();
                while (line != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        line = reader.ReadLine();
                        continue;
                    }
                    var data = line.Split("|");
                    var account = new Account
                    {
                        Username = data[0],
                        Password = data[1],
                        CheckCount = 0,
                        Status = LoginStatus.None,
                        ErrorCount = 0,
                    };
                    accounts.Add(account);
                    line = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Read data error {ex}");
            }
            return accounts;
        }

        public static List<Tuple<string, string>> ReadData(string path)
        {
            var accounts = new List<Tuple<string, string>>();
            try
            {
                using var reader = new StreamReader(path);
                var line = reader.ReadLine();
                while (line != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        line = reader.ReadLine();
                        continue;
                    }
                    var data = line.Split("|");
                    if (data.Length > 1)
                    {
                        accounts.Add(Tuple.Create(data[0], data[1]));
                    }
                    else
                    {
                        accounts.Add(Tuple.Create(data[0], string.Empty));
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Read data error {ex}");
            }
            return accounts;
        }
    }
}
