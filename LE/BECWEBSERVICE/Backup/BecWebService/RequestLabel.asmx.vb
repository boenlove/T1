Option Explicit On 

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data.OracleClient
Imports System.Text
Imports System.Configuration
Imports System.IO
Public Enum logTypes
    Reprint = 1
    AbortSkid = 2
    DeleteSchedule = 3
End Enum
<System.Web.Services.WebService(Namespace:="http://tempuri.org/BecWebService/RequestLabel")> _
Public Class RequestLabel
    Inherits System.Web.Services.WebService

    Public Enum StandardPackTypes
        ICS = 0
        MTMS
        ICS_MASTER
        MTMS_MASTER
    End Enum

    Private Structure MessageStructure
        Public PartNumber, Destination, OperatorId, PackageCode, LotNumber, ComponentIdentifiers() As String
        Public Machine, ResponseQueue, SerialNumber, Department, PrinterName, ECL As String
        Public StartTime, EndTime As Date
        Public Quantity, BoxCount As Integer
        Public StandardPackType As StandardPackTypes
    End Structure

    Public Structure requestpacket
        Public Machine As String
        Public part As String
        Public CustPart As String
        Public OperID As String
        Public Command As String
        Public RequestedWhen As Object
        Public completedwhen As Object
        Public ActiveRequest As Boolean
        Public ProdRun As Long
        Public Tool As String
        Public PackageCode As String
        Public Quantity As String
        Public Alert As String
        Public MfgDept As String
        Public OperName As String
        Public Revision As String
        Public Color As String
        Public Description As String
        Public GROSSWT As String
        Public NETWT As String
        Public UOM As String
        Public Material As String
        Public NBRCav As String
        Public Serial As String
        Public CNTR As String
        Public Customer As String
        Public NumPkgs As Integer
        Public lblType As String  'Master, Container - specifies whether the label data is for container label or master label
        Public IDsfx As String
        Public Cavity As String
        Public lCntrID As Long
        Public lmesMachID As Long
        Public lmesPartID As Long
        Public lMesToolId As Long
        Public lStdPackID As Long
        Public ScheduleRequired As Boolean
        Public Department As String
        Public ReportGroupID As Long
        Public SerializedBECs As String
    End Structure
    Public Structure ICS_dataPacket
        Public Std_non_std_flag As String
        Public PART_NBR As String
        Public FormSize_CD As String
        Public Quantity As String
        Public Weight As String
        Public ecl As String
        Public Serial_number As String
        Public Department As String
        Public PACKAGE_CODE As String
        Public Pay_suffix As String
        Public MATID As String
        Public Description As String
        Public Warehouse_loc As String
        Public Customer_code As String
        Public Customer_part_no As String
        Public Broadcast_code As String
        Public Today_label_counter As String
        Public Full_label_counter As String
        Public Labels_ordered As String
        Public Delivery_loc_number As String
        Public Ref_seq_data_id As String
        Public Ref_seq_number As String
        Public Package_12z_seq_number As String
        Public Package_13z_seq_number As String
        Public Package_14z_seq_number As String
        Public Package_15z_seq_number As String
        Public Package_16z_seq_number As String
        Public Package_17z_seq_number As String
        Public check_digit As String
        Public series As String
        Public serial_type As String
        Public serial_8digits As String
        Public ICSDest As String
        Public CustomerName As String
        Public CustomerStreetAddr As String
        Public CustomerCity As String
        Public CntrPackageType As String
        Public PrinterID As String
        Public Customer As String
        Public lblType As String
        Public Machine As String
        Public SortIndex As Integer
        Public ProdRunID As Long
        Public BoxCount As String
        Public SupplierID As String
        Public Cisco_Code As String
    End Structure
    Private moCN As OracleConnection
    'Private rsOra as ora
    Friend trans As OracleTransaction 'this is used to control the rollback
    Const LOG_SOURCE = "BecWebService"

    Const NEW_SN_SEPARATOR = "<<newSN>>"
    Private newSN_2 As String
    Private newSN_3 As String
    Private newSN_4 As String

    Public Class Cordial
        Inherits SoapHeader
        Public UserID As String
        Public Password As String
    End Class
    Public greeting As New Cordial
    Private MatID As Integer 'DSM: add for use in GetICSDockCodes 

    Public Class clsDestination
        'DSM: add Location for SAP service call to PostSerializedProduction
        Public location As String 'DSM: added for SAP
        Public mes_part_id As Long
        Public MachineName As String
        Public Part_nbr As String
        Public ScheduleID As Long
        Public mes_machine_id As Long
        Public Revision_Physical As String
        Public CNTR As String
        Public Package_Code As String
        Public QtyPkgsperSkidReq As Long
        Public QtyPkgsUsed As Long
        Public ControlFlag As String 'used to specify how to interact with the schedule "peek;reserve;willprint"
        Public Completed As Boolean  'this schedule entry has been satisfied.  This can be used to determine when to print a skid summary also.
        Public MatID As String  'matid associated with the schedule entry. this feature is not being enforced from the scheudling side so is not currently used.
        Public SerialNbr As String
        Public UseGeneric As Boolean 'used to pass along the fact that we will be working with a generic label.
        Public QtyPieces As Int16
        Public LookFor As String 'this is what we will look for when getting the ICS data.  This is done so we can look by serial or machine_name       
    End Class
#Region " Web Services Designer Generated Code "
    Public Sub New()
        MyBase.New()

        'This call is required by the Web Services Designer.
        InitializeComponent()

        'Add your own initialization code after the InitializeComponent() call

    End Sub
    'Required by the Web Services Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Web Services Designer
    'It can be modified using the Web Services Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        'CODEGEN: This procedure is required by the Web Services Designer
        'Do not modify it using the code editor.
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
#End Region
    Private Sub GetSAPBecPartCntrInfo(ByVal delphi_part_nbr As String, ByVal shipto_nbr As String, ByVal ship_package As String, ByRef SAPdataset As DataSet)

        'David Maibor (DSM) update the BEC_Part_Cntr_Info table in SAPdataset with the info or 'failure'
        Dim sqli As New StringBuilder
        Dim cn As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim cmd As New OracleCommand
        Dim da As OracleDataAdapter
        Dim ds As New DataSet

        Try
            cn.Open()

            sqli.Append("select PRIME_CNTR,PRIME_UOI,GROSS_WEIGHT ")
            sqli.Append("FROM SAP.BEC_PART_CNTR_INFO ")
            sqli.Append("WHERE PLANT = 'FT23' and ")
            sqli.Append("MATERIAL_NBR = '" & delphi_part_nbr & "' and ")
            sqli.Append("SHIP_TO_CODE = '" & shipto_nbr & "' and ")
            sqli.Append("SHIP_PACKAGE = '" & ship_package & "'")

            With cmd
                .Connection = cn
                .CommandText = sqli.ToString
                .CommandType = CommandType.Text
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                'DSM: update the table in SAPdataset with the conainer info
                SAPdataset.Tables("BEC_Part_Cntr_Info").Rows(0).Item("result") = "success"
                SAPdataset.Tables("BEC_Part_Cntr_Info").Rows(0).Item("std_pack_cntr") = ds.Tables(0).Rows(0).Item("PRIME_CNTR")
                SAPdataset.Tables("BEC_Part_Cntr_Info").Rows(0).Item("quantity") = ds.Tables(0).Rows(0).Item("PRIME_UOI")
                SAPdataset.Tables("BEC_Part_Cntr_Info").Rows(0).Item("weight") = ds.Tables(0).Rows(0).Item("GROSS_WEIGHT")
            Else
                SAPdataset.Tables("BEC_Part_Cntr_Info").Rows(0).Item("result") = "failure: no data in SAP BEC_PART_CNTR_INFO table"
            End If
        Catch ex As Exception
            SAPdataset.Tables("BEC_Part_Cntr_Info").Rows(0).Item("result") = "failure: exception in BECWEBSERVICE, GetSAPBecPartCntrInfo routine"
        Finally
            cn.Close()
            cn.Dispose()
            da.Dispose()
            cmd.Dispose()
            ds.Dispose()
        End Try
    End Sub
    Private Sub GetSAPCustomerData(ByVal delphi_part_nbr As String, ByVal shipto_nbr As String, ByVal ship_package As String, ByRef SAPdataset As DataSet)
        '6/2/08: DSM: return SAPdataset with two tables if successful, otherwise result is 'failure' in BEC_
        '3/19/08: David Maibor (DSM)created
        Dim sqli As New StringBuilder
        Dim cn As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim cmd As New OracleCommand
        Dim da As OracleDataAdapter
        Dim ds As New DataSet
        Dim dt As New DataTable("SAP_label_data")


        Try
            cn.Open()

            'Get the customer info from the SAP label_data table using the passed in delphi part# & shipto_nbr. If successful put the table
            ' into SAPdataset
            sqli.Append("select cust_name_1 AS cust_name,")
            sqli.Append("cust_address,")
            sqli.Append("cust_city,")
            sqli.Append("cust_state,")
            sqli.Append("cust_zip_code,")
            sqli.Append("cust_cntry,")
            sqli.Append("ship_to,")
            sqli.Append("material_desc AS part_description,")
            sqli.Append("plant_dock_line_1,")
            sqli.Append("DUNNS_CODE ")  'ACR 4/19/08 Need to read Dunns Code from Label Data for SAP transmission
            sqli.Append(" from snapmgr.label_data ")
            If shipto_nbr.Trim = "generic" Then   'ACR 4/20/08 this call was failing for generics
                sqli.Append("where material_nbr = '" & delphi_part_nbr & "'")
            Else
                sqli.Append("where material_nbr = '" & delphi_part_nbr & "' and SHIP_TO  = '" & shipto_nbr & "'")
            End If

            With cmd
                .Connection = cn
                .CommandText = sqli.ToString
                .CommandType = CommandType.Text
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(SAPdataset)

            If SAPdataset.Tables(1).Rows.Count > 0 Then
                SAPdataset.Tables(1).TableName = "SAP_label_data"
                'Get the SAP cntr info 
                GetSAPBecPartCntrInfo(delphi_part_nbr, shipto_nbr, ship_package, SAPdataset)
            Else
                'No info in the label data table, reutrn 'failure' in the BEC_Part_Cntr_Info table
                SAPdataset.Tables("BEC_Part_Cntr_Info").Rows(0).Item("result") = "failure: in BECWEBSERVICE, GetSAPCustomerData, SAP label_data table has no data."
                'std_pack_cntr
                Exit Sub
            End If

        Catch ex As Exception
            SAPdataset.Tables("BEC_Part_Cntr_Info").Rows(0).Item("result") = "failure: in BECWEBSERVICE, GetSAPCustomerData failed."
        Finally
            cn.Close()
            cn.Dispose()
            da.Dispose()
            cmd.Dispose()
            ds.Dispose()
        End Try
    End Sub
    Private Function GetSAPShiptoNbr(ByVal serial_nbr As String) As DataSet

        '3/29/08 David Maibor (DSM). Created. Get the Delphi Part # & SAP Shipto number from the ICS LABEL DATA TABLE using the Serial# to
        '  locate the right record. The Shipto # is placed in the Customer_Filler field by the scheduling program

        Dim sqli As New StringBuilder
        Dim cn As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim cmd As New OracleCommand
        Dim da As OracleDataAdapter
        Dim ds As New DataSet


        Try
            cn.Open()

            'Get the Shipto_Nbr contained in the customer_filler field of the ics_label_data view
            sqli.Append("select part_nbr As Delphi_part_nbr,customer_filler AS shipto_nbr from pltfloor.ics_label_data_vw ")
            sqli.Append("where serial_nbr  = '" & serial_nbr & "' and rownum = 1")
            With cmd
                .Connection = cn
                .CommandText = sqli.ToString
                .CommandType = CommandType.Text
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)

            Return ds

        Catch ex As Exception
            'not able to get the customer_filler record for this machine
        Finally
            cn.Close()
            cn.Dispose()
            da.Dispose()
            cmd.Dispose()
            ds.Dispose()
        End Try

    End Function
    Private Function Build_BEC_Part_Cntr_Info_table() As DataTable
        Dim dr As DataRow
        Dim dt As New DataTable("BEC_Part_Cntr_Info")

        'Create a table for BEC_Part_Cntr_Info with a field for the result of getting the info
        dt.Columns.Add("result", GetType(System.String))
        dt.Columns.Add("std_pack_cntr", GetType(System.String))
        dt.Columns.Add("quantity", GetType(System.Double))
        dt.Columns.Add("weight", GetType(System.Double))
        dr = dt.NewRow
        dt.Rows.Add(dr)
        Return dt
    End Function
    <WebMethod()> _
    Public Function GetSAPLabelData(ByVal serial_nbr As String, ByVal ship_package As String) As DataSet
        '6/2/08 DSM: modify to extract data from SAP.BEC_Part_Cntr_Info table and SNAPMGR.Label_data table. Add this table to the returned Dataset. 
        '3/19/08: David Maibor (DSM)created to extract data from the SAP LABEL_DATA table for some of the parameters in the SAP PostSerializedProduction web method
        '  called by the BecScanner program


        Dim dt As New DataTable
        Dim ds As New DataSet   'Temp dataset
        Dim Delphi_part_nbr As String
        Dim SAPdataset As New DataSet   'Returning dataset with info
        Dim shipto_nbr As String

        dt = Build_BEC_Part_Cntr_Info_table()
        SAPdataset.Tables.Add(dt)

        Try
            ds = GetSAPShiptoNbr(serial_nbr)
            'Return failure in BEC_Part_Cntr_Info table if didn't get Shipto info
            If ds.Tables(0).Rows.Count = 0 Then
                SAPdataset.Tables("BEC_Part_Cntr_Info").Rows(0).Item("result") = "failure: in BECWEBSERVICE, GetSAPShiptoNbr has no data."
            Else
                Delphi_part_nbr = ds.Tables(0).Rows(0).Item("Delphi_part_nbr")
                shipto_nbr = ds.Tables(0).Rows(0).Item("shipto_nbr")
                ds.Clear()
                GetSAPCustomerData(Delphi_part_nbr, shipto_nbr, ship_package, SAPdataset)
            End If
        Catch ex As Exception
            SAPdataset.Tables("BEC_Part_Cntr_Info").Rows(0).Item("result") = "failure: in BECWEBSERVICE, GetSAPLabelData method failed."
        Finally
            GetSAPLabelData = SAPdataset
        End Try

    End Function

    <WebMethod()> _
        Public Function PrintLabel_P23(ByVal dsRequest As DataSet) As String
        Dim Destination As New clsDestination
        Dim stMessage As MessageStructure
        Dim retv As Boolean
        Dim rets As String
        Dim dloc As New DataSet
        Dim sqli As New StringBuilder
        Dim cnOra As OracleConnection
        Dim cmd As New OracleCommand
        Dim da As New OracleDataAdapter
        Dim labelreq As requestpacket
        Dim dsICS_data As New DataSet
        Dim labelfile As String
        Dim theError As String
        Dim el As Integer
        Dim FindThis As String


        'Dim trans As OracleTransaction

        Try
            'Open up a Connection to the DataBase
            If cnOra Is Nothing Then
                el = 204
                cnOra = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
                el = 205
                cnOra.Open()
                cmd.Connection = cnOra
                el = 208
                trans = cnOra.BeginTransaction
                cmd.Transaction = trans
            End If

            'This decodes the message from the queue and returns the variable in the structure
            '1  decode input and lookup mespart and mesmachine for this request
            el = 216
            DecodeMessage(dsRequest.Tables(0), labelreq)
            el = 218
            labelreq.lmesPartID = ConvertFromParttoMesPart(labelreq.part, cnOra)
            el = 220
            labelreq.lmesMachID = ConvertFromMachineNbrtoMesMachineID(labelreq.Machine, cnOra)
            If labelreq.Department.Trim <> "" Then
                labelreq.ReportGroupID = CType(ConvertFromReportGroupIDtoDept(labelreq.Department, cnOra, True), Long)
            End If

            '2  4/21/05  PAW added this code to get the destination for the ICS label--customer_direct_ICS only
            With Destination
                .mes_machine_id = labelreq.lmesMachID
                .MachineName = labelreq.Machine
                .mes_part_id = labelreq.lmesPartID
                .Part_nbr = labelreq.part
                .Package_Code = labelreq.PackageCode
                .Revision_Physical = labelreq.Revision
                .QtyPieces = labelreq.Quantity
                .ControlFlag = "WillPrint"
            End With
            If labelreq.ScheduleRequired Then
                el = 237
                rets = GetShipDestination_withAutoGeneric(Destination, cnOra) 'returns either failure statement, "serial:########## or "MachineName:ScheduleID-QtyPackagesPrinted" 

                If InStr(rets, "Failure") <> 0 Then
                    'dont get the dock codes
                    Throw New Exception(rets)
                Else
                    If Destination.UseGeneric = False Then
                        el = 243
                        dloc = GetICSDockCodes(Destination.mes_part_id, Destination.Part_nbr, Destination.CNTR, cnOra)
                    End If
                End If
            Else
                'production run was set for Generics.
                'The way the logic is designed, a call to reserve a serial is done when they start packing.  So, at this point, where the 
                'label is requested, there should already be a serial number, but there may not be one.  So, we need to attempt to use 
                'one already reserved first.  Then if for somereason there is not one, we must reserve one.

                Destination.UseGeneric = True
                rets = ReserveSerials(Destination, 1)
                'rets = 1882817926
                'rets = 1888001854 'DUNNS CODE: 084668367, PART: 25905683, MATERIAL HANDLING CODE: MU14, PLANT DOCK: SH CMA
                'rets = 1883975635
                If InStr(rets, "Failure") <> 0 Then
                    'failed to reserve a serial
                    Throw New Exception("Failure reserving serial number.")
                Else
                    'since directly calling reserveserials returns a serial, put the prefix that calling the schedule will put
                    rets = "Serial:" & rets
                End If
            End If

            Try
                el = 249
                FindThis = Mid(rets, InStr(1, rets, ":") + 1)
                If InStr(rets, "Serial:") <> 0 Then
                    dsICS_data = GETICSDATA(labelreq, True, FindThis)
                Else
                    dsICS_data = GETICSDATA(labelreq, False, FindThis)
                End If

            Catch ex As Exception
                If InStr(ex.Message, "The data value could not be converted for reasons") <> 0 Then
                    Throw New Exception("Failure Getting ICS Data: ICS Label Data not available")
                Else
                    Throw New Exception(ex.GetBaseException.Message)
                End If
            End Try

            el = 259
            If (dsICS_data.Tables(0).Rows.Count > 0) Then
                If Not Destination.UseGeneric Then
                    '4 GEt delivery location from Oracle if there was one, and overlay it.
                    If dloc.Tables(0).Rows.Count > 0 Then
                        With dsICS_data.Tables(0).Rows(0)
                            .Item("Ref_seq_nbr") = dloc.Tables(0).Rows(0).Item("First_Qual_Text")
                            .Item("Delivery_loc_nbr") = dloc.Tables(0).Rows(0).Item("PKG_11Z")
                            .Item("PKG_12Z_SEGM_NBR") = dloc.Tables(0).Rows(0).Item("PKG_12Z")
                            .Item("PKG_13Z_SEGM_NBR") = dloc.Tables(0).Rows(0).Item("PKG_13Z")
                            .Item("PKG_14Z_SEGM_NBR") = dloc.Tables(0).Rows(0).Item("PKG_14Z")
                            .Item("PKG_15Z_SEGM_NBR") = dloc.Tables(0).Rows(0).Item("PKG_15Z")
                            .Item("PKG_16Z_SEGM_NBR") = dloc.Tables(0).Rows(0).Item("PKG_16Z")
                            .Item("PKG_17Z_SEGM_NBR") = dloc.Tables(0).Rows(0).Item("PKG_17Z")
                        End With
                    Else
                        '9/12/05 this is added to prevent labeling without Dock data
                        Throw New Exception("Failure: Locating DLOC and Dock data in Mesdba.ICS_Delivery_Location")
                    End If
                    '5 todo  update label buffer
                    Try
                        If labelreq.ScheduleRequired And Destination.QtyPkgsperSkidReq > 1 And (Destination.Package_Code = "Z" Or Destination.Package_Code = "C") And (Not Destination.UseGeneric) Then
                            'save to buffer
                            sqli.Remove(0, sqli.Length)
                            sqli.Append("Insert into mesdba.ics_label_buffer (Serial_nbr, Schedule_ID, Machine_ID) ")
                            sqli.Append(" Values(" & dsICS_data.Tables(0).Rows(0).Item("Serial_Nbr"))
                            sqli.Append(", " & Destination.ScheduleID & ", " & Destination.mes_machine_id & ")")
                            With cmd
                                .Transaction = trans
                                .CommandText = sqli.ToString
                                .ExecuteNonQuery()
                                .CommandText = "Commit"
                                .ExecuteNonQuery()
                            End With
                        End If
                    Catch ex As Exception
                        If ConfigurationSettings.AppSettings.Get("LogEvents") = "True" Then
                            If LogEvents("Failure to Update ICS_LABEL_BUFFER in printlabel_p23" & ex.Message & Space(1) & ex.stacktrace) Then
                            End If
                        End If
                        'LogErrors(ex.Message, "PrintLabel_23", "Failure to Update ICS_LABEL_BUFFER")
                        Throw New Exception("Failure: Saving to ICS_LABEL_BUFFER :" & ex.Message)
                    End Try
                End If

                'Save the standard pack
                el = 305
                mesSaveICSStandardPack(labelreq, dsICS_data, cnOra)
                el = 307
                saveSerializedProduct(cnOra, labelreq.lStdPackID, labelreq.lmesPartID, labelreq.lmesMachID, labelreq.SerializedBECs, "PrintLabelP23")

                '7 format the ship label using the label template
                el = 308
                labelfile = FormatOutput(dsICS_data, False)

                'DSM: Add new serial numbers created to the label string if package code is '4C'
                If Destination.Package_Code = "4C" Then
                    labelfile &= NEW_SN_SEPARATOR & newSN_2 & NEW_SN_SEPARATOR & newSN_3 & NEW_SN_SEPARATOR & newSN_4
                End If

                '8 print skid summary if required. and append to the label file
                If labelreq.ScheduleRequired And Destination.QtyPkgsperSkidReq > 1 And Destination.Completed And ConfigurationSettings.AppSettings.Get("UseSkidSummary") = "True" And (Destination.Package_Code = "Z" Or Destination.Package_Code = "C") And (Not Destination.UseGeneric) Then
                    'Generate the skid summary and append to labelfile
                    Dim skidsum As String
                    el = 318
                    skidsum = GenerateSkidSummary(cnOra, cmd, Destination)
                    labelfile &= skidsum 'append the skid summ to the labelfile
                End If
            Else
                'could not locate label data in ics table
                Throw New Exception("Failure: ICS label data not available for: " & Destination.Part_nbr & " " & Destination.Revision_Physical & " " & Destination.Package_Code & " " & Destination.CNTR)
            End If
            trans.Commit()
            Return dsICS_data.Tables(0).Rows(0).Item("Serial_Nbr") & "</?Serial>" & labelfile 'this is the header, --containing the serial number

        Catch ex As Exception
            trans.Rollback()
            If ConfigurationSettings.AppSettings.Get("LogEvents") = "True" Then
                If LogEvents("Failure in PrintLabel_P23 " & ex.Message & Space(1) & ex.StackTrace) Then
                End If
            End If
            'LogErrors(ex.Message, "PrintLabel_P23", ex.Message)
            Try
                rets = GenerateErrorLabel(labelreq.part, labelreq.Machine, labelreq.PackageCode, labelreq.CNTR, "N/A", ex.Message, "Contact Local Support")
                Return ex.Message & Space(2) & el & "</?BWSError>" & rets
            Catch exx As Exception
            End Try

        Finally
            If Not cnOra Is Nothing AndAlso cnOra.State = ConnectionState.Open Then cnOra.Close()
            Try
                cnOra.Close()
                cnOra.Dispose()
            Catch ey As Exception
            End Try

            Try
                dloc.Dispose()
            Catch ey As Exception
            End Try

            Try
                dsICS_data.Dispose()
            Catch ey As Exception
            End Try

            Try
                cmd.Dispose()
            Catch ey As Exception
            End Try

            Try
                da.Dispose()
            Catch ey As Exception
            End Try
        End Try
    End Function
    <WebMethod(), SoapHeader("greeting")> _
    Public Function ViewErrorLog() As String

        If greeting Is Nothing Then
            Return "Please supply proper credentials"
        Else
            If greeting.UserID = ConfigurationSettings.AppSettings.Get("GreetingUserID") And greeting.Password = ConfigurationSettings.AppSettings.Get("GreetingPasscode") Then
                'youre in
            Else
                Return "Please supply proper credentials"
            End If
        End If

        Dim sr As StreamReader
        Dim sErrLogPath As String
        Dim retv As String
        On Error Resume Next
        sErrLogPath = ConfigurationSettings.AppSettings.Get("ErrorLogPath")
        retv = sr.ReadToEnd()
        sr.Close()
        Return retv
    End Function

    <WebMethod(), SoapHeader("greeting")> _
        Public Function saveSerializedProducts(ByVal SerialNbr As Long, ByVal mesMachineID As Long, ByVal SerializedList As String) As String
        GoTo skipgreeting
        If greeting Is Nothing Then
            Return "Please supply proper credentials"
        Else
            If greeting.UserID = ConfigurationSettings.AppSettings.Get("GreetingUserID") And greeting.Password = ConfigurationSettings.AppSettings.Get("GreetingPasscode") Then
                'youre in
            Else
                Return "Please supply proper credentials"
            End If
        End If
