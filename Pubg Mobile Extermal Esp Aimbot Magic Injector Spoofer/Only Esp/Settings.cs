using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUBGMESP
{
    internal class Settings
    {
        public static bool PlayerESP = true;
        public static bool PlayerBone = false;
        public static bool PlayerBox = false;
        public static bool Player3dBox = true;
        public static bool PlayerLines = true;
        public static bool PlayerHealth = false;
        public static bool ItemESP = false;
        public static bool VehicleESP = false;
        public static bool ShowMenu = true;

        // aimbot
        public static bool aimEnabled = true;
        public static bool bDrawFow = true;
        public static int bSmooth = 11;
        public static int bFovInt = 2;
        public static int bPredict = 1;
        public static int bYAxis = 2;
        public static int bAimKeyINT = 2;
        public static string[] aimkeys = { "CAPSLOCK", "LBUTTON", "RBUTTON", "LSHIFT", "V", "E", "Q" };
        public static System.Windows.Forms.Keys[] bAimKeys = new System.Windows.Forms.Keys[] { System.Windows.Forms.Keys.CapsLock, System.Windows.Forms.Keys.LButton, System.Windows.Forms.Keys.RButton, System.Windows.Forms.Keys.LShiftKey, System.Windows.Forms.Keys.V, System.Windows.Forms.Keys.E, System.Windows.Forms.Keys.Q };
        public static float[] bFovArray = new float[]
        {
            60f,s
            90f,
            120f,
            160f,
            300f,
            300f
        };
    }
}
