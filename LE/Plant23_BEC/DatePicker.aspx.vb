Partial Class DatePicker
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Calendar1 As System.Web.UI.WebControls.Calendar

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
        'Put user code to initialize the page here
    End Sub
    Protected Sub Change_Date(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not Request.QueryString("textbox") Is Nothing Then
            Dim strScript As String = "<script>window.opener.document.forms(0)." + Request.QueryString("textbox").ToString + ".value = '"
            strScript += CalPopup.SelectedDate.ToShortDateString
            strScript += "';self.close()"
            strScript += "</" + "script>"
            RegisterClientScriptBlock("Calendar_ChangeDate", strScript)
        End If
    End Sub


End Class
