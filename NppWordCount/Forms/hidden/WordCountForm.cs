﻿using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NppPluginNET;

namespace NppWordCount.Forms
{
	public partial class WordCountForm : Form
	{
		IntPtr hwndRebar   = IntPtr.Zero;
		IntPtr hwndToolbar = IntPtr.Zero;
		Control toolbarShownCanary;

		bool currentlyCheckingToolbarVisiblity = false;

		public WordCountForm()
		{
			toolbarShownCanary = new Control();
			toolbarShownCanary.HandleDestroyed += new EventHandler(toolbarShownCanary_HandleDestroyed);

			InitializeComponent();

            /* Maybe we should just hook the toolbar's window procedure instead of hoping that nobody else 
			 * tries this canary trick with a window just atop our canary. Because that would prevent 
			 * WM_PAINT messages from being delivered to our canary...
			 */
			toolbarShownCanary.Width  		   = 1;
			toolbarShownCanary.Height 		   = 1;
			toolbarShownCanary.Left   		   = 0;
			toolbarShownCanary.Top 	  		   = 0;
			toolbarShownCanary.Paint 		   += toolbarShownCanary_Paint;
		}

		void toolbarShownCanary_HandleDestroyed(object sender, EventArgs e)
		{
			EventHandler tick = null;
			tick = (_sender, _e) =>
			{
				timerDelay.Tick -= tick;
				timerDelay.Stop();

				CheckToolbarVisiblity();
			};

			timerDelay.Tick += tick;
			timerDelay.Start();
		}

		void InitToolbar()
		{
			IntPtr main = GetNppMainWindow();
			hwndRebar 	= IntPtr.Zero;
			hwndToolbar = IntPtr.Zero;

			Win32.SetParent(toolbarShownCanary.Handle, Handle);

			Win32.EnumChildWindows(main, child =>
			{
				if (Win32.GetParent(child) != main)
					return true;

				StringBuilder sb = new StringBuilder(256);
				Win32.GetClassName(child, sb, sb.Capacity);

				/* There are two rebar controls: one for the tool bar, the other for incemental search
				 */
				if (sb.ToString() == "ReBarWindow32")
				{
					sb = null;

					RECT rect;
					Win32.GetClientRect(child, out rect);

					Win32.EnumChildWindows(child, rebarChild =>
					{
						StringBuilder sb2 = new StringBuilder(256);
						Win32.GetClassName(rebarChild, sb2, sb2.Capacity);
					
						if (sb2.ToString() == "ToolbarWindow32")
						{
							hwndToolbar = rebarChild;
							return false;
						}
					
						return true;
					});

					if (hwndToolbar != IntPtr.Zero)
					{
						Win32.SetParent(toolbarShownCanary.Handle, hwndToolbar);

						hwndRebar = child;
						return false;
					}
				}

				sb = null;
				return true;
			});

			if (hwndRebar == IntPtr.Zero)
			{
				RECT rect;
				Win32.GetClientRect(main, out rect);

				FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
				Location = new Point(rect.Left - Width, rect.Top);

				ClientSize = frmCount.Size;

				MaximumSize = new Size(0, Size.Height);
			}
			else
			{
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

				MaximumSize = new Size(0, frmCount.Size.Height);
			}
		}

		// -1 on error
		static int GetRebarBandIndexByChildHandle(IntPtr hwndRebar, IntPtr hwndChild)
		{
			Win32.REBARBANDINFO band = new Win32.REBARBANDINFO();
			band.cbSize 			 = System.Runtime.InteropServices.Marshal.SizeOf(band);
			int count 				 = (int)Win32.SendMessage(hwndRebar, (NppMsg)Win32.RB_GETBANDCOUNT, 0, 0);

			for (int i = 0; i < count; ++i)
			{
				band.fMask 	   = Win32.RBBIM_CHILD;
				band.hwndChild = IntPtr.Zero;
				Win32.SendMessage(hwndRebar, Win32.RB_GETBANDINFOW, i, ref band);

				if (band.hwndChild == hwndChild)
					return i;
			}

			return -1;
		}

