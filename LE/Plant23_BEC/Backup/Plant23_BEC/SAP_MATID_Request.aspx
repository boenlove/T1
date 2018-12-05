<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SAP_MATID_Request.aspx.vb" Inherits="Plant23_BEC.SAP_MATID_Request" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>SAP_BEC_LabelRequest</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table2" style="Z-INDEX: 104; LEFT: 32px; WIDTH: 672px; POSITION: absolute; TOP: 424px; HEIGHT: 51px"
				cellSpacing="1" cellPadding="1" width="672" border="1">
				<TR>
					<TD><asp:label id="Label13" runat="server" Width="83px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True">Status</asp:label></TD>
					<TD><asp:label id="lblStatus" runat="server" Width="577px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Height="48px"></asp:label></TD>
				</TR>
				<TR>
					<TD></TD>
					<TD></TD>
				</TR>
			</TABLE>
			<asp:label id="lblDisplay" style="Z-INDEX: 108; LEFT: 272px; POSITION: absolute; TOP: 16px"
				runat="server" Width="328px" ForeColor="Red">Label</asp:label><asp:hyperlink id="hpkReturntoMain" style="Z-INDEX: 105; LEFT: 16px; POSITION: absolute; TOP: 40px"
				runat="server" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink><asp:label id="Label4" style="Z-INDEX: 102; LEFT: 272px; POSITION: absolute; TOP: 40px" runat="server"
				Width="272px" ForeColor="MidnightBlue" Font-Size="17pt" Font-Bold="True" Font-Names="Arial"> MATID Maintenance</asp:label>
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 32px; POSITION: absolute; TOP: 128px" cellSpacing="1"
				cellPadding="1" width="300" bgColor="azure" border="1">
				<TR>
					<TD><asp:label id="lblMATID" runat="server" Width="83px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial">MATID</asp:label></TD>
					<TD style="WIDTH: 134px"><asp:textbox id="txtMATID" runat="server" Width="96px" ForeColor="Red" Font-Size="10pt" Font-Bold="True"
							Font-Names="Arial" BackColor="Gainsboro" ReadOnly="True" Visible="False"></asp:textbox><asp:button id="btnModify_MATID" runat="server" Width="32px" ForeColor="Navy" Font-Size="8pt"
							Visible="False" Text="Go"></asp:button></TD>
					<TD><asp:label id="lblMATIDstatus" runat="server" Width="32px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" Visible="False">Status</asp:label><asp:dropdownlist id="ddlStatus" runat="server" Width="102px" ForeColor="Navy" Font-Size="8pt" Font-Names="Arial"
							BackColor="LightYellow" Visible="False" AutoPostBack="True">
							<asp:ListItem Value="Select..">Select..</asp:ListItem>
							<asp:ListItem Value="A">A - Active</asp:ListItem>
							<asp:ListItem Value="O">O - Obsolete</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD></TD>
					<TD></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD><asp:label id="Label3" runat="server" Width="128px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial">Part Number</asp:label></TD>
					<TD style="WIDTH: 134px"><asp:textbox id="txtPart_Nbr" runat="server" Width="152px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial" Visible="False"></asp:textbox></TD>
					<TD><asp:label id="Label7" runat="server" Width="136px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True">Pay Suffix</asp:label></TD>
					<TD><asp:label id="Label10" runat="server" Width="83px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial">Type of Label</asp:label></TD>
					<TD><asp:textbox id="txtSTD_FLAG" runat="server" Width="64px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial" Visible="False"></asp:textbox><asp:label id="lblCreateMATID" runat="server" Width="41px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" Font-Names="Arial" Visible="False">Y</asp:label></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 19px"><asp:dropdownlist id="cmbPart_Nbr" runat="server" Width="152px" ForeColor="Navy" Font-Size="8pt" Font-Names="Arial"
							BackColor="LightYellow" AutoPostBack="True"></asp:dropdownlist></TD>
					<TD style="WIDTH: 134px; HEIGHT: 19px"><asp:label id="lblDescription" runat="server" Width="144px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" Font-Names="Arial"></asp:label></TD>
					<TD style="HEIGHT: 19px"><asp:textbox id="txtPAY_SUFFIX" runat="server" Width="64px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" Font-Names="Arial"></asp:textbox></TD>
					<TD style="HEIGHT: 19px"><asp:dropdownlist id="cmbSTD_FLAG" runat="server" Width="64px" ForeColor="Navy" Font-Size="8pt" Font-Names="Arial"
							BackColor="LightYellow" AutoPostBack="True">
							<asp:ListItem></asp:ListItem>
							<asp:ListItem Value="I">I</asp:ListItem>
							<asp:ListItem Value="S">S</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD style="HEIGHT: 19px"></TD>
					<TD style="HEIGHT: 19px"></TD>
					<TD style="HEIGHT: 19px"></TD>
				</TR>
				<TR>
					<TD><asp:label id="Label8" runat="server" Width="144px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial">Destination (CNTR TYPE)</asp:label></TD>
					<TD style="WIDTH: 134px"><asp:label id="Label1" runat="server" Width="40px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial">Dept</asp:label></TD>
					<TD><asp:label id="lblSAP_Customer" runat="server" Width="120px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" Font-Names="Arial">Customer</asp:label></TD>
					<TD><asp:label id="Label14" runat="server" Width="120px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial">SAP Ship To</asp:label></TD>
					<TD><asp:label id="Label9" runat="server" Width="104px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial">Cust. Part No.</asp:label></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 16px"><asp:dropdownlist id="ddlDestination" runat="server" Width="80px" ForeColor="Navy" Font-Size="8pt"
							Font-Names="Arial" BackColor="LightYellow" AutoPostBack="True">
							<asp:ListItem></asp:ListItem>
							<asp:ListItem Value="I">I</asp:ListItem>
							<asp:ListItem Value="S">S</asp:ListItem>
						</asp:dropdownlist><asp:textbox id="txtDestination" runat="server" Width="64px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" Font-Names="Arial"></asp:textbox></TD>
					<TD style="WIDTH: 134px; HEIGHT: 16px"><asp:dropdownlist id="ddlDept" runat="server" Width="65px" ForeColor="Navy" Font-Size="8pt" Font-Names="Arial"
							BackColor="LightYellow" AutoPostBack="True">
							<asp:ListItem></asp:ListItem>
							<asp:ListItem Value="I">I</asp:ListItem>
							<asp:ListItem Value="S">S</asp:ListItem>
						</asp:dropdownlist><asp:textbox id="txtDept" runat="server" Width="56px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial" Visible="False"></asp:textbox></TD>
					<TD style="HEIGHT: 16px"><asp:textbox id="txtCUSTOMER_CODE" runat="server" Width="80px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" Font-Names="Arial" BackColor="LightYellow" MaxLength="5"></asp:textbox></TD>
					<TD style="HEIGHT: 16px"><asp:dropdownlist id="ddlSHIP_TO" runat="server" Width="104px" ForeColor="Navy" Font-Size="8pt" Font-Names="Arial"
							BackColor="LightYellow" AutoPostBack="True"></asp:dropdownlist></TD>
					<TD style="HEIGHT: 16px"><asp:dropdownlist id="ddlCUST_PART_NBR" runat="server" Width="128px" ForeColor="Navy" Font-Size="8pt"
							Font-Names="Arial" BackColor="LightYellow" AutoPostBack="True"></asp:dropdownlist></TD>
					<TD style="HEIGHT: 16px"></TD>
					<TD style="HEIGHT: 16px"></TD>
				</TR>
				<TR>
					<TD><asp:label id="Label5" runat="server" Width="83px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True">ECL</asp:label></TD>
					<TD style="WIDTH: 134px"><asp:label id="Label11" runat="server" Width="83px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True">Quantity</asp:label></TD>
					<TD><asp:label id="Label12" runat="server" Width="83px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True">Weight</asp:label></TD>
					<TD><asp:label id="Label6" runat="server" Width="104px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial">Package Code</asp:label></TD>
					<TD></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD><asp:textbox id="txtECL" runat="server" Width="64px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial" BackColor="LightYellow"></asp:textbox></TD>
					<TD style="WIDTH: 134px"><asp:textbox id="txtQUANTITY" runat="server" Width="64px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial" BackColor="LightYellow"></asp:textbox></TD>
					<TD><asp:textbox id="txtWEIGHT" runat="server" Width="64px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial" BackColor="LightYellow"></asp:textbox></TD>
					<TD><asp:dropdownlist id="ddlPackageCode" runat="server" Width="65px" ForeColor="Navy" Font-Size="8pt"
							Font-Names="Arial" BackColor="LightYellow" AutoPostBack="True">
							<asp:ListItem Value="Select..">Select..</asp:ListItem>
							<asp:ListItem Value="C">C</asp:ListItem>
							<asp:ListItem Value="4C">4C</asp:ListItem>
							<asp:ListItem Value="Y">Y</asp:ListItem>
							<asp:ListItem Value="Z">Z</asp:ListItem>
							<asp:ListItem Value="XY">XY</asp:ListItem>
						</asp:dropdownlist><asp:textbox id="txtPackageCode" runat="server" Width="40px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" Font-Names="Arial" Visible="False"></asp:textbox></TD>
					<TD><asp:label id="lblCUSTOMER_NAME" runat="server" Width="120px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" Font-Names="Arial" Visible="False">Customer Name</asp:label></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD><asp:button id="btnVerifyMATID" runat="server" Width="100px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" BackColor="Cornsilk" Text="Verify"></asp:button></TD>
					<TD style="WIDTH: 134px"></TD>
					<TD></TD>
					<TD></TD>
					<TD><asp:textbox id="txtCUST_PART_NBR" runat="server" Width="24px" ForeColor="Navy" Font-Size="8pt"
							Font-Bold="True" Font-Names="Arial" Visible="False"></asp:textbox><asp:textbox id="txtSHIP_TO" runat="server" Width="24px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial" Visible="False"></asp:textbox><asp:textbox id="txtSTATUS" runat="server" Width="24px" ForeColor="Navy" Font-Size="8pt" Font-Bold="True"
							Font-Names="Arial" Visible="False"></asp:textbox></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD><asp:button id="btnReset" runat="server" Width="100px" ForeColor="Navy" Font-Size="8pt" Text="Reset"></asp:button></TD>
					<TD style="WIDTH: 134px"><asp:button id="btnCreateMATID" runat="server" Width="104px" ForeColor="Navy" Font-Size="9pt"
							Font-Bold="True" BackColor="Cornsilk" Visible="False" Text="Generate MATID"></asp:button></TD>
					<TD><asp:button id="btnModify" runat="server" Width="100px" ForeColor="DarkBlue" Font-Size="9pt"
							Font-Bold="True" BackColor="White" Text="Edit MATID"></asp:button></TD>
					<TD><asp:button id="btnSaveMATID" runat="server" Width="100px" ForeColor="DarkBlue" Font-Size="9pt"
							Font-Bold="True" BackColor="LemonChiffon" Visible="False" Text="Save MATID"></asp:button></TD>
					<TD><asp:button id="Button1" runat="server" Width="100px" ForeColor="DarkBlue" Font-Size="9pt" Font-Bold="True"
							BackColor="LemonChiffon" Visible="False" Text="Del"></asp:button></TD>
					<TD></TD>
					<TD></TD>
				</TR>
			</TABLE>
			<asp:image id="Image1" style="Z-INDEX: 103; LEFT: 16px; POSITION: absolute; TOP: 8px" runat="server"
				Width="214px" Height="24px" ImageUrl="file:///C:\Inetpub\wwwroot\SAP_BEC_LabelRequest\Pictures\delphi_Logo.gif"></asp:image></form>
	</body>
</HTML>
