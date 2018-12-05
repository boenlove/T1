<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Kiosk_ConfigFile_AdminCode_Update.aspx.vb" Inherits="Plant23_BEC.Kiosk_ConfigFile_AdminCode_Update"%>
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
			<P align="center"><FONT face="Arial"><STRONG>Admin Code Change Page</STRONG></FONT></P>
			<STRONG><FONT face="Arial">
					<P align="center">
						<asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
					<P align="center">
						<asp:hyperlink id="hpkReturntoMain" runat="server" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink></P>
					<P align="left">
						<TABLE class="tblFormat" id="tbleAdminCodeChange" style="WIDTH: 464px; HEIGHT: 81px" cellSpacing="1"
							cellPadding="1" width="464" border="1">
							<TR>
								<TD style="WIDTH: 168px; HEIGHT: 15px">
									<P align="right">PC Name:</P>
								</TD>
								<TD style="HEIGHT: 15px">
									<asp:dropdownlist id="cmbPCNames" runat="server" Width="175px" AutoPostBack="True" Font-Names="Verdana"
										Font-Size="8pt"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 168px; HEIGHT: 21px">
									<P align="right">Old AdminCode:</P>
								</TD>
								<TD style="HEIGHT: 21px"><asp:label id="lblAdminCode" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 168px; HEIGHT: 24px">
									<P align="right">New AdminCode:</P>
								</TD>
								<TD style="HEIGHT: 24px">
									<asp:textbox id="txtAdminCode" runat="server" Width="72px" MaxLength="3" Font-Names="Verdana"
										Font-Size="8pt"></asp:textbox></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 168px">
									<P align="right">Old ExceptionAdminCode:</P>
								</TD>
								<TD>
									<asp:label id="lblExceptionAdminCode" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 168px">
									<P align="right">New ExceptionAdminCode:</P>
								</TD>
								<TD>
									<asp:textbox id="txtExceptionAdminCode" runat="server" Width="72px" MaxLength="3" Font-Names="Verdana"
										Font-Size="8pt"></asp:textbox></TD>
							</TR>
						</TABLE>
					</P>
					<P align="left"><STRONG><FONT face="Arial">Note: A blank will not be saved to the xml file.</FONT></STRONG></P>
				</FONT></STRONG>
			<P align="left"><asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></P>
			<P align="left">
				<asp:label id="lblMessage1" runat="server" CssClass="lblError"></asp:label></P>
			<P align="left">
				<asp:label id="lblMessage2" runat="server" CssClass="lblError"></asp:label></P>
			<P><asp:button id="btnSubmit" runat="server" Text="Submit"></asp:button>
				<asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox></P>
			<P>&nbsp;</P>
		</form>
	</body>
</HTML>
