using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace kuk_duk
{
    class MouseHook
    {
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        //Declare hook handle as int.
        static int hHook = 0;

        //Declare mouse hook constant.
        //For other hook types, you can obtain these values from Winuser.h in Microsoft SDK.
        public const int WH_MOUSE = 14; //WH_MOUSE_LL

        //Declare MouseHookProcedure as HookProc type.
        static HookProc MouseHookProcedure;

        protected static bool IsRegistered = false;

        //Declare wrapper managed POINT class.
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }
        
        //Declare wrapper managed MouseHookStruct class.
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        //Import for SetWindowsHookEx function.
        //Use this function to install thread-specific hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
        IntPtr hInstance, int threadId);

        //Import for UnhookWindowsHookEx.
        //Call this function to uninstall the hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //Import for CallNextHookEx.
        //Use this function to pass the hook information to next hook procedure in chain.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode,
        IntPtr wParam, IntPtr lParam);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public static void MouseHookStart()
        {
            MouseHookProcedure = new HookProc(MouseHookProc);
            hHook = SetWindowsHookEx(WH_MOUSE, MouseHookProcedure, GetModuleHandle("user32"), 0);    
        }
        public static int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            
            //Marshall the data from callback.
            MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
            if(!IsRegistered)
                return 1;
            return 0;
        }
        public static void MouseHookEnd()
        {
            IsRegistered = true;
        }
        public static void set_regist()
        {
            IsRegistered = false;
        }
    }
}
