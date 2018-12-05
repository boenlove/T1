<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SAP_MATID_Inquiry.aspx.vb" Inherits="Plant23_BEC.SAP_MATID_Inquiry" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>MATID_Inquiry</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:image id="Image1" style="Z-INDEX: 101; LEFT: 16px; POSITION: absolute; TOP: 8px" runat="server"
				ImageUrl="file:///C:\Inetpub\wwwroot\SAP_BEC_LabelRequest\Pictures\delphi_Logo.gif" Width="214px"
				Height="24px"></asp:image><asp:label id="lblAndOr" style="Z-INDEX: 126; LEFT: 560px; POSITION: absolute; TOP: 136px"
				runat="server" Width="48px" Height="16px" BorderWidth="1px" BorderStyle="None" Visible="False" BackColor="Transparent"
				Font-Names="Arial" ForeColor="Navy" Font-Size="8pt"></asp:label><asp:dropdownlist id="ddlAndOr" style="Z-INDEX: 125; LEFT: 504px; POSITION: absolute; TOP: 120px"
				runat="server" Width="49px" BackColor="LightYellow" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" AutoPostBack="True">
				<asp:ListItem></asp:ListItem>
				<asp:ListItem Value="and">and</asp:ListItem>
				<asp:ListItem Value="or">or</asp:ListItem>
			</asp:dropdownlist><asp:label id="lblPackageCode" style="Z-INDEX: 123; LEFT: 712px; POSITION: absolute; TOP: 112px"
				runat="server" Width="32px" Height="24px" BorderWidth="1px" BorderStyle="None" Visible="False" BackColor="Transparent"
				Font-Names="Arial" ForeColor="Navy" Font-Size="8pt"></asp:label><asp:label id="Label6" style="Z-INDEX: 122; LEFT: 560px; POSITION: absolute; TOP: 120px" runat="server"
				Width="64px" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" Font-Bold="True">Package</asp:label><asp:dropdownlist id="ddlPackageCode" style="Z-INDEX: 121; LEFT: 616px; POSITION: absolute; TOP: 120px"
				runat="server" Width="65px" BackColor="LightYellow" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" AutoPostBack="True">
				<asp:ListItem Value="..">..</asp:ListItem>
				<asp:ListItem Value="C">C</asp:ListItem>
				<asp:ListItem Value="4C">4C</asp:ListItem>
				<asp:ListItem Value="Y">Y</asp:ListItem>
				<asp:ListItem Value="Z">Z</asp:ListItem>
				<asp:ListItem Value="BU">BU</asp:ListItem>
                <asp:ListItem Value="UC">UC</asp:ListItem>
                <asp:ListItem Value="LE">LE</asp:ListItem>
			</asp:dropdownlist><asp:label id="lblSTD_FLAG" style="Z-INDEX: 120; LEFT: 424px; POSITION: absolute; TOP: 136px"
				runat="server" Width="48px" Height="16px" BorderWidth="1px" BorderStyle="None" Visible="False" BackColor="Transparent"
				Font-Names="Arial" ForeColor="Navy" Font-Size="8pt"></asp:label><asp:dropdownlist id="cmbSTD_FLAG" style="Z-INDEX: 119; LEFT: 424px; POSITION: absolute; TOP: 120px"
				runat="server" Width="74px" BackColor="LightYellow" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" AutoPostBack="True">
				<asp:ListItem Value="..">..</asp:ListItem>
				<asp:ListItem Value="I">I</asp:ListItem>
				<asp:ListItem Value="S">S</asp:ListItem>
			</asp:dropdownlist><asp:label id="Label5" style="Z-INDEX: 118; LEFT: 360px; POSITION: absolute; TOP: 120px" runat="server"
				Width="56px" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" Font-Bold="True">orType</asp:label><asp:textbox id="Textbox1" style="Z-INDEX: 112; LEFT: 456px; POSITION: absolute; TOP: 80px" runat="server"
				Width="72px" BackColor="LightGoldenrodYellow" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"></asp:textbox><asp:hyperlink id="hpkReturntoMain" style="Z-INDEX: 117; LEFT: 16px; POSITION: absolute; TOP: 40px"
				runat="server" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink><asp:button id="btnModify_MATID" style="Z-INDEX: 116; LEFT: 496px; POSITION: absolute; TOP: 80px"
				runat="server" Width="32px" ForeColor="Navy" Font-Size="8pt" Text="Go"></asp:button><asp:label id="Label2" style="Z-INDEX: 114; LEFT: 360px; POSITION: absolute; TOP: 88px" runat="server"
				Width="56px" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" Font-Bold="True">or MATID</asp:label><asp:textbox id="txtMATID" style="Z-INDEX: 113; LEFT: 424px; POSITION: absolute; TOP: 80px" runat="server"
				Width="72px" BackColor="LightGoldenrodYellow" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"></asp:textbox><asp:label id="lblShipTo" style="Z-INDEX: 111; LEFT: 288px; POSITION: absolute; TOP: 120px"
				runat="server" Width="80px" Height="24px" BorderWidth="1px" BorderStyle="None" Visible="False" BackColor="Transparent" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt">ShipTo</asp:label><asp:dropdownlist id="cmbShipTo" style="Z-INDEX: 110; LEFT: 136px; POSITION: absolute; TOP: 120px"
				runat="server" Width="152px" BackColor="LightYellow" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" AutoPostBack="True"></asp:dropdownlist><asp:label id="Label3" style="Z-INDEX: 109; LEFT: 24px; POSITION: absolute; TOP: 120px" runat="server"
				Width="104px" Height="24px" BorderWidth="1px" BorderStyle="None" BackColor="Transparent" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"> or SAP Ship To</asp:label><asp:label id="lblMessage" style="Z-INDEX: 108; LEFT: 24px; POSITION: absolute; TOP: 368px"
				runat="server" Width="680px" Height="48px" BorderWidth="1px" BorderStyle="None" BackColor="Transparent" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt"></asp:label><asp:label id="Label1" style="Z-INDEX: 107; LEFT: 496px; POSITION: absolute; TOP: 24px" runat="server"
				Width="80px" Height="24px" BorderWidth="1px" BorderStyle="None" Visible="False" BackColor="Transparent" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt"></asp:label><asp:label id="lblCount" style="Z-INDEX: 106; LEFT: 608px; POSITION: absolute; TOP: 24px" runat="server"
				Width="80px" Height="24px" BorderWidth="1px" BorderStyle="None" Visible="False" BackColor="Transparent" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt"></asp:label><asp:label id="lblPart_Nbr" style="Z-INDEX: 105; LEFT: 288px; POSITION: absolute; TOP: 88px"
				runat="server" Width="64px" Height="24px" BorderWidth="1px" BorderStyle="None" Visible="False" BackColor="Transparent" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt">Part Number</asp:label><asp:datagrid id="dgMATID_Grid" style="Z-INDEX: 104; LEFT: 8px; POSITION: absolute; TOP: 176px"
				runat="server" BorderWidth="1px" BorderStyle="None" BackColor="FloralWhite" Font-Names="Arial" Font-Size="9pt" AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3">
				<FooterStyle Font-Bold="True" ForeColor="Navy" BackColor="Silver"></FooterStyle>
				<SelectedItemStyle Font-Size="Smaller" Font-Names="Verdana" Font-Bold="True" HorizontalAlign="Center"
					ForeColor="Black" Width="40px"></SelectedItemStyle>
				<EditItemStyle Font-Size="Smaller" Font-Names="Verdana" HorizontalAlign="Center" Width="40px"></EditItemStyle>
				<AlternatingItemStyle Font-Size="9pt" Font-Names="Arial" CssClass="dgAltItem" BackColor="Linen"></AlternatingItemStyle>
				<ItemStyle Width="40px" CssClass="dgItem"></ItemStyle>
				<HeaderStyle Font-Size="8pt" Font-Bold="True" CssClass="dgHeader" BackColor="Wheat"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="PART_NBR" HeaderText="Part Number"></asp:BoundColumn>
					<asp:BoundColumn DataField="CUSTOMER_PART_NBR" HeaderText="Cust Part Nbr"></asp:BoundColumn>
					<asp:BoundColumn DataField="MATID" HeaderText="MAT ID"></asp:BoundColumn>
					<asp:BoundColumn DataField="DESTINATION" HeaderText="Destination"></asp:BoundColumn>
					<asp:BoundColumn DataField="SHIP_TO" HeaderText="SAP Ship To"></asp:BoundColumn>
					<asp:BoundColumn DataField="ECL" HeaderText="ECL"></asp:BoundColumn>
					<asp:BoundColumn DataField="PACKAGE_CODE" HeaderText="Package Code"></asp:BoundColumn>
					<asp:BoundColumn DataField="QUANTITY" HeaderText="Quantity"></asp:BoundColumn>
					<asp:BoundColumn DataField="STD_FLAG" HeaderText="Type"></asp:BoundColumn>
					<asp:BoundColumn DataField="DEPT" HeaderText="Dept"></asp:BoundColumn>
					<asp:BoundColumn DataField="STATUS" HeaderText="Status"></asp:BoundColumn>
					<asp:BoundColumn DataField="WEIGHT" HeaderText="Weight"></asp:BoundColumn>
				</Columns>
				<PagerStyle HorizontalAlign="Center" ForeColor="Black" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
			</asp:datagrid><asp:label id="lblSAP_Customer" style="Z-INDEX: 102; LEFT: 24px; POSITION: absolute; TOP: 88px"
				runat="server" Width="112px" Height="24px" BorderWidth="1px" BorderStyle="None" BackColor="Transparent"
				Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" Font-Bold="True">Select Part Number</asp:label><asp:label id="Label4" style="Z-INDEX: 100; LEFT: 264px; POSITION: absolute; TOP: 40px" runat="server"
				Width="184px" ForeColor="SteelBlue" Font-Size="17pt" Font-Bold="True">MATID Inquiry</asp:label><asp:dropdownlist id="cmbPart_Nbr" style="Z-INDEX: 103; LEFT: 136px; POSITION: absolute; TOP: 80px"
				runat="server" Width="152px" BackColor="LightYellow" Font-Names="Arial" ForeColor="Navy" Font-Size="8pt" AutoPostBack="True"></asp:dropdownlist></form>
	</body>
</HTML>
