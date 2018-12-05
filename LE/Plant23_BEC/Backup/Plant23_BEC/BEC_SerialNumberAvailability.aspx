<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_SerialNumberAvailability.aspx.vb" Inherits="Plant23_BEC.BEC_SerialNumberAvailability"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BEC_SerialNumberAvailability</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P align="center"><STRONG><FONT size="2">BEC Serial Number Availability</FONT></STRONG></P>
			<P align="center">
				<asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
			<P align="center">
				<asp:label id="lblMessage" runat="server" CssClass="lblerror" Font-Size="8pt"></asp:label></P>
			<P align="center">
				<asp:Label id="lblSortOrder" runat="server" Font-Size="XX-Small" ForeColor="Red" Font-Bold="True">Label</asp:Label><br>
			</P>
			<P align="center">
				<TABLE id="tblResults" title="Results Table" style="WIDTH: 280px" cellSpacing="0" cellPadding="0"
					align="center" border="0" runat="server">
					<TR>
						<TD style="WIDTH: 310px; HEIGHT: 12px" align="left"></TD>
						<TD style="WIDTH: 210px; HEIGHT: 12px" align="right">
							<asp:hyperlink id="hpkReturntoMain" runat="server" Font-Bold="True" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 310px; HEIGHT: 12px" align="left" colSpan="1" rowSpan="1"></TD>
						<TD style="WIDTH: 210px; HEIGHT: 12px" align="right"><asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 520px; HEIGHT: 15px" colSpan="2"><asp:datagrid id="dgICS_LABEL_DATA" runat="server" CellPadding="2" BorderWidth="1px" BorderStyle="Solid"
								BorderColor="Silver" AutoGenerateColumns="False" AllowSorting="True" OnSortCommand="dgICS_LABEL_DATA_SortCommand" Visible="False">
								<SelectedItemStyle Wrap="False"></SelectedItemStyle>
								<EditItemStyle Wrap="False" HorizontalAlign="Right"></EditItemStyle>
								<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
								<ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="dgItem" VerticalAlign="Middle"></ItemStyle>
								<HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="PART_NBR" SortExpression="PART NBR" HeaderText="Part Nbr">
										<HeaderStyle Width="15px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ECL" SortExpression="ECL" HeaderText="ECL">
										<HeaderStyle Width="15px"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DESTINATION" SortExpression="DESTINATION" HeaderText="Dest"></asp:BoundColumn>
									<asp:BoundColumn DataField="PACKAGE_CODE" SortExpression="PACKAGE_CODE" HeaderText="Package Cd">
										<HeaderStyle Width="15px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="MATID" SortExpression="MATID" HeaderText="Matid"></asp:BoundColumn>
									<asp:BoundColumn DataField="QUANTITY" SortExpression="QUANTITY" HeaderText="Std Pack"></asp:BoundColumn>
									<asp:BoundColumn DataField="COUNT(SERIAL_NBR)" SortExpression="COUNT(SERIAL_NBR)" HeaderText="Labels Available"></asp:BoundColumn>
								</Columns>
								<PagerStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
						</TD>
					</TR>
					<TR>
						<TD class="note" align="center" colSpan="2" style="WIDTH: 520px">All information is 
							Delphi Confidential</TD>
					</TR>
				</TABLE>
			</P>
			<P align="left">
				<asp:TextBox id="txtPartNumber" runat="server" Visible="False"></asp:TextBox>
				<asp:TextBox id="txtECL" runat="server" Visible="False"></asp:TextBox>
				<asp:TextBox id="txtDestination" runat="server" Visible="False"></asp:TextBox>
				<asp:TextBox id="txtPackageCode" runat="server" Visible="False"></asp:TextBox>
				<asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox></P>
		</form>
	</body>
</HTML>
