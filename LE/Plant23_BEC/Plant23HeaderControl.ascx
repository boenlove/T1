<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Plant23HeaderControl.ascx.vb" Inherits="Plant23_BEC.Plant23HeaderControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" debug="True"%>
<DIV align="left"><asp:panel id="pnlPlant23" runat="server" Height="80px" BackColor="#004C7A" BorderColor="LightGray"
		BorderStyle="Solid" Font-Names="Verdana" Font-Size="XX-Small" Width="814px" ForeColor="White">
		<H6>Plant 23&nbsp;&nbsp;
		</H6>
		<H6>
			<asp:HyperLink id="hplkBEC_Setup" ForeColor="White" runat="server" NavigateUrl="BEC_Setup.aspx">BEC Setup</asp:HyperLink>&nbsp; 
			|&nbsp;
			<asp:HyperLink id="hplkBEC_Ship_Destination" tabIndex="1" ForeColor="White" runat="server" NavigateUrl="BEC_Ship_Destination.aspx">BEC Ship Destination</asp:HyperLink>&nbsp; 
			| &nbsp;
			<asp:HyperLink id="hplkBEC_Data_Loader" tabIndex="2" ForeColor="White" runat="server" NavigateUrl="BEC_Data_Loader.aspx">BEC Data Loader</asp:HyperLink>&nbsp; 
			|&nbsp;
			<asp:HyperLink id="hplkBEC_Part_Line_Restriction" tabIndex="4" ForeColor="White" runat="server"
				NavigateUrl="BEC_Part_Line_Restriction.aspx">BEC Part Line Restriction</asp:HyperLink>&nbsp; 
			|&nbsp;
			<asp:HyperLink id="hpkBEC_Production_Run" tabIndex="6" ForeColor="White" runat="server" NavigateUrl="BEC_ProductionRun.aspx">BEC Add Production Run</asp:HyperLink>&nbsp; 
			|&nbsp;
			<asp:HyperLink id="hplkSerialNumber_Search" tabIndex="6" ForeColor="White" runat="server" NavigateUrl="SerialNumber_Search.aspx">Serial Number Search</asp:HyperLink></H6>
		<H6>
			<asp:HyperLink id="hplkBEC_Part_Line_Restriction_Deleter" tabIndex="5" ForeColor="White" Width="192px"
				runat="server" NavigateUrl="BEC_Part_Line_Restriction_Deleter.aspx">BEC Part Line Restriction Deleter</asp:HyperLink>&nbsp; 
			|&nbsp;
			<asp:HyperLink id="hplkBEC_Data_Deleter" tabIndex="3" ForeColor="White" runat="server" NavigateUrl="BEC_Data_Deleter.aspx">BEC Data Deleter</asp:HyperLink>&nbsp; 
			|&nbsp;
			<asp:HyperLink id="hplkBEC_UpdateProductionRun" tabIndex="7" ForeColor="White" runat="server" NavigateUrl="BEC_UpdateProductionRun.aspx">BEC Update Production Run</asp:HyperLink>&nbsp; 
			|&nbsp;
			<asp:HyperLink id="hplkBEC_SerialNumbers" tabIndex="7" ForeColor="White" runat="server" NavigateUrl="BEC_SerialNumbers.aspx">BEC Serial Numbers</asp:HyperLink>&nbsp; 
			|&nbsp;
			<asp:HyperLink id="hplkBEC_Generic_SerialNumbers" tabIndex="7" ForeColor="White" runat="server"
				NavigateUrl="BEC_Generic_SerialNumbers.aspx">BEC Generic Serial Numbers</asp:HyperLink>&nbsp; 
			|</H6>
		<P>
			<asp:HyperLink id="hplkBEC_DelphiGMCrossReference" tabIndex="7" ForeColor="White" runat="server"
				NavigateUrl="Delphi_GM_Cross_Reference.aspx" Font-Bold="True">Delphi/GM Cross Reference</asp:HyperLink>&nbsp; 
			|&nbsp;
			<asp:HyperLink id="hplk" tabIndex="7" ForeColor="White" runat="server" NavigateUrl="BEC_Ship_Destination_All_Users.aspx"
				Font-Bold="True">BEC Ship Destination All Users</asp:HyperLink>&nbsp; 
			|&nbsp;
			<asp:HyperLink id="hplkHelp" tabIndex="7" ForeColor="Red" runat="server" NavigateUrl="http://usmscli-dev01/Plant23_BEC/Help/BEC WEB PROGRAM INSTRUCTIONS.htm"
				Font-Bold="True">Help</asp:HyperLink></P>
	</asp:panel></DIV>
