namespace InstaChecker
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            kryptonLabel5 = new Krypton.Toolkit.KryptonLabel();
            kryptonLabel6 = new Krypton.Toolkit.KryptonLabel();
            kryptonLabel7 = new Krypton.Toolkit.KryptonLabel();
            kryptonLabel8 = new Krypton.Toolkit.KryptonLabel();
            kryptonLabel9 = new Krypton.Toolkit.KryptonLabel();
            TotalTxtBox = new Krypton.Toolkit.KryptonTextBox();
            LiveTxtBox = new Krypton.Toolkit.KryptonTextBox();
            DieTxtBox = new Krypton.Toolkit.KryptonTextBox();
            ErrorTxtBox = new Krypton.Toolkit.KryptonTextBox();
            ScannedTxtBox = new Krypton.Toolkit.KryptonTextBox();
            StatusTxtBox = new Krypton.Toolkit.KryptonTextBox();
            ThreadUpDown = new Krypton.Toolkit.KryptonNumericUpDown();
            InputBtn = new Krypton.Toolkit.KryptonButton();
            StartBtn = new Krypton.Toolkit.KryptonButton();
            StopBtn = new Krypton.Toolkit.KryptonButton();
            ContinueBtn = new Krypton.Toolkit.KryptonButton();
            AccountGridView = new Krypton.Toolkit.KryptonDataGridView();
            Username = new Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            Password = new Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            CheckCount = new Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            ErrorCount = new Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            Status = new Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            ConfigAccountBtn = new Krypton.Toolkit.KryptonButton();
            LoginThreadUpDown = new Krypton.Toolkit.KryptonNumericUpDown();
            kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)AccountGridView).BeginInit();
            SuspendLayout();
            // 
            // kryptonLabel3
            // 
            kryptonLabel3.Location = new Point(651, 12);
            kryptonLabel3.Name = "kryptonLabel3";
            kryptonLabel3.Size = new Size(61, 19);
            kryptonLabel3.StateCommon.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            kryptonLabel3.TabIndex = 2;
            kryptonLabel3.Values.Text = "Số luồng:";
            // 
            // kryptonLabel4
            // 
            kryptonLabel4.Location = new Point(651, 47);
            kryptonLabel4.Name = "kryptonLabel4";
            kryptonLabel4.Size = new Size(41, 19);
            kryptonLabel4.StateCommon.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            kryptonLabel4.TabIndex = 3;
            kryptonLabel4.Values.Text = "Tổng:";
            // 
            // kryptonLabel5
            // 
            kryptonLabel5.Location = new Point(651, 82);
            kryptonLabel5.Name = "kryptonLabel5";
            kryptonLabel5.Size = new Size(35, 19);
            kryptonLabel5.StateCommon.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            kryptonLabel5.TabIndex = 4;
            kryptonLabel5.Values.Text = "Live:";
            // 
            // kryptonLabel6
            // 
            kryptonLabel6.Location = new Point(651, 121);
            kryptonLabel6.Name = "kryptonLabel6";
            kryptonLabel6.Size = new Size(31, 19);
            kryptonLabel6.StateCommon.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            kryptonLabel6.TabIndex = 5;
            kryptonLabel6.Values.Text = "Die:";
            // 
            // kryptonLabel7
            // 
            kryptonLabel7.Location = new Point(651, 156);
            kryptonLabel7.Name = "kryptonLabel7";
            kryptonLabel7.Size = new Size(29, 19);
            kryptonLabel7.StateCommon.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            kryptonLabel7.TabIndex = 6;
            kryptonLabel7.Values.Text = "Lỗi:";
            // 
            // kryptonLabel8
            // 
            kryptonLabel8.Location = new Point(651, 195);
            kryptonLabel8.Name = "kryptonLabel8";
            kryptonLabel8.Size = new Size(61, 19);
            kryptonLabel8.StateCommon.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            kryptonLabel8.TabIndex = 7;
            kryptonLabel8.Values.Text = "Đã check:";
            // 
            // kryptonLabel9
            // 
            kryptonLabel9.Location = new Point(651, 236);
            kryptonLabel9.Name = "kryptonLabel9";
            kryptonLabel9.Size = new Size(68, 19);
            kryptonLabel9.StateCommon.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            kryptonLabel9.TabIndex = 8;
            kryptonLabel9.Values.Text = "Trạng thái:";
            // 
            // TotalTxtBox
            // 
            TotalTxtBox.Location = new Point(731, 43);
            TotalTxtBox.Name = "TotalTxtBox";
            TotalTxtBox.ReadOnly = true;
            TotalTxtBox.Size = new Size(104, 22);
            TotalTxtBox.StateCommon.Back.Color1 = Color.FromArgb(255, 255, 128);
            TotalTxtBox.StateCommon.Content.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            TotalTxtBox.TabIndex = 11;
            TotalTxtBox.Text = "0";
            TotalTxtBox.TextAlign = HorizontalAlignment.Center;
            // 
            // LiveTxtBox
            // 
            LiveTxtBox.Location = new Point(731, 79);
            LiveTxtBox.Name = "LiveTxtBox";
            LiveTxtBox.ReadOnly = true;
            LiveTxtBox.Size = new Size(104, 22);
            LiveTxtBox.StateCommon.Back.Color1 = Color.FromArgb(128, 255, 128);
            LiveTxtBox.StateCommon.Content.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LiveTxtBox.TabIndex = 12;
            LiveTxtBox.Text = "0";
            LiveTxtBox.TextAlign = HorizontalAlignment.Center;
            // 
            // DieTxtBox
            // 
            DieTxtBox.Location = new Point(731, 118);
            DieTxtBox.Name = "DieTxtBox";
            DieTxtBox.ReadOnly = true;
            DieTxtBox.Size = new Size(104, 22);
            DieTxtBox.StateCommon.Back.Color1 = Color.FromArgb(255, 128, 128);
            DieTxtBox.StateCommon.Content.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            DieTxtBox.TabIndex = 13;
            DieTxtBox.Text = "0";
            DieTxtBox.TextAlign = HorizontalAlignment.Center;
            // 
            // ErrorTxtBox
            // 
            ErrorTxtBox.Location = new Point(731, 153);
            ErrorTxtBox.Name = "ErrorTxtBox";
            ErrorTxtBox.ReadOnly = true;
            ErrorTxtBox.Size = new Size(104, 22);
            ErrorTxtBox.StateCommon.Back.Color1 = Color.FromArgb(255, 128, 128);
            ErrorTxtBox.StateCommon.Content.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            ErrorTxtBox.TabIndex = 14;
            ErrorTxtBox.Text = "0";
            ErrorTxtBox.TextAlign = HorizontalAlignment.Center;
            // 
            // ScannedTxtBox
            // 
            ScannedTxtBox.Location = new Point(731, 192);
            ScannedTxtBox.Name = "ScannedTxtBox";
            ScannedTxtBox.ReadOnly = true;
            ScannedTxtBox.Size = new Size(104, 22);
            ScannedTxtBox.StateCommon.Back.Color1 = Color.FromArgb(255, 255, 128);
            ScannedTxtBox.StateCommon.Content.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            ScannedTxtBox.TabIndex = 15;
            ScannedTxtBox.Text = "0";
            ScannedTxtBox.TextAlign = HorizontalAlignment.Center;
            // 
            // StatusTxtBox
            // 
            StatusTxtBox.Location = new Point(731, 233);
            StatusTxtBox.Name = "StatusTxtBox";
            StatusTxtBox.ReadOnly = true;
            StatusTxtBox.Size = new Size(104, 22);
            StatusTxtBox.StateCommon.Back.Color1 = Color.Yellow;
            StatusTxtBox.StateCommon.Content.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            StatusTxtBox.TabIndex = 16;
            StatusTxtBox.Text = "Chưa bắt đầu";
            StatusTxtBox.TextAlign = HorizontalAlignment.Center;
            // 
            // ThreadUpDown
            // 
            ThreadUpDown.Location = new Point(731, 9);
            ThreadUpDown.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            ThreadUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            ThreadUpDown.Name = "ThreadUpDown";
            ThreadUpDown.Size = new Size(104, 21);
            ThreadUpDown.StateCommon.Content.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            ThreadUpDown.TabIndex = 17;
            ThreadUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // InputBtn
            // 
            InputBtn.CornerRoundingRadius = -1F;
            InputBtn.Location = new Point(356, 279);
            InputBtn.Name = "InputBtn";
            InputBtn.Size = new Size(107, 25);
            InputBtn.StateCommon.Back.Color1 = Color.Cyan;
            InputBtn.StateCommon.Back.Color2 = Color.Cyan;
            InputBtn.StateCommon.Content.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            InputBtn.TabIndex = 18;
            InputBtn.Values.Text = "Nhập dữ liệu";
            InputBtn.Click += InputBtn_Click;
            // 
            // StartBtn
            // 
            StartBtn.CornerRoundingRadius = -1F;
            StartBtn.Location = new Point(12, 326);
            StartBtn.Name = "StartBtn";
            StartBtn.Size = new Size(109, 25);
            StartBtn.StateCommon.Back.Color1 = Color.LawnGreen;
            StartBtn.StateCommon.Back.Color2 = Color.LawnGreen;
            StartBtn.StateCommon.Content.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            StartBtn.TabIndex = 19;
            StartBtn.Values.Text = "Check từ đầu";
            StartBtn.Click += StartBtn_Click;
            // 
            // StopBtn
            // 
            StopBtn.CornerRoundingRadius = -1F;
            StopBtn.Enabled = false;
            StopBtn.Location = new Point(356, 326);
            StopBtn.Name = "StopBtn";
            StopBtn.Size = new Size(107, 25);
            StopBtn.StateCommon.Back.Color1 = Color.FromArgb(255, 128, 128);
            StopBtn.StateCommon.Back.Color2 = Color.FromArgb(255, 128, 128);
            StopBtn.StateCommon.Content.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            StopBtn.TabIndex = 20;
            StopBtn.Values.Text = "Dừng lại";
            StopBtn.Click += StopBtn_Click;
            // 
            // ContinueBtn
            // 
            ContinueBtn.CornerRoundingRadius = -1F;
            ContinueBtn.Location = new Point(186, 326);
            ContinueBtn.Name = "ContinueBtn";
            ContinueBtn.Size = new Size(110, 25);
            ContinueBtn.StateCommon.Back.Color1 = Color.LawnGreen;
            ContinueBtn.StateCommon.Back.Color2 = Color.LawnGreen;
            ContinueBtn.StateCommon.Content.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            ContinueBtn.TabIndex = 21;
            ContinueBtn.Values.Text = "Check tiếp";
            ContinueBtn.Click += ContinueBtn_Click;
            // 
            // AccountGridView
            // 
            AccountGridView.AllowUserToAddRows = false;
            AccountGridView.AllowUserToDeleteRows = false;
            AccountGridView.AllowUserToResizeRows = false;
            AccountGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            AccountGridView.ColumnHeadersHeight = 28;
            AccountGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            AccountGridView.Columns.AddRange(new DataGridViewColumn[] { Username, Password, CheckCount, ErrorCount, Status });
            AccountGridView.Location = new Point(12, 12);
            AccountGridView.Name = "AccountGridView";
            AccountGridView.RowHeadersVisible = false;
            AccountGridView.RowTemplate.Height = 25;
            AccountGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            AccountGridView.Size = new Size(619, 243);
            AccountGridView.TabIndex = 22;
            // 
            // Username
            // 
            Username.DataPropertyName = "Username";
            Username.DefaultCellStyle = dataGridViewCellStyle1;
            Username.HeaderText = "Tài khoản";
            Username.Name = "Username";
            Username.ReadOnly = true;
            Username.Width = 124;
            // 
            // Password
            // 
            Password.DataPropertyName = "Password";
            Password.DefaultCellStyle = dataGridViewCellStyle2;
            Password.HeaderText = "Mật khẩu";
            Password.Name = "Password";
            Password.ReadOnly = true;
            Password.Width = 123;
            // 
            // CheckCount
            // 
            CheckCount.DataPropertyName = "CheckCount";
            CheckCount.DefaultCellStyle = dataGridViewCellStyle3;
            CheckCount.HeaderText = "Đã check";
            CheckCount.Name = "CheckCount";
            CheckCount.ReadOnly = true;
            CheckCount.Width = 124;
            // 
            // ErrorCount
            // 
            ErrorCount.DataPropertyName = "ErrorCount";
            ErrorCount.DefaultCellStyle = dataGridViewCellStyle4;
            ErrorCount.HeaderText = "Check lỗi";
            ErrorCount.Name = "ErrorCount";
            ErrorCount.ReadOnly = true;
            ErrorCount.Width = 123;
            // 
            // Status
            // 
            Status.DataPropertyName = "Status";
            Status.DefaultCellStyle = dataGridViewCellStyle5;
            Status.HeaderText = "Trạng thái";
            Status.Name = "Status";
            Status.ReadOnly = true;
            Status.Width = 124;
            // 
            // ConfigAccountBtn
            // 
            ConfigAccountBtn.CornerRoundingRadius = -1F;
            ConfigAccountBtn.Location = new Point(12, 279);
            ConfigAccountBtn.Name = "ConfigAccountBtn";
            ConfigAccountBtn.Size = new Size(106, 25);
            ConfigAccountBtn.StateCommon.Back.Color1 = Color.Cyan;
            ConfigAccountBtn.StateCommon.Back.Color2 = Color.Cyan;
            ConfigAccountBtn.StateCommon.Content.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            ConfigAccountBtn.TabIndex = 23;
            ConfigAccountBtn.Values.Text = "Nhập account";
            ConfigAccountBtn.Click += ConfigAccountBtn_Click;
            // 
            // LoginThreadUpDown
            // 
            LoginThreadUpDown.Location = new Point(221, 283);
            LoginThreadUpDown.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            LoginThreadUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            LoginThreadUpDown.Name = "LoginThreadUpDown";
            LoginThreadUpDown.Size = new Size(104, 21);
            LoginThreadUpDown.StateCommon.Content.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LoginThreadUpDown.TabIndex = 25;
            LoginThreadUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // kryptonLabel1
            // 
            kryptonLabel1.Location = new Point(124, 285);
            kryptonLabel1.Name = "kryptonLabel1";
            kryptonLabel1.Size = new Size(91, 19);
            kryptonLabel1.StateCommon.ShortText.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            kryptonLabel1.TabIndex = 24;
            kryptonLabel1.Values.Text = "Số luồng login:";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(850, 374);
            Controls.Add(LoginThreadUpDown);
            Controls.Add(kryptonLabel1);
            Controls.Add(ConfigAccountBtn);
            Controls.Add(AccountGridView);
            Controls.Add(ContinueBtn);
            Controls.Add(StopBtn);
            Controls.Add(StartBtn);
            Controls.Add(InputBtn);
            Controls.Add(ThreadUpDown);
            Controls.Add(StatusTxtBox);
            Controls.Add(ScannedTxtBox);
            Controls.Add(ErrorTxtBox);
            Controls.Add(DieTxtBox);
            Controls.Add(LiveTxtBox);
            Controls.Add(TotalTxtBox);
            Controls.Add(kryptonLabel9);
            Controls.Add(kryptonLabel8);
            Controls.Add(kryptonLabel7);
            Controls.Add(kryptonLabel6);
            Controls.Add(kryptonLabel5);
            Controls.Add(kryptonLabel4);
            Controls.Add(kryptonLabel3);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FrmMain";
            Text = "Instagram Checker";
            ((System.ComponentModel.ISupportInitialize)AccountGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private Krypton.Toolkit.KryptonLabel kryptonLabel7;
        private Krypton.Toolkit.KryptonLabel kryptonLabel8;
        private Krypton.Toolkit.KryptonLabel kryptonLabel9;
        private Krypton.Toolkit.KryptonTextBox TotalTxtBox;
        private Krypton.Toolkit.KryptonTextBox LiveTxtBox;
        private Krypton.Toolkit.KryptonTextBox DieTxtBox;
        private Krypton.Toolkit.KryptonTextBox ErrorTxtBox;
        private Krypton.Toolkit.KryptonTextBox ScannedTxtBox;
        private Krypton.Toolkit.KryptonTextBox StatusTxtBox;
        private Krypton.Toolkit.KryptonNumericUpDown ThreadUpDown;
        private Krypton.Toolkit.KryptonButton InputBtn;
        private Krypton.Toolkit.KryptonButton StartBtn;
        private Krypton.Toolkit.KryptonButton StopBtn;
        private Krypton.Toolkit.KryptonButton ContinueBtn;
        private Krypton.Toolkit.KryptonDataGridView AccountGridView;
        private Krypton.Toolkit.KryptonDataGridViewTextBoxColumn Username;
        private Krypton.Toolkit.KryptonDataGridViewTextBoxColumn Password;
        private Krypton.Toolkit.KryptonDataGridViewTextBoxColumn CheckCount;
        private Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ErrorCount;
        private Krypton.Toolkit.KryptonDataGridViewTextBoxColumn Status;
        private Krypton.Toolkit.KryptonButton ConfigAccountBtn;
        private Krypton.Toolkit.KryptonNumericUpDown LoginThreadUpDown;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
    }
}