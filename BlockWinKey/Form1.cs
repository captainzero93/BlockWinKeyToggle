using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BlockWinKey
{
    public partial class Form1 : Form
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int VK_LWIN = 0x5B;
        private const int VK_RWIN = 0x5C;
        private const int MAX_LOG_LINES = 100;

        private readonly LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private volatile bool _isWindowsKeyBlocked;
        private readonly StringBuilder _logBuilder;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public Form1()
        {
            _logBuilder = new StringBuilder(128);
            InitializeComponent();
            _proc = HookCallback;
            InitializeHook();
            InitializeTrayIcon();
        }

        private void InitializeHook()
        {
            _hookID = SetHook(_proc);
            Log("Hook initialized");
        }

        private void InitializeTrayIcon()
        {
            trayIcon.Icon = System.Drawing.SystemIcons.Application;
            trayIcon.Visible = true;
            trayIcon.Text = "Windows Key Blocker";

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Show", null, (s, e) => { Show(); WindowState = FormWindowState.Normal; });
            contextMenu.Items.Add("Exit", null, Exit);

            trayIcon.ContextMenuStrip = contextMenu;
            trayIcon.DoubleClick += (s, e) => { Show(); WindowState = FormWindowState.Normal; };
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            Process curProcess = Process.GetCurrentProcess();
            ProcessModule curModule = curProcess.MainModule;
            try
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
            finally
            {
                if (curProcess != null) curProcess.Dispose();
                if (curModule != null) curModule.Dispose();
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && _isWindowsKeyBlocked &&
                (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == VK_LWIN || vkCode == VK_RWIN)
                {
                    Log($"Windows key blocked");
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private void toggleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _isWindowsKeyBlocked = toggleCheckBox.Checked;
            trayIcon.Text = $"Windows Key Blocker ({(_isWindowsKeyBlocked ? "ON" : "OFF")})";
            Log($"Windows key blocking {(_isWindowsKeyBlocked ? "enabled" : "disabled")}");
        }

        private void Exit(object sender, EventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void Log(string message)
        {
            if (logTextBox.IsDisposed) return;

            _logBuilder.Clear()
                      .Append('[')
                      .Append(DateTime.Now.ToString("HH:mm:ss"))
                      .Append("] ")
                      .Append(message);

            if (logTextBox.InvokeRequired)
            {
                logTextBox.BeginInvoke(new Action(() => UpdateLog(_logBuilder.ToString())));
            }
            else
            {
                UpdateLog(_logBuilder.ToString());
            }
        }

        private void UpdateLog(string message)
        {
            logTextBox.AppendText(message + Environment.NewLine);

            var lines = logTextBox.Lines;
            if (lines.Length > MAX_LOG_LINES)
            {
                logTextBox.Lines = lines.Skip(lines.Length - MAX_LOG_LINES).ToArray();
            }

            logTextBox.SelectionStart = logTextBox.TextLength;
            logTextBox.ScrollToCaret();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

    }
}