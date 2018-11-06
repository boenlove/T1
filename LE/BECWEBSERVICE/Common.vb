Option Explicit On

'*****************************************************************************
' Name.......: Common       
' Purpose....: Common helper functions   
' Comments...:
' References.: System
'              System.Collections
'              System.Configuration
'              System.Diagnostics
' Components.:
'*****************************************************************************
' Revision History
'
'
'*****************************************************************************
Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Diagnostics
Imports System.IO

Module Common

    '***************************************************************************
    ' Function...: BuildConnectionString
    ' Purpose....: Build the database server connection string from the 
    '              application configuration file
    ' Parameters.: None
    ' Returns....: Database connection string
    '***************************************************************************
    Friend Function BuildConnectionString() As String

        ' local variables
        Dim sConnect As String
        Dim sDataSource As String
        Dim sPassword As String
        Dim sProvider As String
        Dim sUserID As String
        Dim sServerType As String

        'Server Type : Test, Production ...........
        sServerType = ConfigurationSettings.AppSettings("ServerType")

        'If the server type does not end with a . then add one
        If Not sServerType.EndsWith(".") Then
            sServerType += "."
        End If

        sDataSource = ConfigurationSettings.AppSettings(sServerType & "Datasource")
        sPassword = ConfigurationSettings.AppSettings(sServerType & "Password")
        'sProvider = ConfigurationSettings.AppSettings(sServer & "Provider")
        sUserID = ConfigurationSettings.AppSettings(sServerType & "UserName")

        ' build the connection string
        sConnect = "Server=" & sDataSource & ";" & _
          "USER ID=" & sUserID & ";" & _
          "PASSWORD=" & sPassword & ";"

        Return sConnect

    End Function ' BuildConnectionString
    Friend Function GetOracleConnectionString(ByVal OracleConnection_KeyName As String) As String
        Dim servertype As String
        Dim ConnString As String
        Try
            servertype = ConfigurationSettings.AppSettings.Get("ServerType")
            If Not servertype.EndsWith(".") Then
                servertype += "."
            End If
            ConnString = ConfigurationSettings.AppSettings.Get(servertype & OracleConnection_KeyName)
            Return ConnString
        Catch ex As Exception
            Return "Failure GetOracleConnectionString: " & ex.Message
        End Try
    End Function

    '***************************************************************************
    ' Function...: GetSchemaName
    ' Purpose....: Get the database schema name from the associated application
    '              configuration file.  For ASP.NET applications, this will be 
    '              in the web.config.  For Windows Forms applications, this will 
    '              be in the <appName>.exe.config file.
    ' Parameters.: None
    ' Returns....: String containing the database schema name.
    '***************************************************************************
    Friend Function GetSchemaName() As String

        ' local variables
        Dim sSchema As String

        sSchema = ConfigurationSettings.AppSettings.Get("DBSchema")

        If sSchema.EndsWith(".") Then
            Return sSchema
        Else
            Return sSchema & "."
        End If
    End Function ' GetSchemaName


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

        Return (ConfigurationSettings.AppSettings.Get("Logging").ToUpper = "TRUE")

    End Function ' IsLoggingOn


    '***************************************************************************
    ' Procedure..: LogEvent
    ' Purpose....: Log an entry in the specified event log.
    ' Parameters.: psSource (String)
    '                The source by which the application is registered on the 
    '                specified computer.
    '              psMessage (String)
    '                The string to write to the event log.
    '              plType (EventLogEntryType)
    '                One of the EventLogEntryType values
    '                  Error
    '                    An error event. This indicates a significant problem 
    '                    the user should know about; usually a loss of 
    '                    functionality or data.
    '                  Failure Audit
    '                    A Failure audit event. This indicates a security event 
    '                    that occurs when an audited access attempt fails; for 
    '                    example, a failed attempt to open a file.
    '                  Information
    '                    An information event. This indicates a significant, 
    '                    successful operation.
    '                  Success Audit
    '                    A success audit event. This indicates a security event 
    '                    that occurs when an audited access attempt is 
    '                    successful; for example, logging on successfully.
    '                  Warning
    '                    A warning event. This indicates a problem that is not 
    '                    immediately significant, but that may signify 
    '                    conditions that could cause future problems.
    '                  piEventID (Integer)
    '                    The application-specific identifier for the event.
    '                  piCategory (Short)
    '                    The application-specific subcategory associated with 
    '                    the message.
    ' Returns....: None
    '***************************************************************************
    Friend Sub LogEvent(ByVal psSource As String, _
                        ByVal psMessage As String, _
                        Optional ByVal plEventLogEntryType As EventLogEntryType = EventLogEntryType.Error, _
                        Optional ByVal piEventID As Integer = 0, _
                        Optional ByVal piCategory As Short = 0)

        ' local variables
        Dim sLogName As String

        ' if the source hasn't been registered...
        If Not EventLog.SourceExists(psSource) Then
            ' get the log name setting from the application configuration file and 
            ' create the event source
            EventLog.CreateEventSource(psSource, ConfigurationSettings.AppSettings("EventLogName"))
        End If

        ' write the entry to the event log
        EventLog.WriteEntry(psSource, psMessage, plEventLogEntryType, piEventID, piCategory)

    End Sub ' LogEvent
    Public Function GetServerType() As String
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
        Catch ex As Exception
            Return "Failure getting server type: " & ex.Message
        End Try
    End Function ' GetServerType
    Friend Sub LogMsg(psErrDescr As String, Optional ByVal psParams As String = "", Optional ByVal psParams2 As String = "")
        Dim sFileName As String
        Dim sFilePath As String = System.AppDomain.CurrentDomain.BaseDirectory() & "ErrorLogs\"
        Dim sAppName As String
        Dim fs As FileStream
        Dim sw As StreamWriter

        If Not Directory.Exists(sFilePath) Then
            Directory.CreateDirectory(sFilePath)
        End If

        sAppName = System.Reflection.Assembly.GetExecutingAssembly.Location
        sFileName = sFilePath & "Log.txt"
        fs = New FileStream(sFileName, FileMode.Append)
        sw = New StreamWriter(fs)
        sw.WriteLine(System.DateTime.Now & "  " & psErrDescr & " " & psParams & " " & psParams2 & " ")
        sw.Close()
    End Sub
End Module
