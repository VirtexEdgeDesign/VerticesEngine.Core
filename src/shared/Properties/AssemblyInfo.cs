using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if __ANDROID__ 
using Android.App;

[assembly: AssemblyTitle("VerticesEngine")]
[assembly: AssemblyDescription("The Android Port of Vertices Engine, built upon MonoGame")]

#else

[assembly: AssemblyTitle("VerticesEngine")]
[assembly: AssemblyDescription("The Desktop Port of Vertices Engine, built upon MonoGame")]


// On Windows, the following GUID is for the ID of the typelib if this
// project is exposed to COM. On other platforms, it unique identifies the
// title storage container when deploying this assembly to the device.
[assembly: Guid("4fbf1f9c-2d6a-4c82-9311-1265c6731a8c")]
#endif
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyProduct("VerticesEngine")]
[assembly: AssemblyCompany("Virtex Edge Design")]
[assembly: AssemblyCopyright("Copyright ©  2023")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "MonoGame incorrectly warns of min target version being set below 31.", Scope = "namespaceanddescendants", Target = "~N:XXX")]
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type. Only Windows
// assemblies support COM.
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("2.0.0.999")]