
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PUBGMESP.SigScanSharp;
using ShpVector3 = SharpDX.Vector3;
using ShpVector2 = SharpDX.Vector2;
namespace PUBGMESP
{
    public partial class MainForm : Form
    {
        #region Modules
        SigScanSharp sigScan;
        GameMemSearch ueSearch;
        ESPForm espForm;
        AimbotForm aimbotForm;
        #endregion

        #region Variables
        const string WINDOW_NAME = "腾讯手游助手【极速傲引擎】";
        const string WINDOW_NAME_G = "Gameloop【Turbo AOW Motoru】";
        IntPtr hwnd = IntPtr.Zero;
        RECT rect;
        long uWorld;
        long uWorlds;
        long uLevel;
        long gNames;
        long viewWorld;
        long gameInstance;
        long playerController;
        long playerCarry;
        int myTeamID;
        long uMyself;
        long myWorld;
        long uCamera;
        long uCursor;
        long uMyObject;
        Vector3 myObjectPos;
        long entityEntry;
        long entityCount;
        #endregion

        public MainForm()
        {
            InitializeComponent();
        }s

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void Btn_Activate_Click(object sender, EventArgs e)
        {
            // Enable Debug Privilige
            EnableDebugPriv();
            // Get Window Handle
            hwnd = FindWindow("TXGuiFoundation", "Gameloop【Turbo AOW Engine-4.4】");
            Console.WriteLine(hwnd);
            if (hwnd == IntPtr.Zero)
            {
                hwnd = FindWindow("TXGuiFoundation", WINDOW_NAME_G);
                if (hwnd == IntPtr.Zero)
                {
                    MessageBox.Show("Please Open Emulator First!!!");
                    return;
                }
            }
            hwnd = FindWindowEx(hwnd, 0, "AEngineRenderWindowClass", "AEngineRenderWindow");

            // Find true aow_exe process
            var aowHandle = FindTrueAOWHandle();

            // Initialize Memory
            Mem.Initialize(aowHandle);
            if (Mem.m_pProcessHandle == IntPtr.Zero)
            {
                MessageBox.Show("Error", "Cannot initialize simulator memory, please restart simulator then retry");
                return;
            }
            else
            {
                // Initialize SigScan
                sigScan = new SigScanSharp(Mem.m_pProcessHandle);
            }

            // Find UWorld Offset
            ueSearch = new GameMemSearch(sigScan);
            var cands = ueSearch.ViewWorldSearchCandidates();
            viewWorld = ueSearch.GetViewWorld(cands);
            uWorld = viewWorld - 4416288;
            gNames = viewWorld - 4453088;
            if (uWorld > 0)
            {
                // Start Drawing ESP
                LoopTimer.Enabled = true;
                UpdateTimer.Enabled = true;
                GetWindowRect(hwnd, out rect);
                espForm = new ESPForm(rect, ueSearch);
                aimbotForm = new AimbotForm(rect, ueSearch);
                new Thread(ESPThread).Start();
                new Thread(AimbotThread).Start();
                new Thread(InfoThread).Start();
                Btn_Activate.Enabled = false;
                Btn_Activate.Text = "Injected";
            }
            else
            {
                MessageBox.Show("Unable to initialize, please check if simulator and game is running");
            }
        }

