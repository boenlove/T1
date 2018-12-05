Public Class BEC_SerialNumberAvailability
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dgICS_LABEL_DATA As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblSortOrder As System.Web.UI.WebControls.Label
    Protected WithEvents txtPartNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtECL As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDestination As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPackageCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSecurityCheck As System.Web.UI.WebControls.TextBox
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
            'Put user code to initialize the page here
            txtPartNumber.Text = Request("PartNbr")
            txtECL.Text = Request("ECL")
            txtDestination.Text = Request("Destination")
            txtPackageCode.Text = Request("PackageCode")
            If Not Page.IsPostBack Then
                viewstate("sortField") = "PART_NBR"
                viewstate("sortDirection") = "ASC"
                lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
                Call Populate_dgICS_LABEL_DATA()
            End If
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If
    End Sub
    Sub dgICS_LABEL_DATA_SortCommand(ByVal Source As Object, ByVal E As DataGridSortCommandEventArgs)
        If E.SortExpression.ToString() = viewstate("sortField").ToString() Then
            Select Case viewstate("sortDirection").ToString()
                Case "ASC"
                    viewstate("sortDirection") = "DESC"
                Case "DESC"
                    viewstate("sortDirection") = "ASC"
            End Select
        Else
            viewstate("sortField") = E.SortExpression
            viewstate("sortDirection") = "DESC"
        End If
        ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
        lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
        Populate_dgICS_LABEL_DATA()
    End Sub
    Sub Populate_dgICS_LABEL_DATA()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlGetLabelData As String
        Dim SearchDate As String
        Try
            SearchDate = DateAdd(DateInterval.Day, -350, DateTime.Today())
            sel = "SELECT PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, MATID, COUNT(SERIAL_NBR)"
            frm = " FROM PLTFLOOR.ICS_LABEL_DATA_VW"
            whr = " WHERE PART_NBR = '" & txtPartNumber.Text & "' AND ECL = '" & txtECL.Text & "' AND DESTINATION = '" & txtDestination.Text & "' AND PACKAGE_CODE = '" & txtPackageCode.Text & "' AND MOD_TMSTM >= to_date('" & SearchDate & "', 'MM/DD/YYYY HH:MI:SS AM') AND USED_STATUS = 0"
            grp = " GROUP BY PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, MATID"
            ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
            sqlGetLabelData = sel & frm & whr & grp & ord
            'sqlGetLabelData = "SELECT PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, COUNT(PART_NBR), MATID, MOD_TMSTM FROM PLTFLOOR.ICS_LABEL_DATA_VW WHERE PART_NBR = '" & cmbPartNumber.SelectedItem.Text & "'AND ECL = '" & cmbECL.SelectedValue & "' AND DESTINATION = '" & cmbDestination.SelectedValue & "' AND PACKAGE_CODE = '" & cmbPackageCode.SelectedValue & "' AND MOD_TMSTM >= to_date('" & SearchDate & "  12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM') AND USED_STATUS = 0 GROUP BY PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, MATID, MOD_TMSTM ORDER BY MOD_TMSTM"
            'Response.Write(sqlGetLabelData)
            ' Create a Command object with the SQL statement.
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlGetLabelData, MyConn)
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
                lblMessage.Text = "No Records were found."
                dgICS_LABEL_DATA.Visible = False
                lblMessage.Visible = True
                tblResults.Visible = False
            Else
                dgICS_LABEL_DATA.DataSource = objDataReader
                dgICS_LABEL_DATA.DataBind()
                dgICS_LABEL_DATA.Visible = True
                lblMessage.Visible = False
                tblResults.Visible = True
            End If
            If MyConn.State = ConnectionState.Open Then
                MyConn.Dispose()
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
End Class
