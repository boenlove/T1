Public Class BEC_ProductionRun
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtMesPartId As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Requiredfieldvalidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Comparevalidator1 As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Requiredfieldvalidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Comparevalidator2 As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents Requiredfieldvalidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Requiredfieldvalidator5 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Requiredfieldvalidator6 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Requiredfieldvalidator7 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Comparevalidator3 As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Requiredfieldvalidator8 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Requiredfieldvalidator9 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Requiredfieldvalidator10 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtLayerCnt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBroadCastCd As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBoxCount As System.Web.UI.WebControls.TextBox
    Protected WithEvents rblContainer As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents txtPkgCD As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStdPack As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtECL As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPartNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmbDept As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnAddStdPack As System.Web.UI.WebControls.Button
    Protected WithEvents btnEditStdPack As System.Web.UI.WebControls.Button
    Protected WithEvents dgProductionRun As System.Web.UI.WebControls.DataGrid
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
                viewstate("sortField") = "PRODUCTION_RUN_ID"
                viewstate("sortDirection") = "ASC"
            End If
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If
        tblResults.Visible = False
    End Sub

    Private Sub btnAddStdPack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddStdPack.Click
        Try
            If Len(txtPartNumber.Text) = 8 Then
                txtMesPartId.Text = GetMESPartId(txtPartNumber.Text, "")
                If txtMesPartId.Text <> "" Then
                    Dim insPink_Pass As String
                    Dim sqlPartLineRestriction As String
                    Dim ins, vlu, sql, sqlCheck
                    Dim MyConn As New OleDb.OleDbConnection(strConn)
                    sqlCheck = "SELECT * FROM MESDBA.PRODUCTION_RUN WHERE MES_PART_ID = " & txtMesPartId.Text & " and REV_PHYSICAL = '" & txtECL.Text & "' and STD_PACK_QTY = " & txtStdPack.Text & " and PACKAGE_CODE = '" & UCase(txtPkgCD.Text) & "' and TOTAL_PART_QTY = " & txtBoxCount.Text & " and CONTAINER_CODE = '" & rblContainer.SelectedValue & "' and PROCESS_ID = '" & txtBroadCastCd.Text & "' and STD_PACK_COMPLETE_QTY = " & txtLayerCnt.Text & " and REPORT_GROUP_ID = " & cmbDept.SelectedValue & " and SYSTEM_FLAG = 'I'"
                    ins = "INSERT INTO MESDBA.PRODUCTION_RUN (MES_PART_ID, REV_PHYSICAL, STD_PACK_QTY, PACKAGE_CODE, CONTAINER_CODE, TOTAL_PART_QTY, PROCESS_ID, STD_PACK_COMPLETE_QTY, REPORT_GROUP_ID, SYSTEM_FLAG) "
                    vlu = " VALUES (" & txtMesPartId.Text & ", '" & txtECL.Text & "', " & txtStdPack.Text & ", '" & UCase(txtPkgCD.Text) & "', '" & rblContainer.SelectedValue & "', " & txtBoxCount.Text & ", '" & txtBroadCastCd.Text & "', " & txtLayerCnt.Text & ", " & cmbDept.SelectedValue & ", 'I')"
                    sql = ins & vlu
                    'Response.Write(sqlCheck)
                    'Response.Write(sql)
                    ' Create a Command object with the SQL statement.
                    MyConn.Open()
                    Dim objCommand As New OleDb.OleDbCommand(sqlCheck, MyConn)
                    Dim objDataReader As OleDb.OleDbDataReader
                    objDataReader = objCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                    If objDataReader.HasRows = False Then
                        If MyConn.State = ConnectionState.Open Then
                            MyConn.Close()
                        End If
                        Dim CmdInsert_Production_Run As New OleDb.OleDbCommand(sql, MyConn)
                        Dim CmdCommit As New OleDb.OleDbCommand("commit", MyConn)
                        CmdInsert_Production_Run.Connection.Open()
                        CmdInsert_Production_Run.ExecuteNonQuery()
                        'Response.Write(sql)
                        CmdCommit.ExecuteNonQuery()
                        If CmdInsert_Production_Run.Connection.State = ConnectionState.Open Then
                            CmdInsert_Production_Run.Connection.Close()
                        End If
                        If CmdCommit.Connection.State = ConnectionState.Open Then
                            CmdCommit.Connection.Close()
                        End If
                        Call Populate_dgProductionRun()
                        lblMsg.Text = "The record was inserted..."
                        lblMsg.Visible = True
                    Else
                        Call Populate_dgProductionRun()
                        lblMsg.Text = "That record already exists. Try again..."
                        lblMsg.Visible = True

                    End If
                Else
                    lblMsg.Text = "Part Number is not in Oracle Table.  Please check your Part Number and contact Angela Bridges @ 601-925-2754"
                    lblMsg.Visible = True
                End If

            Else
                lblMsg.Text = "Please input an 8 digit Part Number."
                lblMsg.Visible = True
            End If
        Catch ex As Exception
            lblMsg.Text = "The following error has occurred: " & ex.Message
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        End Try

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
        Dim sqlUpdateProductionRun As String = "UPDATE MESDBA.PRODUCTION_RUN SET REV_PHYSICAL = '" & txtECL.Text & "', STD_PACK_QTY = " & txtStdPack.Text & ", TOTAL_PART_QTY = " & txtBoxCnt.Text & ", PROCESS_ID = '" & txtBroadCastCd.Text & "', STD_PACK_COMPLETE_QTY = " & txtLayerCnt.Text & ", PACKAGE_CODE = '" & txtPackageCd.Text & "' WHERE PRODUCTION_RUN_ID = " & e.Item.Cells(8).Text & " "
        Dim objCommand As New OleDb.OleDbCommand(sqlUpdateProductionRun, MyConn)
        Response.Write(sqlUpdateProductionRun)
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
        Try
            sel = "SELECT PRODUCTION_RUN_ID, REV_PHYSICAL, STD_PACK_QTY, TOTAL_PART_QTY, PROCESS_ID, STD_PACK_COMPLETE_QTY, PACKAGE_CODE"
            frm = " FROM MESDBA.PRODUCTION_RUN"
            whr = " WHERE MES_PART_ID = '" & txtMesPartId.Text & "'"
            ord = " ORDER BY " & viewstate("sortField") & " " & viewstate("sortDirection") & ""
            sqlGetProductionRun = sel & frm & whr & ord
            ' Create a Command object with the SQL statement.
            'Response.Write(sqlGetProductionRun)
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
                lblMsg.Text = "No Records were found for Part Number:" & txtPartNumber.Text & ""
                lblMsg.Visible = True
                dgProductionRun.Visible = False
                tblResults.Visible = False
            Else
                dgProductionRun.DataSource = objDataReader
                dgProductionRun.DataBind()
                dgProductionRun.Visible = True
                tblResults.Visible = True
                lblMsg.Visible = False
            End If
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
            End If
        Catch ex As Exception
            lblMsg.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message
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
            Dim sqlDeleteProductionRun As String = "DELETE FROM MESDBA.PRODUCTION_RUN WHERE PRODUCTION_RUN_ID = " & e.Item.Cells(8).Text & " "
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
            lblMsg.Text = "The following error has occurred: " & ex.Message
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
        End Try
    End Sub
    Private Sub btnEditStdPack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditStdPack.Click
        Try
            Response.Redirect("BEC_UpdateProductionRun.aspx?DEPT=" & cmbDept.SelectedValue)
        Catch ex As Exception
            lblMsg.Text = "The following error has occurred: " & ex.Message
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        End Try
    End Sub

End Class
