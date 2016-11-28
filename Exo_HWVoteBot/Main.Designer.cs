namespace Exo_HWVoteBot
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.WB_Main = new System.Windows.Forms.WebBrowser();
			this.BT_EnableDisable = new System.Windows.Forms.Button();
			this.LBL_Status = new System.Windows.Forms.Label();
			this.TM_ConnectedCheck = new System.Windows.Forms.Timer(this.components);
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.qUitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.voteNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.disconnectMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TM_VoteCheck = new System.Windows.Forms.Timer(this.components);
			this.NTI_Main = new System.Windows.Forms.NotifyIcon(this.components);
			this.CB_WindowsStartup = new System.Windows.Forms.CheckBox();
			this.TM_VoteProcess = new System.Windows.Forms.Timer(this.components);
			this.CMS_NTI = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.quitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.voteNowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuStrip1.SuspendLayout();
			this.CMS_NTI.SuspendLayout();
			this.SuspendLayout();
			// 
			// WB_Main
			// 
			this.WB_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.WB_Main.Location = new System.Drawing.Point(12, 27);
			this.WB_Main.MinimumSize = new System.Drawing.Size(20, 20);
			this.WB_Main.Name = "WB_Main";
			this.WB_Main.ScriptErrorsSuppressed = true;
			this.WB_Main.Size = new System.Drawing.Size(1102, 455);
			this.WB_Main.TabIndex = 0;
			this.WB_Main.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WB_Main_DocumentCompleted);
			// 
			// BT_EnableDisable
			// 
			this.BT_EnableDisable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BT_EnableDisable.Enabled = false;
			this.BT_EnableDisable.Location = new System.Drawing.Point(12, 488);
			this.BT_EnableDisable.Name = "BT_EnableDisable";
			this.BT_EnableDisable.Size = new System.Drawing.Size(874, 60);
			this.BT_EnableDisable.TabIndex = 1;
			this.BT_EnableDisable.Text = "Enable";
			this.BT_EnableDisable.UseVisualStyleBackColor = true;
			this.BT_EnableDisable.Click += new System.EventHandler(this.BT_EnableDisable_Click);
			// 
			// LBL_Status
			// 
			this.LBL_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.LBL_Status.Location = new System.Drawing.Point(892, 488);
			this.LBL_Status.Name = "LBL_Status";
			this.LBL_Status.Size = new System.Drawing.Size(222, 60);
			this.LBL_Status.TabIndex = 2;
			this.LBL_Status.Text = "Status";
			this.LBL_Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TM_ConnectedCheck
			// 
			this.TM_ConnectedCheck.Interval = 5000;
			this.TM_ConnectedCheck.Tick += new System.EventHandler(this.TM_ConnectedCheck_Tick);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.qUitToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.voteNowToolStripMenuItem,
            this.disconnectMeToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1126, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// qUitToolStripMenuItem
			// 
			this.qUitToolStripMenuItem.Name = "qUitToolStripMenuItem";
			this.qUitToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
			this.qUitToolStripMenuItem.Text = "Quit";
			this.qUitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
			this.refreshToolStripMenuItem.Text = "Refresh";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
			// 
			// voteNowToolStripMenuItem
			// 
			this.voteNowToolStripMenuItem.Name = "voteNowToolStripMenuItem";
			this.voteNowToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
			this.voteNowToolStripMenuItem.Text = "Vote Now";
			this.voteNowToolStripMenuItem.Click += new System.EventHandler(this.voteNowToolStripMenuItem_Click);
			// 
			// disconnectMeToolStripMenuItem
			// 
			this.disconnectMeToolStripMenuItem.Name = "disconnectMeToolStripMenuItem";
			this.disconnectMeToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
			this.disconnectMeToolStripMenuItem.Text = "Disconnect Me";
			this.disconnectMeToolStripMenuItem.Click += new System.EventHandler(this.disconnectMeToolStripMenuItem_Click);
			// 
			// TM_VoteCheck
			// 
			this.TM_VoteCheck.Interval = 10;
			this.TM_VoteCheck.Tick += new System.EventHandler(this.TM_VoteCheck_Tick);
			// 
			// NTI_Main
			// 
			this.NTI_Main.BalloonTipText = "I\'m now minimized. Double Click me to open me again.";
			this.NTI_Main.BalloonTipTitle = "Exo\'s Heroes-WoW Vote Bot";
			this.NTI_Main.ContextMenuStrip = this.CMS_NTI;
			this.NTI_Main.Icon = ((System.Drawing.Icon)(resources.GetObject("NTI_Main.Icon")));
			this.NTI_Main.Text = "Double Click me to open me again.";
			this.NTI_Main.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NTI_Main_MouseDoubleClick);
			// 
			// CB_WindowsStartup
			// 
			this.CB_WindowsStartup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CB_WindowsStartup.AutoSize = true;
			this.CB_WindowsStartup.Location = new System.Drawing.Point(972, 4);
			this.CB_WindowsStartup.Name = "CB_WindowsStartup";
			this.CB_WindowsStartup.Size = new System.Drawing.Size(142, 17);
			this.CB_WindowsStartup.TabIndex = 4;
			this.CB_WindowsStartup.Text = "Run at Windows Startup";
			this.CB_WindowsStartup.UseVisualStyleBackColor = true;
			this.CB_WindowsStartup.CheckedChanged += new System.EventHandler(this.CB_WindowsStartup_CheckedChanged);
			// 
			// TM_VoteProcess
			// 
			this.TM_VoteProcess.Interval = 1000;
			this.TM_VoteProcess.Tick += new System.EventHandler(this.TM_VoteProcess_Tick);
			// 
			// CMS_NTI
			// 
			this.CMS_NTI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem1,
            this.voteNowToolStripMenuItem1,
            this.toolStripSeparator1,
            this.openToolStripMenuItem1});
			this.CMS_NTI.Name = "CMS_NTI";
			this.CMS_NTI.Size = new System.Drawing.Size(126, 76);
			// 
			// quitToolStripMenuItem1
			// 
			this.quitToolStripMenuItem1.Name = "quitToolStripMenuItem1";
			this.quitToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
			this.quitToolStripMenuItem1.Text = "Open";
			this.quitToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
			// 
			// openToolStripMenuItem1
			// 
			this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
			this.openToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
			this.openToolStripMenuItem1.Text = "Quit";
			this.openToolStripMenuItem1.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
			// 
			// voteNowToolStripMenuItem1
			// 
			this.voteNowToolStripMenuItem1.Name = "voteNowToolStripMenuItem1";
			this.voteNowToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
			this.voteNowToolStripMenuItem1.Text = "Vote Now";
			this.voteNowToolStripMenuItem1.Click += new System.EventHandler(this.voteNowToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1126, 560);
			this.Controls.Add(this.CB_WindowsStartup);
			this.Controls.Add(this.LBL_Status);
			this.Controls.Add(this.BT_EnableDisable);
			this.Controls.Add(this.WB_Main);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(450, 350);
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Exo HW Vote Bot";
			this.Shown += new System.EventHandler(this.Main_Shown);
			this.Resize += new System.EventHandler(this.Main_Resize);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.CMS_NTI.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.WebBrowser WB_Main;
		private System.Windows.Forms.Button BT_EnableDisable;
		private System.Windows.Forms.Label LBL_Status;
		private System.Windows.Forms.Timer TM_ConnectedCheck;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem qUitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem disconnectMeToolStripMenuItem;
		private System.Windows.Forms.Timer TM_VoteCheck;
		private System.Windows.Forms.NotifyIcon NTI_Main;
		private System.Windows.Forms.CheckBox CB_WindowsStartup;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.Timer TM_VoteProcess;
		private System.Windows.Forms.ToolStripMenuItem voteNowToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip CMS_NTI;
		private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem voteNowToolStripMenuItem1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
	}
}