        private void InfoThread()
        {
            // offset
            int controllerOffset, posOffset, healthOffset, nameOffset, teamIDOffset, poseOffset, statusOffset;
            controllerOffset = 96;
            posOffset = 336;
            healthOffset = 2064;
            nameOffset = 1512;
            teamIDOffset = 1552;
            statusOffset = 868;
            poseOffset = 288;
            while (true)
            {
                // Read Basic Offset
                uWorlds = Mem.ReadMemory<int>(uWorld);
                uLevel = Mem.ReadMemory<int>(uWorlds + 32);
                gameInstance = Mem.ReadMemory<int>(uWorlds + 36);
                playerController = Mem.ReadMemory<int>(gameInstance + controllerOffset);
                playerCarry = Mem.ReadMemory<int>(playerController + 32);
                uMyObject = Mem.ReadMemory<int>(playerCarry + 792); //788 old value
                //uMyself = Mem.ReadMemory<int>(uLevel + 124);
                //uMyself = Mem.ReadMemory<int>(uMyself + 36);
                //uMyself = Mem.ReadMemory<int>(uMyself + 312);
                //uCamera = Mem.ReadMemory<int>(playerCarry + 804) + 832;
                //uCursor = playerCarry + 732;
                //myWorld = Mem.ReadMemory<int>(uMyObject + 312);
                //myObjectPos = Mem.ReadMemory<Vector3>(myWorld + posOffset);
                entityEntry = Mem.ReadMemory<int>(uLevel + 112);
                entityCount = Mem.ReadMemory<int>(uLevel + 120); //116 old value
                // Initilize Display Data
                DisplayData data = new DisplayData(viewWorld, uMyObject);
                List<PlayerData> playerList = new List<PlayerData>();
                List<ItemData> itemList = new List<ItemData>();
                List<VehicleData> vehicleList = new List<VehicleData>();
                List<BoxData> boxList = new List<BoxData>();
                List<GrenadeData> grenadeList = new List<GrenadeData>();
                for (int i = 0; i < entityCount; i++)
                {
                    long entityAddv = Mem.ReadMemory<int>(entityEntry + i * 4);
                    long entityStruct = Mem.ReadMemory<int>(entityAddv + 16);
                    string entityType = GameData.GetEntityType(gNames, entityStruct);
                    if (Settings.PlayerESP)
                    {
                        // if entity is player
                        if (GameData.IsPlayer(entityType))
                        {
                            //Console.WriteLine(entityType);
                            long playerWorld = Mem.ReadMemory<int>(entityAddv + 312);
                            // read player info
                            // dead player continue
                            int status = Mem.ReadMemory<int>(playerWorld + statusOffset);
  
                            if (status == 6)
                                continue;
                                
                            // Enemy weapon
                            var enemy_weapon = GameData.GetEntityType(gNames, Mem.ReadMemory<int>(Mem.ReadMemory<int>(entityAddv + 5124) + 16));
                            if (string.IsNullOrEmpty(enemy_weapon))
                                enemy_weapon = "Fist";
                            else
                            {
                                try
                                {
                                    var enemy_weaponList = enemy_weapon.Split('_').ToList();
                                    enemy_weapon = enemy_weaponList[2];
                                }
                                catch
                                {
                                    enemy_weapon = "Unknown";
                                }

                            }
                            Console.WriteLine(enemy_weapon);

                            string name = Encoding.Unicode.GetString(Mem.ReadMemory(Mem.ReadMemory<int>(entityAddv + nameOffset), 32));
                            name = name.Substring(0, name.IndexOf('\0'));
                            PlayerData playerData = new PlayerData
                            {
                                Type = entityType,
                                Address = entityAddv,
                                Position = Mem.ReadMemory<ShpVector3>(playerWorld + posOffset),
                                Status = status,
                                Pose = Mem.ReadMemory<int>(playerWorld + poseOffset),
                                IsRobot = Mem.ReadMemory<int>(entityAddv + 692) == 0 ? true : false,
                                Health = Mem.ReadMemory<float>(entityAddv + healthOffset),
                                Name = name,
                                TeamID = Mem.ReadMemory<int>(entityAddv + teamIDOffset),
                                //IsTeam = isTeam
                            };
                            if (playerData.Address == uMyObject || playerData.Address == uMyself)
                            {
                                myTeamID = playerData.TeamID;
                                continue;
                            }
                            if (playerData.TeamID == myTeamID)
                                continue;
                            //Console.WriteLine(entityType);
                            playerList.Add(playerData);
                            continue;
                        }
                    }
                    if (Settings.ItemESP)
                    {
                        // check if this entity is item
                        Item item = GameData.GetItemType(entityType);
                        if (item != Item.Useless)
                        {
                            // Read Item Info
                            ItemData itemData = new ItemData
                            {
                                Name = item.GetDescription(),
                                Position = Mem.ReadMemory<ShpVector3>(Mem.ReadMemory<int>(entityAddv + 312) + posOffset),
                                Type = item
                            };
                            itemList.Add(itemData);
                        }
                        // check if this entity is box
                        if (GameData.IsBox(entityType))
                        {
                            // Read Box Info
                            long boxEntity = Mem.ReadMemory<int>(entityAddv + 312);
                            BoxData boxData = new BoxData();
                            boxData.Position = Mem.ReadMemory<ShpVector3>(boxEntity + posOffset);
                            boxList.Add(boxData);
                            continue;
                        }
                    }
                    if (Settings.VehicleESP)
                    {
                        Vehicle vehicle = GameData.GetVehicleType(entityType);
                        if (vehicle != Vehicle.Unknown)
                        {
                            // Read Vehicle Info
                            VehicleData vehicleData = new VehicleData
                            {
                                Position = Mem.ReadMemory<ShpVector3>(Mem.ReadMemory<int>(entityAddv + 312) + posOffset),
                                Type = vehicle,
                                Name = vehicle.GetDescription()
                            };
                            vehicleList.Add(vehicleData);
                            continue;
                        }
                    }
                    // check if the entity is a grenade
                    Grenade grenade = GameData.GetGrenadeType(entityType);
                    if (grenade != Grenade.Unknown)
                    {
                        long grenadeEntity = Mem.ReadMemory<int>(entityAddv + 312);
                        GrenadeData greData = new GrenadeData
                        {
                            Type = grenade,
                            Position = Mem.ReadMemory<ShpVector3>(grenadeEntity + posOffset)
                        };
                        grenadeList.Add(greData);
                    }
                }
                data.Players = playerList.ToArray();
                data.Items = itemList.ToArray();
                data.Vehicles = vehicleList.ToArray();
                data.Boxes = boxList.ToArray();
                data.Grenades = grenadeList.ToArray();
                espForm.UpdateData(data);
                aimbotForm.UpdateData(data);
                Thread.Sleep(10);
            }
        }

