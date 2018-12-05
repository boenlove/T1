Partial Class BEC_Data_Loader

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
        If x = "LOCAL." Or x = "TEST." Then
            lblDisplay.Visible = True
            lblDisplay.Text = "TEST ENVIRONMENT"
        End If
        'txtSecurityCheck.Text = GetSecurity(UCase(Right(Request.ServerVariables("LOGON_USER"), 6)))
        txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23"
        If txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23" Or txtSecurityCheck.Text = "BEC_ICS_WEB_USER_P23" Then
            If Not Page.IsPostBack Then
                '
            End If
            Select Case cmbPackage.SelectedValue
                Case "Y"
                    txtPkgs_Per_Skid_Qty.Text = 1
                Case "Z"
                    'txtPkgs_Per_Skid_Qty.Text = 12
                    cmbPkgSize.Visible = True
                Case "4C"
                    txtPkgs_Per_Skid_Qty.Text = 4
                Case "C"
                    txtPkgs_Per_Skid_Qty.Text = 12
                Case "BU"
                    'txtPkgs_Per_Skid_Qty.Text = 12
                    txtPkgs_Per_Skid_Qty.Text = 1
                Case "UC"
                    'txtPkgs_Per_Skid_Qty.Text = 12
                    txtPkgs_Per_Skid_Qty.Text = 8
                Case "LE" 'jerry 11012018 for LE
                    txtPkgs_Per_Skid_Qty.Text = 1
            End Select
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If
    End Sub
    Private Sub btnLoadData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadData.Click
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim ins, vlu, sql
        Dim MyConn2 As New OleDb.OleDbConnection(strConn)
        Dim sqlGetLabelData As String
        Dim SearchDate As String
        Try
            If txtPkgs_Per_Skid_Qty.Text = "0" Then
                lblMessage.Visible = True
                lblMessage.Text = " Error: Invalid Packages per Skid!!!"
                Exit Sub
            End If
            If Len(txtPartNumber.Text) = 8 Then
                txtMesPartId.Text = GetMESPartId(txtPartNumber.Text, "")
                SearchDate = DateAdd(DateInterval.Day, -350, DateTime.Today())
                sel = "SELECT PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, COUNT(SERIAL_NBR), DELIVERY_LOC_NBR, to_char(MOD_TMSTM,'mm/dd/yyyy')as TimeStamp"
                frm = " FROM PLTFLOOR.ICS_LABEL_DATA_VW"
                whr = " WHERE PART_NBR = '" & txtPartNumber.Text & "' AND DESTINATION = '" & txtContainer.Text & "' AND ECL = '" & txtECL.Text & "' AND PACKAGE_CODE = '" & cmbPackage.SelectedValue & "' AND MOD_TMSTM >= to_date('" & SearchDate & "', 'MM/DD/YYYY HH:MI:SS AM') AND USED_STATUS = 0"
                grp = " GROUP BY PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, DELIVERY_LOC_NBR, to_char(MOD_TMSTM,'mm/dd/yyyy')"
                ord = " ORDER BY PART_NBR"
                sqlGetLabelData = sel & frm & whr & grp & ord
                'sqlGetLabelData = "SELECT PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, COUNT(PART_NBR), MATID, MOD_TMSTM FROM PLTFLOOR.ICS_LABEL_DATA_VW WHERE PART_NBR = '" & cmbPartNumber.SelectedItem.Text & "'AND ECL = '" & cmbECL.SelectedValue & "' AND DESTINATION = '" & cmbDestination.SelectedValue & "' AND PACKAGE_CODE = '" & cmbPackageCode.SelectedValue & "' AND MOD_TMSTM >= to_date('" & SearchDate & "  12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM') AND USED_STATUS = 0 GROUP BY PART_NBR, ECL, DESTINATION, PACKAGE_CODE, QUANTITY, MATID, MOD_TMSTM ORDER BY MOD_TMSTM"
                'Response.Write(sqlGetLabelData)
                ' Create a Command object with the SQL statement.
                MyConn.Open()
                Dim objCommand As New OleDb.OleDbCommand(sqlGetLabelData, MyConn)
                Dim objDataReader As OleDb.OleDbDataReader
                objDataReader = objCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                If objDataReader.HasRows = True Then
                    If txtMatId.Text <> "" Then
                        If txtContainer.Text <> "" Then
                            ins = "INSERT INTO MESDBA.ICS_SCHEDULE_POOL (MES_PART_ID, REVISION_PHYSICAL, BUILD_PRIORITY, CONTAINER_CODE, PACKAGE_CODE, PKGS_PER_SKID_QTY, MATID, INSERT_USERID) "
                            vlu = "VALUES (" & txtMesPartId.Text & ", '" & txtECL.Text & "', " & txtPriority.Text & " , '" & UCase(txtContainer.Text) & "' , '" & cmbPackage.SelectedValue & "', " & txtPkgs_Per_Skid_Qty.Text & ", '" & txtMatId.Text & "', '" & UCase(Right(Request.ServerVariables("LOGON_USER"), 6)) & "')"
                            sql = ins & vlu
                        Else
                            ins = "INSERT INTO MESDBA.ICS_SCHEDULE_POOL (MES_PART_ID, REVISION_PHYSICAL, BUILD_PRIORITY, PACKAGE_CODE, PKGS_PER_SKID_QTY, MATID, INSERT_USERID) "
                            vlu = "VALUES (" & txtMesPartId.Text & ", '" & txtECL.Text & "', " & txtPriority.Text & " , '" & cmbPackage.SelectedValue & "', " & txtPkgs_Per_Skid_Qty.Text & ", '" & txtMatId.Text & "', '" & UCase(Right(Request.ServerVariables("LOGON_USER"), 6)) & "')"
                            sql = ins & vlu
                        End If
                    Else
                        If txtContainer.Text <> "" Then
                            ins = "INSERT INTO MESDBA.ICS_SCHEDULE_POOL (MES_PART_ID, REVISION_PHYSICAL, BUILD_PRIORITY, CONTAINER_CODE, PACKAGE_CODE, PKGS_PER_SKID_QTY, INSERT_USERID) "
                            vlu = "VALUES (" & txtMesPartId.Text & ", '" & txtECL.Text & "', " & txtPriority.Text & " , '" & UCase(txtContainer.Text) & "' , '" & cmbPackage.SelectedValue & "', " & txtPkgs_Per_Skid_Qty.Text & ", '" & UCase(Right(Request.ServerVariables("LOGON_USER"), 6)) & "')"
                            sql = ins & vlu
                        Else
                            ins = "INSERT INTO MESDBA.ICS_SCHEDULE_POOL (MES_PART_ID, REVISION_PHYSICAL, BUILD_PRIORITY, PACKAGE_CODE, PKGS_PER_SKID_QTY, INSERT_USERID) "
                            vlu = "VALUES (" & txtMesPartId.Text & ", '" & txtECL.Text & "', " & txtPriority.Text & " , '" & cmbPackage.SelectedValue & "', " & txtPkgs_Per_Skid_Qty.Text & ", '" & UCase(Right(Request.ServerVariables("LOGON_USER"), 6)) & "')"
                            sql = ins & vlu
                        End If
                    End If
                    'Response.Write(sql)
                    Dim CmdInsert_ICS_Schedule_Pool As New OleDb.OleDbCommand(sql, MyConn2)
                    Dim CmdCommit As New OleDb.OleDbCommand("commit", MyConn2)
                    Dim j As Integer = 0
                    CmdInsert_ICS_Schedule_Pool.Connection.Open()
                    Do While j < txtScheduleEntries.Text
                        CmdInsert_ICS_Schedule_Pool.ExecuteNonQuery()
                        'Response.Write(sql)
                        j = j + 1
                    Loop
                    CmdCommit.ExecuteNonQuery()
                    If CmdInsert_ICS_Schedule_Pool.Connection.State = ConnectionState.Open Then
                        CmdInsert_ICS_Schedule_Pool.Dispose()
                        MyConn2.Dispose()
                    End If
                    If CmdCommit.Connection.State = ConnectionState.Open Then
                        CmdCommit.Dispose()
                        MyConn2.Dispose()
                    End If
                    lblRecordsInserted.Text = "" & j & " rows were inserted..."
                    lblRecordsInserted.Visible = True
                Else
                    lblMessage.Visible = True
                    lblMessage.Text = "No MATID exists for this Part Number, to Load Data Anyway Click the button below. Please remember that there could be serious Labeling Errors if the MATID is not loaded for this Part Number!!!"
                    btnLoadDataAnyway.Visible = True
                    btnLoadData.Visible = False
                    btnWhatsSetup.Visible = False
                End If
            Else
                lblRecordsInserted.Text = "Please input an 8 digit Part Number."
                lblRecordsInserted.Visible = True
            End If
        Catch ex As Exception
            lblMessage.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
            MyConn2.Dispose()
        End Try


    End Sub
    Private Sub cmbPackage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPackage.SelectedIndexChanged
        Select Case cmbPackage.SelectedValue
            Case "Y"
                txtPkgs_Per_Skid_Qty.Text = 1
                cmbPkgSize.Visible = False
            Case "Z"
                'txtPkgs_Per_Skid_Qty.Text = 12
                cmbPkgSize.Visible = True
            Case "4C"
                txtPkgs_Per_Skid_Qty.Text = 4
                cmbPkgSize.Visible = False
            Case "C"
                txtPkgs_Per_Skid_Qty.Text = 12
                cmbPkgSize.Visible = False
            Case "BU"
                txtPkgs_Per_Skid_Qty.Text = 1
                cmbPkgSize.Visible = False
            Case "UC"
                txtPkgs_Per_Skid_Qty.Text = 8
                cmbPkgSize.Visible = False
            Case "LE" 'jerry 11012018 for LE
                txtPkgs_Per_Skid_Qty.Text = 1
                cmbPkgSize.Visible = False

        End Select
        lblPkgQty.Text = txtPkgs_Per_Skid_Qty.Text
    End Sub
    'Private Sub txtPartNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPartNumber.TextChanged
    '   txtMesPartId.Text = GetMESPartId(txtPartNumber.Text, "")
    'End Sub
    Private Sub btnWhatsSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWhatsSetup.Click
        Response.Redirect("BEC_Setup.aspx")
    End Sub

    Private Sub btnLoadDataAnyway_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadDataAnyway.Click
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim ins, vlu, sql
        Try
            If Len(txtPartNumber.Text) = 8 Then
                txtMesPartId.Text = GetMESPartId(txtPartNumber.Text, "")
                If txtMesPartId.Text <> "" Then
                    If txtMatId.Text <> "" Then
                        If txtContainer.Text <> "" Then
                            ins = "INSERT INTO MESDBA.ICS_SCHEDULE_POOL (MES_PART_ID, REVISION_PHYSICAL, BUILD_PRIORITY, CONTAINER_CODE, PACKAGE_CODE, PKGS_PER_SKID_QTY, MATID, INSERT_USERID) "
                            vlu = "VALUES (" & txtMesPartId.Text & ", '" & txtECL.Text & "', " & txtPriority.Text & " , '" & UCase(txtContainer.Text) & "' , '" & cmbPackage.SelectedValue & "', " & txtPkgs_Per_Skid_Qty.Text & ", '" & txtMatId.Text & "', '" & UCase(Right(Request.ServerVariables("LOGON_USER"), 6)) & "')"
                            sql = ins & vlu
                        Else
                            ins = "INSERT INTO MESDBA.ICS_SCHEDULE_POOL (MES_PART_ID, REVISION_PHYSICAL, BUILD_PRIORITY, PACKAGE_CODE, PKGS_PER_SKID_QTY, MATID, INSERT_USERID) "
                            vlu = "VALUES (" & txtMesPartId.Text & ", '" & txtECL.Text & "', " & txtPriority.Text & " , '" & cmbPackage.SelectedValue & "', " & txtPkgs_Per_Skid_Qty.Text & ", '" & txtMatId.Text & "', '" & UCase(Right(Request.ServerVariables("LOGON_USER"), 6)) & "')"
                            sql = ins & vlu
                        End If
                    Else
                        If txtContainer.Text <> "" Then
                            ins = "INSERT INTO MESDBA.ICS_SCHEDULE_POOL (MES_PART_ID, REVISION_PHYSICAL, BUILD_PRIORITY, CONTAINER_CODE, PACKAGE_CODE, PKGS_PER_SKID_QTY, INSERT_USERID) "
                            vlu = "VALUES (" & txtMesPartId.Text & ", '" & txtECL.Text & "', " & txtPriority.Text & " , '" & UCase(txtContainer.Text) & "' , '" & cmbPackage.SelectedValue & "', " & txtPkgs_Per_Skid_Qty.Text & ", '" & UCase(Right(Request.ServerVariables("LOGON_USER"), 6)) & "')"
                            sql = ins & vlu
                        Else
                            ins = "INSERT INTO MESDBA.ICS_SCHEDULE_POOL (MES_PART_ID, REVISION_PHYSICAL, BUILD_PRIORITY, PACKAGE_CODE, PKGS_PER_SKID_QTY, INSERT_USERID) "
                            vlu = "VALUES (" & txtMesPartId.Text & ", '" & txtECL.Text & "', " & txtPriority.Text & " , '" & cmbPackage.SelectedValue & "', " & txtPkgs_Per_Skid_Qty.Text & ", '" & UCase(Right(Request.ServerVariables("LOGON_USER"), 6)) & "')"
                            sql = ins & vlu
                        End If
                    End If
                    'Response.Write(sql)
                    Dim CmdInsert_ICS_Schedule_Pool As New OleDb.OleDbCommand(sql, MyConn)
                    Dim CmdCommit As New OleDb.OleDbCommand("commit", MyConn)
                    Dim j As Integer = 0
                    CmdInsert_ICS_Schedule_Pool.Connection.Open()
                    Do While j < txtScheduleEntries.Text
                        CmdInsert_ICS_Schedule_Pool.ExecuteNonQuery()
                        'Response.Write(sql)
                        j = j + 1
                    Loop
                    CmdCommit.ExecuteNonQuery()
                    If CmdInsert_ICS_Schedule_Pool.Connection.State = ConnectionState.Open Then
                        CmdInsert_ICS_Schedule_Pool.Dispose()
                        MyConn.Dispose()
                    End If
                    If CmdCommit.Connection.State = ConnectionState.Open Then
                        CmdCommit.Dispose()
                        MyConn.Dispose()
                    End If
                    lblRecordsInserted.Text = "" & j & " rows were inserted..."
                    lblRecordsInserted.Visible = True
                    btnWhatsSetup.Visible = True
                    btnLoadData.Visible = True
                    btnLoadDataAnyway.Visible = False
                    lblMessage.Visible = False

                Else
                    lblMessage.Text = "Part Number is not in Oracle Table.  Please check your Part Number and contact Angela Bridges @ 601-925-2754"
                    lblMessage.Visible = True
                    lblRecordsInserted.Text = "Records not inserted."
                    lblRecordsInserted.Visible = True
                End If
            Else
                lblMessage.Text = "Please input an 8 digit Part Number."
                lblRecordsInserted.Text = "Records not inserted."
                lblRecordsInserted.Visible = True
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

    Private Sub cmbPkgSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPkgSize.SelectedIndexChanged
        Select Case cmbPkgSize.SelectedValue
            Case "0"
                txtPkgs_Per_Skid_Qty.Text = 0
            Case "1"
                txtPkgs_Per_Skid_Qty.Text = 1
            Case "2"
                txtPkgs_Per_Skid_Qty.Text = 2
            Case "3"
                txtPkgs_Per_Skid_Qty.Text = 3
            Case "4"
                txtPkgs_Per_Skid_Qty.Text = 4
            Case "5"
                txtPkgs_Per_Skid_Qty.Text = 5
            Case "6"
                txtPkgs_Per_Skid_Qty.Text = 6
            Case "7"
                txtPkgs_Per_Skid_Qty.Text = 7
            Case "8"
                txtPkgs_Per_Skid_Qty.Text = 8
            Case "9"
                txtPkgs_Per_Skid_Qty.Text = 9
            Case "10"
                txtPkgs_Per_Skid_Qty.Text = 10
            Case "11"
                txtPkgs_Per_Skid_Qty.Text = 11
            Case "12"
                txtPkgs_Per_Skid_Qty.Text = 12

        End Select
        lblPkgQty.Text = txtPkgs_Per_Skid_Qty.Text
    End Sub
End Class





