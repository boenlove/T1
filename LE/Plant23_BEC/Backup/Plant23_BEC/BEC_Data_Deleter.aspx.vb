Public Class BEC_Data_Deleter
    Inherits System.Web.UI.Page
#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents lblPartNumber As System.Web.UI.WebControls.Label
    Protected WithEvents lblECL As System.Web.UI.WebControls.Label
    Protected WithEvents lblContainer As System.Web.UI.WebControls.Label
    Protected WithEvents lblPackageCode As System.Web.UI.WebControls.Label
    Protected WithEvents cmbPartNumber As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbECL As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbPackageCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbContainer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblRecordsDeleted As System.Web.UI.WebControls.Label
    Protected WithEvents btnDeleteData As System.Web.UI.WebControls.Button
    Protected WithEvents txtMesPartId As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSecurityCheck As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
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
        lblMessage.Text = ""
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
                Call Populate_cmbECL()
                Call Populate_cmbContainer()
                Call Populate_cmbPackageCode()
            End If
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If
    End Sub
    Private Sub btnDeleteData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteData.Click
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Try
            txtMesPartId.Text = GetMESPartId(cmbPartNumber.SelectedItem.Text, "")
            Dim sql, sqlCheck
            sqlCheck = "SELECT * FROM MESDBA.ICS_SCHEDULE_POOL WHERE MES_PART_ID = " & txtMesPartId.Text & " and REVISION_PHYSICAL = '" & cmbECL.SelectedValue & "' and CONTAINER_CODE = '" & cmbContainer.SelectedValue & "' and PACKAGE_CODE = '" & cmbPackageCode.SelectedValue & "'"
            sql = "DELETE MESDBA.ICS_SCHEDULE_POOL WHERE MES_PART_ID = " & txtMesPartId.Text & " and REVISION_PHYSICAL = '" & cmbECL.SelectedValue & "' and CONTAINER_CODE = '" & cmbContainer.SelectedValue & "' and PACKAGE_CODE = '" & cmbPackageCode.SelectedValue & "'"
            'Response.Write(sqlCheck)
            'Response.Write(cmbPartNumber.SelectedItem.Text)
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlCheck, MyConn)
            Dim objDataReader As OleDb.OleDbDataReader
            objDataReader = objCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
            If objDataReader.HasRows = True Then
                If MyConn.State = ConnectionState.Open Then
                    MyConn.Close()
                End If
                MyConn = New OleDb.OleDbConnection(strConn)
                Dim CmdDelete_ICS_Schedule_Pool As New OleDb.OleDbCommand(sql, MyConn)

                Dim j As Integer = 0
                CmdDelete_ICS_Schedule_Pool.Connection.Open()
                CmdDelete_ICS_Schedule_Pool.ExecuteNonQuery()
                If CmdDelete_ICS_Schedule_Pool.Connection.State = ConnectionState.Open Then
                    CmdDelete_ICS_Schedule_Pool.Connection.Close()
                End If
                Dim CmdCommit As New OleDb.OleDbCommand("commit", MyConn)
                CmdCommit.Connection.Open()
                CmdCommit.ExecuteNonQuery()
                If CmdCommit.Connection.State = ConnectionState.Open Then
                    CmdCommit.Connection.Close()
                End If
                lblRecordsDeleted.Text = "The records for Part Number " & cmbPartNumber.SelectedItem.Text & " have been deleted"
                lblRecordsDeleted.Visible = True
            Else
                lblRecordsDeleted.Text = "That record does not exists, try again..."
                lblRecordsDeleted.Visible = True
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
    Private Sub lblWhatsSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect("BEC_Setup.aspx?txtPartNumber=" & cmbPartNumber.SelectedValue & "&txtContainer=" & cmbContainer.SelectedValue & "")
    End Sub
    Sub Populate_cmbPartNumber()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbPartNumber As String = "SELECT PART_NBR, ip.MES_PART_ID FROM MESDBA.ICS_SCHEDULE_POOL ip, MESDBA.MES_PART mp WHERE ip.MES_PART_ID = mp.MES_PART_ID GROUP BY mp.PART_NBR, ip.MES_PART_ID ORDER BY PART_NBR ASC"
        Dim CmdPartNumber As New OleDb.OleDbCommand(sqlCmbPartNumber, MyConn)
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
    Sub Populate_cmbECL()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbECL As String = "SELECT REVISION_PHYSICAL, ip.MES_PART_ID FROM MESDBA.ICS_SCHEDULE_POOL ip, MESDBA.MES_PART mp WHERE ip.MES_PART_ID = '" & cmbPartNumber.SelectedValue & "' and ip.MES_PART_ID = mp.MES_PART_ID  GROUP BY ip.REVISION_PHYSICAL, ip.MES_PART_ID ORDER BY REVISION_PHYSICAL ASC"
        Dim CmdECL As New OleDb.OleDbCommand(sqlCmbECL, MyConn)
        MyConn.Open()
        objDR = CmdECL.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        cmbECL.DataSource = objDR
        cmbECL.DataValueField = "REVISION_PHYSICAL"
        cmbECL.DataTextField = "REVISION_PHYSICAL"
        cmbECL.DataBind()
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
    End Sub
    Sub Populate_cmbContainer()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbContainer As String = "SELECT CONTAINER_CODE, ip.MES_PART_ID FROM MESDBA.ICS_SCHEDULE_POOL ip, MESDBA.MES_PART mp WHERE ip.MES_PART_ID = '" & cmbPartNumber.SelectedValue & "' and ip.MES_PART_ID = mp.MES_PART_ID  GROUP BY ip.CONTAINER_CODE, ip.MES_PART_ID ORDER BY CONTAINER_CODE ASC"
        Dim CmdContainer As New OleDb.OleDbCommand(sqlCmbContainer, MyConn)
        MyConn.Open()
        objDR = CmdContainer.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        cmbContainer.DataSource = objDR
        cmbContainer.DataValueField = "CONTAINER_CODE"
        cmbContainer.DataTextField = "CONTAINER_CODE"
        cmbContainer.DataBind()
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
    End Sub
    Sub Populate_cmbPackageCode()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbPackageCode As String = "SELECT PACKAGE_CODE, ip.MES_PART_ID FROM MESDBA.ICS_SCHEDULE_POOL ip, MESDBA.MES_PART mp WHERE ip.MES_PART_ID = '" & cmbPartNumber.SelectedValue & "' and ip.MES_PART_ID = mp.MES_PART_ID  GROUP BY ip.PACKAGE_CODE, ip.MES_PART_ID ORDER BY PACKAGE_CODE ASC"
        Dim CmdPackageCode As New OleDb.OleDbCommand(sqlCmbPackageCode, MyConn)
        MyConn.Open()
        objDR = CmdPackageCode.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        cmbPackageCode.DataSource = objDR
        cmbPackageCode.DataValueField = "PACKAGE_CODE"
        cmbPackageCode.DataTextField = "PACKAGE_CODE"
        cmbPackageCode.DataBind()
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
    End Sub

    Private Sub cmbPartNumber_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPartNumber.SelectedIndexChanged
        txtMesPartId.Text = GetMESPartId(cmbPartNumber.SelectedItem.Text, "")
        Call Populate_cmbECL()
        Call Populate_cmbContainer()
        Call Populate_cmbPackageCode()
    End Sub
End Class
