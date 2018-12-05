Public Class MenuCntrl
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbtnDelphip As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnManufacturing As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnHelp As System.Web.UI.WebControls.LinkButton
    Protected WithEvents UltraWebMenu2 As Infragistics.WebUI.UltraWebNavigator.UltraWebMenu

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

    End Sub

    Private Sub lbtnManufacturing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnManufacturing.Click
        Response.Redirect("http://vader.ped.gmeds.com/mfg/index.htm")
    End Sub

    Private Sub lbtnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnHelp.Click
        Dim jsHelp As String

        jsHelp = "<script>window.showHelp('../Help/index.htm');</script>"
        Response.Write(jsHelp)
    End Sub
End Class
