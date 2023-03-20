using System;
using System.Collections.Generic;
using System.Text;
#if !__MOBILE__
using System.Management;
#endif

namespace VerticesEngine.Utilities
{
    /// <summary>
    /// Holds the current system info for the machine running the game
    /// </summary>
    public static class vxSystemInfo
    {
        static readonly string[] SizeSuffixes =
                      { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

#if !__MOBILE__
        private static string TryToGetInfo(ManagementObject obj, string key)
        {
            try
            {
                return obj[key].ToString();
            }
            catch(Exception ex)
            {
                vxConsole.WriteException($"Error Getting System Info {key}", ex);
                return "Unkown";
            }

        }
#endif

        public static SortedDictionary<string, string> Stats = new SortedDictionary<string, string>();

        public static void Init()
        {

#if !__MOBILE__
            try
            {
                ManagementObjectSearcher myProcessorObject = new ManagementObjectSearcher("select * from Win32_Processor");

                //Console.WriteLine("==== Processor ====");
                foreach (ManagementObject obj in myProcessorObject.Get())
                {
                    Stats.Add("Cpu", TryToGetInfo(obj, "Name"));
                    Stats.Add("CpuCoreCount", TryToGetInfo(obj, "NumberOfCores"));
                    Stats.Add("CpuLogicalProcCount", TryToGetInfo(obj, "NumberOfLogicalProcessors"));

                    //Console.WriteLine("Name  -  " + obj["Name"]);
                    //Console.WriteLine("DeviceID  -  " + obj["DeviceID"]);
                    //Console.WriteLine("Manufacturer  -  " + obj["Manufacturer"]);
                    //Console.WriteLine("CurrentClockSpeed  -  " + obj["CurrentClockSpeed"]);
                    //Console.WriteLine("Caption  -  " + obj["Caption"]);
                    //Console.WriteLine("NumberOfCores  -  " + obj["NumberOfCores"]);
                    //Console.WriteLine("NumberOfEnabledCore  -  " + obj["NumberOfEnabledCore"]);
                    //Console.WriteLine("NumberOfLogicalProcessors  -  " + obj["NumberOfLogicalProcessors"]);
                    //Console.WriteLine("Architecture  -  " + obj["Architecture"]);
                    //Console.WriteLine("Family  -  " + obj["Family"]);
                    //Console.WriteLine("ProcessorType  -  " + obj["ProcessorType"]);
                    //Console.WriteLine("Characteristics  -  " + obj["Characteristics"]);
                    //Console.WriteLine("AddressWidth  -  " + obj["AddressWidth"]);
                }

                //Console.WriteLine("==== Memory ====");
                //Console.WriteLine("==== OS ====");
                ManagementObjectSearcher myOperativeSystemObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");

                foreach (ManagementObject obj in myOperativeSystemObject.Get())
                {
                    Stats.Add("OS", TryToGetInfo(obj, "Caption"));
                    Stats.Add("OSVersion", TryToGetInfo(obj, "Version"));
                    try
                    {
                        Stats.Add("MemPhysicalTotal", SizeSuffix((long)Convert.ToDouble(obj["TotalVisibleMemorySize"]) * 1000));
                        Stats.Add("MemPhysicalFree", SizeSuffix((long)Convert.ToDouble(obj["FreePhysicalMemory"]) * 1000));
                        Stats.Add("MemVirtualTotal", SizeSuffix((long)Convert.ToDouble(obj["TotalVirtualMemorySize"]) * 1000));
                        Stats.Add("MemVirtualFree", SizeSuffix((long)Convert.ToDouble(obj["FreeVirtualMemory"]) * 1000));
                    }
                    catch { }

                    //Console.WriteLine("Caption  -  " + obj["Caption"]);
                    //Console.WriteLine("Version  -  " + obj["Version"]);
                    //Console.WriteLine("ProductType  -  " + obj["ProductType"]);
                    //Console.WriteLine("OSType  -  " + obj["OSType"]);
                    //Console.WriteLine("Total Visible Memory: {0} ", SizeSuffix((long)Convert.ToDouble(obj["TotalVisibleMemorySize"]) * 1000));
                    //Console.WriteLine("Free Physical Memory: {0} ", SizeSuffix((long)Convert.ToDouble(obj["FreePhysicalMemory"]) * 1000));
                    //Console.WriteLine("Total Virtual Memory: {0} ", SizeSuffix((long)Convert.ToDouble(obj["TotalVirtualMemorySize"]) * 1000));
                    //Console.WriteLine("Free Virtual Memory: {0} ", SizeSuffix((long)Convert.ToDouble(obj["FreeVirtualMemory"]) * 1000));
                }

                //Console.WriteLine("==== Video ====");

                // GPU Info
                ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");

                foreach (ManagementObject obj in myVideoObject.Get())
                {
                    Stats.Add("GPU", TryToGetInfo(obj, "Name"));
                    Stats.Add("GPUDriverVersion", TryToGetInfo(obj, "DriverVersion"));
                    Stats.Add("GPUMem", SizeSuffix((long)Convert.ToDouble(obj["AdapterRAM"])));

                    //Console.WriteLine("Name  -  " + obj["Name"]);
                    //Console.WriteLine("Status  -  " + obj["Status"]);
                    //Console.WriteLine("Caption  -  " + obj["Caption"]);
                    //Console.WriteLine("DeviceID  -  " + obj["DeviceID"]);
                    //Console.WriteLine("AdapterRAM  -  " + obj["AdapterRAM"]);
                    //Console.WriteLine("AdapterRAM  -  " + SizeSuffix((long)Convert.ToDouble(obj["AdapterRAM"])));
                    //Console.WriteLine("AdapterDACType  -  " + obj["AdapterDACType"]);
                    //Console.WriteLine("Monochrome  -  " + obj["Monochrome"]);
                    //Console.WriteLine("InstalledDisplayDrivers  -  " + obj["InstalledDisplayDrivers"]);
                    //Console.WriteLine("DriverVersion  -  " + obj["DriverVersion"]);
                    //Console.WriteLine("VideoProcessor  -  " + obj["VideoProcessor"]);
                    //Console.WriteLine("VideoArchitecture  -  " + obj["VideoArchitecture"]);
                    //Console.WriteLine("VideoMemoryType  -  " + obj["VideoMemoryType"]);
                }
            }
            catch(Exception ex)
            {
                vxConsole.WriteException(ex);
            }
#endif
        }
    }
}
