// ***********************************************************
// ������Ӳ���ϵͳ,ȫ�ֳ������ݶ�����.
// Creator:YangMingkun  Date:2012-3-20
// Copyright : Heren Health Services Co.,Ltd.
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using Heren.NurDoc.DAL.DbAccess;

namespace Heren.NurDoc.Data
{
    public struct SystemConst
    {
        /// <summary>
        /// ����ֵ����
        /// </summary>
        public struct ReturnValue
        {
            /// <summary>
            /// ����ֵ=0
            /// </summary>
            public const short OK = 0;

            /// <summary>
            /// ����ֵ=1
            /// </summary>
            public const short FAILED = 1;

            /// <summary>
            /// ����ֵ=2
            /// </summary>
            public const short CANCEL = 2;

            /// <summary>
            /// ����ֵ=3
            /// </summary>
            public const short EXCEPTION = 3;

            /// <summary>
            /// ��Դδ����=4
            /// </summary>
            public const short NO_FOUND = 4;
        }

        /// <summary>
        /// ����ʵ������ӳ������
        /// </summary>
        public struct MappingName
        {
            /// <summary>
            /// ������Ӳ���ϵͳ
            /// </summary>
            public const string NURDOC_SYS = "NurDoc";

            /// <summary>
            /// ������Ӳ������ù���ϵͳ
            /// </summary>
            public const string CONFIG_SYS = "NdsConfig";

            /// <summary>
            /// ������Ӳ����Զ�����ϵͳ
            /// </summary>
            public const string UPGRADE_SYS = "NdsUpgrade";

            /// <summary>
            /// ������Ӳ���������Ϣ��
            /// </summary>
            public const string INFOLIB_SYS = "InfoLib";

            /// <summary>
            /// ������Ӳ���������Ϣ��
            /// </summary>
            public const string HEALTH_TECH_SYS = "HealthTech";
        }

        /// <summary>
        /// �������������йس���
        /// </summary>
        public struct StartupArgs
        {
            /// <summary>
            /// ���������е����ָ���
            /// </summary>
            public const string GROUP_SPLIT = ";";

            /// <summary>
            /// ���������е������ֶηָ���
            /// </summary>
            public const string FIELD_SPLIT = ",";

            /// <summary>
            /// ���������ַ����ڵı�ת���ַ�
            /// </summary>
            public const string ESCAPED_CHAR = " ";

            /// <summary>
            /// ���������ַ����ڵ�ת���
            /// </summary>
            public const string ESCAPE_CHAR = "$";
        }

        /// <summary>
        /// ĳЩģ��̶�ʹ�õı�
        /// </summary>
        public struct FormName
        {
            /// <summary>
            /// �����¼ģ��֮������̬������
            /// </summary>
            public const string SHIFT_WARD_STATUS = "������̬";

            /// <summary>
            /// �����¼ģ��֮������Ϣ������
            /// </summary>
            public const string SHIFT_PATIENT = "������Ϣ";

            /// <summary>
            /// �����¼ģ��֮���ಡ��ɸѡ������
            /// </summary>
            public const string SHIFT_PATIENT_FILTER = "����ɸѡ";

            /// <summary>
            /// �����¼ģ��֮������Ϣ������
            /// </summary>
            public const string SHIFT_SPECIALPATIENT = "���ⲡ�˽�����Ϣ";

            /// <summary>
            /// ������ͼģ��֮������ͼ�򿨱�����
            /// </summary>
            public const string BEDCARD_SIMPLE_CARD = "������ͼ��";

            /// <summary>
            /// ������ͼģ��֮������Ϣ������
            /// </summary>
            public const string PATIENT_DETAIL_INFO = "������Ϣ";

            /// <summary>
            /// ������ͼģ��֮������ͼϸ��������
            /// </summary>
            public const string BEDCARD_DETAIL_CARD = "������ͼϸ��";

            /// <summary>
            /// ������ͼģ��֮������ͼ�б������
            /// </summary>
            public const string BEDCARD_PATIENT_LIST = "������ͼ�б�";

            /// <summary>
            /// �����밲ȫ�����¼ģ��֮�����밲ȫ�����¼��Ϣ������
            /// </summary>
            public const string Quality_Safty_INFO = "�����밲ȫ�����¼";

            /// <summary>
            /// ������ͼģ��֮��ʿ�Ű���ͼϸ��������
            /// </summary>
            public const string ROSTERING_CARD = "��ʿ�Ű���ͼϸ��";
        }

        /// <summary>
        /// �����ļ����������Ƴ���
        /// </summary>
        public struct ConfigKey
        {
            /// <summary>
            /// Ĭ�ϵ�¼���û�ID��
            /// </summary>
            public const string DEFAULT_LOGIN_USERID = "Heren.NurDoc.DefaultLoginUser";

