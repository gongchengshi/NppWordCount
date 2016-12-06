using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NppWordCount.Forms;
using NppPluginNET;
using System.Runtime.InteropServices;

namespace NppWordCount
{
    class Main
    {
        internal const string PluginName = "Live Word Count";

		internal static WordCountForm WordCountForm { get; private set; }

        internal static void CommandMenuInit()
        {
            PluginBase.SetCommand(0, "Toggle Word Count", ToggleWordCount);
        }

		internal static string GetNativeLangXml()
		{
			// %appdata%\Notepad++\nativeLang.xml

			string result = Path.Combine(
				Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					"Notepad++"),
				"nativeLang.xml");

			if (File.Exists(result))
				return result;

			StringBuilder sb = new StringBuilder(1024);
			Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETNPPDIRECTORY, sb.Capacity, sb);

			string nppDir = sb.ToString();
			result = Path.Combine(nppDir, "nativeLang.xml");
			if (File.Exists(result))
				return result;

			result = Path.Combine(Path.Combine(nppDir, "localization"), "english.xml");
			if (File.Exists(result))
				return result;

			return null;
		}

		internal static IntPtr FindPluginMenuItem(uint commandId, out uint index)
		{
			IntPtr pluginsMenu = Win32.SendMessage(
				PluginBase.nppData._nppHandle,
				NppMsg.NPPM_GETMENUHANDLE,
				(int)NppMsg.NPPPLUGINMENU,
				0);

			index = 0;

			if (pluginsMenu == IntPtr.Zero)
				return IntPtr.Zero;

			for (uint i = 0; i < Win32.GetMenuItemCount(pluginsMenu); ++i)
			{
				IntPtr subMenu = Win32.GetSubMenu(pluginsMenu, i, true);
				if (subMenu == IntPtr.Zero)
					continue;

				for (uint j = 0; j < Win32.GetMenuItemCount(subMenu); ++j)
				{
					if (Win32.GetMenuItemId(subMenu, j, true) == commandId)
					{
						index = j;
						return subMenu;
					}
				}
			}

			return IntPtr.Zero;
		}

		internal static void PluginReady()
		{
			WordCountForm = new WordCountForm();

			WordCountForm.CheckToolbarVisiblity();

			Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
		}

        internal static void PluginCleanUp()
        {
        }

		public static IntPtr GetMainWindow()
		{
			IntPtr dummy;
			IntPtr thisThread = Win32.GetWindowThreadProcessId(PluginBase.nppData._nppHandle, out dummy);
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

		internal static void MakeNppOwnerOf(Form form)
		{
			Win32.SetWindowLongPtr(form.Handle, Win32.GWL_HWNDPARENT, GetMainWindow());
		}

		internal static void ToggleWordCount()
        {
            WordCountForm = null;
        }

        internal static Timer Timer = new Timer();

        static Main()
        {
            Timer.Interval = 500;
            Timer.Tick += Timer_Tick;
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();
            CountChars();
        }

        private static void CountChars()
        {
            var curSci = PluginBase.GetCurrentScintilla();
            var length = (int)Win32.SendMessage(curSci, SciMsg.SCI_GETLENGTH, 0, 0);

            var text = new StringBuilder(length);

            Win32.SendMessage(curSci, SciMsg.SCI_GETTEXT, length, text);

            var split = text.ToString().Split(' ', '\n');

            var numWords = split.Count(word => !string.IsNullOrWhiteSpace(word));

            WordCountForm.txtWordCount.Text = "Word Count: " + numWords;
        }

        internal static void RecalcWordCount()
        {
            if (!Timer.Enabled)
            {
                Timer.Start();
            }
        }
    }
}