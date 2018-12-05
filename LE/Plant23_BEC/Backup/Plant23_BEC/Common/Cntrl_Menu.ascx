<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Cntrl_Menu.ascx.vb" Inherits="Plant23_BEC.Cntrl_Menu" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!--<cc1:menu id="WebMainMenu" runat="server" BackColor="Gray" BorderStyle="Outset" Cursor="Pointer"
	BorderWidth="1px" Width="256px" Height="16px" Font-Names="Arial" Font-Size="6pt" ForeColor="White">
	<SelectedMenuItemStyle BackColor="LightGray"></SelectedMenuItemStyle>
</cc1:menu>
<P>-->
	<TABLE class="NavMenu" id="tblMenu" cellSpacing="0" cellPadding="0" width="195" border="0">
		<TR>
			<TD class="tblNavTitle">Plant 23 BEC</TD>
		</TR>
		<TR>
			<TD class="NavMenu">
				<cc1:Menu id="WebMainMenu1" Cursor="Pointer" runat="server" CssClass="NavMenu" HighlightTopMenu="True"
					Width="195px" SubMenuCssClass="SubNavMenu" SubMenuCssClass-Length="10" ClickToOpen="False" DefaultMouseOverCssClass-Length="0">
					<UnselectedMenuItemStyle Font-Size="8pt"></UnselectedMenuItemStyle>
					<SelectedMenuItemStyle Font-Size="8pt" BackColor="#B0B0B0"></SelectedMenuItemStyle>
				</cc1:Menu></TD>
		</TR>
	</TABLE>
</P>
