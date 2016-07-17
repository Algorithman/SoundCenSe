// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: ISynchronizeInvokeExtension.cs
// 
// Last modified: 2016-07-17 22:06

using System.ComponentModel;
using System.Windows.Forms;

namespace SoundCenSe.Utility
{
    public static class ISynchronizeInvokeExtension
    {
        public static void InvokeIfRequired(this ISynchronizeInvoke obj,
            MethodInvoker action)
        {
            if (obj.InvokeRequired)
            {
                var args = new object[0];
                obj.Invoke(action, args);
            }
            else
            {
                action();
            }
        }
    }
}