Public Class BEC_SerialNumberAvailability_withDLoc
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblPartNumber As System.Web.UI.WebControls.Label
    Protected WithEvents cmbPartNumber As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblDestination As System.Web.UI.WebControls.Label
    Protected WithEvents cmbDestination As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents txtMesPartId As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblPackageCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblECL As System.Web.UI.WebControls.Label
    Protected WithEvents cmbPackageCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbECL As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtSecurityCheck As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblDisplay As System.Web.UI.WebControls.Label
    Protected WithEvents dgICS_LABEL_DATA As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCount As System.Web.UI.WebControls.Label
    Protected WithEvents lblSortOrder As System.Web.UI.WebControls.Label
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
        If txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23" Then
            'Put user code to initialize the page here
            If Not Page.IsPostBack Then
                viewstate("sortField") = "PART_NBR"
                viewstate("sortDirection") = "ASC"
                lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
                Call Populate_cmbPartNumber()
                Call Populate_cmbDestination()
                Call Populate_cmbECL()
                Call Populate_cmbPackageCode()
                tblResults.Visible = False
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
            viewstate("sortDirection") = "ASC"
        End If
        ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
        lblSortOrder.Text = "Current Sort Order is " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
        Populate_dgICS_LABEL_DATA()
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
    Sub Populate_cmbDestination()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbDestination As String = "SELECT CONTAINER_CODE, ip.MES_PART_ID FROM MESDBA.ICS_SCHEDULE_POOL ip, MESDBA.MES_PART mp WHERE ip.MES_PART_ID = '" & cmbPartNumber.SelectedValue & "' and ip.MES_PART_ID = mp.MES_PART_ID  GROUP BY ip.CONTAINER_CODE, ip.MES_PART_ID ORDER BY CONTAINER_CODE ASC"
        Dim CmdDestination As New OleDb.OleDbCommand(sqlCmbDestination, MyConn)
        MyConn.Open()
        objDR = CmdDestination.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        cmbDestination.DataSource = objDR
        cmbDestination.DataValueField = "CONTAINER_CODE"
        cmbDestination.DataTextField = "CONTAINER_CODE"
        cmbDestination.DataBind()
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
    Sub Populate_cmbECL()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbECL As String = "SELECT REVISION_PHYSICAL, ip.MES_PART_ID FROM MESDBA.ICS_SCHEDULE_POOL ip, MESDBA.MES_PART mp WHERE ip.MES_PART_ID = '" & cmbPartNumber.SelectedValue & "' and ip.MES_PART_ID = mp.MES_PART_ID  GROUP BY ip.REVISION_PHYSICAL, ip.MES_PART_ID ORDER BY REVISION_PHYSICAL"
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
    Sub dgICS_LABEL_DATA_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles dgICS_LABEL_DATA.DeleteCommand
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Try
            Dim sqlUpdateICS_LABEL_DATA As String
            If Val(Trim(e.Item.Cells(7).Text)) = "0" Then
                sqlUpdateICS_LABEL_DATA = "UPDATE MESDBA.VW_ICS_LABEL_DATA SET USED_STATUS = 9, MOD_TMSTM = SYSDATE WHERE USED_STATUS = 0 AND PART_NBR = '" & e.Item.Cells(0).Text & "' and ECL = '" & e.Item.Cells(1).Text & "' and DESTINATION = '" & e.Item.Cells(2).Text & "' and PACKAGE_CODE = '" & e.Item.Cells(3).Text & "' and trunc(MOD_TMSTM) = to_date('" & e.Item.Cells(8).Text & "', 'MM/DD/YYYY')"
            Else
                sqlUpdateICS_LABEL_DATA = "UPDATE MESDBA.VW_ICS_LABEL_DATA SET USED_STATUS = 9, MOD_TMSTM = SYSDATE WHERE USED_STATUS = 0 AND PART_NBR = '" & e.Item.Cells(0).Text & "' and ECL = '" & e.Item.Cells(1).Text & "' and DESTINATION = '" & e.Item.Cells(2).Text & "' and PACKAGE_CODE = '" & e.Item.Cells(3).Text & "' and DELIVERY_LOC_NBR = '" & e.Item.Cells(7).Text & "' and trunc(MOD_TMSTM) = to_date('" & e.Item.Cells(8).Text & "', 'MM/DD/YYYY')"
            End If
            Dim objCommand As New OleDb.OleDbCommand(sqlUpdateICS_LABEL_DATA, MyConn)
            Dim sqlCommit As String = "COMMIT"
            Dim objCommitCommand As New OleDb.OleDbCommand(sqlCommit, MyConn)
            Dim SearchDate As String
            Dim sqlGetLabelData As String
            Dim MyDataset As New DataSet
            Dim MyTable As New DataTable
            'Response.Write(sqlUpdateICS_LABEL_DATA)
            'Response.Write(IsDBNull(Trim(e.Item.Cells(7).Text)))
            'Response.Write(Val(Trim(e.Item.Cells(7).Text)))
            SearchDate = DateAdd(DateInterval.Day, -350, DateTime.Today())
            sel = "SELECT PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, COUNT(SERIAL_NBR), DELIVERY_LOC_NBR, to_char(MOD_TMSTM,'mm/dd/yyyy')as TimeStamp"
            frm = " FROM PLTFLOOR.ICS_LABEL_DATA_VW"
            whr = " WHERE PART_NBR = '" & cmbPartNumber.SelectedItem.Text & "' AND DESTINATION = '" & cmbDestination.SelectedValue & "' AND ECL = '" & cmbECL.SelectedValue & "' AND PACKAGE_CODE = '" & cmbPackageCode.SelectedValue & "' AND MOD_TMSTM >= to_date('" & SearchDate & "', 'MM/DD/YYYY HH:MI:SS AM') AND USED_STATUS = 0"
            grp = " GROUP BY PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, DELIVERY_LOC_NBR, to_char(MOD_TMSTM,'mm/dd/yyyy')"
            ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
            sqlGetLabelData = sel & frm & whr & grp & ord
            'sqlGetLabelData = "SELECT PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, COUNT(PART_NBR), MATID, MOD_TMSTM FROM PLTFLOOR.ICS_LABEL_DATA_VW WHERE PART_NBR = '" & cmbPartNumber.SelectedItem.Text & "'AND ECL = '" & cmbECL.SelectedValue & "' AND DESTINATION = '" & cmbDestination.SelectedValue & "' AND PACKAGE_CODE = '" & cmbPackageCode.SelectedValue & "' AND MOD_TMSTM >= to_date('" & SearchDate & "  12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM') AND USED_STATUS = 0 GROUP BY PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, MATID, MOD_TMSTM ORDER BY MOD_TMSTM"
            'Response.Write(sqlGetLabelData)
            ' Create a Command object with the SQL statement.
            MyConn.Open()
            Dim objCommandSelect As New OleDb.OleDbDataAdapter(sqlGetLabelData, MyConn)
            objCommandSelect.Fill(MyDataset)
            ' Create a new DataTable object and assign to it
            ' the new table in the Tables collection.
            MyTable = MyDataset.Tables(0)
            If MyTable.Rows.Count < 2 Then
                lblMessage.Visible = True
                lblMessage.Text = "The labels you selected cannot be deleted, you have not ordered any new labels"
            Else

                'lblMessage.Text = sqlUpdateICS_LABEL_DATA
                objCommand.ExecuteNonQuery()
                dgICS_LABEL_DATA.EditItemIndex = -1
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
                Call Populate_dgICS_LABEL_DATA()
                lblMessage.Visible = True
                lblMessage.Text = "The labels you selected were deleted."
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
    Sub Populate_dgICS_LABEL_DATA()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlGetLabelData As String
        Dim SearchDate As String
        Try
            SearchDate = DateAdd(DateInterval.Day, -350, DateTime.Today())
            sel = "SELECT PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, COUNT(SERIAL_NBR), MATID, DELIVERY_LOC_NBR, to_char(MOD_TMSTM,'mm/dd/yyyy')as TimeStamp"
            frm = " FROM PLTFLOOR.ICS_LABEL_DATA_VW"
            whr = " WHERE PART_NBR = '" & cmbPartNumber.SelectedItem.Text & "' AND DESTINATION = '" & cmbDestination.SelectedValue & "' AND ECL = '" & cmbECL.SelectedValue & "' AND PACKAGE_CODE = '" & cmbPackageCode.SelectedValue & "' AND MOD_TMSTM >= to_date('" & SearchDate & "', 'MM/DD/YYYY HH:MI:SS AM') AND USED_STATUS = 0"
            grp = " GROUP BY PART_NBR, ECL, DESTINATION, PACKAGE_CODE, MATID, QUANTITY, DELIVERY_LOC_NBR, to_char(MOD_TMSTM,'mm/dd/yyyy')"
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
                lblSortOrder.Visible = True
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
    Private Sub cmbPartNumber_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPartNumber.SelectedIndexChanged
        txtMesPartId.Text = GetMESPartId(cmbPartNumber.SelectedItem.Text, "")
        Call Populate_cmbDestination()
        Call Populate_cmbECL()
        Call Populate_cmbPackageCode()
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Call Populate_dgICS_LABEL_DATA()
    End Sub
End Class
