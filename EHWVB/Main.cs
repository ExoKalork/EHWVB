using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EHWVB
{
	public partial class Main : Form
	{
		#region Functions import

		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
		[DllImport("Kernel32.dll")]
		static extern int GetLastError();

		#endregion

		#region Global Variables

		Version version = new Version("1.3");
		RegistryKey windowsStartup = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
		string WSParams = "/minimized /enabled /WaitForInternet";
		bool enabled = false;
		bool needConnect = false;
		bool disconnecting = false;
		bool connectCheck = false;
		bool voting = false;
		int currentSite = 0;
		string logFile = "";

		#endregion

		public Main()
		{
			InitializeComponent();
			ChangeWorkingPath();
			Log("Loading...");
			if (!Array.Exists(Environment.GetCommandLineArgs(), arg => arg == "/nonamecheck"))
				CheckFileName();
			CheckIfWSIsEnabled();
		}

		#region Windows Startup

		private void ChangeWorkingPath()
		{
			var wp = new FileInfo(Application.ExecutablePath);
			Directory.SetCurrentDirectory(wp.DirectoryName);
		}
		private void CheckIfWSIsEnabled()
		{
			if (windowsStartup.GetValue("EHWVB") == null)
				CB_WindowsStartup.Checked = false;
			else
			{
				Log("Startup value in registry exists, switching checkbox.");
				CB_WindowsStartup.Checked = true;
			}
		}
		private void CB_WindowsStartup_CheckedChanged(object sender, EventArgs e)
		{
			if (CB_WindowsStartup.Checked)
			{
				Log("Adding startup value to registry.");
				windowsStartup.SetValue("EHWVB", Application.ExecutablePath + " " + WSParams);
			}
			else
			{
				Log("Removing startup value to registry.");
				windowsStartup.DeleteValue("EHWVB", false);
			}
		}

		#endregion

		#region Loading

		private void CheckFileName()
		{
			string currentName = Process.GetCurrentProcess().ProcessName;
			if (currentName != "EHWVB")
			{
				Log("My name isn't right ! Trying to rename myself...");
				if (File.Exists(currentName + ".exe"))
				{
					if (File.Exists("EHWVB.exe"))
					{
						File.Delete("EHWVB.exe");
						System.Threading.Thread.Sleep(100);
					}
					File.Move(currentName + ".exe", "EHWVB.exe");
					Process.Start("EHWVB.exe", "/nsic");
					Log("Done ! Launching the new file and closing.");
					Environment.Exit(0);
				}
			}
		}
		private void Main_Shown(object sender, EventArgs e)
		{
			LBL_Status.Text = "Loading...";

			((Control)WB_Main).Enabled = false;
			if (Array.Exists(Environment.GetCommandLineArgs(), arg => arg == "/minimized"))
				WindowState = FormWindowState.Minimized;

			if (Array.Exists(Environment.GetCommandLineArgs(), arg => arg == "/WaitForInternet"))
			{
				if (!CheckInternetConnection(true))
					TM_WaitForInternet.Start();
				else
				{
					CheckForNewVersion();
					ConnectCheck();
				}
			}
			else
			{
				if (CheckInternetConnection())
				{
					CheckForNewVersion();
					ConnectCheck();
				}
			}
		}
		private bool CheckInternetConnection(bool silent = false)
		{
			Log("Checking for internet connection...");
			LBL_Status.Text = "Checking for internet connection...";
			if (CheckWebsiteAccess("http://www.google.com"))
			{
				Log("Checking for heroes-wow.com availability...");
				LBL_Status.Text = "Checking for heroes-wow.com availability...";
				if (CheckWebsiteAccess("http://heroes-wow.com/wotlk/"))
				{
					return true;
				}
				else
				{
					if (!silent)
					{
						Log("Heroes-WoW's website seems down. Closing.");
						MessageBox.Show("Heroes-WoW's website seems down. Try again later.");
						Application.Exit();
					}
					else
						Log("Heroes-WoW's website seems down. Retrying in 10 seconds.");
				}
			}
			else
			{
				if (!silent)
				{
					Log("No active connection found. Closing.");
					MessageBox.Show("You need an active internet connection to use Exo's Vote Bot.");
					Application.Exit();
				}
				else
					Log("No active connection found. Retrying in 10 seconds.");
			}
			return false;
		}
		private void TM_WaitForInternet_Tick(object sender, EventArgs e)
		{
			TM_WaitForInternet.Stop();
			if (CheckInternetConnection(true))
			{
				CheckForNewVersion();
				ConnectCheck();
			}
			else
				TM_WaitForInternet.Start();
		}
		private void CheckForNewVersion()
		{
			Log("Checking for new version...");
			WebClient versionDownloader = new WebClient();
			versionDownloader.DownloadFile("https://raw.githubusercontent.com/ExoKalork/EHWVB/master/version.txt", "EHWVBVersion.txt");
			StreamReader reader = new StreamReader("EHWVBVersion.txt");
			if (Version.Parse(reader.ReadLine()) > version)
			{
				Log("New version available ! Prompting user...");
				DialogResult dialogResult = MessageBox.Show("New version available ! Do you want to download it ?", "EHWVB", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					Log("User agreed ! Opening download page.");
					reader.Close();
					File.Delete("EHWVBVersion.txt");
					Process.Start("https://github.com/ExoKalork/EHWVB/releases");
					Application.Exit();
				}
			}
			reader.Close();
			File.Delete("EHWVBVersion.txt");
			Log("Done checking version !");
		}

		#endregion

		private void WB_Main_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			WB_Main.Stop();
			Disconnect();
			Connect();
			VoteProcess();
		}

		#region VoteProcess

		private void VoteProcess()
		{
			if (enabled && voting && WB_Main.Url.ToString() == "https://heroes-wow.com/wotlk/index.php?page=vote")
			{
				Log("Voted on site " + currentSite);
				LBL_Status.Text = "Voted on site " + currentSite;
				TM_VoteProcess.Start();
				WB_Main.AllowNavigation = false;
			}
		}
		private void TM_VoteProcess_Tick(object sender, EventArgs e)
		{
			TM_VoteProcess.Stop();
			WB_Main.AllowNavigation = true;
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
					Log("Successfully voted !");
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
		private void EnableDisable()
		{
			if (!enabled)
			{
				Log("Enabling.");
				enabled = true;
				BT_EnableDisable.Text = "Disable";
				TM_VoteCheck.Interval = 10;
				TM_VoteCheck.Start();
			}
			else
			{
				Log("Disabling.");
				enabled = false;
				if (voting)
					WB_Main.Navigate("https://heroes-wow.com/wotlk/");
				currentSite = 0;
				BT_EnableDisable.Text = "Enable";
				LBL_Status.Text = "Ready !";
				TM_VoteCheck.Stop();
			}
		}
		private void BT_EnableDisable_Click(object sender, EventArgs e)
		{
			EnableDisable();
		}
		private void TM_VoteCheck_Tick(object sender, EventArgs e)
		{
			TM_VoteCheck.Stop();
			TM_VoteCheck.Interval = 600000;
			Log("Checking if I need to vote...");
			LBL_Status.Text = "Checking if I need to vote...";
			if (File.Exists("LastVote"))
			{
				try
				{
					StreamReader reader = new StreamReader("LastVote");
					string lastVote = reader.ReadLine();
					if (DateTime.Parse(lastVote).AddHours(12) < DateTime.Now)
						Vote(false);
					else
					{
						string type = " Hours";
						int nextVote = DateTimeCeil(DateTime.Parse(lastVote).AddHours(12) - DateTime.Now, new TimeSpan(1, 0, 0)).Hour;
						if (nextVote == 1)
						{
							type = " Minutes";
							nextVote = DateTimeCeil(DateTime.Parse(lastVote).AddHours(12) - DateTime.Now, new TimeSpan(0, 1, 0)).Minute;
						}
						if (nextVote == 1)
						{
							type = " Secondes";
							nextVote = DateTimeCeil(DateTime.Parse(lastVote).AddHours(12) - DateTime.Now, new TimeSpan(0, 0, 1)).Second;
						}
						Log("No need to vote. Next vote in " + nextVote + type + ". Waiting.");
						LBL_Status.Text = "I don't need to vote. Next vote in " + nextVote + type + ". Waiting...";
						TM_VoteCheck.Start();
					}
					reader.Close();
				}
				catch
				{
					Log("[WARNING] LastVote file is corrupted, deleting it...");
					LBL_Status.Text = "LastVote file is corrupted, deleting it...";
					File.Delete("LastVote");
					Log("Success !");
					Vote(false);
				}
			}
			else
				Vote(false);
		}
		private void Vote(bool online)
		{
			voting = true;
			if (!online)
			{
				ConnectCheck();
			}
			else
			{
				Log("Voting...");
				LBL_Status.Text = "Voting...";
				currentSite++;
				if (!InternetSetCookie("http://heroes-wow.com", "HTTP_REFERER", "http%3A%2F%2Fwww.xtremetop100.com%2Fout.php%3Fsite%3D1132349385"))
					MessageBox.Show("An error occurred. Please open a bug report on Github with this error code : " + GetLastError());
				WB_Main.Navigate("https://heroes-wow.com/wotlk/execute.php?take=vote&site=1");
			}
		}

		#endregion

		#region Connect

		private void Connect()
		{
			if (connectCheck)
			{
				if (!needConnect)
				{
					if (WB_Main.Url.ToString() == "https://heroes-wow.com/wotlk/index.php?page=login")
						TM_ConnectedCheck.Start();
					else if (WB_Main.Url.ToString() == "https://heroes-wow.com/wotlk/index.php" && TM_ConnectedCheck.Enabled)
					{
						connectCheck = false;
						TM_ConnectedCheck.Stop();
						BT_EnableDisable.Enabled = true;
						LBL_Status.Text = "Ready !";
						Log("He is ! Ready.");
						if (voting)
							Vote(true);
						else
							if (Array.Exists(Environment.GetCommandLineArgs(), arg => arg == "/enabled"))
								EnableDisable();
					}
				}
				else if (needConnect && WB_Main.Url.ToString() == "https://heroes-wow.com/wotlk/index.php?page=loginb")
				{
					connectCheck = false;
					MessageBox.Show("Connection successfull !");
					needConnect = false;
					((Control)WB_Main).Enabled = false;
					BT_EnableDisable.Enabled = true;
					LBL_Status.Text = "Ready !";
					if (voting)
						Vote(true);
					else
						if (Array.Exists(Environment.GetCommandLineArgs(), arg => arg == "/enabled"))
							EnableDisable();
				}
			}
		}
		private void ConnectCheck()
		{
			connectCheck = true;
			WB_Main.Navigate("https://heroes-wow.com/wotlk/index.php?page=login");
			Log("Checking if user is online...");
			LBL_Status.Text = "Checking if user is online...";
		}
		private void TM_ConnectedCheck_Tick(object sender, EventArgs e)
		{
			TM_ConnectedCheck.Stop();
			Log("He isn't. Prompting user to log in.");
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

		#endregion

		#region Disconnect

		private void Disconnect()
		{
			if (disconnecting)
			{
				if (WB_Main.Url.ToString() == "https://heroes-wow.com/")
					WB_Main.Navigate("https://heroes-wow.com/wotlk/logout.php");
				else if (WB_Main.Url.ToString() == "https://heroes-wow.com/wotlk/")
				{
					disconnecting = false;
					Log("Done ! Now closing.");
					MessageBox.Show("Done !");
					Application.Exit();
				}
			}
		}
		private void disconnectMeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!enabled && !connectCheck && !needConnect)
			{
				Log("User wants to be disconnected. Redirecting...");
				disconnecting = true;
				WB_Main.Navigate("https://heroes-wow.com/");
			}
		}

		#endregion

		#region User Control

		private void Main_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
			{
				Log("Going minimized !");
				NTI_Main.Visible = true;
				NTI_Main.ShowBalloonTip(500);
				Hide();
			}
		}
		private void NTI_Main_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Log("Double clicked, coming back up !");
			NTI_Main.Visible = false;
			Show();
			WindowState = FormWindowState.Normal;
		}
		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Log("User input to close, closing.");
			Application.Exit();
		}
		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!voting && !needConnect)
			{
				Log("Refreshing current page.");
				WB_Main.Navigate("https://heroes-wow.com/wotlk/");
			}
			else
				MessageBox.Show("Please wait until your current task is done.");
		}
		private void voteNowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (enabled)
			{
				Log("User input to vote now.");
				Vote(false);
			}
			else
				MessageBox.Show("Please click enable first.");
		}
		private void openToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Log("Right click menu to open clicked, coming back up !");
			NTI_Main.Visible = false;
			Show();
			WindowState = FormWindowState.Normal;
		}

		#endregion

		#region Tools

		private bool CheckWebsiteAccess(string url)
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
		private void Log(string line)
		{
			if (logFile == string.Empty)
				setLogFile();
			StreamWriter writer = new StreamWriter(@"Logs\" + logFile, true);
			writer.WriteLine(DateTime.Now + " : " + line);
			writer.Close();
		}
		private void setLogFile()
		{
			if (!Directory.Exists("Logs"))
				Directory.CreateDirectory("Logs");
			DateTime currentTime = DateTime.Now;
			if (File.Exists(@"Logs\" + formattedDateTime(currentTime)))
			{
				File.Delete(@"Logs\" + formattedDateTime(currentTime));
				System.Threading.Thread.Sleep(100);
			}
			File.Create(@"Logs\" + formattedDateTime(currentTime)).Close();
			logFile = formattedDateTime(currentTime);
		}
		private string formattedDateTime(DateTime value)
		{
			return "" + value.Month + "-" + value.Day + "-" + value.Year + " " + value.Hour + "." + value.Minute + "." + value.Second + ".log";
		}
		private DateTime DateTimeCeil(TimeSpan date, TimeSpan span)
		{
			long ticks = (date.Ticks + span.Ticks - 1) / span.Ticks;
			return new DateTime(ticks * span.Ticks);
		}

		#endregion
	}
}
