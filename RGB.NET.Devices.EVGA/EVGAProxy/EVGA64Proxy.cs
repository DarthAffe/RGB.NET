using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EVGAProxy
{
    public class EVGA64Proxy
    {
        //copy paste awful log function for the win
        public static void Log(string msg)
        {
            try
            {
                File.AppendAllText(Path.Combine(Path.GetTempPath(), "evgaproxy.log"), DateTime.Now.ToString() + ": " + (msg ?? "") + "\r\n");
            }
            catch {
                System.Threading.Thread.Sleep(100);
                try
                {
                    File.AppendAllText(Path.Combine(Path.GetTempPath(), "evgaproxy.log"), DateTime.Now.ToString() + ": " + (msg ?? "") + "\r\n");
                }
                catch { }
            }
        }
        private string _evgaPath = null;
        private string _precisionExe = null;
        private string _precisionExeName = null;
        private Type _ledInterface;
        private PropertyInfo _zoneSupported;
        private PropertyInfo _zoneSet;
        /* possibly useful in the future, but not used now
        private MethodInfo _setLEDOff;
        private MethodInfo _getDefaultColor;
        */
        private MethodInfo _setLEDStaticOn;        
        
        private List<object> _instances = new List<object>();
        public EVGA64Proxy()
        {
            try
            {

                string assyPath = null;
                string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //attempt to determine where PrecisionX is installed via the registry.  I don't think this will work with the steam version
                //TODO: steam version support
                try
                {
                    RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    if (baseKey == null)
                    {
                        throw new Exception("Unable to find 64 bit HKLM registry root");
                    }
                    RegistryKey key = baseKey.OpenSubKey(@"SOFTWARE\EVGA Precision X1");
                    if (key == null)
                    {
                        throw new Exception("EVGA Precision X1 doesn't seem to be installed (no key)");
                    }
                    var dirval = key.GetValue(@"Install_Dir")?.ToString();
                    if (string.IsNullOrWhiteSpace(dirval))
                    {
                         throw new Exception("EVGA Precision X1 doesn't seem to be installed (no installdir)");
                    }
                    _evgaPath = dirval;
                    var exeName = "PrecisionX_x64";
                    assyPath = Path.Combine(_evgaPath, exeName + ".exe");
                    if (!File.Exists(assyPath))
                    {
                        throw new Exception("PrecisionX_x64.exe don't seem to exist at the path defined in the registry");
                    }
                    _precisionExeName = exeName;
                    _precisionExe = assyPath;
                }
                catch (Exception ex)
                {
                    //caught for the sake of logging and helping whoever is unfortunate enough to try and support this
                    Log("Exception trying to determine the location of PrecisionX_x64.exe from the registry: " + ex.Message);
                    throw;
                }

                //copy any DLLs found in the PrecisionX path to the temp folder so that they can be resolved appropriately when the primary
                //  assembly is loaded
                var dlls = Directory.GetFiles(_evgaPath, "*.dll").ToList();
                foreach (var dll in dlls)
                {
                    try
                    {
                        File.Copy(dll, Path.Combine(rootPath, Path.GetFileName(dll)), true);
                    }
                    catch (Exception ex)
                    {
                        //catch the error, but continue anyways because we're really just winging this whole thing anyways and the overridden assembly resolution may take care of the problem
                        Log($"Failed copying {dll}: {ex.Message}");
                    }
                }
                
                var curDir = Directory.GetCurrentDirectory();
                //set the current directory to the PrecisionX install path.  This is somewhat of a "hopeful mitigation" to help assembly resolution for other versions of precisionX that may come out in the future that don't follow .Net's assembly resolving
                Directory.SetCurrentDirectory(_evgaPath);

                //add a helpful assembly resolver to the app domain so we can help EVGA's components locate assemblies
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                try
                {
                    Assembly evgaAssembly = null;

                    //Load the main PrecixionX_x6t4.exe file which is where most of the code is.  try/catch is just for the sake of logging
                    try
                    {
                        evgaAssembly = Assembly.Load(_precisionExeName);
                    }
                    catch (Exception ex)
                    {
                        Log("Failed to load the EVGA Precision X1 executable assembly: " + ex.Message);
                        throw;
                    }

                    //attempt to find the ILedCtrl interface type that seems to be implemented by the EVGA LED control classes
                    var ledInterface = evgaAssembly.GetType("PX18.Model.ILedCtrl");
                    if (ledInterface == null)
                    {
                        throw new Exception("Failed to find ILedCtrl interface");
                    }
                    _ledInterface = ledInterface;
                    /*
                     * here are a couple of other useful things on the interface that may be useful for anybody that expands this in the future
                    _setLEDOff = _ledInterface.GetMethod("SetLEDOff");
                    _getDefaultColor = _ledInterface.GetMethod("GetDefaultColor");
                    */

                    _zoneSet = _ledInterface.GetProperty("ZoneSet");                    
                    _setLEDStaticOn = _ledInterface.GetMethod("SetLEDStaticOn");

                    _zoneSupported = ledInterface.GetProperty("ZoneSupported");

                    List<Type> ledControls;
                    //try/catch just for helpfulness to troubleshooters
                    try
                    {
                        ledControls = evgaAssembly.GetTypes().Where(x => x.IsClass && ledInterface.IsAssignableFrom(x)).ToList();
                    }
                    catch (ReflectionTypeLoadException fle)
                    {
                        foreach (var g in fle.LoaderExceptions)
                        {
                            Log("Loader exception trying to load types from the PrecisionX executable: "+g.Message);
                        }
                        throw;
                    }

                    foreach (var control in ledControls)
                    {
                        //for whatever reason, IsInitialized isn't part of the interface, but it appears to be what's used
                        //  to determine if there's a relevant product installed on the system for the library.  Use reflection
                        //  to get it, if the class doesn't have the property, skip the class.
                        var initializedProp = control.GetProperty("IsInitialized");
                        if (initializedProp == null)
                        {
                            continue;
                        }
                        object instance = null;
                        try
                        {
                            //there are two types of constructors in the version I've seen.

                            //the first one is ctor(uint gpuIndex, bool isSLIBridge)
                            var con = control.GetConstructor(new Type[] { typeof(uint), typeof(bool) });
                            if (con == null)
                            {
                                //some (older?) classes don't appear to have the SLIbridge parameter, so it's just the GPU index
                                con = control.GetConstructor(new Type[] { typeof(uint) });
                                if (con == null)
                                {
                                    //if neither of these constructors is found, not sure what to do with the class, so skip it
                                    continue;
                                }
                                else
                                {
                                    instance = Activator.CreateInstance(control, new object[] { (uint)0 });
                                }
                            }
                            else
                            {
                                instance = Activator.CreateInstance(control, new object[] { (uint)0, false });
                            }
                        }
                        catch (Exception ex)
                        {
                            //since this is all as rag-tag a show as code should ever be, any exceptions trying to reflectively instantiate the class
                            //should just skip this class and move on
                            continue;
                        }
                        if (instance != null)
                        {
                            //if we have an instance, call the IsInitialized property we found.
                            //  based on what seems to be true, if it isn't initialized, the device isn't plugged in or supported or whatever
                            bool isInitted = (bool)initializedProp.GetValue(instance, null);

                            //if it *is* initialized, also get the ZonesSupported property which is a bitwise set of flags which
                            //  appear to indicate how many LED zones (i.e. individual RGB LEDS) are supported by the device.
                            //  if it's 0, nothing is supported so we won't add it to the list of instances we're worried about
                            if (isInitted && (uint)_zoneSupported.GetValue(instance, null) > 0)
                            {
                                _instances.Add(instance);
                            }
                            //TODO: should probably try and reflectively dispose instances we don't want...
                        }
                    }
                }
                finally
                {
                    //revert the current directory and remove the assembly resolve event
                    Directory.SetCurrentDirectory(curDir);
                    AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                throw;
            }
        }

        public int GetNumberOfDevices()
        {
            return _instances.Count();
        }

        public int GetLedCount(int deviceId)
        {
            if (deviceId < 0 || deviceId >= _instances.Count())
            {
                return -1;
            }
            //loop through each of the bits in the supported zones and count how many are "1"
            var zones = (uint)_zoneSupported.GetValue(_instances[deviceId]);
            int count = 0;
            for (int i= 0; i< 32; i++)
            {
                if (((zones>>i) & 1) == 1)
                {
                    count++;
                }
            }
            return count;
        }

        public void SetColor(int deviceId, int ledId, byte a, byte r, byte g, byte b)
        {
            try
            {
                if (deviceId < 0 || deviceId >= _instances.Count())
                {
                    return;
                }
                if (ledId < 0 || ledId > 31)
                {
                    return;
                }
                var instance = _instances[deviceId];
                _zoneSet.SetValue(instance, (uint)1 << ledId, null);
                _setLEDStaticOn.Invoke(instance, new object[] { new System.Windows.Media.Color() { A = a, R = r, B = b, G = g } });
            }
            catch { }
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //try and load any assemblies out of the PrecisionX root path.
            //  this really probably isn't needed since we're gonna copy them all.
            //  check for .dll or .exe just to be as future proof as reasonably possible
            var name = args.Name.Split(',')[0];
            var assName = Path.Combine(_evgaPath, name + ".dll");
            if (!File.Exists(assName))
            {
                assName = Path.Combine(_evgaPath, name + ".exe");
            }
            if (File.Exists(assName))
            {
                try
                {
                    var a = Assembly.Load(File.ReadAllBytes(assName));
                    return a;
                }
                catch (Exception ex)
                {
                    //if the assembly fails to load, it's a dog and pony show that we even know what we're doing here, so just
                    //  pretend it was all ok and continue on like we didn't even want it to load
                }
            }
            //if the file didn't exists as a .dll or an .exe in the PrecisionX root folder, continue crudely hacking our way through code
            //  by attempting to resolve it from the embedded resources of the PrecisionX_x64.exe file
            return LoadFromEvgaResource(name);
        }

        private Assembly evgaAssembly;
        private List<string> evgaResources = null;
        private Assembly LoadFromEvgaResource(string name)
        {
            //EVGA has a bunch of assemblies and other resources embedded in the primary executable.  
            //  to try and save some churn, rather than extracting them, we'll try and load them directly from the manifest resource stream
            //  out of the executable
            try
            {
                //since this may be called multiple times, don't load it until it is needed, but don't load it more than once
                if (evgaAssembly == null)
                {
                    //ReflectionOnly load the PrecisionX_x64.exe file which I think gets the resource streams too
                    evgaAssembly = Assembly.ReflectionOnlyLoadFrom(_precisionExe);
                }
                //same as above, get the list of resource names when we need it, but only once
                if (evgaResources == null)
                {
                    evgaResources = new List<string>();
                    evgaResources.AddRange(evgaAssembly.GetManifestResourceNames());
                }
                //see if there's any that end with the name +.dll of the assembly trying to load
                var found = evgaResources.FirstOrDefault(x => x.ToLower().EndsWith(name.ToLower() + ".dll"));
                if (found == null)
                {
                    return null;
                }
                //if it exists, load the resource into a byte array and try loading the assembly from that.
                //NOTE: this doesn't work with the ManagedNvApi.dll because it's got native code in it.
                //      that single DLL is why a lot of this hell is even neccesary
                using (var mrs = evgaAssembly.GetManifestResourceStream(found))
                {
                    using (var ms = new MemoryStream())
                    {
                        mrs.CopyTo(ms);
                        return Assembly.Load(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                //really if anything goes wrong, it was all duct tape and string to begin with and we're just doing our best, so don't crash at least...
                return null;
            }
        }

    }
}
