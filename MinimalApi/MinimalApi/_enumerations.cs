using Stratos.Core;

namespace MinimalApi;

//public enum AuthenticationTypes
//{
//    Basic,
//    None,
//    Token
//}

//public enum BackgroundReportSwitches
//{
//    [StringValue(@"/DatabaseName")]
//    DatabaseName,
//    [StringValue(@"/Help")]
//    Help,
//    [StringValue(@"/IsSchedules")]
//    IsSchedules,
//    [StringValue(@"/Guids")]
//    Guids,
//}

//public enum CallType
//{
//    Get,
//    Let,
//    Method,
//    Set
//}

public enum ConnectionType
{
    [StringValue("ODB")]
    Odbc,
    [StringValue("ORA")]
    Oracle,
    [StringValue("SQL")]
    Sql,
    [StringValue("RDB")]
    Rdb,
    [StringValue("JET")]
    Jet
}

//public enum DateRangeTypes
//{
//    CurrentWeekSunSat = 1,
//    CurrentWeekMonSun = 2,
//    PriorWeekSunSat = 3,
//    PriorWeekMonSun = 4,
//    CurrentDay = 5,
//    Custom = 6,
//    PriorDay = 7,
//    Prior7Days = 8,
//    CurrentCalendarYear = 9,
//    PriorCalendarYear = 10,
//    TwoDaysPrior = 11,
//    TwelveMonthRolling = 12,
//    Prior28Days = 13
//}

//public enum HttpVerb
//{
//    Delete,
//    Get,
//    Post,
//    Put
//}

//public enum Languages
//{
//    English = 1,
//    French = 2,
//    Spanish = 3
//}

//public enum MediaTypes
//{
//    Json,
//    Xml
//}

//public enum MessageEventTypes
//{
//    Status,
//    Warning,
//    Error
//}

//[Flags]
//public enum OverridableBys
//{
//    None = 0,
//    UI = 1,
//    Handheld = 2,
//    All = UI | Handheld
//}

//public enum OverrideApprovalGroups
//{
//    None = 0,
//    ConsumeNonQuality = 1,
//    ConsumePreMinimumAge = 2,
//    ConsumeOldAge = 3,
//    ConsumeNewer = 4,
//    ConsumeWithoutQaTestLot = 5,
//    ConsumeBeforePriorQaTestLotComplete = 6,
//    ShipNewer = 7,
//    ExpiredProduct = 8,
//    ShipInventoryWithoutProcessOrder = 9,
//    OvershipOutboundDeliveries = 10
//}

//public enum ProductDisplayTypes
//{
//    [StringValue("Product Code")]
//    ProductCode,
//    [StringValue("Material")]
//    MaterialNumber
//}

////public enum ProductGroupTypes
////{
////    SAPClass = 1,
////    UserDefined = 2,
////    ProductSet = 3,
////    BoxShippingSet = 4,
////    TcdClass = 5
////}

//public enum PurposeNames
//{
//    EwmAuthentication,
//    ItAuthentication,
//    MiiAuthentication,
//    RestAuthentication
//}

//public enum ReportOutputStatuses
//{
//    Orphaned = 1,
//    Running = 2,
//    Finished = 3,
//    Completed = 4
//}

//public enum SerialNumberFormatTypes
//{
//    BoarsHead = 1,
//    FourStar = 2,
//    CargillMeatSolutions = 3,
//    PatrickCudahay = 4,
//    Godshalls = 5,
//    HoneySuckleWhite = 6,
//    SwissAmerican = 7,
//    Calgary = 8,
//    NoProductionDate = 9
//}

//public enum SerializationTypes
//{
//    Html,
//    Json,
//    Xml,
//    XmlText
//}

//public enum SourceSystems
//{
//    [StringValue("CBS")]
//    Cbs,
//    [StringValue("COM")]
//    Com,
//    [StringValue("IPFS")]
//    Ipfs,
//    [StringValue("NAV")]
//    Nav,
//    [StringValue("PRISM")]
//    Prism,
//    [StringValue("PROC")]
//    Procurement,
//    [StringValue("PROTEAN")]
//    Protean,
//    [StringValue("RETRO")]
//    Retrotech,
//    [StringValue("SAP")]
//    Sap,
//    [StringValue("TCD")]
//    Tcd
//}

//public enum UnitOfMeasures
//{
//    [StringValue("EA")]
//    Each = 0,
//    [StringValue("CS")]
//    Cases = 1,
//    [StringValue("KGM")]
//    Kilograms = 2,
//    [StringValue("LBR")]
//    Pounds = 3,
//    [StringValue("FA")]
//    Fahrenheit = 4,
//    [StringValue("C")]
//    Celsius = 5,
//    [StringValue("BIN")]
//    Bin = 6
//}

//public enum YieldPriceTypes
//{
//    [StringValue("Breakeven")]
//    Breakeven = 1,
//    [StringValue("Fixed")]
//    Fixed = 2,
//    [StringValue("Sales")]
//    Sales = 3,
//}