/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if SLS_SDK_40
[assembly: AssemblyTitle("SLS SDK .NET 4.0")]
#elif SLS_SDK_35
[assembly: AssemblyTitle("SLS SDK .NET 3.5")]
#else
[assembly: AssemblyTitle("SLS SDK .NET")]
#endif

[assembly: AssemblyDescription(".NET SDK for Aliyun Simple Log Service (SLS)")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Aliyun")]
[assembly: AssemblyProduct("SLSSDK")]
[assembly: AssemblyCopyright("Copyright © Aliyun 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("4328d16b-4359-4acd-a34e-d5c0224572c0")]
[assembly: InternalsVisibleTo("SLSSDKTest")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.6.0")]
[assembly: AssemblyFileVersion("0.6.0")]
