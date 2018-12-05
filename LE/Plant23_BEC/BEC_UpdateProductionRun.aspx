<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BEC_UpdateProductionRun.aspx.vb" Inherits="Plant23_BEC.BEC_UpdateProductionRun"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BEC_UpdateProductionRun</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="/Plant23_BEC/Common/MFG_Style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P align="center">
				<asp:label id="lblDisplay" runat="server" CssClass="lblError"></asp:label><BR>
				&nbsp;</P>
			</TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>
			<DIV>
				<TABLE class="tblFormat" id="tblCriteria" title="BEC Update Production Run" style="WIDTH: 360px; HEIGHT: 129px"
					cellSpacing="0" cellPadding="3" width="360" align="center" border="0">
					<TR>
						<TD class="tbltitle" style="WIDTH: 210px" align="left" bgColor="#ffffff"></TD>
						<TD style="WIDTH: 150px" align="right" bgColor="#ffffff">
							<asp:hyperlink id="hpkReturntoMain" runat="server" NavigateUrl="Index.aspx" Font-Bold="True">Return to Main</asp:hyperlink></TD>
					</TR>
					<TR>
						<TD class="tbltitle" style="WIDTH: 210px" align="left" bgColor="#ffffff">Kiosk 
							Database Manager</TD>
						<TD style="WIDTH: 150px" align="right" bgColor="#ffffff"><asp:button id="btnStdPackAdd" runat="server" Text="Create Production Run" Font-Size="8pt" Width="125px"></asp:button></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 499px" align="left" background="../Images/Divider.jpg" colSpan="2">&nbsp;
							<asp:label id="lblRqstPart" runat="server" Visible="False"></asp:label><asp:label id="lblPartSelIndex" runat="server" Visible="False" Enabled="False"></asp:label><asp:label id="lblSelectedPartID" runat="server" Visible="False" Enabled="False"></asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 210px" align="right"><asp:label id="lblDept" runat="server">Dept:</asp:label></TD>
						<TD style="WIDTH: 150px"><asp:dropdownlist id="cmbDept" runat="server" Font-Size="8pt" AutoPostBack="True">
								<asp:ListItem Value="1125">2341</asp:ListItem>
								<asp:ListItem Value="1126">2342</asp:ListItem>
								<asp:ListItem Value="1127">2343</asp:ListItem>
							</asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 210px" align="right">
							<P align="right"><asp:label id="lblPart" runat="server">Part Number:</asp:label></P>
						</TD>
						<TD style="WIDTH: 150px"><asp:dropdownlist id="cmbPartNumber" runat="server" Font-Size="8pt" Width="104px" AutoPostBack="True"></asp:dropdownlist>&nbsp;</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 210px" align="right"></TD>
						<TD style="WIDTH: 150px">&nbsp;
						</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 499px" align="left" colSpan="2"><asp:label id="lblMsg" runat="server" Font-Size="8pt" CssClass="lblerror"></asp:label></TD>
					</TR>
				</TABLE>
			</DIV>
			<DIV><BR>
				&nbsp;
			</DIV>
			<TABLE id="tblResults" title="Results Table" style="WIDTH: 280px" cellSpacing="0" cellPadding="0"
				align="center" border="0" runat="server">
				<TR>
					<TD style="WIDTH: 310px" align="left" colSpan="1" rowSpan="1"></TD>
					<TD style="WIDTH: 210px" align="right"><asp:label id="lblCount" runat="server" Font-Size="XX-Small" ForeColor="Black"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 520px" colSpan="2"><asp:datagrid id="dgProductionRun" runat="server" Width="577px" AllowSorting="True" AutoGenerateColumns="False"
							GridLines="Horizontal" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" OnDeleteCommand="dgProductionRun_Delete" OnSortCommand="dgProductionRun_SortCommand"
							OnCancelCommand="dgProductionRun_Cancel" OnUpdateCommand="dgProductionRun_Update" OnEditCommand="dgProductionRun_Edit">
							<SelectedItemStyle Wrap="False"></SelectedItemStyle>
							<EditItemStyle Wrap="False" HorizontalAlign="Right"></EditItemStyle>
							<AlternatingItemStyle CssClass="dgAltItem"></AlternatingItemStyle>
							<ItemStyle Wrap="False" HorizontalAlign="Left" CssClass="dgItem" VerticalAlign="Middle"></ItemStyle>
							<HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="REV_PHYSICAL" HeaderText="ECL">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STD_PACK_QTY" HeaderText="Std. Pack">
									<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="TOTAL_PART_QTY" HeaderText="Box Cnt">
									<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PROCESS_ID" HeaderText="Broadcast Cd">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STD_PACK_COMPLETE_QTY" HeaderText="Layer Cnt">
									<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PACKAGE_CODE" HeaderText="Package Cd">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CONTAINER_CODE" HeaderText="Customer"></asp:BoundColumn>
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
								<asp:BoundColumn Visible="False" DataField="production_run_id" ReadOnly="True"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Package_Code" ReadOnly="True"></asp:BoundColumn>
							</Columns>
							<PagerStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
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
		</form>
	</body>
</HTML>
