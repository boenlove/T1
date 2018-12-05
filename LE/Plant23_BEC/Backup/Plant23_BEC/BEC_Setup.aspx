<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_Setup.aspx.vb" Inherits="Plant23_BEC.BEC_Setup"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BEC_Setup</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<H1 align="center"><FONT face="Verdana" size="1"></FONT></H1>
			<H1 align="center"><FONT face="Verdana" size="2">BEC What's Setup</FONT></H1>
			<P align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
			<P align="center">
				<asp:hyperlink id="hpkReturntoMain" runat="server" Font-Bold="True" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink></P>
			<P align="center">
				<TABLE class="tblformat" id="tblWhatsSetup" title="Plant 23 BEC What's Up" style="WIDTH: 312px; HEIGHT: 67px"
					cellSpacing="0" cellPadding="0" width="312" align="center" border="0">
					<TR>
						<TD style="WIDTH: 129px; HEIGHT: 12px">
							<P align="right"><asp:label id="lblPartNumber" runat="server" Font-Names="Verdana">Part Number:</asp:label></P>
						</TD>
						<TD style="WIDTH: 297px; HEIGHT: 12px">&nbsp;&nbsp;
							<asp:dropdownlist id="cmbPartNumber" runat="server" Font-Names="Verdana" Font-Size="XX-Small"></asp:dropdownlist>&nbsp;&nbsp;</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 129px; HEIGHT: 1px">
							<P align="right"><asp:label id="lblECL" runat="server" Font-Names="Verdana">Container:</asp:label></P>
						</TD>
						<TD style="WIDTH: 297px; HEIGHT: 1px">&nbsp;
							<asp:textbox id="txtContainer" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="48px"></asp:textbox>&nbsp;<FONT face="Verdana" size="1">Leave 
								Blank to show all.</FONT></TD>
					</TR>
				</TABLE>
			</P>
			<P align="center"><asp:button id="btnWhatsSetup" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True"
					Text="What's Setup"></asp:button></P>
			<P align="center"><asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></P>
			<P></P>
			<P align="center"><asp:textbox id="txtMesPartId" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtPartNumber" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox></P>
			<P align="center"><asp:label id="lblNoDataFound" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True"
					Visible="False" ForeColor="Red">Label</asp:label></P>
			<P align="left">
				<TABLE id="tblResults" title="Results Table" style="WIDTH: 280px; HEIGHT: 160px" cellSpacing="0"
					cellPadding="0" align="center" border="0" runat="server">
					<TR>
						<TD style="WIDTH: 310px; HEIGHT: 12px" align="left" colSpan="1" rowSpan="1"></TD>
						<TD style="WIDTH: 210px; HEIGHT: 12px" align="right">
							<asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 520px; HEIGHT: 15px" colSpan="2">
							<asp:datagrid id="dgWhatsSetup" runat="server" Font-Names="Verdana" Font-Size="XX-Small" CellPadding="3"
								BackColor="White" BorderStyle="None" AutoGenerateColumns="False">
								<FooterStyle ForeColor="Silver" BackColor="White"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="White"></SelectedItemStyle>
								<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
								<ItemStyle CssClass="dgItem"></ItemStyle>
								<HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" ForeColor="White" CssClass="dgHeader"></HeaderStyle>
								<Columns>
									<asp:TemplateColumn></asp:TemplateColumn>
									<asp:BoundColumn DataField="PART_NBR" SortExpression="PART_NBR" HeaderText="PartNo"></asp:BoundColumn>
									<asp:BoundColumn DataField="REVISION_PHYSICAL" SortExpression="REVISION_PHYSICAL" HeaderText="ECL"></asp:BoundColumn>
									<asp:BoundColumn DataField="CONTAINER_CODE" SortExpression="CONTAINER_CODE" HeaderText="CNTR"></asp:BoundColumn>
									<asp:BoundColumn DataField="PACKAGE_CODE" SortExpression="PACKAGE_CODE" HeaderText="PKG"></asp:BoundColumn>
									<asp:BoundColumn DataField="BUILD_PRIORITY" SortExpression="BUILD_PRIORITY" HeaderText="PRIORITY"></asp:BoundColumn>
									<asp:BoundColumn DataField="PART_COUNT" SortExpression="PART_COUNT" HeaderText="COUNT(PART_NBR)"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Left" BackColor="White" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD class="note" style="WIDTH: 520px" align="center" colSpan="2">All information is 
							Delphi Confidential</TD>
					</TR>
				</TABLE>
			</P>
		</form>
	</body>
</HTML>
