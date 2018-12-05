Imports System.Data.OracleClient
'Imports Oracle.ManagedDataAccess.Client

Partial Class SAP_MATID_Inquiry
    Inherits System.Web.UI.Page

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

        If IsPostBack = False Then
            ddlAndOr.SelectedValue = "or"
            GetPartsAndLoadCombo()
            GetShipToAndLoadCombo()
        End If

    End Sub

    Friend Function GetConnectionStringNonOle() As String
        'instead of user legacy OleDB, use faster Oracle methods
        Dim conString As String = GetConnectionString()
        Dim liClean As Integer = InStr(conString, ";")
        conString = Mid(conString, liClean + 1)
        Return conString
    End Function

    Private Sub Populate_dgMATID_Grid()
        Dim conString As String = GetConnectionStringNonOle()
        Dim con As New OracleConnection(conString)
        Dim lsSQL As String
        Dim RecordCount As Integer
        If lblAndOr.Text.Length = 0 Then
            lblAndOr.Text = "or"
        End If

        Try
            Select Case lblCount.Text
                Case "PART"
                    lsSQL = "SELECT * FROM pltfloor.ICS_MATID WHERE PART_NBR = '" & lblPart_Nbr.Text & "' and status = 'A' order by MATID"
                Case "SHIP"
                    lsSQL = "SELECT * FROM pltfloor.ICS_MATID WHERE SHIP_TO  = '" & lblShipTo.Text & "' and status = 'A' order by MATID"
                Case "MATID"
                    If IsNumeric(txtMATID.Text) Then
                        lsSQL = "SELECT * FROM pltfloor.ICS_MATID WHERE matid  = " & CInt(txtMATID.Text)
                    End If
                Case "TYPE"
                    If lblAndOr.Text = "or" Then
                        lsSQL = "SELECT * FROM pltfloor.ICS_MATID WHERE STD_FLAG = '" & lblSTD_FLAG.Text & "' and status = 'A' order by MATID"
                    Else
                        lsSQL = "SELECT * FROM pltfloor.ICS_MATID WHERE STD_FLAG = '" & lblSTD_FLAG.Text & "' and status = 'A' and Package_Code = '" & lblPackageCode.Text & "' order by MATID"
                    End If
                Case "PKGE"
                    If lblAndOr.Text = "or" Then
                        lsSQL = "SELECT * FROM pltfloor.ICS_MATID WHERE Package_Code = '" & lblPackageCode.Text & "' and status = 'A' order by MATID"
                    Else
                        lsSQL = "SELECT * FROM pltfloor.ICS_MATID WHERE Package_Code = '" & lblPackageCode.Text & "' and status = 'A' and STD_FLAG = '" & lblSTD_FLAG.Text & "'  order by MATID"
                    End If
            End Select

            Dim cmd As New OracleCommand
            Dim r As System.Data.OracleClient.OracleString
            If con.State = ConnectionState.Closed Then con.Open()
            With cmd
                .Connection = con
                .CommandType = CommandType.Text
                .CommandText = lsSQL
                .ExecuteNonQuery()
            End With
            Dim dr As OracleDataReader = cmd.ExecuteReader()

            'con.Open()

            'Dim objCommand As New OleDb.OleDbCommand(sqlGetForecastSummary, con)
            'Dim objDataReader As OleDb.OleDbDataReader
            'objDataReader = objCommand.ExecuteReader

            dgMATID_Grid.DataSource = dr
            dgMATID_Grid.DataBind()
            dgMATID_Grid.Visible = True

            While dr.Read()
                RecordCount = RecordCount + 1
                ' lblCount.Text = "MATID's found: " & RecordCount.ToString
            End While
            'If RecordCount = 0 Then
            '    lblCount.Visible = True
            '    lblCount.Text = "Record Count: 0"
            'Else
            '    lblCount.Visible = True
            '    lblCount.Text = "Record Count: " & RecordCount
            'End If
            dr.Close()

        Catch ex As Exception
            lblMessage.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            'If IsLoggingOn() Then
            '    LogError(ex.Source, ex.Message)
            'End If
            'LogEvent(ex.Message)
        End Try



    End Sub

    Private Sub cmbPart_Nbr_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPart_Nbr.SelectedIndexChanged
        lblPart_Nbr.Text = cmbPart_Nbr.SelectedValue
        lblCount.Text = "PART"
        dgMATID_Grid.DataBind()
        Populate_dgMATID_Grid()
        txtMATID.Text = ""
        cmbShipTo.SelectedValue = ".."
        ddlPackageCode.SelectedValue = ".."
        cmbSTD_FLAG.SelectedValue = ".."
    End Sub

    Sub GetPartsAndLoadCombo()
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
            lsSQL = "SELECT distinct part_Nbr FROM pltfloor.ICS_MATID order by Part_Nbr" '"

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
            cmbPart_Nbr.Items.Add("..")
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
            'lblDescription.Text = "Part Not found in SAP File"
            'Throw New Exception(exole.Message)
            'lblDescription.Text = "Part Not found in SAP File"
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
    Sub GetShipToAndLoadCombo()
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
            lsSQL = "SELECT distinct SHIP_TO FROM pltfloor.ICS_MATID order by Ship_To" '"

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
            cmbShipTo.Items.Clear()
            cmbShipTo.Items.Add("..")
            While dr.Read()
                '  liSAP_STD_PACK_QTY As Integer
                '  liICS_STD_PACK_QTY As Integer

                'liSAP_STD_PACK_QTY = dr.Item("STD_PACK_QTY")
                'lsPART_NBR = dr.Item("MATERIAL_NBR")
                lsSHIP_TO = (IIf(IsDBNull(dr.Item("SHIP_TO")), "", dr.Item("SHIP_TO")))

                cmbShipTo.Items.Add(lsSHIP_TO)

                'Dim liSerial_NBR As Long
                'liSerial_NBR = CLng(lsSERIAL_NBR + 1)
                'lsSERIAL_NBR = liSerial_NBR.ToString
                'llRECORD_ID = llRECORD_ID + 1
            End While

        Catch exole As OracleException
            'lblDescription.Text = "Part Not found in SAP File"
            'Throw New Exception(exole.Message)
            'lblDescription.Text = "Part Not found in SAP File"
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
    Private Sub cmbShipTo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbShipTo.SelectedIndexChanged
        lblCount.Text = "SHIP"
        lblShipTo.Text = cmbShipTo.SelectedValue
        dgMATID_Grid.DataBind()
        Populate_dgMATID_Grid()
        txtMATID.Text = ""
        cmbPart_Nbr.SelectedValue = ".."
        ddlPackageCode.SelectedValue = ".."
        cmbSTD_FLAG.SelectedValue = ".."
    End Sub
    Private Sub dgMATID_Grid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgMATID_Grid.SelectedIndexChanged

    End Sub
    Private Sub btnModify_MATID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModify_MATID.Click
        lblCount.Text = "MATID"
        dgMATID_Grid.DataBind()
        Populate_dgMATID_Grid()
        cmbPart_Nbr.SelectedValue = ".."
        cmbShipTo.SelectedValue = ".."
        ddlPackageCode.SelectedValue = ".."
        cmbSTD_FLAG.SelectedValue = ".."
    End Sub

    Private Sub cmbSTD_FLAG_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSTD_FLAG.SelectedIndexChanged
        lblCount.Text = "TYPE"
        lblSTD_FLAG.Text = cmbSTD_FLAG.SelectedValue
        dgMATID_Grid.DataBind()
        Populate_dgMATID_Grid()
        txtMATID.Text = ""
        cmbPart_Nbr.SelectedValue = ".."
        txtMATID.Text = ""
        cmbShipTo.SelectedValue = ".."
        If Me.ddlAndOr.SelectedValue = "or" Then
            Me.ddlPackageCode.SelectedValue = ".."
        End If
    End Sub

    Private Sub ddlPackageCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPackageCode.SelectedIndexChanged
        lblCount.Text = "PKGE"
        lblPackageCode.Text = ddlPackageCode.SelectedValue
        dgMATID_Grid.DataBind()
        Populate_dgMATID_Grid()
        txtMATID.Text = ""
        cmbPart_Nbr.SelectedValue = ".."
        txtMATID.Text = ""
        cmbShipTo.SelectedValue = ".."
        If Me.ddlAndOr.SelectedValue = "or" Then
            cmbSTD_FLAG.SelectedValue = ".."
        End If
    End Sub

    Private Sub ddlAndOr_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlAndOr.SelectedIndexChanged
        lblAndOr.Text = ddlAndOr.SelectedValue
    End Sub
End Class
