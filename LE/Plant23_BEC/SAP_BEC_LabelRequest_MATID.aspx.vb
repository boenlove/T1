Imports System.Data.OracleClient
'Imports Oracle.ManagedDataAccess.Client

Partial Class SAP_BEC_LabelRequest_MATID

    Inherits System.Web.UI.Page
    Public llRECORD_ID As Long
    Public lsSTD_FLAG As String
    Public lsPART_NBR As String
    Public lsFORM_CODE As String
    Public lsDESTINATION As String
    Public liQUANTITY As Integer
    Public liWEIGHT As Integer
    Public lsECL As String
    Public lsSERIAL_NBR As String
    Public lsDEPT As String
    Public lsPACKAGE_CODE As String
    Public lsPAY_SUFFIX As String
    Public liMATID As Integer
    Public lsDESCRIPTION As String
    Public lsWAREHOUSE_LOC As String
    Public lsCUSTOMER_CODE As String
    Public lsCUSTOMER_PART_NBR As String
    Public lsBROADCAST_CODE As String
    Public lsDELIVERY_LOC_NBR As String
    Public lsREF_SEQ_DATA_ID As String
    Public lsREF_SEQ_NBR As String
    Public lsPKG_12Z_SEGM_NBR As String
    Public lsPKG_13Z_SEGM_NBR As String
    Public lsPKG_14Z_SEGM_NBR As String
    Public lsPKG_15Z_SEGM_NBR As String
    Public lsPKG_16Z_SEGM_NBR As String
    Public lsPKG_17Z_SEGM_NBR As String
    Public lsCUSTOMER_NAME As String
    Public lsCUSTOMER_STREET_ADDR As String
    Public lsCUSTOMER_CITY As String
    Public lsICS_CNTR_PACKAGE_TYPE As String
    Public liCHECK_SUM As Integer
    Public liUSED_STATUS As Integer
    Public lsTIME_USED As String
    Public ldTIME_USED As Date
    Public lsMACHINE_NAME As String
    Public lsMOD_USERID As String
    Public lsMOD_TMSTM As String
    Public ldMOD_TMSTM As Date
    Public lsCUSTOMER_FILLER As String
    Public lsCUSTOMER_PN_FILLER As String
    Public lsCISCO_CODE As String
    Public lsREQ_STORE As String
    Public lsSHIP_TO As String

    Public liSAP_STD_PACK_QTY As Integer
    Public liICS_STD_PACK_QTY As Integer
    Public liICS_RecordsToCreate As Integer
    Public liICS_RecordsCreated As Integer
    Public lsMaterial_Nbr As String
    'Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Public lbDataError As Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lsCUSTOMER_NAME = Session("CUSTOMER_NAME")
        liMATID = Session("MATID")
        Dim x As String = GetServerType()
        If x = "LOCAL." Or x = "TEST." Then
            lblDisplay.Visible = True
            lblDisplay.Text = "TEST ENVIRONMENT"
        End If
        'txtSecurityCheck.Text = GetSecurity(UCase(Right(Request.ServerVariables("LOGON_USER"), 6)))
        txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23"
        If txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23" Or txtSecurityCheck.Text = "BEC_ICS_WEB_USER_P23" Then
            If IsPostBack = False Then
                CleanVerificationFields()
                Me.cmbSTD_FLAG.SelectedValue = "I"
                'Dim i As Integer
                'For i = 1 To 120
                '    Dim liNextSerial As Int64 = GetNextSerialViaSequencerNew()
                '    liNextSerial = liNextSerial
                '    lstbxInsertResults.Items.Add(liNextSerial)
                'Next i
            End If
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If

    End Sub
    Sub InsertICSLabelData()
        Dim lsSQL As String
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim sContainerType As String = ""
        Dim lMachineID As Long = 0
        Dim lResetCounters As Long = 0
        Dim dr As DataRow
        Dim lCurrentShiftQty As Long = 0
        Dim shiftDate As Date
        Dim shiftID As Int32
        Dim bMoveToHistory As Boolean
        Dim lsSQL1 As String
        Dim lsSQL2 As String
        lsCUSTOMER_PN_FILLER = " "
        If txtSTD_FLAG.Text = "S" Then
            lsSTD_FLAG = "S"
            lsDESTINATION = ""
            lsFORM_CODE = ""
            lsCUSTOMER_CODE = ""
            lsCUSTOMER_PART_NBR = Session("MATID_CUSTOMER_PART_NBR")
            lsCUSTOMER_PART_NBR = lsCUSTOMER_PART_NBR
            lsBROADCAST_CODE = ""
            lsDELIVERY_LOC_NBR = ""
            lsREF_SEQ_DATA_ID = ""
            lsREF_SEQ_NBR = ""
            lsPKG_12Z_SEGM_NBR = ""
            lsPKG_13Z_SEGM_NBR = ""
            lsPKG_14Z_SEGM_NBR = ""
            lsPKG_15Z_SEGM_NBR = ""
            lsPKG_16Z_SEGM_NBR = ""
            lsPKG_17Z_SEGM_NBR = ""
            lsCUSTOMER_NAME = ""
            lsCUSTOMER_STREET_ADDR = ""
            lsCUSTOMER_CITY = ""
            lsICS_CNTR_PACKAGE_TYPE = ""
            lsCUSTOMER_FILLER = "generic" ' can not be upper case since the Kiosk checks for lowercase
            'lsCUSTOMER_PN_FILLER = ""
        End If

        Try
            lsSQL1 = "INSERT INTO SNAPMGR.ICS_LABEL_DATA (" & _
            " RECORD_ID, " & _
            " STD_FLAG, " & _
            " PART_NBR, " & _
            " FORM_CODE, " & _
            " DESTINATION, " & _
            " QUANTITY, " & _
            " WEIGHT, " & _
            " ECL, " & _
            " SERIAL_NBR, " & _
            " DEPT, " & _
            " PACKAGE_CODE, " & _
            " PAY_SUFFIX, " & _
            " MATID, " & _
            " DESCRIPTION, " & _
            " WAREHOUSE_LOC, " & _
            " CUSTOMER_CODE, " & _
            " CUSTOMER_PART_NBR, " & _
            " BROADCAST_CODE, " & _
            " DELIVERY_LOC_NBR, " & _
            " REF_SEQ_DATA_ID, " & _
            " REF_SEQ_NBR, " & _
            " PKG_12Z_SEGM_NBR, " & _
            " PKG_13Z_SEGM_NBR, " & _
            " PKG_14Z_SEGM_NBR, " & _
            " PKG_15Z_SEGM_NBR, " & _
            " PKG_16Z_SEGM_NBR, " & _
            " PKG_17Z_SEGM_NBR, " & _
            " CUSTOMER_NAME, " & _
            " CUSTOMER_STREET_ADDR, " & _
            " CUSTOMER_CITY, " & _
            " ICS_CNTR_PACKAGE_TYPE, " & _
            " CHECK_SUM, " & _
            " USED_STATUS, " & _
            " TIME_USED, " & _
            " MACHINE_NAME, " & _
            " MOD_USERID, " & _
            " MOD_TMSTM, " & _
            " CUSTOMER_FILLER, " & _
            " CUSTOMER_PN_FILLER, " & _
            " CISCO_CODE, " & _
            " REQ_STORE) " & _
            " VALUES ( "

            lsSQL2 = "'" & llRECORD_ID & "'," & _
