<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Kiosk_EditPCName.aspx.vb" Inherits="Plant23_BEC.Kiosk_AddPCName"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Kiosk_AddPCName</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P align="center"><FONT face="Arial"><STRONG>PCNames Edit </STRONG></FONT><FONT face="Arial">
					<STRONG>Page</STRONG></FONT></P>
			<P align="left"><STRONG><FONT face="Arial">
						<TABLE class="tblFormat" id="Table1" style="WIDTH: 368px; HEIGHT: 56px" cellSpacing="1"
							cellPadding="1" width="368" border="1">
							<TR>
								<TD style="WIDTH: 114px; HEIGHT: 31px">
									<P align="right">PC Name:</P>
								</TD>
								<TD style="HEIGHT: 31px">
									<asp:DropDownList id="cmbPCNames" runat="server" AutoPostBack="True" Width="175px"></asp:DropDownList>&nbsp;</TD>
							</TR>
							<TR>
								<TD style="WIDTH: 114px">
									<P align="right">Insert PC Name:</P>
								</TD>
								<TD>
									<asp:textbox id="txtPCNames" runat="server" Width="168px" MaxLength="3"></asp:textbox></TD>
							</TR>
						</TABLE>
					</FONT></STRONG>
			</P>
			<P align="left">
				<asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></P>
			<P>
				<asp:button id="btnEdit" runat="server" Text="Edit PCName"></asp:button>&nbsp;
				<asp:Button id="btnAdd" runat="server" Text="Insert PCName"></asp:Button></P>
			<P>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</P>
		</form>
	</body>
</HTML>
