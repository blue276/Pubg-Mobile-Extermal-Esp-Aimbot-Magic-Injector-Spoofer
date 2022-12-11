using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MagicaTrainer
{
	class Program
	{
		private static bool _exit = false;
		private List<ConsoleKey> _keyQueue;
		private static double _speedDefaultValue;
		private static bool speedHackEnter;
		private static void Main(string[] args)
		{
			int defaultHeight = Console.WindowHeight;
			Console.WindowHeight = 35;
			Console.CursorVisible = false;
			Console.Write(" " +
			              "                                                                               \n" +
			              "                                   ╦                                           \n" +
			              "                                  ▐▌$   ╔@╣▓▓▄╖,                               \n" +
			              "                                 ,█▓▓▄▓▓▓▓▒▓███▌▒▒▒$                           \n" +
			              "                                ▄█▓█████▓▒▓█████▒▒▒µ                           \n" +
			              "                               ▄████████▌▒█████▓▓▓▒╣                           \n" +
			              "                               ▀█████▓▓▓▒▒▒▓▓▒▓▓▓▓Ü▒`                          \n" +
			              "                               ▐███▓▓▓▌▒▒▒╫▄▓▓▓▓██Ü ░                          \n" +
			              "                               ▐▓█▓█▓▓▓╣▒▒╢▒▓▓▓▒█▀                             \n" +
			              "                               ▓████▓█▓▒▄╬╢╣▓▓█╣▓C                             \n" +
			              "                               █████▓█▓╫█▌╫▓▓█▓▓▓                              \n" +
			              "                               ███████▓▓▓▓▓███▓▓▌                              \n" +
			              "                ╓╗╖            █████████▓█▓╬▓▓▓▓▓                              \n" +
			              "               ▐▓▓▓▓       ,╓▄▓▓████████▓▒▒▓▓▓▓▓▓L                             \n" +
			              "                ▓▓▓▓µ ,╖@╫▓▓▓▓██████▓▓▓▓╢╢▓▓▓▓▓▓▓▓@,                           \n" +
			              "                ▓▓▓▓▓@╣╢▒▒▒▓▓▓█████████▓▓█▓█▓▓▓▓▓▓▓▓▓▄,                        \n" +sssssssssssssssssssss
			              "               ▄▓▓▓▓▓╣╢▓▓▓▓▓███▓███████████▓▓▓▓▓▓▓▓▓▓▓▓▓▄                      \n" +
			              "              ████▓▓▓▓▓▒▒▒▒▒██████▓██████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▄                    \n" +
			              "             ]██████▓▓▓▓▓▓▓▓▓██████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓g                  \n" +
			              "             ╫██████▓▓▓▓▓╢╢╣▓█████▓▓▓▓▓▓▓▓▓▓▓▓▓╣▓▓▓▓▓█▓▓▓▓▓▓▓▓╖                \n" +
			              "             ▓▓▓█████▓▓▓▓▓▄███████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▓▓▓▓▓                \n" +
			              "              ▓▓▓█████▓▓▓█████████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓███▓▓▓▓▓▓▓▓▓               \n" +
			              "               ╙▀█████████████████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▓▓▓▓▓▓               \n" +
			              "                   Y▓██████████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓████▓▓▓▓▓▓▓▓▓Ü              \n" +
			              "                   ,███████████▓▓▓█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▓▓▓▓▓U              \n" +
			              "                   █████▓██████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█▓▓▓▓▓▓▓▓▓▓               \n" +
			              "                   ███▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓               \n" +
			              "                   ▓███▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█▓▓▓▓▓▓▓█▓▓▀               \n" +
			              "                  ]▓▓██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▓▓▓█▓▓                \n" +
			              "                  ▓████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓███▓▓▓▓▓▓▓▓▓▓▓▓▓                  \n" +
			              "                  ╙▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀                   \n" +
			              "                     - THE OFFICIAL ALF SEAL OF APPROVAL -\n" + 
						 "                          developed by Simon Sessingø ");

			// Let people enjoy the title screen
			Thread.Sleep(5000);

			Console.Clear();

			Console.WindowHeight = defaultHeight;

			// Create new thread that listens for key inputs

			var keyboardThread = new Thread(StartKeyboardListener);
			keyboardThread.IsBackground = true;
			keyboardThread.Start();

			// Create new thread that hacks the game
			var trainerThread = new Thread(GameTrainer);
			trainerThread.IsBackground = true;
			trainerThread.Start();

			while (!_exit)
			{
				StringBuilder sf = new StringBuilder();
				sf.AppendLine("");
				sf.AppendLine("  [ MAGICKA 2 TRAINER ]  ");
				sf.AppendLine("");
				if (!Magicka.IsRunning())
				{
					sf.AppendLine("");
					sf.AppendLine("  Please launch Magicka 2...");
				}
				else
				{
					sf.AppendLine("  HP: " + Config.Health);
					sf.AppendLine("");

					sf.AppendLine(String.Format("  - Press 1 to {0} Health hack", ((Config.EnableHealthHack) ? "disable" : "enable")));

					if (Config.EnableSpeedHack)
					{
						sf.AppendLine(String.Format("  - Press 2 to disable speed hack (current speed: {0})", Config.Speed));
					}
					else
					{
						sf.AppendLine("  - Press 2 to enable speed hack");
					}

					if (speedHackEnter)
					{
						Console.WriteLine("");
						Console.Write("  Enter speed-value [1-30]: ");
						double newSpeed;
						if (double.TryParse(Console.ReadLine(), out newSpeed))
						{
							if (newSpeed > 0 && newSpeed <= 30)
							{
								speedHackEnter = false;
								Config.EnableSpeedHack = true;
								Config.Speed = newSpeed;
							}
						}
					}
				}

				Console.Clear();
				
				Console.WriteLine(sf);
				Thread.Sleep(100);
			}

		}

		private static void GameTrainer()
		{
			while (!_exit)
			{
				if (Magicka.IsRunning())
				{
					int baseAddr = (int) Magicka.GetProcess().MainModule.BaseAddress;

					int pointer = Trainer.ReadMultiLevelPointer("magicka2", baseAddr + 0x4611C4, 4, new[] {0x7b8, 0x68c, 0x14, 0x60});
					Config.Health = Trainer.ReadDouble("magicka2", pointer);

					if (Math.Abs(Config.Health) <= 0)
					{
						pointer = Trainer.ReadMultiLevelPointer("magicka2", baseAddr + 0x000F2A20, 4, new[] { 0x404, 0x58c, 0xcc, 0x194, 0x120 });
						Config.Health = Trainer.ReadDouble("magicka2", pointer);
					}

					if (Math.Abs(Config.Health) <= 0)
					{
						pointer = Trainer.ReadMultiLevelPointer("magicka2", baseAddr + 0x000F2A20, 4, new[] { 0x404, 0x58c, 0xcc, 0x194, 0x120 });
						Config.Health = Trainer.ReadDouble("magicka2", pointer);
					}

					if (Math.Abs(Config.Health) <= 0)
					{
						pointer = Trainer.ReadMultiLevelPointer("magicka2", baseAddr + 0x000F2A20, 4, new[] { 0x420, 0x5ac, 0xcc, 0x194, 0x120 });
						Config.Health = Trainer.ReadDouble("magicka2", pointer);
					}

					if (Math.Abs(Config.Health) <= 0)
					{
						pointer = Trainer.ReadMultiLevelPointer("magicka2", baseAddr + 0x000F2A20, 4, new[] { 0x420, 0x5ac, 0xcc, 0x194, 0x120 });
						Config.Health = Trainer.ReadDouble("magicka2", pointer);
					}
					
					// Only write when the information is availible, otherwise the system might
					// see the memory as unavailible and use different slots.

					if (_speedDefaultValue <= 0)
					{
						_speedDefaultValue = Trainer.ReadDouble("magicka2", pointer + 0x00F0);
					}

					if (Config.EnableHealthHack)
					{
						// Health hack
						Trainer.WriteDouble("magicka2", pointer, 1000);
					}

					if (Config.Health > 0)
					{
						// Speed hack
						Trainer.WriteDouble("magicka2", pointer + 0x00F0, (Config.EnableSpeedHack) ? Config.Speed : _speedDefaultValue);
					}
				}
			}
		}

		private static void StartKeyboardListener()
		{
			while (!_exit)
			{
				ConsoleKeyInfo key = System.Console.ReadKey(true);

				switch (key.Key)
				{
					case ConsoleKey.D1:
						Config.EnableHealthHack = (!Config.EnableHealthHack);
						break;
					case ConsoleKey.D2:
						if (Config.EnableSpeedHack)
						{
							Config.EnableSpeedHack = false;
						}
						else
						{
							speedHackEnter = true;
						}
						break;
					case ConsoleKey.Escape:
						_exit = true;
						break;
				}
			}
		}
	}
}
