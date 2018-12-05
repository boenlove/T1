<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SAP_BEC_LabelRequest_MATID.aspx.vb" Inherits="Plant23_BEC.SAP_BEC_LabelRequest_MATID" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>SAP_BEC_LabelRequest</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bgColor="#f5f5f2" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:label id="lblStatus" style="Z-INDEX: 103; LEFT: 40px; POSITION: absolute; TOP: 448px"
				runat="server" Font-Size="8pt" ForeColor="Navy" Font-Bold="True" Height="40px" Width="623px"></asp:label><asp:hyperlink id="hpkReturntoMain" style="Z-INDEX: 106; LEFT: 16px; POSITION: absolute; TOP: 40px"
				runat="server" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink><asp:label id="Label4" style="Z-INDEX: 102; LEFT: 264px; POSITION: absolute; TOP: 40px" runat="server"
				Font-Size="17pt" ForeColor="SteelBlue" Font-Bold="True" Width="320px">Label Request from SAP/MES</asp:label>
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 16px; WIDTH: 712px; POSITION: absolute; TOP: 120px; HEIGHT: 155px"
				cellSpacing="1" cellPadding="1" width="712" bgColor="ivory" border="0">
				<TR>
					<TD style="WIDTH: 126px"><asp:label id="Label2" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px" Width="88px"
							BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial">MATID</asp:label></TD>
					<TD style="WIDTH: 170px"><asp:textbox id="txtMATID" runat="server" Font-Size="8pt" ForeColor="Navy" Width="104px" BackColor="LightYellow"
							Font-Names="Arial"></asp:textbox></TD>
					<TD><asp:label id="txtMATID_Status" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px"
							Width="48px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial"
							Visible="False"></asp:label></TD>
					<TD style="WIDTH: 53px"><asp:label id="lblECL" runat="server" Font-Size="8pt" ForeColor="Navy" Font-Bold="True" Height="16px"
							Width="46px" BackColor="AliceBlue" BorderWidth="0px" Visible="False">ECL:</asp:label></TD>
					<TD><asp:label id="lblPackageCode" runat="server" Font-Size="8pt" ForeColor="Navy" Font-Bold="True"
							Height="16px" Width="80px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid"
							Font-Names="Arial" Visible="False">Package Code</asp:label></TD>
					<TD style="WIDTH: 181px"><asp:label id="lblDestination" runat="server" Font-Size="8pt" ForeColor="Navy" Font-Bold="True"
							Height="16px" Width="72px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial"
							Visible="False">Destination</asp:label></TD>
					<TD><asp:label id="lblSTD_FLAG" runat="server" Font-Size="8pt" ForeColor="Navy" Font-Bold="True"
							Height="16px" Width="32px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid"
							Font-Names="Arial" Visible="False">Type</asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 126px"><asp:label id="Label3" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px" Width="72px"
							BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial"> Part Number</asp:label></TD>
					<TD style="WIDTH: 170px"><asp:textbox id="txtPart_Nbr" runat="server" Font-Size="8pt" ForeColor="Navy" Width="104px" BackColor="LightYellow"
							Font-Names="Arial"></asp:textbox><asp:button id="btnVerifyMATID" runat="server" Font-Size="8pt" ForeColor="Navy" Width="40px"
							Text="Verify"></asp:button></TD>
					<TD><asp:label id="lblDescription" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px"
							Width="128px" BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial"></asp:label></TD>
					<TD style="WIDTH: 53px"><asp:label id="txtECL" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px" Width="48px"
							BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial" Visible="False"></asp:label></TD>
					<TD><asp:label id="txtPackageCode" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px"
							Width="96px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial"
							Visible="False"></asp:label></TD>
					<TD style="WIDTH: 181px"><asp:label id="txtDestination" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px"
							Width="66px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial" Visible="False"></asp:label></TD>
					<TD><asp:dropdownlist id="cmbSTD_FLAG" runat="server" Font-Size="8pt" ForeColor="Navy" Width="64px" BackColor="LightYellow"
							Font-Names="Arial" Visible="False" AutoPostBack="True">
							<asp:ListItem></asp:ListItem>
							<asp:ListItem Value="I">I</asp:ListItem>
							<asp:ListItem Value="S">S</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 126px; HEIGHT: 21px"></TD>
					<TD style="WIDTH: 170px; HEIGHT: 21px"></TD>
					<TD style="HEIGHT: 21px"><asp:label id="txtDept" runat="server" Font-Size="8pt" ForeColor="Silver" Height="24px" Width="40px"
							BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial" Visible="False"></asp:label></TD>
					<TD style="WIDTH: 53px; HEIGHT: 21px"><asp:label id="txtQuantity" runat="server" Font-Size="8pt" ForeColor="Silver" Height="24px"
							Width="56px" BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial" Visible="False"></asp:label></TD>
					<TD style="HEIGHT: 21px"><asp:label id="txtWeight" runat="server" Font-Size="8pt" ForeColor="Silver" Height="24px" Width="56px"
							BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial" Visible="False"></asp:label></TD>
					<TD style="WIDTH: 181px; HEIGHT: 21px"><asp:label id="txtPAY_SUFFIX" runat="server" Font-Size="8pt" ForeColor="Silver" Height="24px"
							Width="56px" BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial" Visible="False"></asp:label></TD>
					<TD style="HEIGHT: 21px"><asp:label id="txtSTD_FLAG" runat="server" Font-Size="8pt" ForeColor="Silver" Height="24px"
							Width="32px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial" Visible="False"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 126px"><asp:label id="lblSAP_Customer" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px"
							Width="80px" BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial">Customer Name</asp:label></TD>
					<TD style="WIDTH: 170px"><asp:dropdownlist id="cmbSAP_Customer" runat="server" Font-Size="8pt" ForeColor="Navy" Width="152px"
							BackColor="LightYellow" Font-Names="Arial" AutoPostBack="True"></asp:dropdownlist></TD>
					<TD><asp:label id="lblCUST_ADDRESS" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px"
							Width="128px" BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial"></asp:label></TD>
					<TD style="WIDTH: 53px"><asp:label id="lblCUST_CITY" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px"
							Width="48px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial"></asp:label></TD>
					<TD><asp:label id="lblCUST_STATE" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px"
							Width="88px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial"></asp:label></TD>
					<TD style="WIDTH: 181px"><asp:label id="lblSHIP_TO_H" runat="server" Font-Size="8pt" ForeColor="Navy" Font-Bold="True"
							Height="20px" Width="48px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial"
							Visible="False">Ship To:</asp:label><asp:label id="lblSHIP_TO" runat="server" Font-Size="8pt" ForeColor="Navy" Height="20px" Width="84px"
							BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial"></asp:label></TD>
					<TD><asp:label id="lblPlant_Dock_Line_1_H" runat="server" Font-Size="8pt" ForeColor="Navy" Font-Bold="True"
							Height="20px" Width="72px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid"
							Font-Names="Arial" Visible="False">Mat H Code:</asp:label><asp:label id="lblPlant_Dock_Line_1" runat="server" Font-Size="8pt" ForeColor="Navy" Height="20px"
							Width="84px" BackColor="Transparent" BorderWidth="1px" BorderStyle="Solid" Font-Names="Arial" Visible="False"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 126px; HEIGHT: 19px"><asp:label id="lblNoOfLabels" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px"
							Width="88px" BackColor="Transparent" BorderWidth="1px" BorderStyle="None" Font-Names="Arial">Number of Labels</asp:label></TD>
					<TD style="WIDTH: 170px; HEIGHT: 19px"><asp:textbox id="txtNoOfLabels" runat="server" Font-Size="8pt" ForeColor="Navy" Width="104px"
							BackColor="LightYellow" Font-Names="Arial"></asp:textbox></TD>
					<TD style="HEIGHT: 19px"></TD>
					<TD style="WIDTH: 53px; HEIGHT: 19px"></TD>
					<TD style="HEIGHT: 19px"></TD>
					<TD style="WIDTH: 181px; HEIGHT: 19px"><asp:label id="lblFoundShipTo" runat="server" Font-Size="8pt" ForeColor="Navy" Height="24px"
							Width="32px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial" Visible="False"></asp:label></TD>
					<TD style="HEIGHT: 19px"><asp:label id="lblDEST_LOC_H" runat="server" Font-Size="8pt" ForeColor="Navy" Font-Bold="True"
							Height="20px" Width="72px" BackColor="Transparent" BorderWidth="0px" BorderStyle="Solid" Font-Names="Arial"
							Visible="False">Dest. Code:</asp:label><asp:label id="lblDEST_LOC" runat="server" Font-Size="8pt" ForeColor="Navy" Height="20px" Width="84px"
							BackColor="Transparent" BorderWidth="1px" BorderStyle="Solid" Font-Names="Arial" Visible="False"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 126px"><asp:button id="cmdReset" runat="server" Font-Size="8pt" ForeColor="Navy" Width="88px" Text="Reset"></asp:button></TD>
					<TD style="WIDTH: 170px"><asp:button id="btnCreateLabelsFromSAP" runat="server" Font-Size="8pt" ForeColor="Navy" Width="88px"
							Text="Generate Labels"></asp:button></TD>
					<TD></TD>
					<TD style="WIDTH: 53px"></TD>
					<TD></TD>
					<TD style="WIDTH: 181px"></TD>
					<TD></TD>
				</TR>
			</TABLE>
			<asp:listbox id="lstbxInsertResults" style="Z-INDEX: 104; LEFT: 32px; POSITION: absolute; TOP: 312px"
				runat="server" Font-Size="8pt" ForeColor="Navy" Height="128px" Width="590px"></asp:listbox><asp:image id="Image1" style="Z-INDEX: 105; LEFT: 16px; POSITION: absolute; TOP: 8px" runat="server"
				Height="24px" Width="214px" ImageUrl="file:///C:\Inetpub\wwwroot\SAP_BEC_LabelRequest\Pictures\delphi_Logo.gif"></asp:image><asp:textbox id="txtSecurityCheck" style="Z-INDEX: 107; LEFT: 512px; POSITION: absolute; TOP: 8px"
				runat="server" ForeColor="Transparent" BorderStyle="None" Visible="False"></asp:textbox><asp:label id="lblDisplay" style="Z-INDEX: 108; LEFT: 248px; POSITION: absolute; TOP: 16px"
				runat="server" ForeColor="Red" Width="248px"></asp:label></form>
	</body>
</HTML>
