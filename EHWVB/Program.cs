using System;
using System.Threading;
using System.Windows.Forms;

namespace EHWVB
{
	static class Program
	{
		static Mutex m;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			if (!Array.Exists(Environment.GetCommandLineArgs(), arg => arg == "/nsic"))
			{
				bool first = false;
				m = new Mutex(true, Application.ProductName.ToString(), out first);
				if (first)
				{
					Start();
					m.ReleaseMutex();
				}
				else
					MessageBox.Show("EHWVB is already running. Check your notification area, and double click the icon.");
			}
			else
				Start();
		}

		static void Start()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Main());
		}
	}
}
