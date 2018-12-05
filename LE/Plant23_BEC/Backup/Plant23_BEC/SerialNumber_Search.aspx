<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SerialNumber_Search.aspx.vb" Inherits="Plant23_BEC.SerialNumber_Search"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>SerialNumber_Search</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P>&nbsp;</P>
			<P align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
			<TABLE class="tblformat" id="tbStandardPackHistory" title="Standard Pack History" style="WIDTH: 489px; HEIGHT: 246px"
				cellSpacing="0" cellPadding="0" width="489" align="center" border="0">
				<TR>
					<TD class="tbltitle" style="WIDTH: 576px; HEIGHT: 17px" bgColor="#ffffff" colSpan="2">
						<P align="right">
							<asp:hyperlink id="hpkReturntoMain" runat="server" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink></P>
					</TD>
				</TR>
				<TR>
					<TD class="tbltitle" style="WIDTH: 576px; HEIGHT: 17px" bgColor="#ffffff" colSpan="2">Standard 
						Pack History</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 337px; HEIGHT: 36px" align="right" colSpan="1" rowSpan="1"><asp:requiredfieldvalidator id="rfvSearchBy" runat="server" Font-Names="Verdana" ControlToValidate="cmbSearchBy"
							ErrorMessage="*"></asp:requiredfieldvalidator><asp:label id="lblSearchBy" runat="server" Font-Names="Verdana">Search By:</asp:label></TD>
					<TD style="WIDTH: 320px; HEIGHT: 36px"><asp:dropdownlist id="cmbSearchBy" runat="server" Font-Names="Verdana" AutoPostBack="True" Font-Size="XX-Small"
							Width="264px">
							<asp:ListItem Value="Start Date and End Date (BEC)" Selected="True">Start Date and End Date (BEC)</asp:ListItem>
							<asp:ListItem Value="Start Date and End Date (BAG)">Start Date and End Date (BAG)</asp:ListItem>
							<asp:ListItem Value="Serial Number Range (BEC)">Serial Number Range (BEC)</asp:ListItem>
							<asp:ListItem Value="Serial Number Range (BAG)">Serial Number Range (BAG)</asp:ListItem>
							<asp:ListItem Value="Sub-Standard ID (BEC)">Sub-Standard ID (BEC)</asp:ListItem>
							<asp:ListItem Value="Sub-Standard ID (BAG)">Sub-Standard ID (BAG)</asp:ListItem>
							<asp:ListItem Value="Alphanumeric Serial Number Pattern">Alphanumeric Serial Number Pattern</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 337px; HEIGHT: 28px">
						<P align="right"><asp:requiredfieldvalidator id="rfvStart" runat="server" Font-Names="Verdana" ControlToValidate="txtStart" ErrorMessage="*"></asp:requiredfieldvalidator><asp:label id="lblStart" runat="server"></asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 28px"><asp:textbox id="txtStart" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="104px"
							MaxLength="23"></asp:textbox>&nbsp;&nbsp;&nbsp;
						<asp:imagebutton id="imgCalStartDate" runat="server" ImageUrl="Images/calendar.gif" CausesValidation="False"
							Visible="False"></asp:imagebutton><asp:calendar id="StartCalendar" runat="server" Visible="False" BackColor="#D9E4EB" BorderColor="Teal">
							<DayHeaderStyle CssClass="DayHeaderStyle"></DayHeaderStyle>
							<TitleStyle CssClass="TitleStyle"></TitleStyle>
							<WeekendDayStyle CssClass="WeekendDayStyle"></WeekendDayStyle>
							<OtherMonthDayStyle CssClass="OtherMonthDay"></OtherMonthDayStyle>
						</asp:calendar></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 337px; HEIGHT: 27px">
						<P align="right"><asp:label id="lblEnd" runat="server"></asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 27px"><asp:textbox id="txtEnd" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="104px"
							MaxLength="23"></asp:textbox>&nbsp;&nbsp;&nbsp;
						<asp:imagebutton id="imgCalEndDate" runat="server" ImageUrl="Images/calendar.gif" CausesValidation="False"
							Visible="False"></asp:imagebutton><asp:calendar id="EndCalendar" runat="server" Visible="False" BackColor="#D9E4EB" BorderColor="Teal"
							BorderStyle="Solid" SelectedDate="2006-02-06">
							<DayHeaderStyle CssClass="DayHeaderStyle"></DayHeaderStyle>
							<TitleStyle CssClass="TitleStyle"></TitleStyle>
							<WeekendDayStyle CssClass="WeekendDayStyle"></WeekendDayStyle>
							<OtherMonthDayStyle CssClass="OtherMonthDay"></OtherMonthDayStyle>
						</asp:calendar></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 337px; HEIGHT: 1px">
						<P align="right"><asp:label id="lblPlant" runat="server" Font-Names="Verdana">Plant:</asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 1px"><asp:dropdownlist id="cmbPlant" runat="server" Font-Names="Verdana" AutoPostBack="True" Font-Size="XX-Small"
							Width="96px">
							<asp:ListItem Value="0" Selected="True">-- Select --</asp:ListItem>
							<asp:ListItem Value="Plant 23">Plant 23</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 337px; HEIGHT: 17px">
						<P align="right"><asp:label id="lbDepartment" runat="server" Font-Names="Verdana">Department:</asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 17px"><asp:dropdownlist id="cmbDepartment" runat="server" Font-Names="Verdana" AutoPostBack="True" Font-Size="XX-Small"
							Width="269px"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 337px; HEIGHT: 15px" align="right"><asp:label id="lblShift" runat="server" Font-Names="Verdana"> Shift:</asp:label></TD>
					<TD style="WIDTH: 320px; HEIGHT: 15px"><asp:dropdownlist id="cmbShift" runat="server" Font-Names="Verdana" AutoPostBack="True" Font-Size="XX-Small"
							Width="96px"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 337px; HEIGHT: 26px">
						<P align="right"><asp:label id="lblMachines" runat="server" Font-Names="Verdana">Machines:</asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 26px"><asp:dropdownlist id="cmbMachineName" runat="server" Font-Names="Verdana" AutoPostBack="True" Font-Size="XX-Small"
							Width="96px"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 337px; HEIGHT: 26px">
						<P align="right"><asp:label id="lbPartNumber" runat="server" Font-Names="Verdana" Width="85px">Part Number:</asp:label></P>
					</TD>
					<TD style="WIDTH: 320px; HEIGHT: 26px"><asp:textbox id="txtPartNumber" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="120px"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 337px; HEIGHT: 26px"><asp:label id="lblMsg" runat="server" CssClass="lblError"></asp:label></TD>
					<TD style="WIDTH: 320px; HEIGHT: 26px">
						<P align="right"><asp:button id="btnLookUp" runat="server" Text="Lookup History"></asp:button></P>
					</TD>
				</TR>
			</TABLE>
			<P align="right"><asp:textbox id="txtMesToolId" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtMesPartId" runat="server" Visible="False"></asp:textbox></P>
			<P align="center">
				<TABLE id="tblResults" title="Results Table" style="WIDTH: 520px; HEIGHT: 12px" cellSpacing="0"
					cellPadding="0" width="520" align="center" border="0" runat="server">
					<tr>
						<TD align="left" colSpan="2"><asp:button id="btnExport" runat="server" Font-Size="8pt" Width="90px" CausesValidation="False"
								Text="Export to Excel" ToolTip="Click to Export current Search Criteria to Excel"></asp:button></TD>
					</tr>
					<TR>
						<TD style="WIDTH: 310px; HEIGHT: 11px" align="left" colSpan="1" rowSpan="1"><asp:label id="lblSortOrder" runat="server" Font-Size="XX-Small" Visible="False" ForeColor="Black"></asp:label></TD>
						<TD style="WIDTH: 210px; HEIGHT: 11px" align="right"><asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 15px" colSpan="2"><asp:datagrid id="dgStandardPackHistory" runat="server" Width="287px" BorderStyle="None" AllowSorting="True"
								GridLines="Vertical" CellPadding="3" BorderWidth="1px" OnSortCommand="dgStandardPackHistory_SortCommand" AutoGenerateColumns="False">
								<FooterStyle ForeColor="Black" BackColor="Silver"></FooterStyle>
								<SelectedItemStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" HorizontalAlign="Center"
									ForeColor="Black" Width="40px"></SelectedItemStyle>
								<EditItemStyle Font-Size="Smaller" Font-Names="Verdana" HorizontalAlign="Center" Width="40px"></EditItemStyle>
								<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
								<ItemStyle Width="40px" CssClass="dgItem"></ItemStyle>
								<HeaderStyle CssClass="dgHeader"></HeaderStyle>
								<Columns>
									<asp:HyperLinkColumn Text="Serial_Content" DataNavigateUrlField="Serial_Content" DataNavigateUrlFormatString="javascript:var w=window.open('{0}','','height=250,width=650,resizable=yes,scrollbars=yes,status=no,menubar=no');"></asp:HyperLinkColumn>
									<asp:BoundColumn DataField="SERIAL_NBR" SortExpression="SERIAL_NBR" ReadOnly="True" HeaderText="Serial Nbr"></asp:BoundColumn>
									<asp:BoundColumn DataField="INSERT_TMSTM" SortExpression="INSERT_TMSTM" ReadOnly="True" HeaderText="Make Date-Time"></asp:BoundColumn>
									<asp:BoundColumn DataField="MACHINE_NAME" SortExpression="MACHINE_NAME" ReadOnly="True" HeaderText="Machine"></asp:BoundColumn>
									<asp:BoundColumn DataField="PART_NBR" SortExpression="PART_NBR" ReadOnly="True" HeaderText="Part Nbr"></asp:BoundColumn>
									<asp:BoundColumn DataField="REV_PHYSICAL" SortExpression="REV_PHYSICAL" ReadOnly="True" HeaderText="ECL"></asp:BoundColumn>
									<asp:BoundColumn DataField="LABEL_CODE" SortExpression="LABEL_CODE" ReadOnly="True" HeaderText="Label"></asp:BoundColumn>
									<asp:BoundColumn DataField="STORE" SortExpression="STORE" ReadOnly="True" HeaderText="Store"></asp:BoundColumn>
									<asp:BoundColumn DataField="DISPOSITION_CODE" SortExpression="DISPOSITION_CODE" ReadOnly="True" HeaderText="Disposition"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Center" ForeColor="Black" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD class="note" align="center" colSpan="2">All information is Delphi Confidential</TD>
					</TR>
				</TABLE>
			</P>
			<P align="center"><asp:label id="lblNoRecordsFound" runat="server" CssClass="lblMessage" Width="50%" Visible="False"></asp:label></P>
			<P align="left">&nbsp;</P>
		</form>
	</body>
</HTML>
