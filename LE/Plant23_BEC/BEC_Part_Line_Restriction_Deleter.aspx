<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_Part_Line_Restriction_Deleter.aspx.vb" Inherits="Plant23_BEC.BEC_Part_Line_Restriction_Deleter"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BEC_Part_Line_Restriction_Deleter</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<FORM id="Form1" method="post" runat="server">
			<FONT face="Verdana" size="1">
				<H1 align="center"><FONT size="1"></FONT></H1>
				<H1 align="center"><FONT size="1"></FONT></H1>
				<FONT size="1">
					<H1 align="center"><asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></H1>
				</FONT>
				<H1 align="center"><FONT size="2">BEC Part Line Restriction Deleter</FONT>
				</H1>
				<P align="center"><asp:label id="lblMessage" runat="server" CssClass="lblError"></asp:label></P>
			</FONT>
			<TABLE id="tblResults" title="Results Table" style="WIDTH: 280px; HEIGHT: 160px" cellSpacing="0"
				cellPadding="0" align="center" border="0" runat="server">
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
					<TD style="WIDTH: 520px; HEIGHT: 15px" colSpan="2"><asp:datagrid id="dgPartLineRestriction" runat="server" OnDeleteCommand="dgPartLineRestriction_Delete"
							PageSize="1" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="287px" Font-Names="Verdana"
							Font-Size="XX-Small">
							<SelectedItemStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="White"></SelectedItemStyle>
							<EditItemStyle Font-Names="LUKE"></EditItemStyle>
							<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
							<ItemStyle CssClass="dgItem"></ItemStyle>
							<HeaderStyle CssClass="dgHeader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="MACHINE_ID" SortExpression="MACHINE_ID" ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="MES_PART_ID" SortExpression="MES_PART_ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="MACHINE_NAME" SortExpression="MACHINE_NAME" ReadOnly="True" HeaderText="Machine"></asp:BoundColumn>
								<asp:BoundColumn DataField="PART_NBR" SortExpression="PART_NBR" HeaderText="Part Nbr">
									<ItemStyle Font-Size="XX-Small" Font-Names="Verdana"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CONTAINER_CODE" SortExpression="CONTAINER_CODE" ReadOnly="True" HeaderText="Container"></asp:BoundColumn>
								<asp:BoundColumn DataField="INSERT_USERID" SortExpression="INSERT_USERID" ReadOnly="True" HeaderText="UserId"></asp:BoundColumn>
								<asp:BoundColumn DataField="INSERT_TMSTM" SortExpression="INSERT_TMSTM" ReadOnly="True" HeaderText="TimeStamp"></asp:BoundColumn>
								<asp:ButtonColumn Text="Delete" CommandName="Delete"></asp:ButtonColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Center" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
					</TD>
				</TR>
				<TR>
					<TD class="note" align="center" colSpan="2" style="WIDTH: 520px">All information is 
						Delphi Confidential</TD>
				</TR>
			</TABLE>
			<P align="center">
				<asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox></P>
			<P align="left">&nbsp;</P>
		</FORM>
	</body>
</HTML>
