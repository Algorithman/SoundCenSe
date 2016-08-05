using System;

namespace Misc
{
	public class DwarfFortressRunningEventArgs : EventArgs
	{
		#region Properties

		public int ProcessId { get; set; }

		#endregion

		public DwarfFortressRunningEventArgs (int processId)
		{
			ProcessId = processId;
		}
	}
}

