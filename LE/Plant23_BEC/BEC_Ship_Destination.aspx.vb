Imports System.Data
Partial Class BEC_Ship_Destination
    Inherits System.Web.UI.Page
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
        If Not IsPostBack Then
            If x = "LOCAL." Or x = "TEST." Then
                lblDisplay.Visible = True
                lblDisplay.Text = "TEST ENVIRONMENT"
            End If
            'txtSecurityCheck.Text = GetSecurity(UCase(Right(Request.ServerVariables("LOGON_USER"), 6)))
            txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23"
            If txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23" Or txtSecurityCheck.Text = "BEC_ICS_WEB_USER_P23" Then

                Call Populate_cmbPartNumber()
                Call Populate_dgSchedule()
            Else
                Response.Redirect("BEC_Security_Denied.aspx")
            End If
        End If
    End Sub
    Sub dgSchedule_Edit(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        ' Set the EditItemIndex property to the index of the item clicked 
        ' in the DataGrid control to enable editing for that item. Be sure
        ' to rebind the DateGrid to the data source to refresh the control.
        dgSchedule.EditItemIndex = e.Item.ItemIndex
        Call Populate_dgSchedule()
    End Sub
    Sub dgSchedule_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        ' Set the EditItemIndex property to -1 to exit editing mode.
        ' Be sure to rebind the DateGrid to the data source to refresh
        ' the control.
        dgSchedule.EditItemIndex = -1
        Call Populate_dgSchedule()
    End Sub
    Sub dgSchedule_PageChange(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        Dim start As Integer
        start = dgSchedule.CurrentPageIndex * dgSchedule.PageSize
        dgSchedule.CurrentPageIndex = e.NewPageIndex
        Call Populate_dgSchedule()
    End Sub
    Sub dgSchedule_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Try
            Dim sqlDeleteSchedule As String
            If cmbEditChoice.SelectedValue = "Single" Then
                sqlDeleteSchedule = "DELETE FROM MESDBA.ICS_SCHEDULE_POOL WHERE SCHEDULE_ID = " & e.Item.Cells(4).Text & ""
            ElseIf cmbEditChoice.SelectedValue = "Multi" Then
                sqlDeleteSchedule = "DELETE FROM MESDBA.ICS_SCHEDULE_POOL WHERE REVISION_PHYSICAL = " & e.Item.Cells(7).Text & " and CONTAINER_CODE = '" & e.Item.Cells(8).Text & "' and PACKAGE_CODE = '" & e.Item.Cells(9).Text & "' and PKGS_PER_SKID_QTY = " & e.Item.Cells(10).Text & " and MES_PART_ID = " & e.Item.Cells(13).Text & ""
            End If
            Dim objCommand As New OleDb.OleDbCommand(sqlDeleteSchedule, MyConn)
            Dim sqlCommit As String = "COMMIT"
            Dim objCommitCommand As New OleDb.OleDbCommand(sqlCommit, MyConn)

            'Response.Write(sqlDeleteSchedule)
            MyConn.Open()
            objCommand.ExecuteNonQuery()
            dgSchedule.EditItemIndex = -1
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
            Call Populate_dgSchedule()
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

    Sub dgSchedule_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim txtPriority As TextBox = CType(e.Item.Cells(3).Controls(0), TextBox)
        Dim sqlUpdateSchedule As String
        Try
            If cmbEditChoice.SelectedValue = "Single" Then
                sqlUpdateSchedule = "UPDATE MESDBA.ICS_SCHEDULE_POOL SET BUILD_PRIORITY = " & txtPriority.Text & " WHERE SCHEDULE_ID = " & e.Item.Cells(4).Text & " "
            ElseIf cmbEditChoice.SelectedValue = "Multi" Then
                sqlUpdateSchedule = "UPDATE MESDBA.ICS_SCHEDULE_POOL SET BUILD_PRIORITY = " & txtPriority.Text & " WHERE MES_PART_ID = " & cmbPartNumber.SelectedValue & " and REVISION_PHYSICAL = " & e.Item.Cells(7).Text & " and CONTAINER_CODE = '" & e.Item.Cells(8).Text & "' and PACKAGE_CODE = '" & e.Item.Cells(9).Text & "' "
            End If
            Dim objCommand As New OleDb.OleDbCommand(sqlUpdateSchedule, MyConn)
            'Response.Write(sqlUpdateSchedule)
            MyConn.Open()
            objCommand.ExecuteNonQuery()
            dgSchedule.EditItemIndex = -1
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
            End If
            Call Populate_dgSchedule()
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
    Sub Populate_dgSchedule()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlGetSchedule As String
        Try
            If rdoPackageType.SelectedValue = "" Then
                sqlGetSchedule = "SELECT ('BEC_SerialNumberAvailability.aspx?PartNbr=' || PART_NBR || '&ECL=' || REVISION_PHYSICAL || '&PackageCode=' || PACKAGE_CODE || '&Destination=' || CONTAINER_CODE ||'') AS Check_Labels, SCHEDULE_ID, ip.MES_PART_ID, BUILD_PRIORITY, PART_NBR, REVISION_PHYSICAL, CONTAINER_CODE, PACKAGE_CODE, PKGS_PER_SKID_QTY, PKGS_USED_QTY, MACHINE_NAME, MATID, ip.INSERT_USERID FROM MESDBA.MES_PART mp, (MESDBA.ICS_SCHEDULE_POOL ip LEFT JOIN MESDBA.MACHINE m ON ip.MACHINE_ID = m.MACHINE_ID) WHERE ip.MES_PART_ID = mp. MES_PART_ID and mp.MES_PART_ID = '" & cmbPartNumber.SelectedValue & "' ORDER BY BUILD_PRIORITY, SCHEDULE_ID"
            Else
                sqlGetSchedule = "SELECT ('BEC_SerialNumberAvailability.aspx?PartNbr=' || PART_NBR || '&ECL=' || REVISION_PHYSICAL || '&PackageCode=' || PACKAGE_CODE || '&Destination=' || CONTAINER_CODE ||'') AS Check_Labels, SCHEDULE_ID, ip.MES_PART_ID, BUILD_PRIORITY, PART_NBR, REVISION_PHYSICAL, CONTAINER_CODE, PACKAGE_CODE, PKGS_PER_SKID_QTY, PKGS_USED_QTY, MACHINE_NAME, MATID, ip.INSERT_USERID FROM MESDBA.MES_PART mp, (MESDBA.ICS_SCHEDULE_POOL ip LEFT JOIN MESDBA.MACHINE m ON ip.MACHINE_ID = m.MACHINE_ID) WHERE ip.MES_PART_ID = mp. MES_PART_ID and mp.MES_PART_ID = '" & cmbPartNumber.SelectedValue & "' and PACKAGE_CODE = '" & rdoPackageType.SelectedValue & "' ORDER BY BUILD_PRIORITY, SCHEDULE_ID"
            End If
            '
            ' Create a Command object with the SQL statement.
            'Response.Write(sqlGetSchedule)
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlGetSchedule, MyConn)
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
                If rdoPackageType.SelectedValue = "" And cmbPartNumber.SelectedValue <> "" Then
                    lblNoRecordsFound.Text = "No Records were found for Part Number: " & cmbPartNumber.SelectedItem.Text & ""
                ElseIf cmbPartNumber.SelectedValue <> "" Then
                    lblNoRecordsFound.Text = "No Records were found for Part Number: " & cmbPartNumber.SelectedItem.Text & " and Container Type: " & rdoPackageType.SelectedValue & ""
                Else
                    lblNoRecordsFound.Text = "No Records were found."
                End If
                lblNoRecordsFound.Visible = True
                dgSchedule.Visible = False
                tblResults.Visible = False
            Else
                dgSchedule.DataSource = objDataReader
                dgSchedule.DataBind()
                dgSchedule.Visible = True
                lblNoRecordsFound.Visible = False
                tblResults.Visible = True
            End If
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
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
    Sub Populate_cmbPartNumber()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbPartNumber As String = "SELECT PART_NBR, ip.MES_PART_ID FROM MESDBA.ICS_SCHEDULE_POOL ip, MESDBA.MES_PART mp WHERE ip.MES_PART_ID = mp.MES_PART_ID GROUP BY mp.PART_NBR, ip.MES_PART_ID ORDER BY PART_NBR ASC"
        Dim CmdPartNumber As New OleDb.OleDbCommand(sqlCmbPartNumber, MyConn)
        'Response.Write(sqlCmbPartNumber)
        MyConn.Open()
        objDR = CmdPartNumber.ExecuteReader
        cmbPartNumber.DataSource = objDR
        cmbPartNumber.DataValueField = "MES_PART_ID"
        cmbPartNumber.DataTextField = "PART_NBR"
        cmbPartNumber.DataBind()
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
    End Sub
    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        dgSchedule.Visible = True
        Call Populate_dgSchedule()
    End Sub


End Class
