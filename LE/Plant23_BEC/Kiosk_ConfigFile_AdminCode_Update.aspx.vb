Imports System.Xml
Partial Class Kiosk_ConfigFile_AdminCode_Update
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
                If Not Page.IsPostBack Then
                    Dim oDOm As System.xml.XmlDocument = New XmlDocument
                    Populate_cmbPCName()
                    oDOm.Load(cmbPCNames.SelectedValue & "\c$\Program Files\Delphi Packard Electric\BEC_SPL\AppConfig.xml")
                    Dim oNode As XmlNode = oDOm.SelectSingleNode("//Properties/AdminCode")
                    lblAdminCode.Text = oNode.InnerXml
                    Dim oNode2 As XmlNode = oDOm.SelectSingleNode("//Properties/ExceptionsAdminCode")
                    lblExceptionAdminCode.Text = oNode2.InnerXml
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
            Dim oNode As XmlNode = oDOm.SelectSingleNode("//Properties/AdminCode")
            If txtAdminCode.Text = "" Then
                oNode.InnerText = lblAdminCode.Text
                lblMessage1.Text = "XML File Not Updated with AdminCode"
            Else
                oNode.InnerText = txtAdminCode.Text
                lblMessage1.Text = "XML File Updated with AdminCode"
            End If
            Dim oNode2 As XmlNode = oDOm.SelectSingleNode("//Properties/ExceptionsAdminCode")
            If txtExceptionAdminCode.Text = "" Then
                oNode2.InnerText = lblExceptionAdminCode.Text
                lblMessage2.Text = "XML File Not Updated with ExceptionsAdminCode"
            Else
                oNode2.InnerText = txtExceptionAdminCode.Text
                lblMessage2.Text = "XML File Updated with ExceptionsAdminCode"
            End If
            oDOm.Save(cmbPCNames.SelectedValue & "\c$\Program Files\Delphi Packard Electric\BEC_SPL\AppConfig.xml")
            lblAdminCode.Text = oNode.InnerXml
            lblExceptionAdminCode.Text = oNode2.InnerXml
            'Response.Write(cmbPCNames.SelectedValue & "\c$\AppConfig.xml")
        Catch ex As Exception
            'Redirect to Error Page or take other action to handle error.
            lblMessage.Text = "Problem, An Error Has Occurred"
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
        End Try
    End Sub

    Private Sub cmbPCNames_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPCNames.SelectedIndexChanged
        Try
            Dim oDOm As System.xml.XmlDocument = New XmlDocument
            oDOm.Load(cmbPCNames.SelectedValue & "\c$\Program Files\Delphi Packard Electric\BEC_SPL\AppConfig.xml")
            Dim oNode As XmlNode = oDOm.SelectSingleNode("//Properties/AdminCode")
            lblAdminCode.Text = oNode.InnerXml
            Dim oNode2 As XmlNode = oDOm.SelectSingleNode("//Properties/ExceptionsAdminCode")
            lblExceptionAdminCode.Text = oNode2.InnerXml
            lblMessage.Text = "PC Found"
        Catch ex As Exception
            lblMessage.Text = "" & cmbPCNames.SelectedValue & " is not set up for this function"
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        End Try
    End Sub
End Class
