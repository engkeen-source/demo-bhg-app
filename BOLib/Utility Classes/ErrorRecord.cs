using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;


namespace BOLib
{
    // There should be only one instance of this type per AppDomain.
    [Serializable]
    public sealed class ErrorRecorder : ISerializable
    {
        // This is the one instance of this type.
        private static readonly ErrorRecorder theOneObject = null;

        // Here are the instance fields.
        private string _StackTrace;
        private object[] _ObjectCarrier;
        private bool _ShowMessage;

        private GEnum.AuditLogMode _LogMode = GEnum.AuditLogMode.Error;
        private GEnum.SystemCode _CodeKey = 0;
        private int _LogDk = 0;
        private string _LogDocID = "";
        private DateTime _LogDocDate = DateTime.Now.Date;
        private string _LogDocTypeNm = "";

        public string StackTrace
        {
            get { return _StackTrace; }
            set { _StackTrace = value; }
        }

        public object[] ObjectCarrier
        {
            get { return _ObjectCarrier; }
            set { _ObjectCarrier = value; }
        }

        public bool ShowMessage
        {
            get { return _ShowMessage; }
            set { _ShowMessage = value; }
        }

        public GEnum.AuditLogMode LogMode
        {
            get { return _LogMode; }
            set { _LogMode = value; }
        }

        public GEnum.SystemCode CodeKey
        {
            get { return _CodeKey; }
            set { _CodeKey = value; }
        }

        public int LogDk
        {
            get { return _LogDk; }
            set { _LogDk = value; }
        }

        public string LogDocID
        {
            get { return _LogDocID; }
            set { _LogDocID = value; }
        }

        public DateTime LogDocDate
        {
            get { return _LogDocDate; }
            set { _LogDocDate = value; }
        }

        public string LogDocTypeNm
        {
            get { return _LogDocTypeNm; }
            set { _LogDocTypeNm = value; }
        }

        // Private constructor allowing this type to construct the Record.
        public ErrorRecorder(bool P_ShowMessage, string P_StackTrace, object[] P_ObjectCarrier)
        {
            _ShowMessage = P_ShowMessage;
            _StackTrace = P_StackTrace;
            _ObjectCarrier = P_ObjectCarrier;
        }

        public ErrorRecorder(bool P_ShowMessage, string P_StackTrace, object[] P_ObjectCarrier,GEnum.AuditLogMode LogMode)
        {
            _ShowMessage = P_ShowMessage;
            _StackTrace = P_StackTrace;
            _ObjectCarrier = P_ObjectCarrier;
            _LogMode = LogMode;
        }

        public ErrorRecorder(bool P_ShowMessage, string P_StackTrace, object[] P_ObjectCarrier, GEnum.AuditLogMode LogMode,GEnum.SystemCode CodeKey)
        {
            _ShowMessage = P_ShowMessage;
            _StackTrace = P_StackTrace;
            _ObjectCarrier = P_ObjectCarrier;
            _LogMode = LogMode;
            _CodeKey = CodeKey;
        }


        // A method returning a reference to the Record.
        public static ErrorRecorder GetRecord()
        {
            return theOneObject;
        }

        // A method called when serializing a Record.
        [SecurityPermissionAttribute(SecurityAction.LinkDemand,
        Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(
            SerializationInfo info, StreamingContext context)
        {
            // Instead of serializing this object, 
            // serialize a RecordSerializationHelp instead.
            info.SetType(typeof(SingletonSerializationHelper));
            // No other values need to be added.
        }

        // Note: ISerializable's special constructor is not necessary 
        // because it is never called.
    }


    [Serializable]
    internal sealed class SingletonSerializationHelper : IObjectReference
    {
        // This object has no fields (although it could).

        // GetRealObject is called after this object is deserialized.
        public Object GetRealObject(StreamingContext context)
        {
            // When deserialiing this object, return a reference to 
            // the Record object instead.
            return ErrorRecorder.GetRecord();
        }
    }
}
