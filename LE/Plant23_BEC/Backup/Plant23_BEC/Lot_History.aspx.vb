Public Class Lot_History
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtJob As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSecurityCheck As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCount As System.Web.UI.WebControls.Label
    Protected WithEvents lblSortOrder As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
    Protected WithEvents tblResults As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents dgLOT As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtProductId As System.Web.UI.WebControls.TextBox

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
        txtProductId.Text = Request("Product_Id")
        If Not Page.IsPostBack Then
            viewstate("sortField") = "LOT_SERIAL_NBR"
                viewstate("sortDirection") = "ASC"
                lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
                Call Populate_dgLOT()
        End If
    End Sub
    Sub dgLOT_SortCommand(ByVal Source As Object, ByVal E As DataGridSortCommandEventArgs)
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
        Populate_dgLOT()
    End Sub
    Sub Populate_dgLOT()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlGetLotData As String
        Dim SearchDate As String
        Try
            SearchDate = DateAdd(DateInterval.Day, -350, DateTime.Today())
            sel = "SELECT lh.LOT_SERIAL_NBR, lh.LOT_TMSTM"
            frm = " FROM MESDBA.LOT_TRACE_HISTORY lth, MESDBA.SERIALIZED_PRODUCT sp, MESDBA.LOT_HISTORY lh"
            whr = " WHERE sp.PRODUCT_ID = lth.PRODUCT_ID and lh.LOT_ID = lth.LOT_ID and lth.PRODUCT_ID = '" & txtProductId.Text & "'"
            ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
            sqlGetLotData = sel & frm & whr & ord
            'Response.Write(sqlGetLotData)
            ' Create a Command object with the SQL statement.
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlGetLotData, MyConn)
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
                dgLOT.Visible = False
                lblMessage.Visible = True
                lblSortOrder.Visible = False
                tblResults.Visible = False
            Else
                dgLOT.DataSource = objDataReader
                dgLOT.DataBind()
                dgLOT.Visible = True
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
