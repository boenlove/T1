<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Kiosk_ConfigFile_Update.aspx.vb" Inherits="Plant23_BEC.Kiosk_ConfigFile_Update"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Kiosk_ConfigFile_AdminCode_Update</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P align="center"><FONT face="Arial"><STRONG>AppConfig File Change Page</STRONG></FONT></P>
			<P align="center">
				<asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
			<P align="center">
				<asp:hyperlink id="hpkReturntoMain" runat="server" NavigateUrl="Index.aspx" Font-Bold="True">Return to Main</asp:hyperlink></P>
			<P align="left"><STRONG><FONT face="Arial">
						<TABLE class="tblFormat" id="tbleAdminCodeChange" style="WIDTH: 584px; HEIGHT: 171px" cellSpacing="1"
							cellPadding="1" width="584" border="1">
							<TR>
								<TD style="WIDTH: 130px; HEIGHT: 20px">
									<P align="right">PC Name:</P>
								</TD>
								<TD style="HEIGHT: 20px"><asp:dropdownlist id="cmbPCNames" runat="server" Width="175px" AutoPostBack="True" Font-Size="8pt"
										Font-Names="Verdana"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 130px; HEIGHT: 21px">
									<P align="right">Old Machine Name:</P>
								</TD>
								<TD style="HEIGHT: 21px"><asp:label id="lblMachineName" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 130px; HEIGHT: 1px">
									<P align="right">Old Plant:</P>
								</TD>
								<TD style="HEIGHT: 1px"><asp:label id="lblPlant" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 130px; HEIGHT: 1px">
									<P align="right">Old Line Number:</P>
								</TD>
								<TD style="HEIGHT: 1px"><asp:label id="lblLineNumber" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 130px; HEIGHT: 16px">
									<P align="right">Old Department:</P>
								</TD>
								<TD style="HEIGHT: 16px"><asp:label id="lblDepartment" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 130px; HEIGHT: 23px">
									<P align="right">New Department:</P>
								</TD>
								<TD style="HEIGHT: 23px"><asp:dropdownlist id="cmbDept" runat="server" AutoPostBack="True" Font-Size="8pt">
										<asp:ListItem Value="1125">02341</asp:ListItem>
										<asp:ListItem Value="1126">02342</asp:ListItem>
										<asp:ListItem Value="1127">02343</asp:ListItem>
									</asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 130px; HEIGHT: 24px">
									<P align="right">New Machine Name:</P>
								</TD>
								<TD style="HEIGHT: 24px"><asp:dropdownlist id="cmbMachineName" runat="server" Width="96px" Font-Size="8pt" Font-Names="Verdana"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 130px; HEIGHT: 6px">
									<P align="right">New Plant:</P>
								</TD>
								<TD style="HEIGHT: 6px">
									<asp:textbox id="txtPlant" runat="server" Width="72px" MaxLength="2" Font-Size="8pt" Font-Names="Verdana"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 130px; HEIGHT: 18px">
									<P align="right">New Line Number:</P>
								</TD>
								<TD style="HEIGHT: 18px"><asp:textbox id="txtLineNumber" runat="server" Width="72px" MaxLength="4" Font-Size="8pt" Font-Names="Verdana"></asp:textbox></TD>
							</TR>
						</TABLE>
					</FONT></STRONG>
			</P>
			<P align="left"><asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></P>
			<P><asp:button id="btnSubmit" runat="server" Text="Submit"></asp:button>
				<asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox></P>
			<P>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</P>
			<P>&nbsp;</P>
		</form>
	</body>
</HTML>
