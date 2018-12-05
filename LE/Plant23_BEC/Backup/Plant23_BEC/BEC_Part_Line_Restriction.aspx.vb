Public Class BEC_Part_Line_Restriction
    Inherits System.Web.UI.Page
#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnLoadData As System.Web.UI.WebControls.Button
    Protected WithEvents txtMesPartId As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblRecordsInserted As System.Web.UI.WebControls.Label
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmbContainer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblContainer As System.Web.UI.WebControls.Label
    Protected WithEvents lblPartNumber As System.Web.UI.WebControls.Label
    Protected WithEvents lblMachineName As System.Web.UI.WebControls.Label
    Protected WithEvents txtPartNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmbMachineName As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dgPartLineRestriction As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtSecurityCheck As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
    Protected WithEvents lblCount As System.Web.UI.WebControls.Label
    Protected WithEvents tblResults As System.Web.UI.HtmlControls.HtmlTable
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
        'txtSecurityCheck.Text = GetSecurity(UCase(Right(Request.ServerVariables("LOGON_USER"), 6)))
        txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23"
        If txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23" Or txtSecurityCheck.Text = "BEC_ICS_WEB_USER_P23" Then
            If Not Page.IsPostBack Then
                Call Populate_cmbMachineName()
            End If
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If
        tblResults.Visible = False
    End Sub
    Private Sub btnLoadData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadData.Click
        txtMesPartId.Text = GetMESPartId(txtPartNumber.Text, "")
        Dim insPink_Pass As String
        Dim sqlPartLineRestriction As String
        Dim ins, vlu, sql
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        ins = "INSERT INTO MESDBA.PART_MACHINE_RESTRICTION (MES_PART_ID, MACHINE_ID, CONTAINER_CODE, INSERT_USERID) "
        vlu = "VALUES (" & txtMesPartId.Text & ", '" & cmbMachineName.SelectedValue & "', '" & cmbContainer.SelectedValue & "', '" & UCase(Right(Request.ServerVariables("LOGON_USER"), 6)) & "')"
        sql = ins & vlu
        'Response.Write(sql)
        sqlPartLineRestriction = "SELECT MES_PART_ID, MACHINE_ID, CONTAINER_CODE FROM MESDBA.PART_MACHINE_RESTRICTION WHERE MES_PART_ID = " & txtMesPartId.Text & " and MACHINE_ID = '" & cmbMachineName.SelectedValue & "' and CONTAINER_CODE = '" & cmbContainer.SelectedValue & "'"
        ' Create a Command object with the SQL statement.
        'Response.Write(sqlPartLineRestriction)
        MyConn.Open()
        Dim objCommand As New OleDb.OleDbCommand(sqlPartLineRestriction, MyConn)
        Dim objDataReader As OleDb.OleDbDataReader
        objDataReader = objCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        If objDataReader.HasRows = False Then
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
            End If
            Dim CmdInsert_Part_Machine_Restriction As New OleDb.OleDbCommand(sql, MyConn)
            Dim CmdCommit As New OleDb.OleDbCommand("commit", MyConn)
            CmdInsert_Part_Machine_Restriction.Connection.Open()
            CmdInsert_Part_Machine_Restriction.ExecuteNonQuery()
            'Response.Write(sql)
            CmdCommit.ExecuteNonQuery()
            If CmdInsert_Part_Machine_Restriction.Connection.State = ConnectionState.Open Then
                CmdInsert_Part_Machine_Restriction.Connection.Close()
            End If
            If CmdCommit.Connection.State = ConnectionState.Open Then
                CmdCommit.Connection.Close()
            End If
            lblRecordsInserted.Text = "The record was inserted..."
            lblRecordsInserted.Visible = True
            Call Populate_dgPartLineRestriction()
            dgPartLineRestriction.Visible = True
            tblResults.Visible = True
        Else
            lblRecordsInserted.Text = "That record already exists, try again..."
            lblRecordsInserted.Visible = True
            Call Populate_dgPartLineRestriction()
            dgPartLineRestriction.Visible = True
            tblResults.Visible = True
        End If
    End Sub
    Sub dgPartLineRestriction_PageChange(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        Dim start As Integer
        start = dgPartLineRestriction.CurrentPageIndex * dgPartLineRestriction.PageSize
        dgPartLineRestriction.CurrentPageIndex = e.NewPageIndex
        Call Populate_dgPartLineRestriction()
    End Sub
    Sub Populate_cmbMachineName()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbMachineName As String = "SELECT MACHINE_NAME, MACHINE_ID FROM MESDBA.MACHINE WHERE REPORT_GROUP_ID = '1125' OR REPORT_GROUP_ID = '1126' OR REPORT_GROUP_ID = '1127' GROUP BY MACHINE_NAME, MACHINE_ID ORDER BY MACHINE_NAME ASC"
        'Dept 2341 - 1125 - Underhood
        'Dept 2342 - 1126 - Mid
        'Dept 2343 - 1127 - Left
        Dim CmdMachineName As New OleDb.OleDbCommand(sqlCmbMachineName, MyConn)
        MyConn.Open()
        objDR = CmdMachineName.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        cmbMachineName.DataSource = objDR
        cmbMachineName.DataValueField = "MACHINE_ID"
        cmbMachineName.DataTextField = "MACHINE_NAME"
        cmbMachineName.DataBind()
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
    End Sub
    Sub Populate_dgPartLineRestriction()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlPartLineRestriction As String
        sqlPartLineRestriction = "SELECT MACHINE_NAME, PART_NBR, CONTAINER_CODE, mr.INSERT_USERID, mr.INSERT_TMSTM FROM MESDBA.MES_PART mp, (MESDBA.PART_MACHINE_RESTRICTION mr LEFT JOIN MESDBA.MACHINE m ON mr.MACHINE_ID = m.MACHINE_ID) WHERE mr.MES_PART_ID = mp. MES_PART_ID ORDER BY PART_NBR"
        ' Create a Command object with the SQL statement.
        'Response.Write(sqlPartLineRestriction)
        MyConn.Open()
        Dim objCommand As New OleDb.OleDbCommand(sqlPartLineRestriction, MyConn)
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
        dgPartLineRestriction.DataSource = objDataReader
        dgPartLineRestriction.DataBind()
        dgPartLineRestriction.Visible = True
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
    End Sub
    Private Sub txtPartNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        txtMesPartId.Text = GetMESPartId(txtPartNumber.Text, "")
    End Sub
End Class
