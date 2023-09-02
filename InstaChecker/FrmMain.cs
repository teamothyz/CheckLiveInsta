using CheckLiveInsta;
using ChromeDriverLibrary;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace InstaChecker
{
    public partial class FrmMain : Form
    {
        private readonly CountModel _count = new();
        private readonly List<Tuple<string, string>> _accounts = new();
        private CancellationTokenSource _cancelTokens = new();

        public FrmMain()
        {
            InitializeComponent();

            TotalTxtBox.DataBindings.Add("Text", _count, "Total");
            LiveTxtBox.DataBindings.Add("Text", _count, "Live");
            DieTxtBox.DataBindings.Add("Text", _count, "Die");
            ErrorTxtBox.DataBindings.Add("Text", _count, "Error");
            ScannedTxtBox.DataBindings.Add("Text", _count, "Scanned");

            var config = new ConfigurationBuilder().AddJsonFile("account.config.json").Build();
            UsernameTxtBox.Text = config["Username"] ?? string.Empty;
            PasswordTxtBox.Text = config["Password"] ?? string.Empty;
            ActiveControl = kryptonLabel1;
        }

        private void InputBtn_Click(object sender, EventArgs e)
        {
            ActiveControl = kryptonLabel1;
            var dialog = new OpenFileDialog();
            var rs = dialog.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                _ = Task.Run(() =>
                {
                    _accounts.Clear();
                    var accounts = FileUtil.ReadData(dialog.FileName);
                    _accounts.AddRange(accounts);
                    Invoke(() =>
                    {
                        MessageBox.Show(this, "Load dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _count.Total = _accounts.Count;
                        _count.Live = 0;
                        _count.Die = 0;
                        _count.Error = 0;
                        _count.Scanned = 0;
                    });
                });
            }
        }

        private async void StartBtn_Click(object sender, EventArgs e)
        {
            ActiveControl = kryptonLabel1;
            try
            {
                EnableBtn(false);
                if (string.IsNullOrEmpty(UsernameTxtBox.Text) || string.IsNullOrEmpty(PasswordTxtBox.Text))
                {
                    MessageBox.Show(this, "Vui lòng nhập tài khoản và mật khẩu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!_accounts.Any())
                {
                    MessageBox.Show(this, "Vui lòng nhập danh sách tài khoản cần kiểm tra", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Invoke(() =>
                {
                    _count.Live = 0;
                    _count.Die = 0;
                    _count.Error = 0;
                    _count.Scanned = 0;
                });

                _cancelTokens = new();
                await Task.Run(() =>
                {
                    var headers = InstaRequestService.LoginAndGetHeaders(UsernameTxtBox.Text, PasswordTxtBox.Text, _cancelTokens.Token);
                    if (!headers.Any())
                    {
                        Invoke(() =>
                        {
                            MessageBox.Show(this, "Đăng nhập thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        });
                    }
                    CheckAccountLoop(headers, _cancelTokens.Token);
                });
            }
            catch (Exception ex)
            {
                Invoke(() => MessageBox.Show(this, $"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error));
                Log.Error($"Start error: {ex}");
            }
            finally
            {
                Invoke(() => MessageBox.Show(this, "Chương trình đã dừng lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information));
                EnableBtn(true);
            }
        }

        public void CheckAccountLoop(Dictionary<string, string> headers, CancellationToken token)
        {
            try
            {
                var countLocker = new object();
                var count = 0;
                FileUtil.Init();

                var tasks = new List<Task>();
                for (int i = 0; i < (int)ThreadUpDown.Value; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        while (!token.IsCancellationRequested)
                        {
                            Tuple<string, string> account = null!;
                            lock (countLocker)
                            {
                                if (count >= _accounts.Count) return;
                                account = _accounts[count];
                                count++;
                                if (account == null) return;
                            }
                            var success = InstaRequestService.CheckAccount(account.Item1, headers, token).Result;
                            FileUtil.WriteGeneral(account, success);

                            switch (success)
                            {
                                case true:
                                    lock (_count.Locker)
                                    {
                                        Invoke(() =>
                                        {
                                            _count.Live++;
                                            _count.Scanned++;
                                        });
                                    }
                                    FileUtil.WriteLive(account);
                                    break;
                                case false:
                                    lock (_count.Locker)
                                    {
                                        Invoke(() =>
                                        {
                                            _count.Die++;
                                            _count.Scanned++;
                                        });
                                    }
                                    FileUtil.WriteDie(account);
                                    break;
                                default:
                                    lock (_count.Locker)
                                    {
                                        Invoke(() =>
                                        {
                                            _count.Error++;
                                            _count.Scanned++;
                                        });
                                    }
                                    FileUtil.WriteError(account);
                                    break;
                            }
                            Thread.Sleep(100);
                        }
                    }, token));
                }
                Task.WaitAll(tasks.ToArray(), token);
            }
            catch (Exception ex)
            {
                Log.Error($"Loop checking account error: {ex}");
            }
            finally
            {
                FileUtil.Close();
            }
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            ActiveControl = kryptonLabel1;
            _cancelTokens.Cancel();
            ChromeDriverInstance.KillAllChromes();
        }

        private void EnableBtn(bool enable)
        {
            Invoke(() =>
            {
                UsernameTxtBox.ReadOnly = !enable;
                PasswordTxtBox.ReadOnly = !enable;

                ThreadUpDown.Enabled = enable;
                InputBtn.Enabled = enable;
                StartBtn.Enabled = enable;
                StopBtn.Enabled = !enable;

                StatusTxtBox.Text = enable ? "Đã dừng" : "Đang chạy";
                StatusTxtBox.StateCommon.Back.Color1 = enable ? Color.Red : Color.Green;
            });
        }
    }
}