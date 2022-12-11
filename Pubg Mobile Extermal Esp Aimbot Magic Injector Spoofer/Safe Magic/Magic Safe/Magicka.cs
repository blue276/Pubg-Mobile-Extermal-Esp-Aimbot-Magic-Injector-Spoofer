using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MagicaTrainer
{
	public class Magicka
	{
		public static bool IsRunning()
		{
			var game = Process.GetProcessesByName("Magicka2");
			return (game.Length > 0);
		}

		public static Process GetProcess()
		{
			return Process.GetProcessesByName("Magicka2").FirstOrDefault();
		}
	}
}
s