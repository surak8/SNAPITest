//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

//using Colt3.ZebraScanner;
using CoreScanner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using CL = NSSNAPITest.LogWrapper;

namespace NSSNAPITest {

    public partial class SNAPITestForm {
#if USE_COLT3
        Colt3.ZebraScanner.ZScanner _scanner;
#else
        ZebraWrapper _scanner;
#endif

        public SNAPITestForm() {
            InitializeComponent();
        }
        void exitClick(object sender, EventArgs ea) {
            CancelEventArgs cea = new CancelEventArgs();

            Application.Exit(cea);
            if (cea.Cancel) {
                return;
            }
            Application.Exit();
        }
        void formLoad(object sender, EventArgs ea) {
            string status;

#if USE_COLT3
            _scanner = new Colt3.ZebraScanner.ZScanner();
#else
            _scanner = new ZebraWrapper();
#endif
            if (_scanner.Initialize(
                OnZebraPlugAndPlayEventDelegate,
                OnZebraBarcodeEventDelegate,
                test1,
                test2,
                OnZebraScannerNotificationEventDelegate, OnZebraDisplayMessageDelegate,
                out status) == false) {
                LogWrapper.logError(CL.makeSig(MethodBase.GetCurrentMethod()) + " " + status);
                return;
            }
            LogWrapper.log(MethodBase.GetCurrentMethod(), "scanner initialized....");
            short[] scannerTypeArray = new short[10];
#if USE_COLT3
            if (_scanner.Open(scannerTypeArray, out status) == false) {
                LogWrapper.logError(CL.makeSig(MethodBase.GetCurrentMethod()) + " " + status);
                return;
            }
#else
            //_scanner.Open ()
            if (_scanner.open(scannerTypeArray, out status) == false) {
                LogWrapper.logError(CL.makeSig(MethodBase.GetCurrentMethod()) + " " + status);
                return;
            }
#endif
            LogWrapper.log(MethodBase.GetCurrentMethod(), "scanner opened OK");
        }
        [STAThread()]
        public static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SNAPITestForm());
        }

        void tsmiTest_Click(object sender, EventArgs e) {
            short nscanners;
            string zzz, status;

            List<ScannerInfo> info1;

#if USE_COLT3
            List<Colt3.ZebraScanner.ScannerInfo> info;
            var ret = _scanner.GetScanners(out nscanners, out info, out zzz, out status);
#else
            var ret = _scanner.GetScanners(out nscanners, out info, out zzz, out status);
#endif
            if (ret)
                //ZebraWrapper.parseScannerXml(zzz);
                ZebraWrapper.parseScannerXml<ScannerData>(zzz);
            LogWrapper.log(MethodBase.GetCurrentMethod());
            //_scanner.
        }

        void test2(byte[] frameBuffer) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
        }

        void test1(long size, short imageFormat, byte[] frameBuffer) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
        }

        void OnZebraDisplayMessageDelegate(string msg) {
            LogWrapper.log(MethodBase.GetCurrentMethod(), "MSG=" + msg);
        }

        void OnZebraScannerNotificationEventDelegate(short notificationType, ref string scannerData) {
            LogWrapper.log("NOTIFYTYPE=" + notificationType);
        }

        void OnZebraBarcodeEventDelegate(string barcode) {
            LogWrapper.log("barcode=" + barcode);
        }

        void OnZebraPlugAndPlayEventDelegate(bool attached) {
            LogWrapper.log("attached=" + attached);
        }

        void SNAPITestForm_FormClosed(object sender, FormClosedEventArgs e) {
            _scanner.Close();
            _scanner = null;
        }

        void tsmiSetSNAPI_Click(object sender, EventArgs e) {
            string status;
            var v = _scanner.SetDeviceSwitchHostMode(Colt3.ZebraScanner.ZScanner.USB_SNAPIwoIMAGING, false, true, out status);

            LogWrapper.log(MethodBase.GetCurrentMethod());
        }

        void tsmiSetUSB_Click(object sender, EventArgs e) {
            //string status;
            //var v = _scanner.SetDeviceSwitchHostMode(ZScanner.USB_HIDKB, false, true, out status);

            LogWrapper.log(MethodBase.GetCurrentMethod());

        }
    }

    public class ZebraWrapper {
        public delegate void OnZebraBarcodeEventDelegate(string barcode);
        public delegate void OnZebraImageEventDelegate(Int64 size, short imageFormat, byte[] frameBuffer);
        public delegate void OnZebraVideoEventDelegate(byte[] frameBuffer);
        public delegate void OnZebraPlugAndPlayEventDelegate(bool attached);
        public delegate void OnZebraScannerNotificationEventDelegate(short notificationType, ref string scannerData);
        public delegate void OnZebraDisplayMessageDelegate(string msg); internal void Close() {
            LogWrapper.log(MethodBase.GetCurrentMethod());
            //throw new NotImplementedException();
        }

        public bool Initialize(OnZebraPlugAndPlayEventDelegate onPlugAndPlayEventDelegate, OnZebraBarcodeEventDelegate onBarcodeEventDelegate, OnZebraImageEventDelegate onImageEventDelegate, OnZebraVideoEventDelegate onVideoEventDelegate, OnZebraScannerNotificationEventDelegate onScannerNotificationEventDelegate, OnZebraDisplayMessageDelegate onDisplayMessageDelegate, out string status) {
            try {
                _OnPlugAndPlayEventDelegate = onPlugAndPlayEventDelegate;
                _CoreScanner.PNPEvent -= new CoreScanner._ICoreScannerEvents_PNPEventEventHandler(OnPNPEvent);
                _CoreScanner.PNPEvent += new CoreScanner._ICoreScannerEvents_PNPEventEventHandler(OnPNPEvent);

                _OnBarcodeEventDelegate = onBarcodeEventDelegate;
                _CoreScanner.BarcodeEvent -= new _ICoreScannerEvents_BarcodeEventEventHandler(OnBarcodeEventSdk);
                _CoreScanner.BarcodeEvent += new _ICoreScannerEvents_BarcodeEventEventHandler(OnBarcodeEventSdk);

                if (onImageEventDelegate != null) {
                    _OnImageEventDelegate = onImageEventDelegate;
                    _CoreScanner.ImageEvent -= new CoreScanner._ICoreScannerEvents_ImageEventEventHandler(OnImageEvent);
                    _CoreScanner.ImageEvent += new CoreScanner._ICoreScannerEvents_ImageEventEventHandler(OnImageEvent);
                }
                if (onVideoEventDelegate != null) {
                    _OnVideoEventDelegate = onVideoEventDelegate;
                    _CoreScanner.VideoEvent -= new CoreScanner._ICoreScannerEvents_VideoEventEventHandler(OnVideoEvent);
                    _CoreScanner.VideoEvent += new CoreScanner._ICoreScannerEvents_VideoEventEventHandler(OnVideoEvent);
                }
                if (onScannerNotificationEventDelegate != null) {
                    _OnScannerNotificationEventDelegate = onScannerNotificationEventDelegate;
                    _CoreScanner.ScannerNotificationEvent -= new _ICoreScannerEvents_ScannerNotificationEventEventHandler(OnScannerNotification);
                    _CoreScanner.ScannerNotificationEvent += new _ICoreScannerEvents_ScannerNotificationEventEventHandler(OnScannerNotification);
                }
                _OnDisplayMessageDelegate = onDisplayMessageDelegate;

                //_Xml = new XmlReader();
                //_Xml=XmlReader.Create()

                status = "OK";
                return true;
            } catch (Exception ex) {
                status = ex.Message;
                return false;
            }
        }

        void OnScannerNotification(short notificationType, ref string pScannerData) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
        }

        void OnVideoEvent(short eventType, int size, ref object sfvideoData, ref string pScannerData) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
        }

        void OnImageEvent(short eventType, int size, short imageFormat, ref object sfImageData, ref string pScannerData) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
        }

        void OnBarcodeEventSdk(short eventType, ref string pscanData) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
        }

        void OnPNPEvent(short eventType, ref string ppnpData) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
        }

        //XmlReader _Xml;
        CCoreScanner _CoreScanner = new CCoreScanner();
        OnZebraBarcodeEventDelegate _OnBarcodeEventDelegate;
        OnZebraImageEventDelegate _OnImageEventDelegate;
        OnZebraVideoEventDelegate _OnVideoEventDelegate;
        OnZebraPlugAndPlayEventDelegate _OnPlugAndPlayEventDelegate;
        OnZebraScannerNotificationEventDelegate _OnScannerNotificationEventDelegate;
        OnZebraDisplayMessageDelegate _OnDisplayMessageDelegate;
        public bool open() {
            string status;
            short[] scannerTypesArray = new short[10];

            return open(scannerTypesArray, out status);
        }
        bool _IsOpen;
        // Available values for 'status
        const int STATUS_SUCCESS = 0;
        const int STATUS_FAIL = 1;
        const int STATUS_LOCKED = 10;

        // Scanner types
        public const short SCANNER_TYPES_ALL = 1;
        public const short SCANNER_TYPES_SNAPI = 2;
        public const short SCANNER_TYPES_SSI = 3;
        public const short SCANNER_TYPES_RSM = 4;
        public const short SCANNER_TYPES_IMAGING = 5;
        public const short SCANNER_TYPES_IBMHID = 6;
        public const short SCANNER_TYPES_NIXMODB = 7;
        public const short SCANNER_TYPES_HIDKB = 8;
        public const short SCANNER_TYPES_IBMTT = 9;
        public const short SCALE_TYPES_IBM = 10;

        const int NUM_SCANNER_EVENTS = 6;

        const int SUBSCRIBE_BARCODE = 1;
        const int SUBSCRIBE_IMAGE = 2;
        const int SUBSCRIBE_VIDEO = 4;
        const int SUBSCRIBE_RMD = 8;
        const int SUBSCRIBE_PNP = 16;
        const int SUBSCRIBE_OTHER = 32;

        const int REGISTER_FOR_EVENTS = 1001;

        public bool open(short[] scannerTypesArray, out string status) {
            int zStatus;
            LogWrapper.logInfo(string.Format("Open()"));
            if (_IsOpen) {
                LogWrapper.logWarning(string.Format("Open() Zebra Scanner is already open"));
                status = "OK";
                return true;
            }
            int appHandle = 0;
            try {
                short sizeOfScannerTypesArray = 1;
                scannerTypesArray[0] = SCANNER_TYPES_ALL;
                zStatus = STATUS_FAIL;
                _CoreScanner.Open(appHandle, scannerTypesArray, sizeOfScannerTypesArray, out zStatus);
                if (STATUS_SUCCESS == zStatus) {
                    _IsOpen = true;
                    if (RegisterForEvents(out status) == false) {
                        LogWrapper.logError(string.Format("RegisterForEvents() FAILED: {0}", status));
                        _IsOpen = false;
                        return false;
                    }
                    status = "OK";
                    return true;
                } else {
                    status = decodeStatusMessage(zStatus);
                    LogWrapper.logError(string.Format("_CoreScanner.Open() FAILED: {0}", status));
                    return false;
                }
            } catch (Exception ex) {
                status = ex.Message;
                LogWrapper.logError(string.Format("Open() FAILED: {0}", status));
                return false;
            }
        }

        string GetRegUnregIDs(out int nEvents) {
            string strIDs = "";
            nEvents = NUM_SCANNER_EVENTS;
            strIDs = SUBSCRIBE_BARCODE.ToString();
            strIDs += "," + SUBSCRIBE_IMAGE.ToString();
            strIDs += "," + SUBSCRIBE_VIDEO.ToString();
            strIDs += "," + SUBSCRIBE_RMD.ToString();
            strIDs += "," + SUBSCRIBE_PNP.ToString();
            strIDs += "," + SUBSCRIBE_OTHER.ToString();
            return strIDs;
        }

        bool RegisterForEvents(out string status) {
            try {
                int nEvents = 0;
                string strEvtIDs = GetRegUnregIDs(out nEvents);
                string inXml = "<inArgs>" +
                                    "<cmdArgs>" +
                                    "<arg-int>" + nEvents + "</arg-int>" +
                                    "<arg-int>" + strEvtIDs + "</arg-int>" +
                                    "</cmdArgs>" +
                                "</inArgs>";

                int opCode = REGISTER_FOR_EVENTS;
                string outXml = "";
                return ExecCmd(opCode, ref inXml, out outXml, out status);
            } catch (Exception ex) {
                status = ex.Message;
                return false;
            }
        }

        bool ExecCmd(int opCode, ref string inXml, out string outXml, out string status) {
            int zStatus;
            outXml = "";
            if (_IsOpen) {
                try {
                    _CoreScanner.ExecCommand(opCode, ref inXml, out outXml, out zStatus);
                    if (STATUS_SUCCESS == zStatus) {
                        status = "OK";
                        return true;
                    } else {
                        status = decodeStatusMessage(zStatus);
                        return false;
                    }
                } catch (Exception ex) {
                    status = ex.Message;
                    return false;
                }
            } else {
                status = "Scanner is closed";
                return false;
            }
        }

        string decodeStatusMessage(int zStatus) {
            try {
                switch (zStatus) {
                    case 0: return "SUCCESS";
                    case 10: return "Device is locked by another application";
                    case 100: return "Invalid application handle. Reserved parameter";
                    case 101: return "Required Comm Lib is unavailable to support the requested Type";
                    case 102: return "Null buffer pointer";
                    case 103: return "Invalid buffer pointer";
                    case 104: return "Incorrect buffer size";
                    case 105: return "Requested Type IDs are duplicated";
                    case 106: return "Incorrect value for number of Types";
                    case 107: return "Invalid argument";
                    case 108: return "Invalid scanner ID";
                    case 109: return "Incorrect value for number of Event IDs";
                    case 110: return "Event IDs are duplicated";
                    case 111: return "Invalid value for Event ID";
                    case 112: return "Required device is unavailable";
                    case 113: return "Opcode is invalid";
                    case 114: return "Invalid value for Type";
                    case 115: return "Opcode does not support asynchronous method";
                    case 116: return "Device does not support the Opcode";
                    case 117: return "Operation failed in device";
                    case 118: return "Request failed in CoreScanner";
                    case 120: return "Device busy. Applications should retry command.";
                    case 200: return "CoreScanner is already opened";
                    case 201: return "CoreScanner is already closed";
                    case 202: return "CoreScanner is closed";
                    case 300: return "Invalid XML";
                    case 301: return "XML Reader could not be instantiated";
                    case 308: return "Arguments in inXML are not valid";
                    default: return string.Format("zStatus {0} TODO KPD Need to code", zStatus);
                }
            } catch (Exception ex) {
                return string.Format("zStatus {0} {1}", zStatus, ex.Message);
            }
        }


        public bool SetDeviceSwitchHostMode(string mode, bool silent, bool permanent, out string status, int scannerId = 1) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
            status = null;
            return false;
        }
        public const int MAX_NUM_DEVICES = 255;/* Maximum number of scanners to be connected*/

        XmlReader _Xml;

        public bool GetScanners(out short numberOfScanners, out List<ScannerInfo> scannerInfoList,
            out string outXml,
            out string status) {

            int scannerCount = 0, zStatus;
            int[] connectedScannerIdArray;
            ScannerInfo[] scannerInfoArray;
            LogWrapper.log(MethodBase.GetCurrentMethod());

            numberOfScanners = 0;
            outXml = string.Empty;
            scannerInfoList = new List<ScannerInfo>();
            connectedScannerIdArray = new int[MAX_NUM_DEVICES];
            try {
                if (_IsOpen == false) {
                    status = "Scanner is closed";
                    return false;
                }
                _CoreScanner.GetScanners(out numberOfScanners, connectedScannerIdArray, out outXml, out zStatus);
                if (STATUS_SUCCESS == zStatus) {
                    if (numberOfScanners < 1) {
                        status = "No Scanners Were Found";
                        return false;
                    }
                    scannerInfoArray = new ScannerInfo[MAX_NUM_DEVICES];
                    for (int i = 0; i < MAX_NUM_DEVICES; i++)
                        scannerInfoArray[i] = new ScannerInfo();

                    scannerInfoList = parseScannerXml<ScannerData>(outXml);
                    //_Xml.ReadXmlstring_GetScanners(outXml, scannerInfoArray, numberOfScanners, out scannerCount, out status);
                    for (int i = 0; i < scannerCount; i++)
                        scannerInfoList.Add(scannerInfoArray[i]);
                    status = "OK";
                    return true;
                } else {
                    status = decodeStatusMessage(zStatus);
                    return false;
                }
            } catch (Exception ex) {
                status = ex.Message;
                return false;
            }
        }

        public static List<ScannerInfo> parseScannerXml<T>(string outXml) where T : IFixable {
            List<ScannerInfo> ret = new List<ScannerInfo>();
            StringBuilder sb = new StringBuilder();
            XmlReaderSettings xrs = new XmlReaderSettings();
            XmlSerializer xs;
            //XmlDeserializationEvents xde;
            T myObj;
            object anObj;

            using (StringReader sr = new StringReader(outXml)) {
                xrs.ValidationEventHandler += Xrs_ValidationEventHandler;
                xrs.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                using (XmlReader xr = XmlReader.Create(sr, xrs)) {
                    try {
                        xs = new XmlSerializer(typeof(ScannerData));
                        //xs.
                        //anObj = xs.Deserialize(xr, new XmlDeserializationEvents)
                        //xde = new XmlDeserializationEvents();
                        //xs.OnUnknownAttribute = unknownAttribute;
                        //xde.OnUnknownElement = unknownElement;
                        //xde.OnUnknownNode = unknownNode;
                        //xde.OnUnreferencedObject = unrefObj;
                        //anObj = xs.Deserialize(xr, xde);
                        xs.UnknownAttribute += foundUnknownAttribute;
                        xs.UnknownElement += foundUnknownElement;
                        xs.UnknownNode += foundUnknownNode;
                        xs.UnreferencedObject += foundUnrefObj;

                        anObj = xs.Deserialize(xr);
                        if (anObj != null) {
                            myObj = (T)anObj;
                            myObj.fixup();
                            //anObj.
                        }
                        System.Diagnostics.Trace.WriteLine("here");
                    } catch (Exception ex) {
                        LogWrapper.log(MethodBase.GetCurrentMethod(), ex);
                    }

                }
            }
            return ret;
        }

        private static void foundUnrefObj(object sender, UnreferencedObjectEventArgs e) {
            throw new NotImplementedException();
        }

        private static void foundUnknownNode(object sender, XmlNodeEventArgs e) {
            throw new NotImplementedException();
        }

        private static void foundUnknownElement(object sender, XmlElementEventArgs e) {
            throw new NotImplementedException();
        }

        static void foundUnknownAttribute(object sender, XmlAttributeEventArgs e) {
            throw new NotImplementedException();
        }

        static void unrefObj(object sender, UnreferencedObjectEventArgs e) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
        }

        static void unknownNode(object sender, XmlNodeEventArgs e) {
            LogWrapper.log(MethodBase.GetCurrentMethod(), e.ObjectBeingDeserialized.GetType().Name + ": have " + e.NodeType + " called " + e.Name);
        }

        static void unknownElement(object sender, XmlElementEventArgs e) {
            LogWrapper.log(MethodBase.GetCurrentMethod(), " found: " + e.Element.Name);
        }

        static void unknownAttribute(object sender, XmlAttributeEventArgs e) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
        }

        static void Xrs_ValidationEventHandler(object sender, ValidationEventArgs e) {
            LogWrapper.log(MethodBase.GetCurrentMethod());
        }
    }
}