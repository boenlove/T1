<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_Ship_Destination_All_Users.aspx.vb" Inherits="Plant23_BEC.BEC_Ship_Destination_All_Users"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BEC Ship Destination</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<H1 align="center"></H1>
			<FONT size="2">
				<H1 align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></H1>
				<H1 align="center"><asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></H1>
				<H1 align="center">
			</FONT><FONT size="2">BEC Ship Destinations -- All Users</FONT></H1>
			<P align="center">
				<TABLE class="tblformat" id="tblBECShipDestination" style="WIDTH: 743px; HEIGHT: 248px"
					cellSpacing="0" cellPadding="0" width="743" align="left" border="0">
					<TR>
						<TD style="WIDTH: 189px" vAlign="top" align="center">
							<P align="left">&nbsp;
								<asp:hyperlink id="hpkReturntoMain" runat="server" Width="240px" Font-Bold="True" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</P>
						</TD>
						<TD vAlign="top" align="center"></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 189px" vAlign="top" align="center">
							<P align="left"><asp:label id="lblPartNumber" runat="server">Part Number:</asp:label>&nbsp;
								<asp:dropdownlist id="cmbPartNumber" runat="server" DataValueField="PART_NBR" DataTextField="PART_NBR"
									Width="120px" Font-Names="Verdana" Font-Size="XX-Small"></asp:dropdownlist></P>
							<P align="left"><asp:label id="lblPackageType" runat="server" Font-Names="Verdana" Font-Size="XX-Small">Package Type</asp:label></P>
							<P align="left"><asp:radiobuttonlist id="rdoPackageType" runat="server" Width="176px" Font-Names="Verdana" Font-Size="XX-Small"
									Height="120px">
									<asp:ListItem Value="Z">Singles</asp:ListItem>
									<asp:ListItem Value="C">Cardboard Singles</asp:ListItem>
									<asp:ListItem Value="Y">Zytec</asp:ListItem>
									<asp:ListItem Value="4C">Cardboard Zytec</asp:ListItem>
								</asp:radiobuttonlist></P>
							<P><asp:button id="btnSubmit" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Text="Refresh Data"></asp:button></P>
							<P align="left">&nbsp;</P>
						</TD>
						<TD vAlign="top" align="center">
							<P align="left"><br>
							</P>
							<P align="left">
								<TABLE id="tblResults" title="Results Table" style="WIDTH: 280px; HEIGHT: 160px" cellSpacing="0"
									cellPadding="0" align="center" border="0" runat="server">
									<TR>
										<TD style="WIDTH: 310px; HEIGHT: 12px" align="left" colSpan="1" rowSpan="1"></TD>
										<TD style="WIDTH: 210px; HEIGHT: 12px" align="right"><asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 520px; HEIGHT: 15px" colSpan="2"><asp:datagrid id="dgSchedule" runat="server" Width="287px" GridLines="Vertical" CellPadding="3"
												BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False" Visible="False">
												<FooterStyle ForeColor="Black" BackColor="Silver"></FooterStyle>
												<SelectedItemStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" HorizontalAlign="Center"
													ForeColor="Black" Width="40px"></SelectedItemStyle>
												<EditItemStyle Font-Size="Smaller" Font-Names="Verdana" HorizontalAlign="Center" Width="40px"></EditItemStyle>
												<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
												<ItemStyle Width="40px" CssClass="dgItem"></ItemStyle>
												<HeaderStyle CssClass="dgHeader"></HeaderStyle>
												<Columns>
													<asp:HyperLinkColumn Text="Check" DataNavigateUrlField="Check_Labels"></asp:HyperLinkColumn>
													<asp:BoundColumn DataField="BUILD_PRIORITY" SortExpression="BUILD_PRIORITY" HeaderText="Priority"></asp:BoundColumn>
													<asp:BoundColumn DataField="SCHEDULE_ID" SortExpression="SCHEDULE_ID" ReadOnly="True" HeaderText="Id"></asp:BoundColumn>
													<asp:BoundColumn DataField="MACHINE_NAME" SortExpression="MACHINE_NAME" ReadOnly="True" HeaderText="Line#"></asp:BoundColumn>
													<asp:BoundColumn DataField="PART_NBR" SortExpression="PART_NBR" ReadOnly="True" HeaderText="PartNo"></asp:BoundColumn>
													<asp:BoundColumn DataField="REVISION_PHYSICAL" SortExpression="REVISION_PHYSICAL" ReadOnly="True"
														HeaderText="ECL"></asp:BoundColumn>
													<asp:BoundColumn DataField="CONTAINER_CODE" SortExpression="CONTAINER_CODE" ReadOnly="True" HeaderText="Dest"></asp:BoundColumn>
													<asp:BoundColumn DataField="PACKAGE_CODE" SortExpression="PACKAGE_CODE" ReadOnly="True" HeaderText="PKG"></asp:BoundColumn>
													<asp:BoundColumn DataField="PKGS_PER_SKID_QTY" SortExpression="PKGS_PER_SKID_QTY" ReadOnly="True"
														HeaderText="PerSkid"></asp:BoundColumn>
													<asp:BoundColumn DataField="PKGS_USED_QTY" SortExpression="PKGS_USED_QTY" ReadOnly="True" HeaderText="Labeled"></asp:BoundColumn>
													<asp:BoundColumn DataField="MATID" SortExpression="MATID" ReadOnly="True" HeaderText="MATID"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="MES_PART_ID" SortExpression="MES_PART_ID"></asp:BoundColumn>
													<asp:BoundColumn DataField="INSERT_USERID" SortExpression="INSERT_USERID" ReadOnly="True" HeaderText="UserId"></asp:BoundColumn>
												</Columns>
												<PagerStyle HorizontalAlign="Center" ForeColor="Black" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
											</asp:datagrid></TD>
									</TR>
									<TR>
										<TD class="note" style="WIDTH: 520px" align="center" colSpan="2">All information is 
											Delphi Confidential</TD>
									</TR>
								</TABLE>
							</P>
							<P><asp:label id="lblNoRecordsFound" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
									Visible="False" Font-Bold="True"></asp:label></P>
						</TD>
					</TR>
				</TABLE>
			</P>
			<P align="center"><asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtServerType" runat="server" Visible="False"></asp:textbox></P>
			<P align="center">&nbsp;</P>
			<P align="center">&nbsp;</P>
			<P align="center">&nbsp;</P>
			<P align="center">&nbsp;</P>
			<P align="center">&nbsp;</P>
			<P align="center">&nbsp;</P>
			<P align="center">&nbsp;</P>
			</FONT></STRONG></form>
	</body>
</HTML>