"'" & lsSTD_FLAG & "'," & _
"'" & lsPART_NBR & "'," & _
"'" & lsFORM_CODE & "'," & _
"'" & lsDESTINATION & "'," & _
"'" & liQUANTITY & "'," & _
"'" & liWEIGHT & "'," & _
"'" & lsECL & "'," & _
"'" & lsSERIAL_NBR & "'," & _
"'" & lsDEPT & "'," & _
"'" & lsPACKAGE_CODE & "'," & _
"'" & lsPAY_SUFFIX & "'," & _
"'" & liMATID & "'," & _
"'" & lsDESCRIPTION & "'," & _
"'" & lsWAREHOUSE_LOC & "'," & _
"'" & lsCUSTOMER_CODE & "'," & _
"'" & lsCUSTOMER_PART_NBR & "'," & _
"'" & lsBROADCAST_CODE & "'," & _
"'" & lsDELIVERY_LOC_NBR & "'," & _
"'" & lsREF_SEQ_DATA_ID & "'," & _
"'" & lsREF_SEQ_NBR & "'," & _
"'" & lsPKG_12Z_SEGM_NBR & "'," & _
"'" & lsPKG_13Z_SEGM_NBR & "'," & _
"'" & lsPKG_14Z_SEGM_NBR & "'," & _
"'" & lsPKG_15Z_SEGM_NBR & "'," & _
"'" & lsPKG_16Z_SEGM_NBR & "'," & _
"'" & lsPKG_17Z_SEGM_NBR & "'," & _
"'" & lsCUSTOMER_NAME & "'," & _
"'" & lsCUSTOMER_STREET_ADDR & "'," & _
"'" & lsCUSTOMER_CITY & "'," & _
"'" & lsICS_CNTR_PACKAGE_TYPE & "'," & _
"'" & liCHECK_SUM & "'," & _
"'" & liUSED_STATUS & "'," & _
"'" & lsTIME_USED & "'," & _
"'" & lsMACHINE_NAME & "'," & _
"'" & lsMOD_USERID & "'," & _
"'" & lsMOD_TMSTM & "'," & _
"'" & lsCUSTOMER_FILLER & "'," & _
"'" & lsCUSTOMER_PN_FILLER & "'," & _
"'" & lsCISCO_CODE & "'," & _
"'" & lsREQ_STORE & "')"

            lsSQL = lsSQL1 + lsSQL2


            'Open the connection if it's closed
            Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = lsSQL
                .ExecuteNonQuery()
            End With
            liICS_RecordsCreated = liICS_RecordsCreated + 1
            Me.lstbxInsertResults.Items.Add("Inserted: " & Me.liICS_RecordsCreated & " - Serial #: " & lsSERIAL_NBR & " Part # " & lsPART_NBR)
        Catch exole As OracleException
            Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()

        End Try
    End Sub
    Sub ReadmaxSerialNumber()
        Dim lsSQL As String
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim sContainerType As String = ""
        Dim lMachineID As Long = 0
        Dim lResetCounters As Long = 0
        'Dim dr As DataRow
        Dim lCurrentShiftQty As Long = 0
        Dim shiftDate As Date
        Dim shiftID As Int32
        Dim bMoveToHistory As Boolean
        Try
            lsSQL = "SELECT max(SERIAL_NBR) AS MAXSERIAL, max(RECORD_ID) as MAXRECORDID FROM SNAPMGR.ICS_LABEL_DATA"

            Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = lsSQL
                .ExecuteNonQuery()
            End With
            Dim dr As OracleDataReader = cmd.ExecuteReader() ' VB.NET
            dr.Read()
            lsSERIAL_NBR = dr.Item("MAXSERIAL")
            llRECORD_ID = dr.Item("MAXRECORDID")

            Dim liSerial_NBR As Long
            liSerial_NBR = CLng(lsSERIAL_NBR + 1)
            lsSERIAL_NBR = liSerial_NBR.ToString
            llRECORD_ID = llRECORD_ID + 1
        Catch exole As OracleException
            Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
        End Try
    End Sub
    Sub UpdateICS_MATID_with_SHIP_TO()
        Dim lsSQL As String
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cmd As New OracleCommand
        Dim ds As New DataSet

        Dim SQL As String = ""
        Dim sContainerType As String = ""
        Dim lMachineID As Long = 0
        Dim lResetCounters As Long = 0
        'Dim dr As DataRow
        Dim lCurrentShiftQty As Long = 0
        Dim shiftDate As Date
        Dim shiftID As Int32
        Dim bMoveToHistory As Boolean
        Try
            lsSQL = "update pltfloor.ICS_MATID set SHIP_TO = '" & lblSHIP_TO.Text & "' where "
            lsSQL = lsSQL + " MATID = " & CInt(txtMATID.Text)

            Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = lsSQL
                .ExecuteNonQuery()
            End With
            'Dim dr As OracleDataReader = cmd.ExecuteReader() ' VB.NET
            'dr.Read()
            'lsSERIAL_NBR = dr.Item("MAXSERIAL")
            'llRECORD_ID = dr.Item("MAXRECORDID")

            'Dim liSerial_NBR As Long
            'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
            'lsSERIAL_NBR = liSerial_NBR.ToString
            llRECORD_ID = llRECORD_ID + 1
        Catch exole As OracleException
            Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
        End Try
    End Sub
    Sub ReadSAPDataByPartCustAndLoadFields()
        Dim lsSQL As String
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim sContainerType As String = ""
        Dim lMachineID As Long = 0
        Dim lResetCounters As Long = 0
        'Dim dr As DataRow
        Dim lCurrentShiftQty As Long = 0
        Dim shiftDate As Date
        Dim shiftID As Int32
        Dim bMoveToHistory As Boolean
        liICS_RecordsCreated = 0
        Me.liICS_RecordsToCreate = 0
        lsCUSTOMER_NAME = Session("CUSTOMER_NAME")
        Try
            If lblSHIP_TO.Text.Length > 5 Then
                'lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '12040756'"
                lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '" & txtPart_Nbr.Text & "'"
                lsSQL = lsSQL + " and SHIP_TO = '" & Me.lblSHIP_TO.Text & "'"
            Else
                lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '" & txtPart_Nbr.Text & "'"
                lsSQL = lsSQL + " and cust_name_1 = '" & lsCUSTOMER_NAME & "'"
            End If
            Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = lsSQL
                .ExecuteNonQuery()
            End With
            Dim dr As OracleDataReader = cmd.ExecuteReader() ' VB.NET
            dr.Read()

            lbDataError = False

            liSAP_STD_PACK_QTY = dr.Item("STD_PACK_QTY") 'TODO comes from SAP or ICS?
            lsPART_NBR = dr.Item("MATERIAL_NBR")
            lsDESCRIPTION = (IIf(IsDBNull(dr.Item("MATERIAL_DESC")), "", dr.Item("MATERIAL_DESC")))
            lsCUSTOMER_NAME = (IIf(IsDBNull(dr.Item("CUST_NAME_1")), "", dr.Item("CUST_NAME_1")))
            If lsCUSTOMER_NAME.Length > 29 Then
                lsCUSTOMER_NAME = lsCUSTOMER_NAME.Substring(0, 29)
            End If
            Session("CUSTOMER_NAME") = lsCUSTOMER_NAME
            lsCUSTOMER_STREET_ADDR = (IIf(IsDBNull(dr.Item("CUST_ADDRESS")), "", dr.Item("CUST_ADDRESS")))
            lblCUST_ADDRESS.Text = lsCUSTOMER_STREET_ADDR
            lsCUSTOMER_CITY = (IIf(IsDBNull(dr.Item("CUST_CITY")), "", dr.Item("CUST_CITY")))
            lblCUST_CITY.Text = lsCUSTOMER_CITY
            lblCUST_STATE.Text = (IIf(IsDBNull(dr.Item("CUST_STATE")), "", dr.Item("CUST_STATE")))
            'lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("STD_PACK_CNTR")), 0, dr.Item("STD_PACK_CNTR")))
            'lsCUSTOMER_FILLER = (IIf(IsDBNull(dr.Item("SALES_DOC_NBR")), 0, dr.Item("SALES_DOC_NBR")))
            lsCUSTOMER_FILLER = (IIf(IsDBNull(dr.Item("SHIP_TO")), " ", dr.Item("SHIP_TO")))
            lblSHIP_TO.Text = lsCUSTOMER_FILLER

            'lsDELIVERY_LOC_NBR = (IIf(IsDBNull(dr.Item("UNLOADING_POINT")), "", dr.Item("UNLOADING_POINT")))
            lsDELIVERY_LOC_NBR = (IIf(IsDBNull(dr.Item("PLANT_DOCK_LINE_1")), "", dr.Item("PLANT_DOCK_LINE_1")))
            If lsDELIVERY_LOC_NBR.Length > 10 Then
                lsDELIVERY_LOC_NBR = lsDELIVERY_LOC_NBR.Substring(0, 10)
            End If
            If lsDELIVERY_LOC_NBR.Length < 1 Then
                lbDataError = True
            End If

            'TODO GROSS OR NET WEIGHT?:
            liWEIGHT = (IIf(IsDBNull(dr.Item("GROSS_WGT")), 0, dr.Item("GROSS_WGT")))
            Dim ldWeight As Double
            ldWeight = (IIf(IsDBNull(dr.Item("GROSS_WGT")), 0, dr.Item("GROSS_WGT")))
            lsPKG_14Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PLANT_DOCK_LINE_3")), "", dr.Item("PLANT_DOCK_LINE_3")))
            lsPKG_15Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PLANT_DOCK_LINE_4")), "", dr.Item("PLANT_DOCK_LINE_4")))
            lsREF_SEQ_NBR = (IIf(IsDBNull(dr.Item("PURCHASE_ORDER")), "", dr.Item("PURCHASE_ORDER")))
            'lsPKG_12Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("DEST_DOCK_LOC")), "", dr.Item("DEST_DOCK_LOC")))
            lsPKG_12Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("DEST_LOC")), "", dr.Item("DEST_LOC")))
            lsPKG_17Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("MATERIAL_DESC")), "", dr.Item("MATERIAL_DESC")))
            lsPKG_14Z_SEGM_NBR = lsPKG_14Z_SEGM_NBR.Replace("  ", " ")
            If lsPKG_14Z_SEGM_NBR.Length > 19 Then
                lsPKG_14Z_SEGM_NBR = lsPKG_14Z_SEGM_NBR.Substring(0, 19)
            End If
            If lsPKG_15Z_SEGM_NBR.Length > 19 Then
                lsPKG_15Z_SEGM_NBR = lsPKG_15Z_SEGM_NBR.Substring(0, 19)
            End If
            If lsPKG_17Z_SEGM_NBR.Length > 9 Then
                lsPKG_17Z_SEGM_NBR = lsPKG_17Z_SEGM_NBR.Substring(0, 9)
            End If
        Catch exole As OracleException
            Me.lblDescription.Text = "Part Not found in SAP File"
            Throw New Exception(exole.Message)
            Me.lblDescription.Text = "Part Not found in SAP File"
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
            'ReadHIST_ICS_Label_Data()
        End Try
    End Sub
    Sub GetCustomersFromSAP_Label_DataByPart()
        Dim lsSQL As String
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim sContainerType As String = ""
        Dim lMachineID As Long = 0
        Dim lResetCounters As Long = 0
        'Dim dr As DataRow
        Dim lCurrentShiftQty As Long = 0
        Dim shiftDate As Date
        Dim shiftID As Int32
        Dim bMoveToHistory As Boolean
        liICS_RecordsCreated = 0
        Me.liICS_RecordsToCreate = 0
        Dim lsICSCity As String = Session("ICS_CUSTOMER_CITY")
        lblPlant_Dock_Line_1_H.Text = "Mat H. Code:"
        lblDEST_LOC_H.Text = "Dest. Code:"
        Try
            'lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '12040756'"
            If lblFoundShipTo.Text = "YES" Then
                lsSQL = "select CUST_NAME_1,MATERIAL_DESC,dest_loc, PLANT_DOCK_LINE_1 FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '" & txtPart_Nbr.Text & "'"
                lsSQL = lsSQL + " and   SHIP_TO = '" & lblSHIP_TO.Text & "'"
                lsSQL = lsSQL + " group by CUST_NAME_1,MATERIAL_DESC,dest_loc, PLANT_DOCK_LINE_1 "
            Else
                lsSQL = "select CUST_NAME_1,MATERIAL_DESC,dest_loc, PLANT_DOCK_LINE_1  FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '" & txtPart_Nbr.Text & "'"
                lsSQL = lsSQL + " and   CUST_CITY like '%" & lsICSCity & "%'"
                lsSQL = lsSQL + " group by CUST_NAME_1,MATERIAL_DESC,dest_loc, PLANT_DOCK_LINE_1 "
            End If
            Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = lsSQL
                .ExecuteNonQuery()
            End With
            Dim dr As OracleDataReader = cmd.ExecuteReader() ' VB.NET
            lsCUSTOMER_NAME = " "
            cmbSAP_Customer.Items.Clear()
            cmbSAP_Customer.Items.Add("Select Customer")
            'cmbSAP_Customer.Items.Add("Select Customer Name..")
            While dr.Read()
                '  liSAP_STD_PACK_QTY As Integer
                '  liICS_STD_PACK_QTY As Integer

                'liSAP_STD_PACK_QTY = dr.Item("STD_PACK_QTY")
                'lsPART_NBR = dr.Item("MATERIAL_NBR")
                lsDESCRIPTION = (IIf(IsDBNull(dr.Item("MATERIAL_DESC")), "", dr.Item("MATERIAL_DESC")))
                lblDescription.Text = lsDESCRIPTION
                lsCUSTOMER_NAME = (IIf(IsDBNull(dr.Item("CUST_NAME_1")), "", dr.Item("CUST_NAME_1")))
                lblPlant_Dock_Line_1.Text = (IIf(IsDBNull(dr.Item("PLANT_DOCK_LINE_1")), "", dr.Item("PLANT_DOCK_LINE_1")))
                Session("CUSTOMER_NAME") = lsCUSTOMER_NAME
                If lsCUSTOMER_NAME.Length > 29 Then
                    lsCUSTOMER_NAME = lsCUSTOMER_NAME.Substring(0, 29)
                End If
                lblDEST_LOC.Text = (IIf(IsDBNull(dr.Item("dest_loc")), "", dr.Item("dest_loc")))
                'lsCUSTOMER_CITY = (IIf(IsDBNull(dr.Item("CUST_CITY")), "", dr.Item("CUST_CITY")))
                'lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("STD_PACK_CNTR")), 0, dr.Item("STD_PACK_CNTR")))
                cmbSAP_Customer.Items.Add(lsCUSTOMER_NAME)

                'Dim liSerial_NBR As Long
                'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
                'lsSERIAL_NBR = liSerial_NBR.ToString
                llRECORD_ID = llRECORD_ID + 1

            End While

        Catch exole As OracleException
            Me.lblDescription.Text = "Part Not found in SAP File.."
            Throw New Exception(exole.Message)
            Me.lblDescription.Text = "Part Not found in SAP File.."
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
            If llRECORD_ID = 1 Then
                ReadSAPDataByPartCustAndLoadFields()
                cmbSAP_Customer.SelectedValue = lsCUSTOMER_NAME
                If Me.lbDataError = True Then
                    If lblPlant_Dock_Line_1.Text.Length = 0 Then
                        lblStatus.Visible = True
                        lblStatus.ForeColor = Color.Red
                        ShowSAPFields()
                        lblStatus.Text = "This Part Number/Destination does NOT have Material Handling Code information.."
                        lstbxInsertResults.Items.Add(lblStatus.Text)
                        Me.btnCreateLabelsFromSAP.Visible = False
                    End If
                    If Me.lblDEST_LOC.Text.Length = 0 Then
                        lblStatus.Visible = True
                        lblStatus.ForeColor = Color.Red
                        ShowSAPFields()
                        lblStatus.Text = "This Part Number/Destination does NOT have Plant Dock information.."
                        lstbxInsertResults.Items.Add(lblStatus.Text)
                        Me.btnCreateLabelsFromSAP.Visible = False
                    End If
                Else
                    Me.btnCreateLabelsFromSAP.Visible = True
                    lblStatus.ForeColor = Color.Black
                    lblStatus.Text = " "
                    ShowVerificationFields()
                    ShowSAPFields()
                End If
            End If
        End Try
    End Sub
    Sub GetSAP_Label_DataInfo()
        Dim lsSQL As String
        'Dim conString As String = GetConnectionString()
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim sContainerType As String = ""
        Dim lMachineID As Long = 0
        Dim lResetCounters As Long = 0
        'Dim dr As DataRow
        Dim lCurrentShiftQty As Long = 0
        Dim shiftDate As Date
        Dim shiftID As Int32
        Dim bMoveToHistory As Boolean
        liICS_RecordsCreated = 0
        Me.liICS_RecordsToCreate = 0
        Try
            lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '" & lsMaterial_Nbr & "'"

            Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = lsSQL
                .ExecuteNonQuery()
            End With
            Dim dr As OracleDataReader = cmd.ExecuteReader() ' VB.NET
            dr.Read()
            '  liSAP_STD_PACK_QTY As Integer
            '  liICS_STD_PACK_QTY As Integer
            liSAP_STD_PACK_QTY = dr.Item("STD_PACK_QTY")
            lsPART_NBR = dr.Item("MATERIAL_NBR")
            lsDESCRIPTION = (IIf(IsDBNull(dr.Item("MATERIAL_DESC")), "", dr.Item("MATERIAL_DESC")))
            lblDescription.Text = lsDESCRIPTION
            lsCUSTOMER_NAME = (IIf(IsDBNull(dr.Item("CUST_NAME_1")), "", dr.Item("CUST_NAME_1")))
            Session("CUSTOMER_NAME") = lsCUSTOMER_NAME
            lsCUSTOMER_STREET_ADDR = (IIf(IsDBNull(dr.Item("CUST_ADDRESS")), "", dr.Item("CUST_ADDRESS")))
            lsCUSTOMER_CITY = (IIf(IsDBNull(dr.Item("CUST_CITY")), "", dr.Item("CUST_CITY")))
            'lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("STD_PACK_CNTR")), 0, dr.Item("STD_PACK_CNTR")))

            Dim liSerial_NBR As Long
            liSerial_NBR = CLng(lsSERIAL_NBR + 1)
            lsSERIAL_NBR = liSerial_NBR.ToString
            llRECORD_ID = llRECORD_ID + 1
            'ReadHIST_ICS_Label_Data()
        Catch exole As OracleException
            Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
            'ReadHIST_ICS_Label_Data()
        End Try
    End Sub
    Sub LoadFields()
        '        lsSTD_FLAG = Session("STD_FLAG")
        '        lsDESTINATION = Me.txtDestination.Text
        '        liQUANTITY = Me.txtQuantity.Text
        '        liWEIGHT()
        '        lsECL = txtECL.Text
        'lsDEPT =
        'lsPACKAGE_CODE =
        ' lsPAY_SUFFIX=
        '        lsWAREHOUSE_LOC()
        '        lsCUSTOMER_CODE()
        '        lsCUSTOMER_PART_NBR()
        '        lsBROADCAST_CODE = " "
        '        lsREF_SEQ_DATA_ID()
    End Sub
    Sub ReadHIST_ICS_Label_And_InsertData()
        Dim lsSQL As String
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim sContainerType As String = ""
        Dim lMachineID As Long = 0
        Dim lResetCounters As Long = 0
        'Dim dr As DataRow
        Dim lCurrentShiftQty As Long = 0
        Dim shiftDate As Date
        Dim shiftID As Int32
        Dim bMoveToHistory As Boolean
        Dim liRecordsFound As Integer
        Try
            liMATID = Session("MATID")
            lsSTD_FLAG = Session("STD_FLAG")
            lsDESTINATION = Me.txtDestination.Text
            Dim j As Integer = 1

            If j <> 1 Then
                'This routine is not needed any longer- Do NOT read ICS_LABEL_DATA to assemble data
                ' we just need ICS_MATID plus SAP LABEL_DATA


                'Dim sel, whe, vlu, ord
                ''sel = "SELECT * FROM SNAPMGR.ICS_LABEL_DATA WHERE part_nbr = '" & Me.txtPart_Nbr.Text & "'  "
                ''whe = " and MATID = " & liMATID

                'sel = "SELECT * FROM SNAPMGR.ICS_LABEL_DATA WHERE part_nbr = '" & Me.txtPart_Nbr.Text & "'  "
                'whe = " and MATID = " & liMATID

                ''whe = " and ECL = '" & txtECL.Text & "' and DESTINATION is null and PACKAGE_CODE = '" & lsPACKAGE_CODE & "'"
                ''If txtDestination.Text.Length > 0 Then

                ''    whe = " and ECL = '" & txtECL.Text & "' and DESTINATION = '" & txtDestination.Text & "' and PACKAGE_CODE = '" & lsPACKAGE_CODE & "'"
                ''End If
                'ord = " order by MOD_TMSTM desc "
                'lsSQL = sel + whe + ord
                ''lsSQL = "SELECT * FROM SNAPMGR.ICS_LABEL_DATA WHERE part_nbr = '" & lsPART_NBR & "' "
                ''PART_NBR(, ECL, DESTINATION, PACKAGE_CODE)
                'Dim cmd As New OracleCommand
                'Dim r As System.Data.OracleClient.OracleString
                'If con.State = ConnectionState.Closed Then con.Open()
                'With cmd
                '    .Connection = con
                '    .CommandType = CommandType.Text
                '    .CommandText = lsSQL
                '    .ExecuteNonQuery()
                'End With
                'Dim dr As OracleDataReader = cmd.ExecuteReader() ' VB.NET
                'dr.Read()
                ''  liSAP_STD_PACK_QTY As Integer
                ''  liICS_STD_PACK_QTY As Integer
                'liICS_STD_PACK_QTY = dr.Item("quantity")

                '' Me.liICS_RecordsToCreate = liSAP_STD_PACK_QTY / liICS_STD_PACK_QTY


                'llRECORD_ID = llRECORD_ID
                ''lsSTD_FLAG = (IIf(IsDBNull(dr.Item("STD_FLAG")), "", dr.Item("STD_FLAG")))
                'lsSTD_FLAG = Session("STD_FLAG") 'should come from ICS_MATID
                'lsPART_NBR = lsPART_NBR
                'lsFORM_CODE = " "
                'lsDESTINATION = (IIf(IsDBNull(dr.Item("DESTINATION")), "", dr.Item("DESTINATION")))
                'liQUANTITY = liICS_STD_PACK_QTY
                'liWEIGHT = liWEIGHT ' FROM SAP
                ''liWEIGHT = (IIf(IsDBNull(dr.Item("WEIGHT")), 0, dr.Item("WEIGHT")))
                '' Need to determine crossref for STD_PACK_CNTR below:
                ''lsECL = (IIf(IsDBNull(dr.Item("ECL")), 0, dr.Item("ECL")))
                ''txtECL.Text = "02" 'REMOVE
                'lsECL = txtECL.Text ' from screen
                'lsSERIAL_NBR = (IIf(IsDBNull(dr.Item("SERIAL_NBR")), "", dr.Item("SERIAL_NBR")))
                'lsSERIAL_NBR = lsSERIAL_NBR ' will be set up below by Max call
                'lsDEPT = (IIf(IsDBNull(dr.Item("DEPT")), "", dr.Item("DEPT")))
                'lsPACKAGE_CODE = (IIf(IsDBNull(dr.Item("PACKAGE_CODE")), "", dr.Item("PACKAGE_CODE")))
                'lsPAY_SUFFIX = (IIf(IsDBNull(dr.Item("PAY_SUFFIX")), "", dr.Item("PAY_SUFFIX")))
                ''liMATID = (IIf(IsDBNull(dr.Item("MATID")), 0, dr.Item("MATID")))
                'liMATID = liMATID
                'lsDESCRIPTION = lsDESCRIPTION ' from SAP
                'lsWAREHOUSE_LOC = (IIf(IsDBNull(dr.Item("WAREHOUSE_LOC")), "", dr.Item("WAREHOUSE_LOC")))
                'lsCUSTOMER_CODE = (IIf(IsDBNull(dr.Item("CUSTOMER_CODE")), "", dr.Item("CUSTOMER_CODE")))
                ''TODO IS THIS THE MES PART# OR CUST PART #
                'lsCUSTOMER_PART_NBR = (IIf(IsDBNull(dr.Item("CUSTOMER_PART_NBR")), "", dr.Item("CUSTOMER_PART_NBR")))
                'lsBROADCAST_CODE = " " 'TODO BLANK ALWAYS?
                ''lsDELIVERY_LOC_NBR = (IIf(IsDBNull(dr.Item("DELIVERY_LOC_NBR")), "", dr.Item("DELIVERY_LOC_NBR")))
                'lsDELIVERY_LOC_NBR = lsDELIVERY_LOC_NBR  ' FROM SAP UNLOADING_POINT
                'lsREF_SEQ_DATA_ID = (IIf(IsDBNull(dr.Item("REF_SEQ_DATA_ID")), "", dr.Item("REF_SEQ_DATA_ID")))

                ''TODO shoud come from SAP? needed for label printing. 
                ''lsREF_SEQ_NBR = (IIf(IsDBNull(dr.Item("REF_SEQ_NBR")), "", dr.Item("REF_SEQ_NBR")))

                ''TODO shoud come from SAP? needed for label printing. missing last 2 characters in label 
                ''lsPKG_12Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_12Z_SEGM_NBR")), "", dr.Item("PKG_12Z_SEGM_NBR")))
                'lsPKG_13Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_13Z_SEGM_NBR")), "", dr.Item("PKG_13Z_SEGM_NBR")))
                ''lsPKG_14Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_14Z_SEGM_NBR")), "", dr.Item("PKG_14Z_SEGM_NBR")))
                ''lsPKG_15Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_15Z_SEGM_NBR")), "", dr.Item("PKG_15Z_SEGM_NBR")))
                'lsPKG_16Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_16Z_SEGM_NBR")), "", dr.Item("PKG_16Z_SEGM_NBR")))
                ''lsPKG_17Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_17Z_SEGM_NBR")), "", dr.Item("PKG_17Z_SEGM_NBR")))
                'lsCUSTOMER_NAME = lsCUSTOMER_NAME ' from sap
                'Session("CUSTOMER_NAME") = lsCUSTOMER_NAME
                'lsCUSTOMER_STREET_ADDR = lsCUSTOMER_STREET_ADDR ' from sap
                'lsCUSTOMER_CITY = lsCUSTOMER_CITY
                'lsICS_CNTR_PACKAGE_TYPE = lsICS_CNTR_PACKAGE_TYPE ' from sap
                ''lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("ICS_CNTR_PACKAGE_TYPE")), 0, dr.Item("ICS_CNTR_PACKAGE_TYPE")))
                'liCHECK_SUM = 4
                'liUSED_STATUS = 0
                'lsTIME_USED = Now().ToString("dd-MMM-yy")
                'lsMACHINE_NAME = (IIf(IsDBNull(dr.Item("MACHINE_NAME")), "", dr.Item("MACHINE_NAME")))
                'lsMOD_USERID = "AUTO_SAP"
                'lsMOD_TMSTM = Now().ToString("dd-MMM-yy")
                'lsCUSTOMER_FILLER = lsCUSTOMER_FILLER  ' send sales_Doc_nbr for kiosk reading of unique label record from SAP

                ''lsCUSTOMER_FILLER = (IIf(IsDBNull(dr.Item("CUSTOMER_FILLER")), 0, dr.Item("CUSTOMER_FILLER")))
                'lsCUSTOMER_PN_FILLER = (IIf(IsDBNull(dr.Item("CUSTOMER_PN_FILLER")), "", dr.Item("CUSTOMER_PN_FILLER")))
                'lsCISCO_CODE = (IIf(IsDBNull(dr.Item("CISCO_CODE")), "", dr.Item("CISCO_CODE")))
                'lsREQ_STORE = (IIf(IsDBNull(dr.Item("REQ_STORE")), "", dr.Item("REQ_STORE")))

            End If
            liRecordsFound = liRecordsFound + 1

        Catch exole As OracleException
            Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
            If liRecordsFound > 0 Then
                ' ReadmaxSerialNumber()
                Dim liSerial_NBR As Long
                liICS_RecordsToCreate = CInt(txtNoOfLabels.Text)
                Dim i As Integer
                Read_MATID_From_ICS_MATID()
                For i = 1 To liICS_RecordsToCreate
                    'llRECORD_ID = GetNextSerialViaSequencer()
                    'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
                    liSerial_NBR = GetNextSerialViaSequencerNew()

                    lsSERIAL_NBR = liSerial_NBR.ToString
                    llRECORD_ID = llRECORD_ID + 1
                    InsertICSLabelData()
                Next
                lblStatus.ForeColor = Color.Navy
                lblStatus.Text = "records inserted: " & liICS_RecordsCreated.ToString
            Else
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "No Previous data found : " & liICS_RecordsCreated.ToString
            End If
        End Try
    End Sub
    Sub Read_MATID_From_ICS_MATID()
        Dim lsSQL As String
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim sContainerType As String = ""
        Dim lMachineID As Long = 0
        Dim lResetCounters As Long = 0
        'Dim dr As DataRow
        Dim lCurrentShiftQty As Long = 0
        Dim shiftDate As Date
        Dim shiftID As Int32
        Dim bMoveToHistory As Boolean
        Dim liRecordsFound As Integer
        liRecordsFound = 0
        Try

            Dim sel, whe, vlu, ord
            sel = "SELECT * FROM pltfloor.ICS_MATID  "
            whe = " where MATID = " & liMATID & " and part_nbr = '" & txtPart_Nbr.Text & "'"
            'If txtDestination.Text.Length > 0 Then
            '    whe = " and ECL = '" & txtECL.Text & "' and DESTINATION = '" & txtDestination.Text & "' and PACKAGE_CODE = '" & lsPACKAGE_CODE & "'"
            'End If
            'ord = " order by MOD_TMSTM desc "
            lsSQL = sel + whe
            'lsSQL = "SELECT * FROM SNAPMGR.ICS_LABEL_DATA WHERE part_nbr = '" & lsPART_NBR & "' "
            'PART_NBR(, ECL, DESTINATION, PACKAGE_CODE)
            Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = lsSQL
                .ExecuteNonQuery()
            End With
            Dim dr As OracleDataReader = cmd.ExecuteReader() ' VB.NET
            dr.Read()

            liICS_STD_PACK_QTY = dr.Item("quantity")

            ' liICS_RecordsToCreate = liSAP_STD_PACK_QTY / liICS_STD_PACK_QTY

            ' llRECORD_ID = llRECORD_ID
            liMATID = (IIf(IsDBNull(dr.Item("MATID")), 0, dr.Item("MATID")))
            Session("MATID") = liMATID
            'lsPART_NBR = lsPART_NBR

            lsSTD_FLAG = (IIf(IsDBNull(dr.Item("STD_FLAG")), "", dr.Item("STD_FLAG")))
            'lsSTD_FLAG = "I" 'TODO REMOVE
            Session("STD_FLAG") = lsSTD_FLAG
            cmbSTD_FLAG.SelectedValue = lsSTD_FLAG

            lsFORM_CODE = (IIf(IsDBNull(dr.Item("FORM_CODE")), "", dr.Item("FORM_CODE")))

            txtSTD_FLAG.Text = lsSTD_FLAG

            lsDESTINATION = (IIf(IsDBNull(dr.Item("DESTINATION")), "", dr.Item("DESTINATION")))
            txtDestination.Text = lsDESTINATION
            liQUANTITY = IIf(IsDBNull(dr.Item("QUANTITY")), "", dr.Item("QUANTITY"))
            txtQuantity.Text = liQUANTITY
            liWEIGHT = (IIf(IsDBNull(dr.Item("WEIGHT")), 0, dr.Item("WEIGHT")))

            txtWeight.Text = liWEIGHT

            lsECL = (IIf(IsDBNull(dr.Item("ECL")), 0, dr.Item("ECL")))
            txtECL.Text = lsECL
            'lsECL = txtECL.Text ' from screen
            'lsSERIAL_NBR = lsSERIAL_NBR ' will be set up below by Max call
            lsDEPT = (IIf(IsDBNull(dr.Item("DEPT")), "", dr.Item("DEPT")))
            txtDept.Text = lsDEPT
            lsPACKAGE_CODE = (IIf(IsDBNull(dr.Item("PACKAGE_CODE")), "", dr.Item("PACKAGE_CODE")))
            txtPackageCode.Text = lsPACKAGE_CODE
            lsPAY_SUFFIX = (IIf(IsDBNull(dr.Item("PAY_SUFFIX")), "", dr.Item("PAY_SUFFIX")))
            txtPAY_SUFFIX.Text = lsPAY_SUFFIX
            'lsDESCRIPTION = (IIf(IsDBNull(dr.Item("DESCRIPTION")), 0, dr.Item("DESCRIPTION")))
            'lblDescription.Text = lsDESCRIPTION ' from SAP
            lsWAREHOUSE_LOC = (IIf(IsDBNull(dr.Item("WAREHOUSE_LOC")), "", dr.Item("WAREHOUSE_LOC")))
            lsCUSTOMER_CODE = (IIf(IsDBNull(dr.Item("CUSTOMER_CODE")), "", dr.Item("CUSTOMER_CODE")))
            lblSHIP_TO.Text = (IIf(IsDBNull(dr.Item("SHIP_TO")), "", dr.Item("SHIP_TO")))
            lblFoundShipTo.Text = "NO"
            lsSHIP_TO = lblSHIP_TO.Text
            If lsSHIP_TO.Length > 3 Then
                lblFoundShipTo.Text = "YES"
            End If
            'TODO IS THIS THE MES PART# OR CUST PART #
            lsCUSTOMER_PART_NBR = (IIf(IsDBNull(dr.Item("CUSTOMER_PART_NBR")), "", dr.Item("CUSTOMER_PART_NBR")))
            Session("MATID_CUSTOMER_PART_NBR") = lsCUSTOMER_PART_NBR
            lsBROADCAST_CODE = (IIf(IsDBNull(dr.Item("BROADCAST_CODE")), "", dr.Item("BROADCAST_CODE")))
            ' lsDELIVERY_LOC_NBR = (IIf(IsDBNull(dr.Item("DELIVERY_LOC_NBR")), "", dr.Item("DELIVERY_LOC_NBR")))
            'lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("ICS_CNTR_PACKAGE_TYPE")), "", dr.Item("ICS_CNTR_PACKAGE_TYPE")))
            'lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("ICS_CNTR_PACKAGE_TYPE")), 0, dr.Item("ICS_CNTR_PACKAGE_TYPE")))
            ' liCHECK_SUM = 4
            'liUSED_STATUS = 0
            'lsTIME_USED = Now().ToString("dd-MMM-yy")
            'lsMACHINE_NAME = (IIf(IsDBNull(dr.Item("MACHINE_NAME")), 0, dr.Item("MACHINE_NAME")))
            'lsMOD_USERID = "AUTO_SAP"
            'lsMOD_TMSTM = Now().ToString("dd-MMM-yy")
            '' lsCUSTOMER_FILLER = (IIf(IsDBNull(dr.Item("CUSTOMER_FILLER")), 0, dr.Item("CUSTOMER_FILLER")))
            'lsCUSTOMER_PN_FILLER = (IIf(IsDBNull(dr.Item("CUSTOMER_PN_FILLER")), 0, dr.Item("CUSTOMER_PN_FILLER")))
            'lsCISCO_CODE = (IIf(IsDBNull(dr.Item("CISCO_CODE")), 0, dr.Item("CISCO_CODE")))
            'lsREQ_STORE = (IIf(IsDBNull(dr.Item("REQ_STORE")), 0, dr.Item("REQ_STORE")))
            liRecordsFound = liRecordsFound + 1
            lbDataError = False

        Catch exole As OracleException
            'Throw New Exception(exole.Message)
            Me.lblDescription.Text = "MATID / Part combination is not on file"
        Catch ex As Exception
            Me.lblDescription.Text = "MATID / Part combination is not on file"
            'Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()

        End Try
    End Sub

    Sub VerifyThatMATID_IsNotObsolete()
        Dim lsSQL As String
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim sContainerType As String = ""
        Dim lMachineID As Long = 0
        Dim lResetCounters As Long = 0
        'Dim dr As DataRow
        Dim lCurrentShiftQty As Long = 0
        Dim shiftDate As Date
        Dim shiftID As Int32
        Dim bMoveToHistory As Boolean
        Dim liRecordsFound As Integer
        liRecordsFound = 0
        Try

            Dim sel, whe, vlu, ord
            sel = "SELECT * FROM pltfloor.ICS_MATID  "
            whe = " where MATID = " & liMATID & " "
            'If txtDestination.Text.Length > 0 Then
            '    whe = " and ECL = '" & txtECL.Text & "' and DESTINATION = '" & txtDestination.Text & "' and PACKAGE_CODE = '" & lsPACKAGE_CODE & "'"
            'End If
            'ord = " order by MOD_TMSTM desc "
            lsSQL = sel + whe
            'lsSQL = "SELECT * FROM SNAPMGR.ICS_LABEL_DATA WHERE part_nbr = '" & lsPART_NBR & "' "
            'PART_NBR(, ECL, DESTINATION, PACKAGE_CODE)
            Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = lsSQL
                .ExecuteNonQuery()
            End With
            Dim dr As OracleDataReader = cmd.ExecuteReader() ' VB.NET
            dr.Read()

            Dim lsStatus As String

            txtMATID_Status.Text = (IIf(IsDBNull(dr.Item("STATUS")), "", dr.Item("STATUS")))

            liRecordsFound = liRecordsFound + 1
            lbDataError = False

        Catch exole As OracleException
            'Throw New Exception(exole.Message)
            Me.lblDescription.Text = "MATID  is not on file"
        Catch ex As Exception
            Me.lblDescription.Text = "MATID is not on file"
            'Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()

        End Try
    End Sub
    Sub CleanFieds()
        liMATID = 0
        txtMATID.Text = ""
        txtPart_Nbr.Text = ""
        lsMaterial_Nbr = ""
        lsPART_NBR = ""
        lsCUSTOMER_PART_NBR = ""
        lblDescription.Text = ""
        lstbxInsertResults.Items.Clear()
        txtECL.Text = ""
        txtDestination.Text = ""
        txtPackageCode.Text = ""
        txtSTD_FLAG.Text = ""

        lblECL.Text = ""
        lblDestination.Text = ""
        lblPackageCode.Text = ""
        lblSTD_FLAG.Text = ""

        lsCUSTOMER_NAME = ""
        txtNoOfLabels.Text = ""
        btnCreateLabelsFromSAP.Visible = False
        cmbSAP_Customer.Visible = False
        lblSAP_Customer.Visible = False
        txtNoOfLabels.Visible = False
        lblNoOfLabels.Visible = False
        lblSHIP_TO.Visible = False
        lblSHIP_TO_H.Visible = False
        lblPlant_Dock_Line_1.Visible = False
        lblPlant_Dock_Line_1_H.Visible = False
        lblDEST_LOC.Visible = False
        lblDEST_LOC_H.Visible = False

        lblECL.Visible = False
        lblDestination.Visible = False
        lblPackageCode.Visible = False
        lblSTD_FLAG.Visible = False
        cmbSTD_FLAG.Visible = False
        txtECL.Visible = False
        txtDestination.Visible = False
        txtPackageCode.Visible = False
        txtSTD_FLAG.Visible = False

        lblCUST_ADDRESS.Text = ""
        lblCUST_CITY.Text = ""
        lblCUST_STATE.Text = ""
        lblSHIP_TO.Text = ""
        'lblSHIP_TO_H.Text = ""
        lblStatus.Text = ""
        lblPlant_Dock_Line_1.Text = ""
        'lblPlant_Dock_Line_1_H.Text = ""
        lblDEST_LOC.Text = ""
        'lblDEST_LOC_H.Text = ""
        UnLockAllFields()
    End Sub
    Sub UnLockAllFields()

        txtMATID.Enabled = True
        txtPart_Nbr.Enabled = True
        txtNoOfLabels.Enabled = True
        btnCreateLabelsFromSAP.Enabled = True
        cmbSAP_Customer.Enabled = True
        btnCreateLabelsFromSAP.Enabled = True
        btnVerifyMATID.Enabled = True

    End Sub
    Sub LockAllFields()

        txtMATID.Enabled = False
        txtPart_Nbr.Enabled = False
        txtNoOfLabels.Enabled = False
        btnCreateLabelsFromSAP.Enabled = False
        cmbSAP_Customer.Enabled = False
        btnCreateLabelsFromSAP.Enabled = False
        btnVerifyMATID.Enabled = False

    End Sub
    Sub CleanVerificationFields()
        lsCUSTOMER_PART_NBR = ""
        lsCUSTOMER_NAME = ""
        txtNoOfLabels.Text = ""
        lblDescription.Text = " "
        btnCreateLabelsFromSAP.Visible = False
        cmbSAP_Customer.Items.Clear()
        cmbSAP_Customer.Visible = False
        lblSAP_Customer.Visible = False
        txtNoOfLabels.Visible = False
        lblNoOfLabels.Visible = False

        txtECL.Visible = False
        txtPackageCode.Visible = False
        txtDestination.Visible = False
        txtSTD_FLAG.Visible = False
        cmbSTD_FLAG.Visible = False

        lblECL.Visible = False

        lblPackageCode.Visible = False
        lblDestination.Visible = False
        lblSTD_FLAG.Visible = False


        lblCUST_ADDRESS.Text = ""
        lblCUST_CITY.Text = ""
        lblCUST_STATE.Text = ""
        lblSHIP_TO.Text = ""
        'lblSHIP_TO_H.Text = ""
        lblPlant_Dock_Line_1.Text = ""
        'lblPlant_Dock_Line_1_H.Text = ""
        lblDEST_LOC.Text = ""
        'lblDEST_LOC_H.Text = ""
        'lblStatus.ForeColor = Color.Transparent
        lblDEST_LOC.BackColor = Color.Transparent
        lblPlant_Dock_Line_1.BackColor = Color.Transparent
    End Sub


    Private Sub btnVerifyMATID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerifyMATID.Click

        Try
            CleanVerificationFields()
            If Not IsNumeric(txtMATID.Text) Then
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "Error - Non Numeric MATID"
                Exit Sub
            End If

            liMATID = CInt(txtMATID.Text)
            If Not IsNumeric(txtPart_Nbr.Text) Then
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "Error - Non numeric Part Number"
                Exit Sub
            End If

            txtMATID_Status.Text = "."
            Me.VerifyThatMATID_IsNotObsolete()
            If txtMATID_Status.Text = "O" Then
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "MATID is Obsolete"
                Exit Sub
            End If


            lbDataError = True
            Read_MATID_From_ICS_MATID()
            If lbDataError = False Then
                GetCustomersFromSAP_Label_DataByPart()
            Else
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "Invalid MATID / Part Number"
                Exit Sub
            End If

            ' ReadSAPDataByPartCustAndLoadFields()
            If lbDataError = False Then
                '  btnCreateLabelsFromSAP.Visible = True
                ShowVerificationFields()
                ShowSAPFields()
            End If
            If lbDataError = True Then
                Dim liFlag As Integer = 0
                If Me.lblPlant_Dock_Line_1.Text.Length = 0 Then
                    lblPlant_Dock_Line_1.BackColor = Color.Red
                    lblStatus.ForeColor = Color.Red
                    lblStatus.Text = "DLOC"
                    Me.ShowSAPFields()
                    lblStatus.Text = "This Part Number/Destination does NOT have Material Handling Code information.."
                    'Me.lstbxInsertResults.Items.Add("This Part Number/Destination does NOT have Material Handling Code information..")
                    liFlag = liFlag + 1
                End If
                If Me.lblDEST_LOC.Text.Length = 0 Then
                    lblDEST_LOC.BackColor = Color.Red
                    lblStatus.ForeColor = Color.Red
                    lblStatus.Text = "DLOC"
                    Me.ShowSAPFields()
                    lblStatus.Text = "This Part Number/Destination does NOT have Plant Dock  information.."
                    'Me.lstbxInsertResults.Items.Add("This Part Number/Destination does NOT have Plant Dock information..")
                    liFlag = liFlag + 1
                    Exit Sub
                End If
                If liFlag = 0 Then
                    lblStatus.ForeColor = Color.Red
                    lblStatus.Text = "Invalid MATID / Part Number"
                    lblDescription.Text = "Invalid MATID / Part Number"
                    Exit Sub
                End If
            End If

            If txtSTD_FLAG.Text = "S" Then
                HideFieldsForGenericRequest()
                btnCreateLabelsFromSAP.Visible = True
            End If
        Catch exole As OracleException
            'Throw New Exception(exole.Message)
            Me.lblDescription.Text = "Invalid MATID / Part combination "
            lblStatus.ForeColor = Color.Red
            lblStatus.Text = "Invalid MATID / Part Number"
        Catch ex As Exception
            Me.lblDescription.Text = "Invalid MATID / Part combination "
            lblStatus.ForeColor = Color.Red
            lblStatus.Text = "Invalid MATID / Part Number"
            Throw New Exception(ex.Message)
        Finally

        End Try
    End Sub
    Sub ShowVerificationFields()
        lblECL.Visible = True
        lblPackageCode.Visible = True
        lblDestination.Visible = True
        lblSTD_FLAG.Visible = True
        lblDescription.Visible = True
        txtECL.Visible = True
        txtPackageCode.Visible = True
        txtDestination.Visible = True
        cmbSTD_FLAG.Visible = True
    End Sub
    Sub HideVerificationFields()
        lblECL.Visible = False
        lblPackageCode.Visible = False
        lblDestination.Visible = False
        lblSTD_FLAG.Visible = False
        lblDescription.Visible = False
        txtECL.Visible = False
        txtPackageCode.Visible = False
        txtDestination.Visible = False
        cmbSTD_FLAG.Visible = False
    End Sub

    Sub ShowSAPFields()
        cmbSAP_Customer.Visible = True
        cmbSTD_FLAG.Visible = True
        lblDEST_LOC.Visible = True
        lblDEST_LOC_H.Visible = True
        lblDestination.Visible = True
        lblECL.Visible = True
        lblNoOfLabels.Visible = True
        lblPackageCode.Visible = True
        lblPlant_Dock_Line_1.Visible = True
        lblPlant_Dock_Line_1_H.Visible = True
        lblSAP_Customer.Visible = True
        lblSAP_Customer.Visible = True
        lblSAP_Customer.Visible = True
        lblSHIP_TO.Visible = True
        lblSHIP_TO_H.Visible = True
        lblSTD_FLAG.Visible = True
        txtDestination.Visible = True
        txtECL.Visible = True
        txtNoOfLabels.Visible = True
        txtPackageCode.Visible = True
        lblCUST_ADDRESS.Visible = True
        lblCUST_CITY.Visible = True
        lblCUST_STATE.Visible = True

    End Sub
    Friend Function GetConnectionStringk() As String
        ' local variables
        Dim sConnect As String
        Dim sDataSource As String
        Dim sPassword As String
        Dim sProvider As String
        Dim sServerType As String
        Dim sUserID As String
        ' Dim moEncrypt As New DPH.Common.Object.Encryption.SymmetricEncrypt("DELPHI", "MIT")
        'TODO: Uncomment encryption settings
        ' get the server type setting from the machine.config file
        sServerType = ConfigurationSettings.AppSettings("ServerType")
        'If Not sServerType.EndsWith(".") Then
        '    sServerType = sServerType & "."
        'End If
        '="DATA SOURCE=tv10.usohwar;USER ID=mesweb_p23;PASSWORD=mes23full;
        'sUserID = "mesweb_p23"
        'sPassword = "mes23full"
        sUserID = "mz36gf"
        sPassword = "mz36gf1234"
        sDataSource = "tv10.usohwar"
        'sDataSource = "prod.fanta"
        sProvider = "OraOLEDB.Oracle.1"

        ' build the connection string
        ' sConnect = "PROVIDER=" & sProvider & ";" & _
        sConnect = "DATA SOURCE=" & sDataSource & ";" & _
                    "USER ID=" & sUserID & ";" & _
                    "PASSWORD=" & sPassword & ";"

        Return sConnect
    End Function ' GetConnectionString
    Friend Function GetConnectionStringTest() As String
        ' local variables
        Dim sConnect As String
        Dim sDataSource As String
        Dim sPassword As String
        Dim sProvider As String
        Dim sServerType As String
        Dim sUserID As String
        ' Dim moEncrypt As New DPH.Common.Object.Encryption.SymmetricEncrypt("DELPHI", "MIT")
        'TODO: Uncomment encryption settings
        ' get the server type setting from the machine.config file
        sServerType = ConfigurationSettings.AppSettings("ServerType")
        'If Not sServerType.EndsWith(".") Then
        '    sServerType = sServerType & "."
        'End If
        '="DATA SOURCE=tv10.usohwar;USER ID=mesweb_p23;PASSWORD=mes23full;
        'sUserID = "mesweb_p23"
        'sPassword = "mes23full"
        sUserID = "mz36gf"
        sPassword = "mz36gf1234"
        sDataSource = "tv10.usohwar"
        'sDataSource = "prod.fanta"
        sProvider = "OraOLEDB.Oracle.1"

        ' build the connection string
        ' sConnect = "PROVIDER=" & sProvider & ";" & _
        sConnect = "DATA SOURCE=" & sDataSource & ";" & _
                    "USER ID=" & sUserID & ";" & _
                    "PASSWORD=" & sPassword & ";"

        Return sConnect
    End Function ' GetConnectionString

    Private Sub btnCreateLabelsFromSAP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateLabelsFromSAP.Click
        If Not IsNumeric(txtNoOfLabels.Text) Then
            lblStatus.ForeColor = Color.Red
            lblStatus.Text = "Error - Invalid number of labels"
            Exit Sub
        End If
        liICS_RecordsToCreate = CInt(txtNoOfLabels.Text)
        If liICS_RecordsToCreate < 1 Then
            lblStatus.ForeColor = Color.Red
            lblStatus.Text = "Error - Number must be greater than zero"
            Exit Sub
        End If
        If liICS_RecordsToCreate > 500 Then
            lblStatus.ForeColor = Color.Red
            lblStatus.Text = "Error - Too many labels"
            Exit Sub
        End If
        If lsCUSTOMER_NAME = "" Then
            lblStatus.ForeColor = Color.Red
            lblStatus.Text = "Error - Please select a Customer"
            Exit Sub
        End If

        ReadSAPDataByPartCustAndLoadFields()
        UpdateICS_MATID_with_SHIP_TO()
        ReadHIST_ICS_Label_And_InsertData()


        LockAllFields()
        If Me.txtSTD_FLAG.Text = "S" Then
            HideFieldsForGenericRequest()
        End If
        'Populate_dgICS_LABEL_DATA()
    End Sub
    'Sub Populate_dgICS_LABEL_DATA()
    '    Dim lsSQL As String
    '    Dim strConn As String = GetConnectionString()

    '    Dim MyConn As New OleDb.OleDbConnection(strConn)
    '    Dim sqlGetLabelData As String
    '    Dim SearchDate As String
    '    Dim sel, frm, whr, grp, ord As String
    '    Dim RecordCount As Integer
    '    Try
    '        SearchDate = DateAdd(DateInterval.Day, -350, DateTime.Today())
    '        sel = "SELECT PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, COUNT(SERIAL_NBR), MATID, DELIVERY_LOC_NBR, to_char(MOD_TMSTM,'mm/dd/yyyy')as TimeStamp"
    '        frm = " FROM PLTFLOOR.ICS_LABEL_DATA_VW"
    '        whr = " WHERE PART_NBR = '" & txtPart_Nbr.Text & "' AND DESTINATION = '" & txtDestination.Text & "' AND ECL = '" & txtECL.Text & "' AND PACKAGE_CODE = '" & Me.txtPackageCode.Text & "' AND MOD_TMSTM >= to_date('" & SearchDate & "', 'MM/DD/YYYY HH:MI:SS AM') AND USED_STATUS = 0"
    '        grp = " GROUP BY PART_NBR, ECL, DESTINATION, PACKAGE_CODE, MATID, QUANTITY, DELIVERY_LOC_NBR, to_char(MOD_TMSTM,'mm/dd/yyyy')"
    '        ord = " "
    '        sqlGetLabelData = sel & frm & whr & grp & ord
    '        'sqlGetLabelData = "SELECT PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, COUNT(PART_NBR), MATID, MOD_TMSTM FROM PLTFLOOR.ICS_LABEL_DATA_VW WHERE PART_NBR = '" & cmbPartNumber.SelectedItem.Text & "'AND ECL = '" & cmbECL.SelectedValue & "' AND DESTINATION = '" & cmbDestination.SelectedValue & "' AND PACKAGE_CODE = '" & cmbPackageCode.SelectedValue & "' AND MOD_TMSTM >= to_date('" & SearchDate & "  12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM') AND USED_STATUS = 0 GROUP BY PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, MATID, MOD_TMSTM ORDER BY MOD_TMSTM"
    '        'Response.Write(sqlGetLabelData)
    '        ' Create a Command object with the SQL statement.
    '        MyConn.Open()
    '        Dim objCommand As New OleDb.OleDbCommand(sqlGetLabelData, MyConn)
    '        Dim objDataReader As OleDb.OleDbDataReader
    '        objDataReader = objCommand.ExecuteReader
    '        RecordCount = 0
    '        While objDataReader.Read()
    '            RecordCount = RecordCount + 1
    '        End While
    '        'If RecordCount = 0 Then
    '        '    lblCount.Visible = True
    '        '    lblCount.Text = "Record Count: 0"
    '        'Else
    '        '    lblCount.Visible = True
    '        '    lblCount.Text = "Record Count: " & RecordCount
    '        'End If
    '        objDataReader.Close()
    '        objDataReader = objCommand.ExecuteReader
    '        If objDataReader.HasRows = False Then
    '            lblStatus.Text = "No Records were found."
    '            dgICS_LABEL_DATA.Visible = False
    '            'lblStatus.Visible = True
    '            'lblStatus.Visible = False
    '        Else
    '            dgICS_LABEL_DATA.DataSource = objDataReader
    '            dgICS_LABEL_DATA.DataBind()
    '            dgICS_LABEL_DATA.Visible = True
    '            'lblMessage.Visible = False
    '            'lblSortOrder.Visible = True
    '            'tblResults.Visible = True
    '        End If
    '        If MyConn.State = ConnectionState.Open Then
    '            MyConn.Dispose()
    '        End If
    '    Catch ex As Exception
    '        lblStatus.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
    '        'If IsLoggingOn() Then
    '        '    LogError(ex.Source, ex.Message)
    '        'End If
    '        'LogEvent(ex.Message)
    '    Finally
    '        MyConn.Dispose()
    '    End Try
    'End Sub


    Private Sub cmbSAP_Customer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSAP_Customer.SelectedIndexChanged
        Session("CUSTOMER_NAME") = cmbSAP_Customer.SelectedItem.Text
        Dim lsCust As String = cmbSAP_Customer.SelectedItem.Text
        lstbxInsertResults.Items.Clear()
        If InStr(lsCust, "Select", CompareMethod.Text) > 0 Then
            Exit Sub
        End If
        lbDataError = True
        ReadSAPDataByPartCustAndLoadFields()
        If Me.lbDataError = True Then
            If lblPlant_Dock_Line_1.Text.Length = 0 Then
                lblPlant_Dock_Line_1.BackColor = Color.Red
                lblStatus.Visible = True
                lblStatus.ForeColor = Color.Red
                lblSAP_Customer.Visible = True
                lblSAP_Customer.Visible = True
                cmbSAP_Customer.Visible = True
                cmbSAP_Customer.Visible = True
                lblStatus.Text = "This Part Number/Destination does NOT have Material Handling Code information.."
                'Me.lstbxInsertResults.Items.Add("This Part Number/Destination does NOT have Material Handling Code information..")
            End If
            If Me.lblDEST_LOC.Text.Length = 0 Then
                lblDEST_LOC.BackColor = Color.Red
                lblStatus.Visible = True
                lblStatus.ForeColor = Color.Red
                lblSAP_Customer.Visible = True
                lblSAP_Customer.Visible = True
                cmbSAP_Customer.Visible = True
                cmbSAP_Customer.Visible = True
                lblStatus.Text = "This Part Number/Destination does NOT have Plant Dock information.."
                'Me.lstbxInsertResults.Items.Add("This Part Number/Destination does NOT have Plant Dock information..")
            End If
            Exit Sub
        End If
        ShowVerificationFields()
        ShowSAPFields()
        lblStatus.ForeColor = Color.Black
        lblStatus.Text = " "
        If lblSHIP_TO.Text <> "" Then
            btnCreateLabelsFromSAP.Visible = True
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.CleanFieds()
    End Sub

    Private Sub lstbxInsertResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstbxInsertResults.SelectedIndexChanged

    End Sub

    Private Sub cmbSTD_FLAG_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSTD_FLAG.SelectedIndexChanged
        txtSTD_FLAG.Text = cmbSTD_FLAG.SelectedValue
        If Me.txtSTD_FLAG.Text = "S" Then
            HideFieldsForGenericRequest()
        Else
            cmbSAP_Customer.Visible = True
            lblSAP_Customer.Visible = True
            'txtNoOfLabels.Visible = True
            'lblNoOfLabels.Visible = True
            'lblSHIP_TO.Visible = False
            'lblSHIP_TO_H.Visible = False
            lblCUST_ADDRESS.Visible = True
            lblCUST_CITY.Visible = True
            lblCUST_STATE.Visible = True
        End If
    End Sub
    Sub HideFieldsForGenericRequest()
        cmbSAP_Customer.Visible = False
        lblSAP_Customer.Visible = False
        txtNoOfLabels.Visible = True
        lblNoOfLabels.Visible = True
        lblSHIP_TO.Visible = False
        lblSHIP_TO_H.Visible = False
        lblPlant_Dock_Line_1.Visible = False
        lblPlant_Dock_Line_1_H.Visible = False
        lblDEST_LOC.Visible = False
        lblDEST_LOC_H.Visible = False
        lblCUST_ADDRESS.Visible = False
        lblCUST_CITY.Visible = False
        lblCUST_STATE.Visible = False
        Me.btnCreateLabelsFromSAP.Visible = True
    End Sub
    Private Function GetNextSerialViaSequencer() As Int64

        '4/15/08 DSM: created to get nextval from sequencer for 4-C S/N's 

        Dim cmd As New OracleCommand
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cnOra As New OracleConnection(ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection"))
        Dim nextval As Int64
        Dim sqds As New DataSet
        Dim sqda As OracleDataAdapter
        Dim sqstring As String

        Try
            'cnOra.Open()
            'sqstring.Remove(0, sqstring.Length)
            sqstring = "select PLTFLOOR.BEC_SERIAL_NBR.nextval from dual"



            'Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()


            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = sqstring
                .ExecuteNonQuery()
            End With
            sqda = New OracleDataAdapter(cmd)
            sqda.Fill(sqds)

            If sqds.Tables(0).Rows.Count > 0 Then
                nextval = sqds.Tables(0).Rows(0).Item("NextVal")
                GetNextSerialViaSequencer = nextval
            Else
                Throw New Exception("Failure Aborting Skid: Could not get nextval from Standard_Pack_SN Sequencer.")
            End If

        Catch ex As Exception
            Return "</?BWSError> Failure generating new S/N's : " & " " & ex.Message
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

            'Try
            '    cnOra.Close()
            'Catch ex As Exception
            'End Try

            'Try
            '    cnOra.Dispose()
            'Catch ex As Exception
            'End Try

        End Try

    End Function
    Private Function GetNextSerialViaSequencerNew() As Int64
        Dim lsSQL As String
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        'Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim SQL As String = ""
        Dim sContainerType As String = ""
        Dim lMachineID As Long = 0
        Dim lResetCounters As Long = 0
        'Dim dr As DataRow
        Dim lCurrentShiftQty As Long = 0
        Dim shiftDate As Date
        Dim shiftID As Int32
        Dim bMoveToHistory As Boolean
        Try
            lsSQL = "select PLTFLOOR.BEC_SERIAL_NBR.nextval as NextSerialNbr from dual"

            Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = lsSQL
                .ExecuteNonQuery()
            End With
            Dim dr As OracleDataReader = cmd.ExecuteReader() ' VB.NET
            dr.Read()
            lsSERIAL_NBR = dr.Item("NextSerialNbr")
            llRECORD_ID = dr.Item("NextSerialNbr")
            GetNextSerialViaSequencerNew = llRECORD_ID
            'Dim liSerial_NBR As Long
            'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
            'lsSERIAL_NBR = liSerial_NBR.ToString
            'llRECORD_ID = llRECORD_ID + 1
        Catch exole As OracleException
            Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
        End Try

    End Function
    Friend Function GetConnectionStringNonOle() As String
        'instead of user legacy OleDB, use faster Oracle methods
        Dim conString As String = GetConnectionString()
        Dim liClean As Integer = InStr(conString, ";")
        conString = Mid(conString, liClean + 1)
        Return conString
    End Function

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Me.CleanFieds()
        'Me.UnLockAllFields()
        CleanVerificationFields()

    End Sub
End Class
