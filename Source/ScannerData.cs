
using System;
using System.Reflection;
using System.Xml.Serialization;
using NSSNAPITest;

public interface IFixable {
    void fixup();
}
[XmlRoot("scanners")]
public class ScannerData : IFixable{
    [XmlElement("scanner")]
    public ScannerInfo scanners;

    void IFixable.fixup() {
        LogWrapper.log(MethodBase.GetCurrentMethod());
    }
}

public class ScannerInfo {
    [XmlAttribute("type")]
    public string scannerType;

    [XmlElement("scannerID")]
    public string scannerID;

    [XmlElement("serialnumber")]
    public string serialNumber;

    [XmlElement("GUID")]
    public Guid scannerGUID;

    [XmlElement("VID")]
    public string vid;

    [XmlElement("PID")]
    public string pid;

    [XmlElement("modelnumber")]
    public string modelNumber;

    [XmlElement("DoM")]
    public string dateOfManufacture;

    [XmlElement("firmware")]
    public string firmwareVersion;
}