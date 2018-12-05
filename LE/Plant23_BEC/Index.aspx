<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Index.aspx.vb" Inherits="Plant23_BEC.Home"%>
<%@ Register TagPrefix="uc1" TagName="Cntrl_Header" Src="Common/Cntrl_Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Cntrl_Menu" Src="Common/Cntrl_Menu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Cntrl_Footer" Src="Common/Cntrl_Footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Plant23_BEC</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%" height="100%">
				<TR>
					<TD colSpan="2" valign="top" style="HEIGHT: 12px">
						<uc1:Cntrl_Header id="Cntrl_Header1" runat="server"></uc1:Cntrl_Header></TD>
				</TR>
				<TR>
					<TD class="Background" vAlign="top" style="WIDTH: 210px">
						<uc1:Cntrl_Menu id="Cntrl_Menu1" runat="server"></uc1:Cntrl_Menu></TD>
					<TD vAlign="top" align="center">
						<P>&nbsp;</P>
						<P>&nbsp;</P>
						<P>&nbsp;</P>
						<P>&nbsp;</P>
						<P>&nbsp;</P>
						<P>&nbsp;</P>
						<P>
							<TABLE id="Table2" cellSpacing="0" cellPadding="2" width="450" align="center" border="0">
								<TR>
									<TD><BR>
										<asp:Label id="lblTitle" runat="server" CssClass="tblTitle">Plant 23 BEC System</asp:Label></TD>
								</TR>
								<TR>
									<TD class="tblTitle"><IMG src="Images/Bar.jpg"></TD>
								</TR>
								<TR>
									<TD>
										<asp:Label id="lblDisclaimer" runat="server">DISCLAIMER: All information contained in this site is confidential. Using the "Back" button located on the browsers toolbar may cause unexpected results. Use the application's menu to navigate through this site.</asp:Label></TD>
								</TR>
								<TR>
									<TD align="center">&nbsp;
									</TD>
								</TR>
								<TR>
									<TD></TD>
								</TR>
							</TABLE>
						</P>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left" colSpan="2">
						<uc1:Cntrl_Footer id="Cntrl_Footer1" runat="server"></uc1:Cntrl_Footer></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
