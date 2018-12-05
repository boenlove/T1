<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DatePicker.aspx.vb" Inherits="Plant23_BEC.DatePicker"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>DatePicker</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:calendar id="CalPopup" runat="server" OnSelectionChanged="Change_Date" Font-Names="Verdana"
				Font-Size="Smaller" Width="232px" CellSpacing="1" ForeColor="Black" NextPrevFormat="ShortMonth"
				Height="200px" BorderStyle="Solid" BorderColor="Black" BackColor="White">
				<TodayDayStyle ForeColor="White" BackColor="#999999"></TodayDayStyle>
				<DayStyle BackColor="#CCCCCC"></DayStyle>
				<NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="White"></NextPrevStyle>
				<DayHeaderStyle Font-Size="8pt" Font-Bold="True" Height="8pt" ForeColor="#333333"></DayHeaderStyle>
				<SelectedDayStyle ForeColor="White" BackColor="#333399"></SelectedDayStyle>
				<TitleStyle Font-Size="12pt" Font-Bold="True" Height="12pt" ForeColor="White" BackColor="LightSlateGray"></TitleStyle>
				<OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
			</asp:calendar>&nbsp;
		</form>
	</body>
</HTML>
