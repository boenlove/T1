<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics.WebUI.UltraWebNavigator.v2" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="MenuCntrl.ascx.vb" Inherits="Plant23_BEC.MenuCntrl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE class="tblLogo" id="tblLogo" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<TR>
		<TD vAlign="middle" bgColor="#004c7a" height="35"><IMG src="../Images/P19_Logo.jpg"></TD>
		<TD vAlign="top" align="right" bgColor="#004c7a" height="35">
			<asp:LinkButton id="lbtnManufacturing" runat="server" CausesValidation="False">NA Manufacturing</asp:LinkButton>&nbsp;&nbsp;|&nbsp;&nbsp;
			<asp:LinkButton id="lbtnHelp" runat="server" CausesValidation="False">Help</asp:LinkButton>&nbsp;</TD>
	</TR>
	<TR>
		<TD bgColor="#004c7a" colSpan="2" height="7" style="HEIGHT: 7px">
			<ignav:UltraWebMenu id="UltraWebMenu2" runat="server" ImageDirectory="/Tools/Menus/Infragistics/ig_Images/"
				JavaScriptFilename="/Tools/Menus/Infragistics/ig_scripts/ig_webmenu2.js" DefaultIslandClass="IslandClass"
				SeparatorClass="SeparatorClass" DisabledClass="DisabledClass" HoverClass="HoverClass" FileUrl=" "
				Width="576px">
				<ExpandEffects ShadowColor="LightGray"></ExpandEffects>
				<Styles>
					<ignav:Style Cursor="hand" BorderWidth="1px" Font-Size="8pt" Font-Names="MS Sans Serif" BorderColor="#004C7A"
						ForeColor="White" BackColor="#004C7A" CssClass="IslandClass"></ignav:Style>
					<ignav:Style Cursor="default" ForeColor="White" CssClass="HoverClass"></ignav:Style>
					<ignav:Style Cursor="default" BorderWidth="1px" BorderStyle="Outset" CssClass="TopHover"></ignav:Style>
					<ignav:Style BorderWidth="1px" BorderColor="#004C7A" BorderStyle="Solid" CssClass="TopClass"></ignav:Style>
					<ignav:Style BackgroundImage="ig_menuSep.gif" CssClass="SeparatorClass" CustomRules="background-repeat:repeat-x; "></ignav:Style>
					<ignav:Style ForeColor="LightGray" CssClass="DisabledClass"></ignav:Style>
				</Styles>
				<Items>
					<ignav:Item TargetUrl="../index.aspx" Text="Home"></ignav:Item>
					<ignav:Item Text="Kiosk Database Manager">
						<Items>
							<ignav:Item TargetUrl="../KioskDBMgr/AddStdPack.aspx" HoverImageUrl="../Images/Arrow_Yellow.gif"
								ImageUrl="../Images/Arrow_Gray.gif" Text="Add New Entry"></ignav:Item>
							<ignav:Item TargetUrl="../KioskDBMgr/UpdateStdPack.aspx" HoverImageUrl="../Images/Arrow_Yellow.gif"
								ImageUrl="../Images/Arrow_Gray.gif" Text="View/Perform Maintenance"></ignav:Item>
						</Items>
						<Style>

<Padding Top="3px" Right="10px">
</Padding>

						</Style>
					</ignav:Item>
					<ignav:Item Text="MTMS Location Maintenance">
						<Items>
							<ignav:Item TargetUrl="../LocationMaint/AddLoc.aspx" HoverImageUrl="../Images/Arrow_Yellow.gif"
								ImageUrl="../Images/Arrow_Gray.gif" Text="Add New Entry"></ignav:Item>
							<ignav:Item TargetUrl="../LocationMaint/View_Edit.aspx" HoverImageUrl="../Images/Arrow_Yellow.gif"
								ImageUrl="../Images/Arrow_Gray.gif" Text="View/Perform Maintenance"></ignav:Item>
						</Items>
					</ignav:Item>
					<ignav:Item TargetUrl="../LotTraceability/Search.aspx" Text="Quality Control Lot Traceability"></ignav:Item>
				</Items>
				<Levels>
					<ignav:Level LevelHoverClass="TopHover" LevelClass="TopClass" Index="0"></ignav:Level>
					<ignav:Level Index="1"></ignav:Level>
				</Levels>
			</ignav:UltraWebMenu></TD>
	</TR>
	<TR>
		<TD background="../Images/Bar.jpg" colSpan="2" height="20">&nbsp;&nbsp;
		</TD>
	</TR>
</TABLE>
