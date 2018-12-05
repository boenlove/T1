Imports System.Xml
Partial Class Kiosk_ConfigFile_Update
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
        If txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23" Then
            Try
                Call Populate_cmbMachineName()
                If Not Page.IsPostBack Then
                    Dim oDOm As System.xml.XmlDocument = New XmlDocument
                    Populate_cmbPCName()
                    oDOm.Load(cmbPCNames.SelectedValue & "\c$\Program Files\Delphi Packard Electric\BEC_SPL\AppConfig.xml")
                    Dim oNode As XmlNode = oDOm.SelectSingleNode("//Properties/MachineName")
                    lblMachineName.Text = oNode.InnerXml
                    Dim oNode2 As XmlNode = oDOm.SelectSingleNode("//Properties/Plant")
                    lblPlant.Text = oNode2.InnerXml
                    Dim oNode3 As XmlNode = oDOm.SelectSingleNode("//Properties/LineNumber")
                    lblLineNumber.Text = oNode3.InnerXml
                    Dim oNode4 As XmlNode = oDOm.SelectSingleNode("//Properties/Department")
                    lblDepartment.Text = oNode4.InnerXml
                End If
            Catch ex As Exception
                lblMessage.Text = "" & cmbPCNames.SelectedValue & " is not set up for this function"
                If IsLoggingOn() Then
                    LogError(ex.Source, ex.Message)
                End If
                LogEvent(ex.Message)
            End Try
        Else
            Response.Redirect("BEC_Security_Denied.aspx")
        End If
    End Sub
    Sub Populate_cmbPCName()
        Try
            Dim oDOm As System.xml.XmlDocument = New XmlDocument
            oDOm.Load("C:\BEC_Kiosk\PCNames.xml")
            Dim oNode As XmlNode
            Dim oNodeList As XmlNodeList = oDOm.GetElementsByTagName("PCNAME")
            Dim strResults
            For Each oNode In oNodeList
                cmbPCNames.Items.Add(oNode.FirstChild().Value)
                cmbPCNames.DataBind()
            Next

        Catch ex As Exception
            lblMessage.Text = "" & cmbPCNames.SelectedValue & " is not set up for this function"
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        End Try

    End Sub
    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            Dim oDOm As System.xml.XmlDocument = New XmlDocument
            oDOm.Load(cmbPCNames.SelectedValue & "\c$\Program Files\Delphi Packard Electric\BEC_SPL\AppConfig.xml")
            Dim oNode As XmlNode = oDOm.SelectSingleNode("//Properties/MachineName")
            oNode.InnerText = cmbMachineName.SelectedValue
            Dim oNode2 As XmlNode = oDOm.SelectSingleNode("//Properties/Plant")
            oNode2.InnerText = txtPlant.Text
            Dim oNode3 As XmlNode = oDOm.SelectSingleNode("//Properties/LineNumber")
            oNode3.InnerText = txtLineNumber.Text
            Dim oNode4 As XmlNode = oDOm.SelectSingleNode("//Properties/Department")
            oNode4.InnerText = cmbDept.SelectedValue
            oDOm.Save(cmbPCNames.SelectedValue & "\c$\Program Files\Delphi Packard Electric\BEC_SPL\AppConfig.xml")
            lblMessage.Text = "XML File Updated"
            lblMachineName.Text = oNode.InnerXml
            lblPlant.Text = oNode2.InnerXml
            lblLineNumber.Text = oNode3.InnerXml
            lblDepartment.Text = oNode4.InnerXml
            'Response.Write(cmbPCNames.SelectedValue & "\c$\AppConfig.xml")
        Catch ex As Exception
            'Redirect to Error Page or take other action to handle error.
            lblMessage.Text = "XML File Not Updated"
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
        End Try
    End Sub
    Sub Populate_cmbMachineName()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim objDR As OleDb.OleDbDataReader
        Dim sqlCmbMachineName As String = "SELECT MACHINE_NAME, MACHINE_ID FROM MESDBA.MACHINE WHERE REPORT_GROUP_ID = '" & cmbDept.SelectedValue & "' GROUP BY MACHINE_NAME, MACHINE_ID"
        'Dept 2341 - 1125 - Underhood
        'Dept 2342 - 1126 - Mid
        'Dept 2343 - 1127 - Left
        Dim CmdMachineName As New OleDb.OleDbCommand(sqlCmbMachineName, MyConn)
        MyConn.Open()
        objDR = CmdMachineName.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        cmbMachineName.DataSource = objDR
        cmbMachineName.DataValueField = "MACHINE_NAME"
        cmbMachineName.DataTextField = "MACHINE_NAME"
        cmbMachineName.DataBind()
        If MyConn.State = ConnectionState.Open Then
            MyConn.Close()
        End If
    End Sub

    Private Sub cmbPCNames_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPCNames.SelectedIndexChanged
        Try
            Dim oDOm As System.xml.XmlDocument = New XmlDocument
            oDOm.Load(cmbPCNames.SelectedValue & "\c$\Program Files\Delphi Packard Electric\BEC_SPL\AppConfig.xml")
            Dim oNode As XmlNode = oDOm.SelectSingleNode("//Properties/MachineName")
            lblMachineName.Text = oNode.InnerXml
            Dim oNode2 As XmlNode = oDOm.SelectSingleNode("//Properties/Plant")
            lblPlant.Text = oNode2.InnerXml
            Dim oNode3 As XmlNode = oDOm.SelectSingleNode("//Properties/LineNumber")
            lblLineNumber.Text = oNode3.InnerXml
            Dim oNode4 As XmlNode = oDOm.SelectSingleNode("//Properties/Department")
            lblDepartment.Text = oNode4.InnerXml
            lblMessage.Text = "PC Found"
        Catch ex As Exception
            lblMessage.Text = "" & cmbPCNames.SelectedValue & " is not set up for this function"
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        End Try
    End Sub

    Private Sub cmbDept_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDept.SelectedIndexChanged
        Call Populate_cmbMachineName()
    End Sub
End Class