skipgreeting:
        Dim cnOra As New System.Data.OracleClient.OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim stdPk As Long
        Dim mespart As Long
        Dim da As OracleDataAdapter
        Dim dt As New DataTable
        Dim cmd As New OracleCommand

        Const caller = "BECSPL" 'this is used to control whether to use the transaction or not
        Try
            cnOra.Open()
            'need to get the standard pack and mes_part id for the call
            With cmd
                .CommandType = CommandType.Text
                .CommandText = "Select standard_pack_id, mes_part_ID from mesdba.Standard_Pack where serial_nbr = '" & SerialNbr & "'"
                .Connection = cnOra
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                stdPk = dt.Rows(0).Item("Standard_Pack_Id")
                mespart = dt.Rows(0).Item("mes_part_id")
            Else
                Throw New Exception("Failure in saveSerializedProducts --Standard Pack not found for SerialNbr: " & SerialNbr)
            End If
            saveSerializedProduct(cnOra, stdPk, mespart, mesMachineID, SerializedList, caller)
        Catch ex As Exception
            Throw New Exception("Failure in saveSerializedProduct: " & ex.Message)
        Finally
            cnOra.Close()
            cnOra.Dispose()
            dt.Dispose()
            da.Dispose()
        End Try

    End Function

    Private Sub saveSerializedProduct(ByRef cnOra As OracleConnection, ByVal StdPackID As Long, ByVal mesPartID As Long, ByVal mesMachineID As Long, ByVal SerializedList As String, ByVal Caller As String)
        'this is used to save the serialized product in the table and associate with it the standard_pack_id
        'This procedure assumes SerializedList will be a string delimited by semi-colon or unique serialized becIDS

        Dim sql As New StringBuilder
        Dim cmd As New OracleCommand
        Dim i As Int16, ub As Int16
        Dim ar() As String
        Dim ar1() As String
        Dim er As New StringBuilder

        Try
            If Len(Trim(SerializedList)) = 0 Then GoTo exithere 'empty list, don't bother with trying to save
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                cnOra.Open()
            End If
            cmd.Connection = cnOra
            If Caller = "BECSPL" Then
                'don't use the transaction
            Else
                'called by printlabel_p23, use the transaction for rollback
                cmd.Transaction = trans
            End If

            ar = Split(SerializedList, "^^") 'separates the serial,timestamp pairs for other serial,timestamp pairs

            Try
                ub = UBound(ar)
            Catch ex As Exception
                Throw New Exception("Failure in saveSerializedProduct: List of Serilized BECs is empty")
                GoTo exithere
            End Try
            For i = 0 To ub
                If Len(ar(i)) > 0 Then
                    ar1 = Split(ar(i), "^") 'used to separate between the serial and the timestamp
                    sql.Remove(0, sql.Length)
                    sql.Append("Insert into mesdba.serialized_product (standard_pack_id,serial_nbr,machine_id,mes_part_id,produced_tmstm) ")
                    sql.Append(" Values( ")
                    sql.Append(StdPackID & ",")
                    If ar1(0).Length > 22 Then   'AC added code to truncate possible overflow situation 8/13/2008
                        ar1(0) = Mid(ar1(0), 1, 22)  'AC 8/13/2008
                    End If                           'AC 8/13/2008
                    sql.Append("'" & ar1(0) & "',")
                    sql.Append(mesMachineID & ",")
                    sql.Append(mesPartID & ",")
                    sql.Append("TO_DATE('" & ar1(1) & "', 'MM/DD/YYYY hh:mi:ss am')")
                    sql.Append(")")
                    cmd.CommandText = sql.ToString
                    Try
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        er.Append("Failure Saving Serialized Product : " & ex.Message & sql.ToString & ";")
                    End Try
                End If
            Next i
            If Len(er.ToString) > 0 Then Throw New Exception(er.ToString)
