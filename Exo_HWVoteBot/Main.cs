using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Exo_HWVoteBot
{
	public partial class Main : Form
	{
		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
		[DllImport("Kernel32.dll")]
		static extern int GetLastError();

		Version version = new Version("1.2");
		RegistryKey windowsStartup = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
		bool enabled = false;
		bool needConnect = false;
		bool disconnecting = false;
		bool voting = false;
		int currentSite = 0;

		public Main()
		{
			InitializeComponent();

			if (windowsStartup.GetValue("EHWVB /minimized /enabled") == null)
				CB_WindowsStartup.Checked = false;
			else
				CB_WindowsStartup.Checked = true;
		}

		private void Main_Shown(object sender, EventArgs e)
		{
			LBL_Status.Text = "Loading...";

			((Control)WB_Main).Enabled = false;

			LBL_Status.Text = "Checking for internet connection...";
			if (CheckInternetConnection("http://www.google.com"))
			{
				LBL_Status.Text = "Checking for new version...";
				WebClient versionDownloader = new WebClient();
				versionDownloader.DownloadFile("https://raw.githubusercontent.com/ExoKalork/EHWVB/master/version.txt", "version");
				StreamReader reader = new StreamReader("version");
				if (Version.Parse(reader.ReadLine()) > version)
				{
					DialogResult dialogResult = MessageBox.Show("New version available ! Do you want to download it ?", "EHWVB", MessageBoxButtons.YesNo);
					if (dialogResult == DialogResult.Yes)
					{
						reader.Close();
						File.Delete("version");
						Process.Start("https://github.com/ExoKalork/EHWVB/releases");
						Environment.Exit(0);
					}
				}
				reader.Close();
				File.Delete("version");

				LBL_Status.Text = "Checking for heroes-wow.com availability...";
				if (CheckInternetConnection("http://heroes-wow.com/wotlk/"))
				{
					ConnectCheck();

					string[] args = Environment.GetCommandLineArgs();
					foreach (string arg in args)
					{
						switch (arg)
						{
							case "/minimized":
								NTI_Main.Visible = true;
								WindowState = FormWindowState.Minimized;
								Hide();
								break;
						}
					}
				}
				else
				{
					MessageBox.Show("Heroes-WoW's website seems down. Try again later.");
					Environment.Exit(0);
				}
			}
			else
			{
				MessageBox.Show("You need an active internet connection to use Exo's Vote Bot.");
				Environment.Exit(0);
			}
		}

		private void ConnectCheck()
		{
			WB_Main.Navigate("https://heroes-wow.com/wotlk/index.php?page=login");
			LBL_Status.Text = "Checking if user is online...";
		}

		private void WB_Main_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			if (!enabled)
			{
				if (disconnecting)
				{
					if (WB_Main.Url.ToString() == "https://heroes-wow.com/")
						WB_Main.Navigate("https://heroes-wow.com/wotlk/logout.php");
					else if (WB_Main.Url.ToString() == "https://heroes-wow.com/wotlk/")
					{
						disconnecting = false;
						MessageBox.Show("Done !");
						Environment.Exit(0);
					}
				}
				else if (!needConnect)
				{
					if (WB_Main.Url.ToString() == "https://heroes-wow.com/wotlk/index.php?page=login")
						TM_ConnectedCheck.Start();
					else if (WB_Main.Url.ToString() == "https://heroes-wow.com/wotlk/index.php" && TM_ConnectedCheck.Enabled)
					{
						TM_ConnectedCheck.Stop();
						BT_EnableDisable.Enabled = true;
						LBL_Status.Text = "Ready !";
						if (Array.Exists(Environment.GetCommandLineArgs(), arg => arg == "/enabled"))
							EnableDisable();
					}
				}
				else if (needConnect && WB_Main.Url.ToString() == "https://heroes-wow.com/wotlk/index.php?page=loginb")
				{
					MessageBox.Show("Connection successfull !");
					needConnect = false;
					((Control)WB_Main).Enabled = false;
					BT_EnableDisable.Enabled = true;
					LBL_Status.Text = "Ready !";
					if (Array.Exists(Environment.GetCommandLineArgs(), arg => arg == "/enabled"))
						EnableDisable();
				}
			}
			else
			{
				if (WB_Main.Url.ToString() == "https://heroes-wow.com/wotlk/index.php?page=vote")
				{
					LBL_Status.Text = "Voted on site " + currentSite;
					switch (currentSite)
					{
						case 1:
							currentSite++;
							InternetSetCookie("http://heroes-wow.com", "HTTP_REFERER", "http%3A%2F%2Fwww.top100arena.com%2Fout.asp%3Fid%3D44752");
							WB_Main.Navigate("https://heroes-wow.com/wotlk/execute.php?take=vote&site=2");
							break;
						case 2:
							currentSite++;
							InternetSetCookie("http://heroes-wow.com", "HTTP_REFERER", "http%3A%2F%2Fwww.openwow.com%2Fvisit%3D2125");
							WB_Main.Navigate("https://heroes-wow.com/wotlk/execute.php?take=vote&site=3");
							break;
						case 3:
							currentSite++;
							InternetSetCookie("http://heroes-wow.com", "HTTP_REFERER", "http%3A%2F%2Fwww.rpg-paradize.com%2Fsite-Heroes%2BWoW%2B548%2Band%2B335a%2B255%2BLevel-22237");
							WB_Main.Navigate("https://heroes-wow.com/wotlk/execute.php?take=vote&site=4");
							break;
						case 4:
							currentSite++;
							InternetSetCookie("http://heroes-wow.com", "HTTP_REFERER", "http%3A%2F%2Ftopg.org%2Fserver-heroes-wow-id347987");
							WB_Main.Navigate("https://heroes-wow.com/wotlk/execute.php?take=vote&site=5");
							break;
						case 5:
							currentSite++;
							InternetSetCookie("http://heroes-wow.com", "HTTP_REFERER", "http%3A%2F%2Fwww.wowstatus.net%2F%7Ewowstatus%2Fserverlist%2Fout%2Fid%2F725206%2Flink%2Fhomepage");
							WB_Main.Navigate("https://heroes-wow.com/wotlk/execute.php?take=vote&site=6");
							break;
						case 6:
							currentSite += 2;
							InternetSetCookie("http://heroes-wow.com", "HTTP_REFERER", "http%3A%2F%2Ftopwow.ru%2Findex.php");
							WB_Main.Navigate("https://heroes-wow.com/wotlk/execute.php?take=vote&site=8");
							break;
						case 8:
							currentSite += 2;
							InternetSetCookie("http://heroes-wow.com", "HTTP_REFERER", "http%3A%2F%2Fwww.wowtop.es%2Findex.php");
							WB_Main.Navigate("https://heroes-wow.com/wotlk/execute.php?take=vote&site=10");
							break;
						case 10:
							currentSite++;
							InternetSetCookie("http://heroes-wow.com", "HTTP_REFERER", "http%3A%2F%2Fwww.gowonda.com%2Fserveur-wow-4956-Heroes-WoW-5.4.8-and-3.3.5a-255-Level.htm");
							WB_Main.Navigate("https://heroes-wow.com/wotlk/execute.php?take=vote&site=11");
							break;
						case 11:
							LBL_Status.Text = "Successfully voted !";
							voting = false;
							currentSite = 0;
							if (!File.Exists("LastVote"))
								File.Create("LastVote").Close();
							StreamWriter writer = new StreamWriter("LastVote");
							writer.Write(DateTime.Now);
							writer.Close();
							TM_VoteCheck.Start();
							WB_Main.Navigate("http://heroes-wow.com/wotlk/");
							break;
					}
				}
			}
		}

		private void TM_ConnectedCheck_Tick(object sender, EventArgs e)
		{
			TM_ConnectedCheck.Stop();
			if (WindowState == FormWindowState.Minimized)
			{
				NTI_Main.Visible = false;
				Show();
				WindowState = FormWindowState.Normal;
			}
			MessageBox.Show("Please connect to Heroes-WoW's website.");
			LBL_Status.Text = "Waiting for connection...";
			needConnect = true;
			((Control)WB_Main).Enabled = true;
			WB_Main.Navigate("https://heroes-wow.com/wotlk/index.php?page=login");
		}

		private bool CheckInternetConnection(string url)
		{
			try
			{
				using (var client = new WebClient())
				{
					using (var stream = client.OpenRead(url))
					{
						return true;
					}
				}
			}
			catch
			{
				return false;
			}
		}

		private void BT_EnableDisable_Click(object sender, EventArgs e)
		{
			EnableDisable();
		}

		private void EnableDisable()
		{
			if (!enabled)
			{
				enabled = true;
				BT_EnableDisable.Text = "Disable";
				TM_VoteCheck.Interval = 10;
				TM_VoteCheck.Start();
			}
			else
			{
				enabled = false;
				if (voting)
					WB_Main.Navigate("https://heroes-wow.com/wotlk/");
				currentSite = 0;
				BT_EnableDisable.Text = "Enable";
				LBL_Status.Text = "Ready !";
				TM_VoteCheck.Stop();
			}
		}

		private void disconnectMeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!enabled)
			{
				disconnecting = true;
				WB_Main.Navigate("https://heroes-wow.com/");
			}
		}

		private void qUitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		private void TM_VoteCheck_Tick(object sender, EventArgs e)
		{
			TM_VoteCheck.Stop();
			TM_VoteCheck.Interval = 600000;
			LBL_Status.Text = "Checking if I need to vote...";
			if (File.Exists("LastVote"))
			{
				try
				{
					StreamReader reader = new StreamReader("LastVote");
					string lastVote = reader.ReadLine();
					if (DateTime.Parse(lastVote).AddHours(12) < DateTime.Now)
						Vote();
					else
					{
						LBL_Status.Text = "I don't need to vote. Waiting...";
						TM_VoteCheck.Start();
					}
					reader.Close();
				}
				catch
				{
					LBL_Status.Text = "LastVote file is corrupted, deleting it...";
					File.Delete("LastVote");
					Vote();
				}
			}
			else
				Vote();
		}

		private void Vote()
		{
			LBL_Status.Text = "Voting...";
			voting = true;
			currentSite++;
			if (!InternetSetCookie("http://heroes-wow.com", "HTTP_REFERER", "http%3A%2F%2Fwww.xtremetop100.com%2Fout.php%3Fsite%3D1132349385"))
				MessageBox.Show("An error occurred. Please open a bug report on Github with this error code : " + GetLastError());
			WB_Main.Navigate("https://heroes-wow.com/wotlk/execute.php?take=vote&site=1");
		}

		private void Main_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
			{
				NTI_Main.Visible = true;
				NTI_Main.ShowBalloonTip(500);
				Hide();
			}
		}

		private void NTI_Main_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			NTI_Main.Visible = false;
			Show();
			WindowState = FormWindowState.Normal;
		}

		private void CB_WindowsStartup_CheckedChanged(object sender, EventArgs e)
		{
			if (CB_WindowsStartup.Checked)
				windowsStartup.SetValue("EHWVB /minimized /enabled", Application.ExecutablePath);
			else
				windowsStartup.DeleteValue("EHWVB /minimized /enabled", false);
		}

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!voting && !needConnect)
				WB_Main.Navigate("https://heroes-wow.com/wotlk/");
			else
				MessageBox.Show("Please wait until your current task is done.");
		}
	}
}
