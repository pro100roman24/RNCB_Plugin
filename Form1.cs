using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RNCB_Plugin
{
    public partial class Form1 : Form
    {
        public IntPtr front1;
        public Size Resolution;
        public const decimal Xconst = 2.97m;  //это отношение ширины и высоты кастомного окна к ширине и высоте дисплея, чтоб на любом дисплее размер подгонялся автоматически
        public const decimal Yconst = 2.343m; //можно менять эти значения, чтоб сделать своё окно другого размера
        public int output;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public static bool setParentWindow(IntPtr hWndChild, IntPtr hWndNewParent)
        {
            IntPtr previousParent = SetParent(hWndChild, hWndNewParent);
            return (previousParent == null ? false : true);
        }
        public Form1(IntPtr front, string param1, int param2)
        {
            InitializeComponent();
            front1 = front;
            Resolution = Screen.PrimaryScreen.Bounds.Size;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetForegroundWindow(this.Handle);
            setParentWindow(this.Handle, front1);
            SetClientSizeCore(Convert.ToInt32(Resolution.Width / Xconst), Convert.ToInt32(Resolution.Height / Yconst)); //тут задаем ширину и высоту окна разделив ширину и высоту дисплея на наши константы
            SetDesktopLocation((Resolution.Width / 2 - Convert.ToInt32(Resolution.Width / Xconst / 2)), (Resolution.Height / 2 - Convert.ToInt32(Resolution.Height / Yconst / 2))); //тут выравниваем наше окно по центру дисплея
            TopMost = true;
            TopLevel = true;
            WindowState = FormWindowState.Minimized;
            WindowState = FormWindowState.Normal;
            BringToFront();
            Focus();
        }
    }
}
