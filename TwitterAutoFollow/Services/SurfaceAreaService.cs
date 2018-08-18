using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitterAutoFollow.Services
{
    public static class SurfaceAreaService
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        public static void MouseClick(Point p)
        {
            SetCursorPos(p.X, p.Y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, p.X, p.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, p.X, p.Y, 0, 0);
        }
        public static void PageDown()
        {
            SendKeys.SendWait("{PGDN}");
            SendKeys.Flush();
        }
    }
}