        private void ESPThread()
        {
            espForm.Initialize();
            while (true)
            {
                espForm.Update();
                //Thread.Sleep(10);
            }
        }
        private void AimbotThread()
        {
            aimbotForm.Initialize();
            while (true)
            {
                aimbotForm.Update();
                //Thread.Sleep(10);
            }
        }
        private IntPtr FindTrueAOWHandle()
        {
            IntPtr aowHandle = IntPtr.Zero;
            uint maxThread = 0;
            IntPtr handle = CreateToolhelp32Snapshot(0x2, 0);
            if ((int)handle > 0)
            {
                ProcessEntry32 pe32 = new ProcessEntry32();
                pe32.dwSize = (uint)Marshal.SizeOf(pe32);
                int bMore = Process32First(handle, ref pe32);
                while (bMore == 1)
                {
                    IntPtr temp = Marshal.AllocHGlobal((int)pe32.dwSize);
                    Marshal.StructureToPtr(pe32, temp, true);
                    ProcessEntry32 pe = (ProcessEntry32)Marshal.PtrToStructure(temp, typeof(ProcessEntry32));
                    Marshal.FreeHGlobal(temp);
                    if (pe.szExeFile.Contains("aow_exe.exe") && pe.cntThreads > maxThread)
                    {
                        maxThread = pe.cntThreads;
                        aowHandle = (IntPtr)pe.th32ProcessID;
                    }

                    bMore = Process32Next(handle, ref pe32);
                }
                CloseHandle(handle);
            }
            return aowHandle;
        }


        private bool EnableDebugPriv()
        {
            IntPtr hToken = IntPtr.Zero;
            if (!OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref hToken))
            {
                return false;
            }
            LUID luid = new LUID();
            if (!LookupPrivilegeValue(null, "SeDebugPrivilege", ref luid))
            {
                CloseHandle(hToken);
                return false;
            }
            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();
            tp.PrivilegeCount = 1;
            tp.Privileges = new LUID_AND_ATTRIBUTES[1];
            tp.Privileges[0].Luid = luid;
            tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
            if (!AdjustTokenPrivileges(hToken, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero))
            {
                return false;
            }
            CloseHandle(hToken);
            return true;
        }



