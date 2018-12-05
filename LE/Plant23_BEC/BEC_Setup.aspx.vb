Imports System.Text
Partial Class BEC_Setup
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
        Dim x As String = GetServerType()
        If x = "LOCAL." Or x = "TEST." Then
            lblDisplay.Visible = True
            lblDisplay.Text = "TEST ENVIRONMENT"
        End If
        'txtSecurityCheck.Text = GetSecurity(UCase(Right(Request.ServerVariables("LOGON_USER"), 6)))
        txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23"
        If txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23" Or txtSecurityCheck.Text = "BEC_ICS_WEB_USER_P23" Then
            If Not Page.IsPostBack Then
                Call Populate_cmbPartNumber()
                Call Populate_dgWhatsSetup()
            End If
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If
    End Sub
    Sub Populate_dgWhatsSetup()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        'Dim sqlGetSchedule As New StringBuilder
        Dim sqlGetSchedule As String
        Try
            If txtContainer.Text <> "" Then
                sqlGetSchedule = "SELECT mp.PART_NBR, REVISION_PHYSICAL, CONTAINER_CODE, PACKAGE_CODE, BUILD_PRIORITY, COUNT(PART_NBR) AS PART_COUNT FROM MESDBA.ICS_SCHEDULE_POOL ip, MESDBA.MES_PART mp WHERE ip.MES_PART_ID = mp.MES_PART_ID and ip.MES_PART_ID = '" & cmbPartNumber.SelectedValue & "' and CONTAINER_CODE = '" & UCase(txtContainer.Text) & "' GROUP BY PART_NBR, REVISION_PHYSICAL, CONTAINER_CODE, PACKAGE_CODE, BUILD_PRIORITY ORDER BY BUILD_PRIORITY"
            Else
                sqlGetSchedule = "SELECT mp.PART_NBR, REVISION_PHYSICAL, CONTAINER_CODE, PACKAGE_CODE, BUILD_PRIORITY, COUNT(PART_NBR) AS PART_COUNT FROM MESDBA.ICS_SCHEDULE_POOL ip, MESDBA.MES_PART mp WHERE ip.MES_PART_ID = mp.MES_PART_ID and ip.MES_PART_ID = '" & cmbPartNumber.SelectedValue & "' GROUP BY PART_NBR, REVISION_PHYSICAL, CONTAINER_CODE, PACKAGE_CODE, BUILD_PRIORITY ORDER BY BUILD_PRIORITY"
            End If
            'Response.Write(sqlGetSchedule)
            ' Create a Command object with the SQL statement.
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlGetSchedule, MyConn)
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
                If txtContainer.Text = "" And cmbPartNumber.SelectedValue <> "" Then
                    lblNoDataFound.Text = "No Records were found for Part Number: " & cmbPartNumber.SelectedItem.Text & ""
                ElseIf cmbPartNumber.SelectedValue <> "" Then
                    lblNoDataFound.Text = "No Records were found for Part Number: " & cmbPartNumber.SelectedItem.Text & " and Container: " & txtContainer.Text & ""
                Else
                    lblNoDataFound.Text = "No Records were found."
                End If
                dgWhatsSetup.Visible = False
                If cmbPartNumber.SelectedValue <> "" Then
                    lblNoDataFound.Visible = True
                End If
                tblResults.Visible = False
            Else
                dgWhatsSetup.DataSource = objDataReader
                dgWhatsSetup.DataBind()
                dgWhatsSetup.Visible = True
                lblNoDataFound.Visible = False
                tblResults.Visible = True
            End If
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
            End If
        Catch ex As Exception
            lblMessage.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub
    Sub Populate_cmbPartNumber()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbPartNumber As String = "SELECT PART_NBR, ip.MES_PART_ID FROM MESDBA.ICS_SCHEDULE_POOL ip, MESDBA.MES_PART mp WHERE ip.MES_PART_ID = mp.MES_PART_ID GROUP BY mp.PART_NBR, ip.MES_PART_ID ORDER BY PART_NBR ASC"
        Dim CmdPartNumber As New OleDb.OleDbCommand(sqlCmbPartNumber, MyConn)
        'Response.Write(sqlCmbPartNumber)
        MyConn.Open()
        objDR = CmdPartNumber.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        cmbPartNumber.DataSource = objDR
        cmbPartNumber.DataValueField = "MES_PART_ID"
        cmbPartNumber.DataTextField = "PART_NBR"
        cmbPartNumber.DataBind()
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
    End Sub
    Private Sub btnWhatsSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWhatsSetup.Click
        Populate_dgWhatsSetup()
    End Sub

    Private Sub dgWhatsSetup_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Dim row As DataGridItem
        Dim count As Integer
        count = 0
        For Each row In dgWhatsSetup.Items
            count = count + 1
            row.Cells(0).Text = count
        Next
    End Sub
End Class
