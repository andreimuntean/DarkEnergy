using System;
using System.Diagnostics;
using SharpDX;

namespace DarkEnergy
{
    static class ExceptionManager
    {
        public static void Log(string message)
        {
            Debugger.Log(5, "Error", "\n" + message);
        }

        public static void Log(Exception exception)
        {
            Debugger.Log(5, "Error", "\n" + exception.Message);
        }

        public static void Log(SharpDXException exception)
        {
            Debugger.Log(5, "DirectX Error", "\n" + exception.Message);
        }
    }
}
