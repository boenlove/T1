Imports System.Data
Imports System.Text
Imports System.Configuration
Imports System.IO
Imports System.Diagnostics
Imports System.Diagnostics.Process
Public Class Export
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dgStandardPackHistory As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label

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

            '*************************************************************
            'Get the Export Type querystring to determine whether to bind
            'Event History, Press History or Reel History to the datagrid
            '*************************************************************
            ExportToExcel(Request.QueryString("TYPE"))

        End If
    End Sub

    Private Sub ExportToExcel(ByVal ExportType As String)

        '*************************************************************
        'Determine the type of Export and instantiate the object
        '*************************************************************
        Dim ds As New DataSet
        Dim tw As New System.IO.StringWriter
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)

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
        Select Case ExportType
            Case "Start Date and End Date (BEC)"
                selDate.Append("SELECT ('Serial_History.aspx?job=BEC&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                frmDate.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp")
                whrDate.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND sp.INSERT_TMSTM >= to_date('" & Request.QueryString("START") & " 12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM') AND sp.INSERT_TMSTM <= to_date('" & Request.QueryString("END") & " 12:00:07 AM', 'MM/DD/YYYY HH:MI:SS AM') ")
                If Request.QueryString("DEPT") <> "" Then
                    whrDate.Append("AND sp.REPORT_GROUP_ID = '" & Request.QueryString("DEPT") & "'")
                End If
                If Request.QueryString("MACHINE") <> "0" Then
                    whrDate.Append("AND sp.MACHINE_ID = '" & Request.QueryString("MACHINE_ID") & "'")
                End If
                If Request.QueryString("SHIFT") <> "0" Then
                    whrDate.Append("AND sp.SHIFT_ID = '" & Request.QueryString("SHIFT_ID") & "'")
                End If
                If Request.QueryString("PART_ID") <> "" Then
                    whrDate.Append("AND sp.MES_PART_ID = '" & Request.QueryString("PART_ID") & "'")
                End If
                sqlGetStandardPackHistory = selDate.ToString & frmDate.ToString & whrDate.ToString & ordStandardPack
            Case "Start Date and End Date (BAG)"
                selDate.Append("SELECT ('Serial_History.aspx?job=BAG&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                frmDate.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp")
                whrDate.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND sp.INSERT_TMSTM >= to_date('" & Request.QueryString("START") & " 12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM') AND sp.INSERT_TMSTM <= to_date('" & Request.QueryString("END") & " 12:00:07 AM', 'MM/DD/YYYY HH:MI:SS AM') ")
                If Request.QueryString("DEPT") <> "" Then
                    whrDate.Append("AND sp.REPORT_GROUP_ID = '" & Request.QueryString("DEPT") & "'")
                End If
                If Request.QueryString("MACHINE") <> "0" Then
                    whrDate.Append("AND sp.MACHINE_ID = '" & Request.QueryString("MACHINE_ID") & "'")
                End If
                If Request.QueryString("SHIFT") <> "0" Then
                    whrDate.Append("AND sp.SHIFT_ID = '" & Request.QueryString("SHIFT_ID") & "'")
                End If
                If Request.QueryString("PART_ID") <> "" Then
                    whrDate.Append("AND sp.MES_PART_ID = '" & Request.QueryString("PART_ID") & "'")
                End If
                sqlGetStandardPackHistory = selDate.ToString & frmDate.ToString & whrDate.ToString & ordStandardPack
            Case "Serial Number Range (BEC)"
                selSerial.Append("SELECT ('Serial_History.aspx?job=BEC&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                frmSerial.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp")
                whrSerial.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND sp.SERIAL_NBR >= '" & Request.QueryString("START") & "' AND sp.SERIAL_NBR <= '" & Request.QueryString("END") & "'")
                If Request.QueryString("DEPT") <> "" Then
                    whrSerial.Append("AND sp.REPORT_GROUP_ID = '" & Request.QueryString("DEPT") & "'")
                End If
                If Request.QueryString("MACHINE") <> "0" Then
                    whrSerial.Append("AND sp.MACHINE_ID = '" & Request.QueryString("MACHINE_ID") & "'")
                End If
                If Request.QueryString("SHIFT") <> "0" Then
                    whrSerial.Append("AND sp.SHIFT_ID = '" & Request.QueryString("SHIFT_ID") & "'")
                End If
                If Request.QueryString("PART_ID") <> "" Then
                    whrSerial.Append("AND sp.MES_PART_ID = '" & Request.QueryString("PART_ID") & "'")
                End If
                sqlGetStandardPackHistory = selSerial.ToString & frmSerial.ToString & whrSerial.ToString & ordStandardPack
            Case "Serial Number Range (BAG)"
                selSerial.Append("SELECT ('Serial_History.aspx?job=BAG&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                frmSerial.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp")
                whrSerial.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND sp.SERIAL_NBR >= '" & Request.QueryString("START") & "' AND sp.SERIAL_NBR <= '" & Request.QueryString("END") & "'")
                If Request.QueryString("DEPT") <> "" Then
                    whrSerial.Append("AND sp.REPORT_GROUP_ID = '" & Request.QueryString("DEPT") & "'")
                End If
                If Request.QueryString("MACHINE") <> "0" Then
                    whrSerial.Append("AND sp.MACHINE_ID = '" & Request.QueryString("MACHINE_ID") & "'")
                End If
                If Request.QueryString("SHIFT") <> "0" Then
                    whrSerial.Append("AND sp.SHIFT_ID = '" & Request.QueryString("SHIFT_ID") & "'")
                End If
                If Request.QueryString("PART_ID") <> "" Then
                    whrSerial.Append("AND sp.MES_PART_ID = '" & Request.QueryString("PART_ID") & "'")
                End If
                sqlGetStandardPackHistory = selSerial.ToString & frmSerial.ToString & whrSerial.ToString & ordStandardPack
            Case "Sub-Standard ID (BAG)"
                selBag.Append("SELECT ('Serial_History.aspx?job=BAG&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                frmBag.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp, MESDBA.SUB_STANDARD_PACK ssp")
                whrBag.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND ssp.STANDARD_PACK_ID = sp.STANDARD_PACK_ID AND ssp.SUB_STANDARD_PACK_ID >= '" & Request.QueryString("START") & "' AND ssp.SUB_STANDARD_PACK_ID <= '" & Request.QueryString("END") & "'")
                grpBag.Append(" GROUP BY sp.SERIAL_NBR, sp.STANDARD_PACK_ID, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                If Request.QueryString("DEPT") <> "" Then
                    whrBag.Append("AND sp.REPORT_GROUP_ID = '" & Request.QueryString("DEPT") & "'")
                End If
                If Request.QueryString("MACHINE") <> "0" Then
                    whrBag.Append(" AND sp.MACHINE_ID = '" & Request.QueryString("MACHINE_ID") & "'")
                    grpBag.Append(" ,sp.MACHINE_ID")
                End If
                If Request.QueryString("SHIFT") <> "0" Then
                    whrBag.Append(" AND sp.SHIFT_ID = '" & Request.QueryString("SHIFT_ID") & "'")
                    grpBag.Append(" ,sp.SHIFT_ID")
                End If
                If Request.QueryString("PART_ID") <> "" Then
                    whrBag.Append(" AND sp.MES_PART_ID = '" & Request.QueryString("PART_ID") & "'")
                    grpBag.Append(" ,sp.MES_PART_ID")
                End If
                sqlGetStandardPackHistory = selBag.ToString & frmBag.ToString & whrBag.ToString & grpBag.ToString & ordStandardPack
            Case "Sub-Standard ID (BEC)"
                selBec.Append("SELECT ('Serial_History.aspx?job=BEC&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                frmBec.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp, MESDBA.SERIALIZED_PRODUCT ssp")
                whrBec.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND ssp.STANDARD_PACK_ID = sp.STANDARD_PACK_ID AND ssp.PRODUCT_ID >= '" & Request.QueryString("START") & "' AND ssp.PRODUCT_ID <= '" & Request.QueryString("END") & "'")
                grpBec.Append(" GROUP BY sp.SERIAL_NBR, sp.STANDARD_PACK_ID, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                If Request.QueryString("DEPT") <> "" Then
                    whrBec.Append("AND sp.REPORT_GROUP_ID = '" & Request.QueryString("DEPT") & "'")
                End If
                If Request.QueryString("MACHINE") <> "0" Then
                    whrBec.Append(" AND sp.MACHINE_ID = '" & Request.QueryString("MACHINE_ID") & "'")
                    grpBec.Append(" ,sp.MACHINE_ID")
                End If
                If Request.QueryString("SHIFT") <> "0" Then
                    whrBec.Append(" AND sp.SHIFT_ID = '" & Request.QueryString("SHIFT_ID") & "'")
                    grpBec.Append(" ,sp.SHIFT_ID")
                End If
                If Request.QueryString("PART_ID") <> "" Then
                    whrBec.Append(" AND sp.MES_PART_ID = '" & Request.QueryString("PART_ID") & "'")
                    grpBec.Append(" ,sp.MES_PART_ID")
                End If
                sqlGetStandardPackHistory = selBec.ToString & frmBec.ToString & whrBec.ToString & grpBec.ToString & ordStandardPack
            Case "Alphanumeric Serial Number Pattern"
                selSerial.Append("SELECT ('Serial_History.aspx?job=BEC&StandardPack_Id=' || sp.STANDARD_PACK_ID || '') AS Serial_Content, sp.SERIAL_NBR, sp.INSERT_TMSTM, sp.REV_PHYSICAL, m.MACHINE_NAME, mp.PART_NBR, sp.LABEL_CODE, sp.STORE, sp.DISPOSITION_CODE")
                frmSerial.Append(" FROM MESDBA.STANDARD_PACK sp, MESDBA.MACHINE m, MESDBA.MES_PART mp")
                whrSerial.Append(" WHERE sp.MACHINE_ID = m.MACHINE_ID AND sp.MES_PART_ID = mp.MES_PART_ID AND UPPER(sp.SERIAL_NBR) like '%" & UCase(Request.QueryString("START")) & "%'")
                If Request.QueryString("DEPT") <> "0" Then
                    whrDate.Append("AND sp.REPORT_GROUP_ID = '" & Request.QueryString("DEPT") & "'")
                End If
                If Request.QueryString("MACHINE") <> "0" Then
                    whrDate.Append("AND sp.MACHINE_ID = '" & Request.QueryString("MACHINE") & "'")
                End If
                If Request.QueryString("SHIFT") <> "0" Then
                    whrDate.Append("AND sp.SHIFT_ID = '" & Request.QueryString("SHIFT") & "'")
                End If
                If Request.QueryString("PART_ID") <> "" Then
                    whrDate.Append("AND sp.MES_PART_ID = '" & Request.QueryString("PART_ID") & "'")
                End If
                sqlGetStandardPackHistory = selSerial.ToString & frmSerial.ToString & whrSerial.ToString & ordStandardPack
        End Select
        'Response.Write(sqlGetStandardPackHistory)
        ' Create a Command object with the SQL statement.
        MyConn.Open()
        Dim CmdGetData As New OleDb.OleDbDataAdapter(sqlGetStandardPackHistory, MyConn)
        CmdGetData.Fill(ds)
        ''*************************************************************
        ''Bind the dataset to the datagrid
        ''*************************************************************
        dgStandardPackHistory.DataSource = ds
        dgStandardPackHistory.DataBind()

        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()

        End If
        '*************************************************************
        'Export the datagrid to Excel
        '*************************************************************
        Response.ContentType = "application/vnd.ms-excel"
        Response.Charset = ""
        Me.EnableViewState = False
        Response.AppendHeader("Content-Disposition", "FileName = StandardPackHistory_" & Date.Today.ToShortDateString & ".xls")
        dgStandardPackHistory.RenderControl(hw)
        Response.Write(tw.ToString())
        Response.End()

    End Sub
End Class

