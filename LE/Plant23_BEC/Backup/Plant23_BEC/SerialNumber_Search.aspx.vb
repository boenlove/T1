Imports System.Data
Imports System.Text
Imports System.Configuration
Imports System.IO
Imports System.Diagnostics
Imports System.Diagnostics.Process
Public Class SerialNumber_Search
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblSearchBy As System.Web.UI.WebControls.Label
    Protected WithEvents rfvSearchBy As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblPlant As System.Web.UI.WebControls.Label
    Protected WithEvents lbDepartment As System.Web.UI.WebControls.Label
    Protected WithEvents lblShift As System.Web.UI.WebControls.Label
    Protected WithEvents txtPartNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmbSearchBy As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbPlant As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbShift As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbMachineName As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbDepartment As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtEnd As System.Web.UI.WebControls.TextBox
    Protected WithEvents imgCalEndDate As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblEnd As System.Web.UI.WebControls.Label
    Protected WithEvents lblStart As System.Web.UI.WebControls.Label
    Protected WithEvents txtStart As System.Web.UI.WebControls.TextBox
    Protected WithEvents imgCalStartDate As System.Web.UI.WebControls.ImageButton
    Protected WithEvents rfvStart As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents btnLookUp As System.Web.UI.WebControls.Button
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoRecordsFound As System.Web.UI.WebControls.Label
    Protected WithEvents txtMesPartId As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMesToolId As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSortOrder As System.Web.UI.WebControls.Label
    Protected WithEvents dgStandardPackHistory As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCount As System.Web.UI.WebControls.Label
    Protected WithEvents tblResults As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents btnExport As System.Web.UI.WebControls.Button
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
    Protected WithEvents StartCalendar As System.Web.UI.WebControls.Calendar
    Protected WithEvents EndCalendar As System.Web.UI.WebControls.Calendar
    Protected WithEvents lblMachines As System.Web.UI.WebControls.Label
    Protected WithEvents lbPartNumber As System.Web.UI.WebControls.Label
    Protected WithEvents hpkReturntoMain As System.Web.UI.WebControls.HyperLink
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
        If x = "LOCAL." Or x = "TEST." Then
            lblDisplay.Visible = True
            lblDisplay.Text = "TEST ENVIRONMENT"
        End If
        If Not Page.IsPostBack Then
            tblResults.Visible = False
            viewstate("sortField") = "SERIAL_NBR"
            viewstate("sortDirection") = "ASC"
            lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
            Call Populate_cmbMachineName()
            Call Populate_cmbDept()
            Call Populate_cmbShift()
        End If
        Select Case cmbSearchBy.SelectedValue
            Case "Start Date and End Date (BAG)"
                lblStart.Text = "(BAG) Start Date:"
                lblEnd.Text = "(BAG) End Date:"
                imgCalStartDate.Visible = True
                imgCalEndDate.Visible = True
                txtStart.Enabled = False
                txtEnd.Enabled = False
            Case "Start Date and End Date (BEC)"
                lblStart.Text = "(BEC) Start Date:"
                lblEnd.Text = "(BEC) End Date:"
                imgCalStartDate.Visible = True
                imgCalEndDate.Visible = True
                txtStart.Enabled = False
                txtEnd.Enabled = False
            Case "Serial Number Range (BAG)"
                lblStart.Text = "(BAG) Start Serial Number:"
                lblEnd.Text = "(BAG) End Serial Number:"
                imgCalStartDate.Visible = False
                imgCalEndDate.Visible = False
                txtStart.Enabled = True
                txtEnd.Enabled = True
            Case "Serial Number Range (BEC)"
                lblStart.Text = "(BEC) Start Serial Number:"
                lblEnd.Text = "(BEC) End Serial Number:"
                imgCalStartDate.Visible = False
                imgCalEndDate.Visible = False
                txtStart.Enabled = True
                txtEnd.Enabled = True
            Case "Sub-Standard ID (BAG)"
                lblStart.Text = "Start BAG:"
                lblEnd.Text = "End BAG:"
                imgCalStartDate.Visible = False
                imgCalEndDate.Visible = False
                txtStart.Enabled = True
                txtEnd.Enabled = True
            Case "Sub-Standard ID (BEC)"
                lblStart.Text = "Start BEC:"
                lblEnd.Text = "End BEC:"
                imgCalStartDate.Visible = False
                imgCalEndDate.Visible = False
                txtStart.Enabled = True
                txtEnd.Enabled = True
            Case "Alphanumeric Serial Number Pattern"
                lblStart.Text = "Start Serial Number:"
                lblEnd.Text = "End Serial Number:"
                lblEnd.Enabled = False
                imgCalStartDate.Visible = False
                imgCalEndDate.Visible = False
                txtStart.Enabled = True
                txtEnd.Enabled = False
        End Select
    End Sub
    Sub GetPopupDate(ByVal TargetTextBox As TextBox, ByVal TargetPage As Page)
        Dim strScript As String = "<script language=javascript>window.open('DatePicker.aspx?textbox=" & TargetTextBox.ID & "','cal','width=250,height=225,left=270,top=180')</script>"
        TargetPage.RegisterClientScriptBlock("GetPopupDate", strScript)
    End Sub
    Private Sub imgCalStartDate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCalStartDate.Click
        'Call GetPopupDate(txtStart, Page)
        StartCalendar.Visible = True
    End Sub

    Private Sub imgCalEndDate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCalEndDate.Click
        'Call GetPopupDate(txtEnd, Page)
        EndCalendar.Visible = True
    End Sub
    Sub Populate_cmbMachineName()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbMachineName As String = "SELECT MACHINE_NAME, MACHINE_ID FROM MESDBA.MACHINE WHERE REPORT_GROUP_ID = '" & cmbDepartment.SelectedValue & "' GROUP BY MACHINE_NAME, MACHINE_ID ORDER BY MACHINE_NAME ASC"
        Dim CmdMachineName As New OleDb.OleDbCommand(sqlCmbMachineName, MyConn)
        'Dept 2341 - 1125 - Underhood
        'Dept 2342 - 1126 - Mid
        'Dept 2343 - 1127 - Left
        'Response.Write(sqlCmbMachineName)
        Try
            MyConn.Open()
            objDR = CmdMachineName.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
            cmbMachineName.DataSource = objDR
            cmbMachineName.DataValueField = "MACHINE_ID"
            cmbMachineName.DataTextField = "MACHINE_NAME"
            cmbMachineName.DataBind()
            cmbMachineName.Items.Insert(0, New ListItem("--- All ---", "0"))
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
            End If
        Catch ex As Exception
            lblMsg.Visible = True
            lblMsg.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub
    Sub Populate_cmbShift()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbShift As String = "SELECT SHIFT_NAME, SHIFT_ID FROM MESDBA.SHIFT WHERE REPORT_GROUP_ID = '" & cmbDepartment.SelectedValue & "' ORDER BY SHIFT_NAME ASC"
        Dim CmdShift As New OleDb.OleDbCommand(sqlCmbShift, MyConn)
        Try
            MyConn.Open()
            objDR = CmdShift.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
            cmbShift.DataSource = objDR
            cmbShift.DataValueField = "SHIFT_ID"
            cmbShift.DataTextField = "SHIFT_NAME"
            cmbShift.DataBind()
            cmbShift.Items.Insert(0, New ListItem("--- All ---", "0"))
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
            End If
        Catch ex As Exception
            lblMsg.Visible = True
            lblMsg.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub
    Sub Populate_cmbDept()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlCmbDept As String
        sqlCmbDept = "SELECT ltrim(DEPT,0)||' - ' || DESCR as DEPT_DESCR, ID FROM MESDBA.VW_DEPARTMENT WHERE DEPT LIKE '0" & Right(cmbPlant.SelectedValue, 2) & "%' and CLOSED_DATE IS NULL ORDER BY DEPT"
        Dim objDR As OleDb.OleDbDataReader
        Dim CmdDept As New OleDb.OleDbCommand(sqlCmbDept, MyConn)
        'Execute sql statement for cmbDept
        Try
            MyConn.Open()
            objDR = CmdDept.ExecuteReader
            cmbDepartment.DataSource = objDR
            cmbDepartment.DataTextField = "DEPT_DESCR"
            cmbDepartment.DataValueField = "ID"
            cmbDepartment.DataBind()
            cmbDepartment.Items.Insert(0, New ListItem("--- Select ---", "0"))
            CmdDept.Dispose()
            objDR.Close()
            MyConn.Close()
            MyConn.Dispose()
        Catch ex As Exception
            lblMsg.Visible = True
            lblMsg.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub
    Private Sub cmbDepartment_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDepartment.SelectedIndexChanged
        Call Populate_cmbMachineName()
        Call Populate_cmbShift()
    End Sub
    Private Sub btnLookUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLookUp.Click
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Try
            Dim sqlGetTool As String
            Dim sqlGetLot As String
            If txtPartNumber.Text <> "" Then
                txtMesPartId.Text = GetMESPartId(txtPartNumber.Text, "")
            End If
            Select Case cmbSearchBy.SelectedValue
                Case "Start Date and End Date (BEC)", "Start Date and End Date (BAG)"
                    If IsDate(txtStart.Text) = True And txtEnd.Text = "" Then
                        txtEnd.Text = DateAdd(DateInterval.Day, 1, CDate(txtStart.Text))
                        If DateDiff("d", CDate(txtStart.Text), CDate(txtEnd.Text)) >= 0 Then
                            If cmbPlant.SelectedValue = "0" Or cmbDepartment.SelectedValue = "0" Or cmbDepartment.SelectedValue = "" Then
                                lblMsg.Text = "Select Plant and/or Dept."
                                tblResults.Visible = False
                            Else
                                Call Populate_dgStandardPackHistory()
                            End If
                        Else
                            lblMsg.Text = "Date is in wrong order."
                            tblResults.Visible = False
                        End If
                    ElseIf IsDate(txtStart.Text) = True And IsDate(txtEnd.Text) = True Then
                        If DateDiff("d", CDate(txtStart.Text), CDate(txtEnd.Text)) >= 0 Then
                            If cmbPlant.SelectedValue = "0" Or cmbDepartment.SelectedValue = "0" Or cmbDepartment.SelectedValue = "" Then
                                lblMsg.Text = "Select Plant and/or Dept."
                                tblResults.Visible = False
                            Else
                                Call Populate_dgStandardPackHistory()
                            End If
                        Else
                            lblMsg.Text = "Date is in wrong order."
                            tblResults.Visible = False
                        End If
                    ElseIf IsDate(txtStart.Text) = False Or IsDate(txtEnd.Text) = False Then
                        lblMsg.Text = "Date is in wrong format."
                        tblResults.Visible = False
                    End If

                Case "Serial Number Range (BEC)", "Serial Number Range (BAG)", "Sub-Standard ID (BEC)", "Sub-Standard ID (BAG)"
                    If Val(txtStart.Text) > 0 Then
                        If txtEnd.Text = "" Then
                            txtEnd.Text = txtStart.Text + 1
                            Call Populate_dgStandardPackHistory()
                        ElseIf txtEnd.Text <> "" And Val(txtEnd.Text) < Val(txtStart.Text) Then
                            lblMsg.Text = "Data is in wrong order."
                            tblResults.Visible = False
                        ElseIf txtEnd.Text <> "" And Val(txtEnd.Text) > 0 Then
                            Call Populate_dgStandardPackHistory()
                        ElseIf txtEnd.Text <> "" And Val(txtEnd.Text) = 0 Then
                            lblMsg.Text = "Data is in wrong format."
                            tblResults.Visible = False
                        End If

                    Else
                        lblMsg.Text = "Data is in wrong format/wrong order."
                        tblResults.Visible = False
                    End If
                Case "Alphanumeric Serial Number Pattern"
                    Call Populate_dgStandardPackHistory()
            End Select
        Catch ex As Exception
            lblMsg.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub

    Private Sub cmbPlant_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPlant.SelectedIndexChanged
        Call Populate_cmbDept()
    End Sub
    Sub Populate_dgStandardPackHistory()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlGetStandardPackHistory As String
        Dim whrDate As New StringBuilder
        Dim selDate As New StringBuilder
        Dim frmDate As New StringBuilder
        Dim grpDate As New StringBuilder
        Dim whrSerial As New StringBuilder
        Dim selSerial As New StringBuilder
        Dim frmSerial As New StringBuilder
        Dim grpSerial As New StringBuilder
        Dim whrBag As New StringBuilder
        Dim selBag As New StringBuilder
        Dim frmBag As New StringBuilder
        Dim grpBag As New StringBuilder
        Dim whrBec As New StringBuilder
        Dim selBec As New StringBuilder
        Dim frmBec As New StringBuilder
        Dim grpBec As New StringBuilder
        Try
            Select Case cmbSearchBy.SelectedValue
                Case "Start Date and End Date (BEC)"
                    selDate.Append("SELECT ('Serial_History.aspx?job=BEC&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                    frmDate.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp")
                    whrDate.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND sp.INSERT_TMSTM >= to_date('" & txtStart.Text & " 12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM') AND sp.INSERT_TMSTM <= to_date('" & txtEnd.Text & " 12:00:07 AM', 'MM/DD/YYYY HH:MI:SS AM') ")
                    If cmbDepartment.SelectedValue <> "0" Then
                        whrDate.Append("AND sp.REPORT_GROUP_ID = '" & cmbDepartment.SelectedValue & "'")
                    End If
                    If cmbMachineName.SelectedValue <> "0" Then
                        whrDate.Append("AND sp.MACHINE_ID = '" & cmbMachineName.SelectedItem.Value & "'")
                    End If
                    If cmbShift.SelectedValue <> "0" Then
                        whrDate.Append("AND sp.SHIFT_ID = '" & cmbShift.SelectedItem.Value & "'")
                    End If
                    If txtPartNumber.Text <> "" Then
                        whrDate.Append("AND sp.MES_PART_ID = '" & txtMesPartId.Text & "'")
                    End If
                    sqlGetStandardPackHistory = selDate.ToString & frmDate.ToString & whrDate.ToString & ordStandardPack
                Case "Start Date and End Date (BAG)"
                    selDate.Append("SELECT ('Serial_History.aspx?job=BAG&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                    frmDate.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp")
                    whrDate.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND sp.INSERT_TMSTM >= to_date('" & txtStart.Text & " 12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM') AND sp.INSERT_TMSTM <= to_date('" & txtEnd.Text & " 12:00:07 AM', 'MM/DD/YYYY HH:MI:SS AM') ")
                    If cmbDepartment.SelectedValue <> "" Then
                        whrDate.Append("AND sp.REPORT_GROUP_ID = '" & cmbDepartment.SelectedValue & "'")
                    End If
                    If cmbMachineName.SelectedValue <> "0" Then
                        whrDate.Append("AND sp.MACHINE_ID = '" & cmbMachineName.SelectedItem.Value & "'")
                    End If
                    If cmbShift.SelectedValue <> "0" Then
                        whrDate.Append("AND sp.SHIFT_ID = '" & cmbShift.SelectedItem.Value & "'")
                    End If
                    If txtPartNumber.Text <> "" Then
                        whrDate.Append("AND sp.MES_PART_ID = '" & txtMesPartId.Text & "'")
                    End If
                    sqlGetStandardPackHistory = selDate.ToString & frmDate.ToString & whrDate.ToString & ordStandardPack
                Case "Serial Number Range (BEC)"
                    selSerial.Append("SELECT ('Serial_History.aspx?job=BEC&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                    frmSerial.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp")
                    whrSerial.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND sp.SERIAL_NBR >= '" & txtStart.Text & "' AND sp.SERIAL_NBR <= '" & txtEnd.Text & "'")
                    sqlGetStandardPackHistory = selSerial.ToString & frmSerial.ToString & whrSerial.ToString & ordStandardPack
                Case "Serial Number Range (BAG)"
                    selSerial.Append("SELECT ('Serial_History.aspx?job=BAG&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                    frmSerial.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp")
                    whrSerial.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND sp.SERIAL_NBR >= '" & txtStart.Text & "' AND sp.SERIAL_NBR <= '" & txtEnd.Text & "'")
                    sqlGetStandardPackHistory = selSerial.ToString & frmSerial.ToString & whrSerial.ToString & ordStandardPack
                Case "Sub-Standard ID (BAG)"
                    selBag.Append("SELECT ('Serial_History.aspx?job=BAG&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                    frmBag.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp, MESDBA.SUB_STANDARD_PACK ssp")
                    whrBag.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND ssp.STANDARD_PACK_ID = sp.STANDARD_PACK_ID AND ssp.SUB_STANDARD_PACK_ID >= '" & txtStart.Text & "' AND ssp.SUB_STANDARD_PACK_ID <= '" & txtEnd.Text & "'")
                    grpBag.Append(" GROUP BY sp.SERIAL_NBR, sp.STANDARD_PACK_ID, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                    sqlGetStandardPackHistory = selBag.ToString & frmBag.ToString & whrBag.ToString & grpBag.ToString & ordStandardPack
                Case "Sub-Standard ID (BEC)"
                    selBec.Append("SELECT ('Serial_History.aspx?job=BEC&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                    frmBec.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp, MESDBA.SERIALIZED_PRODUCT ssp")
                    whrBec.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND ssp.STANDARD_PACK_ID = sp.STANDARD_PACK_ID AND ssp.PRODUCT_ID >= '" & txtStart.Text & "' AND ssp.PRODUCT_ID <= '" & txtEnd.Text & "'")
                    grpBec.Append(" GROUP BY sp.SERIAL_NBR, sp.STANDARD_PACK_ID, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                    sqlGetStandardPackHistory = selBec.ToString & frmBec.ToString & whrBec.ToString & grpBec.ToString & ordStandardPack
                Case "Alphanumeric Serial Number Pattern"
                    selSerial.Append("SELECT ('Serial_History.aspx?job=BEC&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                    frmSerial.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp")
                    whrSerial.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND UPPER(sp.SERIAL_NBR) like '%" & UCase(txtStart.Text) & "%'")
                    If cmbDepartment.SelectedValue <> "0" Then
                        whrDate.Append("AND sp.REPORT_GROUP_ID = '" & cmbDepartment.SelectedValue & "'")
                    End If
                    If cmbMachineName.SelectedValue <> "0" Then
                        whrDate.Append("AND sp.MACHINE_ID = '" & cmbMachineName.SelectedItem.Value & "'")
                    End If
                    If cmbShift.SelectedValue <> "0" Then
                        whrDate.Append("AND sp.SHIFT_ID = '" & cmbShift.SelectedItem.Value & "'")
                    End If
                    If txtPartNumber.Text <> "" Then
                        whrDate.Append("AND sp.MES_PART_ID = '" & txtMesPartId.Text & "'")
                    End If
                    sqlGetStandardPackHistory = selSerial.ToString & frmSerial.ToString & whrSerial.ToString & ordStandardPack
            End Select
            ' Create a Command object with the SQL statement.
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlGetStandardPackHistory, MyConn)
            Dim objDataReader As OleDb.OleDbDataReader
            objDataReader = objCommand.ExecuteReader
            RecordCount = 0
            While objDataReader.Read()
                RecordCount = RecordCount + 1
            End While
            If RecordCount = 0 Then
                lblCount.Visible = True
                lblCount.Text = "Record Count: 0"
            Else
                lblCount.Visible = True
                lblCount.Text = "Record Count: " & RecordCount
            End If
            objDataReader.Close()
            objDataReader = objCommand.ExecuteReader
            If objDataReader.HasRows = False Then
                lblNoRecordsFound.Text = "No Records were found."
                lblNoRecordsFound.Visible = True
                dgStandardPackHistory.Visible = False
                tblResults.Visible = False
                lblSortOrder.Visible = False
            ElseIf objDataReader.HasRows = True Then
                dgStandardPackHistory.DataSource = objDataReader
                dgStandardPackHistory.DataBind()
                dgStandardPackHistory.Visible = True
                tblResults.Visible = True
                lblSortOrder.Visible = True
                lblNoRecordsFound.Visible = False
            End If

            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()

            End If
            lblMsg.Text = ""
        Catch ex As Exception
            lblMsg.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            lblMsg.Visible = True
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub
    Sub dgStandardPackHistory_SortCommand(ByVal Source As Object, ByVal E As DataGridSortCommandEventArgs)
        If E.SortExpression.ToString() = viewstate("sortField").ToString() Then
            Select Case viewstate("sortDirection").ToString()
                Case "ASC"
                    viewstate("sortDirection") = "DESC"
                Case "DESC"
                    viewstate("sortDirection") = "ASC"
            End Select
        Else
            viewstate("sortField") = E.SortExpression
            viewstate("sortDirection") = "ASC"
        End If
        ordStandardPack = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
        lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
        Populate_dgStandardPackHistory()
    End Sub
    Private Sub cmbSearchBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchBy.SelectedIndexChanged
        Select Case cmbSearchBy.SelectedValue
            Case "Start Date and End Date (BAG)"
                lblStart.Text = "(BAG) Start Date:"
                lblEnd.Text = "(BAG) End Date:"
                txtStart.Text = ""
                txtEnd.Text = ""
                lblMsg.Text = ""
                imgCalStartDate.Visible = True
                imgCalEndDate.Visible = True
            Case "Start Date and End Date (BEC)"
                lblStart.Text = "(BEC) Start Date:"
                lblEnd.Text = "(BEC) End Date:"
                txtStart.Text = ""
                txtEnd.Text = ""
                lblMsg.Text = ""
                imgCalStartDate.Visible = True
                imgCalEndDate.Visible = True
            Case "Serial Number Range (BAG)"
                lblStart.Text = "(BAG) Start Serial Number:"
                lblEnd.Text = "(BAG) End Serial Number:"
                txtStart.Text = ""
                txtEnd.Text = ""
                lblMsg.Text = ""
                imgCalStartDate.Visible = False
                imgCalEndDate.Visible = False
            Case "Serial Number Range (BEC)"
                lblStart.Text = "(BEC) Start Serial Number:"
                lblEnd.Text = "(BEC) End Serial Number:"
                txtStart.Text = ""
                txtEnd.Text = ""
                lblMsg.Text = ""
                imgCalStartDate.Visible = False
                imgCalEndDate.Visible = False
            Case "Sub-Standard ID (BAG)"
                lblStart.Text = "Start BAG:"
                lblEnd.Text = "End BAG:"
                txtStart.Text = ""
                txtEnd.Text = ""
                lblMsg.Text = ""
                imgCalStartDate.Visible = False
                imgCalEndDate.Visible = False
            Case "Sub-Standard ID (BEC)"
                lblStart.Text = "Start BEC:"
                lblEnd.Text = "End BEC:"
                txtStart.Text = ""
                txtEnd.Text = ""
                lblMsg.Text = ""
                imgCalStartDate.Visible = False
                imgCalEndDate.Visible = False
        End Select
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim sbString As New System.Text.StringBuilder
        Dim URL As String

        sbString.Append("TYPE=" & cmbSearchBy.SelectedValue & "&")
        sbString.Append("START=" & txtStart.Text & "&")
        sbString.Append("END=" & txtEnd.Text & "&")
        sbString.Append("MACHINE=" & cmbMachineName.SelectedValue & "&")
        sbString.Append("MACHINE_ID=" & cmbMachineName.SelectedItem.Value & "&")
        sbString.Append("DEPT=" & cmbDepartment.SelectedValue & "&")
        sbString.Append("SHIFT=" & cmbShift.SelectedValue & "&")
        sbString.Append("SHIFT_ID=" & cmbShift.SelectedItem.Value & "&")
        sbString.Append("PART_ID=" & txtMesPartId.Text & "&")

        'Response.Write("<script>window.open('ExportToExcel/Export.aspx?" & sbString.ToString & "','Excel','height=200,width=650');</script>")
        'Response.Write("<script>window.open('ExportToExcel/Export.aspx?" & sbString.ToString & "','','');</script>")

        URL = "ExportSerialHistory.aspx?" & sbString.ToString
        Response.Write("<script>window.open('" + URL + "','Report','height=300,width=500,resizable=yes,scrollbars=yes,status=no,menubar=yes');</script>")

    End Sub

    Private Sub EndCalendar_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles EndCalendar.SelectionChanged
        EndCalendar.Visible = False
        txtEnd.Text = EndCalendar.SelectedDate
    End Sub

    Private Sub StartCalendar_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles StartCalendar.SelectionChanged
        StartCalendar.Visible = False
        txtStart.Text = StartCalendar.SelectedDate
    End Sub
End Class
