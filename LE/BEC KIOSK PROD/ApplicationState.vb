Option Explicit On
Imports System.Configuration.ConfigurationSettings
Imports Oracle.ManagedDataAccess.Client
Imports System.Text
Imports System.IO

Public Class ApplicationState
    '4/16/08 David Maibor (DSM): Add destinationLocation for SAP service call, PostSerializedProduction
    Public Enum ActionType As Integer
        PROCESS_BROADCASTCODE = 1
        PROCESS_SCANPARTS_COMMAND = 2
        PROCESS_PARTSCAN_SINGLEBOX = 3
        PROCESS_CORRECT_LAYER_COUNT = 4
        PROCESS_CORRECT_PACKAGE_COUNT = 5
        PROCESS_CORRECT_BOX_COUNT = 6
        PROCESS_IS_LAYER_COMPLETE = 7
        PROCESS_IS_BOX_COMPLETE = 8
        PROCESS_IS_PACKAGE_COMPLETE = 9
        PROCESS_IS_TRAY_COMPLETE = 10
    End Enum
    Private configSettings As New ConfigurationSettings

    'Private local variables
    Private pDestinationLocation As String 'DSM: added for SAP call
    Private pNextAction As String
    Private pContainerType As String
    Private pInProgress As Boolean
    Private pECL As String
    Private pLabelType As String
    Private pMESPartID As Long
    Private pDelphiPartNumber As String
    Private pCustomerPartNumber As String
    Private pBroadcastCode As String
    Private pCurrentLotNBR As String
    Private pDepartment As String
    Private pStartTime As DateTime
    Private pEndTime As DateTime
    Private pIsLastBox As Boolean
    Private pLastScan As DateTime
    Private pLocation As String
    Private pNumberOfBoxes As Integer
    Private pNumberOfPartsperBox As Integer
    Private pNumberOfLayersPerBox As Integer
    Private pNumberPkgsPerSkid As Int16
    Private pDestinationCode As String
    Private pPackageCode As String
    Private pProductionRunNbr As String
    Private pStdPack As Integer
    Private pSystemFlag As String
    Private pTotalPartsScanned As Integer
    Private pcustomerCode As String
    Private pNextActionType As ActionType
    Private bScheduleRequired As Boolean
    Private pDelayBetweenScans As Integer
    Private pLastSerial As String
    Private pBecSeries As String
    Private pSkidPkgCount As Integer
    Private pBufferLabelCount As Int16
    Private pBufferLabelPointer As Int16
    Private pscheduleID As Long
    Public BufferedLabels() As String 'used to house the buffered labels
    Public Scans() As String
    Public ScanTime() As String
    Public RemovedBECs() As String
    
    Public Sub New()

        On Error Resume Next
        'Retrieve the application state and set the properties
        pNextAction = configSettings.Appsettings("//ApplicationState/NextAction")
        pContainerType = configSettings.Appsettings("//ApplicationState/ContainerType")
        pInProgress = Convert.ToBoolean(configSettings.Appsettings("//ApplicationState/InProgress"))
        pECL = configSettings.Appsettings("//ApplicationState/ECL")
        pLabelType = configSettings.Appsettings("//ApplicationState/LabelType")
        pMESPartID = Convert.ToInt64(configSettings.Appsettings("//ApplicationState/MESPartID"))
        pDelphiPartNumber = configSettings.Appsettings("//ApplicationState/DelphiPartNumber")
        pCustomerPartNumber = configSettings.Appsettings("//ApplicationState/CustomerPartNumber")
        pBroadcastCode = configSettings.Appsettings("//ApplicationState/BroadcastCode")
        pCurrentLotNBR = configSettings.Appsettings("//ApplicationState/CurrentLotNBR")
        pDepartment = configSettings.Appsettings("//Properties/Department")
        pStartTime = Convert.ToDateTime(configSettings.Appsettings("//ApplicationState/StartTime"))
        pEndTime = Convert.ToDateTime(configSettings.Appsettings("//ApplicationState/EndTime"))
        pIsLastBox = Convert.ToBoolean(configSettings.Appsettings("//ApplicationState/IsLastBox"))
        pLastScan = Convert.ToDateTime(configSettings.Appsettings("//ApplicationState/LastScan"))
        pLocation = configSettings.Appsettings("//ApplicationState/Location")
        pNumberOfBoxes = Convert.ToInt32(configSettings.Appsettings("//ApplicationState/NumberOfBoxes"))
        pNumberOfLayersPerBox = Convert.ToInt32(configSettings.Appsettings("//ApplicationState/NumberOfLayersPerBox"))
        pPackageCode = configSettings.Appsettings("//ApplicationState/PackageCode")
        pProductionRunNbr = configSettings.Appsettings("//ApplicationState/ProductionRunNbr")
        pStdPack = Convert.ToInt32(configSettings.Appsettings("//ApplicationState/StdPack"))
        pSystemFlag = configSettings.Appsettings("//ApplicationState/SystemFlag")
        pTotalPartsScanned = Convert.ToInt32(configSettings.Appsettings("//ApplicationState/TotalPartsScanned"))
        pcustomerCode = configSettings.Appsettings("//ApplicationState/CustomerCode")
        pDelayBetweenScans = configSettings.Appsettings("//ApplicationState/DelayBetweenScans")
        pLastSerial = configSettings.Appsettings("//ApplicationState/LastSerialNbr")
        bScheduleRequired = configSettings.Appsettings("//ApplicationState/ScheduleRequired")
        pBecSeries = configSettings.Appsettings("//ApplicationState/becseries")
        pDestinationCode = configSettings.Appsettings("//ApplicationState/DestinationCode")
        pDestinationLocation = configSettings.Appsettings("//ApplicationState/DestinationLocation") 'DSM: added for SAP call
        pNumberPkgsPerSkid = configSettings.Appsettings("//ApplicationState/NumberPkgsPerSkid")
        pSkidPkgCount = configSettings.Appsettings("//ApplicationState/SkidPkgCount")
        Dim sNextActionType As String = configSettings.Appsettings("//ApplicationState/ActionType")
        pBufferLabelCount = configSettings.Appsettings("//ApplicationState/BufferedLabelCount")
        pBufferLabelPointer = configSettings.Appsettings("//ApplicationState/BufferedLabelPointer")
        pscheduleID = configSettings.Appsettings("//ApplicationState/scheduleID")

        Select Case sNextActionType
            Case "PROCESS_BROADCASTCODE"
                pNextActionType = ActionType.PROCESS_BROADCASTCODE
            Case "PROCESS_SCANPARTS_COMMAND"
                pNextActionType = ActionType.PROCESS_SCANPARTS_COMMAND
            Case "PROCESS_PARTSCAN_SINGLEBOX"
                pNextActionType = ActionType.PROCESS_PARTSCAN_SINGLEBOX
            Case "PROCESS_CORRECT_LAYER_COUNT"
                pNextActionType = ActionType.PROCESS_CORRECT_LAYER_COUNT
            Case "PROCESS_CORRECT_PACKAGE_COUNT"
                pNextActionType = ActionType.PROCESS_CORRECT_PACKAGE_COUNT
            Case "PROCESS_CORRECT_BOX_COUNT"
                pNextActionType = ActionType.PROCESS_CORRECT_BOX_COUNT
            Case "PROCESS_IS_LAYER_COMPLETE"
                pNextActionType = ActionType.PROCESS_IS_LAYER_COMPLETE
            Case "PROCESS_IS_BOX_COMPLETE"
                pNextActionType = ActionType.PROCESS_IS_BOX_COMPLETE
            Case "PROCESS_IS_PACKAGE_COMPLETE"
                pNextActionType = ActionType.PROCESS_IS_PACKAGE_COMPLETE
        End Select

        Dim s As String = ""
        s = configSettings.Appsettings("//ApplicationState/InProgress")
        If s <> "True" Then
            'Start the application from scratch
            NextAction = "Choose Container Type"
            InProgress = False
        End If

    End Sub
   
    
    Public Property LastSerialNbr()
        Get
            Return pLastSerial
        End Get
        Set(ByVal Value)
            pLastSerial = CType(Value, String)
            configSettings.Appsettings("//ApplicationState/LastSerialNbr") = Value
        End Set
    End Property
    Public Property CustomerCode() As String
        Get
            Return pcustomerCode
        End Get
        Set(ByVal Value As String)
            pcustomerCode = Value
            configSettings.Appsettings("//ApplicationState/CustomerCode") = Value
        End Set
    End Property
    Public Property ScheduleRequired() As Boolean
        Get
            ScheduleRequired = bScheduleRequired
        End Get
        Set(ByVal Value As Boolean)
            bScheduleRequired = Value
            configSettings.Appsettings("//ApplicationState/ScheduleRequired") = Value.ToString
        End Set
    End Property
    Public ReadOnly Property CurrentBoxCount() As Integer
        Get
            If pTotalPartsScanned Mod NumberOfPartsPerBox = 0 Then
                Return pTotalPartsScanned \ NumberOfPartsPerBox
            Else
                Return pTotalPartsScanned \ NumberOfPartsPerBox + 1
            End If
        End Get
    End Property
    Public ReadOnly Property CurrentBoxPartCount() As Int64
        Get
            If pTotalPartsScanned Mod NumberOfPartsPerBox = 0 Then
                Return NumberOfPartsPerBox
            Else
                Return pTotalPartsScanned Mod NumberOfPartsPerBox
            End If
        End Get
    End Property
    Public ReadOnly Property IsFirstPartOfBox() As Boolean
        Get
            Return ((pTotalPartsScanned Mod NumberOfPartsPerBox) = 1)
        End Get
    End Property

    Public ReadOnly Property NumberOfPartsPerBox() As Int32
        Get
            Return StdPack / NumberOfBoxes
        End Get
    End Property

    Public ReadOnly Property IsMultipleBoxes() As Boolean
        Get
            Return pNumberOfBoxes > 1
        End Get
    End Property

    Public Property NextActionType() As ActionType
        Get
            Return pNextActionType
        End Get
        Set(ByVal Value As ActionType)
            pNextActionType = Value
            configSettings.Appsettings("//ApplicationState/ActionType") = Value.ToString
        End Set
    End Property
    Public Property BufferedLabelCount() As Int16
        Get
            BufferedLabelCount = pBufferLabelCount
        End Get
        Set(ByVal Value As Int16)
            pBufferLabelCount = Value
            configSettings.Appsettings("//ApplicationState/BufferedLabelCount") = Value
        End Set
    End Property
    Public Property BufferedLabelPointer() As Int16
        Get
            BufferedLabelPointer = pBufferLabelPointer
        End Get
        Set(ByVal Value As Int16)
            pBufferLabelPointer = Value
            configSettings.Appsettings("//ApplicationState/BufferedLabelPointer") = Value
        End Set
    End Property
    Public Property SystemFlag() As String
        Get
            Return pSystemFlag
        End Get
        Set(ByVal Value As String)
            pSystemFlag = Value
            configSettings.Appsettings("//ApplicationState/SystemFlag") = Value
        End Set
    End Property

    Public Property TotalPartsScanned() As Integer
        Get
            Return pTotalPartsScanned
        End Get
        Set(ByVal Value As Integer)
            pTotalPartsScanned = Value
            configSettings.Appsettings("//ApplicationState/TotalPartsScanned") = Value.ToString
        End Set
    End Property

    Public Property StdPack() As Integer
        Get
            Return pStdPack
        End Get
        Set(ByVal Value As Integer)
            pStdPack = Value
            configSettings.Appsettings("//ApplicationState/StdPack") = Value.ToString
        End Set
    End Property

    Public Property PackageCode() As String
        Get
            Return pPackageCode
        End Get
        Set(ByVal Value As String)
            pPackageCode = Value
            configSettings.Appsettings("//ApplicationState/PackageCode") = Value
        End Set
    End Property

    Public Property ProductionRunNbr() As String
        Get
            Return pProductionRunNbr
        End Get
        Set(ByVal Value As String)
            pProductionRunNbr = Value
            configSettings.Appsettings("//ApplicationState/ProductionRunNbr") = Value
        End Set
    End Property

    Public Property NumberOfLayersPerBox() As Integer
        Get
            Return pNumberOfLayersPerBox
        End Get
        Set(ByVal Value As Integer)
            pNumberOfLayersPerBox = Value
            configSettings.Appsettings("//ApplicationState/NumberOfLayersPerBox") = Value.ToString
        End Set
    End Property

    Public Property NumberOfBoxes() As Integer
        Get
            Return pNumberOfBoxes
        End Get
        Set(ByVal Value As Integer)
            pNumberOfBoxes = Value
            configSettings.Appsettings("//ApplicationState/NumberOfBoxes") = Value.ToString
        End Set
    End Property

    Public Property LastScan() As DateTime
        Get
            Return pLastScan
        End Get
        Set(ByVal Value As DateTime)
            pLastScan = Value
            configSettings.Appsettings("//ApplicationState/LastScan") = Value.ToString
        End Set
    End Property
    Public Property DelayBetweenScans()
        Get
            DelayBetweenScans = pDelayBetweenScans
        End Get
        Set(ByVal Value)
        End Set
    End Property

    Public Property Location() As String
        Get
            Return pLocation
        End Get
        Set(ByVal Value As String)
            pLocation = Value
            configSettings.Appsettings("//ApplicationState/Location") = Value
        End Set
    End Property

    Public Property IsLastBox() As Boolean
        Get
            Return pIsLastBox
        End Get
        Set(ByVal Value As Boolean)
            pIsLastBox = Value
            configSettings.Appsettings("//ApplicationState/IsLastBox") = Value.ToString
        End Set
    End Property

    Public Property StartTime() As DateTime
        Get
            Return pStartTime
        End Get
        Set(ByVal Value As DateTime)
            pStartTime = Value
            configSettings.Appsettings("//ApplicationState/StartTime") = Value.ToString
        End Set
    End Property

    Public Property EndTime() As DateTime
        Get
            Return pEndTime
        End Get
        Set(ByVal Value As DateTime)
            pEndTime = Value
            configSettings.Appsettings("//ApplicationState/EndTime") = Value.ToString
        End Set
    End Property

    Public Property Department() As String
        Get
            Return pDepartment
        End Get
        Set(ByVal Value As String)
            pDepartment = Value
            configSettings.Appsettings("//Properties/Department") = Value
        End Set
    End Property

    Public Property BroadcastCode() As String
        Get
            Return pBroadcastCode
        End Get
        Set(ByVal Value As String)
            pBroadcastCode = Value
            configSettings.Appsettings("//ApplicationState/BroadcastCode") = Value
        End Set
    End Property
    Public Property BecSeries() As String
        Get
            Return pBecSeries
        End Get
        Set(ByVal Value As String)
            pBecSeries = Value
            configSettings.Appsettings("//ApplicationState/becseries") = Value
        End Set
    End Property


    Public Property CurrentLotNbr() As String
        Get
            Return pCurrentLotNBR
        End Get
        Set(ByVal Value As String)
            pCurrentLotNBR = Value
            configSettings.Appsettings("//ApplicationState/CurrentLotNBR") = Value
        End Set
    End Property

    Public Property MESPartID() As Long
        Get
            Return pMESPartID
        End Get
        Set(ByVal Value As Long)
            pMESPartID = Value
            configSettings.Appsettings("//ApplicationState/MESPartID") = Value.ToString
        End Set
    End Property

    Public Property DelphiPartNumber() As String
        Get
            Return pDelphiPartNumber
        End Get
        Set(ByVal Value As String)
            pDelphiPartNumber = Value
            configSettings.Appsettings("//ApplicationState/DelphiPartNumber") = Value
        End Set
    End Property
    Public Property CustomerPartNumber() As String

        Get
            Return pCustomerPartNumber
        End Get
        Set(ByVal Value As String)
            On Error Resume Next
            pCustomerPartNumber = Value & ""
            configSettings.Appsettings("//ApplicationState/CustomerPartNumber") = Value
        End Set
    End Property

    Public Property NextAction() As String
        Get
            Return pNextAction
        End Get
        Set(ByVal Value As String)
            pNextAction = Value
            configSettings.Appsettings("//ApplicationState/NextAction") = Value
        End Set
    End Property

    Public Property ContainerType() As String
        Get
            Return pContainerType
        End Get
        Set(ByVal Value As String)
            pContainerType = Value
            configSettings.Appsettings("//ApplicationState/ContainerType") = Value
        End Set
    End Property

    Public Property InProgress() As Boolean
        Get
            Return pInProgress
        End Get
        Set(ByVal Value As Boolean)
            pInProgress = Value
            configSettings.Appsettings("//ApplicationState/InProgress") = Value.ToString
        End Set
    End Property

    Public Property ECL() As String
        Get
            Return pECL
        End Get
        Set(ByVal Value As String)
            pECL = Value
            configSettings.Appsettings("//ApplicationState/ECL") = Value
        End Set
    End Property

    Public Property LabelType() As String
        Get
            Return pLabelType
        End Get
        Set(ByVal Value As String)
            pLabelType = Value
            configSettings.Appsettings("//ApplicationState/LabelType") = Value
        End Set
    End Property
    Public Property DestinationLocation() As String
        'DSM: added for SAP call
        Get
            Return pDestinationLocation
        End Get
        Set(ByVal Value As String)
            pDestinationLocation = Value
            configSettings.Appsettings("//ApplicationState/DestinationLocation") = Value
        End Set
    End Property
    Public Property DestinationCode() As String
        Get
            Return pDestinationCode
        End Get
        Set(ByVal Value As String)
            pDestinationCode = Value
            configSettings.Appsettings("//ApplicationState/DestinationCode") = Value
        End Set
    End Property
    Public Property ScheduleId() As Long
        Get
            ScheduleId = pscheduleID
        End Get
        Set(ByVal Value As Long)
            pscheduleID = Value
            configSettings.Appsettings("//ApplicationState/scheduleID") = Value
        End Set
    End Property



    Public Property NumberPkgsPerSkid()
        Get
            Return pNumberPkgsPerSkid
        End Get
        Set(ByVal Value)
            pNumberPkgsPerSkid = Value
            configSettings.Appsettings("//ApplicationState/NumberPkgsPerSkid") = Value
        End Set
    End Property
    Public Property SkidPackageCount()
        Get
            Return pSkidPkgCount
        End Get
        Set(ByVal Value)
            pSkidPkgCount = Value
            configSettings.Appsettings("//ApplicationState/SkidPkgCount") = Value
        End Set
    End Property


End Class
