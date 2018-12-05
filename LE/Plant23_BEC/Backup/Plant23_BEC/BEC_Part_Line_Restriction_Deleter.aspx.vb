Public Class BEC_Part_Line_Restriction_Deleter
    Inherits System.Web.UI.Page
#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents dgPartLineRestriction As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtSecurityCheck As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
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
            Call Populate_dgPartLineRestriction()
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If
    End Sub
    Sub dgPartLineRestriction_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Try
            Dim sqlDeletePartLineRestriction As String = "DELETE FROM MESDBA.PART_MACHINE_RESTRICTION WHERE MACHINE_ID = " & e.Item.Cells(0).Text & " and MES_PART_ID = " & e.Item.Cells(1).Text & " and CONTAINER_CODE = '" & e.Item.Cells(4).Text & "' "
            Dim objCommand As New OleDb.OleDbCommand(sqlDeletePartLineRestriction, MyConn)
            Dim sqlCommit As String = "COMMIT"
            Dim objCommitCommand As New OleDb.OleDbCommand(sqlCommit, MyConn)
            'Response.Write(sqlDeletePartLineRestriction)
            MyConn.Open()
            objCommand.ExecuteNonQuery()
            dgPartLineRestriction.EditItemIndex = -1
            If MyConn.State = ConnectionState.Open Then
                objCommand.Dispose()
                MyConn.Close()
            End If
            MyConn.Open()
            objCommitCommand.ExecuteNonQuery()
            If MyConn.State = ConnectionState.Open Then
                objCommitCommand.Dispose()
                MyConn.Close()
            End If
            MyConn.Dispose()
            Call Populate_dgPartLineRestriction()
        Catch ex As Exception
            lblMessage.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub
    Sub Populate_dgPartLineRestriction()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlPartLineRestriction As String
        Try
            sqlPartLineRestriction = "SELECT MACHINE_NAME, PART_NBR, CONTAINER_CODE, mr.INSERT_USERID, mr.INSERT_TMSTM, mr.MACHINE_ID, mr.MES_PART_ID FROM MESDBA.MES_PART mp, (MESDBA.PART_MACHINE_RESTRICTION mr LEFT JOIN MESDBA.MACHINE m ON mr.MACHINE_ID = m.MACHINE_ID) WHERE mr.MES_PART_ID = mp. MES_PART_ID ORDER BY PART_NBR"
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
            If objDataReader.HasRows = True Then
                dgPartLineRestriction.DataSource = objDataReader
                dgPartLineRestriction.DataBind()
                dgPartLineRestriction.Visible = True
                tblResults.Visible = True
            Else
                lblMessage.Text = "No Data Was Found..."
                tblResults.Visible = False
            End If
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
            End If
        Catch ex As Exception
            lblMessage.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub
End Class
