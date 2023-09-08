using CheckLiveInsta;
using ChromeDriverLibrary;
using Serilog;
using System.ComponentModel;

namespace InstaChecker
{
    public partial class FrmMain : Form
    {
        private readonly CountModel _countModel = new();
        private readonly List<Tuple<string, string>> _checkingAccounts = new();
        private readonly BindingList<Account> _accounts = new();
        private int _countCheck = 0;
        private readonly object _countCheckLocker = new();
        private DateTime _session = DateTime.Now;

        private CancellationTokenSource _cancelTokens = new();

        public FrmMain()
        {
            InitializeComponent();

            TotalTxtBox.DataBindings.Add("Text", _countModel, "Total");
            LiveTxtBox.DataBindings.Add("Text", _countModel, "Live");
            DieTxtBox.DataBindings.Add("Text", _countModel, "Die");
            ErrorTxtBox.DataBindings.Add("Text", _countModel, "Error");
            ScannedTxtBox.DataBindings.Add("Text", _countModel, "Scanned");

            AccountGridView.AutoGenerateColumns = false;
            AccountGridView.DataSource = _accounts;
            ActiveControl = kryptonLabel3;
        }

        private void InputBtn_Click(object sender, EventArgs e)
        {
            ActiveControl = kryptonLabel3;
            var dialog = new OpenFileDialog();
            var rs = dialog.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                _ = Task.Run(() =>
                {
                    _checkingAccounts.Clear();
                    var accounts = FileUtil.ReadData(dialog.FileName);
                    _checkingAccounts.AddRange(accounts);
                    Invoke(() =>
                    {
                        MessageBox.Show(this, "Load dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _countModel.Total = _checkingAccounts.Count;
                        _countModel.Live = 0;
                        _countModel.Die = 0;
                        _countModel.Error = 0;
                        _countModel.Scanned = 0;
                        _countCheck = 0;
                        _session = DateTime.Now;
                    });
                });
            }
        }

        private async void StartBtn_Click(object sender, EventArgs e)
        {
            ActiveControl = kryptonLabel3;
            try
            {
                EnableBtn(false);
                if (!_accounts.Any())
                {
                    MessageBox.Show(this, "Vui lòng nhập tài khoản và mật khẩu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!_checkingAccounts.Any())
                {
                    MessageBox.Show(this, "Vui lòng nhập danh sách tài khoản cần kiểm tra", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Invoke(() =>
                {
                    _countModel.Live = 0;
                    _countModel.Die = 0;
                    _countModel.Error = 0;
                    _countModel.Scanned = 0;
                });

                _cancelTokens = new();
                _session = DateTime.Now;
                _countCheck = 0;
                await Task.Run(async () => await CheckAccountLoop(_cancelTokens.Token));
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

        public async Task CheckAccountLoop(CancellationToken token)
        {
            try
            {
                var tasks = new List<Task>();
                foreach (var account in _accounts)
                {
                    if (token.IsCancellationRequested) break;
                    if (account.Status != LoginStatus.Success && account.Status != LoginStatus.Die)
                    {
                        var task = Task.Run(() => InstaRequestService.LoginAndGetHeaders(account, token), token);
                        tasks.Add(task);
                        if (tasks.Count == (int)LoginThreadUpDown.Value)
                        {
                            await Task.WhenAll(tasks);
                            tasks.Clear();
                            ChromeDriverInstance.KillAllChromes();
                        }
                    }
                }

                if (tasks.Any())
                {
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                    ChromeDriverInstance.KillAllChromes();
                }
                //FileUtil.WriteData(_session, _accounts.Where(acc => acc.Status == LoginStatus.Success).ToList());
                FileUtil.Init(_session);
                for (int i = 0; i < (int)ThreadUpDown.Value; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        while (!token.IsCancellationRequested)
                        {
                            Tuple<string, string> checkAccount = null!;
                            Account? account = null;

                            lock (_countCheckLocker)
                            {
                                if (_countCheck >= _checkingAccounts.Count) return;
                                checkAccount = _checkingAccounts[_countCheck];

                                account = _accounts.Where(a => a.CheckCount < 450 && a.Status == LoginStatus.Success).OrderBy(a => a.Username).FirstOrDefault();
                                if (account != null) account.CheckCount++;
                                else return;

                                _countCheck++;
                            }

                            if (checkAccount == null || account == null) return;
                            var success = InstaRequestService.CheckAccount(checkAccount.Item1, account.Headers, token).Result;
                            FileUtil.WriteGeneral(checkAccount, success);

                            switch (success)
                            {
                                case true:
                                    lock (_countModel.Locker)
                                    {
                                        Invoke(() =>
                                        {
                                            _countModel.Live++;
                                            _countModel.Scanned++;
                                        });
                                    }
                                    FileUtil.WriteLive(checkAccount);
                                    break;
                                case false:
                                    lock (_countModel.Locker)
                                    {
                                        Invoke(() =>
                                        {
                                            _countModel.Die++;
                                            _countModel.Scanned++;
                                        });
                                    }
                                    FileUtil.WriteDie(checkAccount);
                                    break;
                                default:
                                    lock (_countModel.Locker)
                                    {
                                        Invoke(() =>
                                        {
                                            _countModel.Error++;
                                            _countModel.Scanned++;
                                            account.ErrorCount++;
                                        });
                                    }
                                    FileUtil.WriteError(checkAccount);
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
            ActiveControl = kryptonLabel3;
            _cancelTokens.Cancel();
            ChromeDriverInstance.KillAllChromes();
        }

        private void EnableBtn(bool enable)
        {
            Invoke(() =>
            {
                ThreadUpDown.Enabled = enable;
                InputBtn.Enabled = enable;
                StartBtn.Enabled = enable;
                ContinueBtn.Enabled = enable;
                LoginThreadUpDown.Enabled = enable;
                ConfigAccountBtn.Enabled = enable;

                StopBtn.Enabled = !enable;

                StatusTxtBox.Text = enable ? "Đã dừng" : "Đang chạy";
                StatusTxtBox.StateCommon.Back.Color1 = enable ? Color.Red : Color.Green;
            });
        }

        private async void ContinueBtn_Click(object sender, EventArgs e)
        {
            ActiveControl = kryptonLabel3;
            try
            {
                EnableBtn(false);
                if (!_accounts.Any())
                {
                    MessageBox.Show(this, "Vui lòng nhập tài khoản và mật khẩu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!_checkingAccounts.Any())
                {
                    MessageBox.Show(this, "Vui lòng nhập danh sách tài khoản cần kiểm tra", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _cancelTokens = new();
                await Task.Run(() => CheckAccountLoop(_cancelTokens.Token));
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

        private void ConfigAccountBtn_Click(object sender, EventArgs e)
        {
            ActiveControl = kryptonLabel3;
            var dialog = new OpenFileDialog();
            var rs = dialog.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                _ = Task.Run(() =>
                {
                    Invoke(() => _accounts.Clear());
                    var accounts = FileUtil.ReadAccount(dialog.FileName);
                    foreach (var account in accounts.OrderBy(a => a.Username))
                    {
                        Invoke(() => _accounts.Add(account));
                    }
                    Invoke(() =>
                    {
                        MessageBox.Show(this, "Load accounts thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                });
            }
        }
    }
}