// ***********************************************************
// ���������ù���ϵͳ,ģ�塢����������������Ľӿ�.
// Author : YangMingkun, Date : 2012-6-6
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Heren.NurDoc.Config.DockForms
{
    internal interface IDesignEditForm
    {
        bool IsDisposed { get; }

        bool IsModified { get; }

        PropertyGrid PropertyGrid { get; set; }
    }
}
