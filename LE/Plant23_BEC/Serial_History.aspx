<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Serial_History.aspx.vb" Inherits="Plant23_BEC.Serial_History"%>
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
			<P>&nbsp;</P>
			<P align="center"><STRONG><FONT size="2">BEC Serial Number History</FONT></STRONG></P>
			<P align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
			<P align="center"><asp:label id="lblMessage" runat="server" CssClass="lblerror" Font-Size="8pt"></asp:label></P>
			<P align="center"><asp:label id="lblSortOrder" runat="server" Font-Size="XX-Small" ForeColor="Red" Font-Bold="True"
					Visible="False"></asp:label><br>
				<TABLE id="tblResults" title="Results Table" style="WIDTH: 304px; HEIGHT: 294px" cellSpacing="0"
					cellPadding="0" align="center" border="0" runat="server">
					<TR>
						<TD style="WIDTH: 160px" align="right" colSpan="2"><asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
					</TR>                    
					<TR>
						<TD style="WIDTH: 160px" align="center" colSpan="2"><asp:datagrid id="dgSUB_STANDARD_PACK" runat="server" Visible="False" OnSortCommand="dgSUB_STANDARD_PACK_SortCommand"
								AllowSorting="True" AutoGenerateColumns="False" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" Width="402px">
								<SelectedItemStyle Wrap="False"></SelectedItemStyle>
								<EditItemStyle Wrap="False" HorizontalAlign="Right"></EditItemStyle>
								<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
								<ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="dgItem" VerticalAlign="Middle"></ItemStyle>
								<HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="SUB_STANDARD_PACK_ID" SortExpression="SUB_STANDARD_PACK_ID" HeaderText="Sub_Std_Pck">
										<HeaderStyle Width="15px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="INSERT_TMSTM" SortExpression="INSERT_TMSTM" HeaderText="Time_Stamp">
										<HeaderStyle Width="15px"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
							</asp:datagrid><asp:datagrid id="dgPRODUCT_ID" runat="server" Visible="False" OnSortCommand="dgPRODUCT_ID_SortCommand"
								AllowSorting="True" AutoGenerateColumns="False" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
								CellPadding="2" Width="402px">
								<SelectedItemStyle Wrap="False"></SelectedItemStyle>
								<EditItemStyle Wrap="False" HorizontalAlign="Right"></EditItemStyle>
								<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
								<ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="dgItem" VerticalAlign="Middle"></ItemStyle>
								<HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
								<Columns>
									<asp:HyperLinkColumn Text="Lot_Content" DataNavigateUrlField="Lot_Content" DataNavigateUrlFormatString="javascript:var w=window.open('{0}','','height=250,width=650,resizable=yes,scrollbars=yes,status=no,menubar=no');"></asp:HyperLinkColumn>
									<asp:BoundColumn DataField="SERIAL_NBR" SortExpression="SERIAL_NBR" HeaderText="Serial Number">
										<HeaderStyle Width="15px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PRODUCED_TMSTM" SortExpression="PRODUCED_TMSTM" HeaderText="Time_Stamp">
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
				<asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtStandardPackId" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtJob" runat="server" Visible="False"></asp:textbox>
		</form>
		<P>&nbsp;</P>
	</body>
</HTML>
