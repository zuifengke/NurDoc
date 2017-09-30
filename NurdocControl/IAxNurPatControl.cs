using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NurdocControl
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    //[Guid("A6C6DB5B-15B8-3D1E-9E8E-43F17A577B8F")]
    public interface IAxNurPatControl
    {

        void SwitchPatient(string szPatientID, string szVisitID,string szUserID);
         
        void test();

        void OpenNurDoc(string szPatientID, string szVisitID, string szUserID, string szDocTypeID, string szDocID);
    }
}
