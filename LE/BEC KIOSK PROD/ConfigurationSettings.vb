Imports System
Imports System.Collections
Imports System.Xml
Imports System.text
Imports System.io



Public Class ConfigurationSettings
    Private ssectionTag As String = ""
    'todo fix this
    Private xmlFile As String = Application.StartupPath & "\AppConfig.xml"

    Public Property Appsettings(ByVal Key As String) As String
        Get
            Dim oDOm As System.xml.XmlDocument = New XmlDocument
            Try
                oDOm.Load(xmlFile)
                Dim oNode As XmlNode = oDOm.SelectSingleNode(Key)
                Appsettings = oNode.InnerXml
            Catch
                'Redirect to Error Page or take other action to handle error.
            End Try
        End Get

        Set(ByVal Value As String)
            Dim oDOm As System.xml.XmlDocument = New XmlDocument
            Try
                oDOm.Load(xmlFile)
                Dim oNode As XmlNode = oDOm.SelectSingleNode(Key)
                oNode.InnerText = Value
                oDOm.Save(xmlFile)
            Catch
                'Redirect to Error Page or take other action to handle error.
            End Try

        End Set
    End Property

    Public Sub New()

    End Sub
End Class ' ConfigurationSettings class
