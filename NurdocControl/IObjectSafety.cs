using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NurdocControl
{
    [Guid("A6C6DB5B-15B8-3D1E-9E8E-43F17A577B81"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    interface IObjectSafety
    {
        [PreserveSig]
        void GetInterfacceSafyOptions(System.Int32 riid, out System.Int32 pdwSupportedOptions, out System.Int32 pdwEnabledOptions);
        [PreserveSig]
        void SetInterfaceSafetyOptions(System.Int32 riid, System.Int32 dwOptionsSetMask, System.Int32 dwEnabledOptions);

        //void SwitchPatient(string szPatientID, string szVisitID);

        //void test();
    }

    //[ComImport, Guid("A6C6DB5B-15B8-3D1E-9E8E-43F17A577B8F")]
    //[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    //public interface IObjectSafety
    //{
    //    [PreserveSig]
    //    void GetInterfaceSafetyOptions(int riid
    //        , [MarshalAs(UnmanagedType.U4)]out int pdwSupportedOptions
    //        , [MarshalAs(UnmanagedType.U4)]out int pdwEnabledOptions);

    //    [PreserveSig]
    //    void SetInterfaceSafetyOptions(int riid
    //        , [MarshalAs(UnmanagedType.U4)]int dwOptionSetMask
    //        , [MarshalAs(UnmanagedType.U4)]int dwEnabledOptions);

    //    void SwitchPatient(string szPatientID, string szVisitID);

    //    void test();
    //}
}
