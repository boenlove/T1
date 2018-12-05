Public Class BEC_UpdateProductionRun
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblPart As System.Web.UI.WebControls.Label
    Protected WithEvents lblSelectedPartID As System.Web.UI.WebControls.Label
    Protected WithEvents lblPartSelIndex As System.Web.UI.WebControls.Label
    Protected WithEvents lblRqstPart As System.Web.UI.WebControls.Label
    Protected WithEvents btnStdPackAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmbPartNumber As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dgProductionRun As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblDept As System.Web.UI.WebControls.Label
    Protected WithEvents cmbDept As System.Web.UI.WebControls.DropDownList
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
            If Not Page.IsPostBack Then
                Call Populate_cmbPartNumber()
                viewstate("sortField") = "PRODUCTION_RUN_ID"
                viewstate("sortDirection") = "ASC"
                Call Populate_dgProductionRun()
            End If
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If
    End Sub
    Sub Populate_cmbPartNumber()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbPartNumber As String = "SELECT PART_NBR, pr.MES_PART_ID FROM MESDBA.PRODUCTION_RUN pr, MESDBA.MES_PART mp WHERE pr.MES_PART_ID = mp.MES_PART_ID and pr.REPORT_GROUP_ID = " & cmbDept.SelectedValue & " GROUP BY mp.PART_NBR, pr.MES_PART_ID ORDER BY PART_NBR ASC"
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
    Sub dgProductionRun_Edit(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        ' Set the EditItemIndex property to the index of the item clicked 
        ' in the DataGrid control to enable editing for that item. Be sure
        ' to rebind the DateGrid to the data source to refresh the control.
        dgProductionRun.EditItemIndex = e.Item.ItemIndex
        Call Populate_dgProductionRun()
    End Sub
    Sub dgProductionRun_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim txtECL As TextBox = e.Item.Cells(0).Controls(0)
        Dim txtStdPack As TextBox = e.Item.Cells(1).Controls(0)
        Dim txtBoxCnt As TextBox = e.Item.Cells(2).Controls(0)
        Dim txtBroadCastCd As TextBox = e.Item.Cells(3).Controls(0)
        Dim txtLayerCnt As TextBox = e.Item.Cells(4).Controls(0)
        Dim txtPackageCd As TextBox = e.Item.Cells(5).Controls(0)
        Dim sqlUpdateProductionRun As String = "UPDATE MESDBA.PRODUCTION_RUN SET REV_PHYSICAL = '" & txtECL.Text & "', STD_PACK_QTY = " & txtStdPack.Text & ", TOTAL_PART_QTY = " & txtBoxCnt.Text & ", PROCESS_ID = '" & txtBroadCastCd.Text & "', STD_PACK_COMPLETE_QTY = " & txtLayerCnt.Text & ", PACKAGE_CODE = '" & UCase(txtPackageCd.Text) & "' WHERE PRODUCTION_RUN_ID = " & e.Item.Cells(9).Text & " "
        Dim objCommand As New OleDb.OleDbCommand(sqlUpdateProductionRun, MyConn)
        'Response.Write(sqlUpdateProductionRun)
        MyConn.Open()
        objCommand.ExecuteNonQuery()
        dgProductionRun.EditItemIndex = -1
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
        Call Populate_dgProductionRun()
    End Sub
    Sub dgProductionRun_SortCommand(ByVal Source As Object, ByVal E As DataGridSortCommandEventArgs)
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
        Populate_dgProductionRun()
    End Sub
    Sub Populate_dgProductionRun()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlGetProductionRun As String
        sel = "SELECT PRODUCTION_RUN_ID, REV_PHYSICAL, STD_PACK_QTY, TOTAL_PART_QTY, PROCESS_ID, STD_PACK_COMPLETE_QTY, PACKAGE_CODE, CONTAINER_CODE"
        frm = " FROM MESDBA.PRODUCTION_RUN"
        whr = " WHERE MES_PART_ID = '" & cmbPartNumber.SelectedValue & "'"
        ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
        sqlGetProductionRun = sel & frm & whr & ord
        ' Create a Command object with the SQL statement.
        'Response.Write(sqlGetProductionRun)
        Try
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlGetProductionRun, MyConn)
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
                If cmbPartNumber.SelectedValue <> "" Then
                    lblMsg.Text = "No Records were found for Part Number: " & cmbPartNumber.SelectedItem.Text & ""
                    dgProductionRun.Visible = False
                Else
                    lblMsg.Text = "No Records were found for Dept: " & cmbDept.SelectedItem.Text & ""
                    dgProductionRun.Visible = False
                End If
                tblResults.Visible = False
            Else
                dgProductionRun.DataSource = objDataReader
                dgProductionRun.DataBind()
                dgProductionRun.Visible = True
                lblMsg.Visible = False
                tblResults.Visible = True
            End If
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
            End If
        Catch ex As Exception
            lblMsg.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub
    Sub dgProductionRun_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        ' Set the EditItemIndex property to -1 to exit editing mode.
        ' Be sure to rebind the DateGrid to the data source to refresh
        ' the control.
        dgProductionRun.EditItemIndex = -1
        Call Populate_dgProductionRun()
    End Sub
    Sub dgProductionRun_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Try
            Dim sqlDeleteProductionRun As String = "DELETE FROM MESDBA.PRODUCTION_RUN WHERE PRODUCTION_RUN_ID = " & e.Item.Cells(9).Text & " "
            Dim objCommand As New OleDb.OleDbCommand(sqlDeleteProductionRun, MyConn)
            Dim sqlCommit As String = "COMMIT"
            Dim objCommitCommand As New OleDb.OleDbCommand(sqlCommit, MyConn)
            'Response.Write(sqlDeleteProductionRun)
            MyConn.Open()
            objCommand.ExecuteNonQuery()
            dgProductionRun.EditItemIndex = -1
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
            Call Populate_dgProductionRun()
        Catch ex As Exception
            lblMsg.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub
    Private Sub btnStdPackAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStdPackAdd.Click
        Try
            Response.Redirect("BEC_ProductionRun.aspx")
        Catch ex As Exception
            lblMsg.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        End Try
    End Sub

    Private Sub cmbPartNumber_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPartNumber.SelectedIndexChanged
        Call Populate_dgProductionRun()
    End Sub

    Private Sub cmbDept_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDept.SelectedIndexChanged
        Call Populate_cmbPartNumber()
        Call Populate_dgProductionRun()
    End Sub
End Class
