<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_Part_Line_Restriction.aspx.vb" Inherits="Plant23_BEC.BEC_Part_Line_Restriction"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BEC_Part_Line_Restriction</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<FORM id="Form1" method="post" runat="server">
			<H1 align="center"><FONT face="Verdana" size="1"></FONT></H1>
			<FONT face="Verdana" size="1">
				<H1 align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></H1>
				<P align="center"><asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></P>
			</FONT>
			<P align="center">
				<TABLE id="tblPartLineRestriction" title="Plant 23 BEC Part Line Restriction" style="WIDTH: 442px; HEIGHT: 104px"
					cellSpacing="0" cellPadding="0" width="442" align="center" bgColor="whitesmoke" border="0">
					<TR>
						<TD class="tbltitle" style="WIDTH: 553px; HEIGHT: 13px" bgColor="#ffffff" colSpan="2">
							<P align="right">
								<asp:hyperlink id="hpkReturntoMain" runat="server" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink></P>
						</TD>
					</TR>
					<TR>
						<TD class="tbltitle" style="WIDTH: 553px" bgColor="#ffffff" colSpan="2">KIOSK 
							Database Manager - BEC Part Line Restriction Loader</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 129px; HEIGHT: 27px" align="right" colSpan="1" rowSpan="1"><asp:label id="lblPartNumber" runat="server" Font-Names="Verdana">Part Number:</asp:label></TD>
						<TD style="WIDTH: 305px; HEIGHT: 27px">&nbsp;
							<asp:textbox id="txtPartNumber" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="95px"
								MaxLength="8"></asp:textbox></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 129px; HEIGHT: 23px">
							<P align="right"><asp:label id="lblMachineName" runat="server" Font-Names="Verdana">Machine Name:</asp:label></P>
						</TD>
						<TD style="WIDTH: 305px; HEIGHT: 23px">&nbsp;
							<asp:dropdownlist id="cmbMachineName" runat="server" Font-Names="Verdana" Font-Size="XX-Small"></asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 129px; HEIGHT: 24px">
							<P align="right"><asp:label id="lblContainer" runat="server" Font-Names="Verdana">Container:</asp:label></P>
						</TD>
						<TD style="WIDTH: 305px; HEIGHT: 24px">&nbsp;
							<asp:dropdownlist id="cmbContainer" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Width="52px">
								<asp:ListItem Value="AR">AR</asp:ListItem>
								<asp:ListItem Value="FT">FT</asp:ListItem>
								<asp:ListItem Value="FW">FW</asp:ListItem>
								<asp:ListItem Value="HU">HU</asp:ListItem>
								<asp:ListItem Value="IP">IP</asp:ListItem>
								<asp:ListItem Value="JI">JI</asp:ListItem>
								<asp:ListItem Value="OT">OT</asp:ListItem>
								<asp:ListItem Value="PE">PE</asp:ListItem>
								<asp:ListItem Value="SL">SL</asp:ListItem>
								<asp:ListItem Value="TL">TL</asp:ListItem>
							</asp:dropdownlist>&nbsp;&nbsp;</TD>
					</TR>
				</TABLE>
			</P>
			<P align="center"><asp:button id="btnLoadData" runat="server" Font-Names="Verdana" Font-Size="XX-Small" Font-Bold="True"
					Text="Load Data"></asp:button>&nbsp;&nbsp;&nbsp;</P>
			<P align="center"><asp:textbox id="txtMesPartId" runat="server" Enabled="False" Visible="False"></asp:textbox><asp:button id="Button1" runat="server" Text="Button" Visible="False"></asp:button></P>
			<P align="center"><asp:label id="lblRecordsInserted" runat="server" Font-Names="Verdana" Font-Size="XX-Small"
					Font-Bold="True" Visible="False"></asp:label></P>
			<TABLE id="tblResults" title="Results Table" style="WIDTH: 280px; HEIGHT: 160px" cellSpacing="0"
				cellPadding="0" align="center" border="0" runat="server">
				<TR>
					<TD style="WIDTH: 310px; HEIGHT: 12px" align="left" colSpan="1" rowSpan="1"></TD>
					<TD style="WIDTH: 210px; HEIGHT: 12px" align="right"><asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 520px; HEIGHT: 15px" colSpan="2">
						<DIV align="center"><asp:datagrid id="dgPartLineRestriction" runat="server" Font-Names="Verdana" Width="280px" Font-Size="XX-Small"
								Visible="False" OnPageIndexChanged="dgPartLineRestriction_PageChange" GridLines="Vertical" CellPadding="3"
								BackColor="White" BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False">
								<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
								<ItemStyle CssClass="dgItem"></ItemStyle>
								<HeaderStyle CssClass="dgHeader"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="MACHINE_NAME" SortExpression="MACHINE_NAME" ReadOnly="True" HeaderText="Machine"
										DataFormatString="{0:d}"></asp:BoundColumn>
									<asp:BoundColumn DataField="PART_NBR" SortExpression="PART_NBR" HeaderText="Part Nbr">
										<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CONTAINER_CODE" SortExpression="CONTAINER_CODE" ReadOnly="True" HeaderText="Container"></asp:BoundColumn>
									<asp:BoundColumn DataField="INSERT_TMSTM" SortExpression="INSERT_TMSTM" ReadOnly="True" HeaderText="TimeStamp"></asp:BoundColumn>
									<asp:BoundColumn DataField="INSERT_USERID" SortExpression="INSERT_USERID" ReadOnly="True" HeaderText="UserId"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Center" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></DIV>
					</TD>
				</TR>
				<TR>
					<TD class="note" align="center" colSpan="2" style="WIDTH: 520px">All information is 
						Delphi Confidential</TD>
				</TR>
			</TABLE>
			<P>
				<asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox></P>
			<P>&nbsp;</P>
		</FORM>
	</body>
</HTML>
