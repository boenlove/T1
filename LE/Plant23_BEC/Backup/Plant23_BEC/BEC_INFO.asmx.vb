Imports System.Web.Services
Imports System.Data.OracleClient
<System.Web.Services.WebService(Namespace:="http://tempuri.org/Plant23_BEC/BEC_INFO")> _
Public Class BEC_INFO
    Inherits System.Web.Services.WebService

#Region " Web Services Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Web Services Designer.
        InitializeComponent()

        'Add your own initialization code after the InitializeComponent() call

    End Sub

    'Required by the Web Services Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Web Services Designer
    'It can be modified using the Web Services Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        'CODEGEN: This procedure is required by the Web Services Designer
        'Do not modify it using the code editor.
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region
    <WebMethod(Description:="Retrieves ICS Delivery Location based on Part Number and Container.")> _
Public Function GetICSDeliveryLocation(ByVal psPartNumber As String, ByVal psContainer As String) As DataSet
        Dim strConn As String = "Provider=MSDAORA.1;Password=adavis6875;User ID=adavis;Data Source=TV92.TCRIX;Persist Security Info=True"
        Dim MyConn As New OracleConnection(strConn)
        Dim CmdICS_Delivery_Location As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim sel, frm, whr As String
        Dim sqlGetICS_DELIVERY_LOCATION As String
        Try
            sel = "SELECT MES_PART_ID, PART_NBR, CNTR, DEST, CSOURCE, RLSEDATE, FIRST_REF_QUAL, FIRST_QUAL_TEXT, PKG_11Z, PKG_12Z, PKG_13Z, PKG_14Z, PKG_15Z, PKG_16Z, PKG_17Z, MOD_USERID, MOD_TMSTM"
            frm = " FROM SO_BEC.ICS_DELIVERY_LOCATION"
            whr = " WHERE PART_NBR = '" & psPartNumber & "' and CNTR = '" & psContainer & "'"
            sqlGetICS_DELIVERY_LOCATION = sel & frm & whr
            'Response.Write(sqlGetICS_DELIVERY_LOCATION)
            ' Create a Command object with the SQL statement.
            MyConn.Open()
            ' get the information
            With CmdICS_Delivery_Location
                .Connection = MyConn
                .CommandType = CommandType.Text
                .CommandText = sqlGetICS_DELIVERY_LOCATION
                .ExecuteNonQuery()
            End With
            'fill the dataset
            da = New OracleDataAdapter(CmdICS_Delivery_Location)
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception(ex.Message)
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
            CmdICS_Delivery_Location.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try

    End Function
    Public Function GetPartLineRestriction(ByVal psMesPartId As String, ByVal psMesMachineId As String, ByVal psContainer As String) As Boolean
        Dim strConn As String = "Provider=MSDAORA.1;Password=adavis6875;User ID=adavis;Data Source=TV92.TCRIX;Persist Security Info=True"
        Dim MyConn As New OracleConnection(strConn)
        Dim dr As OracleDataReader
        Dim CmdGetPartLineRestriction As New OracleCommand
        Dim sqlGetPartLineRestriction As String
        Dim sel, frm, whr As String

        Try
            sel = "SELECT *"
            frm = " FROM SO_BEC.PART_LINE_RESTRICTION"
            whr = " WHERE MES_PART_ID = '" & psMesPartId & "' and MES_MACHINE_ID = '" & psMesMachineId & "' and CNTR = '" & psContainer & "'"
            sqlGetPartLineRestriction = sel & frm & whr

            'Open the connection if it is closed
            If MyConn.State = ConnectionState.Closed Then MyConn.Open()

            'Command object properties
            With CmdGetPartLineRestriction
                .Connection = MyConn
                .CommandType = CommandType.Text
                .CommandText = sqlGetPartLineRestriction
            End With

            'Execute the reader
            dr = CmdGetPartLineRestriction.ExecuteReader(CommandBehavior.CloseConnection)

            'Check to see if anything was returned and set the variable to the value
            If dr.HasRows Then
                Return True
            Else
                Return False
            End If


        Catch ex As Exception
            Throw New Exception(ex.Message)
            If IsLoggingOn() Then
                LogError(ex.Source, ex.Message)
            End If
            LogEvent(ex.Message)
        Finally
            MyConn.Dispose()
            CmdGetPartLineRestriction.Dispose()
            dr.Dispose()
        End Try

    End Function

End Class