		public void CheckToolbarVisiblity()
		{
			if(currentlyCheckingToolbarVisiblity || toolbarShownCanary == null)
				return;

			try
			{
				currentlyCheckingToolbarVisiblity = true;
				if (!Win32.IsWindow(hwndRebar) || !Win32.IsWindow(hwndToolbar))
				{
					InitToolbar();

					Win32.SetWindowLong(
						Handle,
						Win32.GWL_STYLE,
						Win32.WS_CHILD | Win32.GetWindowLong(Handle, Win32.GWL_STYLE));

				}
				
				Win32.REBARBANDINFO band = new Win32.REBARBANDINFO();
				band.cbSize 			 = System.Runtime.InteropServices.Marshal.SizeOf(band);

				bool show = false; //Win32.IsWindowVisible(hwndToolbar);
				int toolbarIndex = GetRebarBandIndexByChildHandle(hwndRebar, hwndToolbar);
				if (toolbarIndex >= 0)
				{
					band.fMask = Win32.RBBIM_STYLE;
					
					Win32.SendMessage(hwndRebar, Win32.RB_GETBANDINFOW, toolbarIndex, ref band);

					show = (band.fStyle & Win32.RBBS_HIDDEN) == 0;
				}

				if (show == Visible)
				{
					return;
				}

				int searchBarIndex = GetRebarBandIndexByChildHandle(hwndRebar, Handle);
				if (searchBarIndex >= 0)
				{
					Win32.SendMessage(hwndRebar, (NppMsg)Win32.RB_SHOWBAND, searchBarIndex, show ? 1 : 0);

					if (searchBarIndex > 0 && show)
					{
						Win32.SendMessage(hwndRebar, (NppMsg)Win32.RB_MINIMIZEBAND, searchBarIndex - 1, 0);
						Win32.SendMessage(hwndRebar, (NppMsg)Win32.RB_MAXIMIZEBAND, searchBarIndex - 1, 1);
					}

					return;
				}

				// not yet inserted

				band.fMask 		= Win32.RBBIM_CHILD | Win32.RBBIM_SIZE | Win32.RBBIM_IDEALSIZE | Win32.RBBIM_CHILDSIZE;
				band.fStyle 	= Win32.RBBS_GRIPPERALWAYS;
				band.hwndChild 	= Handle;
				band.cx 		= Size.Width;
				band.cxIdeal 	= Size.Width;
				band.cxMinChild = 120;
				band.cyMinChild = frmCount.Height;
				band.cyMaxChild = frmCount.Height;
				band.cyChild 	= 0;
				band.cyIntegral = 0;

				Width = band.cxMinChild;

				if (!show)
					band.fStyle |= Win32.RBBS_HIDDEN;

				int count = (int)Win32.SendMessage(hwndRebar, (NppMsg)Win32.RB_GETBANDCOUNT, 0, 0);
				Win32.SendMessage(hwndRebar, Win32.RB_INSERTBANDW, count, ref band);
				if (count > 0 && show)
				{
					Win32.SendMessage(hwndRebar, (NppMsg)Win32.RB_MINIMIZEBAND, count - 1, 0);
					Win32.SendMessage(hwndRebar, (NppMsg)Win32.RB_MAXIMIZEBAND, count - 1, 1);
				}
			}
			finally
			{
				currentlyCheckingToolbarVisiblity = false;
			}
		}

		private IntPtr GetNppMainWindow()
		{
			IntPtr dummy;
			IntPtr thisThread = Win32.GetWindowThreadProcessId(Handle, out dummy);
			IntPtr parent = PluginBase.nppData._nppHandle;
			while (parent != IntPtr.Zero)
			{
				IntPtr grandParent = Win32.GetParent(parent);

				if (Win32.GetWindowThreadProcessId(grandParent, out dummy) != thisThread)
					break;

				parent = grandParent;
			}

			return parent;
		}

		private bool suppressKeyPress;

		private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (suppressKeyPress)
			{
				suppressKeyPress = false;
				e.Handled = true;
			}
		}

		private void toolbarShownCanary_Paint(object sender, PaintEventArgs e)
		{
			CheckToolbarVisiblity();
		}

		private void SearchForm_SizeChanged(object sender, EventArgs e)
		{
			CheckToolbarVisiblity();
		}
	}
}
