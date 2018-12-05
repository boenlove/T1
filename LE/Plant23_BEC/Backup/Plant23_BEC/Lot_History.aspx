<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Lot_History.aspx.vb" Inherits="Plant23_BEC.Lot_History"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Lot_History</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P align="center"><STRONG><FONT size="2">BEC&nbsp;Lot Number History</FONT></STRONG></P>
			<P align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
			<P align="center"><asp:label id="lblMessage" runat="server" CssClass="lblerror" Font-Size="8pt"></asp:label></P>
			<P align="center"><asp:label id="lblSortOrder" runat="server" Font-Size="XX-Small" Visible="False" Font-Bold="True"
					ForeColor="Red"></asp:label><BR>
				<TABLE id="tblResults" title="Results Table" style="WIDTH: 241px; HEIGHT: 186px" cellSpacing="0"
					cellPadding="0" align="center" border="0" runat="server">
					<TR>
						<TD style="WIDTH: 160px" align="right" colSpan="2"><asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 160px" align="center" colSpan="2"><asp:datagrid id="dgLOT" runat="server" Visible="False" CellPadding="2" BorderWidth="1px" BorderStyle="Solid"
								BorderColor="Silver" AutoGenerateColumns="False" AllowSorting="True" OnSortCommand="dgLOT_SortCommand" Width="240px">
								<SelectedItemStyle Wrap="False"></SelectedItemStyle>
								<EditItemStyle Wrap="False" HorizontalAlign="Right"></EditItemStyle>
								<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
								<ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="dgItem" VerticalAlign="Middle"></ItemStyle>
								<HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="LOT_SERIAL_NBR" SortExpression="LOT_SERIAL_NBR" HeaderText="Lot Serial Nbr">
										<HeaderStyle Width="15px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="LOT_TMSTM" SortExpression="LOT_TMSTM" HeaderText="Time_Stamp">
										<HeaderStyle Width="15px"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD class="note" style="WIDTH: 160px" align="center" colSpan="2">All information is 
							Delphi Confidential</TD>
					</TR>
				</TABLE>
				<asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtProductId" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtJob" runat="server" Visible="False"></asp:textbox></P>
		</form>
	</body>
</HTML>