            /// <summary>
            /// �û��鿴Readme��ʱ��
            /// </summary>
            public const string USER_README_TIME = "Heren.NurDoc.Readme";

            /// <summary>
            /// ���������Ƿ񵯳��༭
            /// </summary>
            public const string NUR_DOC_SHOW_AS_MODEL = "Heren.NurDoc.ShowAsModel";

            /// <summary>
            /// �����밲ȫ�����¼�Ƿ񵯳��༭
            /// </summary>
            public const string QC_SHOW_AS_MODEL = "Heren.NurDoc.ShowAsModel";

            /// <summary>
            /// �໤��¼���Ƿ񱣴����ʾԤ��
            /// </summary>
            public const string NUR_DOC_SAVE_PREVIEW = "Heren.NurDoc.SavePreview";

            /// <summary>
            /// ���������Ƿ񱣴��ֱ�ӷ����б�
            /// </summary>
            public const string NUR_DOC_SAVE_RETURN = "Heren.NurDoc.SaveReturn";

            /// <summary>
            /// �����밲ȫ�����¼�Ƿ񱣴��ֱ�ӷ����б�
            /// </summary>
            public const string QC_SAVE_RETURN = "Heren.NurDoc.SaveReturn";


            /// <summary>
            /// ����ƻ��Ƿ񱣴��ֱ�ӷ����б�
            /// </summary>
            public const string NCP_SAVE_RETURN = "Heren.NCP.SaveReturn";
            
            /// <summary>
            /// �����¼�Ƿ񵯳��༭
            /// </summary>
            public const string NUR_REC_SHOW_AS_MODEL = "Heren.NurDoc.NurRec.ShowAsModel";

            /// <summary>
            /// �����¼����󷵻��б�
            /// </summary>
            public const string NUR_REC_SAVE_RETURN = "Heren.NurDoc.NurRec.SaveReturn";

            /// <summary>
            /// ��λ����ͼ��ʾģʽ����
            /// </summary>
            public const string PATIENT_VIEW = "Heren.NurDoc.BedView.PatientView";

            /// <summary>
            /// ��λ����ͼ��ʾλ������
            /// </summary>
            public const string WHITEBOARD_LOCATION = "Heren.NurDoc.Whiteboard.Location";

            /// <summary>
            /// ������Ӳ���ϵͳ��ǰ�汾��Ϣ
            /// </summary>
            public const string CURRENT_VERSION = "Heren.NurDoc.Current.Verion";

            /// <summary>
            /// ϵͳ��������
            /// </summary>
            public const string SYSTEM_NAME = "Heren.NurDoc.SystemName";

            /// <summary>
            /// ��������ڵ��б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string TASK_NODE_EXPAND = "Heren.NurDoc.TaskNode.ExpandAll";

            /// <summary>
            /// ��ѯͳ���б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string STATISTIC_EXPAND = "Heren.NurDoc.Statistic.ExpandAll";

            /// <summary>
            /// �����밲ȫ�����¼�б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string QC_EXPAND = "Heren.NurDoc.QC.ExpandAll";

            /// <summary>
            /// �����¼�������б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string NUR_REC_EXPAND = "Heren.NurDoc.NurRec.ExpandAll";

            /// <summary>
            /// �ĵ��б�1���б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string NURDOC_LIST1_EXPAND = "Heren.NurDoc.DocumentList1.ExpandAll";

            /// <summary>
            /// �ĵ��б�2���б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string NURDOC_LIST2_EXPAND = "Heren.NurDoc.DocumentList2.ExpandAll";

            /// <summary>
            /// �ĵ��б�3���б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string NURDOC_LIST3_EXPAND = "Heren.NurDoc.DocumentList3.ExpandAll";

            /// <summary>
            /// �ĵ��б�4���б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string NURDOC_LIST4_EXPAND = "Heren.NurDoc.DocumentList4.ExpandAll";

            /// <summary>
            /// ר�ƻ����������б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string SPECIAL_NURSING_EXPAND = "Heren.NurDoc.SpecialNursing.ExpandAll";

            /// <summary>
            /// �໤��¼�������б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string GRAPH_NURSING_EXPAND = "Heren.NurDoc.GraphNursing.ExpandAll";

            /// <summary>
            /// �����������б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string NURSING_ASSESSMENT_EXPAND = "Heren.NurDoc.NursingAssessment.ExpandAll";

            /// <summary>
            /// �������۽ڵ��б�Ĭ���Ƿ�չ������
            /// </summary>
            public const string EVALUATION_EXPAND = "Heren.NurDoc.Evaluation.ExpandAll";
        }
    }
}
