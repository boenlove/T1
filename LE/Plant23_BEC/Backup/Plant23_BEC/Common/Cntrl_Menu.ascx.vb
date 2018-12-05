Public Class Cntrl_Menu
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents tblLayout As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents WebMainMenu As skmMenu.Menu
    Protected WithEvents WebMainMenu1 As skmMenu.Menu
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
        WebMainMenu1.DataSource = Server.MapPath("Menu_Items.xml")
        WebMainMenu1.DataBind()

    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Me.WebMainMenu1.Dispose()
    End Sub

    Private Sub WebMainMenu1_MenuItemClick(ByVal sender As System.Object, ByVal e As skmMenu.MenuItemClickEventArgs) Handles WebMainMenu1.MenuItemClick

    End Sub
End Class


