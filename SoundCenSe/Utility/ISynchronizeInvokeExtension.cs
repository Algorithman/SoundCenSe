// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: ISynchronizeInvokeExtension.cs
// 
// Last modified: 2016-07-30 19:37

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