Imports System.Data.OracleClient
'Imports Oracle.ManagedDataAccess.Client


Partial Class SAP_MATID_Request

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
    Public lsSTATUS As String

    'Protected WithEvents lblStatus As System.Web.UI.WebControls.Label

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
        Dim x As String = GetServerType()
        Dim lsSecurityCheck As String
        If x = "LOCAL." Or x = "TEST." Then
            lblDisplay.Visible = True
            lblDisplay.Text = "TEST ENVIRONMENT"
        End If
        'lsSecurityCheck = GetSecurity(UCase(Right(Request.ServerVariables("LOGON_USER"), 6)))
        lsSecurityCheck = "BEC_ICS_WEB_ADMN_P23"
        If lsSecurityCheck = "BEC_ICS_WEB_ADMN_P23" Or lsSecurityCheck = "BEC_ICS_WEB_USER_P23" Then
            If IsPostBack = False Then
                lsCUSTOMER_NAME = Session("CUSTOMER_NAME")
                liMATID = Session("MATID")
                GetPartsAndLoadCombo()
                txtPAY_SUFFIX.Text = "00"
                GetDestinationsAndLoadCombo()
                'GetSHIP_TOAndLoadCombo()
                GetDeptAndLoadCombo()
                btnCreateMATID.Visible = False
                txtMATID.Text = ""
            End If
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If
    End Sub
    Sub LoadInsertFields()
        Try
            GetMaxMATID()
            txtMATID.Text = liMATID
            liMATID = liMATID
            lsPART_NBR = txtPart_Nbr.Text
            lsFORM_CODE = ""
            lsDESTINATION = txtDestination.Text
            liQUANTITY = txtQUANTITY.Text
            liWEIGHT = txtWEIGHT.Text
            lsECL = txtECL.Text
            lsDEPT = txtDept.Text
            lsPACKAGE_CODE = txtPackageCode.Text
            lsPAY_SUFFIX = txtPAY_SUFFIX.Text
            lsWAREHOUSE_LOC = "02"
            lsCUSTOMER_CODE = txtCUSTOMER_CODE.Text
            lsSHIP_TO = txtSHIP_TO.Text
            lsCUSTOMER_PART_NBR = txtCUST_PART_NBR.Text
            lsBROADCAST_CODE = ""
            lsDELIVERY_LOC_NBR = ""
            lsICS_CNTR_PACKAGE_TYPE = ""
            lsCUSTOMER_FILLER = ""
            lsSTATUS = "A"
            'lsTIME_USED  = txt .text
            lsMACHINE_NAME = ""
            lsMOD_USERID = "MESWEB"
            'lsMOD_TMSTM  = txt .text
            lsSTD_FLAG = txtSTD_FLAG.Text
            If lsSTD_FLAG.Length = 0 Then
                lsSTD_FLAG = Me.cmbSTD_FLAG.SelectedValue
            End If
        Catch exole As OracleException
            'Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally


        End Try
    End Sub
    Sub LoadUpdateFields()
        Try


            lsPART_NBR = txtPart_Nbr.Text
            lsFORM_CODE = ""
            lsDESTINATION = txtDestination.Text
            liQUANTITY = txtQUANTITY.Text
            liWEIGHT = txtWEIGHT.Text
            lsECL = txtECL.Text
            lsDEPT = txtDept.Text
            lsPACKAGE_CODE = txtPackageCode.Text
            lsPAY_SUFFIX = txtPAY_SUFFIX.Text
            lsWAREHOUSE_LOC = "02"
            lsCUSTOMER_CODE = txtCUSTOMER_CODE.Text
            lsSHIP_TO = txtSHIP_TO.Text
            lsCUSTOMER_PART_NBR = txtCUST_PART_NBR.Text
            lsBROADCAST_CODE = ""
            lsDELIVERY_LOC_NBR = ""
            lsICS_CNTR_PACKAGE_TYPE = ""
            lsCUSTOMER_FILLER = ""
            lsSTATUS = txtSTATUS.Text
            'lsTIME_USED  = txt .text
            lsMACHINE_NAME = ""
            lsMOD_USERID = "MESWEB"
            'lsMOD_TMSTM  = txt .text
            txtSTD_FLAG.Text = cmbSTD_FLAG.SelectedValue
            lsSTD_FLAG = txtSTD_FLAG.Text

        Catch exole As OracleException
            'Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally


        End Try
    End Sub
    Sub InsertICS_MATID()
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
        lsSTATUS = "A"

        Try
            LoadInsertFields()
            lsSQL1 = "INSERT INTO pltfloor.ICS_MATID (" & _
            " MATID, " & _
            " PART_NBR, " & _
            " FORM_CODE, " & _
            " DESTINATION, " & _
            " QUANTITY, " & _
            " WEIGHT, " & _
            " ECL, " & _
            " DEPT, " & _
            " PACKAGE_CODE, " & _
            " PAY_SUFFIX, " & _
            " WAREHOUSE_LOC, " & _
            " CUSTOMER_CODE, " & _
            " SHIP_TO, " & _
            " CUSTOMER_PART_NBR, " & _
            " BROADCAST_CODE, " & _
            " DELIVERY_LOC_NBR, " & _
            " ICS_CNTR_PACKAGE_TYPE, " & _
            " CUSTOMER_FILLER, " & _
            " STATUS, " & _
            " TIME_USED, " & _
            " MOD_USERID, " & _
            " MOD_TMSTM, " & _
            " STD_FLAG) " & _
            " VALUES ( "

            lsSQL2 = "'" & liMATID & "'," & _
