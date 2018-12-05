Imports System.Data
Partial Class BEC_Ship_Destination_All_Users
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
            Call Populate_cmbPartNumber()
            Call Populate_dgSchedule()
        End If
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
