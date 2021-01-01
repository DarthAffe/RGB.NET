using Microsoft.Win32;
using RGB.NET.Core;
using RGB.NET.Devices.EVGA.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.IO.Pipes;

namespace RGB.NET.Devices.EVGA
{
    /*
     * This class does something absolutely incredible and is guaranteed to impress and gain the interest of the women, men or other that you have your eye on.
     * 
     * Because this is an x86 (or AnyCPU or whatever) DLL, it cannot directly load in-process the assemblies in EVGA's PrecisionX which are all compiled x64.
     * 
     * To work around this:
     * 
     * This plugin embeds a copy of the x64 compiled EVGAProxy.exe as a resource in this x86 DLL.
     * 
     * When the DLL is constructed, it extracts EVGAProxy.exe to a temp folder and attemps to run it
     * 
     * It then uses some very rudimentary, potentially fragile named pipes to handle inter process communication.
     * 
     * The x64 EGVA libraries are copied around and loaded by EVGAProxy.exe in its x64 process, and simple command/responses are shuffled through the named pipe 
     *  streams to this plugin DLL.
     *  
     * If something goes wrong (or right) here or in EVGAProxy, it tries logging it to the user's temp folder into evgaproxy.log, but really what could go wrong?
     */
    public class EVGADeviceProvider : IRGBDeviceProvider
    {
        private string _evgaPath = null;
        private string _precisionExe = null;
        private string _precisionExeName = null;
        private Type _comInterface;
        private Process _proxyProc;
        private NamedPipeClientStream nps;
        private static byte[] MAGIC = new byte[] { 0xB, 0xE, 0xE, 0xF };
        private byte[] ReadMsg(Stream nps)
        {
            byte[] magic = new byte[4];
            int readlen = nps.Read(magic, 0, 4);
            if (readlen != 4 || !(magic[0] == 0xB && magic[1] == 0xE && magic[2] == 0xE && magic[3] == 0xF))
            {
                throw new Exception("No beef");
            }

            byte[] bMsg = new byte[16];
            readlen = nps.Read(bMsg, 0, bMsg.Length);
            if (readlen != 16)
            {
                throw new Exception("Bad data");
            }

            return bMsg;
        }
        private int GetDeviceCount()
        {
            nps.Write(MAGIC, 0, 4);
            byte[] bMsg = new byte[16];
            bMsg[0] = 1;
            nps.Write(bMsg, 0, bMsg.Length);
            bMsg = ReadMsg(nps);
            if (bMsg[0] != 2)
            {
                throw new Exception("Wrong response");
            }
            return bMsg[1];
        }

        private int GetLedCount(int deviceId)
        {
            nps.Write(MAGIC, 0, 4);
            var bMsg = new byte[16];
            bMsg[0] = 5;
            bMsg[1] = (byte)deviceId;
            nps.Write(bMsg, 0, bMsg.Length);
            bMsg = ReadMsg(nps);
            if (bMsg[0] != 6)
            {
                throw new Exception("Wrong response");
            }
            return bMsg[1];
        }

        // just a log function written to be as bad as possible while technically functional in most cases.
        public static void Log(string msg)
        {
            try
            {
                File.AppendAllText(Path.Combine(Path.GetTempPath(), "evgaproxy.log"), DateTime.Now.ToString() + ": " + (msg ?? "") + "\r\n");
            }
            catch
            {
                System.Threading.Thread.Sleep(100);
                try
                {
                    File.AppendAllText(Path.Combine(Path.GetTempPath(), "evgaproxy.log"), DateTime.Now.ToString() + ": " + (msg ?? "") + "\r\n");
                }
                catch { }
            }
        }
        internal void SetLed(int deviceId, int ledId, byte a, byte r, byte g, byte b)
        {
            var bMsg = new byte[16];
            bMsg[0] = 10;
            bMsg[1] = (byte)deviceId;
            bMsg[2] = (byte)ledId;
            bMsg[3] = a;
            bMsg[4] = r;
            bMsg[5] = g;
            bMsg[6] = b;
            nps.Write(MAGIC, 0, 4);
            nps.Write(bMsg, 0, bMsg.Length);
        }

        public EVGADeviceProvider()
        {
            Log("Constructor hit");
            try
            {
                Log("Trying to make temp folder");
                var tempFolder = Path.Combine(Path.GetTempPath(), "evgaproxy");
                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }
                try
                {
                    string proxyPath = Path.Combine(tempFolder, "EVGAProxy.exe");
                    using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("RGB.NET.Devices.EVGA.EVGAProxy.exe"))
                    {
                        using (var fs = File.Open(proxyPath, FileMode.Create))
                        {
                            s.CopyTo(fs);
                        }
                    }
                    ProcessStartInfo psi = new ProcessStartInfo(proxyPath) { UseShellExecute = true, WorkingDirectory = tempFolder, CreateNoWindow = true };
                    _proxyProc = Process.Start(psi);
                }
                catch (Exception ex)
                {
                    //couldn't write it, maybe it's already there and running?  hope for the best and try connecting to a running executable
                    Log("Wasn't able to copy EVGAProxy.exe to temp folder, continuing anyways and hoping for the best");
                }
                nps = new NamedPipeClientStream(".", "evgargbled", PipeDirection.InOut);
                try
                {
                    nps.Connect(5000);
                }
                catch
                {
                    throw new Exception("Coudln't connect to EVGAProxy");
                }
                EVGADeviceInfo devInfo = new EVGADeviceInfo();
                _devices.Clear();

                var numDevs = GetDeviceCount();
                for (int i = 0; i < numDevs; i++)
                {
                    int ledCount = GetLedCount(i);
                    _devices.Add(new EVGADevice(devInfo, SetLed, (uint)i, (uint)ledCount));
                }
            }
            catch (Exception ex)
            {
                Log($"Exception in rgb.net: {ex.Message}");
                throw;
            }
        }

        private static EVGADeviceProvider _instance;
        
        public static EVGADeviceProvider Instance => _instance ?? new EVGADeviceProvider();

        //Since Initialize doesn't seem to be supported by JackNet yet, I guess always return true?
        public bool IsInitialized => true;

        private List<IRGBDevice> _devices = new List<IRGBDevice>();
        public IEnumerable<IRGBDevice> Devices => _devices;

        public bool HasExclusiveAccess => false;

        public void Dispose()
        {
            if (_proxyProc != null)
            {
                try
                {
                    _proxyProc.Kill();
                } catch
                {
                    //who cares what goes wrong with killing it
                }
                finally
                {
                    _proxyProc = null;
                }
            }
        }

        public bool Initialize(RGBDeviceType loadFilter = (RGBDeviceType)(-1), bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            //JackNet doesn't seem to support devices that have IRGBDeviceProviderLoader.RequiresInitialization => true, so all of the init work is stuffed in the constructor
            return true;
        }

        public void ResetDevices()
        {
            //I don't even know what this does.  Because I haven't looked.
        }
    }
}