"'" & lsPART_NBR & "'," & _
"'" & lsFORM_CODE & "'," & _
"'" & lsDESTINATION & "'," & _
"'" & liQUANTITY & "'," & _
"'" & liWEIGHT & "'," & _
"'" & lsECL & "'," & _
"'" & lsDEPT & "'," & _
"'" & lsPACKAGE_CODE & "'," & _
"'" & lsPAY_SUFFIX & "'," & _
"'" & lsWAREHOUSE_LOC & "'," & _
"'" & lsCUSTOMER_CODE & "'," & _
"'" & lsSHIP_TO & "'," & _
"'" & lsCUSTOMER_PART_NBR & "'," & _
"'" & lsBROADCAST_CODE & "'," & _
"'" & lsDELIVERY_LOC_NBR & "'," & _
"'" & lsICS_CNTR_PACKAGE_TYPE & "'," & _
"'" & lsCUSTOMER_FILLER & "'," & _
"'" & lsSTATUS & "'," & _
"'" & lsTIME_USED & "'," & _
"'" & lsMOD_USERID & "'," & _
"'" & lsMOD_TMSTM & "'," & _
"'" & lsSTD_FLAG & "')"

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
            Me.txtMATID.Text = liMATID.ToString
            Me.lblStatus.Text = "Successfully Inserted: " & liMATID.ToString
            Me.txtMATID.Text = liMATID.ToString
            Me.txtMATID.Visible = True
        Catch exole As OracleException
            'Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()

        End Try
    End Sub
    Sub UpdateICS_MATID()
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
        LoadUpdateFields()
        If IsNumeric(txtMATID.Text) Then
            liMATID = CInt(txtMATID.Text)
        Else
            Exit Sub
        End If


        Try
            lsSQL = "Update pltfloor.ICS_MATID set " & _
            "PART_NBR = '" & lsPART_NBR & "'," & _
            "FORM_CODE = '" & lsFORM_CODE & "'," & _
            "DESTINATION = '" & lsDESTINATION & "'," & _
            "QUANTITY =" & liQUANTITY & " ," & _
            "WEIGHT = " & liWEIGHT & " ," & _
            "ECL = '" & lsECL & "'," & _
            "DEPT = '" & lsDEPT & "'," & _
            "PACKAGE_CODE = '" & lsPACKAGE_CODE & "'," & _
            "PAY_SUFFIX = '" & lsPAY_SUFFIX & "'," & _
            "WAREHOUSE_LOC = '" & lsWAREHOUSE_LOC & "'," & _
            "CUSTOMER_CODE = '" & lsCUSTOMER_CODE & "'," & _
            "SHIP_TO = '" & lsSHIP_TO & "'," & _
            "CUSTOMER_PART_NBR = '" & lsCUSTOMER_PART_NBR & "'," & _
            "BROADCAST_CODE = '" & lsBROADCAST_CODE & "'," & _
            "DELIVERY_LOC_NBR = '" & lsDELIVERY_LOC_NBR & "'," & _
            "ICS_CNTR_PACKAGE_TYPE = '" & lsICS_CNTR_PACKAGE_TYPE & "'," & _
            "CUSTOMER_FILLER = '" & lsCUSTOMER_FILLER & "'," & _
            "STATUS = '" & txtSTATUS.Text & "'," & _
            "MOD_USERID = '" & lsMOD_USERID & "'," & _
            "MOD_TMSTM = '" & lsMOD_TMSTM & "'," & _
            "STD_FLAG = '" & lsSTD_FLAG & "' where  MATID = " & liMATID




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
            'Me.txtMATID.Text = liMATID.ToString
            Me.lblStatus.Text = "Successfully updated: " & liMATID.ToString
        Catch exole As OracleException
            'Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()

        End Try
    End Sub


    Sub GetMaxMATID()
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
            lsSQL = "SELECT max(MATID) AS MAXMATID  FROM PLTFLOOR.ICS_MATID"

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
            liMATID = dr.Item("MAXMATID")
            liMATID = liMATID + 1

        Catch exole As OracleException
            'Throw New Exception(exole.Message)
        Catch ex As Exception
            If liMATID = 0 Then
                liMATID = 1
            Else
                Throw New Exception(ex.Message)
            End If
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
        End Try
    End Sub
    Sub ReadSAPDataByPartCustname()
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
            'lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '12040756'"
            lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '" & txtPart_Nbr.Text & "'"
            lsSQL = lsSQL + " and cust_name_1 = '" & lsCUSTOMER_NAME & "'"
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
            Session("CUSTOMER_NAME") = lsCUSTOMER_NAME
            lsCUSTOMER_STREET_ADDR = (IIf(IsDBNull(dr.Item("CUST_ADDRESS")), "", dr.Item("CUST_ADDRESS")))
            lsCUSTOMER_CITY = (IIf(IsDBNull(dr.Item("CUST_CITY")), "", dr.Item("CUST_CITY")))
            lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("STD_PACK_CNTR")), 0, dr.Item("STD_PACK_CNTR")))
            lsCUSTOMER_FILLER = (IIf(IsDBNull(dr.Item("SALES_DOC_NBR")), 0, dr.Item("SALES_DOC_NBR")))
            'lblSHIP_TO.Text = lsCUSTOMER_FILLER
            'TODO same as DELIVERY_LOC_NBR from ICS?
            lsDELIVERY_LOC_NBR = (IIf(IsDBNull(dr.Item("UNLOADING_POINT")), "", dr.Item("UNLOADING_POINT")))
            'TODO GROSS OR NET WEIGHT?:
            liWEIGHT = (IIf(IsDBNull(dr.Item("GROSS_WGT")), 0, dr.Item("GROSS_WGT")))
            'Dim liSerial_NBR As Long
            'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
            'lsSERIAL_NBR = liSerial_NBR.ToString
            'llRECORD_ID = llRECORD_ID + 1
            'ReadHIST_ICS_Label_Data()
        Catch exole As OracleException
            Me.lblDescription.Text = "Part Not found in SAP File"
            ' Throw New Exception(exole.Message)
            Me.lblDescription.Text = "Part Not found in SAP File"
        Catch ex As Exception
            'Throw New Exception(ex.Message)
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
        Try
            'lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '12040756'"
            lsSQL = "SELECT CUST_NAME_1,MATERIAL_DESC FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '" & txtPart_Nbr.Text & "'"
            lsSQL = lsSQL + " group by CUST_NAME_1,MATERIAL_DESC "
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
            'cmbSAP_Customer.Items.Clear()
            'cmbSAP_Customer.Items.Add("Select Customer Name..")
            While dr.Read()
                '  liSAP_STD_PACK_QTY As Integer
                '  liICS_STD_PACK_QTY As Integer

                'liSAP_STD_PACK_QTY = dr.Item("STD_PACK_QTY")
                'lsPART_NBR = dr.Item("MATERIAL_NBR")
                lsDESCRIPTION = (IIf(IsDBNull(dr.Item("MATERIAL_DESC")), "", dr.Item("MATERIAL_DESC")))
                lblDescription.Text = lsDESCRIPTION
                lsCUSTOMER_NAME = (IIf(IsDBNull(dr.Item("CUST_NAME_1")), "", dr.Item("CUST_NAME_1")))
                Session("CUSTOMER_NAME") = lsCUSTOMER_NAME
                'lsCUSTOMER_STREET_ADDR = (IIf(IsDBNull(dr.Item("CUST_ADDRESS")), "", dr.Item("CUST_ADDRESS")))
                'lsCUSTOMER_CITY = (IIf(IsDBNull(dr.Item("CUST_CITY")), "", dr.Item("CUST_CITY")))
                'lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("STD_PACK_CNTR")), 0, dr.Item("STD_PACK_CNTR")))
                ' cmbSAP_Customer.Items.Add(lsCUSTOMER_NAME)

                'Dim liSerial_NBR As Long
                'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
                'lsSERIAL_NBR = liSerial_NBR.ToString
                'llRECORD_ID = llRECORD_ID + 1
            End While

        Catch exole As OracleException
            Me.lblDescription.Text = "Part Not found in SAP File"
            ' Throw New Exception(exole.Message)
            Me.lblDescription.Text = "Part Not found in SAP File"
        Catch ex As Exception
            'Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
            'ReadHIST_ICS_Label_Data()
        End Try
    End Sub
    Sub GetSAP_Label_DataInfo()
        Dim lsSQL As String
        'Dim conString As String = GetConnectionStringNonOle()
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
            lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("STD_PACK_CNTR")), 0, dr.Item("STD_PACK_CNTR")))

            Dim liSerial_NBR As Long
            liSerial_NBR = CLng(lsSERIAL_NBR + 1)
            lsSERIAL_NBR = liSerial_NBR.ToString
            llRECORD_ID = llRECORD_ID + 1
            'ReadHIST_ICS_Label_Data()
        Catch exole As OracleException
            'Throw New Exception(exole.Message)
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
            Dim sel, whe, vlu, ord
            sel = "SELECT * FROM SNAPMGR.ICS_LABEL_DATA WHERE part_nbr = '" & Me.txtPart_Nbr.Text & "'  "
            whe = " and MATID = " & liMATID
            'whe = " and ECL = '" & txtECL.Text & "' and DESTINATION is null and PACKAGE_CODE = '" & lsPACKAGE_CODE & "'"
            'If txtDestination.Text.Length > 0 Then

            '    whe = " and ECL = '" & txtECL.Text & "' and DESTINATION = '" & txtDestination.Text & "' and PACKAGE_CODE = '" & lsPACKAGE_CODE & "'"
            'End If
            ord = " order by MOD_TMSTM desc "
            lsSQL = sel + whe + ord
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
            '  liSAP_STD_PACK_QTY As Integer
            '  liICS_STD_PACK_QTY As Integer
            liICS_STD_PACK_QTY = dr.Item("quantity")

            Me.liICS_RecordsToCreate = liSAP_STD_PACK_QTY / liICS_STD_PACK_QTY


            llRECORD_ID = llRECORD_ID
            lsSTD_FLAG = (IIf(IsDBNull(dr.Item("STD_FLAG")), "", dr.Item("STD_FLAG")))
            lsPART_NBR = lsPART_NBR
            lsFORM_CODE = " "
            lsDESTINATION = (IIf(IsDBNull(dr.Item("DESTINATION")), "", dr.Item("DESTINATION")))
            liQUANTITY = liICS_STD_PACK_QTY
            liWEIGHT = liWEIGHT ' FROM SAP
            'liWEIGHT = (IIf(IsDBNull(dr.Item("WEIGHT")), 0, dr.Item("WEIGHT")))
            ' Need to determine crossref for STD_PACK_CNTR below:
            'lsECL = (IIf(IsDBNull(dr.Item("ECL")), 0, dr.Item("ECL")))
            'txtECL.Text = "02" 'REMOVE
            lsECL = txtECL.Text ' from screen
            lsSERIAL_NBR = lsSERIAL_NBR ' will be set up below by Max call
            lsDEPT = (IIf(IsDBNull(dr.Item("DEPT")), "", dr.Item("DEPT")))
            lsPACKAGE_CODE = (IIf(IsDBNull(dr.Item("PACKAGE_CODE")), "", dr.Item("PACKAGE_CODE")))
            lsPAY_SUFFIX = (IIf(IsDBNull(dr.Item("PAY_SUFFIX")), "", dr.Item("PAY_SUFFIX")))
            'liMATID = (IIf(IsDBNull(dr.Item("MATID")), 0, dr.Item("MATID")))
            liMATID = liMATID
            lsDESCRIPTION = lsDESCRIPTION ' from SAP
            lsWAREHOUSE_LOC = (IIf(IsDBNull(dr.Item("WAREHOUSE_LOC")), "", dr.Item("WAREHOUSE_LOC")))
            lsCUSTOMER_CODE = (IIf(IsDBNull(dr.Item("CUSTOMER_CODE")), "", dr.Item("CUSTOMER_CODE")))
            'TODO IS THIS THE MES PART# OR CUST PART #
            lsCUSTOMER_PART_NBR = (IIf(IsDBNull(dr.Item("CUSTOMER_PART_NBR")), "", dr.Item("CUSTOMER_PART_NBR")))
            lsBROADCAST_CODE = " " 'TODO BLANK ALWAYS?
            'lsDELIVERY_LOC_NBR = (IIf(IsDBNull(dr.Item("DELIVERY_LOC_NBR")), "", dr.Item("DELIVERY_LOC_NBR")))
            lsDELIVERY_LOC_NBR = lsDELIVERY_LOC_NBR  ' FROM SAP UNLOADING_POINT
            lsREF_SEQ_DATA_ID = (IIf(IsDBNull(dr.Item("REF_SEQ_DATA_ID")), "", dr.Item("REF_SEQ_DATA_ID")))

            'TODO shoud come from SAP? needed for label printing. 
            lsREF_SEQ_NBR = (IIf(IsDBNull(dr.Item("REF_SEQ_NBR")), "", dr.Item("REF_SEQ_NBR")))

            'TODO shoud come from SAP? needed for label printing. missing last 2 characters in label 
            lsPKG_12Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_12Z_SEGM_NBR")), "", dr.Item("PKG_12Z_SEGM_NBR")))
            lsPKG_13Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_13Z_SEGM_NBR")), "", dr.Item("PKG_13Z_SEGM_NBR")))
            lsPKG_14Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_14Z_SEGM_NBR")), "", dr.Item("PKG_14Z_SEGM_NBR")))
            lsPKG_15Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_15Z_SEGM_NBR")), "", dr.Item("PKG_15Z_SEGM_NBR")))
            lsPKG_16Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_16Z_SEGM_NBR")), "", dr.Item("PKG_16Z_SEGM_NBR")))
            lsPKG_17Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_17Z_SEGM_NBR")), "", dr.Item("PKG_17Z_SEGM_NBR")))
            lsCUSTOMER_NAME = lsCUSTOMER_NAME ' from sap
            Session("CUSTOMER_NAME") = lsCUSTOMER_NAME
            lsCUSTOMER_STREET_ADDR = lsCUSTOMER_STREET_ADDR ' from sap
            lsCUSTOMER_CITY = lsCUSTOMER_CITY
            lsICS_CNTR_PACKAGE_TYPE = lsICS_CNTR_PACKAGE_TYPE ' from sap
            'lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("ICS_CNTR_PACKAGE_TYPE")), 0, dr.Item("ICS_CNTR_PACKAGE_TYPE")))
            liCHECK_SUM = 4
            liUSED_STATUS = 0
            lsTIME_USED = Now().ToString("dd-MMM-yy")
            lsMACHINE_NAME = (IIf(IsDBNull(dr.Item("MACHINE_NAME")), 0, dr.Item("MACHINE_NAME")))
            lsMOD_USERID = "AUTO_SAP"
            lsMOD_TMSTM = Now().ToString("dd-MMM-yy")
            lsCUSTOMER_FILLER = lsCUSTOMER_FILLER  ' send sales_Doc_nbr for kiosk reading of unique label record from SAP

            'lsCUSTOMER_FILLER = (IIf(IsDBNull(dr.Item("CUSTOMER_FILLER")), 0, dr.Item("CUSTOMER_FILLER")))
            lsCUSTOMER_PN_FILLER = (IIf(IsDBNull(dr.Item("CUSTOMER_PN_FILLER")), 0, dr.Item("CUSTOMER_PN_FILLER")))
            lsCISCO_CODE = (IIf(IsDBNull(dr.Item("CISCO_CODE")), 0, dr.Item("CISCO_CODE")))
            lsREQ_STORE = (IIf(IsDBNull(dr.Item("REQ_STORE")), 0, dr.Item("REQ_STORE")))
            liRecordsFound = liRecordsFound + 1

        Catch exole As OracleException
            'Throw New Exception(exole.Message)
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
                ' liICS_RecordsToCreate = CInt(txtNoOfLabels.Text)
                Dim i As Integer
                For i = 1 To liICS_RecordsToCreate
                    liSerial_NBR = CLng(lsSERIAL_NBR + 1)
                    lsSERIAL_NBR = liSerial_NBR.ToString
                    llRECORD_ID = llRECORD_ID + 1
                    Me.InsertICS_MATID()
                Next
                lblStatus.ForeColor = Color.Navy
                lblStatus.Text = "records inserted: " & liICS_RecordsCreated.ToString
            Else
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "No Previous data found : " & liICS_RecordsCreated.ToString
            End If
        End Try
    End Sub
    Sub VerifyIfFieldsAreComplete()
        Try
            If txtPart_Nbr.Text.Length = 0 Then
                lblStatus.Text = "Missing Part Number "
                Session("ERROR") = "YES"
                Exit Sub
            End If

            If txtPAY_SUFFIX.Text.Length = 0 Then
                lblStatus.Text = "Missing Pay Suffix "
                Session("ERROR") = "YES"
                Exit Sub
            End If
            If txtSTD_FLAG.Text.Length = 0 Then
                lblStatus.Text = "Missing Type "
                Session("ERROR") = "YES"
                Exit Sub
            End If

            If txtDestination.Text.Length = 0 And txtSTD_FLAG.Text = "I" Then
                lblStatus.Text = "Missing Destination "
                Session("ERROR") = "YES"
                Exit Sub
            End If
            'If txtCUSTOMER_CODE.Text.Length = 0 Then
            '    lblStatus.Text = "Customer Code "
            '    Session("ERROR") = "YES"
            '    Exit Sub
            'End If
            If txtDept.Text.Length = 0 Then
                lblStatus.Text = "Missing Dept"
                Session("ERROR") = "YES"
                Exit Sub
            End If
            If txtECL.Text.Length = 0 Then
                lblStatus.Text = "Missing ECL "
                Session("ERROR") = "YES"
                Exit Sub
            End If

            If txtQUANTITY.Text.Length = 0 Then
                lblStatus.Text = "Missing Qty "
                Session("ERROR") = "YES"
                Exit Sub
            End If
            If Not IsNumeric(txtQUANTITY.Text) Then
                lblStatus.Text = "Non Numeric Qty "
                txtQUANTITY.Text = "0"
                Session("ERROR") = "YES"
                Exit Sub
            End If

            Dim liQuantity As Integer
            liQuantity = CInt(txtQUANTITY.Text)
            If liQuantity < 1 Then
                lblStatus.Text = "Non Numeric Qty "
                txtQUANTITY.Text = "0"
                Session("ERROR") = "YES"
                Exit Sub
            End If


            If txtPackageCode.Text.Length = 0 Then
                lblStatus.Text = "Missing Package Code "
                Session("ERROR") = "YES"
                Exit Sub
            End If
            If txtWEIGHT.Text.Length = 0 Then
                lblStatus.Text = "Missing Weight "
                txtWEIGHT.Text = "0"
                Session("ERROR") = "YES"
                Exit Sub
            End If
            If Not IsNumeric(txtWEIGHT.Text) Then
                lblStatus.Text = "Missing Weight "
                txtWEIGHT.Text = "0"
                Session("ERROR") = "YES"
                Exit Sub
            End If

            If CInt(txtWEIGHT.Text) < 1 Then
                lblStatus.Text = "Missing Weight "
                txtWEIGHT.Text = "0"
                Session("ERROR") = "YES"
                Exit Sub
            End If

            If txtCUST_PART_NBR.Text.Length = 0 Then
                lblStatus.Text = "Missing Cust Part Number from SAP"
                Session("ERROR") = "YES"
                Exit Sub
            End If
            Session("ERROR") = "NO"
        Catch exole As OracleException
            Throw New Exception(exole.Message)
            lblStatus.ForeColor = Color.DarkBlue
            lblStatus.Text = "SAPID does not exist for these fields -  Please click to 'Generate MATID' to create a new record"
        Catch ex As Exception
            lblStatus.ForeColor = Color.DarkBlue
            lblStatus.Text = "SAPID does not exist for these fields -  Please click to 'Generate MATID' to create a new record"
            Throw New Exception(ex.Message)
        Finally

        End Try
    End Sub
    Sub VerifyAllFieldForMATIDExistanceICS()
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

            Dim sel, whe, vlu, ord, whe1, whe2, whe3, whe4, whe5
            sel = "SELECT * FROM SNAPMGR.ICS_LABEL_DATA  "
            whe = " where part_nbr = '" & txtPart_Nbr.Text & "' and PAY_SUFFIX = '" & txtPAY_SUFFIX.Text & "'"
            whe1 = " and STD_FLAG = '" & txtSTD_FLAG.Text & "' and Destination = '" & txtDestination.Text & "'"
            whe2 = " and CUSTOMER_CODE = '" & txtCUSTOMER_CODE.Text & "' and Dept = '" & txtDept.Text & "'"
            whe3 = " and ECL = '" & txtECL.Text & "' and QUANTITY = " & CInt(txtQUANTITY.Text) & ""
            whe4 = " and Package_Code = '" & txtPackageCode.Text & "' and WEIGHT = " & CInt(txtWEIGHT.Text) & ""
            whe5 = " and CUSTOMER_PART_NBR = '" & txtCUST_PART_NBR.Text & "' "
            'If txtDestination.Text.Length > 0 Then
            '    whe = " and ECL = '" & txtECL.Text & "' and DESTINATION = '" & txtDestination.Text & "' and PACKAGE_CODE = '" & lsPACKAGE_CODE & "'"
            'End If
            ord = " order by MOD_TMSTM desc "
            lsSQL = sel + whe + whe1 + whe2 + whe3 + whe4 + whe5 + ord
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

            lbDataError = False

            liICS_STD_PACK_QTY = dr.Item("quantity")
            lblCreateMATID.Text = "NO"
            liICS_RecordsToCreate = liSAP_STD_PACK_QTY / liICS_STD_PACK_QTY


            'llRECORD_ID = llRECORD_ID

            'lsPART_NBR = lsPART_NBR
            'lsFORM_CODE = " "
            'lsSTD_FLAG = (IIf(IsDBNull(dr.Item("STD_FLAG")), "", dr.Item("STD_FLAG")))
            'txtSTD_FLAG.Text = lsSTD_FLAG

            'lsDESTINATION = (IIf(IsDBNull(dr.Item("DESTINATION")), "", dr.Item("DESTINATION")))
            'txtDestination.Text = lsDESTINATION
            'liQUANTITY = liICS_STD_PACK_QTY
            'liWEIGHT = (IIf(IsDBNull(dr.Item("WEIGHT")), 0, dr.Item("WEIGHT")))
            '' Need to determine crossref for STD_PACK_CNTR below:
            'lsECL = (IIf(IsDBNull(dr.Item("ECL")), 0, dr.Item("ECL")))
            'txtECL.Text = lsECL
            ''lsECL = txtECL.Text ' from screen
            'lsSERIAL_NBR = lsSERIAL_NBR ' will be set up below by Max call
            'lsDEPT = "2341" ' remove 
            'lsPACKAGE_CODE = (IIf(IsDBNull(dr.Item("PACKAGE_CODE")), "", dr.Item("PACKAGE_CODE")))
            'txtPackageCode.Text = lsPACKAGE_CODE
            'lsPAY_SUFFIX = (IIf(IsDBNull(dr.Item("PAY_SUFFIX")), "", dr.Item("PAY_SUFFIX")))
            liMATID = (IIf(IsDBNull(dr.Item("MATID")), 0, dr.Item("MATID")))
            Session("MATID") = liMATID
            'lsDESCRIPTION = (IIf(IsDBNull(dr.Item("DESCRIPTION")), 0, dr.Item("DESCRIPTION")))
            'Me.lblDescription.Text = lsDESCRIPTION ' from SAP
            'lsWAREHOUSE_LOC = (IIf(IsDBNull(dr.Item("WAREHOUSE_LOC")), "", dr.Item("WAREHOUSE_LOC")))
            'lsCUSTOMER_CODE = (IIf(IsDBNull(dr.Item("CUSTOMER_CODE")), "", dr.Item("CUSTOMER_CODE")))
            ''TODO IS THIS THE MES PART# OR CUST PART #
            'lsCUSTOMER_PART_NBR = (IIf(IsDBNull(dr.Item("CUSTOMER_PART_NBR")), "", dr.Item("CUSTOMER_PART_NBR")))
            'lsBROADCAST_CODE = " " ' BLANK ALWAYS?
            'lsDELIVERY_LOC_NBR = (IIf(IsDBNull(dr.Item("DELIVERY_LOC_NBR")), "", dr.Item("DELIVERY_LOC_NBR")))
            ''lsREF_SEQ_DATA_ID = (IIf(IsDBNull(dr.Item("REF_SEQ_DATA_ID")), "", dr.Item("REF_SEQ_DATA_ID")))
            '' lsREF_SEQ_NBR = (IIf(IsDBNull(dr.Item("REF_SEQ_NBR")), "", dr.Item("REF_SEQ_NBR")))
            ''lsPKG_12Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_12Z_SEGM_NBR")), "", dr.Item("PKG_12Z_SEGM_NBR")))
            ''lsPKG_13Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_13Z_SEGM_NBR")), "", dr.Item("PKG_13Z_SEGM_NBR")))
            ''lsPKG_14Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_14Z_SEGM_NBR")), "", dr.Item("PKG_14Z_SEGM_NBR")))
            ''lsPKG_15Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_15Z_SEGM_NBR")), "", dr.Item("PKG_15Z_SEGM_NBR")))
            ''lsPKG_16Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_16Z_SEGM_NBR")), "", dr.Item("PKG_16Z_SEGM_NBR")))
            ''lsPKG_17Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_17Z_SEGM_NBR")), "", dr.Item("PKG_17Z_SEGM_NBR")))
            'lsCUSTOMER_NAME = lsCUSTOMER_NAME
            'lsCUSTOMER_STREET_ADDR = lsCUSTOMER_STREET_ADDR
            'lsCUSTOMER_CITY = lsCUSTOMER_CITY
            'lsICS_CNTR_PACKAGE_TYPE = lsICS_CNTR_PACKAGE_TYPE
            ''lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("ICS_CNTR_PACKAGE_TYPE")), 0, dr.Item("ICS_CNTR_PACKAGE_TYPE")))
            'liCHECK_SUM = 4
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

        Catch exole As OracleException
            'Throw New Exception(exole.Message)
            lblCreateMATID.Text = "YES"
            lblStatus.ForeColor = Color.DarkBlue
            lblStatus.Text = "SAPID does not exist for these fields -  Please click to create new MATID"
        Catch ex As Exception
            lblCreateMATID.Text = "YES"
            lblStatus.ForeColor = Color.DarkBlue
            lblStatus.Text = "SAPID does not exist for these fields -  Please click to create new MATID"
            'Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
            If liRecordsFound > 0 Then
                Me.txtMATID.Text = Me.liMATID
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "MATID already exist for these fields "
            End If

        End Try
    End Sub
    Sub VerifyAllFieldForMATIDExistance()
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


            Dim sel, whe, vlu, ord, whe1, whe2, whe3, whe4, whe5
            sel = "SELECT * FROM pltfloor.ICS_MATID "
            whe = " where part_nbr = '" & txtPart_Nbr.Text & "' and PAY_SUFFIX = '" & txtPAY_SUFFIX.Text & "'"
            whe1 = " and STD_FLAG = '" & txtSTD_FLAG.Text & "' and Destination = '" & txtDestination.Text & "'"
            'whe2 = " and CUSTOMER_CODE = '" & txtCUSTOMER_CODE.Text & "' and Dept = '" & txtDept.Text & "'"
            whe2 = "  and Dept = '" & txtDept.Text & "'"
            whe3 = " and ECL = '" & txtECL.Text & "' and QUANTITY = " & CInt(txtQUANTITY.Text) & ""
            'whe4 = " and Package_Code = '" & txtPackageCode.Text & "' and WEIGHT = " & CInt(txtWEIGHT.Text) & ""
            whe4 = " and Package_Code = '" & txtPackageCode.Text & "' "
            whe5 = " and CUSTOMER_PART_NBR = '" & txtCUST_PART_NBR.Text & "' "

            'If txtDestination.Text.Length > 0 Then
            '    whe = " and ECL = '" & txtECL.Text & "' and DESTINATION = '" & txtDestination.Text & "' and PACKAGE_CODE = '" & lsPACKAGE_CODE & "'"
            'End If

            ord = " order by MOD_TMSTM desc "
            lsSQL = sel + whe + whe1 + whe2 + whe3 + whe4 + whe5 + ord

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

            liICS_STD_PACK_QTY = dr.Item("quantity")
            lblCreateMATID.Text = "NO"
            liICS_RecordsToCreate = liSAP_STD_PACK_QTY / liICS_STD_PACK_QTY


            'llRECORD_ID = llRECORD_ID

            'lsPART_NBR = lsPART_NBR
            'lsFORM_CODE = " "
            'lsSTD_FLAG = (IIf(IsDBNull(dr.Item("STD_FLAG")), "", dr.Item("STD_FLAG")))
            'txtSTD_FLAG.Text = lsSTD_FLAG

            'lsDESTINATION = (IIf(IsDBNull(dr.Item("DESTINATION")), "", dr.Item("DESTINATION")))
            'txtDestination.Text = lsDESTINATION
            'liQUANTITY = liICS_STD_PACK_QTY
            'liWEIGHT = (IIf(IsDBNull(dr.Item("WEIGHT")), 0, dr.Item("WEIGHT")))
            '' Need to determine crossref for STD_PACK_CNTR below:
            'lsECL = (IIf(IsDBNull(dr.Item("ECL")), 0, dr.Item("ECL")))
            'txtECL.Text = lsECL
            ''lsECL = txtECL.Text ' from screen
            'lsSERIAL_NBR = lsSERIAL_NBR ' will be set up below by Max call
            'lsDEPT = "2341" ' remove 
            'lsPACKAGE_CODE = (IIf(IsDBNull(dr.Item("PACKAGE_CODE")), "", dr.Item("PACKAGE_CODE")))
            'txtPackageCode.Text = lsPACKAGE_CODE
            'lsPAY_SUFFIX = (IIf(IsDBNull(dr.Item("PAY_SUFFIX")), "", dr.Item("PAY_SUFFIX")))
            liMATID = (IIf(IsDBNull(dr.Item("MATID")), 0, dr.Item("MATID")))
            Session("MATID") = liMATID
            'lsDESCRIPTION = (IIf(IsDBNull(dr.Item("DESCRIPTION")), 0, dr.Item("DESCRIPTION")))
            'Me.lblDescription.Text = lsDESCRIPTION ' from SAP
            'lsWAREHOUSE_LOC = (IIf(IsDBNull(dr.Item("WAREHOUSE_LOC")), "", dr.Item("WAREHOUSE_LOC")))
            'lsCUSTOMER_CODE = (IIf(IsDBNull(dr.Item("CUSTOMER_CODE")), "", dr.Item("CUSTOMER_CODE")))
            ''TODO IS THIS THE MES PART# OR CUST PART #
            'lsCUSTOMER_PART_NBR = (IIf(IsDBNull(dr.Item("CUSTOMER_PART_NBR")), "", dr.Item("CUSTOMER_PART_NBR")))
            'lsBROADCAST_CODE = " " ' BLANK ALWAYS?
            'lsDELIVERY_LOC_NBR = (IIf(IsDBNull(dr.Item("DELIVERY_LOC_NBR")), "", dr.Item("DELIVERY_LOC_NBR")))
            ''lsREF_SEQ_DATA_ID = (IIf(IsDBNull(dr.Item("REF_SEQ_DATA_ID")), "", dr.Item("REF_SEQ_DATA_ID")))
            '' lsREF_SEQ_NBR = (IIf(IsDBNull(dr.Item("REF_SEQ_NBR")), "", dr.Item("REF_SEQ_NBR")))
            ''lsPKG_12Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_12Z_SEGM_NBR")), "", dr.Item("PKG_12Z_SEGM_NBR")))
            ''lsPKG_13Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_13Z_SEGM_NBR")), "", dr.Item("PKG_13Z_SEGM_NBR")))
            ''lsPKG_14Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_14Z_SEGM_NBR")), "", dr.Item("PKG_14Z_SEGM_NBR")))
            ''lsPKG_15Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_15Z_SEGM_NBR")), "", dr.Item("PKG_15Z_SEGM_NBR")))
            ''lsPKG_16Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_16Z_SEGM_NBR")), "", dr.Item("PKG_16Z_SEGM_NBR")))
            ''lsPKG_17Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_17Z_SEGM_NBR")), "", dr.Item("PKG_17Z_SEGM_NBR")))
            'lsCUSTOMER_NAME = lsCUSTOMER_NAME
            'lsCUSTOMER_STREET_ADDR = lsCUSTOMER_STREET_ADDR
            'lsCUSTOMER_CITY = lsCUSTOMER_CITY
            'lsICS_CNTR_PACKAGE_TYPE = lsICS_CNTR_PACKAGE_TYPE
            ''lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("ICS_CNTR_PACKAGE_TYPE")), 0, dr.Item("ICS_CNTR_PACKAGE_TYPE")))
            'liCHECK_SUM = 4
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

        Catch exole As OracleException
            'Throw New Exception(exole.Message)
            lblCreateMATID.Text = "YES"
            lblStatus.ForeColor = Color.DarkBlue
            lblStatus.Text = "SAPID does not exist for these fields -  Please click on 'Generate MATID' to create a new record"
        Catch ex As Exception
            lblCreateMATID.Text = "YES"
            lblStatus.ForeColor = Color.DarkBlue
            lblStatus.Text = "SAPID does not exist for these fields -  Please click to 'Generate MATID' to create a new record"
            'Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
            If liRecordsFound > 0 Then
                Me.txtMATID.Text = Me.liMATID
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "MATID already exist for these fields "
            End If

        End Try
    End Sub
    Sub GetMATID_AndLoadFields()
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

            Dim sel, whe
            sel = "SELECT * FROM pltfloor.ICS_MATID  "
            whe = " where MATID = " & CInt(txtMATID.Text)

            lsSQL = sel + whe

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

            liICS_STD_PACK_QTY = dr.Item("quantity")

            'llRECORD_ID = llRECORD_ID

            'lsPART_NBR = lsPART_NBR
            'lsFORM_CODE = " "
            txtPart_Nbr.Text = (IIf(IsDBNull(dr.Item("Part_Nbr")), "", dr.Item("Part_Nbr")))
            cmbPart_Nbr.SelectedValue = txtPart_Nbr.Text
            Session("MATID") = CInt(txtMATID.Text)
            lsSTD_FLAG = (IIf(IsDBNull(dr.Item("STD_FLAG")), "", dr.Item("STD_FLAG")))
            cmbSTD_FLAG.SelectedValue = lsSTD_FLAG
            txtPAY_SUFFIX.Text = (IIf(IsDBNull(dr.Item("PAY_SUFFIX")), "", dr.Item("PAY_SUFFIX")))
            lsDESTINATION = (IIf(IsDBNull(dr.Item("DESTINATION")), "", dr.Item("DESTINATION")))
            ddlDestination.SelectedValue = lsDESTINATION
            txtDestination.Text = lsDESTINATION
            txtQUANTITY.Text = (IIf(IsDBNull(dr.Item("QUANTITY")), 0, dr.Item("QUANTITY")))
            txtWEIGHT.Text = (IIf(IsDBNull(dr.Item("WEIGHT")), 0, dr.Item("WEIGHT")))
            txtECL.Text = (IIf(IsDBNull(dr.Item("ECL")), "", dr.Item("ECL")))
            txtDept.Text = (IIf(IsDBNull(dr.Item("DEPT")), "", dr.Item("DEPT")))
            ddlDept.SelectedValue = txtDept.Text
            txtCUSTOMER_CODE.Text = (IIf(IsDBNull(dr.Item("CUSTOMER_CODE")), "", dr.Item("CUSTOMER_CODE")))

            txtPackageCode.Text = (IIf(IsDBNull(dr.Item("PACKAGE_CODE")), "", dr.Item("PACKAGE_CODE")))
            ddlPackageCode.SelectedValue = txtPackageCode.Text
            txtSHIP_TO.Text = (IIf(IsDBNull(dr.Item("ship_to")), "", dr.Item("ship_to")))
            'ddlSHIP_TO.SelectedValue = txtSHIP_TO.Text
            txtCUST_PART_NBR.Text = (IIf(IsDBNull(dr.Item("CUSTOMER_PART_NBR")), "", dr.Item("CUSTOMER_PART_NBR")))
            'ddlCUST_PART_NBR.SelectedValue = txtCUST_PART_NBR.Text
            txtSTATUS.Text = (IIf(IsDBNull(dr.Item("STATUS")), "", dr.Item("STATUS")))
            ddlStatus.SelectedValue = txtSTATUS.Text
            liRecordsFound = liRecordsFound + 1

        Catch exole As OracleException
            'Throw New Exception(exole.Message)
            lblCreateMATID.Text = "YES"
            lblStatus.ForeColor = Color.DarkBlue
            lblStatus.Text = "Invalid MATID  "
        Catch ex As Exception
            lblCreateMATID.Text = "YES"
            lblStatus.ForeColor = Color.DarkBlue
            lblStatus.Text = "Invalid MATID   "
            'Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
            If liRecordsFound > 0 Then
                GetSHIP_TOAndLoadCombo()
                If txtSHIP_TO.Text.Length > 4 Then
                    ddlSHIP_TO.SelectedValue = txtSHIP_TO.Text
                End If
                GetCust_Part_NumberAndLoadCombo()
                If txtCUST_PART_NBR.Text.Length > 5 Then
                    ddlCUST_PART_NBR.SelectedValue = txtCUST_PART_NBR.Text
                End If
                ' Me.txtMATID.Text = Me.liMATID
                'lblStatus.ForeColor = Color.Red
                'lblStatus.Text = "MATID already exist for these fields "
            End If

        End Try
    End Sub


    Sub VerifyPartNumbers()
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

            Dim sel, whe, vlu, ord, whe1, whe2, whe3, whe4, whe5
            sel = "SELECT * FROM mesdba.MES_PART "
            whe = " where part_nbr = '" & txtPart_Nbr.Text & "' and PART_id = '" & txtCUST_PART_NBR.Text & "'"
            'whe1 = " and STD_FLAG = '" & txtSTD_FLAG.Text & "' and Destination = '" & txtDestination.Text & "'"
            'whe2 = " and CUSTOMER_CODE = '" & txtCUSTOMER_CODE.Text & "' and Dept = '" & txtDept.Text & "'"
            'whe3 = " and ECL = '" & txtECL.Text & "' and QUANTITY = " & CInt(txtQUANTITY.Text) & ""
            'whe4 = " and Package_Code = '" & txtPackageCode.Text & "' and WEIGHT = " & CInt(txtWEIGHT.Text) & ""
            'whe5 = " and CUSTOMER_PART_NBR = '" & txtCUST_PART_NBR.Text & "' "
            'If txtDestination.Text.Length > 0 Then
            '    whe = " and ECL = '" & txtECL.Text & "' and DESTINATION = '" & txtDestination.Text & "' and PACKAGE_CODE = '" & lsPACKAGE_CODE & "'"
            'End If
            ' ord = " order by MOD_TMSTM desc "
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

            lbDataError = False

            'liICS_STD_PACK_QTY = dr.Item("quantity")

            'liICS_RecordsToCreate = liSAP_STD_PACK_QTY / liICS_STD_PACK_QTY


            'llRECORD_ID = llRECORD_ID

            'lsPART_NBR = lsPART_NBR
            'lsFORM_CODE = " "
            'lsSTD_FLAG = (IIf(IsDBNull(dr.Item("STD_FLAG")), "", dr.Item("STD_FLAG")))
            'txtSTD_FLAG.Text = lsSTD_FLAG

            'lsDESTINATION = (IIf(IsDBNull(dr.Item("DESTINATION")), "", dr.Item("DESTINATION")))
            'txtDestination.Text = lsDESTINATION
            'liQUANTITY = liICS_STD_PACK_QTY
            'liWEIGHT = (IIf(IsDBNull(dr.Item("WEIGHT")), 0, dr.Item("WEIGHT")))
            '' Need to determine crossref for STD_PACK_CNTR below:
            'lsECL = (IIf(IsDBNull(dr.Item("ECL")), 0, dr.Item("ECL")))
            'txtECL.Text = lsECL
            ''lsECL = txtECL.Text ' from screen
            'lsSERIAL_NBR = lsSERIAL_NBR ' will be set up below by Max call
            'lsDEPT = "2341" ' remove 
            'lsPACKAGE_CODE = (IIf(IsDBNull(dr.Item("PACKAGE_CODE")), "", dr.Item("PACKAGE_CODE")))
            'txtPackageCode.Text = lsPACKAGE_CODE
            'lsPAY_SUFFIX = (IIf(IsDBNull(dr.Item("PAY_SUFFIX")), "", dr.Item("PAY_SUFFIX")))
            liMATID = (IIf(IsDBNull(dr.Item("MATID")), 0, dr.Item("MATID")))
            Session("MATID") = liMATID
            'lsDESCRIPTION = (IIf(IsDBNull(dr.Item("DESCRIPTION")), 0, dr.Item("DESCRIPTION")))
            'Me.lblDescription.Text = lsDESCRIPTION ' from SAP
            'lsWAREHOUSE_LOC = (IIf(IsDBNull(dr.Item("WAREHOUSE_LOC")), "", dr.Item("WAREHOUSE_LOC")))
            'lsCUSTOMER_CODE = (IIf(IsDBNull(dr.Item("CUSTOMER_CODE")), "", dr.Item("CUSTOMER_CODE")))
            ''TODO IS THIS THE MES PART# OR CUST PART #
            'lsCUSTOMER_PART_NBR = (IIf(IsDBNull(dr.Item("CUSTOMER_PART_NBR")), "", dr.Item("CUSTOMER_PART_NBR")))
            'lsBROADCAST_CODE = " " ' BLANK ALWAYS?
            'lsDELIVERY_LOC_NBR = (IIf(IsDBNull(dr.Item("DELIVERY_LOC_NBR")), "", dr.Item("DELIVERY_LOC_NBR")))
            ''lsREF_SEQ_DATA_ID = (IIf(IsDBNull(dr.Item("REF_SEQ_DATA_ID")), "", dr.Item("REF_SEQ_DATA_ID")))
            '' lsREF_SEQ_NBR = (IIf(IsDBNull(dr.Item("REF_SEQ_NBR")), "", dr.Item("REF_SEQ_NBR")))
            ''lsPKG_12Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_12Z_SEGM_NBR")), "", dr.Item("PKG_12Z_SEGM_NBR")))
            ''lsPKG_13Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_13Z_SEGM_NBR")), "", dr.Item("PKG_13Z_SEGM_NBR")))
            ''lsPKG_14Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_14Z_SEGM_NBR")), "", dr.Item("PKG_14Z_SEGM_NBR")))
            ''lsPKG_15Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_15Z_SEGM_NBR")), "", dr.Item("PKG_15Z_SEGM_NBR")))
            ''lsPKG_16Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_16Z_SEGM_NBR")), "", dr.Item("PKG_16Z_SEGM_NBR")))
            ''lsPKG_17Z_SEGM_NBR = (IIf(IsDBNull(dr.Item("PKG_17Z_SEGM_NBR")), "", dr.Item("PKG_17Z_SEGM_NBR")))
            'lsCUSTOMER_NAME = lsCUSTOMER_NAME
            'lsCUSTOMER_STREET_ADDR = lsCUSTOMER_STREET_ADDR
            'lsCUSTOMER_CITY = lsCUSTOMER_CITY
            'lsICS_CNTR_PACKAGE_TYPE = lsICS_CNTR_PACKAGE_TYPE
            ''lsICS_CNTR_PACKAGE_TYPE = (IIf(IsDBNull(dr.Item("ICS_CNTR_PACKAGE_TYPE")), 0, dr.Item("ICS_CNTR_PACKAGE_TYPE")))
            'liCHECK_SUM = 4
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

        Catch exole As OracleException
            'Throw New Exception(exole.Message)
            lblStatus.ForeColor = Color.DarkBlue
            lblStatus.Text = "Invalid Part / Customer Part combination"
        Catch ex As Exception
            lblStatus.ForeColor = Color.DarkBlue
            lblStatus.Text = "Invalid Part / Customer Part combination"
            'Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()
            If liRecordsFound > 0 Then
                Me.txtMATID.Text = Me.liMATID
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "Valid part "
            End If

        End Try
    End Sub
    Sub CleanAllFields()
        liMATID = 0
        lblCUSTOMER_NAME.Text = ""
        lblDescription.Text = ""
        lsCUSTOMER_NAME = ""
        lsCUSTOMER_PART_NBR = ""
        lsMaterial_Nbr = ""
        txtPart_Nbr.Text = ""
        txtPAY_SUFFIX.Text = ""
        txtSTD_FLAG.Text = ""
        txtDestination.Text = ""
        txtDept.Text = ""
        txtCUSTOMER_CODE.Text = ""
        txtECL.Text = ""
        txtQUANTITY.Text = ""
        txtWEIGHT.Text = ""
        txtPackageCode.Text = ""
        txtCUST_PART_NBR.Text = ""
        'txtMATID.Text = ""
        txtMATID.BackColor = Color.Gainsboro
        btnCreateMATID.Visible = False
        Me.lblStatus.Text = ""
        UnLockAllFields()
    End Sub
    Sub UnLockAllFields()
        txtCUSTOMER_CODE.Enabled = True
        txtCUST_PART_NBR.Enabled = True
        txtDept.Enabled = True
        txtDestination.Enabled = True
        txtECL.Enabled = True
        txtMATID.Enabled = True
        txtPackageCode.Enabled = True
        txtPart_Nbr.Enabled = True
        txtPAY_SUFFIX.Enabled = True
        txtQUANTITY.Enabled = True
        txtSTD_FLAG.Enabled = True
        txtWEIGHT.Enabled = True
        'Me.btnCreateMATID.Visible = True

        txtCUSTOMER_CODE.BackColor = Color.LightYellow
        txtCUST_PART_NBR.BackColor = Color.LightYellow
        txtDept.BackColor = Color.LightYellow
        txtDestination.BackColor = Color.LightYellow
        txtECL.BackColor = Color.LightYellow
        txtMATID.BackColor = Color.LightYellow
        txtPackageCode.BackColor = Color.LightYellow
        txtPart_Nbr.BackColor = Color.LightYellow
        txtPAY_SUFFIX.BackColor = Color.LightYellow
        txtQUANTITY.BackColor = Color.LightYellow
        txtSTD_FLAG.BackColor = Color.LightYellow
        txtWEIGHT.BackColor = Color.LightYellow


        cmbPart_Nbr.Enabled = True
        ddlStatus.Enabled = True
        cmbSTD_FLAG.Enabled = True
        ddlDestination.Enabled = True
        ddlDept.Enabled = True
        ddlSHIP_TO.Enabled = True
        ddlPackageCode.Enabled = True
        ddlCUST_PART_NBR.Enabled = True
    End Sub

    Sub LockAllFields()
        txtCUSTOMER_CODE.Enabled = False
        txtCUST_PART_NBR.Enabled = False
        txtDept.Enabled = False
        txtDestination.Enabled = False
        txtECL.Enabled = False
        'txtMATID.Enabled = False
        txtPackageCode.Enabled = False
        txtPart_Nbr.Enabled = False
        txtPAY_SUFFIX.Enabled = False
        txtQUANTITY.Enabled = False
        txtSTD_FLAG.Enabled = False
        txtWEIGHT.Enabled = False
        Me.btnCreateMATID.Visible = False

        txtCUSTOMER_CODE.BackColor = Color.LightGray
        txtCUST_PART_NBR.BackColor = Color.LightGray
        txtDept.BackColor = Color.LightGray
        txtDestination.BackColor = Color.LightGray
        txtECL.BackColor = Color.LightGray
        txtMATID.BackColor = Color.LightGray
        txtPackageCode.BackColor = Color.LightGray
        txtPart_Nbr.BackColor = Color.LightGray
        txtPAY_SUFFIX.BackColor = Color.LightGray
        txtQUANTITY.BackColor = Color.LightGray
        txtSTD_FLAG.BackColor = Color.LightGray
        txtWEIGHT.BackColor = Color.LightGray

        cmbPart_Nbr.Enabled = False
        ddlStatus.Enabled = False
        cmbSTD_FLAG.Enabled = False
        ddlDestination.Enabled = False
        ddlDept.Enabled = False
        ddlSHIP_TO.Enabled = False
        ddlPackageCode.Enabled = False
        ddlCUST_PART_NBR.Enabled = False




    End Sub
    Sub CleanVerificationFields()
        lblCUSTOMER_NAME.Text = ""
        lblDescription.Text = ""
        lsCUSTOMER_NAME = ""
        lsCUSTOMER_PART_NBR = ""
        lsMaterial_Nbr = ""
        lsPART_NBR = ""
        txtCUSTOMER_CODE.Text = ""
        txtCUST_PART_NBR.Text = ""
        txtDept.Text = ""
        txtDestination.Text = ""
        txtECL.Text = ""
        txtMATID.Text = ""
        txtPackageCode.Text = ""
        txtPart_Nbr.Text = ""
        txtPAY_SUFFIX.Text = ""
        txtQUANTITY.Text = ""
        txtSTD_FLAG.Text = ""
        txtWEIGHT.Text = ""

    End Sub


    Private Sub btnVerifyMATID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerifyMATID.Click

        Try
            txtDestination.Text = txtDestination.Text.ToUpper
            Session("ERROR") = "YES"
            VerifyIfFieldsAreComplete()
            If Session("ERROR") = "YES" Then
                Exit Sub
            End If
            lbDataError = True
            lblCreateMATID.Text = "NO"
            '  VerifyAllFieldForMATIDExistance()
            'Me.VerifyPartNumbers()
            Me.VerifyAllFieldForMATIDExistance()
            btnCreateMATID.Visible = False
            If lblCreateMATID.Text = "YES" Then
                btnCreateMATID.Visible = True
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
            'Throw New Exception(ex.Message)
        Finally

        End Try
    End Sub

    Friend Function GetConnectionStringNonOle() As String
        'instead of user legacy OleDB, use faster Oracle methods
        Dim conString As String = GetConnectionString()
        Dim liClean As Integer = InStr(conString, ";")
        conString = Mid(conString, liClean + 1)
        Return conString
    End Function

    Private Sub btnCreateMATID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateMATID.Click

        Session("ERROR") = "YES"
        VerifyIfFieldsAreComplete()
        If Session("ERROR") = "YES" Then
            Exit Sub
        End If
        InsertICS_MATID()
        LockAllFields()
        btnCreateMATID.Visible = False

    End Sub

    Sub GetPartsAndLoadCombo()
        Dim lsSQL As String
        Dim conString As String = GetConnectionStringNonOle()
        'Dim liClean As Integer = InStr(conString, ";")
        'conString = Mid(conString, liClean + 1)

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
        Dim lsICSCity As String = Session("ICS_CUSTOMER_CITY")
        Try
            'lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '12040756'"
            lsSQL = "SELECT distinct part_Nbr FROM mesdba.mes_part order by Part_Nbr" '"

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
            Dim lsPartNbr As String
            cmbPart_Nbr.Items.Clear()
            cmbPart_Nbr.Items.Add("Select..")
            While dr.Read()
                '  liSAP_STD_PACK_QTY As Integer
                '  liICS_STD_PACK_QTY As Integer

                'liSAP_STD_PACK_QTY = dr.Item("STD_PACK_QTY")
                'lsPART_NBR = dr.Item("MATERIAL_NBR")
                lsPartNbr = (IIf(IsDBNull(dr.Item("PART_NBR")), "", dr.Item("PART_NBR")))

                cmbPart_Nbr.Items.Add(lsPartNbr)

                'Dim liSerial_NBR As Long
                'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
                'lsSERIAL_NBR = liSerial_NBR.ToString
                'llRECORD_ID = llRECORD_ID + 1
            End While

        Catch exole As OracleException
            'Me.lblDescription.Text = "Part Not found in SAP File"
            Throw New Exception(exole.Message)
            'Me.lblDescription.Text = "Part Not found in SAP File"
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

    Sub GetCust_Part_NumberAndLoadCombo()
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
        Dim lsICSCity As String = Session("ICS_CUSTOMER_CITY")
        Try
            'lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '12040756'"
            lsSQL = "SELECT distinct CUST_PART_NBR FROM snapmgr.label_data where MATERIAL_NBR = '" & Me.txtPart_Nbr.Text & "' order by CUST_PART_NBR" '"

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
            Dim lsCUST_PART_NBR As String
            ddlCUST_PART_NBR.Items.Clear()
            ddlCUST_PART_NBR.Items.Add("Select..")
            While dr.Read()
                '  liSAP_STD_PACK_QTY As Integer
                '  liICS_STD_PACK_QTY As Integer

                'liSAP_STD_PACK_QTY = dr.Item("STD_PACK_QTY")
                'lsPART_NBR = dr.Item("MATERIAL_NBR")
                lsCUST_PART_NBR = (IIf(IsDBNull(dr.Item("CUST_PART_NBR")), "", dr.Item("CUST_PART_NBR")))

                ddlCUST_PART_NBR.Items.Add(lsCUST_PART_NBR)

                'Dim liSerial_NBR As Long
                'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
                'lsSERIAL_NBR = liSerial_NBR.ToString
                'llRECORD_ID = llRECORD_ID + 1
            End While

        Catch exole As OracleException
            'Me.lblDescription.Text = "Part Not found in SAP File"
            Throw New Exception(exole.Message)
            'Me.lblDescription.Text = "Part Not found in SAP File"
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
    Sub GetSHIP_TOAndLoadCombo()
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
        Dim lsICSCity As String = Session("ICS_CUSTOMER_CITY")
        Try
            'lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '12040756'"
            lsSQL = "SELECT distinct SHIP_TO ,material_desc FROM snapmgr.Label_Data where MATERIAL_NBR = '" & Me.txtPart_Nbr.Text & "' order  BY SHIP_TO"

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
            Dim lsSHIP_TO As String
            ddlSHIP_TO.Items.Clear()
            ddlSHIP_TO.Items.Add("Select..")
            While dr.Read()
                '  liSAP_STD_PACK_QTY As Integer
                '  liICS_STD_PACK_QTY As Integer

                'liSAP_STD_PACK_QTY = dr.Item("STD_PACK_QTY")
                'lsPART_NBR = dr.Item("MATERIAL_NBR")
                lsSHIP_TO = (IIf(IsDBNull(dr.Item("SHIP_TO")), "", dr.Item("SHIP_TO")))
                lblDescription.Text = (IIf(IsDBNull(dr.Item("material_desc")), "", dr.Item("material_desc")))

                ddlSHIP_TO.Items.Add(lsSHIP_TO)

                'Dim liSerial_NBR As Long
                'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
                'lsSERIAL_NBR = liSerial_NBR.ToString
                'llRECORD_ID = llRECORD_ID + 1
            End While

        Catch exole As OracleException
            'Me.lblDescription.Text = "Part Not found in SAP File"
            Throw New Exception(exole.Message)
            'Me.lblDescription.Text = "Part Not found in SAP File"
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
    Sub GetDestinationsAndLoadCombo()
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
        'Dim lsICSCity As String = Session("ICS_CUSTOMER_CITY")
        Try
            'lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '12040756'"
            lsSQL = "SELECT distinct destination FROM PLTFLOOR.ICS_MATID order by Destination" '"

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
            Dim lsPartNbr As String
            ddlDestination.Items.Clear()
            ddlDestination.Items.Add("Select..")
            While dr.Read()
                '  liSAP_STD_PACK_QTY As Integer
                '  liICS_STD_PACK_QTY As Integer

                'liSAP_STD_PACK_QTY = dr.Item("STD_PACK_QTY")
                'lsPART_NBR = dr.Item("MATERIAL_NBR")
                lsPartNbr = (IIf(IsDBNull(dr.Item("Destination")), "", dr.Item("Destination")))

                ddlDestination.Items.Add(lsPartNbr)

                'Dim liSerial_NBR As Long
                'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
                'lsSERIAL_NBR = liSerial_NBR.ToString
                'llRECORD_ID = llRECORD_ID + 1
            End While

        Catch exole As OracleException
            'Me.lblDescription.Text = "Part Not found in SAP File"
            Throw New Exception(exole.Message)
            'Me.lblDescription.Text = "Part Not found in SAP File"
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
    Sub GetDeptAndLoadCombo()
        ddlDept.Items.Clear()
        ddlDept.Items.Add("Select..")
        ddlDept.Items.Add("2341")
        ddlDept.Items.Add("2342")
        ddlDept.Items.Add("2343")
        Exit Sub
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
        'Dim lsICSCity As String = Session("ICS_CUSTOMER_CITY")
        Try
            'lsSQL = "SELECT * FROM SNAPMGR.LABEL_DATA WHERE material_nbr = '12040756'"
            lsSQL = "SELECT distinct DEPT FROM PLTFLOOR.ICS_MATID order by DEPT " '"

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
            Dim lsDEPT As String
            ddlDept.Items.Clear()
            ddlDept.Items.Add("Select..")
            While dr.Read()
                '  liSAP_STD_PACK_QTY As Integer
                '  liICS_STD_PACK_QTY As Integer

                'liSAP_STD_PACK_QTY = dr.Item("STD_PACK_QTY")
                'lsPART_NBR = dr.Item("MATERIAL_NBR")
                lsDEPT = (IIf(IsDBNull(dr.Item("DEPT")), "", dr.Item("DEPT")))

                ddlDept.Items.Add(lsDEPT)

                'Dim liSerial_NBR As Long
                'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
                'lsSERIAL_NBR = liSerial_NBR.ToString
                'llRECORD_ID = llRECORD_ID + 1
            End While

        Catch exole As OracleException
            'Me.lblDescription.Text = "Part Not found in SAP File"
            Throw New Exception(exole.Message)
            'Me.lblDescription.Text = "Part Not found in SAP File"
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




    Private Sub lstbxInsertResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cmbPart_Nbr_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPart_Nbr.SelectedIndexChanged
        Me.txtPart_Nbr.Text = cmbPart_Nbr.SelectedValue
        GetCust_Part_NumberAndLoadCombo()
        GetSHIP_TOAndLoadCombo()
    End Sub

    Private Sub cmbSTD_FLAG_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSTD_FLAG.SelectedIndexChanged
        txtSTD_FLAG.Text = cmbSTD_FLAG.SelectedValue
        If txtSTD_FLAG.Text = "S" Then
            ddlDestination.Visible = False
            txtDestination.Visible = False
            ddlSHIP_TO.Visible = False
        Else
            ddlDestination.Visible = True
            txtDestination.Visible = True
            ddlSHIP_TO.Visible = True
        End If
    End Sub

    Private Sub ddlDestination_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlDestination.SelectedIndexChanged
        txtDestination.Text = ddlDestination.SelectedValue
    End Sub


    Private Sub ddlDept_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlDept.SelectedIndexChanged
        txtDept.Text = ddlDept.SelectedValue
    End Sub

    Private Sub ddlPackageCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPackageCode.SelectedIndexChanged
        Me.txtPackageCode.Text = ddlPackageCode.SelectedValue
    End Sub

    Private Sub ddlCUST_PART_NBR_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCUST_PART_NBR.SelectedIndexChanged
        Me.txtCUST_PART_NBR.Text = ddlCUST_PART_NBR.SelectedValue

    End Sub

    Private Sub ddlSHIP_TO_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSHIP_TO.SelectedIndexChanged
        Me.txtSHIP_TO.Text = ddlSHIP_TO.SelectedValue
    End Sub

    Private Sub btnModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModify.Click
        CleanAllFields()
        LockAllFields()
        Me.btnCreateMATID.Visible = False
        txtMATID.ReadOnly = False
        txtMATID.Enabled = True
        lblMATID.Visible = True
        txtMATID.Visible = True
        btnModify_MATID.Enabled = True
        txtMATID.BackColor = Color.LightYellow
        ddlStatus.Visible = True
        lblMATIDstatus.Visible = True
        btnCreateMATID.Visible = False
        btnVerifyMATID.Visible = False
        btnSaveMATID.Visible = True
        btnModify_MATID.Visible = True
        btnModify.Visible = False
    End Sub

    Private Sub btnSaveMATID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveMATID.Click
        UpdateICS_MATID()
        btnSaveMATID.Visible = False
        btnModify.Visible = False
        btnModify_MATID.Visible = False
        LockAllFields()
    End Sub

    Private Sub btnModify_MATID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModify_MATID.Click
        Dim lsMATID As String = txtMATID.Text
        CleanAllFields()

        txtMATID.Text = lsMATID
        'GetDestinationsAndLoadCombo()
        'GetDeptAndLoadCombo()
        GetMATID_AndLoadFields()
    End Sub

    Private Sub ddlStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlStatus.SelectedIndexChanged
        txtSTATUS.Text = ddlStatus.SelectedValue
    End Sub


    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        CleanAllFields()
        UnLockAllFields()
        lsCUSTOMER_NAME = Session("CUSTOMER_NAME")
        liMATID = Session("MATID")
        GetPartsAndLoadCombo()
        txtPAY_SUFFIX.Text = "00"
        GetDestinationsAndLoadCombo()
        'GetSHIP_TOAndLoadCombo()
        GetDeptAndLoadCombo()

        txtMATID.Text = ""
        txtMATID.Visible = False
        lblMATID.Visible = False
        ddlCUST_PART_NBR.Items.Clear()
        ddlSHIP_TO.Items.Clear()
        ddlStatus.Visible = False
        lblMATIDstatus.Visible = False
        btnCreateMATID.Visible = False
        btnVerifyMATID.Visible = True
        ddlPackageCode.SelectedValue = "Select.."
        btnModify.Visible = True
        txtMATID.Enabled = False
        txtMATID.BackColor = Color.LightGray
        btnModify_MATID.Visible = False
        cmbSTD_FLAG.SelectedValue = ""
    End Sub
    Sub DeleteICS_MATID()
        Exit Sub
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

        If IsNumeric(txtMATID.Text) Then
            liMATID = CInt(txtMATID.Text)
        Else
            Exit Sub
        End If


        Try
            lsSQL = "delete from pltfloor.ICS_MATID where  MATID = " & liMATID

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
            'Me.txtMATID.Text = liMATID.ToString
            Me.lblStatus.Text = "Successfully deleted: " & liMATID.ToString
        Catch exole As OracleException
            'Throw New Exception(exole.Message)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Dispose()
            'cmd.Dispose()
            ds.Dispose()
            'da.Dispose()

        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DeleteICS_MATID()
    End Sub
End Class
