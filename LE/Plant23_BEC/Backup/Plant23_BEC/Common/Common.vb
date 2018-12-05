Imports System.Configuration
Imports System.IO
Imports System.Diagnostics
Imports System.Diagnostics.Process
Module Common
    'Public strConn2 As String = "Provider=MSDAORA.1;Password=adavis6875;User ID=adavis;Data Source=TV92.TCRIX;Persist Security Info=True"
    'Public strConn As String = "Provider=MSDAORA.1;Password=mesfull;User ID=mesweb;Data Source=PROD.FANTA;Persist Security Info=True"
    'Public strConn As String = "Provider=MSDAORA.1;Password=mes23full;User ID=mesweb_p23;Data Source=TV10.USOHWAR;Persist Security Info=True"
    Public strConn As String = GetConnectionString()
    Public ord As String
    Public ordStandardPack As String
    Public sel, frm, whr, grp As String
    Public RecordCount As Integer
    Public Function GetMESPartId(ByVal psPartNumber As String, ByVal psUniqueFeature As String) As String
        'On Error GoTo GetMESPartIdErr
        Dim sMessage As String
        Dim sSource As String
        Dim sParameters As String
        Dim CmdGet_Mes_PartId As New OleDb.OleDbCommand
        Dim oDataAdapter = New OleDb.OleDbDataAdapter(CmdGet_Mes_PartId)
        Dim iMESPartId As Integer
        Dim iProcErrNum As Integer
        Dim sProcErrMsg As String
        Dim sProc As String
        Dim sbErrMsg As String
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim Oparm As New OleDb.OleDbParameter
        Try
            ' open the strConn
            MyConn.Open()
            ' set up the OleDb.OleDbCommand object
            With CmdGet_Mes_PartId
                .CommandText = "MESPROC.GET_MES_PART"
                .CommandType = CommandType.StoredProcedure
                .Connection = MyConn
                With .Parameters
                    .Clear()
                    .Add(New OleDb.OleDbParameter("part_nbr", OleDb.OleDbType.VarChar, 30, ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PART_NBR", DataRowVersion.Current, psPartNumber.Trim))
                    .Add(New OleDb.OleDbParameter("unique_feature", OleDb.OleDbType.VarChar, 215, ParameterDirection.Input, True, CType(0, Byte), CType(0, Byte), "UNIQUE_FEATURE", DataRowVersion.Current, IIf(psUniqueFeature.Trim.Equals(String.Empty), Convert.DBNull, psUniqueFeature.Trim)))
                    .Add(New OleDb.OleDbParameter("mes_part_id", OleDb.OleDbType.Numeric, 0, ParameterDirection.ReturnValue, True, 0, 0, "MES_PART_ID", DataRowVersion.Current, iMESPartId))
                    .Add(New OleDb.OleDbParameter("err_num", OleDb.OleDbType.Numeric, 0, ParameterDirection.ReturnValue, True, CType(0, Byte), CType(0, Byte), "", DataRowVersion.Current, iProcErrNum))
                    .Add(New OleDb.OleDbParameter("err_msg", OleDb.OleDbType.VarChar, 100, ParameterDirection.ReturnValue, True, CType(0, Byte), CType(0, Byte), "", DataRowVersion.Current, sProcErrMsg))
                    .Add(New OleDb.OleDbParameter("err_proc", OleDb.OleDbType.VarChar, 100, ParameterDirection.ReturnValue, True, CType(0, Byte), CType(0, Byte), "", DataRowVersion.Current, sProc))
                End With ' .Parameters
            End With ' CmdGet_Mes_PartId
            ' execute the stored procedure
            CmdGet_Mes_PartId.ExecuteNonQuery()
            If CmdGet_Mes_PartId.Parameters("mes_part_id").Value & "" = "" Then
                If (CmdGet_Mes_PartId.Parameters("err_num").Value) <> 0 Then
                    sbErrMsg = "Oracle Error Number: "
                    sbErrMsg = sbErrMsg & CmdGet_Mes_PartId.Parameters("err_num").Value
                    sbErrMsg = sbErrMsg & Chr(13)
                    sbErrMsg = sbErrMsg & "Message: "
                    sbErrMsg = sbErrMsg & CmdGet_Mes_PartId.Parameters("err_msg").Value
                    sParameters = sParameters & "psPartNumber='" & psPartNumber & "',"
                    sParameters = sParameters & "psUniqueFeature='" & psUniqueFeature & "'"
                    GetMESPartId = "Part: " & CmdGet_Mes_PartId.Parameters("part_id").Value & sbErrMsg
                End If
            Else
                GetMESPartId = CmdGet_Mes_PartId.Parameters("mes_part_id").Value()
                'Response.Write(GetMESPartId)
            End If
        Catch oException As System.Exception
            sMessage = oException.Message
            sSource = oException.Source & ":Engineering:Part:GetMESPartId"
            sParameters = sParameters & "psPartNumber='" & psPartNumber & "',"
            sParameters = sParameters & "psUniqueFeature='" & psUniqueFeature & "'"
        Finally
            If MyConn.State = ConnectionState.Open Then
                MyConn.Close()
            End If
        End Try
    End Function
    Public Function GetSecurity(ByVal psUserid As String) As String
        Dim MyConn As New OleDb.OleDbConnection(strConn)
        Dim sqlSecurityCheck As String
        Try
            sqlSecurityCheck = "SELECT sa.SECURITY_ROLE as SEC_ROLE FROM MESDBA.SECURITY_ACCESS sa, MESDBA.VW_PEOPLE vp WHERE vp.ORG_EMP_ID = sa.ORG_EMP_ID  and vp.USER_ID = '" & psUserid & "' and sa.APPLICATION_ID = 5"
            'Create a Command object with the SQL statement.
            'Response.Write(sqlSecurityCheck)
            MyConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(sqlSecurityCheck, MyConn)
            Dim objDataReader As OleDb.OleDbDataReader
            objDataReader = objCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
            If objDataReader.HasRows = True Then
                While objDataReader.Read
                    GetSecurity = objDataReader("SEC_ROLE")
                End While
            Else
                GetSecurity = "Access_Denied"
            End If
            If MyConn.State = ConnectionState.Open Then
                MyConn.Dispose()
            End If
        Finally
            MyConn.Dispose()
        End Try
    End Function
    Friend Sub LogEvent(ByVal psMessage As String, _
                      Optional ByVal plEventLogEntryType As EventLogEntryType = EventLogEntryType.Error, _
                      Optional ByVal piEventID As Integer = 0, _
                      Optional ByVal piCategory As Short = 0)

        Dim sErrMessage As String
        Dim sErrSource As String
        Dim oProcess As System.Diagnostics.Process
        Dim oEventLog As EventLog
        Dim sCurrentSourceEventLog As String
        Dim sEventSource As String
        Dim sEventLogName As String

        Try

            sEventSource = ConfigurationSettings.AppSettings("EventSource")
            sEventLogName = ConfigurationSettings.AppSettings("EventLogName")

            'Check For Current "Event Source / Event Log" Registration
            If EventLog.SourceExists(sEventSource) Then
                oProcess = GetCurrentProcess()
                sCurrentSourceEventLog = EventLog.LogNameFromSourceName(sEventSource, oProcess.MachineName())
                If Not sCurrentSourceEventLog = sEventLogName Then
                    EventLog.DeleteEventSource(sEventSource)
                End If
            End If

            ' write the entry to the event log
            oEventLog = New EventLog(sEventLogName)
            oEventLog.Source = sEventSource
            If Not oEventLog.SourceExists(sEventSource) Then
                oEventLog.CreateEventSource(sEventSource, sEventLogName)
            End If
            oEventLog.WriteEntry(psMessage)

        Catch oException As System.Exception
            sErrMessage = oException.Message
            sErrSource = "Common:LogEvent:" & oException.Source
            If IsLoggingOn() Then
                LogError(sErrSource, sErrMessage)
            End If
        End Try
    End Sub ' LogEvent

    Friend Sub LogError(ByVal psErrSrc As String, _
                         ByVal psErrDescr As String, _
                Optional ByVal psParams As String = "None")

        Dim sFileName As String
        Dim sFilePath As String = System.AppDomain.CurrentDomain.BaseDirectory() & "ErrorLogs\"
        Dim sAppName As String
        Dim fs As FileStream
        Dim sw As StreamWriter

        If Not Directory.Exists(sFilePath) Then
            Directory.CreateDirectory(sFilePath)
        End If

        sAppName = System.Reflection.Assembly.GetExecutingAssembly.Location
        sFileName = sFilePath & "DPHMachnMaintMonitorComponents-" & CStr(Month(Now)) & "-" & CStr(Day(Now)) & "-" & CStr(Year(Now())) & ".err"
        fs = New FileStream(sFileName, FileMode.Append)
        sw = New StreamWriter(fs)
        sw.WriteLine(System.DateTime.Now & " Error: " & psErrDescr & ":" & psErrSrc & " ")
        sw.WriteLine("     Parameters: " & psParams)
        sw.Close()

    End Sub
    '***************************************************************************
    ' Function...: IsLoggingOn
    ' Purpose....: Determines if error logging is on for the associated 
    '              application.  For ASP.NET applications, this will be in the 
    '              web.config.  For Windows Forms applications, this will be in
    '              in the <appName>.exe.config file.
    ' Parameters.: None
    ' Returns....: True if logging is on, otherwise False
    '***************************************************************************
    Friend Function IsLoggingOn() As Boolean

        Return ConfigurationSettings.AppSettings.Get("Logging").Trim.ToUpper.Equals("TRUE")

    End Function ' IsLoggingOn
    Friend Function GetConnectionString() As String
        ' local variables
        Dim sConnect As String
        Dim sDataSource As String
        Dim sPassword As String
        Dim sProvider As String
        Dim sServerType As String
        Dim sUserID As String
        Dim moEncrypt As New DPH.Common.Object.Encryption.SymmetricEncrypt("DELPHI", "MIT")
        'TODO: Uncomment encryption settings
        ' get the server type setting from the machine.config file
        sServerType = ConfigurationSettings.AppSettings("ServerType")
        If Not sServerType.EndsWith(".") Then
            sServerType = sServerType & "."
        End If

        sUserID = ConfigurationSettings.AppSettings(sServerType & "Username")
        sPassword = moEncrypt.Decrypt(ConfigurationSettings.AppSettings(sServerType & "Password"))
        'sPassword = "mz36gf1234"
        sDataSource = ConfigurationSettings.AppSettings(sServerType & "DataSource")
        sProvider = ConfigurationSettings.AppSettings(sServerType & "Provider")

        ' build the connection string
        sConnect = "PROVIDER=" & sProvider & ";" & _
                    "DATA SOURCE=" & sDataSource & ";" & _
                    "USER ID=" & sUserID & ";" & _
                    "PASSWORD=" & sPassword & ";"

        Return sConnect
    End Function ' GetConnectionString
    '***************************************************************************

    ' Function...: GetServerType

    ' Purpose....: Get the server type configuration for database settings from

    '              the <appName>.exe.config file.

    ' Parameters.: None

    ' Returns....: String containing the server type.

    '***************************************************************************

    Friend Function GetServerType() As String
        Dim sServerType As String
        Try
            sServerType = ConfigurationSettings.AppSettings("ServerType")
            If sServerType Is Nothing Then sServerType = ""
            ' if no server has been specified, default to local connection settings
            If sServerType.Equals("") Then
                sServerType = "LOCAL."
            End If
            If Not sServerType.EndsWith(".") Then
                sServerType &= "."
            End If
            Return sServerType
        Catch oException As System.Exception
            Dim sMessage As String
            Dim sSource As String
            sMessage = oException.Message
            sSource = oException.Source & ":Common:GetSchemaName"
            If IsLoggingOn() Then
                LogError(sSource, sMessage, "Common")
            End If
        End Try
    End Function ' GetServerType

End Module
