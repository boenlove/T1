<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Delphi_GM_Cross_Reference.aspx.vb" Inherits="Plant23_BEC.Delphi_GM_Cross_Reference"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Delphi_GM_Cross_Reference</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P align="center"><FONT size="2"><STRONG>Delphi / GM Cross Reference</STRONG></FONT></P>
			<P align="center"><asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></P>
			<P align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
			<P align="center">
				<asp:hyperlink id="hpkReturntoMain" runat="server" Font-Bold="True" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink></P>
			<P align="center">
				<TABLE class="tblFormat" id="tbleAdminCodeChange" style="WIDTH: 464px; HEIGHT: 81px" cellSpacing="1"
					cellPadding="1" width="464" border="1">
					<TR>
						<TD style="WIDTH: 168px; HEIGHT: 15px">
							<P align="right">Delphi Part Number:</P>
						</TD>
						<TD style="HEIGHT: 15px"><asp:dropdownlist id="cmbDelphiPart" runat="server" Font-Size="8pt" Width="175px" Font-Names="Verdana"
								AutoPostBack="True"></asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 168px; HEIGHT: 21px">
							<P align="right">GM Part Number:</P>
						</TD>
						<TD style="HEIGHT: 21px">&nbsp;<asp:label id="lblGMPartNumber" runat="server"></asp:label>&nbsp;</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 168px; HEIGHT: 24px"></TD>
						<TD style="HEIGHT: 24px"></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 168px; HEIGHT: 15px">
							<P align="right">GM Part Number:</P>
						</TD>
						<TD style="HEIGHT: 15px">
							<asp:dropdownlist id="cmbGMPart" runat="server" Font-Size="8pt" Width="175px" Font-Names="Verdana"
								AutoPostBack="True"></asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 168px; HEIGHT: 16px">
							<P align="right">Delphi Part Number:</P>
						</TD>
						<TD style="HEIGHT: 16px">&nbsp;
							<asp:label id="lblDelphiPartNumber" runat="server"></asp:label>&nbsp;</TD>
					</TR>
				</TABLE>
			</P>
			<P align="center">
				<asp:Button id="btnFullList" runat="server" Text="Show Full List"></asp:Button></P>
			<P align="center">
				<TABLE id="tblResults" title="Results Table" style="WIDTH: 280px; HEIGHT: 160px" cellSpacing="0"
					cellPadding="0" align="center" border="0" runat="server">
					<TR>
						<TD style="WIDTH: 310px; HEIGHT: 13px" align="left" colSpan="1" rowSpan="1"><asp:label id="lblSortOrder" runat="server" ForeColor="Black" Font-Size="XX-Small"></asp:label></TD>
						<TD style="WIDTH: 210px; HEIGHT: 13px" align="right"><asp:label id="lblCount" runat="server" ForeColor="Black" Font-Size="XX-Small"></asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 520px; HEIGHT: 15px" colSpan="2"><asp:datagrid id="dgPartNumberCrossReference" runat="server" Font-Size="XX-Small" CellPadding="3"
								BackColor="White" BorderStyle="None" AutoGenerateColumns="False" Font-Names="Verdana" OnSortCommand="dgPartNumberCrossReference_SortCommand"
								AllowSorting="True" Width="290px">
								<FooterStyle ForeColor="Silver" BackColor="White"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="White"></SelectedItemStyle>
								<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
								<ItemStyle CssClass="dgItem"></ItemStyle>
								<HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" ForeColor="White" CssClass="dgHeader"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="PART_NBR" SortExpression="PART_NBR" HeaderText="Delphi Part Number"></asp:BoundColumn>
									<asp:BoundColumn DataField="CUSTOMER_PART_NBR" SortExpression="CUSTOMER_PART_NBR" HeaderText="GM Part Number"></asp:BoundColumn>
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
