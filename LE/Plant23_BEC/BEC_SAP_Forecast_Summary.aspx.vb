Partial Class BEC_SAP_Forecast_Summary
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim x As String = GetServerType()
        If Not IsPostBack Then
            If x = "LOCAL." Or x = "TEST." Then
                lblDisplay.Visible = True
                lblDisplay.Text = "TEST ENVIRONMENT"
            End If
            'txtSecurityCheck.Text = GetSecurity(UCase(Right(Request.ServerVariables("LOGON_USER"), 6)))
            'Logon as Phillip White
            'Department = 02192
            'UserID = WZB5SH
            'Email = Phillip.White@delphi.com
            'UserType = NORTHAMERICA
            'Status = InActive

            'txtSecurityCheck.Text = GetSecurity(ConfigurationSettings.AppSettings("LOGON_USER"))
            txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23"
            If txtSecurityCheck.Text = "BEC_ICS_WEB_ADMN_P23" Or txtSecurityCheck.Text = "BEC_ICS_WEB_USER_P23" Then
                Populate_cmbSelectedWeek()
                Populate_dgForecastSummary()
            Else
                Response.Redirect("BEC_Security_Denied.aspx")
            End If
        End If
    End Sub

    Private Sub Populate_cmbSelectedWeek()
        With cmbSelectedWeek.Items
            .Add("Week 1")
            .Add("Week 2")
            .Add("Week 3")
            .Add("Week 4")
            .Add("Week 5")
            .Add("Week 6")
            .Add("Week 7")
            .Add("Week 8")
            .Add("Week 9")
            .Add("Week 10")
            .Add("Week 11")
            .Add("Week 12")
            .Add("Week 13")
            .Add("Week 14")
            .Add("Week 15")
            .Add("Week 16")
        End With
        cmbSelectedWeek.SelectedIndex = 0
    End Sub

    Private Sub Populate_dgForecastSummary()
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlGetForecastSummary As String
        If Not cmbSelectedWeek.SelectedIndex = -1 Then
            Try
                Select Case Me.cmbSelectedWeek.Items(cmbSelectedWeek.SelectedIndex).ToString()
                    Case "Week 1"
                        sqlGetForecastSummary = "SELECT ICS_DEPT, " & _
                                                    "PART_NBR, " & _
                                                    "CNTR_TYPE AS DESTINATION, " & _
                                                    "WK1_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK1_TOT_CUST_REQ AS CUSTOMER_REQUIRED, " & _
                                                    "WK1_PROJ_AVAIL AS AVAILABLE, " & _
                                                    "WK1_WHSE_REQ AS REQUIRED, " & _
                                                    "WK1_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM SNAPMGR.WHSE_REQMNTS " & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 2"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK2_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK2_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK2_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK2_WHSE_REQ AS REQUIRED," & _
                                                    "WK2_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 3"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK3_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK3_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK3_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK3_WHSE_REQ AS REQUIRED," & _
                                                    "WK3_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 4"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK4_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK4_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK4_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK4_WHSE_REQ AS REQUIRED," & _
                                                    "WK4_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 5"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK5_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK5_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK5_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK5_WHSE_REQ AS REQUIRED," & _
                                                    "WK5_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 6"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK6_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK6_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK6_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK6_WHSE_REQ AS REQUIRED," & _
                                                    "WK6_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 7"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK7_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK7_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK7_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK7_WHSE_REQ AS REQUIRED," & _
                                                    "WK7_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 8"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK8_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK8_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK8_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK8_WHSE_REQ AS REQUIRED," & _
                                                    "WK8_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 9"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK9_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK9_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK9_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK9_WHSE_REQ AS REQUIRED," & _
                                                    "WK9_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 10"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK10_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK10_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK10_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK10_WHSE_REQ AS REQUIRED," & _
                                                    "WK10_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                 "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 11"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK11_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK11_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK11_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK11_WHSE_REQ AS REQUIRED," & _
                                                    "WK11_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 12"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK12_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK12_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK12_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK12_WHSE_REQ AS REQUIRED," & _
                                                    "WK12_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                 "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 13"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK13_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK13_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK13_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK13_WHSE_REQ AS REQUIRED," & _
                                                    "WK13_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 14"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK14_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK14_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK14_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK14_WHSE_REQ AS REQUIRED," & _
                                                    "WK14_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 15"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK15_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK15_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK15_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK15_WHSE_REQ AS REQUIRED," & _
                                                    "WK15_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                    Case "Week 16"
                        sqlGetForecastSummary = "SELECT ICS_DEPT," & _
                                                    "PART_NBR," & _
                                                    "CNTR_TYPE AS DESTINATION," & _
                                                    "WK16_RQMNT_DATE AS BUILD_DATE, " & _
                                                    "WK16_TOT_CUST_REQ AS CUSTOMER_REQUIRED," & _
                                                    "WK16_PROJ_AVAIL AS AVAILABLE," & _
                                                    "WK16_WHSE_REQ AS REQUIRED," & _
                                                    "WK16_ADJ_WHSE_REQ AS ADJUSTED " & _
                                                "FROM(SNAPMGR.WHSE_REQMNTS)" & _
                                                    "WHERE PLANNER = 2300 OR PLANNER = 2330"
                End Select

                MyConn.Open()
                Dim objCommand As New OleDb.OleDbCommand(sqlGetForecastSummary, MyConn)
                Dim objDataReader As OleDb.OleDbDataReader
                objDataReader = objCommand.ExecuteReader

                dgForecastSummary.DataSource = objDataReader
                dgForecastSummary.DataBind()
                dgForecastSummary.Visible = True

                While objDataReader.Read()
                    RecordCount = RecordCount + 1
                End While
                If RecordCount = 0 Then
                    lblCount.Visible = True
                    lblCount.Text = "Record Count: 0"
                Else
                    lblCount.Visible = True
                    lblCount.Text = "Record Count: " & RecordCount
                End If
                objDataReader.Close()

            Catch ex As Exception
                lblMessage.Text = "The following error has occurred: " & ex.Source & ":" & ex.Message & ". Please Contact your web developer."
                If IsLoggingOn() Then
                    LogError(ex.Source, ex.Message)
                End If
                LogEvent(ex.Message)
            End Try


        End If
    End Sub

    Private Sub cmbSelectedWeek_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSelectedWeek.SelectedIndexChanged
        Populate_dgForecastSummary()
    End Sub
End Class