exithere:
        Catch ex As Exception
            If ConfigurationSettings.AppSettings.Get("LogEvents") = "True" Then
                If LogEvents("Failure saving Serialized Product in saveSerializedProduct " & ex.Message & Space(1) & ex.StackTrace) Then
                End If
            End If
            'LogErrors(ex.Message, " saveSerializedProduct", "Failure saving Serialized Product: ")
            Throw New Exception("Failure saving Serialized Product: saveSerializedProduct " & ex.Message)
        Finally
            'because we pass in an open connection, we don't close it
            Try
                cmd.Dispose()
            Catch ey As Exception
            End Try
        End Try
    End Sub

    Private Sub LogErrors(ByVal sErrDesc As String, ByVal sFunctionName As String, ByVal sErrorData As String)

        ' local variables
        Dim iFileNum As Integer
        Dim sAppPath As String
        Dim sErrLogPath As String
        Dim sw As StreamWriter

        On Error Resume Next
        sErrLogPath = ConfigurationSettings.AppSettings.Get("ErrorLogPath")

        sw = New StreamWriter(sErrLogPath)
        sw.WriteLine(Now & ";" & sErrDesc & ";" & sFunctionName & ";" & sErrorData)
        sw.Close()

    End Sub

    <WebMethod()> _
    Public Function LogEvents(ByVal logthis As String) As Boolean
        Dim eventlog1 As New System.Diagnostics.EventLog

        Try
            If Not EventLog.SourceExists("BECWeb") Then
                EventLog.CreateEventSource("BECWeb", "Application")
            End If
            eventlog1.Source = "BECWeb"
            eventlog1.WriteEntry(logthis)
            Return True
        Catch ex As Exception
            'Return "Failure: " & ex.Message
        Finally
            eventlog1.Close()
            eventlog1.Dispose()
        End Try
    End Function

    <WebMethod()> _
    Public Function PrintSkidSummaryOnly(ByVal SkidID As Long, ByVal PartNbr As String, ByVal CNTR As String, ByVal MachineName As String) As String
        'this web method calls the private function GeneratSkidSummary.  One differnece is i must provide a connection
        Dim cnORa As New OracleConnection
        Dim cmd As New OracleCommand
        Dim retv As String
        Dim dest As New clsDestination

        Try
            cnORa = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
            cnORa.Open()
            With dest
                .ScheduleID = SkidID 'use this structure for compliance with current procedure design
                .Part_nbr = PartNbr
                .CNTR = CNTR
                .MachineName = MachineName
            End With
            With cmd
                .Connection = cnORa
            End With
            retv = GenerateSkidSummary(cnORa, cmd, dest)
            Return retv

        Catch ex As Exception
            Throw New Exception("Failure printing skid summary for Skid: " & SkidID)
        Finally
            cmd.Dispose()
            cnORa.Close()
            cnORa.Dispose()
        End Try
    End Function

    <WebMethod()> _
    Public Function ConvertToCustomerPartNumberttt(ByVal PartNumber As String) As String
        Dim oCommand As New OracleCommand
        Dim cnora As New OracleConnection

        Try
            cnora = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
            cnora.Open()

            With oCommand
                .Connection = cnora
                .Transaction = trans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "mitproc.gsd_read_prod_pkg.get_xref_part_proc"
                With .Parameters
                    .Add("P_PART_NBR", OracleType.VarChar, 8).Value = PartNumber
                    .Add("P_UNIQUE_FEATURE", OracleType.VarChar, 30).Value = ""
                    .Add("p_xref_busorg_svr_nbr", OracleType.VarChar, 10).Value = ""
                    .Add("p_xref_busorg_id", OracleType.VarChar, 10).Value = ""
                    .Add("p_xref_busorg_numbng_cd", OracleType.VarChar, 10).Value = ""
                    .Add("p_xref_part_nbr", OracleType.VarChar, 8)
                    .Add("p_xref_unique_feature", OracleType.VarChar, 30)
                    .Add("p_err_num", OracleType.Number)
                    .Add("p_err_msg", OracleType.VarChar, 100)
                    .Add("p_err_proc", OracleType.VarChar, 100)

                    Dim I As Integer
                    For I = 0 To 4
                        .Item(I).Direction = ParameterDirection.InputOutput
                    Next

                    For I = 5 To 9
                        .Item(I).Direction = ParameterDirection.Output
                    Next
                End With

                .ExecuteNonQuery()

                Return CStr(.Parameters("P_XREF_PART_NBR").Value)
            End With
        Catch ex As Exception
            Return "Failure in ConvertToCustomerPartNumber: " & ex.Message
        Finally
            cnora.Close()
            cnora.Dispose()
            oCommand.Dispose()
        End Try

    End Function

    Private Function GetCustomerPNfromICStable(ByVal DelphiPN As String, ByVal Container_code As String, ByRef cnOra As OracleConnection) As String

        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim sSQL As String
        Dim schema As String = GetSchemaName()
        Dim sContainerType As String

        Try
            If cnOra.State = ConnectionState.Closed Then
                cnOra.Open()
            End If
            If Trim(Container_code) = "" Then
                sSQL = "Select Part_nbr,Customer_Part_Nbr, Destination from Pltfloor.Ics_label_data_vw where Part_nbr = '" & DelphiPN & "' and Destination is null and rownum =1"
            ElseIf Container_code.ToUpper = "<ANY>" Then
                sSQL = "Select Part_nbr,Customer_Part_Nbr, Destination from Pltfloor.Ics_label_data_vw where Part_nbr = '" & DelphiPN & "' and rownum =1"
            Else
                sSQL = "Select Part_nbr,Customer_Part_Nbr, Destination from Plantfloor.Ics_label_data_vw where Part_nbr = '" & DelphiPN & "' and Destination = '" & Container_code & "' and rownum =1"
            End If
            With cmd
                .Transaction = trans
                .CommandType = CommandType.Text
                .CommandText = sSQL
                .Connection = cnOra
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Return ds.Tables(0).Rows(0).Item("Customer_Part_Nbr")
                Else
                    Return "Failure: Getting Customer Part Nbr -ICS Data Not Available"
                End If
            Else
                Return "Failure: Getting Customer Part Nbr"
            End If
        Catch ex As Exception
            Return "Failure: " & ex.Message
        Finally
            cmd.Dispose()
            da.Dispose()
            ds.Dispose()
        End Try

    End Function

    <WebMethod()> _
    Public Function PrintSkidSummary(ByVal SerialNbr As Long) As String
        'Using the current schedule ID, Generate the skid summary label
        Dim sqli As String
        Dim dtBuffTmp As New DataTable, dtSkidSum As New DataTable, dtStuff As New DataTable
        Dim da As OracleDataAdapter
        Dim dr As DataRow
        Dim sr As New StreamReader(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("SKIDSUMMARY"))
        Dim line As String
        Dim ostring As New StringBuilder
        Dim tmp As String, i As Int32
        Dim b As New StringBuilder
        Dim CustPN As String
        Dim partNbr As String
        Dim cmd As New OracleCommand
        Dim cntr As String
        Dim retv
        Dim dest As New clsDestination

        Dim cnora As OracleConnection

        Try
            cnora = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
            cnora.Open()
            With cmd
                .Connection = cnora

                'get the partnbr, and cntr
                sqli = "select sp.location_destination, mp.part_nbr from mesdba.standard_pack sp, mesdba.mes_part mp where sp.serial_nbr = '" & SerialNbr & "' and mp.mes_part_id = sp.mes_part_id"
                With cmd
                    .Connection = cnora
                    .CommandType = CommandType.Text
                    .CommandText = sqli
                    .ExecuteNonQuery()
                End With
                da = New OracleDataAdapter(cmd)
                da.Fill(dtStuff)
                If dtStuff.Rows.Count > 0 Then
                    dest.Part_nbr = dtStuff.Rows(0).Item("Part_Nbr")
                    dest.CNTR = dtStuff.Rows(0).Item("location_destination")
                Else
                    Throw New Exception("Failure finding standard pack for this serial number: " & SerialNbr)
                End If

                sqli = "Select lb.schedule_id, lb.machine_id from Mesdba.ICS_Label_Buffer lb where lb.Schedule_ID  in (Select l1.schedule_id from mesdba.ics_label_buffer l1 where l1.serial_nbr = '" & SerialNbr & "') order by mod_tmstm"
                With cmd
                    .CommandText = sqli
                    .ExecuteNonQuery()
                End With
                dtStuff.Clear()
                da.Fill(dtStuff)
                If dtStuff.Rows.Count > 0 Then
                    dest.ScheduleID = dtStuff.Rows(0).Item("Schedule_ID")
                    dest.MachineName = dtStuff.Rows(0).Item("Machine_ID")
                End If

                retv = GenerateSkidSummary(cnora, cmd, dest)
                retv = Replace(retv, Chr(0), "")
                Return retv
            End With

        Catch ex As Exception
            Throw New Exception("Failure in PrintSkidSummary: " & ex.Message)
        Finally
            cnora.Close()
            cnora.Dispose()
            cmd.Dispose()
            dtBuffTmp.Dispose()
            dtSkidSum.Dispose()
            dtStuff.Dispose()
        End Try
    End Function

    Private Function GenerateSkidSummary(ByRef cnOra As OracleConnection, ByRef cmd As OracleCommand, ByRef destination As clsDestination) As String
        'Using the current schedule ID, Generate the skid summary label
        Dim sqli As New StringBuilder
        Dim dtBuffTmp As New DataTable, dtSkidSum As New DataTable
        Dim da As OracleDataAdapter
        Dim dr As DataRow

        Dim sr As New StreamReader(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("SKIDSUMMARY"))
        Dim line As String
        Dim ostring As New StringBuilder
        Dim tmp As String, i As Int32
        Dim b As New StringBuilder
        Dim CustPN As String


        Try
            sqli.Append("Select * from Mesdba.ICS_Label_Buffer where Schedule_ID = " & destination.ScheduleID & " and rownum <=12 order by Serial_Nbr") 'order by mod_tmstm instead of insert_tmstm so we can change the order if a problem occurs
            With cmd
                .CommandType = CommandType.Text
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(dtSkidSum)

            'add columns in the temp buffer which will be used to replace what is on the template with inputs
            dtBuffTmp.Columns.Add("CNTR", GetType(System.String))
            dtBuffTmp.Columns.Add("LINE", GetType(System.String))
            dtBuffTmp.Columns.Add("SKIDID", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR1", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR2", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR3", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR4", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR5", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR6", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR7", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR8", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR9", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR10", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR11", GetType(System.String))
            dtBuffTmp.Columns.Add("SERNBR12", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE1", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE2", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE3", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE4", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE5", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE6", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE7", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE8", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE9", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE10", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE11", GetType(System.String))
            dtBuffTmp.Columns.Add("END_DATE12", GetType(System.String))
            dtBuffTmp.Columns.Add("SHIP_DATE", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR1", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR2", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR3", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR4", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR5", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR6", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR7", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR8", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR9", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR10", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR11", GetType(System.String))
            dtBuffTmp.Columns.Add("PNBR12", GetType(System.String))
            dr = dtBuffTmp.NewRow

            With dr
                .Item("CNTR") = destination.CNTR
                .Item("Line") = destination.MachineName
                .Item("SKIDID") = destination.ScheduleID
                CustPN = GetCustomerPNfromICStable(destination.Part_nbr, "<ANY>", cnOra)
                For i = 0 To dtSkidSum.Rows.Count - 1
                    .Item("SERNBR" & i + 1) = dtSkidSum.Rows(i).Item("Serial_Nbr")
                    .Item("END_DATE" & i + 1) = dtSkidSum.Rows(i).Item("mod_tmstm")
                    .Item("PNBR" & i + 1) = CustPN
                Next i
                dtBuffTmp.Rows.Add(dr)
            End With

            Do
                line = sr.ReadLine()
                ostring.Append(line)
            Loop Until line Is Nothing

            For i = 0 To dtBuffTmp.Columns.Count - 1
                b.Remove(0, b.Length)
                b.Append("%" & dtBuffTmp.Columns(i).ColumnName & "%")
                ostring.Replace(b.ToString, dtBuffTmp.Rows(0).Item(i) & "")
            Next i
            sr.Close()
            ostring.Replace(Chr(0), "") 'take out any nulls 
            Return ostring.ToString

        Catch ex As Exception
            Throw New Exception("Failure in GenerateSkidSummary: " & ex.Message)
        End Try
    End Function

    Private Sub DecodeMessage(ByVal dtInfo As DataTable, ByRef Labelreq As requestpacket)
        With Labelreq
            .lblType = dtInfo.Rows(0).Item("Label Type")
            .part = dtInfo.Rows(0).Item("Part Number")
            .Machine = dtInfo.Rows(0).Item("Machine Name")
            .Quantity = dtInfo.Rows(0).Item("Quantity")
            .OperID = dtInfo.Rows(0).Item("Operator ID")
            .Revision = dtInfo.Rows(0).Item("ECL")
            .PackageCode = dtInfo.Rows(0).Item("Package Code")
            .ScheduleRequired = dtInfo.Rows(0).Item("ScheduleRequired")
            .Department = dtInfo.Rows(0).Item("Department")
            .SerializedBECs = dtInfo.Rows(0).Item("SerializedBECs") & ""
            .ProdRun = dtInfo.Rows(0).Item("ProdRunID")
        End With
    End Sub

    <WebMethod()> _
    Public Function RequiresSkid(ByVal Plant As String, ByVal LineID As String, ByVal PKG_Code As String) As String
        'plant 23 uses a skid of individual labeled totes which need to be tied to the same 
        'destination.  This is what the skid logic is for.
        Select Case Plant
            Case 23
                Select Case LineID
                    Case 1, 2, 3, 4
                        Return False
                    Case Else
                        Select Case PKG_Code
                            Case "Z", "C"
                                Return True
                            Case Else
                                Return False
                        End Select
                End Select
            Case Else
                Return False
        End Select
    End Function

    <WebMethod()> _
    Public Function GetShipDestination_Webmethod(ByRef Destination As clsDestination) As String
        Dim retv As String

        Dim cn As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Try
            If Destination.UseGeneric Then
                retv = ReserveSerials(Destination, 1)
                If InStr(retv, "Failure") <> 0 Then
                    'failed to reserve a serial
                    Return "Failure reserving serial for Generic label as specified in production run.  Call PC&L"
                Else
                    'since directly calling reserveserials returns a serial, put the prefix that calling the schedule will put
                    retv = "Serial:" & retv
                End If
            Else
                cn.Open()
                retv = GetShipDestination_withAutoGeneric(Destination, cn)
            End If
            Return retv
        Catch ex As Exception
            Return "Failure in GetShipDestination_Webmethod: =>" & ex.Message
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Function

    Public Function GetShipDestination(ByRef Destination As clsDestination, ByRef moCN As OracleConnection) As String
        'This function will check the Oracle structure ICS_Schedule_Pool for the schedule entrie belonging to the mes_machine_id.
        'Based on the ControlFlag parameter it will do addition options such as assigne the machine_Id to a row or increment packages completed
        'When this schedule item is completed, it will return completed as true

        Dim ds As New DataSet
        Dim cmd As New OracleCommand
        Dim da As New OracleDataAdapter

        Dim sqli As New StringBuilder
        Dim schema As String = GetSchemaName()
        Dim schedId As Long
        Dim MachID As Long
        Dim numlab As Integer

        Try
            If moCN.State = System.Data.ConnectionState.Closed Then
                moCN.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                moCN.Open()
            Else
                'if the connection is passed in, we will use the transaction
                With cmd
                    .Connection = moCN
                    .Transaction = trans
                    .CommandType = CommandType.Text
                End With
            End If
            Destination.mes_machine_id = ConvertFromMachineNbrtoMesMachineID(Destination.MachineName, moCN)
            Select Case Destination.ControlFlag.ToUpper
                Case "PEEKONLY"
                    sqli.Append("Select sp.*, mp.part_nbr from " & schema & "ics_schedule_pool sp," & schema & "mes_part mp ")
                    sqli.Append(" where (sp.machine_id = " & Destination.mes_machine_id & ") and mp.mes_part_id = sp.mes_part_id")
                    With cmd
                        .CommandText = sqli.ToString
                        .ExecuteNonQuery()
                    End With
                    da = New OracleDataAdapter(cmd)
                    'fill the dataset
                    da.Fill(ds)

                    With ds.Tables(0)
                        If .Rows.Count > 0 Then
                            Destination.mes_part_id = .Rows(0).Item("mes_part_id")
                            Destination.Part_nbr = .Rows(0).Item("Part_Nbr")
                            If IsDBNull(.Rows(0).Item("PKGS_PER_SKID_QTY")) Then
                                Destination.QtyPkgsperSkidReq = 0
                            Else
                                Destination.QtyPkgsperSkidReq = .Rows(0).Item("PKGS_PER_SKID_QTY")
                            End If
                            If IsDBNull(.Rows(0).Item("Pkgs_Used_Qty")) Then
                                Destination.QtyPkgsUsed = 0
                            Else
                                Destination.QtyPkgsUsed = .Rows(0).Item("Pkgs_Used_Qty")
                            End If
                            Destination.ScheduleID = .Rows(0).Item("Schedule_Id")
                            Destination.CNTR = .Rows(0).Item("Container_code") & ""
                            Destination.MatID = .Rows(0).Item("MatId") & ""
                            Destination.Part_nbr = .Rows(0).Item("Part_Nbr") & ""
                            GetShipDestination = .Rows(0).Item("Container_code") & ""
                        Else
                            Return "Failure: getting ShipDestination"
                        End If
                    End With

                Case "RESERVE"
                    'see if one is already reserved first.
                    sqli.Append("Select sp.*, mp.part_nbr from " & schema & "ics_schedule_pool sp," & schema & "mes_part mp ")
                    sqli.Append(" where (sp.machine_id = " & Destination.mes_machine_id & ") and mp.mes_part_id = sp.mes_part_id")
                    sqli.Append(" and sp.mes_part_id = ")
                    sqli.Append(Destination.mes_part_id)
                    sqli.Append(" and sp.Package_Code = '" & Destination.Package_Code & "'")

                    With cmd
                        .CommandText = sqli.ToString
                        .ExecuteNonQuery()
                    End With
                    da = New OracleDataAdapter(cmd)
                    'fill the dataset
                    da.Fill(ds)

                    With ds.Tables(0)
                        If .Rows.Count > 0 Then
                            Destination.mes_part_id = .Rows(0).Item("mes_part_id")
                            Destination.Part_nbr = .Rows(0).Item("Part_Nbr")
                            If IsDBNull(.Rows(0).Item("PKGS_PER_SKID_QTY")) Then
                                Destination.QtyPkgsperSkidReq = 0
                            Else
                                Destination.QtyPkgsperSkidReq = .Rows(0).Item("PKGS_PER_SKID_QTY")
                            End If
                            If IsDBNull(.Rows(0).Item("Pkgs_Used_Qty")) Then
                                Destination.QtyPkgsUsed = 0
                            Else
                                Destination.QtyPkgsUsed = .Rows(0).Item("Pkgs_Used_Qty")
                            End If
                            Destination.ScheduleID = .Rows(0).Item("Schedule_Id")
                            Destination.CNTR = .Rows(0).Item("Container_code") & ""
                            Destination.MatID = .Rows(0).Item("MatId") & ""
                            Destination.Part_nbr = .Rows(0).Item("Part_Nbr") & ""
                            GetShipDestination = .Rows(0).Item("Container_code") & ""
                        Else
                            'delete any that are have been left hanging for this machine, from different passes
                            sqli.Remove(0, sqli.Length)
                            sqli.Append("Delete from mesdba.ics_schedule_pool where Machine_ID = ")
                            sqli.Append(Destination.mes_machine_id)
                            With cmd
                                .CommandText = sqli.ToString
                                .ExecuteNonQuery()
                                .CommandText = "Commit"
                                .ExecuteNonQuery()
                            End With

                            'need to reserve one
                            '8/18/05  PAW modified this because it was pulling SL for restricted lines.
                            sqli.Remove(0, sqli.Length)
                            sqli.Append("update " & schema & "ics_schedule_pool sa set sa.MACHINE_ID = " & Destination.mes_machine_id & ", sa.PKGS_USED_QTY =0 ")
                            sqli.Append(" where sa.schedule_id in (select schedule_id from (Select sp.Schedule_ID from " & schema & "ICs_schedule_pool sp where ")
                            sqli.Append(" (sp.mes_part_id = " & Destination.mes_part_id & ") and (sp.REVISION_PHYSICAL = '" & Destination.Revision_Physical & "')")
                            sqli.Append(" and (sp.package_code = '" & Destination.Package_Code & "') and (sp.MACHINE_ID is null)")
                            sqli.Append(" and (" & Destination.mes_machine_id & " || ':' || sp.mes_part_Id || ':' || sp.CONTAINER_CODE) not in")
                            sqli.Append(" (select r1.MACHINE_ID || ':' || r1.mes_part_Id || ':' || r1.CONTAINER_CODE as f1 from " & schema & "part_machine_restriction r1)")
                            sqli.Append(" order by sp.build_priority, sp.SCHEDULE_ID) where rownum =1)")
                            With cmd
                                .CommandText = sqli.ToString
                                .ExecuteNonQuery()
                                .CommandText = "Commit"
                                .ExecuteNonQuery()
                            End With

                            'try once more to get the schedule_Id
                            sqli.Remove(0, sqli.Length)
                            sqli.Append("Select sp.*, mp.part_nbr from " & schema & "ics_schedule_pool sp," & schema & "mes_part mp ")
                            sqli.Append(" where (sp.machine_id = " & Destination.mes_machine_id & ") and mp.mes_part_id = sp.mes_part_id")
                            sqli.Append(" and sp.mes_part_id = ")
                            sqli.Append(Destination.mes_part_id)
                            sqli.Append(" and sp.Package_Code = '" & Destination.Package_Code & "'")

                            cmd.CommandText = sqli.ToString

                            ds.Clear()
                            cmd.ExecuteNonQuery()
                            da.Fill(ds)
                            If ds.Tables(0).Rows.Count > 0 Then
                                With ds.Tables(0)
                                    Destination.mes_part_id = .Rows(0).Item("mes_part_id")
                                    Destination.Part_nbr = .Rows(0).Item("Part_Nbr")
                                    If IsDBNull(.Rows(0).Item("PKGS_PER_SKID_QTY")) Then
                                        Destination.QtyPkgsperSkidReq = 0
                                    Else
                                        Destination.QtyPkgsperSkidReq = .Rows(0).Item("PKGS_PER_SKID_QTY")
                                    End If

                                    Destination.QtyPkgsUsed = .Rows(0).Item("PKGS_USED_QTY")
                                    Destination.ScheduleID = .Rows(0).Item("Schedule_Id")
                                    Destination.CNTR = .Rows(0).Item("CONTAINER_CODE") & ""
                                    Destination.MatID = .Rows(0).Item("MatId") & ""
                                End With
                                If IsDBNull(.Rows(0).Item("Pkgs_Used_Qty")) Then
                                    Destination.QtyPkgsUsed = 0
                                Else
                                    Destination.QtyPkgsUsed = .Rows(0).Item("Pkgs_Used_Qty")
                                End If
                                Destination.CNTR = .Rows(0).Item("Container_code") & ""
                                GetShipDestination = .Rows(0).Item("Container_code") & ""
                            End If
                        End If
                    End With

                Case "WILLPRINT"
                    sqli.Append("Select sp.*, mp.part_nbr from " & schema & "ics_schedule_pool sp, " & schema & "mes_part mp where sp.MACHINE_ID = " & Destination.mes_machine_id & " and (sp.mes_part_id = " & Destination.mes_part_id & ") and (sp.Package_Code = '" & Destination.Package_Code & "') and (sp.Pkgs_Per_Skid_qty > sp.Pkgs_used_Qty) and sp.mes_part_id = mp.mes_part_id")
                    With cmd
                        .CommandText = sqli.ToString
                        .ExecuteNonQuery()
                    End With
                    da = New OracleDataAdapter(cmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        With ds.Tables(0)
                            Destination.mes_part_id = .Rows(0).Item("mes_part_id")
                            Destination.Part_nbr = .Rows(0).Item("Part_Nbr")
                            If IsDBNull(.Rows(0).Item("PKGS_PER_SKID_QTY")) Then
                                Destination.QtyPkgsperSkidReq = 0
                            Else
                                Destination.QtyPkgsperSkidReq = .Rows(0).Item("PKGS_PER_SKID_QTY")
                            End If
                            If IsDBNull(.Rows(0).Item("Pkgs_Used_Qty")) Then
                                Destination.QtyPkgsUsed = 1 'return the incremented value
                            Else
                                Destination.QtyPkgsUsed = .Rows(0).Item("Pkgs_Used_Qty") + 1 'return the incremented value
                            End If
                            Destination.ScheduleID = .Rows(0).Item("Schedule_Id")
                            Destination.CNTR = .Rows(0).Item("CONTAINER_CODE") & ""
                            Destination.MatID = .Rows(0).Item("MatId") & ""

                            Try
                                numlab = Val(ConfigurationSettings.AppSettings.Get("PkgCode_" & Destination.Package_Code))
                            Catch
                                numlab = 1
                            End Try
                            sqli.Remove(0, sqli.Length)
                            If Destination.QtyPkgsUsed >= Destination.QtyPkgsperSkidReq Or (numlab > 1) Then
                                'this numlab > 1 is to catch master/child relationship labels such as 6C or 4C so we don't treat them like single skids, but buffer them locally
                                '6/31/05  PAW  -This is commented because we will not roll schedule entries once complete.  We will delete them.
                                '8/17/05  PAW  -This is being put back in because they want to roll schedule entries, not delete them
                                sqli.Remove(0, sqli.Length)
                                sqli.Append("select " & schema & "ics_schedule_pool_seq.nextval from dual")
                                cmd.CommandText = sqli.ToString
                                ds.Clear()
                                da.Fill(ds)
                                If ds.Tables(0).Rows.Count > 0 Then
                                    sqli.Remove(0, sqli.Length)
                                    sqli.Append("Update " & schema & "Ics_Schedule_Pool sp set sp.Schedule_Id = " & .Rows(0).Item("NextVal") & ", sp.Pkgs_Used_Qty = 0, sp.MACHINE_ID = null, sp.build_priority = 99")
                                    sqli.Append(" Where sp.schedule_id = " & Destination.ScheduleID)
                                    With cmd
                                        .CommandText = sqli.ToString
                                        .ExecuteNonQuery()
                                        .CommandText = "Commit"
                                        .ExecuteNonQuery()
                                    End With
                                End If
                                Destination.Completed = True
                            Else
                                'increment the pkgsused
                                sqli.Remove(0, sqli.Length)
                                sqli.Append("Update " & schema & "ICs_Schedule_Pool set Pkgs_Used_Qty = " & Destination.QtyPkgsUsed)
                                sqli.Append(" Where schedule_id = " & Destination.ScheduleID)
                                With cmd
                                    .CommandText = sqli.ToString
                                    .ExecuteNonQuery()
                                    .CommandText = "Commit"
                                    .ExecuteNonQuery()
                                End With
                                Destination.Completed = False
                            End If
                            Return Destination.CNTR & ""
                        End With
                    Else 'need to reserve a schedule entry for this machine
                        With ds.Tables(0)
                            'not reserved, reserve one
                            'first when reserving one, delete all others for this line, incase they switched packages in the middle of a skid
                            sqli.Remove(0, sqli.Length)
                            sqli.Append("Delete from mesdba.ics_schedule_pool where Machine_ID = ")
                            sqli.Append(Destination.mes_machine_id)
                            With cmd
                                .CommandText = sqli.ToString
                                .ExecuteNonQuery()
                                .CommandText = "Commit"
                                .ExecuteNonQuery()
                            End With

                            '8/18/05  PAW modified this because it was pulling SL for restricted lines.
                            sqli.Remove(0, sqli.Length)
                            sqli.Append("update " & schema & "ics_schedule_pool sa set sa.MACHINE_ID = " & Destination.mes_machine_id & ", sa.PKGS_USED_QTY =1 ")
                            sqli.Append(" where sa.schedule_id in (select schedule_id from (Select sp.Schedule_ID from " & schema & "ICs_schedule_pool sp where ")
                            sqli.Append(" (sp.mes_part_id = " & Destination.mes_part_id & ") and (sp.REVISION_PHYSICAL = '" & Destination.Revision_Physical & "')")
                            sqli.Append(" and (sp.package_code = '" & Destination.Package_Code & "') and (sp.MACHINE_ID is null)")
                            sqli.Append(" and (" & Destination.mes_machine_id & " || ':' || sp.mes_part_Id || ':' || sp.CONTAINER_CODE) not in")
                            sqli.Append(" (select r1.MACHINE_ID || ':' || r1.mes_part_Id || ':' || r1.CONTAINER_CODE as f1 from " & schema & "part_machine_restriction r1)")
                            sqli.Append(" order by sp.build_priority, sp.SCHEDULE_ID) where rownum =1)")
                            With cmd
                                .CommandText = sqli.ToString
                                .ExecuteNonQuery()
                                .CommandText = "Commit"
                                .ExecuteNonQuery()
                            End With

                            sqli.Remove(0, sqli.Length)
                            sqli.Append("Select sp.*, mp.part_nbr from " & schema & "ics_schedule_pool sp, " & schema & "mes_part mp where sp.MACHINE_ID = " & Destination.mes_machine_id & " and sp.mes_part_id = mp.mes_part_id")
                            'try once more to get the schedule_Id
                            cmd.CommandText = sqli.ToString
                            ds.Clear()
                            cmd.ExecuteNonQuery()
                            da.Fill(ds)
                            If ds.Tables(0).Rows.Count > 0 Then
                                With ds.Tables(0)
                                    Destination.mes_part_id = .Rows(0).Item("mes_part_id")
                                    Destination.Part_nbr = .Rows(0).Item("Part_Nbr")
                                    If IsDBNull(.Rows(0).Item("PKGS_PER_SKID_QTY")) Then
                                        Destination.QtyPkgsperSkidReq = 0
                                    Else
                                        Destination.QtyPkgsperSkidReq = .Rows(0).Item("PKGS_PER_SKID_QTY")
                                    End If

                                    Destination.QtyPkgsUsed = 1
                                    Destination.ScheduleID = .Rows(0).Item("Schedule_Id")
                                    Destination.CNTR = .Rows(0).Item("CONTAINER_CODE") & ""
                                    Destination.MatID = .Rows(0).Item("MatId") & ""
                                End With

                                Try
                                    numlab = Val(ConfigurationSettings.AppSettings.Get("PkgCode_" & Destination.Package_Code))
                                Catch
                                    numlab = 1
                                End Try
                                If Destination.QtyPkgsUsed >= Destination.QtyPkgsperSkidReq Or (numlab > 1) Then
                                    'if pkgsused >= required, then roll it
                                    'this record only required one package, go ahead and roll to bottom
                                    '8/17/05  Put the roll back in instead of the delete
                                    sqli.Remove(0, sqli.Length)
                                    sqli.Append("select " & schema & "ics_schedule_pool_seq.nextval from dual")
                                    cmd.CommandText = sqli.ToString
                                    ds.Clear()
                                    da.Fill(ds)
                                    If ds.Tables(0).Rows.Count > 0 Then
                                        sqli.Remove(0, sqli.Length)
                                        sqli.Append("Update " & schema & "Ics_Schedule_Pool sp set sp.Schedule_Id = " & .Rows(0).Item("NextVal") & ", sp.Pkgs_Used_Qty = 0, sp.MACHINE_ID = null, sp.build_priority = 99")
                                        sqli.Append(" Where sp.schedule_id = " & Destination.ScheduleID)
                                        With cmd
                                            .CommandText = sqli.ToString
                                            .ExecuteNonQuery()
                                            .CommandText = "Commit"
                                            .ExecuteNonQuery()
                                        End With
                                    End If
                                    Destination.Completed = True
                                Else
                                    Destination.Completed = False
                                End If
                                Return Destination.CNTR & ""
                            Else
                                'not found
                                Destination.Completed = False
                                Return "Failure Getting ShipDestination -not scheduled"
                            End If
                        End With
                    End If
            End Select

        Catch ex As Exception
            Throw New Exception("Failure in GetShipDestination: " & ex.Message)
        Finally
            'because we pass in an open connection, we don't close it
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try

    End Function

    Public Function GetShipDestination_withAutoGeneric(ByRef Destination As clsDestination, ByRef CN As OracleConnection) As String
        'This function will check the Oracle structure ICS_Schedule_Pool for the schedule entrie belonging to the mes_machine_id.
        'Based on the ControlFlag parameter it will do addition options such as assigne the machine_Id to a row or increment packages completed
        'When this schedule item is completed, it will return completed as true
        'For the AutoGeneric logic, this function must return to the caller, if it switched to Generic so the correct template can be use.
        'It will return through its name "Serial:##########" or "MachineName:ScheduleID-QtyPackagesPrinted" 
        'this will be used to indicate to the caller, how to lookup the ics label

        Dim ds As New DataSet
        Dim cmd As New OracleCommand
        Dim da As OracleDataAdapter

        Dim sqli As New StringBuilder
        Dim schema As String = GetSchemaName()
        Dim schedId As Long
        Dim MachID As Long
        Dim numlab As Integer
        Dim rets As String

        Try
            If CN.State = System.Data.ConnectionState.Closed Then
                CN.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                CN.Open()
            Else
                'if the connection is passed in, we will use the transaction
                With cmd
                    .Connection = CN
                    .Transaction = trans
                    .CommandType = CommandType.Text
                End With
            End If
            Destination.mes_machine_id = ConvertFromMachineNbrtoMesMachineID(Destination.MachineName, CN)
            Select Case Destination.ControlFlag.ToUpper
                Case "PEEKONLY"
                    sqli.Append("Select sp.*, mp.part_nbr from " & schema & "ics_schedule_pool sp," & schema & "mes_part mp ")
                    sqli.Append(" where (sp.machine_id = " & Destination.mes_machine_id & ") and mp.mes_part_id = sp.mes_part_id")
                    With cmd
                        .CommandText = sqli.ToString
                        .ExecuteNonQuery()
                    End With
                    da = New OracleDataAdapter(cmd)
                    'fill the dataset
                    da.Fill(ds)

                    With ds.Tables(0)
                        If .Rows.Count > 0 Then
                            Destination.mes_part_id = .Rows(0).Item("mes_part_id")
                            Destination.Part_nbr = .Rows(0).Item("Part_Nbr")
                            If IsDBNull(.Rows(0).Item("PKGS_PER_SKID_QTY")) Then
                                Destination.QtyPkgsperSkidReq = 0
                            Else
                                Destination.QtyPkgsperSkidReq = .Rows(0).Item("PKGS_PER_SKID_QTY")
                            End If
                            If IsDBNull(.Rows(0).Item("Pkgs_Used_Qty")) Then
                                Destination.QtyPkgsUsed = 0
                            Else
                                Destination.QtyPkgsUsed = .Rows(0).Item("Pkgs_Used_Qty")
                            End If
                            Destination.ScheduleID = .Rows(0).Item("Schedule_Id")
                            Destination.CNTR = .Rows(0).Item("Container_code") & ""
                            Destination.MatID = .Rows(0).Item("MatId") & ""
                            Destination.Part_nbr = .Rows(0).Item("Part_Nbr") & ""
                            Return "MachineName:" & Destination.ScheduleID & "-" & Destination.QtyPkgsUsed
                        Else
                            Return "Failure: ShipDestination not found for this machine"
                        End If
                    End With

                Case "RESERVE"
                    'see if one is already reserved first.
                    sqli.Append("Select sp.*, mp.part_nbr from " & schema & "ics_schedule_pool sp," & schema & "mes_part mp ")
                    sqli.Append(" where (sp.machine_id = " & Destination.mes_machine_id & ") and mp.mes_part_id = sp.mes_part_id")
                    sqli.Append(" and sp.mes_part_id = ")
                    sqli.Append(Destination.mes_part_id)
                    sqli.Append(" and sp.Package_Code = '" & Destination.Package_Code & "'")

                    With cmd
                        .CommandText = sqli.ToString
                        .ExecuteNonQuery()
                    End With
                    da = New OracleDataAdapter(cmd)
                    'fill the dataset
                    da.Fill(ds)

                    With ds.Tables(0)
                        If .Rows.Count > 0 Then
                            Destination.mes_part_id = .Rows(0).Item("mes_part_id")
                            Destination.Part_nbr = .Rows(0).Item("Part_Nbr")
                            If IsDBNull(.Rows(0).Item("PKGS_PER_SKID_QTY")) Then
                                Destination.QtyPkgsperSkidReq = 0
                            Else
                                Destination.QtyPkgsperSkidReq = .Rows(0).Item("PKGS_PER_SKID_QTY")
                            End If
                            If IsDBNull(.Rows(0).Item("Pkgs_Used_Qty")) Then
                                Destination.QtyPkgsUsed = 0
                            Else
                                Destination.QtyPkgsUsed = .Rows(0).Item("Pkgs_Used_Qty")
                            End If
                            Destination.ScheduleID = .Rows(0).Item("Schedule_Id")
                            Destination.CNTR = .Rows(0).Item("Container_code") & ""
                            Destination.MatID = .Rows(0).Item("MatId") & ""
                            Destination.Part_nbr = .Rows(0).Item("Part_Nbr") & ""
                            Return "MachineName:" & Destination.ScheduleID & "-" & Destination.QtyPkgsUsed + 1
                        Else
                            'call ReservemyNext...
                            rets = ReserveMyNextLabels(Destination, CN, "Reserve")
                            If InStr(rets, "Failure") <> 0 Then
                                Return "Failure in GetShipDestination_withAutoGeneric: " & rets
                            Else
                                'this will contain "Serial:##########" 
                                Return rets
                            End If
                        End If
                    End With

                Case "WILLPRINT"
                    sqli.Append("Select sp.*, mp.part_nbr from " & schema & "ics_schedule_pool sp, " & schema & "mes_part mp where sp.MACHINE_ID = " & Destination.mes_machine_id & " and (sp.mes_part_id = " & Destination.mes_part_id & ") and (sp.Package_Code = '" & Destination.Package_Code & "') and (sp.Pkgs_Per_Skid_qty > sp.Pkgs_used_Qty) and sp.mes_part_id = mp.mes_part_id")
                    With cmd
                        .CommandText = sqli.ToString
                        .ExecuteNonQuery()
                    End With
                    da = New OracleDataAdapter(cmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        With ds.Tables(0)
                            Destination.mes_part_id = .Rows(0).Item("mes_part_id")
                            Destination.Part_nbr = .Rows(0).Item("Part_Nbr")
                            If IsDBNull(.Rows(0).Item("PKGS_PER_SKID_QTY")) Then
                                Destination.QtyPkgsperSkidReq = 0
                            Else
                                Destination.QtyPkgsperSkidReq = .Rows(0).Item("PKGS_PER_SKID_QTY")
                            End If
                            If IsDBNull(.Rows(0).Item("Pkgs_Used_Qty")) Then
                                Destination.QtyPkgsUsed = 1 'return the incremented value
                            Else
                                Destination.QtyPkgsUsed = .Rows(0).Item("Pkgs_Used_Qty") + 1 'return the incremented value
                            End If
                            Destination.ScheduleID = .Rows(0).Item("Schedule_Id")
                            Destination.CNTR = .Rows(0).Item("CONTAINER_CODE") & ""
                            Destination.MatID = .Rows(0).Item("MatId") & ""

                            'update the schedule, either increment packages used or roll the schedule entry to bottom
                            Try
                                numlab = Val(ConfigurationSettings.AppSettings.Get("PkgCode_" & Destination.Package_Code))
                            Catch
                                numlab = 1
                            End Try
                            sqli.Remove(0, sqli.Length)
                            If Destination.QtyPkgsUsed >= Destination.QtyPkgsperSkidReq Or (numlab > 1) Then
                                'this numlab > 1 is to catch master/child relationship labels such as 6C or 4C so we don't treat them like single skids, but buffer them locally
                                '6/31/05  PAW  -This is commented because we will not roll schedule entries once complete.  We will delete them.
                                '8/17/05  PAW  -This is being put back in because they want to roll schedule entries, not delete them
                                sqli.Remove(0, sqli.Length)
                                sqli.Append("select " & schema & "ics_schedule_pool_seq.nextval from dual")
                                cmd.CommandText = sqli.ToString
                                ds.Clear()
                                da.Fill(ds)
                                If ds.Tables(0).Rows.Count > 0 Then
                                    sqli.Remove(0, sqli.Length)
                                    sqli.Append("Update " & schema & "Ics_Schedule_Pool sp set sp.Schedule_Id = " & .Rows(0).Item("NextVal") & ", sp.Pkgs_Used_Qty = 0, sp.MACHINE_ID = null, sp.build_priority = 99")
                                    sqli.Append(" Where sp.schedule_id = " & Destination.ScheduleID)
                                    With cmd
                                        .CommandText = sqli.ToString
                                        .ExecuteNonQuery()
                                        .CommandText = "Commit"
                                        .ExecuteNonQuery()
                                    End With
                                End If
                                Destination.Completed = True
                            Else
                                'increment the pkgsused
                                sqli.Remove(0, sqli.Length)
                                sqli.Append("Update " & schema & "ICs_Schedule_Pool set Pkgs_Used_Qty = " & Destination.QtyPkgsUsed)
                                sqli.Append(" Where schedule_id = " & Destination.ScheduleID)
                                With cmd
                                    .CommandText = sqli.ToString
                                    .ExecuteNonQuery()
                                    .CommandText = "Commit"
                                    .ExecuteNonQuery()
                                End With
                                Destination.Completed = False
                            End If
                            'end of update schedule

                            Return "MachineName:" & Destination.ScheduleID & "-" & Destination.QtyPkgsUsed
                        End With
                    Else
                        'need to reserve a schedule entry for this machine
                        rets = ReserveMyNextLabels(Destination, CN, "WillPrint")
                        If InStr(rets, "Failure") <> 0 Then
                            'failed to reserve labels, meaning we basically can't print at this time
                            Return "Failure in GetShipDestination: " & rets
                        Else
                            'this will contain "Serial:##########" 
                            Return rets
                        End If
                    End If
            End Select

        Catch ex As Exception
            Throw New Exception("Failure in GetShipDestination_withAutoGeneric: " & ex.Message)
        Finally
            'because we pass in an open connection, we don't close it
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try

    End Function

    Private Function ReserveMyNextLabels(ByRef Destination As clsDestination, ByRef CN As OracleConnection, ByVal ControlFlag As String)
        'This function will be called from GetShipDestination.  It will look at the schedule and determine what is next to run.
        'It will do this by checking the schedule, getting the schedule entry, looking for labels, enought to print this schedule item
        'and return that information to the caller.
        'As it looks for the schedule record, when it finds one for which there are not labels abailable, it will remove those schedule entries
        'and email the warning to PCL.  By removing them from the schedule, it ensures that these schedule entries don't continue to clog up future
        'request, with the assumption that PCL will put them back when the label issue is addressed.
        'this function will reserve the serial numbers required to complete, but putting the schedule_id into the machine-name
        'In cases where there are multiple labels --Skids-- it will save the scheudleId along with the 1-12 index of this label in the set.
        'This is done so that for the successive labels request, these labels can be located easily, which is good because the machine-name
        'column is indexed.
        'This function returns either: a serial number or a Failure message.

        Dim ds As New DataSet
        Dim cmd As New OracleCommand
        Dim da As OracleDataAdapter

        Dim sqli As New StringBuilder
        Dim schema As String = GetSchemaName()
        Dim ep As String 'error process
        Dim bout As Boolean = False
        Dim numlab As Int16
        Dim retv As Boolean, ttt As New StringBuilder
        Dim rets As String, rets1 As String

        Try
            If CN.State = System.Data.ConnectionState.Closed Then
                CN.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                CN.Open()
            Else
                'if the connection is passed in, we will use the transaction
                With cmd
                    .Connection = CN
                    .Transaction = trans
                    .CommandType = CommandType.Text
                End With
            End If

            Destination.UseGeneric = False ' set this befor starting
            Select Case ControlFlag
                Case "Reserve"
                    '0 delete any existing schedule entries for this machine
                    sqli.Remove(0, sqli.Length)
                    sqli.Append("Delete from mesdba.ics_schedule_pool where Machine_ID = ")
                    sqli.Append(Destination.mes_machine_id)
                    With cmd
                        .CommandText = sqli.ToString
                        .ExecuteNonQuery()
                        .CommandText = "Commit"
                        .ExecuteNonQuery()
                    End With
GetScheduleR:

                    If Not bout Then '1 If not out of schedule entries, Check the schedule for next scheduled item
                        '8/18/05  PAW modified this because it was pulling SL for restricted lines.
                        sqli.Remove(0, sqli.Length)
                        sqli.Append("update " & schema & "ics_schedule_pool sa set sa.MACHINE_ID = " & Destination.mes_machine_id & ", sa.PKGS_USED_QTY =0 ")
                        sqli.Append(" where sa.schedule_id in (select schedule_id from (Select sp.Schedule_ID from " & schema & "ICs_schedule_pool sp where ")
                        sqli.Append(" (sp.mes_part_id = " & Destination.mes_part_id & ") and (sp.REVISION_PHYSICAL = '" & Destination.Revision_Physical & "')")
                        sqli.Append(" and (sp.package_code = '" & Destination.Package_Code & "') and (sp.MACHINE_ID is null)")
                        sqli.Append(" and (" & Destination.mes_machine_id & " || ':' || sp.mes_part_Id || ':' || sp.CONTAINER_CODE) not in")
                        sqli.Append(" (select r1.MACHINE_ID || ':' || r1.mes_part_Id || ':' || r1.CONTAINER_CODE as f1 from " & schema & "part_machine_restriction r1)")
                        sqli.Append(" order by sp.build_priority, sp.SCHEDULE_ID) where rownum =1)")
                        With cmd
                            .CommandText = sqli.ToString
                            .ExecuteNonQuery()
                            .CommandText = "Commit"
                            .ExecuteNonQuery()
                        End With

                        'Get the schedule record just reserved
                        sqli.Remove(0, sqli.Length)
                        sqli.Append("Select sp.*, mp.part_nbr from " & schema & "ics_schedule_pool sp," & schema & "mes_part mp ")
                        sqli.Append(" where (sp.machine_id = " & Destination.mes_machine_id & ") and mp.mes_part_id = sp.mes_part_id")
                        sqli.Append(" and sp.mes_part_id = ")
                        sqli.Append(Destination.mes_part_id)
                        sqli.Append(" and sp.Package_Code = '" & Destination.Package_Code & "'")

                        cmd.CommandText = sqli.ToString
                        cmd.ExecuteNonQuery()
                        da = New OracleDataAdapter(cmd)
                        ds.Clear()
                        da.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            'destination will be contain serial number of next label to print
                            With ds.Tables(0)
                                Destination.mes_part_id = .Rows(0).Item("mes_part_id")
                                Destination.Part_nbr = .Rows(0).Item("Part_Nbr")
                                If IsDBNull(.Rows(0).Item("PKGS_PER_SKID_QTY")) Then
                                    Destination.QtyPkgsperSkidReq = 0
                                Else
                                    Destination.QtyPkgsperSkidReq = .Rows(0).Item("PKGS_PER_SKID_QTY")
                                End If

                                Destination.QtyPkgsUsed = .Rows(0).Item("PKGS_USED_QTY")
                                Destination.ScheduleID = .Rows(0).Item("Schedule_Id")
                                Destination.CNTR = .Rows(0).Item("CONTAINER_CODE") & ""
                                Destination.MatID = .Rows(0).Item("MatId") & ""

                                If IsDBNull(.Rows(0).Item("Pkgs_Used_Qty")) Then
                                    Destination.QtyPkgsUsed = 0
                                Else
                                    Destination.QtyPkgsUsed = .Rows(0).Item("Pkgs_Used_Qty")
                                End If
                                Destination.CNTR = .Rows(0).Item("Container_code") & ""

                            End With
                            'we have a schedule entrie, now reserve the serials
                            rets = ReserveSerials(Destination, ds.Tables(0).Rows(0).Item("PKGS_PER_SKID_QTY"))

                            If InStr(rets, "Failure") = 0 Then
                                'successfully reserved serial numbers
                                Return "Serial:" & rets
                            Else
                                'This failed because we are out of labels
                                'send email, 
                                ttt.Remove(0, ttt.Length)
                                ttt.Append(" Part Nbr: " & Destination.Part_nbr)
                                ttt.Append(" ECL : " & Destination.Revision_Physical)
                                ttt.Append(" Package code: " & Destination.Package_Code)
                                rets1 = pSendEmail("Label Data Not Available for: ", "Normal", ttt.ToString, "PCL")

                                'delete schedule entrie reserved by this machine and all others with matching parameters 
                                sqli.Remove(0, sqli.Length)
                                sqli.Append("Delete from " & schema & "ics_schedule_pool  where MACHINE_ID = " & Destination.mes_machine_id)
                                sqli.Append(" or (mes_part_id = " & Destination.mes_part_id & ") and (REVISION_PHYSICAL = '" & Destination.Revision_Physical & "')")
                                sqli.Append(" and (package_code = '" & Destination.Package_Code & "') and (MACHINE_ID is null)")
                                sqli.Append(" and (container_code = '" & Destination.CNTR & "')")
                                With cmd
                                    .CommandText = sqli.ToString
                                    .ExecuteNonQuery()
                                    .CommandText = "Commit"
                                    .ExecuteNonQuery()
                                End With
                                'get the next schedule entrie
                                GoTo GetScheduleR
                            End If
                        Else
                            'we failed to reserve a schedule entrie.  We must be out of schedule entries
                            bout = True
                            GoTo GetScheduleR
                        End If
                    Else 'if bout = true
                        'send email to pcl and go with generic                        
                        ttt.Remove(0, ttt.Length)
                        ttt.Append(" Part Nbr: " & Destination.Part_nbr)
                        ttt.Append(" ECL : " & Destination.Revision_Physical)
                        ttt.Append(" Package code: " & Destination.Package_Code)
                        rets1 = pSendEmail("Schedule Entries not found. May have run out of labels.  When the label data is available, re-schedule these: ", "Normal", ttt.ToString, "PCL")

                        'try for generic labels
                        Destination.UseGeneric = True
                        Destination.Part_nbr = ConvertMESParttoPart(Destination.mes_part_id, CN)
                        rets = ReserveSerials(Destination, 1)
                        If InStr(rets, "Failure") = 0 Then
                            'return what was reserved.
                            Return "Serial:" & rets
                        Else
                            'failed to get Generic Labels.
                            'email
                            'fail back to kiosk
                            ttt.Remove(0, ttt.Length)
                            ttt.Append(" Part Nbr: " & Destination.Part_nbr)
                            ttt.Append(" ECL : " & Destination.Revision_Physical)
                            ttt.Append(" Package code: " & Destination.Package_Code)
                            rets1 = pSendEmail("Schedule Entries not found. " & vbCrLf & "Rolled to Generic,  BUT" & vbCrLf & "Generic Serial numbers not Available!", "High", ttt.ToString, "PCL")
                            Return rets
                        End If
                    End If


                Case "WillPrint"
                    ' delete all others for this line, incase they switched packages in the middle of a skid
                    sqli.Remove(0, sqli.Length)
                    sqli.Append("Delete from mesdba.ics_schedule_pool where Machine_ID = ")
                    sqli.Append(Destination.mes_machine_id)
                    With cmd
                        .CommandText = sqli.ToString
                        .ExecuteNonQuery()
                        .CommandText = "Commit"
                        .ExecuteNonQuery()
                    End With
                    bout = False
GetScheduleWP:
                    If Not bout Then 'if not out of schedule entries
                        'get destination
                        '8/18/05  PAW modified this because it was pulling SL for restricted lines.
                        sqli.Remove(0, sqli.Length)
                        sqli.Append("update " & schema & "ics_schedule_pool sa set sa.MACHINE_ID = " & Destination.mes_machine_id & ", sa.PKGS_USED_QTY =1 ")
                        sqli.Append(" where sa.schedule_id in (select schedule_id from (Select sp.Schedule_ID from " & schema & "ICs_schedule_pool sp where ")
                        sqli.Append(" (sp.mes_part_id = " & Destination.mes_part_id & ") and (sp.REVISION_PHYSICAL = '" & Destination.Revision_Physical & "')")
                        sqli.Append(" and (sp.package_code = '" & Destination.Package_Code & "') and (sp.MACHINE_ID is null)")
                        sqli.Append(" and (" & Destination.mes_machine_id & " || ':' || sp.mes_part_Id || ':' || sp.CONTAINER_CODE) not in")
                        sqli.Append(" (select r1.MACHINE_ID || ':' || r1.mes_part_Id || ':' || r1.CONTAINER_CODE as f1 from " & schema & "part_machine_restriction r1)")
                        sqli.Append(" order by sp.build_priority, sp.SCHEDULE_ID) where rownum =1)")
                        With cmd
                            .CommandText = sqli.ToString
                            .ExecuteNonQuery()
                            .CommandText = "Commit"
                            .ExecuteNonQuery()
                        End With

                        'check to see which schedule entrie it reserved
                        sqli.Remove(0, sqli.Length)
                        sqli.Append("Select sp.*, mp.part_nbr from " & schema & "ics_schedule_pool sp, " & schema & "mes_part mp where sp.MACHINE_ID = " & Destination.mes_machine_id & " and sp.mes_part_id = mp.mes_part_id")
                        cmd.CommandText = sqli.ToString
                        ds.Clear()
                        cmd.ExecuteNonQuery()
                        da = New OracleDataAdapter(cmd)
                        da.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            With ds.Tables(0)
                                Destination.mes_part_id = .Rows(0).Item("mes_part_id")
                                Destination.Part_nbr = .Rows(0).Item("Part_Nbr")
                                If IsDBNull(.Rows(0).Item("PKGS_PER_SKID_QTY")) Then
                                    Destination.QtyPkgsperSkidReq = 0
                                Else
                                    Destination.QtyPkgsperSkidReq = .Rows(0).Item("PKGS_PER_SKID_QTY")
                                End If
                                Destination.QtyPkgsUsed = 1
                                Destination.ScheduleID = .Rows(0).Item("Schedule_Id")
                                Destination.CNTR = .Rows(0).Item("CONTAINER_CODE") & ""
                                Destination.MatID = .Rows(0).Item("MatId") & ""
                            End With
                            'reserve the labels
                            rets = ReserveSerials(Destination, ds.Tables(0).Rows(0).Item("PKGS_PER_SKID_QTY"))
                            If InStr(rets, "Failure") = 0 Then
                                Try
                                    numlab = Val(ConfigurationSettings.AppSettings.Get("PkgCode_" & Destination.Package_Code))
                                Catch
                                    numlab = 1
                                End Try
                                If Destination.QtyPkgsUsed >= Destination.QtyPkgsperSkidReq Or (numlab > 1) Then
                                    'if pkgsused >= required, then roll it
                                    'this record only required one package, go ahead and roll to bottom
                                    '8/17/05  Put the roll back in instead of the delete
                                    sqli.Remove(0, sqli.Length)
                                    sqli.Append("select " & schema & "ics_schedule_pool_seq.nextval from dual")
                                    cmd.CommandText = sqli.ToString
                                    ds.Clear()
                                    da = New OracleDataAdapter(cmd)
                                    da.Fill(ds)
                                    If ds.Tables(0).Rows.Count > 0 Then
                                        sqli.Remove(0, sqli.Length)
                                        sqli.Append("Update " & schema & "Ics_Schedule_Pool sp set sp.Schedule_Id = " & ds.Tables(0).Rows(0).Item("NextVal") & ", sp.Pkgs_Used_Qty = 0, sp.MACHINE_ID = null, sp.build_priority = 99")
                                        sqli.Append(" Where sp.schedule_id = " & Destination.ScheduleID)
                                        With cmd
                                            .CommandText = sqli.ToString
                                            .ExecuteNonQuery()
                                            .CommandText = "Commit"
                                            .ExecuteNonQuery()
                                        End With
                                    End If
                                    Destination.Completed = True
                                Else
                                    Destination.Completed = False
                                End If
                                Return "Serial:" & Destination.SerialNbr

                            Else 'failed to reserve the labels, 
                                'send email, 
                                ttt.Remove(0, ttt.Length)
                                ttt.Append(" Part Nbr: " & Destination.Part_nbr)
                                ttt.Append(" ECL : " & Destination.Revision_Physical)
                                ttt.Append(" Package code: " & Destination.Package_Code)
                                rets1 = pSendEmail("Label Data Not Available for: ", "Normal", ttt.ToString, "PCL")

                                'delete schedule entrie reserved by this machine and all others with matching parameters 
                                sqli.Remove(0, sqli.Length)
                                sqli.Append("Delete from " & schema & "ics_schedule_pool  where MACHINE_ID = " & Destination.mes_machine_id)
                                sqli.Append(" or (mes_part_id = " & Destination.mes_part_id & ") and (REVISION_PHYSICAL = '" & Destination.Revision_Physical & "')")
                                sqli.Append(" and (package_code = '" & Destination.Package_Code & "') and (MACHINE_ID is null)")
                                With cmd
                                    .CommandText = sqli.ToString
                                    .ExecuteNonQuery()
                                    .CommandText = "Commit"
                                    .ExecuteNonQuery()
                                End With
                                ' try again with another destination
                                GoTo GetScheduleWP
                            End If
                        Else
                            bout = True 'nothing found
                            GoTo GetScheduleWP
                        End If
                    Else 'out of destinations, try for generic labels

                        'send email
                        ttt.Remove(0, ttt.Length)
                        ttt.Append(" Part Nbr: " & Destination.Part_nbr)
                        ttt.Append(" ECL : " & Destination.Revision_Physical)
                        ttt.Append(" Package code: " & Destination.Package_Code)
                        rets1 = pSendEmail("Schedule Entries with matching label data not found. When the label data is available, re-schedule these: ", "Normal", ttt.ToString, "PCL")

                        'try for generic labels
                        Destination.UseGeneric = True
                        Destination.Part_nbr = ConvertMESParttoPart(Destination.mes_part_id, CN)
                        rets = ReserveSerials(Destination, 1)
                        If InStr(rets, "Failure") = 0 Then
                            'return what was reserved.
                            Return "Serial:" & Destination.SerialNbr
                        Else
                            'failed to get Generic Labels.
                            ttt.Remove(0, ttt.Length)
                            ttt.Append(" Part Nbr: " & Destination.Part_nbr)
                            ttt.Append(" ECL : " & Destination.Revision_Physical)
                            ttt.Append(" Package code: " & Destination.Package_Code)
                            rets1 = pSendEmail("Schedule Entries not found. " & vbCrLf & "Rolled to Generic,  BUT" & vbCrLf & "Generic Serial numbers not Available!", "High", ttt.ToString, "PCL")
                            Return "Failure reserving serial for generic labels"
                        End If
                    End If
            End Select

        Catch ex As Exception
            Return "Failure in ReserveMyNextLabel: " & ex.Message
        Finally
            'because we pass in an open connection, we don't close it
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try
    End Function

    Private Function ReserveSerials(ByRef Destination As clsDestination, ByVal HowMany As Int16) As String
        '2/14/2006  This function must reserve HowMany serials for printing.  It does this by running a sql statement and then verifying the
        '           results.  When the results don't comply, a rollback is done and the Failure is passed back to the caller
        '   It returns either a serial number, or a Failure statement such as FailedNotEnoughSerials of Failure...


        Dim cmd As New OracleCommand
        Dim sqli As New System.Text.StringBuilder
        Dim inSql As New System.Text.StringBuilder
        Dim en As Int16
        Dim ep As String
        Dim MyTran As OracleTransaction
        Dim cmd1 As New OracleCommand
        Dim da As New OracleDataAdapter
        Dim ds As New DataSet
        Dim cn As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnectionICS"))
        Dim destinationlocation As String

        Try
            cn.Open()
            MyTran = cn.BeginTransaction
            cmd.Connection = cn
            cmd.Transaction = MyTran
            Dim lsSQl As String

            If Destination.UseGeneric Then
                'The will print call should already have a serial reserved for Generics.  Look for that one first, then reserve
                ep = "looking for previously reserved Generic Serial"
                sqli.Remove(0, sqli.Length)
                sqli.Append("SELECT SERIAL_NBR FROM pltfloor.ICS_LABEL_DATA_VW WHERE")
                en = 1
                sqli.Append(" PART_NBR = '" & Destination.Part_nbr & "'")
                en = 2
                sqli.Append(" AND QUANTITY = " & Destination.QtyPieces)
                sqli.Append(" AND Used_Status = '1'")
                en = 3
                sqli.Append(" AND Upper(Package_Code) = '" & UCase(Destination.Package_Code) & "'")
                en = 4
                sqli.Append(" AND Upper(ECL) = '" & UCase(Destination.Revision_Physical) & "'")
                sqli.Append(" AND DESTINATION is null ")
                sqli.Append(" AND (Sysdate - mod_tmstm <= 350)") 'make sure n
                en = 5
                sqli.Append(" and machine_name = '" & Destination.MachineName & "-RSV'")
                en = 6
                lsSQl = sqli.ToString
                With cmd
                    .CommandType = CommandType.Text
                    .CommandText = sqli.ToString
                    .ExecuteNonQuery()
                End With
                da = New OracleDataAdapter(cmd)
                da.Fill(ds)
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        Return ds.Tables(0).Rows(0).Item("serial_nbr")
                    Else
                        'no serial found
                    End If
                Else
                    'no serial found
                End If
            End If

            'at this point, one of two conditions exist.
            '1) not using a generic
            '2) using a generic, but did not find one reserved
            '   in Both cases we go through this next reserve serial logic
            'Beginning reserve logic:  this is the sub query
            ep = "building sub-query string for ReserveSerials"
            inSql.Remove(0, inSql.Length)
            inSql.Append("(SELECT SERIAL_NBR FROM pltfloor.ICS_LABEL_DATA_VW WHERE")
            en = 10
            inSql.Append(" PART_NBR = '" & Destination.Part_nbr & "'")
            en = 20
            inSql.Append(" AND QUANTITY = " & Destination.QtyPieces)
            inSql.Append(" AND Used_Status = '0'")
            en = 30
            inSql.Append(" AND Upper(Package_Code) = '" & UCase(Destination.Package_Code) & "'")
            en = 40
            inSql.Append(" AND Upper(ECL) = '" & UCase(Destination.Revision_Physical) & "'")
            If Destination.UseGeneric Then
                inSql.Append(" AND DESTINATION is null ")
            Else
                en = 50
                inSql.Append(" AND upper(DESTINATION) = '" & UCase(Trim$(Destination.CNTR)) & "' ")
            End If
            inSql.Append(" AND (Sysdate - mod_tmstm <= 350)") 'make sure n
            en = 60
            inSql.Append(" AND rownum < " & HowMany + 1 & ")")
            lsSQl = inSql.ToString

            'build main update sql
            sqli.Remove(0, sqli.Length)
            'DSM: don't have rights with TV10 to update ics_label_data_vw
            sqli.Append("Update snapmgr.ics_label_data set used_status = 1, ")
            'sqli.Append("Update pltfloor.ics_label_data_vw  set used_status = 1, ")

            sqli.Append(" machine_name = '")
            en = 70
            If Destination.UseGeneric Then
                sqli.Append(Destination.MachineName & "-RSV'")   'with generic, there will not be a 2-12 follow up and there is not a schedule ID, so we use machinename and .
            Else
                sqli.Append(Destination.ScheduleID & "' || '-' || rownum ")
            End If
            sqli.Append(" where serial_nbr in ")
            en = 80
            sqli.Append(inSql.ToString)
            lsSQl = sqli.ToString

            'reserve the labels
            With cmd
                .CommandType = CommandType.Text
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With

            'check to see if we successfully reserved enough
            With cmd
                sqli.Remove(0, sqli.Length)
                If Destination.UseGeneric Then
                    sqli.Append("Select serial_nbr from pltfloor.ics_label_data_vw where machine_name = '")
                    sqli.Append(Destination.MachineName & "-RSV' and used_status = 1 order by serial_nbr ")
                Else
                    sqli.Append("Select serial_nbr from pltfloor.ics_label_data_vw where machine_name like '")
                    sqli.Append(Destination.ScheduleID & "-%' and used_status = 1 order by serial_nbr ")
                End If
                lsSQl = sqli.ToString
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count < HowMany Then
                'we were not able to reserve all the labels required, we must assume they have run out.
                MyTran.Rollback()
                Return "FailureNotEnoughSerials"
            Else
                MyTran.Commit()
                Destination.SerialNbr = ds.Tables(0).Rows(0).Item("Serial_Nbr")

                'DSM: add embedding destination location in the destination object for SAP call
                destinationlocation = GetDestinationLocation(Destination.SerialNbr)

                If Mid(destinationlocation, 1, 7) = "Failure" Then
                    Destination.location = ""
                    Return destinationlocation
                Else
                    Destination.location = destinationlocation
                End If

                Return ds.Tables(0).Rows(0).Item("Serial_Nbr")
            End If

        Catch ex As Exception
            Return "Failure in ReserveSerials: " & ep & " at line marker " & en & " because " & ex.Message
        Finally
            cmd.Dispose()
            cmd1.Dispose()
            ds.Dispose()
            cn.Close()
            cn.Dispose()
        End Try
    End Function

    Private Function GetDestinationLocation(ByVal serialnbr As String) As String
        '4/16/08 DSM: created to get Destination Location (PKG_12Z_SEGM_NBR field from ICS_LABEL_DATA table)


        Dim cnOra As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim location As String = ""
        Dim sqli As New StringBuilder

        Try
            cnOra.Open()
            sqli.Append("select PKG_12Z_SEGM_NBR from snapmgr.ics_label_data where serial_nbr = '" & serialnbr & "'")
            With cmd
                .Connection = cnOra
                .CommandType = CommandType.Text
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("PKG_12Z_SEGM_NBR")) Then
                    location = ds.Tables(0).Rows(0).Item("PKG_12Z_SEGM_NBR")
                End If
            End If
            Return location
        Catch ex As Exception
            Return "Failure in GetDestinationLocation: " & ex.Message
        End Try

    End Function

    Private Sub mesSaveICSStandardPack(ByRef labelreq As requestpacket, ByRef ics_data_packet As DataSet, ByRef cnOra As OracleConnection)

        Dim sql As New StringBuilder
        Dim da As New OracleDataAdapter
        Dim ds As New DataSet
        Dim cmd As New OracleCommand

        Try
            'first get the standard_pack_id from the sequencer, then use that
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                cnOra.Open()
            End If
            cmd.Connection = cnOra
            cmd.Transaction = trans
            sql.Append("Select mesdba.standard_pack_seq.nextval from dual")
            cmd.CommandText = sql.ToString
            cmd.ExecuteNonQuery()
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            labelreq.lStdPackID = ds.Tables(0).Rows(0).Item("NextVal")
        Catch ex As Exception
            If ConfigurationSettings.AppSettings.Get("LogEvents") = "True then" Then
                If LogEvents("Failure getting standard_pack_seq.nextval in procedure mesSaveICSStandardPack " & ex.Message & Space(1) & ex.StackTrace) Then
                End If
            End If
            'LogErrors(ex.Message, "mesSaveICSStandardPack", "Failure getting standard_pack_seq.nextval")
            Throw New Exception("Failure Saving ICS Standard Pack: (getting standard_pack_seq.nextval: )" & ex.Message)
        End Try

        Try
            sql.Remove(0, sql.Length)
            sql.Append(" Insert into MESDBA.STANDARD_PACK ")
            sql.Append("   ( ")
            sql.Append(" STANDARD_PACK_ID, REPORT_GROUP_ID, OPERATOR_ID, SERIAL_NBR,")
            sql.Append(" MES_PART_ID, REV_PHYSICAL,")
            sql.Append(" QTY,PACKAGE_CODE,CHECK_SUM,")
            sql.Append(" MACHINE_ID, DESCR, System_flag, location_destination, Gross_Weight, produced_tmstm, production_run_id")
            sql.Append(" ) ")
            sql.Append(" Values (")
            sql.Append(labelreq.lStdPackID & ",")
            sql.Append(labelreq.ReportGroupID & ",")
            sql.Append("'" & labelreq.OperID & "',")
            sql.Append("'" & ics_data_packet.Tables(0).Rows(0).Item("Serial_Nbr") & "',")
            sql.Append(labelreq.lmesPartID & ",")
            sql.Append("'" & labelreq.Revision & "',")
            sql.Append(labelreq.Quantity & ",")
            sql.Append("'" & labelreq.PackageCode & "',")
            sql.Append(ics_data_packet.Tables(0).Rows(0).Item("check_sum") & ",")
            sql.Append(labelreq.lmesMachID & ",")
            sql.Append("'BEC_SPL', ")
            sql.Append("'I', ")
            sql.Append("'" & ics_data_packet.Tables(0).Rows(0).Item("Destination") & "',")
            sql.Append(ics_data_packet.Tables(0).Rows(0).Item("Weight") & ",")
            sql.Append("sysdate, ")
            sql.Append(labelreq.ProdRun)  '11/1/05 PAW modified to put production run id from broadcast code
            sql.Append(" )")


            cmd.CommandText = sql.ToString
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            'MsgBox(ex.Message)
            If ConfigurationSettings.AppSettings.Get("LogEvents") = "True" Then
                If LogEvents("Failure saving standard pack mesSaveICSStandardPack: " & ex.Message & Space(1) & ex.StackTrace) Then
                End If
            End If
            'LogErrors(ex.message, " mesSaveICSStandardPack", "Failure saving standard pack: " & sql.ToString)
            Throw New Exception("Failure saving standard pack: " & sql.ToString & "mesSaveICSStandard " & ex.message)
        End Try

        Try
            sql.Remove(0, sql.Length)
            sql.Append(" Insert into MESDBA.ICS_LABEL ")
            sql.Append("  (STANDARD_PACK_ID, PKG_12Z_SEGM_NBR,PKG_13Z_SEGM_NBR, PKG_14Z_SEGM_NBR, PKG_15Z_SEGM_NBR, PKG_16Z_SEGM_NBR,  ")
            sql.Append("  PKG_17Z_SEGM_NBR, CUSTOMER_NAME, CUSTOMER_STREET_ADDR, CUSTOMER_CITY, ICS_CNTR_PACKAGE_TYPE,  ")
            sql.Append("  MATID, PAY_SUFFIX, MOD_USERID,  ")
            sql.Append("  REF_SEQ_DATA_ID, REF_SEQ_NBR, CUSTOMER_PART_NBR, DELIVERY_LOC_NBR)  ")
            sql.Append("  Values (")
            sql.Append(labelreq.lStdPackID & ", ")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("PKG_12Z_SEGM_NBR") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("PKG_13Z_SEGM_NBR") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("PKG_14Z_SEGM_NBR") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("PKG_15Z_SEGM_NBR") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("PKG_16Z_SEGM_NBR") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("PKG_17Z_SEGM_NBR") & "',")

            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("CUSTOMER_NAME") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("CUSTOMER_STREET_ADDR") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("CUSTOMER_CITY") & "',")

            sql.Append(" '" & Mid(ics_data_packet.Tables(0).Rows(0).Item("ICS_CNTR_PACKAGE_TYPE"), 1, 10) & "',") '11/21/05 PAW had to trim this down because the oracle field sizes don't match
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("MATID") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("Pay_suffix") & "',")
            sql.Append(" '" & labelreq.OperID & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("Ref_seq_data_id") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("Ref_seq_nbr") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("Customer_part_nbr") & "',")
            sql.Append(" '" & ics_data_packet.Tables(0).Rows(0).Item("Delivery_loc_nbr") & "'")
            sql.Append(" )")

            cmd.CommandText = sql.ToString
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            If ConfigurationSettings.AppSettings.Get("LogEvents") = "True" Then
                If LogEvents("Failure saving ICS data, procedure mesSaveICSStandardPack: " & ex.Message & Space(1) & ex.stacktrace & Space(1) & sql.ToString) Then
                End If
            End If
            'LogErrors(ex.Message, "mesSaveICSStandardPack", "Failure saving ICS data: " & sql.ToString)
            Throw New Exception("Failure in mesSaveICSStandardPack: " & ex.Message)
        Finally
            'because we pass in an open connection, we don't close it
            Try
                cmd.Dispose()
            Catch ey As Exception
            End Try

            Try
                da.Dispose()
            Catch ey As Exception
            End Try

            Try
                ds.Dispose()
            Catch ey As Exception
            End Try

        End Try

    End Sub

    <WebMethod()> _
    Public Function GetStandardPackDetails(ByVal broadCastCode As String, ByVal container As String, ByVal labelType As String, ByVal ECL As String) As DataSet

        ' local variables
        Dim con As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim SQL As New System.Text.StringBuilder
        Dim schema As String = GetSchemaName()
        Dim sContainerType As String

        Try

            ' define the SQL statement
            Select Case labelType
                Case "ICS"
                    'This is the code which came from OHIO, which has a link to package_infor, etc.  I have commented this out because
                    'we currently don't have that a view loaded on fanta.  The infomation is not required for our implementation either.

                    SQL.Append("SELECT PR.PRODUCTION_RUN_ID, D.DEPT, PR.SYSTEM_FLAG, PR.mes_part_id, PR.PACKAGE_CODE, PR.STD_PACK_QTY AS STANDARD_PACK, ")
                    SQL.Append("PR.TOTAL_PART_QTY AS BOXCOUNT, PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT, PR.CONTAINER_CODE ")
                    SQL.Append("FROM MESDBA.PRODUCTION_RUN PR, MESDBA.VW_PACKAGE_INFORMATION VW, ")
                    SQL.Append("MESDBA.VW_DEPARTMENT D ")
                    SQL.Append("WHERE PR.PROCESS_ID = '" & broadCastCode & "' ")
                    SQL.Append("AND PR.SYSTEM_FLAG = 'I' AND D.ID = PR.REPORT_GROUP_ID ")
                    SQL.Append("AND PR.SYSTEM_FLAG = VW.SYSTEM_FLAG ")
                    SQL.Append("AND PR.REV_PHYSICAL = '" & ECL & "' ")
                    If container = "Returnable" Then
                        sContainerType = "R"
                    Else
                        sContainerType = "E"
                    End If
                    SQL.Append(" AND VW.RETURNABLE_EXPENDIBLE_FLAG = '" & sContainerType & "' ")
                    SQL.Append("AND VW.PKG_CNT_TYPE = PR.PACKAGE_CODE")



                    'SQL.Append("SELECT PR.PRODUCTION_RUN_ID, D.DEPT, PR.SYSTEM_FLAG, PR.mes_part_id, PR.PACKAGE_CODE, ")
                    'SQL.Append(" PR.STD_PACK_QTY AS STANDARD_PACK, PR.TOTAL_PART_QTY AS BOXCOUNT, PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT, ")
                    'SQL.Append(" PR.CONTAINER_CODE FROM MESDBA.PRODUCTION_RUN PR, MESDBA.VW_DEPARTMENT D WHERE PR.PROCESS_ID = '" & broadCastCode & "' ")
                    'SQL.Append("  AND PR.SYSTEM_FLAG = 'I' AND D.ID = PR.REPORT_GROUP_ID AND PR.REV_PHYSICAL = '" & ECL & "'")

                Case "MTMS"
                    SQL.Append("SELECT PR.PRODUCTION_RUN_ID, D.DEPT, PR.SYSTEM_FLAG, PR.mes_part_id, PR.PACKAGE_CODE, PR.STD_PACK_QTY AS STANDARD_PACK, ")
                    SQL.Append("PR.TOTAL_PART_QTY AS BOXCOUNT, PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT, PR.CONTAINER_CODE ")
                    SQL.Append("FROM MESDBA.PRODUCTION_RUN PR, MESDBA.VW_PACKAGE_INFORMATION VW, ")
                    SQL.Append("MESDBA.VW_DEPARTMENT D ")
                    SQL.Append("WHERE PR.PROCESS_ID = '" & broadCastCode & "' ")
                    SQL.Append("AND PR.SYSTEM_FLAG = 'M' AND D.ID = PR.REPORT_GROUP_ID ")
                    SQL.Append("AND PR.SYSTEM_FLAG = VW.SYSTEM_FLAG ")
                    SQL.Append("AND PR.REV_PHYSICAL = '" & ECL & "' ")
                    If container = "Returnable" Then
                        sContainerType = "R"
                    Else
                        sContainerType = "E"
                    End If
                    SQL.Append(" AND VW.RETURNABLE_EXPENDIBLE_FLAG = '" & sContainerType & "' ")
                    SQL.Append("AND VW.PKG_CNT_TYPE = PR.PACKAGE_CODE")
                Case "PILOT"
                    SQL.Append("SELECT PR.PRODUCTION_RUN_ID, PR.mes_part_id, PR.PACKAGE_CODE, PR.STD_PACK_QTY AS STANDARD_PACK, PR.TOTAL_PART_QTY AS BOXCOUNT, PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT, ")
                    SQL.Append("PR.SYSTEM_FLAG ")
                    SQL.Append("FROM MESDBA.PRODUCTION_RUN PR ")
                    SQL.Append("WHERE PR.PROCESS_ID = ")
                    SQL.Append(broadCastCode)
            End Select    ' psBroadcastLabelType

            'Open the connection
            If con.State = System.Data.ConnectionState.Closed Then con.Open()

            ' get the information
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = SQL.ToString
                .ExecuteNonQuery()
            End With

            'fill the dataset
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)

            Return ds
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try

    End Function ' GetStandardPackDetails


    '*****************************************************************************
    '* Function...: GetPartNumber
    '* Purpose....: Get the part number associated with a broadcast code.
    '* Parameters.: psBroadcastCode (String)
    '*                Broadcast code
    '* Returns....: Part number
    '*****************************************************************************
    <WebMethod()> _
    Public Function GetPartNumber(ByVal psBroadcastCode As String) As DataSet

        ' local variables
        Dim con As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim sSQL As String
        Dim schema As String = GetSchemaName()
        'Dim partNumber As String

        Try

            ' define the SQL statement
            sSQL = "select p.part_nbr, p.mes_part_id, pr.rev_physical from "
            sSQL = sSQL & schema & "mes_part p, " & schema & "production_run pr "
            sSQL = sSQL & "where p.MES_PART_ID = pr.MES_PART_ID and pr.PROCESS_ID = '" & psBroadcastCode & "'"

            'Open the connection
            If con.State = ConnectionState.Closed Then con.Open()

            ' get the information
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = sSQL
                .ExecuteNonQuery()
            End With

            'fill the dataset
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)

            Return ds

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try
    End Function ' GetPartNumber

    Private Function ConvertFromMachineNbrtoMesMachineID(ByVal sMachine As String, ByVal cnOra As OracleConnection)
        Dim cmd As New OracleCommand
        Dim da As New OracleDataAdapter
        Dim ds As New DataSet
        Try
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                cnOra.Open()
            End If
            With cmd
                .Connection = cnOra
                .Transaction = trans
                .CommandType = CommandType.Text
                .CommandText = "Select Machine_Id from mesdba.Machine where Machine_Name = '" & sMachine & "'"
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                ConvertFromMachineNbrtoMesMachineID = ds.Tables(0).Rows(0).Item("Machine_Id")
            Else
                Throw New System.Exception("Failure to Lookup Machine_Id for Machine_Name: " & sMachine)
            End If
            ds.Clear()
        Catch ex As Exception
            Throw New Exception("Failure in ConvertFromMachineNbrtoMesMachineID: " & ex.Message)
        Finally
            'because we pass the connection in, we don't close it
            ds.Dispose()
            da.Dispose()
            cmd.Dispose()
        End Try

    End Function

    <WebMethod()> _
    Public Function GotDLOC(ByVal PartNbr As String, ByVal CNTR As String)
        Dim lMesPartID As Long
        Dim cnORa As New OracleConnection
        Dim ds As New DataSet
        Dim i As Int16
        Dim stmp As New StringBuilder

        Try
            If cnORa Is Nothing Then
                cnORa = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
                cnORa.Open()
            End If

            lMesPartID = GetMESPartId(PartNbr, "")
            ds = GetICSDockCodes(lMesPartID, PartNbr, CNTR, cnORa)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For i = 0 To ds.Tables(0).Columns.Count - 1
                        stmp.Append(ds.Tables(0).Columns(i).ColumnName & ": " & ds.Tables(0).Rows(0).Item(i) & ";")
                    Next i
                    Return stmp.ToString
                Else
                    Return "Failure getting DLOC for PartNbr: " & PartNbr & "  CNTR: " & CNTR
                End If
            Else
                Return "Failure getting DLOC for PartNbr: " & PartNbr & " CNTR: " & CNTR & "  Check Oracle database for problems"
            End If

        Catch ex As Exception
            Return "Failure getting DLOC: >>" & ex.Message
        Finally
            cnORa.Close()
            cnORa.Dispose()
        End Try
    End Function

    <WebMethod()> _
    Public Function GetOfflinePrinterName(ByVal reportGroupID As Long) As String
        Dim oflp As String
        Try
            oflp = ConfigurationSettings.AppSettings.Get("OffLinePrinter_" & reportGroupID)
            Return oflp
        Catch ex As Exception
            Return "Failure GettingOfflinePrinterName: =>" & ex.Message
        End Try
    End Function

    <WebMethod(), SoapHeader("greeting")> _
    Public Function GotLabels(ByVal PartNbr As String, ByVal CNTR As String) As String
        Dim cnORa As New OracleConnection
        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim sql As New StringBuilder
        Dim rets As New StringBuilder


        If greeting Is Nothing Then
            Return "Please supply proper credentials"
        Else
            If greeting.UserID = ConfigurationSettings.AppSettings.Get("GreetingUserID") And greeting.Password = ConfigurationSettings.AppSettings.Get("GreetingPasscode") Then
                'youre in
            Else
                Return "Please supply proper credentials"
            End If
        End If

        Try

            cnORa = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
            cnORa.Open()

            sql.Append("Select count(Serial_Nbr) as LabelsAvail, Matid,Package_code from pltfloor.ics_label_data_vw where (Part_nbr = '" & PartNbr & "' or Customer_Part_Nbr = '" & PartNbr & "') and Destination = '" & CNTR & "' group by matid,package_code")

            With cmd
                .CommandType = CommandType.Text
                .CommandText = sql.ToString
                .Connection = cnORa
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim i As Integer
                    For i = 0 To ds.Tables(0).Rows.Count - 1
                        rets.Append("PartNbr: " & PartNbr & "  CNTR:" & CNTR & "  MATID:" & ds.Tables(0).Rows(i).Item("MATID") & "  Package_Code:" & ds.Tables(0).Rows(i).Item("Package_code") & "  Available: " & ds.Tables(0).Rows(i).Item("LabelsAvail") & "VBCRLF")
                    Next i
                Else
                    rets.Append("ICS data NOT available for PartNbr: " & PartNbr & "  CNTR: " & CNTR)
                End If
            Else
                rets.Append("Failure determining ICS label data availability for PartNbr: " & PartNbr & "  CNTR: " & CNTR)
            End If
            Return rets.ToString

        Catch ex As Exception
            Return "Failure determining labels available for PartNbr: " & PartNbr & "  CNTR: " & CNTR & "  >>" & ex.Message
        Finally
            cmd.Dispose()
            cnORa.Close()
            cnORa.Dispose()
            da.Dispose()
            ds.Dispose()
        End Try

    End Function

    Private Function GetICSDockCodes(ByVal lMesPartID As Long, ByVal sPartNbr As String, ByVal sContainer As String, ByRef moCN As OracleConnection) As DataSet

        Dim CmdICS_Delivery_Location As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim sel, frm, whr As String
        Dim sqli As New StringBuilder
        Dim schema As String

        Try
            If moCN.State = System.Data.ConnectionState.Closed Then
                moCN.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                moCN.Open()
            End If
            schema = GetSchemaName()
            If lMesPartID = 0 Then
                'use part_nbr 
                sqli.Append("SELECT MP.Part_nbr, DL.*  FROM " & schema & "ICS_DELIVERY_LOCATION DL, " & schema & "mes_part MP ")
                sqli.Append(" WHERE DL.mes_part_id in ")
                sqli.Append(" (Select m1.mes_part_id from " & schema & "mes_part m1 where m1.part_nbr = '" & sPartNbr & "') ")
                sqli.Append(" and dl.CONTAINER_CODE = '" & sContainer & "' And dl.mes_part_id = mp.mes_part_id")
            Else
                'use lmespartId for lookup COmmented out DM  
                'sqli.Append("SELECT MP.Part_nbr, DL.*  FROM " & schema & "ICS_DELIVERY_LOCATION DL, " & schema & "mes_part MP ")
                'sqli.Append(" WHERE DL.mes_part_id = " & lMesPartID & " and CONTAINER_CODE = '" & sContainer.ToUpper & "'")
                'sqli.Append(" And dl.mes_part_id = mp.mes_part_id")

                '6/12/08 - AC Added dl.used_status = 0  to insure that only current records are read (not 1 or 9 (deleted)
                sqli.Append("SELECT MP.Part_nbr, mp.mes_part_id, destination as Container_Code, 'CS' as Control_Source, dl.Time_used as Release_Date, ")
                sqli.Append(" Ref_Seq_Data_id as FIRST_REF_QUAL, ")
                sqli.Append(" Ref_Seq_NBR as First_Qual_Text, pkg_12z_segm_nbr as PKG_11Z, ")
                sqli.Append(" Delivery_Loc_nbr as PKG_12Z,  PKG_13Z_segm_nbr as PKG_13Z, ")
                sqli.Append(" PKG_14Z_segm_nbr as PKG_14Z,  PKG_15Z_segm_nbr as PKG_15Z, ")
                sqli.Append(" PKG_16Z_segm_nbr as PKG_16Z,  PKG_17Z_segm_nbr as PKG_17Z ")
                sqli.Append(" FROM snapmgr.ICS_label_data DL, " & schema & "mes_part MP   ")
                sqli.Append(" WHERE dl.used_status = 0 and  mp.mes_part_id = " & lMesPartID & " and mp.Part_nbr = dl.part_nbr and destination = '" & sContainer.ToUpper & "'")
                sqli.Append(" ORDER BY dl.mod_tmstm DESC NULLS LAST")


            End If

            With CmdICS_Delivery_Location
                .Connection = moCN
                .Transaction = trans
                .CommandType = CommandType.Text
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With

            'fill the dataset
            da = New OracleDataAdapter(CmdICS_Delivery_Location)
            da.Fill(ds)

        Catch ex As Exception
            Throw New Exception("Failure getting DockCodes: " & ex.Message)
        Finally
            'because the open connection is passed to this, we don't close it
            GetICSDockCodes = ds
            CmdICS_Delivery_Location.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try
    End Function

    Private Function ConvertFromParttoMesPart(ByVal sPartNbr As String, ByRef cnOra As OracleConnection) As Long
        Dim cmd As New OracleCommand
        Dim da As New OracleDataAdapter
        Dim ds As New DataSet
        Try
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                cnOra.Open()
            End If
            With cmd
                .Connection = cnOra
                .Transaction = trans
                .CommandType = CommandType.Text
                .CommandText = "Select mes_part_id from mesdba.mes_part where part_nbr = '" & sPartNbr & "'"
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                ConvertFromParttoMesPart = ds.Tables(0).Rows(0).Item("mes_part_id")
            Else
                Throw New System.Exception("Failure to Lookup mes_part_nbr for Part_nbr: " & sPartNbr)
            End If
            ds.Clear()
        Catch ex As Exception
            Throw New Exception("Failure in ConvertFromParttoMesPart: " & ex.Message)
        Finally
            'because the open connection is passed, we don't close it
            ds.Dispose()
            da.Dispose()
            cmd.Dispose()
        End Try
    End Function

    Private Function ConvertMESParttoPart(ByVal MesPartNbr As Long, ByRef cnOra As OracleConnection) As String
        Dim cmd As New OracleCommand
        Dim da As New OracleDataAdapter
        Dim ds As New DataSet
        Try
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                cnOra.Open()
            End If
            With cmd
                .Connection = cnOra
                .Transaction = trans
                .CommandType = CommandType.Text
                .CommandText = "Select part_nbr from mesdba.mes_part where mes_part_id = " & MesPartNbr
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                ConvertMESParttoPart = ds.Tables(0).Rows(0).Item("part_nbr")
            Else
                Throw New System.Exception("Failure to Lookup Part_nbr for MESPart_ID: " & MesPartNbr)
            End If
            ds.Clear()
        Catch ex As Exception
            Throw New Exception("Failure in ConvertMESParttoPart: " & ex.Message)
        Finally
            'because the open connection is passed, we don't close it
            ds.Dispose()
            da.Dispose()
            cmd.Dispose()
        End Try
    End Function

    Private Function ConvertFromReportGroupIDtoDept(ByVal ID As String, ByRef cnORa As OracleConnection, Optional ByVal ReverseConvert As Boolean = False) As String
        Dim cmd As New OracleCommand
        Dim da As New OracleDataAdapter
        Dim ds As New DataSet
        Try
            If cnORa.State = System.Data.ConnectionState.Closed Then
                cnORa.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                cnORa.Open()
            End If
            With cmd
                .Connection = cnORa
                .Transaction = trans
                .CommandType = CommandType.Text
                If ReverseConvert = True Then
                    If Len(ID.Trim) = 4 Then
                        ID = "0" & ID 'departements contain 5 string digits
                    End If
                    .CommandText = "Select ID from mesdba.vw_department where Dept = '" & ID & "'"
                Else
                    .CommandText = "Select Dept from mesdba.vw_department where ID = " & CLng(ID)
                End If
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                ConvertFromReportGroupIDtoDept = ds.Tables(0).Rows(0).Item(0)
            Else
                Throw New System.Exception("Failure in ConvertFromReportGroupIDtoDept for ID: " & ID)
            End If
            ds.Clear()
        Catch ex As Exception
            Throw New Exception("Failure in ConvertFromReportGroupIDtoDept: " & ex.Message)
        Finally
            'because open connection is passed in, we don't close it
            ds.Dispose()
            da.Dispose()
            cmd.Dispose()
        End Try

    End Function

    <WebMethod()> Public Function testWebMethods()

        If Common.GetServerType = "Production" Then
            Return "This function is turned off for Production"
        End If

        Dim dot As dotMESQualityClass.mesQuality
        Dim ds As New DataSet
        Dim retv As String, result As String



        Try
            Dim fsReadXml As New System.IO.FileStream("C:\Inetpub\wwwroot\BECWebService_RFC100-03-124001\bin\xmlttt.xml", System.IO.FileMode.Open)
            Dim myXmlReader As New System.Xml.XmlTextReader(fsReadXml)
            ds.ReadXml(myXmlReader)
            myXmlReader.Close()
            dot = New dotMESQualityClass.mesQuality(Common.GetOracleConnectionString("OracleConnection"))
            retv = dot.WriteInspectionRecord(ds)
            Return retv
        Catch ex As Exception
            Return "Failure: " & ex.Message
        Finally
            dot = Nothing
        End Try


    End Function

    Private Function GetNextVal() As Int64
        '4/15/08 DSM: created to get nextval from sequencer for 4-C S/N's

        Dim cmd As New OracleCommand
        Dim cnOra As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim nextval As Int64
        Dim sqds As New DataSet
        Dim sqda As OracleDataAdapter
        Dim sqstring As New StringBuilder

        Try
            cnOra.Open()
            sqstring.Remove(0, sqstring.Length)
            sqstring.Append("select pltfloor.BEC_SERIAL_NBR.nextval from dual")
            With cmd
                .Connection = cnOra
                .CommandType = CommandType.Text
                .CommandText = sqstring.ToString
                .ExecuteNonQuery()
            End With
            sqda = New OracleDataAdapter(cmd)
            sqda.Fill(sqds)
            If sqds.Tables(0).Rows.Count > 0 Then
                nextval = sqds.Tables(0).Rows(0).Item("NextVal")
                GetNextVal = nextval
            Else
                Throw New Exception("Failure Aborting Skid:  Could not get nextval from Standard_Pack_SN Sequencer.")
            End If
        Catch ex As Exception
            Return "</?BWSError> Failure generating new S/N's for 4-C Underhood Tote in FormatOutput Function: " & " " & ex.Message
        Finally
            Try
                cmd.Dispose()
            Catch ex As Exception
            End Try
            Try
                sqds.Dispose()
            Catch ex As Exception
            End Try
            Try
                sqda.Dispose()
            Catch ex As Exception
            End Try

            Try
                cnOra.Close()
            Catch ex As Exception
            End Try
            Try
                cnOra.Dispose()
            Catch ex As Exception
            End Try
        End Try

    End Function

    Public Function FormatOutput(ByVal dsICS As DataSet, ByVal Reprint As Boolean) As String
        'This will take the data and format it onto the label and return that

        '4/15/08 David Maibor (DSM) Modify S/N's for Unherhood 4-C containers to use sequencer

        Dim retv, i As Integer
        Dim def_Prn$, prn$
        Dim dsLabelDef As New DataSet
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim labelsRequiredQty As Int32
        Dim template() As String
        Dim sr As StreamReader
        Dim line As String
        Dim ostring As New StringBuilder
        Dim tmp As New StringBuilder
        Dim b As New StringBuilder
        Dim j, k As Int32


        '4/15/08 DSM: added for Underhood 4-C serial numbers

        Dim nextval As Long
        Dim serialnumber_string As String


        'create the table def for the output table
        dt.Columns.Add("PNBR", GetType(System.String))
        dt.Columns.Add("ECL", GetType(System.String))
        dt.Columns.Add("BCL", GetType(System.String))
        dt.Columns.Add("NWM", GetType(System.String))
        dt.Columns.Add("NW", GetType(System.String))
        dt.Columns.Add("GW", GetType(System.String))
        dt.Columns.Add("GWM", GetType(System.String))
        dt.Columns.Add("CS", GetType(System.String))
        dt.Columns.Add("LOT_NBR", GetType(System.String))
        dt.Columns.Add("SER9", GetType(System.String))
        dt.Columns.Add("SER9A", GetType(System.String))
        dt.Columns.Add("SERNBR", GetType(System.String))
        dt.Columns.Add("ST", GetType(System.String))
        dt.Columns.Add("DEPT", GetType(System.String))
        dt.Columns.Add("QTY", GetType(System.String))
        dt.Columns.Add("PCD", GetType(System.String))
        dt.Columns.Add("CNTRS", GetType(System.String))
        dt.Columns.Add("CNTR", GetType(System.String))
        dt.Columns.Add("END_DATE", GetType(System.String))
        dt.Columns.Add("DLN", GetType(System.String))
        dt.Columns.Add("BC", GetType(System.String))
        dt.Columns.Add("QTYPAC", GetType(System.String))
        dt.Columns.Add("RSD", GetType(System.String))
        dt.Columns.Add("RSN", GetType(System.String))
        dt.Columns.Add("P12Z", GetType(System.String))
        dt.Columns.Add("P13Z", GetType(System.String))
        dt.Columns.Add("P14Z", GetType(System.String))
        dt.Columns.Add("P15Z", GetType(System.String))
        dt.Columns.Add("P16Z", GetType(System.String))
        dt.Columns.Add("P17Z", GetType(System.String))
        dt.Columns.Add("SHIP_DATE", GetType(System.String))
        dt.Columns.Add("SNAME", GetType(System.String))
        dt.Columns.Add("SADDR", GetType(System.String))
        dt.Columns.Add("SCITY", GetType(System.String))
        dt.Columns.Add("SSTATE", GetType(System.String))
        dt.Columns.Add("SZIP", GetType(System.String))
        dt.Columns.Add("SCOUNTRY", GetType(System.String))
        dt.Columns.Add("HDR", GetType(System.String))
        dt.Columns.Add("SEP", GetType(System.String))
        dt.Columns.Add("TRL", GetType(System.String))
        dt.Columns.Add("DESC", GetType(System.String))
        dt.Columns.Add("SER8", GetType(System.String))
        dt.Columns.Add("CNAME", GetType(System.String))
        dt.Columns.Add("CADDR", GetType(System.String))
        dt.Columns.Add("CCITY", GetType(System.String))
        dt.Columns.Add("CSTATE", GetType(System.String))
        dt.Columns.Add("CZIP", GetType(System.String))
        dt.Columns.Add("CNTRP", GetType(System.String))
        dt.Columns.Add("SD", GetType(System.String))

        Try
            labelsRequiredQty = ConfigurationSettings.AppSettings.Get("PkgCode_" & dsICS.Tables(0).Rows(0).Item("Package_code"))
            If labelsRequiredQty = 0 Then labelsRequiredQty = 1 'use default
            If labelsRequiredQty > 1 Then labelsRequiredQty += 1 'increment to allow for master label
            ReDim template(labelsRequiredQty)
            'This is where multiple records are created for package codes which require them
            For i = 0 To labelsRequiredQty - 1
                dr = dt.NewRow 'add row
                With dsICS.Tables(0)
                    If Len((.Rows(0).Item("Customer_part_nbr"))) > 0 Then
                        dr.Item("PNBR") = .Rows(0).Item("Customer_part_nbr")
                    Else
                        dr.Item("PNBR") = .Rows(0).Item("Part_Nbr")
                    End If

                    dr.Item("ECL") = .Rows(0).Item("ecl")
                    dr.Item("BCL") = "2P" & .Rows(0).Item("ecl")
                    If labelsRequiredQty = 1 Then
                        dr.Item("NWM") = Int((0 & .Rows(0).Item("Weight")) / 2.2) & " KG"
                        dr.Item("NW") = Int((0 & .Rows(0).Item("Weight")) / 2.2)
                        dr.Item("GW") = Int((0 & .Rows(0).Item("Weight")) / 2.2)
                        dr.Item("GWM") = Int((0 & .Rows(0).Item("Weight")) / 2.2) & " KG"
                        dr.Item("QTY") = .Rows(0).Item("Quantity")
                        dr.Item("BC") = "1"
                    Else 'multiple label scenerio
                        If i > 0 Then
                            dr.Item("QTY") = .Rows(0).Item("Quantity") / (labelsRequiredQty - 1)
                            dr.Item("BC") = i & " of " & labelsRequiredQty - 1
                            dr.Item("QTYPAC") = Int(.Rows(0).Item("Quantity") / (labelsRequiredQty - 1))
                            dr.Item("NWM") = Int((0 & .Rows(0).Item("Weight")) / (labelsRequiredQty - 1) / 2.2) & " KG"
                            dr.Item("NW") = Int((0 & .Rows(0).Item("Weight")) / (labelsRequiredQty - 1) / 2.2)
                            dr.Item("GW") = Int((0 & .Rows(0).Item("Weight")) / (labelsRequiredQty - 1) / 2.2)
                            dr.Item("GWM") = Int((0 & .Rows(0).Item("Weight")) / (labelsRequiredQty - 1) / 2.2) & " KG"
                        Else 'i = 0
                            dr.Item("QTY") = .Rows(0).Item("Quantity") 'master label
                            dr.Item("BC") = labelsRequiredQty - 1
                            dr.Item("QTYPAC") = Int(.Rows(0).Item("Quantity") / (labelsRequiredQty - 1))
                            dr.Item("NWM") = Int((0 & .Rows(0).Item("Weight")) / 2.2) & " KG"
                            dr.Item("NW") = Int((0 & .Rows(0).Item("Weight")) / 2.2)
                            dr.Item("GW") = Int((0 & .Rows(0).Item("Weight")) / 2.2)
                            dr.Item("GWM") = Int((0 & .Rows(0).Item("Weight")) / 2.2) & " KG"
                        End If
                    End If

                    dr.Item("CS") = .Rows(0).Item("check_sum")
                    dr.Item("SER9") = Right$(.Rows(0).Item("Serial_nbr"), 9)
                    'dr.Item("SER9") = "00000"
                    dr.Item("SER9A") = Left$(Right$(.Rows(0).Item("Serial_nbr"), 9), 1) & Right(.Rows(0).Item("Serial_nbr"), 8)
                    'dr.Item("SER9A") = "00000"
                    dr.Item("SERNBR") = .Rows(0).Item("Serial_nbr")
                    'dr.Item("SerNBR") = "00000"
                    dr.Item("DEPT") = .Rows(0).Item("Dept")

                    dr.Item("PCD") = .Rows(0).Item("Package_code")
                    If i > 0 Then  'then this a multiple label scenerio, and we need to containers
                        dr.Item("CNTRS") = i & " of " & labelsRequiredQty - 1
                    End If
                    'DSM: check if NULL for generics. Add ship to after destination if not generic
                    'dr.Item("CNTR") = .Rows(0).Item("Destination")
                    If IsDBNull(.Rows(0).Item("Destination")) Then
                        dr.Item("CNTR") = ""
                    Else
                        dr.Item("CNTR") = .Rows(0).Item("Destination")
                        If Not IsDBNull(.Rows(0).Item("Customer_Filler")) Then
                            dr.Item("CNTR") &= " " & Right(.Rows(0).Item("Customer_Filler"), 6)
                        End If
                    End If


                    dr.Item("END_DATE") = Now.ToShortDateString
                    dr.Item("DLN") = .Rows(0).Item("Delivery_loc_nbr")

                    dr.Item("RSD") = .Rows(0).Item("Ref_seq_data_id")
                    dr.Item("RSN") = .Rows(0).Item("Ref_seq_nbr")
                    'dr.Item("P12Z") = Left(.Rows(0).Item("PKG_12Z_SEGM_NBR") & Space(8), 8)
                    dr.Item("P12Z") = .Rows(0).Item("PKG_12Z_SEGM_NBR") & ""
                    dr.Item("P13Z") = .Rows(0).Item("PKG_13Z_SEGM_NBR") & ""
                    dr.Item("P14Z") = .Rows(0).Item("PKG_14Z_SEGM_NBR") & ""
                    dr.Item("P15Z") = .Rows(0).Item("PKG_15Z_SEGM_NBR") & ""
                    dr.Item("P16Z") = .Rows(0).Item("PKG_16Z_SEGM_NBR") & ""
                    dr.Item("P17Z") = .Rows(0).Item("PKG_17Z_SEGM_NBR") & ""
                    dr.Item("SHIP_DATE") = Now.ToShortDateString

                    dr.Item("SD") = ConfigurationSettings.AppSettings("P23DunCode") 'dunnage code
                    dr.Item("SNAME") = "DELPHI PACKARD"
                    dr.Item("SADDR") = "220 W. Whitworth St."
                    dr.Item("SCITY") = "Hazelhurst"
                    dr.Item("SSTATE") = "MS"
                    dr.Item("SZIP") = "39083"
                    dr.Item("SCOUNTRY") = "USA"
                    'dr.Item("HDR") = Chr(91) & Chr(41) & Chr(62) & "06" & Chr(29)
                    dr.Item("SEP") = Chr(29)
                    'dr.Item("TRL") = Chr(4)

                    'DSM: Revise to create three new S/N's for SAP
                    serialnumber_string = Right(.Rows(0).Item("Serial_nbr"), 8)
                    If i < 2 Then
                        dr.Item("SER8") = Right(.Rows(0).Item("Serial_nbr"), 8)
                        'dr.Item("SER8") = "00000"
                    Else
                        'DSM: Create new S/N for Totes #2/3/4
                        nextval = GetNextVal()
                        serialnumber_string = CStr(nextval)
                        serialnumber_string = Right(serialnumber_string, 8)

                        'Store new 10 digit SN's for appending to the label string returned
                        Select Case i
                            Case 2
                                newSN_2 = CStr(nextval)
                            Case 3
                                newSN_3 = CStr(nextval)
                            Case 4
                                newSN_4 = CStr(nextval)
                        End Select
                        dr.Item("SER8") = serialnumber_string
                    End If


                    dr.Item("DESC") = Left(.Rows(0).Item("Description") & Space(19), 18)

                    If .Rows(0).Item("STD_FLAG") & "" = "I" Then
                        '2D label Printing Properties
                        dr.Item("CNAME") = Left(.Rows(0).Item("Customer_Name") & Space(24), 23)
                        dr.Item("CADDR") = Left(.Rows(0).Item("Customer_Street_Addr") & Space(27), 26)
                        dr.Item("CCITY") = Left(.Rows(0).Item("Customer_City") & Space(27), 26)
                        dr.Item("CNTRP") = .Rows(0).Item("ICS_Cntr_Package_Type") & ""
                        dr.Item("CSTATE") = "" 'this is in another field
                        dr.Item("CZIP") = "" 'this is in another field
                    End If
                    dt.Rows.Add(dr)
                End With

                'DSM: 6/4/08 replace weight & qty on label with values from SAP.BEC_Part_Cntr_Info table, so the values match what is sent to SAP
                Dim gross_weight As Integer
                Dim quantity As Integer
                Dim ship_package As String

                'Set ship_package based on package code
                If dr.Item("PCD") = "C" Or dr.Item("PCD") = "4C" Then
                    ship_package = "CARDBOARD"
                Else
                    ship_package = "RETURNABLE"
                End If

                'Serial number was obtained from ICS_Label_Data as "SER9"
                GetWeightQuantity_For_Label(dr.Item("SERNBR"), ship_package, gross_weight, quantity)

                'Replace weight values on label
                dr.Item("GW") = gross_weight

                'Replace qty values on label
                dr.Item("QTY") = quantity
                dr.Item("QTYPAC") = quantity

                'final step... open the label template file and over write the tags
                '1 determine which label template to use
                Dim strx As String
                strx = Trim(dsICS.Tables(0).Rows(0).Item("Std_Flag"))
                If strx.CompareTo("I") = 0 Then
                    strx = Trim(dsICS.Tables(0).Rows(0).Item("Package_code"))
                    If strx.CompareTo("Z") = 0 Or strx.CompareTo("C") = 0 Then
                        'these are the single label jobs
                        If Reprint = False Then
                            template(0) = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("ICS1724A_300DPI_2D")
                        Else
                            template(0) = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("ICS1724A_300DPI_2D_Reprint")
                        End If
                    Else

                        strx = Trim(dsICS.Tables(0).Rows(0).Item("Package_Code"))
                        If strx.CompareTo("Y") = 0 Then
                            If Reprint = False Then
                                template(0) = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("ICS1724A_300DPI_2DWStubs")
                            Else
                                template(0) = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("ICS1724A_300DPI_2DWStubs_Reprint")
                            End If
                        Else 'other package codes, implies master/child labels

                            If i = 0 Then
                                If Reprint = False Then
                                    template(i) = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("ICS1724B_300DPI_2D") 'master label
                                Else
                                    template(i) = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("ICS1724B_300DPI_2D_Reprint") 'master label
                                End If
                                'template = ConfigurationSettings.AppSettings.Get("stubs300DPI")
                            Else
                                If Reprint = False Then
                                    template(i) = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("ICS1724A_300DPI_2D") 'container label
                                Else
                                    template(i) = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("ICS1724A_300DPI_2D_Reprint") 'container label
                                End If
                            End If
                        End If
                    End If
                Else
                    'Generic templates used here
                    If i = 0 Then
                        template(i) = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("Master300DPI_OLD")
                        'template = ConfigurationSettings.AppSettings.Get("stubs300DPI")
                    Else
                        template(i) = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("Tote300DPI_OLD")
                    End If
                End If

                ' 2 put the data onto the template
                'Create an instance of StreamReader to read from a file.
                sr = New StreamReader(template(i))
                tmp.Remove(0, tmp.Length)
                Do
                    line = sr.ReadLine()
                    tmp.Append(line)
                Loop Until line Is Nothing

                For k = 0 To dt.Columns.Count - 1
                    b.Remove(0, b.Length)
                    b.Append("%" & dt.Columns(k).ColumnName & "%")
                    tmp.Replace(b.ToString, dt.Rows(i).ItemArray(k) & "")
                Next k
                ostring.Append(tmp.ToString)
                If labelsRequiredQty > 1 Then ostring.Append("<<myEndofLabel>>") 'used to show break between labels, kiosk will use this to buffer the labels
                sr.Close()
            Next i

            'Because XML does not support control characters, and the GM bar code has a chr29, here we
            'must replace them with a place holder.
            ostring.Replace(Chr(29), "<Char29>")
            ostring.Replace(Chr(0), "")

            Return ostring.ToString
        Catch ex As Exception
            If ConfigurationSettings.AppSettings.Get("LogEvents") = "True" Then
                If LogEvents("Failure Formatting label in FormatOutput " & ex.Message & Space(1) & ex.StackTrace) Then
                End If
            End If
            'LogErrors(ex.Message, "FormatOutput", "Failure Formatting ICS to Recordset" & Now.Now)
            Throw New Exception("</?BWSError> Failure: FormatOutput>" & ex.Message)

        End Try

    End Function
    Private Sub GetWeightQuantity_For_Label(ByVal serial_nbr As String, ByVal ship_package As String, ByRef weight As Integer, ByRef quantity As Integer)

        '6/4/08 David Maibor(DSM): added to get weight/qty for label from new SAP table

        Dim Delphi_part_nbr As String
        Dim ds As DataSet
        Dim dt As DataTable = Build_BEC_Part_Cntr_Info_table()
        Dim result As String
        Dim shipto_nbr As String

        ds = GetSAPShiptoNbr(serial_nbr)

        If ds.Tables(0).Rows.Count = 0 Then
            Throw New Exception("</?BWSError> Failure: FormatOutput, cannot obtain SAP weight/quantity info.>")
        Else
            Delphi_part_nbr = ds.Tables(0).Rows(0).Item("Delphi_part_nbr")
            shipto_nbr = ds.Tables(0).Rows(0).Item("shipto_nbr")
            ds.Clear()
            ds.Tables.Add(dt)
            GetSAPBecPartCntrInfo(Delphi_part_nbr, shipto_nbr, ship_package, ds)
            result = ds.Tables("BEC_Part_Cntr_Info").Rows(0).Item("result")
            If Mid(result, 1, 7) = "failure" Then
                Throw New Exception("</?BWSError> Failure: FormatOutput, cannot obtain SAP weight/quantity info.>")
            Else
                weight = CInt(ds.Tables("BEC_Part_Cntr_Info").Rows(0).Item("weight"))
                quantity = CInt(ds.Tables("BEC_Part_Cntr_Info").Rows(0).Item("quantity"))
            End If
        End If

    End Sub
    Private Function GETICSDATA_Old(ByRef labelreq As requestpacket) As DataSet

        'This function uses bing variables to update the row in oracle and return in one pass
        '9/12/05  PAW code inserted into the BECweb project, again.
        '9/14/05  PAW Modified code to get the correct ECL
        '11/21/05  PAW modified, adding code to handle nulls

        Dim sqli As New System.Text.StringBuilder
        Dim cnora As New OleDb.OleDbConnection
        Dim ds As New DataSet
        Dim parm As OleDb.OleDbParameter
        Dim cmd As OleDb.OleDbCommand
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim el As Integer


        Try
            el = 2165
            cnora.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OLEDBConnectionICS")
            el = 2167
            cnora.Open()
            el = 2169
            sqli.Append("Update pltfloor.ICS_LABEL_DATA_vw set used_status = 1, time_used = sysdate, Machine_Name = '" & labelreq.Machine & "' WHERE SERIAL_NBR in (SELECT MIN(SERIAL_NBR) FROM pltfloor.ICS_LABEL_DATA_VW WHERE")
            sqli.Append(" PART_NBR = '" & labelreq.part & "'")
            sqli.Append(" AND QUANTITY = '" & labelreq.Quantity & "'")
            sqli.Append(" AND Used_Status = '0'")
            sqli.Append(" AND Upper(Package_Code) = '" & UCase(labelreq.PackageCode) & "'")
            sqli.Append(" AND Upper(ECL) = '" & UCase(labelreq.Revision) & "'")
            If Trim$(labelreq.CNTR) = "" Then
                sqli.Append(" AND DESTINATION is null ")
            Else
                sqli.Append(" AND upper(DESTINATION) = '" & UCase(Trim$(labelreq.CNTR)) & "' ")
            End If
            sqli.Append(" AND (Sysdate - mod_tmstm <= 350))") 'make sure n
            sqli.Append(" RETURNING Serial_Nbr, Part_nbr, STD_FLAG, FORM_CODE, DESTINATION, QUANTITY,")
            sqli.Append("WEIGHT, ECL, DEPT, PACKAGE_CODE,PAY_SUFFIX,MATID,DESCRIPTION,WAREHOUSE_LOC, ")
            sqli.Append("CUSTOMER_CODE, CUSTOMER_PART_NBR, BROADCAST_CODE, DELIVERY_LOC_NBR, REF_SEQ_DATA_ID,")
            sqli.Append("REF_SEQ_NBR, PKG_12Z_SEGM_NBR, PKG_13Z_SEGM_NBR, PKG_14Z_SEGM_NBR, PKG_15Z_SEGM_NBR,")
            sqli.Append("PKG_16Z_SEGM_NBR, PKG_17Z_SEGM_NBR, CUSTOMER_NAME, CUSTOMER_STREET_ADDR,")
            sqli.Append("CUSTOMER_CITY, ICS_CNTR_PACKAGE_TYPE")

            sqli.Append(" into :bndSN, :bndPartNbr,:bndStdFlag,:bndFormCode,:bndDestination,:bndQuantity,")
            sqli.Append(":bndWeight,:bndECL,:bndDept,:bndPackageCode,:bndPaySuffix,:bndMatID,:bndDesc,:bndWarehouseLoc,")
            sqli.Append(":bndCustomerCode,:bndCustPart_Nbr,:bndBroadCastCode,:bndDeliveryLocNbr,:bndRefSeqDataId,")

            sqli.Append(":bndRefSeqNbr,:bndPkg12Z,:bndPkg13Z,:bndPkg14Z,:bndPkg15Z,:bndPkg16Z,:bndPkg17Z,")
            sqli.Append(":bndCustomerName,:bndCustomerStreetAddr,:bndCUstomerCity,:bndICSCNTRPackageType")
            el = 2195
            cmd = New OleDb.OleDbCommand(sqli.ToString, cnora)
            cmd.CommandType = CommandType.Text
            Dim iProcErrNum As Integer
            parm = New OleDb.OleDbParameter("bndSN", OleDb.OleDbType.VarChar, 10, ParameterDirection.Output, True, 0, 0, "Serial_Nbr", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndPartNbr", OleDb.OleDbType.VarChar, 10, ParameterDirection.Output, True, 0, 0, "Part_Nbr", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndStdFlag", OleDb.OleDbType.VarChar, 1, ParameterDirection.Output, True, 0, 0, "Std_flag", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndFormCode", OleDb.OleDbType.VarChar, 3, ParameterDirection.Output, True, 0, 0, "Form_Code", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndDestination", OleDb.OleDbType.VarChar, 4, ParameterDirection.Output, True, 0, 0, "Destination", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndQuantity", OleDb.OleDbType.Numeric, 0, ParameterDirection.Output, True, 0, 0, "Quantity", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndWeight", OleDb.OleDbType.Numeric, 0, ParameterDirection.Output, True, 0, 0, "Weight", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndECL", OleDb.OleDbType.VarChar, 3, ParameterDirection.Output, True, 0, 0, "ECL", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndDept", OleDb.OleDbType.VarChar, 5, ParameterDirection.Output, True, 0, 0, "Dept", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndPackageCode", OleDb.OleDbType.VarChar, 2, ParameterDirection.Output, True, 0, 0, "Package_code", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndPaySuffix", OleDb.OleDbType.Numeric, 0, ParameterDirection.Output, True, 0, 0, "Pay_Suffix", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndMatID", OleDb.OleDbType.Numeric, 0, ParameterDirection.Output, True, 0, 0, "MatID", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndDesc", OleDb.OleDbType.VarChar, 30, ParameterDirection.Output, True, 0, 0, "Description", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndWarehouseLoc", OleDb.OleDbType.VarChar, 2, ParameterDirection.Output, True, 0, 0, "Warehouse_Loc", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndCustomerCode", OleDb.OleDbType.VarChar, 5, ParameterDirection.Output, True, 0, 0, "Customer_code", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndCustPart_Nbr", OleDb.OleDbType.VarChar, 30, ParameterDirection.Output, True, 0, 0, "Customer_Part_Nbr", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndBroadCastCode", OleDb.OleDbType.VarChar, 3, ParameterDirection.Output, True, 0, 0, "BroadCast_Code", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndDeliveryLocNbr", OleDb.OleDbType.VarChar, 8, ParameterDirection.Output, True, 0, 0, "Delivery_Loc_Nbr", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndRefSeqDataId", OleDb.OleDbType.VarChar, 8, ParameterDirection.Output, True, 0, 0, "Ref_Seq_Data_ID", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndRefSeqNbr", OleDb.OleDbType.VarChar, 8, ParameterDirection.Output, True, 0, 0, "Ref_Seq_Nbr", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndPkg12Z", OleDb.OleDbType.VarChar, 7, ParameterDirection.Output, True, 0, 0, "PKG_12Z_SEGM_NBR", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndPkg13Z", OleDb.OleDbType.VarChar, 25, ParameterDirection.Output, True, 0, 0, "PKG_13Z_SEGM_NBR", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndPkg14Z", OleDb.OleDbType.VarChar, 20, ParameterDirection.Output, True, 0, 0, "PKG_14Z_SEGM_NBR", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndPkg15Z", OleDb.OleDbType.VarChar, 20, ParameterDirection.Output, True, 0, 0, "PKG_15Z_SEGM_NBR", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndPkg16Z", OleDb.OleDbType.VarChar, 25, ParameterDirection.Output, True, 0, 0, "PKG_16Z_SEGM_NBR", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndPkg17Z", OleDb.OleDbType.VarChar, 10, ParameterDirection.Output, True, 0, 0, "PKG_17Z_SEGM_NBR", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndCustomerName", OleDb.OleDbType.VarChar, 30, ParameterDirection.Output, True, 0, 0, "Customer_Name", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndCustomerStreetAddr", OleDb.OleDbType.VarChar, 30, ParameterDirection.Output, True, 0, 0, "Customer_Street_Addr", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndCUstomerCity", OleDb.OleDbType.VarChar, 30, ParameterDirection.Output, True, 0, 0, "Customer_City", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            parm = New OleDb.OleDbParameter("bndICSCNTRPackageType", OleDb.OleDbType.VarChar, 31, ParameterDirection.Output, True, 0, 0, "ICS_CNTR_Package_Type", DataRowVersion.Current, iProcErrNum)
            cmd.Parameters.Add(parm)
            el = 2259
            cmd.ExecuteNonQuery() 'get the data

            With dt 'build the column structure
                .Columns.Add("Serial_Nbr", GetType(System.String))
                .Columns.Add("Part_nbr", GetType(System.String))
                .Columns.Add("STD_FLAG", GetType(System.String))
                .Columns.Add("FORM_CODE", GetType(System.String))
                .Columns.Add("DESTINATION", GetType(System.String))
                .Columns.Add("QUANTITY", GetType(System.Int64))

                .Columns.Add("WEIGHT", GetType(System.Single))
                .Columns.Add("ECL", GetType(System.String))
                .Columns.Add("DEPT", GetType(System.String))
                .Columns.Add("PACKAGE_CODE", GetType(System.String))
                .Columns.Add("PAY_SUFFIX", GetType(System.Int64))
                .Columns.Add("MATID", GetType(System.Int64))
                .Columns.Add("DESCRIPTION", GetType(System.String))
                .Columns.Add("WAREHOUSE_LOC", GetType(System.String))

                .Columns.Add("CUSTOMER_CODE", GetType(System.String))
                .Columns.Add("CUSTOMER_PART_NBR", GetType(System.String))
                .Columns.Add("BROADCAST_CODE", GetType(System.String))
                .Columns.Add("DELIVERY_LOC_NBR", GetType(System.String))
                .Columns.Add("REF_SEQ_DATA_ID", GetType(System.String))

                .Columns.Add("REF_SEQ_NBR", GetType(System.String))
                .Columns.Add("PKG_12Z_SEGM_NBR", GetType(System.String))
                .Columns.Add("PKG_13Z_SEGM_NBR", GetType(System.String))
                .Columns.Add("PKG_14Z_SEGM_NBR", GetType(System.String))
                .Columns.Add("PKG_15Z_SEGM_NBR", GetType(System.String))

                .Columns.Add("PKG_16Z_SEGM_NBR", GetType(System.String))
                .Columns.Add("PKG_17Z_SEGM_NBR", GetType(System.String))
                .Columns.Add("CUSTOMER_NAME", GetType(System.String))
                .Columns.Add("CUSTOMER_STREET_ADDR", GetType(System.String))

                .Columns.Add("CUSTOMER_CITY", GetType(System.String))
                .Columns.Add("ICS_CNTR_PACKAGE_TYPE", GetType(System.String))
                .Columns.Add("CHECK_SUM", GetType(System.Int64))
            End With

            dr = dt.NewRow
            el = 2302
            If Convert.IsDBNull(cmd.Parameters("bndSN").Value) Then
                dr.Item("Serial_Nbr") = ""
            Else
                dr.Item("Serial_Nbr") = Trim$(cmd.Parameters("bndSN").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndPartNbr").Value) Then
                dr.Item("Part_nbr") = ""
            Else
                dr.Item("Part_nbr") = Trim$(cmd.Parameters("bndPartNbr").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndStdFlag").Value) Then
                dr.Item("STD_FLAG") = ""
            Else
                dr.Item("STD_FLAG") = Trim$(cmd.Parameters("bndStdFlag").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndFormCode").Value) Then
                dr.Item("FORM_CODE") = ""
            Else
                dr.Item("Form_code") = Trim$(cmd.Parameters("bndFormCode").Value)
            End If
            If Convert.IsDBNull(cmd.Parameters("bndDestination").Value) Then
                dr.Item("DESTINATION") = ""
            Else
                dr.Item("DESTINATION") = Trim$(cmd.Parameters("bndDestination").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndQuantity").Value) Then
                dr.Item("QUANTITY") = 0
            Else
                dr.Item("QUANTITY") = cmd.Parameters("bndQuantity").Value
            End If
            If Convert.IsDBNull(cmd.Parameters("bndWeight").Value) Then
                dr.Item("WEIGHT") = ""
            Else
                dr.Item("WEIGHT") = Trim$(cmd.Parameters("bndWeight").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndECL").Value) Then
                dr.Item("ECL") = ""
            Else
                dr.Item("ECL") = Trim$(cmd.Parameters("bndECL").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndDept").Value) Then
                dr.Item("DEPT") = ""
            Else
                dr.Item("DEPT") = Trim$(cmd.Parameters("bndDept").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndPackageCode").Value) Then
                dr.Item("PACKAGE_CODE") = ""
            Else
                dr.Item("PACKAGE_CODE") = Trim$(cmd.Parameters("bndPackageCode").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndPaySuffix").Value) Then
                dr.Item("PAY_SUFFIX") = 0
            Else
                dr.Item("PAY_SUFFIX") = cmd.Parameters("bndPaySuffix").Value
            End If

            If Convert.IsDBNull(cmd.Parameters("bndMatID").Value) Then
                dr.Item("MATID") = 0
            Else
                dr.Item("MATID") = cmd.Parameters("bndMatID").Value
            End If

            If Convert.IsDBNull(cmd.Parameters("bndDesc").Value) Then
                dr.Item("DESCRIPTION") = ""
            Else
                dr.Item("DESCRIPTION") = Trim$(cmd.Parameters("bndDesc").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndWarehouseLoc").Value) Then
                dr.Item("WAREHOUSE_LOC") = ""
            Else
                dr.Item("WAREHOUSE_LOC") = Trim$(cmd.Parameters("bndWarehouseLoc").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndCustomerCode").Value) Then
                dr.Item("CUSTOMER_CODE") = ""
            Else
                dr.Item("CUSTOMER_CODE") = Trim$(cmd.Parameters("bndCustomerCode").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndCustPart_Nbr").Value) Then
                dr.Item("CUSTOMER_PART_NBR") = ""
            Else
                dr.Item("CUSTOMER_PART_NBR") = Trim$(cmd.Parameters("bndCustPart_Nbr").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndBroadCastCode").Value) Then
                dr.Item("BROADCAST_CODE") = ""
            Else
                dr.Item("BROADCAST_CODE") = Trim$(cmd.Parameters("bndBroadCastCode").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndDeliveryLocNbr").Value) Then
                dr.Item("DELIVERY_LOC_NBR") = ""
            Else
                dr.Item("DELIVERY_LOC_NBR") = Trim$(cmd.Parameters("bndDeliveryLocNbr").Value)
            End If


            If Convert.IsDBNull(cmd.Parameters("bndRefSeqDataId").Value) Then
                dr.Item("REF_SEQ_DATA_ID") = ""
            Else
                dr.Item("REF_SEQ_DATA_ID") = Trim$(cmd.Parameters("bndRefSeqDataId").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndRefSeqNbr").Value) Then
                dr.Item("REF_SEQ_NBR") = ""
            Else
                dr.Item("REF_SEQ_NBR") = Trim$(cmd.Parameters("bndRefSeqNbr").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndPkg12Z").Value) Then
                dr.Item("PKG_12Z_SEGM_NBR") = ""
            Else
                dr.Item("PKG_12Z_SEGM_NBR") = Trim$(cmd.Parameters("bndPkg12Z").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndPkg13Z").Value) Then
                dr.Item("PKG_13Z_SEGM_NBR") = ""
            Else
                dr.Item("PKG_13Z_SEGM_NBR") = Trim$(cmd.Parameters("bndPkg13Z").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndPkg14Z").Value) Then
                dr.Item("PKG_14Z_SEGM_NBR") = ""
            Else
                dr.Item("PKG_14Z_SEGM_NBR") = Trim$(cmd.Parameters("bndPkg14Z").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndPkg15Z").Value) Then
                dr.Item("PKG_15Z_SEGM_NBR") = ""
            Else
                dr.Item("PKG_15Z_SEGM_NBR") = Trim$(cmd.Parameters("bndPkg15Z").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndPkg16Z").Value) Then
                dr.Item("PKG_16Z_SEGM_NBR") = ""
            Else
                dr.Item("PKG_16Z_SEGM_NBR") = Trim$(cmd.Parameters("bndPkg16Z").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndPkg17Z").Value) Then
                dr.Item("PKG_17Z_SEGM_NBR") = ""
            Else
                dr.Item("PKG_17Z_SEGM_NBR") = Trim$(cmd.Parameters("bndPkg17Z").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndCustomerName").Value) Then
                dr.Item("CUSTOMER_NAME") = ""
            Else
                dr.Item("CUSTOMER_NAME") = Trim$(cmd.Parameters("bndCustomerName").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndCustomerStreetAddr").Value) Then
                dr.Item("CUSTOMER_STREET_ADDR") = ""
            Else
                dr.Item("CUSTOMER_STREET_ADDR") = Trim$(cmd.Parameters("bndCustomerStreetAddr").Value)
            End If


            If Convert.IsDBNull(cmd.Parameters("bndCUstomerCity").Value) Then
                dr.Item("CUSTOMER_CITY") = ""
            Else
                dr.Item("CUSTOMER_CITY") = Trim$(cmd.Parameters("bndCUstomerCity").Value)
            End If

            If Convert.IsDBNull(cmd.Parameters("bndICSCNTRPackageType").Value) Then
                dr.Item("ICS_CNTR_PACKAGE_TYPE") = ""
            Else
                dr.Item("ICS_CNTR_PACKAGE_TYPE") = Trim$(cmd.Parameters("bndICSCNTRPackageType").Value)
            End If

            el = 2333
            If Not Convert.IsDBNull(cmd.Parameters("bndSN")) Then
                dr.Item("CHECK_SUM") = Generate_test_digit(Trim$(cmd.Parameters("bndSN").Value))
            Else
                dr.Item("CHECK_SUM") = ""
            End If

            el = 2337
            dt.Rows.Add(dr) 'append new row
            el = 2239
            ds.Tables.Add(dt) 'append the table
            el = 2341
            Return ds 'return data set

        Catch ex As Exception
            Throw New Exception("Failure in GETICSDAtA at line: " & el & "  " & ex.Message & " " & sqli.ToString)
        Finally
            Try
                dt.Dispose()
            Catch ex As Exception
            End Try
            Try
                ds.Dispose()
            Catch ex As Exception
            End Try
            Try
                cmd.Dispose()
            Catch ex As Exception
            End Try
            Try
                cnora.Close()
            Catch ex As Exception
            End Try
            Try
                cnora.Dispose()
            Catch ex As Exception
            End Try
        End Try
    End Function
    Private Function GETICSDATA(ByRef labelreq As requestpacket, ByVal useSErial As Boolean, ByVal FindThis As String) As DataSet

        '02/14/2006 This function will retrieve ICS labels, based on the new logic where the serial number, has been reserved 
        'before hand.  The lookup is done either by serial number of machine_Name
        'This procedure was created to work with the new auto-Generic logic.

        Dim sqli As New System.Text.StringBuilder
        Dim cnora As New OracleConnection
        Dim ds As New DataSet
        Dim cmd As New OracleCommand
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim da As OracleDataAdapter
        Dim el As Integer

        Try
            el = 2165
            cnora.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnectionICS")
            el = 2167
            cnora.Open()
            el = 2169
            If useSErial Then
                sqli.Append("Select * from pltfloor.ics_label_data_vw where serial_nbr = '" & FindThis & "'")
            Else
                'lookup by machine_name
                sqli.Append("Select * from pltfloor.ics_label_data_vw where Machine_Name = '" & FindThis & "'")
            End If

            el = 2180

            With cmd
                .Connection = cnora
                .CommandType = CommandType.Text
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(dt)

            el = 2181
            'Clean the data, removing any nulls; triming out spaces
            If Convert.IsDBNull(dt.Rows(0).Item("serial_nbr")) Then
                dt.Rows(0).Item("Serial_Nbr") = ""
            Else
                dt.Rows(0).Item("Serial_Nbr") = Trim$(dt.Rows(0).Item("Serial_Nbr"))
            End If

            el = 2182
            If Convert.IsDBNull(dt.Rows(0).Item("Part_nbr")) Then
                dt.Rows(0).Item("Part_nbr") = ""
            Else
                dt.Rows(0).Item("Part_nbr") = Trim$(dt.Rows(0).Item("Part_nbr"))
            End If

            el = 2183
            If Convert.IsDBNull(dt.Rows(0).Item("STD_FLAG")) Then
                dt.Rows(0).Item("STD_FLAG") = ""
            Else
                dt.Rows(0).Item("STD_FLAG") = Trim$(dt.Rows(0).Item("STD_FLAG"))
            End If

            el = 2184
            If Convert.IsDBNull(dt.Rows(0).Item("FORM_CODE")) Then
                dt.Rows(0).Item("FORM_CODE") = ""
            Else
                dt.Rows(0).Item("FORM_CODE") = Trim$(dt.Rows(0).Item("FORM_CODE"))
            End If

            el = 2185
            If Convert.IsDBNull(dt.Rows(0).Item("DESTINATION")) Then
                dt.Rows(0).Item("DESTINATION") = ""
            Else
                dt.Rows(0).Item("DESTINATION") = Trim$(dt.Rows(0).Item("DESTINATION"))
            End If

            el = 2186
            If Convert.IsDBNull(dt.Rows(0).Item("QUANTITY")) Then
                dt.Rows(0).Item("QUANTITY") = ""
            Else
                dt.Rows(0).Item("QUANTITY") = Trim$(dt.Rows(0).Item("QUANTITY"))
            End If

            el = 2187
            If Convert.IsDBNull(dt.Rows(0).Item("WEIGHT")) Then
                dt.Rows(0).Item("WEIGHT") = ""
            Else
                dt.Rows(0).Item("WEIGHT") = Trim$(dt.Rows(0).Item("WEIGHT"))
            End If

            el = 2188
            If Convert.IsDBNull(dt.Rows(0).Item("ECL")) Then
                dt.Rows(0).Item("ECL") = ""
            Else
                dt.Rows(0).Item("ECL") = Trim$(dt.Rows(0).Item("ECL"))
            End If

            el = 2189
            If Convert.IsDBNull(dt.Rows(0).Item("DEPT")) Then
                dt.Rows(0).Item("DEPT") = ""
            Else
                dt.Rows(0).Item("DEPT") = Trim$(dt.Rows(0).Item("DEPT"))
            End If

            el = 2190
            If Convert.IsDBNull(dt.Rows(0).Item("PACKAGE_CODE")) Then
                dt.Rows(0).Item("PACKAGE_CODE") = ""
            Else
                dt.Rows(0).Item("PACKAGE_CODE") = Trim$(dt.Rows(0).Item("PACKAGE_CODE"))
            End If

            el = 2191
            If Convert.IsDBNull(dt.Rows(0).Item("PAY_SUFFIX")) Then
                dt.Rows(0).Item("PAY_SUFFIX") = ""
            Else
                dt.Rows(0).Item("PAY_SUFFIX") = Trim$(dt.Rows(0).Item("PAY_SUFFIX"))
            End If

            el = 2192
            If Convert.IsDBNull(dt.Rows(0).Item("MATID")) Then
                dt.Rows(0).Item("MATID") = ""
            Else
                dt.Rows(0).Item("MATID") = Trim$(dt.Rows(0).Item("MATID"))
            End If

            el = 2193
            If Convert.IsDBNull(dt.Rows(0).Item("DESCRIPTION")) Then
                dt.Rows(0).Item("DESCRIPTION") = ""
            Else
                dt.Rows(0).Item("DESCRIPTION") = Trim$(dt.Rows(0).Item("DESCRIPTION"))
            End If

            el = 2194
            If Convert.IsDBNull(dt.Rows(0).Item("WAREHOUSE_LOC")) Then
                dt.Rows(0).Item("WAREHOUSE_LOC") = ""
            Else
                dt.Rows(0).Item("WAREHOUSE_LOC") = Trim$(dt.Rows(0).Item("WAREHOUSE_LOC"))
            End If

            el = 2195
            If Convert.IsDBNull(dt.Rows(0).Item("CUSTOMER_CODE")) Then
                dt.Rows(0).Item("CUSTOMER_CODE") = ""
            Else
                dt.Rows(0).Item("CUSTOMER_CODE") = Trim$(dt.Rows(0).Item("CUSTOMER_CODE"))
            End If

            el = 2196
            If Convert.IsDBNull(dt.Rows(0).Item("CUSTOMER_PART_NBR")) Then
                dt.Rows(0).Item("CUSTOMER_PART_NBR") = ""
            Else
                dt.Rows(0).Item("CUSTOMER_PART_NBR") = Trim$(dt.Rows(0).Item("CUSTOMER_PART_NBR"))
            End If

            el = 2197
            If Convert.IsDBNull(dt.Rows(0).Item("BROADCAST_CODE")) Then
                dt.Rows(0).Item("BROADCAST_CODE") = ""
            Else
                dt.Rows(0).Item("BROADCAST_CODE") = Trim$(dt.Rows(0).Item("BROADCAST_CODE"))
            End If

            el = 2198
            If Convert.IsDBNull(dt.Rows(0).Item("DELIVERY_LOC_NBR")) Then
                dt.Rows(0).Item("DELIVERY_LOC_NBR") = ""
            Else
                dt.Rows(0).Item("DELIVERY_LOC_NBR") = Trim$(dt.Rows(0).Item("DELIVERY_LOC_NBR"))
            End If

            el = 2199
            If Convert.IsDBNull(dt.Rows(0).Item("REF_SEQ_DATA_ID")) Then
                dt.Rows(0).Item("REF_SEQ_DATA_ID") = ""
            Else
                dt.Rows(0).Item("REF_SEQ_DATA_ID") = Trim$(dt.Rows(0).Item("REF_SEQ_DATA_ID"))
            End If

            el = 2200
            If Convert.IsDBNull(dt.Rows(0).Item("REF_SEQ_NBR")) Then
                dt.Rows(0).Item("REF_SEQ_NBR") = ""
            Else
                dt.Rows(0).Item("REF_SEQ_NBR") = Trim$(dt.Rows(0).Item("REF_SEQ_NBR"))
            End If

            el = 2201
            If Convert.IsDBNull(dt.Rows(0).Item("PKG_12Z_SEGM_NBR")) Then
                dt.Rows(0).Item("PKG_12Z_SEGM_NBR") = ""
            Else
                dt.Rows(0).Item("PKG_12Z_SEGM_NBR") = Trim$(dt.Rows(0).Item("PKG_12Z_SEGM_NBR"))
            End If

            el = 2202
            If Convert.IsDBNull(dt.Rows(0).Item("PKG_13Z_SEGM_NBR")) Then
                dt.Rows(0).Item("PKG_13Z_SEGM_NBR") = ""
            Else
                dt.Rows(0).Item("PKG_13Z_SEGM_NBR") = Trim$(dt.Rows(0).Item("PKG_13Z_SEGM_NBR"))
            End If

            el = 2203
            If Convert.IsDBNull(dt.Rows(0).Item("PKG_14Z_SEGM_NBR")) Then
                dt.Rows(0).Item("PKG_14Z_SEGM_NBR") = ""
            Else
                dt.Rows(0).Item("PKG_14Z_SEGM_NBR") = Trim$(dt.Rows(0).Item("PKG_14Z_SEGM_NBR"))
            End If

            el = 2204
            If Convert.IsDBNull(dt.Rows(0).Item("PKG_15Z_SEGM_NBR")) Then
                dt.Rows(0).Item("PKG_15Z_SEGM_NBR") = ""
            Else
                dt.Rows(0).Item("PKG_15Z_SEGM_NBR") = Trim$(dt.Rows(0).Item("PKG_15Z_SEGM_NBR"))
            End If

            el = 2205
            If Convert.IsDBNull(dt.Rows(0).Item("PKG_16Z_SEGM_NBR")) Then
                dt.Rows(0).Item("PKG_16Z_SEGM_NBR") = ""
            Else
                dt.Rows(0).Item("PKG_16Z_SEGM_NBR") = Trim$(dt.Rows(0).Item("PKG_16Z_SEGM_NBR"))
            End If

            el = 2206
            If Convert.IsDBNull(dt.Rows(0).Item("PKG_17Z_SEGM_NBR")) Then
                dt.Rows(0).Item("PKG_17Z_SEGM_NBR") = ""
            Else
                dt.Rows(0).Item("PKG_17Z_SEGM_NBR") = Trim$(dt.Rows(0).Item("PKG_17Z_SEGM_NBR"))
            End If

            el = 2207
            If Convert.IsDBNull(dt.Rows(0).Item("CUSTOMER_NAME")) Then
                dt.Rows(0).Item("CUSTOMER_NAME") = ""
            Else
                dt.Rows(0).Item("CUSTOMER_NAME") = Trim$(dt.Rows(0).Item("CUSTOMER_NAME"))
            End If

            el = 2208
            If Convert.IsDBNull(dt.Rows(0).Item("CUSTOMER_STREET_ADDR")) Then
                dt.Rows(0).Item("CUSTOMER_STREET_ADDR") = ""
            Else
                dt.Rows(0).Item("CUSTOMER_STREET_ADDR") = Trim$(dt.Rows(0).Item("CUSTOMER_STREET_ADDR"))
            End If

            el = 2209
            If Convert.IsDBNull(dt.Rows(0).Item("CUSTOMER_CITY")) Then
                dt.Rows(0).Item("CUSTOMER_CITY") = ""
            Else
                dt.Rows(0).Item("CUSTOMER_CITY") = Trim$(dt.Rows(0).Item("CUSTOMER_CITY"))
            End If

            el = 2210
            If Convert.IsDBNull(dt.Rows(0).Item("ICS_CNTR_PACKAGE_TYPE")) Then
                dt.Rows(0).Item("ICS_CNTR_PACKAGE_TYPE") = ""
            Else
                dt.Rows(0).Item("ICS_CNTR_PACKAGE_TYPE") = Trim$(dt.Rows(0).Item("ICS_CNTR_PACKAGE_TYPE"))
            End If

            el = 2333
            If Not Convert.IsDBNull(dt.Rows(0).Item("Check_Sum")) Then
                dt.Rows(0).Item("CHECK_SUM") = Generate_test_digit(Trim$(dt.Rows(0).Item("Check_Sum")))
            Else
                dt.Rows(0).Item("CHECK_SUM") = ""
            End If

            'before leaving this function, change label record's machineID and time_used to the actual lineID and time printed
            el = 2334
            sqli.Remove(0, sqli.Length)
            sqli.Append("Update snapmgr.ics_label_data set machine_name = '" & labelreq.lmesMachID & "', time_used = sysdate")
            If useSErial Then
                sqli.Append(" where serial_nbr = '" & FindThis & "'")
            Else
                sqli.Append(" where machine_name = '" & FindThis & "'")
            End If
            cmd.CommandText = sqli.ToString
            el = 2335
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Commit"
            cmd.ExecuteNonQuery()

            'append the table to the dataset and get out of here.
            el = 2239
            ds.Tables.Add(dt) 'append the table
            el = 2341
            Return ds 'return data set

        Catch ex As Exception
            Throw New Exception("Failure in GETICSDAtA at line: " & el & "  " & ex.Message & " " & sqli.ToString)
        Finally
            Try
                dt.Dispose()
            Catch ex As Exception
            End Try
            Try
                ds.Dispose()
            Catch ex As Exception
            End Try
            Try
                cmd.Dispose()
            Catch ex As Exception
            End Try
            Try
                cnora.Close()
            Catch ex As Exception
            End Try
            Try
                cnora.Dispose()
            Catch ex As Exception
            End Try
        End Try
    End Function


    Private Function Generate_test_digit(ByVal sn As String) As Integer

        Dim k, j

        For j = 1 To Len(sn)
            k = k + Val(Mid$(sn, j, 1)) * (1 + (j - 1) Mod 2)
        Next j

        Generate_test_digit = k Mod 10

    End Function
    Public Function GetPartLineRestriction(ByVal lMesPartId As Long, ByVal lMesMachineId As Long, ByVal sContainer As String) As Boolean

        Dim MoCN As New OracleConnection
        Dim da As OracleDataAdapter
        Dim ds As New DataSet
        Dim sqlGetPartLineRestriction As String
        Dim sel, frm, whr As String
        Dim sqli As New StringBuilder
        Dim schema As String
        Dim cmd As New OracleCommand


        Try
            MoCN.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
            MoCN.Open()
            schema = GetSchemaName()
            sqli.Append("SELECT * FROM " & schema & "PART_MACHINE_RESTRICTION ")
            sqli.Append(" WHERE MES_PART_ID = " & lMesPartId & " and MACHINE_ID = " & lMesMachineId & " and CONTAINER_CODE = '" & sContainer.ToUpper & "'")

            With cmd
                .Connection = MoCN
                .CommandType = CommandType.Text
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With

            da = New OracleDataAdapter(cmd)
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                GetPartLineRestriction = True
            Else
                GetPartLineRestriction = False
            End If

        Catch ex As Exception
            Throw New Exception("Failure: with GetPartLineREstriction: " & ex.Message)
            GetPartLineRestriction = False
        Finally
            MoCN.Close()
            MoCN.Dispose()
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try

    End Function
    Private Function GenerateErrorLabel(ByVal PartNbr As String, ByVal MachineID As String, ByVal PKG_CODE As String, ByVal CNTR As String, ByVal MATID As String, ByVal ERR_DESCR As String, ByVal MESSAGE As String) As String
        Dim sr As StreamReader
        Dim line As String
        Dim tmp As New StringBuilder

        Try
            sr = New StreamReader(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "ApplicationPath") & ConfigurationSettings.AppSettings.Get("errorlabel"))
            Do
                line = sr.ReadLine()
                tmp.Append(line)
            Loop Until line Is Nothing

            tmp.Replace("%TIME%", TimeString)
            tmp.Replace("%DT%", DateString)
            tmp.Replace("%M%", MachineID)
            tmp.Replace("%PART%", PartNbr)
            tmp.Replace("%PKG%", PKG_CODE)
            tmp.Replace("%CNTR%", CNTR)
            tmp.Replace("%MATID%", MATID)
            tmp.Replace("%ERR%", ERR_DESCR)
            tmp.Replace("%MESSAGE%", MESSAGE)
            Return tmp.ToString
        Catch ex As Exception
            Throw New Exception("Failure GenerateErrorLabel: " & ex.Message)
        Finally
            sr.Close()
        End Try
    End Function
    <WebMethod(), SoapHeader("greeting")> _
    Public Function AbortSkid(ByVal SkidID As Long, ByVal PartNbr As String, ByVal CNTR As String)

        '11/16/05 Paw changed abortskid so that it rolls the schedule entrie to the bottom of the schedule instead
        '           of deleting it.  This complies with the may we manange skids
        If greeting Is Nothing Then
            Return "Please supply proper credentials"
        Else
            If greeting.UserID = ConfigurationSettings.AppSettings.Get("GreetingUserID") And greeting.Password = ConfigurationSettings.AppSettings.Get("GreetingPasscode") Then
                'youre in
            Else
                Return "Please supply proper credentials"
            End If
        End If

        Dim sqli As New StringBuilder
        Dim cnOra As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim retv As Boolean
        Dim nextval As Long
        Dim ttt As New StringBuilder
        Dim rets As String

        Try
            cnOra.Open()
            sqli.Append("select mesdba.ics_schedule_pool_seq.nextval from dual")
            With cmd
                .Connection = cnOra
                .CommandType = CommandType.Text
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                nextval = ds.Tables(0).Rows(0).Item("NextVal")
            Else
                Throw New Exception("Failure Aborting Skid:  Could not get nextval from Schedule_Pool Sequencer.")
            End If

            'retrieve some information for the email
            sqli.Remove(0, sqli.Length)
            sqli.Append("Select sp.*, m.Machine_Name from mesdba.ics_schedule_pool sp, mesdba.machine m where m.machine_id = sp.machine_id and sp.schedule_id = " & SkidID)
            With cmd
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With
            ds.Clear()
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                With ds.Tables(0).Rows(0)
                    ttt.Append(" Line:" & .Item("Machine_Name") & vbCrLf)
                    ttt.Append(" Aborted Skid:" & SkidID & vbCrLf)
                    ttt.Append(" PartNbr:" & PartNbr & vbCrLf)
                    ttt.Append(" Revision:" & .Item("Revision_Physical") & vbCrLf)
                    ttt.Append(" CNTR:" & CNTR & vbCrLf)

                    ttt.Append(" Build_Priority:" & .Item("build_priority") & vbCrLf)
                    ttt.Append(" Package Code:" & .Item("Package_Code") & vbCrLf)
                    ttt.Append(" Pkgs_Used_Qty:" & .Item("PKGS_USED_QTY") & vbCrLf)
                    ttt.Append(" at " & Now.Now & vbCrLf & vbCrLf)
                    ttt.Append(" Please Re-Schedule the replacement " & vbCrLf)
                End With
            End If

            'roll the schedule entry to botton of schedule
            sqli.Remove(0, sqli.Length)
            sqli.Append("Update mesdba.Ics_Schedule_Pool sp set sp.Schedule_Id = " & nextval & ", sp.Pkgs_Used_Qty = 0, sp.MACHINE_ID = null, sp.build_priority = 99")
            sqli.Append(" Where sp.schedule_id = " & SkidID)
            With cmd
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
                .CommandText = "Commit"
                .ExecuteNonQuery()
            End With

            '3 send notification to PC&L

            If Len(ttt.ToString) > 0 Then
                retv = pSendEmail("BEC_SPL Aborted SKID", "High", ttt.ToString, "PCL")
            End If

            '4 unreserve these serial numbers
            rets = UnReserveSerials(SkidID) 'pass the skid ID as the machine_Name because that is how they are reserved
            'ignore the return value of  UnReserveSerials, If this fails, it's not a life or death proposition, so let's not risk confusing the operator. (we will keep an eye on serial numbers, if this becomes an issue it will be addressed later)

            Return "Successfully Aborted Skid: " & SkidID
        Catch ex As Exception
            Return "Failure aborting Skid: " & SkidID & " " & ex.Message
        Finally
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
            cnOra.Close()
            cnOra.Dispose()
        End Try

    End Function
    <WebMethod(), SoapHeader("greeting")> _
        Public Function SendEmail(ByVal Subject As String, ByVal priority As String, ByVal Themessage As String, ByVal Recepients As String) As String

        If greeting Is Nothing Then
            Return "Please supply proper credentials"
        Else
            If greeting.UserID = ConfigurationSettings.AppSettings.Get("GreetingUserID") And greeting.Password = ConfigurationSettings.AppSettings.Get("GreetingPasscode") Then
                'youre in
            Else
                Return "Please supply proper credentials"
            End If
        End If

        Dim i As Int16
        Dim email As New System.Web.Mail.MailMessage
        Dim reciplist As String

        Try
            reciplist = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "Email_" & Recepients)
            If reciplist Is Nothing Then Throw New Exception("Failure: Sending Email => Invalid or empty recepient list")
            email.To = reciplist
            email.From = ConfigurationSettings.AppSettings.Get("Email_From")
            email.Body = Themessage
            email.Subject = Subject
            Select Case priority
                Case "high"
                    email.Priority = Mail.MailPriority.High
                Case "normal"
                    email.Priority = Mail.MailPriority.Normal
                Case Else
                    email.Priority = Mail.MailPriority.Normal
            End Select
            email.Subject = Subject
            email.BodyFormat = Web.Mail.MailFormat.Text
            System.Web.Mail.SmtpMail.Send(email)
            Return True
        Catch ex As Exception
            Return False & " " & ex.Message
        End Try

    End Function
    Private Function pSendEmail(ByVal Subject As String, ByVal priority As String, ByVal Themessage As String, ByVal Recepients As String) As String

        'This function is just like the sendemail web service but does not required the soap header greeting.

        Dim i As Int16
        Dim email As New System.Web.Mail.MailMessage
        Dim reciplist As String

        Try
            reciplist = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "Email_" & Recepients)
            If reciplist Is Nothing Then Throw New Exception("Failure: Sending Email => Invalid or empty recepient list")
            email.To = reciplist
            email.From = ConfigurationSettings.AppSettings.Get("Email_From")
            email.Body = Themessage
            email.Subject = Subject
            Select Case priority
                Case "high"
                    email.Priority = Mail.MailPriority.High
                Case "normal"
                    email.Priority = Mail.MailPriority.Normal
                Case Else
                    email.Priority = Mail.MailPriority.Normal
            End Select
            email.Subject = Subject
            email.BodyFormat = Web.Mail.MailFormat.Text
            System.Web.Mail.SmtpMail.Send(email)
            Return True
        Catch ex As Exception
            Return False & " " & ex.Message
        End Try

    End Function
    <WebMethod()> _
    Public Function GetMachineID(ByVal MachineName As String) As String

        Dim cn As OracleConnection

        Try
            cn = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
            cn.Open()
            Return ConvertFromMachineNbrtoMesMachineID(MachineName, cn)
        Catch ex As Exception
            Return "Failure in GetMAchineID: =>" & ex.Message
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Function
    <WebMethod(), SoapHeader("greeting")> _
    Public Function GetWhatsScheduled(ByVal PartNbr As String, ByVal CNTR As String) As DataSet
        If greeting Is Nothing Then
            Return Nothing
        Else
            If greeting.UserID = ConfigurationSettings.AppSettings.Get("GreetingUserID") And greeting.Password = ConfigurationSettings.AppSettings.Get("GreetingPasscode") Then
                'youre in
            Else
                Return Nothing
            End If
        End If

        Dim cn As New OracleConnection
        Dim da As OracleDataAdapter
        Dim ds As New DataSet
        Dim cmd As New OracleCommand
        Dim sql As New StringBuilder

        Try
            cn = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
            cn.Open()
            If CNTR = "" Then
                sql.Append("select mp.part_nbr as Part, sp.revision_physical as ECL, sp.container_code as CNTR, sp.build_priority as Priority, sp.package_code as PKG, sp.machine_id as Machine, sp.pkgs_per_skid_qty as Pkgs_per_skid, sp.pkgs_used_qty as Pkgs_Used, sp.matid, sp.mod_tmstm as Modified from mesdba.ics_schedule_pool sp, mesdba.mes_part mp where sp.mes_part_id = mp.mes_part_id and mp.part_nbr = '" & PartNbr & "' order by build_priority, schedule_id")
            Else
                sql.Append("select mp.part_nbr as Part, sp.revision_physical as ECL, sp.container_code as CNTR, sp.build_priority as Priority, sp.package_code as PKG, sp.machine_id as Machine, sp.pkgs_per_skid_qty as Pkgs_per_skid, sp.pkgs_used_qty as Pkgs_Used, sp.matid, sp.mod_tmstm as Modified from mesdba.ics_schedule_pool sp, mesdba.mes_part mp where sp.mes_part_id = mp.mes_part_id and mp.part_nbr = '" & PartNbr & "' and Container_Code = '" & CNTR & "' order by build_priority, schedule_id")
            End If
            With cmd
                .Connection = cn
                .CommandType = CommandType.Text
                .CommandText = sql.ToString
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count = 0 Then
                Return Nothing
            Else
                Return ds
            End If

        Catch ex As Exception
            Return Nothing
        Finally
            da.Dispose()
            ds.Dispose()
            cmd.Dispose()
            cn.Close()
            cn.Dispose()
        End Try

    End Function
    <WebMethod()> _
    Public Function GetSkidRecords(ByVal MachineName As String) As DataSet
        'This function will get the skid associated with this machine's current scheduled item
        'for the line.  
        'because process broadcast code clears all open schedule records, this procedure can safely assume that 
        'there is one schedule entrie and no more than one current skid

        Dim sqli As New StringBuilder
        Dim cn As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim cmd As New OracleCommand
        Dim da As OracleDataAdapter
        Dim ds As New DataSet

        Try
            cn.Open()
            sqli.Append("select sp.schedule_id, sp.container_code, lb.serial_nbr, lb.ins_tmstm from mesdba.ics_schedule_pool sp, mesdba.ics_label_buffer lb where sp.MACHINE_ID in (select m.machine_id from mesdba.machine m where m.MACHINE_NAME = '" & MachineName & "') and sp.schedule_id = lb.schedule_ID order by lb.serial_nbr")
            With cmd
                .Connection = cn
                .CommandText = sqli.ToString
                .CommandType = CommandType.Text
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            'not able to get the skid records for this machine
        Finally
            cn.Close()
            cn.Dispose()
            da.Dispose()
            cmd.Dispose()
            ds.Dispose()
        End Try
    End Function

    <WebMethod(), SoapHeader("greeting")> _
    Public Function ReprintICS(ByRef SerialNbrPack() As String) As String
        If greeting Is Nothing Then
            Return "Please supply proper credentials"
        Else
            If greeting.UserID = ConfigurationSettings.AppSettings.Get("GreetingUserID") And greeting.Password = ConfigurationSettings.AppSettings.Get("GreetingPasscode") Then
                'youre in
            Else
                Return "Please supply proper credentials"
            End If
        End If

        'This method will attempt to get the data from mesdba.standard_pack for the given serial number and reply with 
        'a reprint package
        '1 requires at least one serial number
        '2 connect to database
        '3 for each serial get the data
        '4 format the data using the template
        '5 return the data

        Dim retLabel As New StringBuilder
        Dim sqli As New StringBuilder
        Dim cnOra As OracleConnection
        Dim cmd As New OracleCommand
        Dim da As New OracleDataAdapter
        Dim dsICS_data As New DataSet
        Dim labelfile As String
        Dim theError As String
        Dim i As Integer

        Try
            'Open up a Connection to the DataBase
            If cnOra Is Nothing Then
                cnOra = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
                cnOra.Open()
            End If

            If UBound(SerialNbrPack) > 0 Then
                For i = 1 To UBound(SerialNbrPack)
                    sqli.Remove(0, sqli.Length)
                    sqli.Append("Select sp.*,sp.rev_physical as ECL,sp.gross_weight as Weight, sp.location_destination as destination, vw.dept,sp.Qty as Quantity,sp.descr as Description,  il.* from mesdba.standard_pack sp, mesdba.ics_label il, mesdba.vw_department vw where (sp.standard_pack_id = il.standard_pack_id)  and (vw.id = sp.report_group_id) and (sp.serial_nbr = '" & SerialNbrPack(i) & "')")
                    With cmd
                        .Connection = cnOra
                        .CommandType = CommandType.Text
                        .CommandText = sqli.ToString
                        .ExecuteNonQuery()
                    End With
                    da = New OracleDataAdapter(cmd)
                    da.Fill(dsICS_data)
                    If dsICS_data.Tables.Count > 0 Then
                        If dsICS_data.Tables(0).Rows.Count > 0 Then
                            'got the data
                            'determine if this was a generic label.  Because there does not seem to be a good field in the standard pack to determine this, we will check 
                            'for null data on the ref_seq_nbr  --a required field in the 2-D labels
                            dsICS_data.Tables(0).Columns.Add("std_flag", GetType(System.String))
                            If Convert.IsDBNull(dsICS_data.Tables(0).Rows(0).Item("ref_seq_nbr")) Then
                                dsICS_data.Tables(0).Rows(0).Item("std_flag") = "S"
                            Else
                                dsICS_data.Tables(0).Rows(0).Item("std_flag") = "I"
                            End If
                            labelfile = Nothing
                            labelfile = FormatOutput(dsICS_data, True)
                            retLabel.Append(labelfile)
                        Else
                            'no row returned for this one
                            Throw New Exception("Failure: Getting Reprint data for serial number: " & SerialNbrPack(i))
                        End If
                    Else
                        'something wrong, no table returned
                        Throw New Exception("Failure: with reprint.  Serial number " & SerialNbrPack(i))
                    End If
                Next i
                Return retLabel.ToString
            Else
                Return "Must supply at least one serial number"
            End If

        Catch ex As Exception
            If Not cmd.Transaction Is Nothing Then cmd.Transaction.Rollback()
            If ConfigurationSettings.AppSettings.Get("LogEvents") = "True" Then
                If LogEvents("Failure in ReprintICS " & ex.Message & Space(1) & ex.StackTrace) Then
                End If
            End If
            'LogErrors(ex.Message, "PrintLabel_P23", ex.Message)
            Return "Failure in ReprintICS " & ex.Message
        Finally
            If Not cnOra Is Nothing AndAlso cnOra.State = ConnectionState.Open Then cnOra.Close()
            cnOra.Dispose()
            dsICS_data.Dispose()
            cmd.Dispose()
            da.Dispose()
        End Try

    End Function

    <WebMethod(), SoapHeader("greeting")> _
    Public Function DeleteICSBuffer(ByRef SerialNbrPack() As String) As String
        If greeting Is Nothing Then
            Return "Please supply proper credentials"
        Else
            If greeting.UserID = ConfigurationSettings.AppSettings.Get("GreetingUserID") And greeting.Password = ConfigurationSettings.AppSettings.Get("GreetingPasscode") Then
                'youre in
            Else
                Return "Please supply proper credentials"
            End If
        End If

        Dim sqli As New StringBuilder
        Dim cnOra As OracleConnection
        Dim cmd As New OracleCommand
        Dim i As Integer

        Try
            'Open up a Connection to the DataBase
            If cnOra Is Nothing Then
                cnOra = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
                cnOra.Open()
            End If

            If UBound(SerialNbrPack) > 0 Then
                For i = 0 To UBound(SerialNbrPack) - 1
                    sqli.Remove(0, sqli.Length)
                    sqli.Append("Delete from mesdba.ICS_Label_buffer where Serial_Nbr  = '" & SerialNbrPack(i) & "'")
                    With cmd
                        .Connection = cnOra
                        .CommandType = CommandType.Text
                        .CommandText = sqli.ToString
                        .ExecuteNonQuery()
                    End With
                Next i
                Return "Successfully deleted " & UBound(SerialNbrPack) & " serials from skid!"
            End If

        Catch ex As Exception
            Throw New Exception("Failure deleting records from ICS_Label_Buffer for Serial_Nbr: " & SerialNbrPack(i))
            Return "Failure deleting from ICS_Label_buffer"
        Finally
            cmd.Dispose()
            cnOra.Close()
            cnOra.Dispose()
        End Try

    End Function

    <WebMethod(), SoapHeader("greeting")> _
    Public Function DeleteScheduledRecords(ByVal PartNbr As String, ByVal ECL As String, ByVal CNTR As String, ByVal PKG As String, ByVal MachineName As String, ByVal DeleteByMachineName As Boolean) As String
        If greeting Is Nothing Then
            Return "Please supply proper credentials"
        Else
            If greeting.UserID = ConfigurationSettings.AppSettings.Get("GreetingUserID") And greeting.Password = ConfigurationSettings.AppSettings.Get("GreetingPasscode") Then
                'youre in
            Else
                Return "Please supply proper credentials"
            End If
        End If

        Dim sqli As New StringBuilder
        Dim cnOra As OracleConnection
        Dim cmd As New OracleCommand
        Dim i As Integer
        Dim rets As String
        Dim da As New OracleDataAdapter(cmd)
        Dim ds As New DataSet
        Dim schedID As String

        Try
            'Open up a Connection to the DataBase
            If cnOra Is Nothing Then
                cnOra = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
                cnOra.Open()
            End If

            With cmd
                .Connection = cnOra
                .CommandType = CommandType.Text
            End With

            If DeleteByMachineName Then
                'Get the schedule ID so the serial numbers can be unreserved
                sqli.Append("Select Schedule_ID from mesdba.ics_schedule_pool where machine_id = " & MachineName)
                cmd.CommandText = sqli.ToString
                da.Fill(ds)
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        schedID = ds.Tables(0).Rows(0).Item("Schedule_ID")
                    Else
                        schedID = Nothing
                    End If
                Else
                    schedID = Nothing
                End If

                sqli.Remove(0, sqli.Length)
                sqli.Append("Delete from mesdba.ICS_schedule_pool sp where ")
                sqli.Append(" sp.Machine_ID  = " & MachineName)

            Else
                '2/21/2006  Because this section deletes schedule entries where the machine is null, there is no reason to
                '           unreserve serial numbers.  This is controlled by the schedID being nothing.
                sqli.Append("Delete from mesdba.ICS_schedule_pool sp where ")
                sqli.Append("mes_part_id in (select mes_part_id from mesdba.mes_part mp where mp.part_nbr = '" & PartNbr & "')")
                sqli.Append(" and sp.revision_physical = '" & ECL & "'")
                sqli.Append(" and sp.Container_code = '" & CNTR & "'")
                sqli.Append(" and sp.package_code = '" & PKG & "'")
                sqli.Append(" and sp.machine_id is null")
            End If

            With cmd
                'delete schedule entries
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
                .CommandText = "Commit"
                .ExecuteNonQuery()
            End With

            If schedID Is Nothing Then
                'do Not try to unreserve serials
            Else
                rets = UnReserveSerials(schedID) 'release any serial numbers 
            End If

            Return "Successfully deleted schedule entries!"

        Catch ex As Exception
            Throw New Exception("Failure deleting schedule entries")
            Return "Failure deleting schedule entries"
        Finally
            cmd.Dispose()
            cnOra.Close()
            cnOra.Dispose()
            da.Dispose()
            ds.Dispose()
        End Try

    End Function

    <WebMethod()> _
    Public Function GetMESPartId(ByVal psPartNumber As String, ByVal psUniqueFeature As String) As String
        'On Error GoTo GetMESPartIdErr
        Dim sMessage As String
        Dim sSource As String
        Dim sParameters As String
        Dim CmdGet_Mes_PartId As New OleDb.OleDbCommand
        Dim oDataAdapter = New OleDb.OleDbDataAdapter(CmdGet_Mes_PartId)
        Dim iMESPartId As Integer
        Dim iProcErrNum As Integer
        Dim sProcErrMsg As String
        Dim sProc As String
        Dim sbErrMsg As String
        Dim MyConn As New OleDb.OleDbConnection
        Dim Oparm As New OleDb.OleDbParameter

        Try
            ' open the strConn
            MyConn = New OleDb.OleDbConnection("Provider=MSDAORA.1;Password=mesfull;User ID=mesweb;Data Source=PROD.Fanta;Persist Security Info=True")
            MyConn.Open()
            ' set up the OleDb.OleDbCommand object
            With CmdGet_Mes_PartId
                .CommandText = "MESPROC.GET_MES_PART"
                .CommandType = CommandType.StoredProcedure
                .Connection = MyConn
                With .Parameters
                    .Clear()
                    .Add(New OleDb.OleDbParameter("part_nbr", OleDb.OleDbType.VarChar, 30, ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PART_NBR", DataRowVersion.Current, psPartNumber.Trim))
                    .Add(New OleDb.OleDbParameter("unique_feature", OleDb.OleDbType.VarChar, 215, ParameterDirection.Input, True, CType(0, Byte), CType(0, Byte), "UNIQUE_FEATURE", DataRowVersion.Current, IIf(psUniqueFeature.Trim.Equals(String.Empty), Convert.DBNull, psUniqueFeature.Trim)))
                    .Add(New OleDb.OleDbParameter("mes_part_id", OleDb.OleDbType.Numeric, 0, ParameterDirection.ReturnValue, True, 0, 0, "MES_PART_ID", DataRowVersion.Current, iMESPartId))
                    .Add(New OleDb.OleDbParameter("err_num", OleDb.OleDbType.Numeric, 0, ParameterDirection.ReturnValue, True, CType(0, Byte), CType(0, Byte), "", DataRowVersion.Current, iProcErrNum))
                    .Add(New OleDb.OleDbParameter("err_msg", OleDb.OleDbType.VarChar, 100, ParameterDirection.ReturnValue, True, CType(0, Byte), CType(0, Byte), "", DataRowVersion.Current, sProcErrMsg))
                    .Add(New OleDb.OleDbParameter("err_proc", OleDb.OleDbType.VarChar, 100, ParameterDirection.ReturnValue, True, CType(0, Byte), CType(0, Byte), "", DataRowVersion.Current, sProc))
                End With ' .Parameters
            End With ' CmdGet_Mes_PartId
            ' execute the stored procedure
            CmdGet_Mes_PartId.ExecuteNonQuery()
            If CmdGet_Mes_PartId.Parameters("mes_part_id").Value & "" = "" Then
                If (CmdGet_Mes_PartId.Parameters("err_num").Value) <> 0 Then
                    sbErrMsg = "Oracle Error Number: "
                    sbErrMsg = sbErrMsg & CmdGet_Mes_PartId.Parameters("err_num").Value
                    sbErrMsg = sbErrMsg & Chr(13)
                    sbErrMsg = sbErrMsg & "Message: "
                    sbErrMsg = sbErrMsg & CmdGet_Mes_PartId.Parameters("err_msg").Value
                    sParameters = sParameters & "psPartNumber='" & psPartNumber & "',"
                    sParameters = sParameters & "psUniqueFeature='" & psUniqueFeature & "'"
                    GetMESPartId = "Part: " & CmdGet_Mes_PartId.Parameters("part_id").Value & sbErrMsg
                End If
            Else
                Return CmdGet_Mes_PartId.Parameters("mes_part_id").Value()
                'Response.Write(GetMESPartId)
            End If
        Catch oException As System.Exception
            sMessage = oException.Message
            sSource = oException.Source & ":Engineering:Part:GetMESPartId"
            sParameters = sParameters & "psPartNumber='" & psPartNumber & "',"
            sParameters = sParameters & "psUniqueFeature='" & psUniqueFeature & "'"
            Return "Failure in GetMESPartID: " & oException.Message
        Finally
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
                MyConn.Dispose()
            End If
        End Try
    End Function

    <WebMethod()> Public Function ShowVersion()
        Dim v As New System.Version
        Return v.Build & Space(2) & v.Major & Space(2) & v.Minor & Space(2)
    End Function

    <WebMethod()> _
    Public Function EnterLog(ByVal LogType As logTypes, ByVal logMessage As String)

        Dim logpath As String
        Try
            Select Case LogType
                Case logTypes.AbortSkid
                    logpath = ConfigurationSettings.AppSettings.Get("AbortSkidLogPath")
                Case logTypes.DeleteSchedule
                    logpath = ConfigurationSettings.AppSettings.Get("DeleteScheduleLogPath")
                Case logTypes.Reprint
                    logpath = ConfigurationSettings.AppSettings.Get("ReprintLogPath")
            End Select
            Dim sw As New StreamWriter(logpath)
            sw.WriteLine(logMessage)
            sw.Close()
            Return "Success:"

        Catch ex As Exception
            Return "Failure: Logging Error =>" & ex.Message
        Finally
        End Try
    End Function

    <WebMethod()> _
    Public Function SaveInpectionRecord(ByRef ds As DataSet) As String
        Dim dot As New dotMESQualityClass.mesQuality(Common.GetOracleConnectionString("OracleConnection"))
        Dim retv As String
        Try
            'get the connection string
            retv = dot.WriteInspectionRecord(ds)
            Return retv
        Catch ex As Exception
            Return "Failure in SaveInspectionRecord when making call to WriteInspectionRecord: " & retv
        Finally
            dot = Nothing
        End Try
    End Function

    <WebMethod()> _
    Public Function GetQualityProblemCodes(ByRef Result1 As String) As DataSet
        Dim cnora As New OracleConnection
        Dim dot As dotMESQualityClass.mesQuality
        Dim ds As New DataSet
        Try
            dot = New dotMESQualityClass.mesQuality(Common.GetOracleConnectionString("OracleConnection"))
            dot.LanguageCode = "en-us"
            ds = dot.GetProblemCodes("", Result1)
            Return ds
        Catch ex As Exception
            Result1 = "Failure GetQualityProblemCodes: " & ex.Message
        Finally
            Try
                ds.Dispose()
            Catch ex As Exception
            End Try
            Try
                cnora.Close()
                cnora.Dispose()
            Catch ex As Exception
            End Try
        End Try
    End Function

    <WebMethod()> _
    Public Function RunsAt() As String
        On Error Resume Next
        Return Common.GetServerType
    End Function

    <WebMethod(), SoapHeader("greeting")> _
        Public Function UnReserveSerials(ByVal Machine_Name As String) As String
        '2/20/2006  This function is created to accompany the Abort Skid and Delete Schedule Records functions.
        '           Basically what this does, is, when an active schedule entrie is deleted, by designe, there will
        '           be reserved serial numbers for that schedule entrie.  These labels need to be released back to usable
        '           so they can be used, since they are not being used.
        '           There are two possible ways to handle these (1- purge them; 2- release them for future use)
        '           both ways have their pros and cons.  By purging them, you waste them and repeated aborts could run you 
        '           out of labels.
        '           By releasing them there is a slim chance that they might cause confusion
        '           This function returns either the word Success of Failure followed by an error message

        If greeting Is Nothing Then
            Return "Please supply proper credentials"
        Else
            If greeting.UserID = ConfigurationSettings.AppSettings.Get("GreetingUserID") And greeting.Password = ConfigurationSettings.AppSettings.Get("GreetingPasscode") Then
                'youre in
            Else
                Return "Please supply proper credentials"
            End If
        End If

        Dim cn As New OracleConnection
        Dim cmd As New OracleCommand
        Dim sqli As New StringBuilder
        Try
            cn = New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnectionICS"))
            cn.Open()
            sqli.Append("Update pltfloor.ics_label_data_vw set used_status = 0, machine_name = null, time_used = null ")
            sqli.Append(" where (used_status  = 1) and machine_name like '" & Machine_Name & "%'")

            With cmd
                .Connection = cn
                .CommandType = CommandType.Text
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
                .CommandText = "Commit"
                .ExecuteNonQuery()
            End With
            Return "Success"

        Catch ex As Exception
            Return "Failure UnReserving Serials: " & ex.Message
        Finally
            cn.Close()
            cn.Dispose()
            cmd.Dispose()
        End Try

    End Function

End Class
