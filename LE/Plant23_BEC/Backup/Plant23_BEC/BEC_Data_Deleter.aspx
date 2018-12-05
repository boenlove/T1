<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_Data_Deleter.aspx.vb" Inherits="Plant23_BEC.BEC_Data_Deleter"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BEC_Data_Loader</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="/Plant23_BEC/Common/MFG_Style.css" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<FONT face="Verdana" size="1">
				<H1 align="center"><FONT size="1"></FONT></H1>
				<H1 align="center"><FONT size="1"></FONT></H1>
				<H1 align="center"><FONT size="1"></FONT></H1>
			</FONT>
			<P align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label>&nbsp;&nbsp;&nbsp;</P>
			<P align="center"></P>
			<TABLE class="tblformat" id="tblLayout" title="Plant 23 BEC Data Deleter" style="WIDTH: 442px; HEIGHT: 104px"
				cellSpacing="0" cellPadding="0" width="442" align="center" bgColor="whitesmoke" border="0">
				<TBODY>
					<TR>
						<td>
							<TABLE class="tblFormat" id="tblDataDeleter" title="Plant 23 BEC Data Deleter" style="WIDTH: 442px; HEIGHT: 104px"
								cellSpacing="0" cellPadding="0" width="442" align="center" bgColor="whitesmoke" border="0">
								<TR>
									<TD class="tbltitle" style="WIDTH: 553px; HEIGHT: 16px" bgColor="#ffffff" colSpan="2">
										<P align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
											<asp:hyperlink id="hpkReturntoMain" runat="server" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink></P>
									</TD>
								</TR>
								<TR>
									<TD class="tbltitle" style="WIDTH: 553px; HEIGHT: 16px" bgColor="#ffffff" colSpan="2">KIOSK 
										Database Manager - BEC Data Deleter</TD>
								</TR>
								<TR>
									<TD style="WIDTH: 129px; HEIGHT: 26px" align="right" colSpan="1" rowSpan="1"><asp:label id="lblPartNumber" runat="server">Part Number:</asp:label></TD>
									<TD style="WIDTH: 305px; HEIGHT: 26px"><asp:dropdownlist id="cmbPartNumber" runat="server" Font-Size="8pt" AutoPostBack="True"></asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 129px; HEIGHT: 28px">
										<P align="right"><asp:label id="lblECL" runat="server" Font-Names="Verdana">ECL:</asp:label></P>
									</TD>
									<TD style="WIDTH: 305px; HEIGHT: 28px"><asp:dropdownlist id="cmbECL" runat="server" Font-Size="8pt"></asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 129px; HEIGHT: 26px">
										<P align="right"><asp:label id="lblContainer" runat="server">Container:</asp:label></P>
									</TD>
									<TD style="WIDTH: 305px; HEIGHT: 26px"><asp:dropdownlist id="cmbContainer" runat="server" Font-Size="8pt"></asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 129px; HEIGHT: 30px" align="right"><asp:label id="lblPackageCode" runat="server">Package Code:</asp:label></TD>
									<TD style="WIDTH: 305px; HEIGHT: 30px"><asp:dropdownlist id="cmbPackageCode" runat="server" Font-Size="8pt"></asp:dropdownlist></TD>
								</TR>
							</TABLE>
							<P></P>
							<P align="center"><asp:textbox id="txtMesPartId" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox></P>
							<P align="center"><asp:label id="lblRecordsDeleted" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
									Visible="False" Font-Bold="True"></asp:label></P>
							<P align="center"><asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></P>
							<P align="center"><asp:button id="btnDeleteData" runat="server" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
									Text="Delete Data"></asp:button></P>
						</td>
					</TR>
				</TBODY>
			</TABLE>
			<P align="center">&nbsp;</P>
		</form>
	</body>
</HTML>
