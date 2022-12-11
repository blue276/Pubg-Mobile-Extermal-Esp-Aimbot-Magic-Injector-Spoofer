using System.Diagnostics;
using System.Runtime.InteropServices;
using System;

namespace MagicaTrainer
{
	// TODO: Optimize - only create one instance of the process...
	public class Trainer
	{

		#region DLL Imports

		private const int PROCESS_ALL_ACCESS = 0x1F0FFF;

		[DllImport("kernel32")]
		private static extern int OpenProcess(int AccessType, int InheritHandle, int ProcessId);

		[DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
		private static extern byte WriteProcessMemoryByte(int Handle, int Address, ref byte Value, int Size,
			ref int BytesWritten);

		[DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
		private static extern int WriteProcessMemoryInteger(int Handle, int Address, ref int Value, int Size,
			ref int BytesWritten);

		[DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
		private static extern float WriteProcessMemoryFloat(int Handle, int Address, ref float Value, int Size,
			ref int BytesWritten);

		[DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
		private static extern double WriteProcessMemoryDouble(int Handle, int Address, ref double Value, int Size,
			ref int BytesWritten);


		[DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
		private static extern byte ReadProcessMemoryByte(int Handle, int Address, ref byte Value, int Size, ref int BytesRead);

		[DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
		private static extern int ReadProcessMemoryInteger(int Handle, int Address, ref int Value, int Size, ref int BytesRead);

		[DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
		private static extern float ReadProcessMemoryFloat(int Handle, int Address, ref float Value, int Size,
			ref int BytesRead);

		[DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
		private static extern double ReadProcessMemoryDouble(int Handle, int Address, ref double Value, int Size,
			ref int BytesRead);

		[DllImport("kernel32.dll")]
		public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer,
			UInt32 size, out IntPtr lpNumberOfBytesRead);

		[DllImport("kernel32")]
		private static extern int CloseHandle(int Handle);

		[DllImport("user32")]
		private static extern int FindWindow(string sClassName, string sAppName);

		[DllImport("user32")]
		private static extern int GetWindowThreadProcessId(int HWND, out int processId);


		[Flags]
		public enum ThreadAccess : int
		{
			TERMINATE = (0x0001),
			SUSPEND_RESUME = (0x0002),
			GET_CONTEXT = (0x0008),
			SET_CONTEXT = (0x0010),
			SET_INFORMATION = (0x0020),
			QUERY_INFORMATION = (0x0040),
			SET_THREAD_TOKEN = (0x0080),
			IMPERSONATE = (0x0100),
			DIRECT_IMPERSONATION = (0x0200)
		}

		[DllImport("kernel32.dll")]
		private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

		[DllImport("kernel32.dll")]
		private static extern uint SuspendThread(IntPtr hThread);

		[DllImport("kernel32.dll")]
		private static extern int ResumeThread(IntPtr hThread);

		private void SuspendProcess(int PID)
		{
			Process proc = Process.GetProcessById(PID);

			if (proc.ProcessName == string.Empty)
				return;

			foreach (ProcessThread pT in proc.Threads)
			{
				IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint) pT.Id);

				if (pOpenThread == IntPtr.Zero)
				{
					break;
				}

				SuspendThread(pOpenThread);
			}
		}

		public void ResumeProcess(int PID)
		{
			Process proc = Process.GetProcessById(PID);

			if (proc.ProcessName == string.Empty)
				return;

			foreach (ProcessThread pT in proc.Threads)
			{
				IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint) pT.Id);

				if (pOpenThread == IntPtr.Zero)
				{
					break;
				}

				ResumeThread(pOpenThread);
			}
		}

		#endregion


		#region Read

		public static Byte ReadInt8(string EXENAME, int Address)
		{
			byte Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							ReadProcessMemoryByte(Handle, Address, ref Value, 1, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		public static Int16 ReadInt16(string EXENAME, int Address)
		{
			Int16 Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							int Int32_Value = ReadInt32(EXENAME, Address);
							byte[] Int32_Value_Bytes = BitConverter.GetBytes(Int32_Value);
							Value = BitConverter.ToInt16(Int32_Value_Bytes, 0);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
				;
			}
			return Value;
		}

		public static Int32 ReadInt32(string EXENAME, int Address)
		{
			Int32 Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							ReadProcessMemoryInteger(Handle, Address, ref Value, 4, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		public static Int64 ReadInt64(string EXENAME, int Address)
		{
			Int64 Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							byte[] Bytes8 = ReadByteArray(EXENAME, Address, 8);
							Value = BitConverter.ToInt64(Bytes8, 0);
							//ReadProcessMemoryInteger(Handle, Address, ref Value, 4, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		public static float ReadFloat(string EXENAME, int Address)
		{
			float Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							ReadProcessMemoryFloat((int) Handle, Address, ref Value, 4, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		public static double ReadDouble(string EXENAME, int Address)
		{
			double Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							ReadProcessMemoryDouble((int) Handle, Address, ref Value, 8, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		#endregion

		public static int ReadMultiLevelPointer(string EXENAME, int MemoryAddress, uint bytesToRead, Int32[] offsetList)
		{
			Process[] Proc = Process.GetProcessesByName(EXENAME);

			int Bytes = 0;
			IntPtr procHandle = (IntPtr)OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
			IntPtr pointer = (IntPtr)0x0;
			//IF THE PROCESS isnt available we return nothing
			if (procHandle == IntPtr.Zero)
			{
				return 0;
			}

			byte[] btBuffer = new byte[bytesToRead];
			IntPtr lpOutStorage = IntPtr.Zero;

			int pointerAddy = MemoryAddress;
			//int pointerTemp = 0;
			for (int i = 0; i < (offsetList.Length); i++)
			{
				if (i == 0)
				{s
					ReadProcessMemory(
						procHandle,
						(IntPtr)(pointerAddy),
						btBuffer,
						(uint)btBuffer.Length,
						out lpOutStorage);
				}
				pointerAddy = (BitConverter.ToInt32(btBuffer, 0) + offsetList[i]);
				//string pointerAddyHEX = pointerAddy.ToString("X");

				ReadProcessMemory(
					procHandle,
					(IntPtr)(pointerAddy),
					btBuffer,
					(uint)btBuffer.Length,
					out lpOutStorage);
			}
			return pointerAddy;
		}

		#region ReadPointer

		public static Byte ReadPointerInt8(string EXENAME, int Pointer, int[] Offset)
		{
			byte Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger((int) Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							ReadProcessMemoryByte((int) Handle, Pointer, ref Value, 2, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		public static Int16 ReadPointerInt16(string EXENAME, int Pointer, int[] Offset)
		{
			Int16 Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger((int) Handle, Pointer, ref Pointer, 2, ref Bytes);
								Pointer += i;
							}
							ReadInt16(EXENAME, Pointer);
							//ReadProcessMemoryInteger((int)Handle, Pointer, ref Value, 4, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		public static int ReadPointerInt32(string EXENAME, int Pointer, int[] Offset)
		{
			int Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger((int) Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							ReadProcessMemoryInteger((int) Handle, Pointer, ref Value, 4, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		public static Int64 ReadPointerInt64(string EXENAME, int Pointer, int[] Offset)
		{
			Int64 Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger((int) Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							Value = ReadInt64(EXENAME, Pointer);
							//ReadProcessMemoryInteger((int)Handle, Pointer, ref Value, 4, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		public static float ReadPointerFloat(string EXENAME, int Pointer, int[] Offset)
		{
			float Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger((int) Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							ReadProcessMemoryFloat((int) Handle, Pointer, ref Value, 4, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		public static double ReadPointerDouble(string EXENAME, int Pointer, int[] Offset)
		{
			double Value = 0;
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger((int) Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							ReadProcessMemoryDouble((int) Handle, Pointer, ref Value, 8, ref Bytes);
							CloseHandle(Handle);
						}
					}
				}
				catch
				{
				}
			}
			return Value;
		}

		#endregion

		#region Write

		public static void WriteInt8(string EXENAME, int Address, byte Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							WriteProcessMemoryByte(Handle, Address, ref Value, 1, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		public static void WriteInt16(string EXENAME, int Address, Int16 Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							WriteInt16(EXENAME, Address, Value);
							//WriteProcessMemoryByte(Handle, Address, ref Value, 2, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		public static void WriteInt32(string EXENAME, int Address, int Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							WriteProcessMemoryInteger(Handle, Address, ref Value, 4, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		public static void WriteInt64(string EXENAME, int Address, Int64 Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							byte[] Bytes8 = BitConverter.GetBytes(Value);
							WriteByteArray(EXENAME, Address, Bytes8);
							//WriteProcessMemoryInteger(Handle, Address, ref Value, 4, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		public static void WriteFloat(string EXENAME, int Address, float Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							WriteProcessMemoryFloat(Handle, Address, ref Value, 4, ref Bytes);
						}
						CloseHandle(Handle);
					}

				}
				catch
				{
				}
			}
		}

		public static void WriteDouble(string EXENAME, int Address, double Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Bytes = 0;
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							WriteProcessMemoryDouble(Handle, Address, ref Value, 8, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		#endregion

		#region WritePointer

		public static void WritePointerInt8(string EXENAME, int Pointer, int[] Offset, byte Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							int Bytes = 0;
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger(Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							WriteProcessMemoryByte(Handle, Pointer, ref Value, 1, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		public static void WritePointerInt16(string EXENAME, int Pointer, int[] Offset, Int16 Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							int Bytes = 0;
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger(Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							WriteInt16(EXENAME, Pointer, Value);
							//WriteProcessMemoryInteger(Handle, Pointer, ref Value, 4, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		public static void WritePointerInt32(string EXENAME, int Pointer, int[] Offset, int Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							int Bytes = 0;
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger(Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							WriteProcessMemoryInteger(Handle, Pointer, ref Value, 4, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		public static void WritePointerInt64(string EXENAME, int Pointer, int[] Offset, Int64 Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							int Bytes = 0;
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger(Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							Trainer.WriteInt64(EXENAME, Pointer, Value);
							//WriteProcessMemoryInteger(Handle, Pointer, ref Value, 4, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		public static void WritePointerFloat(string EXENAME, int Pointer, int[] Offset, float Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							int Bytes = 0;
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger(Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							WriteProcessMemoryFloat(Handle, Pointer, ref Value, 4, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		public static void WritePointerDouble(string EXENAME, int Pointer, int[] Offset, double Value)
		{
			checked
			{
				try
				{
					Process[] Proc = Process.GetProcessesByName(EXENAME);
					if (Proc.Length != 0)
					{
						int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc[0].Id);
						if (Handle != 0)
						{
							int Bytes = 0;
							foreach (int i in Offset)
							{
								ReadProcessMemoryInteger(Handle, Pointer, ref Pointer, 4, ref Bytes);
								Pointer += i;
							}
							WriteProcessMemoryDouble(Handle, Pointer, ref Value, 8, ref Bytes);
						}
						CloseHandle(Handle);
					}
				}
				catch
				{
				}
			}
		}

		#endregion

		#region Others

		public static byte[] ReadByteArray(string EXENAME, int Address, int count)
		{
			if (count > 0)
			{
				try
				{
					byte[] value = new byte[count];
					for (int i = 0; i < count; i++)
					{
						value[i] = ReadInt8(EXENAME, Address + i);
					}
					return value;
				}
				catch
				{
				}
				;
			}
			return null;
		}

		public static void WriteByteArray(string EXENAME, int address, byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
			{
				WriteInt8(EXENAME, address + i, bytes[i]);
			}
		}

		public static string ReadStringUntilNULL(string EXENAME, int Address)
		{
			string value = "";
			bool endOfString = false;
			int counter = 0;
			while (!endOfString)
			{
				if (ReadInt8(EXENAME, Address + counter) > (byte) 0)
				{
					value += (char) ReadInt8(EXENAME, Address + counter);
				}
				else
				{
					return value;
				}
				counter++;
			}
			return value;
		}

		public static void WriteString(string EXENAME, int Address, string value)
		{
			if (value != null)
			{
				int counter = 0;
				foreach (char chr in value.ToCharArray())
				{
					Trainer.WriteInt8(EXENAME, Address + counter, (byte) chr);
					counter++;
				}
			}
		}

		public static void WriteNOPs(string EXENAME, int address, int count)
		{
			for (int i = 0; i < count; i++)
			{
				WriteInt8(EXENAME, address + i, 0x90);
			}
		}

		public static string CheckGame(string WindowTitle)
		{
			string result = "";
			checked
			{
				try
				{
					int Proc;
					int HWND = FindWindow(null, WindowTitle);
					GetWindowThreadProcessId(HWND, out Proc);
					int Handle = OpenProcess(PROCESS_ALL_ACCESS, 0, Proc);
					if (Handle != 0)
					{
						result = "Game is running...";
					}
					else
					{
						result = "Game is not running...";
					}
					CloseHandle(Handle);
				}
				catch
				{
				}
			}
			return result;
		}

		public static void WriteCodeInjection(string EXENAME, int InjectAddress, byte[] InjectCode, int CodeCaveAddress,
			byte[] CodeCaveCode)
		{
			checked
			{
				try
				{
					WriteByteArray(EXENAME, InjectAddress, InjectCode);
					WriteByteArray(EXENAME, CodeCaveAddress, CodeCaveCode);
				}
				catch
				{
				}
				;
			}
		}

		#endregion
	}
}