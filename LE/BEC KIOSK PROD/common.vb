Imports System.IO

Module common
    Public goappstate As New ApplicationState
    Public goProperties As New Properties
    Public runningExceptions As Integer
    Public ScannerEnabledWhenExceptionsCalled As Integer
    Public runningSetCount As Integer
    Public ScannerEnabledWhenSetCountCalled As Integer
    Public ProcessingContainment As Boolean
    Public ScannedForContainment As String
    Friend Quality_ProbCodes() As String
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
