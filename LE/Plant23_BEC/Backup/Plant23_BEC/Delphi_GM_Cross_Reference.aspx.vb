Public Class Delphi_GM_Cross_Reference
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblGMPartNumber As System.Web.UI.WebControls.Label
    Protected WithEvents cmbDelphiPart As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbGMPart As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblDelphiPartNumber As System.Web.UI.WebControls.Label
    Protected WithEvents btnFullList As System.Web.UI.WebControls.Button
    Protected WithEvents lblSortOrder As System.Web.UI.WebControls.Label
    Protected WithEvents lblCount As System.Web.UI.WebControls.Label
    Protected WithEvents dgPartNumberCrossReference As System.Web.UI.WebControls.DataGrid
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
        tblResults.Visible = False
        If Not Page.IsPostBack Then
            viewstate("sortField") = "PART_NBR"
            viewstate("sortDirection") = "ASC"
            lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
            Call Populate_cmbDelphiPart()
            Call Populate_cmbGMPart()
        End If

    End Sub
    Sub dgPartNumberCrossReference_SortCommand(ByVal Source As Object, ByVal E As DataGridSortCommandEventArgs)
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
        Populate_dgPartNumberCrossReference()
    End Sub
    Sub Populate_dgPartNumberCrossReference()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlPartData As String
        Try
            sel = "SELECT DISTINCT PART_NBR, CUSTOMER_PART_NBR"
            frm = " FROM PLTFLOOR.ICS_LABEL_DATA_VW"
            grp = " GROUP BY PART_NBR, CUSTOMER_PART_NBR"
            ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
            sqlPartData = sel & frm & whr & grp & ord
            'Response.Write(sqlPartData)
            ' Create a Command object with the SQL statement.
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlPartData, MyConn)
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
                dgPartNumberCrossReference.Visible = False
                lblMessage.Visible = True
                tblResults.Visible = False
            Else
                dgPartNumberCrossReference.DataSource = objDataReader
                dgPartNumberCrossReference.DataBind()
                dgPartNumberCrossReference.Visible = True
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
    Sub Populate_cmbDelphiPart()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbDelphiPart As String = "SELECT DISTINCT PART_NBR, CUSTOMER_PART_NBR FROM PLTFLOOR.ICS_LABEL_DATA_VW GROUP BY PART_NBR, CUSTOMER_PART_NBR ORDER BY PART_NBR"
        Dim CmdPartNumber As New OleDb.OleDbCommand(sqlCmbDelphiPart, MyConn)
        MyConn.Open()
        objDR = CmdPartNumber.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        cmbDelphiPart.DataSource = objDR
        cmbDelphiPart.DataValueField = "CUSTOMER_PART_NBR"
        cmbDelphiPart.DataTextField = "PART_NBR"
        cmbDelphiPart.DataBind()
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
    End Sub
    Sub Populate_cmbGMPart()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbGMPart As String = "SELECT DISTINCT CUSTOMER_PART_NBR, PART_NBR FROM PLTFLOOR.ICS_LABEL_DATA_VW GROUP BY CUSTOMER_PART_NBR, PART_NBR ORDER BY CUSTOMER_PART_NBR"
        Dim CmdPartNumber As New OleDb.OleDbCommand(sqlCmbGMPart, MyConn)
        MyConn.Open()
        objDR = CmdPartNumber.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        cmbGMPart.DataSource = objDR
        cmbGMPart.DataValueField = "PART_NBR"
        cmbGMPart.DataTextField = "CUSTOMER_PART_NBR"
        cmbGMPart.DataBind()
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
    End Sub

    Private Sub btnFullList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFullList.Click
        Call Populate_dgPartNumberCrossReference()
    End Sub

    Private Sub cmbDelphiPart_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDelphiPart.SelectedIndexChanged
        lblGMPartNumber.Text = cmbDelphiPart.SelectedItem.Value

    End Sub

    Private Sub cmbGMPart_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbGMPart.SelectedIndexChanged
        lblDelphiPartNumber.Text = cmbGMPart.SelectedItem.Value
    End Sub
End Class
