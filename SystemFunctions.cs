using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_Server_Launcher
{
    class SystemFunctions
    {
        public SystemFunctions()
        {
        }
        public string GetOSBit()
        {
            bool is64bit = Is64Bit();
            if (is64bit)
                return "x64";
            else
                return "x32";
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        public bool IsJava7Installed()
        {
            return (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Java\\jre7\\bin\\java.exe"));
        }
        private static bool Is64Bit()
        {
            bool retVal;
            IsWow64Process(Process.GetCurrentProcess().Handle, out retVal);
            return retVal;
        }
    }
}
