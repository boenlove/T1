<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ExportSerialHistory.aspx.vb" Inherits="Plant23_BEC.Export"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Export</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P>
				<asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label></P>
			<P>
			</P>
			<asp:datagrid id="dgStandardPackHistory" runat="server" AutoGenerateColumns="False" BorderStyle="None"
				BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="287px">
				<FooterStyle ForeColor="Black" BackColor="Silver"></FooterStyle>
				<SelectedItemStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" HorizontalAlign="Center"
					ForeColor="Black" Width="40px"></SelectedItemStyle>
				<EditItemStyle Font-Size="Smaller" Font-Names="Verdana" HorizontalAlign="Center" Width="40px"></EditItemStyle>
				<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
				<ItemStyle Width="40px" CssClass="dgItem"></ItemStyle>
				<HeaderStyle CssClass="dgHeader"></HeaderStyle>
				<Columns>
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
			</asp:datagrid>
		</form>
	</body>
</HTML>
