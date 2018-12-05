<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_Data_Loader.aspx.vb" Inherits="Plant23_BEC.BEC_Data_Loader"%>
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
			<H1 align="center"><FONT face="Verdana" size="1"></FONT></H1>
			<FONT face="Verdana" size="1">
				<H1 align="left">
					<P align="center">
						<asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
			</FONT>
			<TABLE class="tblformat" id="tblDataLoader" title="Plant 23 BEC Data Loader" cellSpacing="0"
				cellPadding="0" width="402" align="center" border="0">
				<TR>
					<TD class="tbltitle" style="WIDTH: 576px; HEIGHT: 16px" bgColor="#ffffff" colSpan="2">
						<P align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:hyperlink id="hpkReturntoMain" runat="server" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink></P>
					</TD>
				</TR>
				<TR>
					<TD class="tbltitle" style="WIDTH: 576px; HEIGHT: 16px" bgColor="#ffffff" colSpan="2">KIOSK 
						Database Manager - BEC Data Loader</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 162px; HEIGHT: 36px" align="right" colSpan="1" rowSpan="1"><asp:requiredfieldvalidator id="rfvPartNumber" runat="server" ErrorMessage="*" ControlToValidate="txtPartNumber"
							Font-Names="Verdana"></asp:requiredfieldvalidator><asp:label id="lblPartNumber" runat="server" Font-Names="Verdana">Part Number:</asp:label></TD>
					<TD style="WIDTH: 320px; HEIGHT: 36px">&nbsp;
						<asp:textbox id="txtPartNumber" runat="server" Font-Names="Verdana" Width="80px" MaxLength="8"
							Font-Size="XX-Small"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 162px; HEIGHT: 24px">
						<P align="right"><asp:requiredfieldvalidator id="rfvECL" runat="server" ErrorMessage="*" ControlToValidate="txtECL" Font-Names="Verdana"></asp:requiredfieldvalidator><asp:label id="lblECL" runat="server" Font-Names="Verdana">ECL:</asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 24px">&nbsp;
						<asp:textbox id="txtECL" runat="server" Font-Names="Verdana" Width="32px" MaxLength="2" Font-Size="XX-Small"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 162px; HEIGHT: 25px">
						<P align="right"><asp:label id="lblPriority" runat="server" Font-Names="Verdana">Priority:</asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 25px">&nbsp;
						<asp:textbox id="txtPriority" runat="server" Font-Names="Verdana" Width="48px" Font-Size="XX-Small">99</asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 162px; HEIGHT: 23px">
						<P align="right"><asp:label id="lblContainer" runat="server" Font-Names="Verdana">Container:</asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 23px">&nbsp;
						<asp:textbox id="txtContainer" runat="server" Font-Names="Verdana" Width="40px" MaxLength="3"
							Font-Size="XX-Small"></asp:textbox>&nbsp;</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 162px; HEIGHT: 32px">
						<P align="right"><asp:label id="lblPackage" runat="server" Font-Names="Verdana">Package:</asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 32px">&nbsp;
						<asp:dropdownlist id="cmbPackage" runat="server" Font-Names="Verdana" AutoPostBack="True" Font-Size="XX-Small">
							<asp:ListItem Value="C">C</asp:ListItem>
							<asp:ListItem Value="4C">4C</asp:ListItem>
							<asp:ListItem Value="Y">Y</asp:ListItem>
							<asp:ListItem Value="Z">Z</asp:ListItem>
							<asp:ListItem Value="XY">XY</asp:ListItem>
						</asp:dropdownlist>&nbsp;
						<asp:dropdownlist id="cmbPkgSize" runat="server" Font-Names="Verdana" Font-Size="XX-Small" AutoPostBack="True"
							Visible="False">
							<asp:ListItem Value="0">..</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
						</asp:dropdownlist>&nbsp;
						<asp:label id="lblPkgQty" runat="server" Font-Names="Verdana" Font-Size="XX-Small" ForeColor="#E0E0E0">0</asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 162px; HEIGHT: 29px" align="right"><asp:label id="lblMatid" runat="server" Font-Names="Verdana">Mat ID:</asp:label></TD>
					<TD style="WIDTH: 320px; HEIGHT: 29px">&nbsp;
						<asp:textbox id="txtMatId" runat="server" Font-Names="Verdana" Width="56px" MaxLength="5" Font-Size="XX-Small"></asp:textbox>&nbsp;
						<asp:label id="lblOption" runat="server" Font-Names="Verdana" Font-Size="XX-Small">Optional</asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 162px; HEIGHT: 26px">
						<P align="right"><asp:label id="lblScheduleEntries" runat="server" Font-Names="Verdana" Width="143px"># of Schedule Entries:</asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 26px">&nbsp;
						<asp:textbox id="txtScheduleEntries" runat="server" Font-Names="Verdana" Width="24px" Font-Size="XX-Small">10</asp:textbox></TD>
				</TR>
			</TABLE>
			<P align="center"><asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></P>
			<P align="center"><asp:button id="btnLoadData" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Text="Load Data"
					Font-Bold="True"></asp:button>&nbsp;&nbsp;&nbsp;
				<asp:button id="btnWhatsSetup" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Text="What's Setup"
					Font-Bold="True" CausesValidation="False"></asp:button></P>
			<P align="center"><asp:button id="btnLoadDataAnyway" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
					Visible="False" Text="Load Data Anyway" Font-Bold="True"></asp:button></P>
			<P align="center"><asp:textbox id="txtPkgs_Per_Skid_Qty" runat="server" Visible="False" Enabled="False"></asp:textbox><asp:textbox id="txtMesPartId" runat="server" Visible="False" Enabled="False"></asp:textbox><asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox></P>
			<P align="center"><asp:label id="lblRecordsInserted" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
					Visible="False" Font-Bold="True"></asp:label></P>
		</form>
		</H1>
	</body>
</HTML>
