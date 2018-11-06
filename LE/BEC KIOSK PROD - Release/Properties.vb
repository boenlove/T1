Option Explicit On 
Option Strict On

Public Class Properties
    Private pmachineName As String = ""
    Private plineNumber As String = ""
    Private pplant As String = ""
    Private pdepartment As String = ""
    Private pdelayBetweenScans As String = ""
    Private pAdminCode As String = ""

    Public Property AdminCode() As String
        Get
            Return pAdminCode
        End Get
        Set(ByVal Value As String)
            pAdminCode = Value
        End Set
    End Property

    Public Property DelayBetweenScans() As String
        Get
            Return pdelayBetweenScans
        End Get
        Set(ByVal Value As String)
            pdelayBetweenScans = Value
        End Set
    End Property

    Public Property MachineName() As String
        Get
            Return pmachineName
        End Get
        Set(ByVal Value As String)
            pmachineName = Value
        End Set
    End Property

    Public Property LineNumber() As String
        Get
            Return plineNumber
        End Get
        Set(ByVal Value As String)
            plineNumber = Value
        End Set
    End Property

    Public Property Plant() As String
        Get
            Return pplant
        End Get
        Set(ByVal Value As String)
            pplant = Value
        End Set
    End Property

    Public Property Department() As String
        Get
            Return pdepartment
        End Get
        Set(ByVal Value As String)
            pdepartment = Value
        End Set
    End Property

    Public Sub New()

        Dim configSettings As New ConfigurationSettings
        Try

            MachineName = configSettings.Appsettings("//Properties/MachineName")
            Plant = configSettings.Appsettings("//Properties/Plant")
            LineNumber = configSettings.Appsettings("//Properties/LineNumber")
            Department = configSettings.Appsettings("//Properties/Department")
            DelayBetweenScans = configSettings.Appsettings("//ApplicationState/DelayBetweenScans")
            AdminCode = configSettings.Appsettings("//Properties/AdminCode")

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            configSettings = Nothing
        End Try
    End Sub
End Class
