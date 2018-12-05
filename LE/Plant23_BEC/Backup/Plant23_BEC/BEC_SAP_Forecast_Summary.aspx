<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_SAP_Forecast_Summary.aspx.vb" Inherits="Plant23_BEC.BEC_SAP_Forecast_Summary"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BEC SAP Forecast Summary</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT size="2">
				<H1 align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></H1>
				<H1 align="center"><asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></H1>
				<H1 align="center">
			</FONT><FONT size="2">BEC SAP ForeCast Summary</FONT></H1>
			<P align="center">
				<TABLE class="tblformat" id="tblBECShipDestination" height="100%" cellSpacing="0" cellPadding="0"
					width="100%" align="left" border="0">
					<TR>
						<TD style="WIDTH: 167px" vAlign="top" align="center" colSpan="2">
							<P align="left">&nbsp;&nbsp;
								<asp:hyperlink id="hpkReturntoMain" runat="server" NavigateUrl="Index.aspx" Font-Bold="True" Width="296px">Return to Main</asp:hyperlink>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</P>
						</TD>
					</TR>
					<TR>
						<TD vAlign="top" align="center">
							<P align="left">
								<center><asp:label id="lblPartNumber" runat="server">Selected Week:</asp:label>
									<p></p>
									<asp:dropdownlist id="cmbSelectedWeek" runat="server" Width="100px" DataValueField="PART_NBR" DataTextField="PART_NBR"
										Font-Names="Verdana" Font-Size="XX-Small"></asp:dropdownlist></center>
							<P></P>
							<P><asp:button id="btnSubmit" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Text="Refresh Data"></asp:button></P>
							<P align="left"><br>
								&nbsp;</P>
							<P align="left">
								<TABLE id="tblResults" title="Results Table" cellSpacing="0" cellPadding="0" align="center"
									border="0" runat="server">
									<TR>
										<TD align="left" colSpan="1" rowSpan="1"></TD>
										<TD align="right"><asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
									</TR>
									<TR>
										<TD colSpan="2"><asp:datagrid id="dgForecastSummary" runat="server" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
												BorderStyle="None" AutoGenerateColumns="False" Visible="False">
												<FooterStyle ForeColor="Black" BackColor="Silver"></FooterStyle>
												<SelectedItemStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" HorizontalAlign="Center"
													ForeColor="Black" Width="40px"></SelectedItemStyle>
												<EditItemStyle Font-Size="Smaller" Font-Names="Verdana" HorizontalAlign="Center" Width="40px"></EditItemStyle>
												<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
												<ItemStyle Width="40px" CssClass="dgItem"></ItemStyle>
												<HeaderStyle CssClass="dgHeader"></HeaderStyle>
												<Columns>
													<asp:BoundColumn DataField="ICS_DEPT" HeaderText="Department Number"></asp:BoundColumn>
													<asp:BoundColumn DataField="PART_NBR" HeaderText="Part Number"></asp:BoundColumn>
													<asp:BoundColumn DataField="DESTINATION" HeaderText="Destination"></asp:BoundColumn>
													<asp:BoundColumn DataField="BUILD_DATE" HeaderText="Build Start Date"></asp:BoundColumn>
													<asp:BoundColumn DataField="CUSTOMER_REQUIRED" HeaderText="Customer Required Amount"></asp:BoundColumn>
													<asp:BoundColumn DataField="AVAILABLE" HeaderText="Available"></asp:BoundColumn>
													<asp:BoundColumn DataField="REQUIRED" HeaderText="Required"></asp:BoundColumn>
													<asp:BoundColumn DataField="ADJUSTED" HeaderText="Adjusted"></asp:BoundColumn>
												</Columns>
												<PagerStyle HorizontalAlign="Center" ForeColor="Black" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
											</asp:datagrid></TD>
									</TR>
									<TR>
										<TD class="note" align="center" colSpan="2">All information is Delphi Confidential</TD>
									</TR>
								</TABLE>
							</P>
							<P><asp:label id="lblNoRecordsFound" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="XX-Small"
									Visible="False"></asp:label></P>
						</TD>
					</TR>
				</TABLE>
				<asp:textbox id="txtSecurityCheck" style="Z-INDEX: 101; LEFT: 128px; POSITION: absolute; TOP: 664px"
					runat="server" Width="1px" Visible="False" Enabled="False"></asp:textbox></P>
		</form>
	</body>
</HTML>
