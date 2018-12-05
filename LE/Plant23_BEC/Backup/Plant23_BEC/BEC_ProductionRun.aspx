<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_ProductionRun.aspx.vb" Inherits="Plant23_BEC.BEC_ProductionRun"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BEC_ProductionRun</title>
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
			<P align="center">
				<TABLE class="tblFormat" id="tblProductionRun" title="BEC Production Run" cellSpacing="0"
					cellPadding="3" width="450" align="center" border="0">
					<TR>
						<TD class="tbltitle" style="WIDTH: 449px" bgColor="#ffffff" colSpan="2">
							<P align="right">
								<asp:hyperlink id="hpkReturntoMain" runat="server" NavigateUrl="Index.aspx">Return to Main</asp:hyperlink></P>
						</TD>
					</TR>
					<TR>
						<TD class="tbltitle" style="WIDTH: 449px" bgColor="#ffffff" colSpan="2">KIOSK 
							Database Manager - Entry Form</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 449px; HEIGHT: 19px" align="center" background="../Images/Divider.jpg"
							colSpan="2">&nbsp;</TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 21px" align="right"><asp:requiredfieldvalidator id="Requiredfieldvalidator10" runat="server" Width="9px" ControlToValidate="txtPartNumber"
								Display="Dynamic" ErrorMessage="Part Number Required">*</asp:requiredfieldvalidator><asp:label id="Label10" runat="server">Dept:</asp:label></TD>
						<TD style="WIDTH: 309px; HEIGHT: 21px"><asp:dropdownlist id="cmbDept" runat="server" Font-Size="8pt">
								<asp:ListItem Value="1125">2341</asp:ListItem>
								<asp:ListItem Value="1126">2342</asp:ListItem>
								<asp:ListItem Value="1127">2343</asp:ListItem>
							</asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD align="right"><asp:requiredfieldvalidator id="Requiredfieldvalidator9" runat="server" Width="9px" ControlToValidate="txtPartNumber"
								Display="Dynamic" ErrorMessage="Part Number Required">*</asp:requiredfieldvalidator><asp:label id="Label9" runat="server">Part Number:</asp:label></TD>
						<TD style="WIDTH: 309px"><asp:textbox id="txtPartNumber" runat="server" Width="112px" Font-Size="8pt" MaxLength="8"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right"><asp:requiredfieldvalidator id="Requiredfieldvalidator8" runat="server" Width="8px" ControlToValidate="txtECL"
								Display="Dynamic" ErrorMessage="ECL Required">*</asp:requiredfieldvalidator><asp:label id="Label8" runat="server" Width="27px">ECL:</asp:label></TD>
						<TD style="WIDTH: 309px"><asp:textbox id="txtECL" runat="server" Width="40px" Font-Size="8pt"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right"><asp:comparevalidator id="Comparevalidator3" runat="server" Width="8px" ControlToValidate="txtStdPack"
								ErrorMessage="Standard Pack Must Be Numeric" Type="Integer" Operator="DataTypeCheck">*</asp:comparevalidator><asp:requiredfieldvalidator id="Requiredfieldvalidator7" runat="server" Width="9px" ControlToValidate="txtStdPack"
								Display="Dynamic" ErrorMessage="Standard Pack Required">*</asp:requiredfieldvalidator><asp:label id="Label7" runat="server">Standard Pack:</asp:label></TD>
						<TD style="WIDTH: 309px" align="left"><asp:textbox id="txtStdPack" runat="server" Width="112px" Font-Size="8pt"></asp:textbox></TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 28px" align="right"><asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server" Width="1px" ControlToValidate="txtPkgCD"
								Display="Dynamic" ErrorMessage="Package Code Required">*</asp:requiredfieldvalidator><asp:label id="Label6" runat="server">Package Code:</asp:label></TD>
						<TD style="WIDTH: 309px; HEIGHT: 28px"><asp:textbox id="txtPkgCD" runat="server" Width="112px" Font-Size="8pt"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right"><asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" Width="8px" ControlToValidate="rblContainer"
								Display="Dynamic" ErrorMessage="Container Required">*</asp:requiredfieldvalidator><asp:label id="Label5" runat="server">Container:</asp:label></TD>
						<TD style="WIDTH: 309px" vAlign="bottom"><asp:radiobuttonlist id="rblContainer" runat="server" Width="169px" Height="20px" RepeatDirection="Horizontal"
								RepeatColumns="2">
								<asp:ListItem Value="CDICS">CDICS</asp:ListItem>
								<asp:ListItem Value="GICS">GICS</asp:ListItem>
							</asp:radiobuttonlist></TD>
					</TR>
					<TR>
						<TD align="right"><asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" Width="9px" ControlToValidate="txtBoxCount"
								Display="Dynamic" ErrorMessage="Box Count Required">*</asp:requiredfieldvalidator><asp:comparevalidator id="Comparevalidator2" runat="server" ControlToValidate="txtBoxCount" ErrorMessage="Box Count Must Be Numeric"
								Type="Integer" Operator="DataTypeCheck">*</asp:comparevalidator><asp:label id="Label4" runat="server">Box Count:</asp:label></TD>
						<TD style="WIDTH: 309px"><asp:textbox id="txtBoxCount" runat="server" Width="112px" Font-Size="8pt"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right"><asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" Width="1px" ControlToValidate="txtBroadCastCD"
								Display="Dynamic" ErrorMessage="Broadcast Code Required">*</asp:requiredfieldvalidator><asp:label id="Label3" runat="server">Broadcast Code:</asp:label></TD>
						<TD style="WIDTH: 309px"><asp:textbox id="txtBroadCastCd" runat="server" Width="112px" Font-Size="8pt"></asp:textbox></TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 23px" align="right"><asp:comparevalidator id="Comparevalidator1" runat="server" ControlToValidate="txtLayerCnt" ErrorMessage="Layer Count Must Be Numeric"
								Type="Integer" Operator="DataTypeCheck">*</asp:comparevalidator><asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" Width="8px" ControlToValidate="txtLayerCnt"
								Display="Dynamic" ErrorMessage="Layer Count Required">*</asp:requiredfieldvalidator><asp:label id="Label2" runat="server">Layer Count:</asp:label></TD>
						<TD style="WIDTH: 309px; HEIGHT: 23px"><asp:textbox id="txtLayerCnt" runat="server" Width="112px" Font-Size="8pt"></asp:textbox></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 449px" align="center" colSpan="2">
							<P align="center"><asp:button id="btnAddStdPack" runat="server" Width="138px" Font-Size="8pt" Font-Bold="True"
									Text="Create Production Run"></asp:button>&nbsp;&nbsp;&nbsp;<asp:button id="btnEditStdPack" runat="server" Width="125px" Font-Size="8pt" Text="Edit Production Run"
									CausesValidation="False"></asp:button></P>
						</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 449px" align="left" colSpan="2"><asp:label id="lblMsg" runat="server" CssClass="lblerror" Font-Size="8pt"></asp:label></TD>
					</TR>
				</TABLE>
			</P>
			<P>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:textbox id="txtMesPartId" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtSecurityCheck" runat="server" Visible="False"></asp:textbox></P>
			<TABLE id="tblResults" title="Results Table" style="WIDTH: 280px; HEIGHT: 160px" cellSpacing="0"
				cellPadding="0" align="center" border="0" runat="server">
				<TR>
					<TD style="WIDTH: 310px; HEIGHT: 12px" align="left" colSpan="1" rowSpan="1"></TD>
					<TD style="WIDTH: 210px; HEIGHT: 12px" align="right"><asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 520px; HEIGHT: 15px" colSpan="2"><asp:datagrid id="dgProductionRun" runat="server" Visible="False" AllowSorting="True" AutoGenerateColumns="False"
							BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" OnDeleteCommand="dgProductionRun_Delete" OnSortCommand="dgProductionRun_SortCommand"
							OnCancelCommand="dgProductionRun_Cancel" OnUpdateCommand="dgProductionRun_Update" OnEditCommand="dgProductionRun_Edit">
							<SelectedItemStyle Wrap="False"></SelectedItemStyle>
							<EditItemStyle Wrap="False" HorizontalAlign="Right"></EditItemStyle>
							<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
							<ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="dgItem" VerticalAlign="Middle"></ItemStyle>
							<HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="REV_PHYSICAL" HeaderText="ECL">
									<HeaderStyle Width="15px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STD_PACK_QTY" HeaderText="Std. Pack">
									<HeaderStyle Width="15px"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="TOTAL_PART_QTY" HeaderText="Box Cnt">
									<HeaderStyle Width="15px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PROCESS_ID" HeaderText="Broadcast Cd">
									<HeaderStyle Width="25px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STD_PACK_COMPLETE_QTY" HeaderText="Layer Cnt">
									<HeaderStyle Width="15px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PACKAGE_CODE" HeaderText="Package Cd">
									<HeaderStyle Width="15px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
									<FooterStyle Wrap="False"></FooterStyle>
								</asp:EditCommandColumn>
								<asp:ButtonColumn Text="Delete" CommandName="Delete">
									<HeaderStyle Wrap="False"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
									<FooterStyle Wrap="False"></FooterStyle>
								</asp:ButtonColumn>
								<asp:BoundColumn Visible="False" DataField="PRODUCTION_RUN_ID" ReadOnly="True"></asp:BoundColumn>
							</Columns>
							<PagerStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="note" style="WIDTH: 520px" align="center" colSpan="2">All information is 
						Delphi Confidential</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
