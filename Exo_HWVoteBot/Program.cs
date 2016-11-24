using System;
using System.Threading;
using System.Windows.Forms;

namespace Exo_HWVoteBot
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
			bool first = false;
			m = new Mutex(true, Application.ProductName.ToString(), out first);
			if ((first))
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Main());
				m.ReleaseMutex();
			}
			else
				MessageBox.Show("EHWVB is already running. Check your notification area, and double click the icon.");
		}
	}
}
