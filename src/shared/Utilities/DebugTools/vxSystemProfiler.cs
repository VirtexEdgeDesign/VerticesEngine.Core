using System;
using System.Diagnostics;

namespace VerticesEngine.Diagnostics
{
	public static class vxSystemProfiler
	{

		public static Process CurrentProcess;

		public static OperatingSystem OSVersion;

		public static int ProcessorCount;

        public static bool Initalised;

#if !__MOBILE__
		public static PerformanceCounter CpuCounter;
#endif
		public static void Init()
		{
            Initalised = false;
            try
            {
                CurrentProcess = Process.GetCurrentProcess();

                OSVersion = Environment.OSVersion;
                ProcessorCount = Environment.ProcessorCount;

#if !__MOBILE__
                CpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
#endif

                // Handle Ram
                TotalPhysicalRam = (float)GetTotalPhysicalRam();
                TotalPhysicalRamInMb = (float)TotalPhysicalRam / 1024 / 1024;
                TotalPhysicalRamInGb = (float)TotalPhysicalRam / 1024 / 1024 / 1024;
                Initalised = true;
            }
            catch(Exception ex)
            {
                vxDebug.DumpError(ex, "vxSystem Profiler Init");
            }
		}


		public static string GetCurrentCpuUsage()
		{
            if (Initalised)
            {
#if !__MOBILE__
                return CpuCounter.NextValue() + "%";
#endif
            }

			return "N/A";
		}


		#region Handling RAM

		/// <summary>
		/// The total physical ram of the system (in bytes).
		/// </summary>
		public static float TotalPhysicalRam;

		public static float TotalPhysicalRamInMb;

		public static float TotalPhysicalRamInGb;


		static float GetTotalPhysicalRam()
		{
            if (Initalised)
            {
#if !__MOBILE__
                if (PerformanceCounterCategory.Exists("Mono Memory"))
                {

                    var pc = new PerformanceCounter("Mono Memory", "Total Physical Memory");

                    return (float)pc.RawValue;
                }
                else
                {
                    return 0;
                    //throw new Exception("Total Physical Ram Not Found");
                }
#endif
            }
			return 0;
		}


		/// <summary>
		/// Gets the current ram usage (in bytes).
		/// </summary>
		/// <returns>The current ram usage.</returns>
		public static float GetPeakVirtualMemorySize64()
        {
            if (Initalised)
            {
                return CurrentProcess.PeakVirtualMemorySize64;
            }
            else
                return 1;
		}

		public static float GetMemoryWorkingSet64()
        {
            if (Initalised)
            {
                return CurrentProcess.WorkingSet64;
            }
            else
                return 1;
		}

		#endregion
	}
}