        #region WIN32 API
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        [
         return: MarshalAs(UnmanagedType.Bool)
        ]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]


        private static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll")]
        public static extern bool GetAsyncKeyState(Keys vKey);

        private const int TOKEN_ADJUST_PRIVILEGES = 0x0020;
        private const int TOKEN_QUERY = 0x00000008;
        private const int SE_PRIVILEGE_ENABLED = 0x00000002;

        [DllImport("advapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);

        [DllImport("kernel32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        private static extern bool CloseHandle(IntPtr handle);

        [StructLayout(LayoutKind.Sequential)]
        private struct LUID
        {
            public UInt32 LowPart;
            public Int32 HighPart;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public UInt32 Attributes;
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID lpLuid);

        struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            [MarshalAs(UnmanagedType.ByValArray)]
            public LUID_AND_ATTRIBUTES[] Privileges;
        }
        // Use this signature if you want the previous state information returned
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
           [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
           ref TOKEN_PRIVILEGES NewState,
           UInt32 BufferLengthInBytes,
           IntPtr prev,
           IntPtr relen);

        [DllImport("KERNEL32.DLL ")]
        public static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processid);
        [DllImport("KERNEL32.DLL ")]
        public static extern int Process32First(IntPtr handle, ref ProcessEntry32 pe);
        [DllImport("KERNEL32.DLL ")]
        public static extern int Process32Next(IntPtr handle, ref ProcessEntry32 pe);

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessEntry32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szExeFile;
        };
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);
        #endregion

        private void Loop_Tick(object sender, EventArgs e)
        {
            GetWindowRect(hwnd, out rect);
            if (espForm != null)
            {
                espForm._window.FitToWindow(hwnd, true);
            }
            if (aimbotForm != null)
            {
                aimbotForm._window.FitToWindow(hwnd, true);
            }
        }

        private void Update_Tick(object sender, EventArgs e)
        {
            if (GetAsyncKeyState(Keys.End))
            {
                System.Environment.Exit(-1);
            }
            if (GetAsyncKeyState(Keys.End))
            {
                this.Close();
            }
            if (GetAsyncKeyState(Keys.Home))
            {
                Settings.ShowMenu = !Settings.ShowMenu;
            }
            if (GetAsyncKeyState(Keys.NumPad1))
            {
                Settings.PlayerESP = !Settings.PlayerESP;
            }
            if (GetAsyncKeyState(Keys.NumPad2))
            {
                Settings.PlayerBox = !Settings.PlayerBox;
            }
            if (GetAsyncKeyState(Keys.NumPad3))
            {
                Settings.PlayerBone = !Settings.PlayerBone;
            }
            if (GetAsyncKeyState(Keys.NumPad4))
            {
                Settings.PlayerLines = !Settings.PlayerLines;
            }
            if (GetAsyncKeyState(Keys.NumPad5))
            {
                Settings.PlayerHealth = !Settings.PlayerHealth;
            }
            if (GetAsyncKeyState(Keys.NumPad6))
            {
                Settings.ItemESP = !Settings.ItemESP;
            }
            if (GetAsyncKeyState(Keys.NumPad7))
            {
                Settings.VehicleESP = !Settings.VehicleESP;
            }
            if (GetAsyncKeyState(Keys.NumPad8))
            {
                Settings.Player3dBox = !Settings.Player3dBox;
            }
            if (GetAsyncKeyState(Keys.F5))
            {
                Settings.aimEnabled = !Settings.aimEnabled;
            }
            if (GetAsyncKeyState(Keys.F6))
            {
                Settings.bDrawFow = !Settings.bDrawFow;
            }
        }
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop ESP
            System.Environment.Exit(-1);
        }
    }
}
