Public Class Serial_History
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents txtSecurityCheck As System.Web.UI.WebControls.TextBox
    Protected WithEvents dgSUB_STANDARD_PACK As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtStandardPackId As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtJob As System.Web.UI.WebControls.TextBox
    Protected WithEvents dgPRODUCT_ID As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
    Protected WithEvents lblCount As System.Web.UI.WebControls.Label
    Protected WithEvents tblResults As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblSortOrder As System.Web.UI.WebControls.Label

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
        txtStandardPackId.Text = Request("StandardPack_Id")
        txtJob.Text = Request("job")
        If Not Page.IsPostBack Then
            If txtJob.Text = "BEC" Then
                viewstate("sortField") = "SERIAL_NBR"
                viewstate("sortDirection") = "ASC"
                lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
                Call Populate_dgPRODUCT_ID()
            ElseIf txtJob.Text = "BAG" Then
                viewstate("sortField") = "SUB_STANDARD_PACK_ID"
                viewstate("sortDirection") = "ASC"
                lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
                Call Populate_dgSUB_STANDARD_PACK()
            End If
        End If
    End Sub
    Sub dgPRODUCT_ID_SortCommand(ByVal Source As Object, ByVal E As DataGridSortCommandEventArgs)
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
        ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
        lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
        Populate_dgSUB_STANDARD_PACK()
    End Sub
    Sub Populate_dgSUB_STANDARD_PACK()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlGetSerialData As String
        Dim SearchDate As String
        Try
            SearchDate = DateAdd(DateInterval.Day, -350, DateTime.Today())
            sel = "SELECT SUB_STANDARD_PACK_ID, INSERT_TMSTM"
            frm = " FROM MESDBA.SUB_STANDARD_PACK"
            whr = " WHERE STANDARD_PACK_ID = '" & txtStandardPackId.Text & "'"
            ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
            sqlGetSerialData = sel & frm & whr & ord
            'Response.Write(sqlGetSerialData)
            ' Create a Command object with the SQL statement.
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlGetSerialData, MyConn)
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
                dgSUB_STANDARD_PACK.Visible = False
                dgPRODUCT_ID.Visible = False
                lblMessage.Visible = True
                lblSortOrder.Visible = False
                tblResults.Visible = False
            Else
                dgSUB_STANDARD_PACK.DataSource = objDataReader
                dgSUB_STANDARD_PACK.DataBind()
                dgSUB_STANDARD_PACK.Visible = True
                dgPRODUCT_ID.Visible = False
                lblSortOrder.Visible = True
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
    Sub dgSUB_STANDARD_PACK_SortCommand(ByVal Source As Object, ByVal E As DataGridSortCommandEventArgs)
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
        Populate_dgSUB_STANDARD_PACK()
    End Sub
    Sub Populate_dgPRODUCT_ID()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlGetSerialData As String
        Dim SearchDate As String
        Try
            SearchDate = DateAdd(DateInterval.Day, -350, DateTime.Today())
            sel = "SELECT ('Lot_History.aspx?Product_Id=' || PRODUCT_ID || '') AS Lot_Content, SERIAL_NBR, PRODUCED_TMSTM"
            frm = " FROM MESDBA.SERIALIZED_PRODUCT"
            whr = " WHERE STANDARD_PACK_ID = '" & txtStandardPackId.Text & "'"
            ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
            sqlGetSerialData = sel & frm & whr & ord
            'Response.Write(sqlGetSerialData)
            ' Create a Command object with the SQL statement.
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlGetSerialData, MyConn)
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
                dgPRODUCT_ID.Visible = False
                dgSUB_STANDARD_PACK.Visible = False
                lblMessage.Visible = True
                lblSortOrder.Visible = False
                tblResults.Visible = False
            Else
                dgPRODUCT_ID.DataSource = objDataReader
                dgPRODUCT_ID.DataBind()
                dgPRODUCT_ID.Visible = True
                dgSUB_STANDARD_PACK.Visible = False
                lblSortOrder.Visible = True
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
