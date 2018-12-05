<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_SerialNumbers.aspx.vb" Inherits="Plant23_BEC.BEC_SerialNumberAvailability_withDLoc"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BEC_SerialNumberAvailability_withDLoc</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
			<P align="center">
				<TABLE class="tblFormat" id="tblICSLabelData" title="Plant 23 BEC ICS Label Data" style="WIDTH: 442px; HEIGHT: 104px"
					cellSpacing="0" cellPadding="0" width="442" align="center" bgColor="whitesmoke" border="0">
					<TR>
						<TD class="tbltitle" style="WIDTH: 553px; HEIGHT: 16px" bgColor="#ffffff" colSpan="2">
							<P align="right"><asp:hyperlink id="hpkReturntoMain" runat="server" Font-Bold="True" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink></P>
						</TD>
					</TR>
					<TR>
						<TD class="tbltitle" style="WIDTH: 553px; HEIGHT: 16px" bgColor="#ffffff" colSpan="2">KIOSK 
							Database Manager - BEC Serial Numbers</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 129px; HEIGHT: 26px" align="right" colSpan="1" rowSpan="1"><asp:label id="lblPartNumber" runat="server">Part Number:</asp:label></TD>
						<TD style="WIDTH: 305px; HEIGHT: 26px"><asp:dropdownlist id="cmbPartNumber" runat="server" AutoPostBack="True" Font-Size="8pt"></asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 129px; HEIGHT: 26px">
							<P align="right"><asp:label id="lblDestination" runat="server">Destination:</asp:label></P>
						</TD>
						<TD style="WIDTH: 305px; HEIGHT: 26px"><asp:dropdownlist id="cmbDestination" runat="server" Font-Size="8pt"></asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 129px; HEIGHT: 22px">
							<P align="right"><asp:label id="lblPackageCode" runat="server">Package Code:</asp:label></P>
						</TD>
						<TD style="WIDTH: 305px; HEIGHT: 22px"><asp:dropdownlist id="cmbPackageCode" runat="server" Font-Size="8pt"></asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 129px; HEIGHT: 26px">
							<P align="right"><asp:label id="lblECL" runat="server">ECL:</asp:label></P>
						</TD>
						<TD style="WIDTH: 305px; HEIGHT: 26px"><asp:dropdownlist id="cmbECL" runat="server" Font-Size="8pt"></asp:dropdownlist></TD>
					</TR>
				</TABLE>
			</P>
			<P align="center"><asp:button id="btnSubmit" runat="server" Text="Submit"></asp:button></P>
			<P align="center"><asp:label id="lblMessage" runat="server" CssClass="lblerror" Font-Size="8pt"></asp:label></P>
			<P align="center">
				<TABLE id="tblResults" title="Results Table" style="WIDTH: 280px; HEIGHT: 160px" cellSpacing="0"
					cellPadding="0" align="center" border="0" runat="server">
					<TR>
						<TD style="WIDTH: 310px; HEIGHT: 12px" align="left" colSpan="1" rowSpan="1"><asp:label id="lblSortOrder" runat="server" Font-Size="XX-Small" Visible="False"></asp:label></TD>
						<TD style="WIDTH: 210px; HEIGHT: 12px" align="right"><asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 520px; HEIGHT: 15px" colSpan="2"><asp:datagrid id="dgICS_LABEL_DATA" runat="server" Visible="False" OnSortCommand="dgICS_LABEL_DATA_SortCommand"
								CellPadding="2" BorderWidth="1px" BorderStyle="Solid" BorderColor="Silver" AutoGenerateColumns="False" AllowSorting="True" OnDeleteCommand="dgICS_LABEL_DATA_Delete">
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
									<asp:BoundColumn DataField="QUANTITY" SortExpression="QUANTITY" HeaderText="Std Pack"></asp:BoundColumn>
									<asp:BoundColumn DataField="COUNT(SERIAL_NBR)" SortExpression="COUNT(SERIAL_NBR)" HeaderText="Labels Available"></asp:BoundColumn>
									<asp:BoundColumn DataField="MATID" SortExpression="MATID" HeaderText="Matid"></asp:BoundColumn>
									<asp:BoundColumn DataField="DELIVERY_LOC_NBR" SortExpression="DELIVERY_LOC_NBR" HeaderText="D Loc"></asp:BoundColumn>
									<asp:BoundColumn DataField="TimeStamp" SortExpression="TimeStamp" HeaderText="TimeStamp" DataFormatString="{0:d}"></asp:BoundColumn>
									<asp:ButtonColumn Text="Delete" CommandName="Delete"></asp:ButtonColumn>
								</Columns>
								<PagerStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD class="note" style="WIDTH: 520px" align="center" colSpan="2">All information is 
							Delphi Confidential</TD>
					</TR>
				</TABLE>
				<br>
			</P>
			<P align="center"></P>
			<P align="left"><asp:textbox id="txtMesPartId" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox></P>
		</form>
	</body>
</HTML>
