Option Explicit On 

Imports System.Web.Services
Imports System.Data.OracleClient

<System.Web.Services.WebService(Namespace := "http://tempuri.org/BecWebService/UpdateMachineStatus")> _
Public Class UpdateMachineStatus
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
        components = New System.ComponentModel.Container()
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

	<WebMethod()> _
	Public Sub UpdateStatus(ByVal mesPartID As Long, ByVal currentProduced As Integer, _
			 ByVal machineName As String, ByVal newRun As Boolean)

		' local variables
		Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()
		Dim con As New OracleConnection(conString)
		'Dim cmd As New OracleCommand
		Dim ds As New DataSet
		Dim SQL As String = ""
		Dim schema As String = GetSchemaName()
		Dim sContainerType As String = ""
		Dim lMachineID As Long = 0
		Dim lResetCounters As Long = 0
		Dim dr As DataRow
		Dim lCurrentShiftQty As Long = 0
		Dim shiftDate As Date
		Dim shiftID As Int32
		Dim bMoveToHistory As Boolean

		Try
			'Open the connection if it's closed
			If con.State = ConnectionState.Closed Then con.Open()

			'Get the machine ID for the associated with the machine name
			lMachineID = MachineID(machineName, con)

			'Retrive the current status
			ds = GetMachineCurrentStatus(lMachineID, con)
			Dim DBcurrentQty As Long = 0
			Dim dbCurrentShiftQty As Long = 0

			For Each dr In ds.Tables(0).Rows
				DBcurrentQty = dr.Item("CURRENT_QTY")
				dbCurrentShiftQty = dr.Item("SHIFT_QTY")
				'shiftDate = dr.Item("SHIFT_DATE")
				'shiftID = dr.Item("SHIFT_ID")
			Next

			lCurrentShiftQty = dbCurrentShiftQty + 1

			UpDate(lMachineID, mesPartID, currentProduced, lCurrentShiftQty, con)

			'SQL = "UPDATE " & schema & "MACHINE_CURRENT_STATUS SET STATUS ='RUNNING' WHERE MACHINE_ID = " & lMachineID

			'With cmd
			'	.Connection = con
			'	.CommandType = CommandType.Text
			'	.CommandText = SQL
			'	.ExecuteNonQuery()
			'End With

		Catch exole As OracleException
			Throw New Exception(exole.Message)
		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			con.Dispose()
			'cmd.Dispose()
			ds.Dispose()
			'da.Dispose()
		End Try
	End Sub

	Private Function MachineID(ByVal machineName As String, ByVal con As OracleConnection) As Long
		Dim sSQL As String = "SELECT MACHINE_ID " & "FROM MESDBA.MACHINE " & "WHERE MACHINE_NAME = '" & machineName & "'"
		Dim ds As New DataSet
		Dim da As New OracleDataAdapter(sSQL, con)
		Dim lMachineID As Long

		Try
			da.Fill(ds)

			lMachineID = CType(ds.Tables(0).Rows(0).Item("Machine_ID"), Long)

			Return lMachineID
		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			ds.Dispose()
			da.Dispose()
		End Try

	End Function

	Private Function GetMachineCurrentStatus(ByVal machineID As Long, ByVal con As OracleConnection) As DataSet
		Dim ds As New DataSet
		Dim sql As String = "Select SHIFT_QTY, CURRENT_QTY, SHIFT_ID, SHIFT_DATE from Mesdba.Machine_current_status where machine_id = " & machineID
		Dim da As New OracleDataAdapter(sql, con)

		Try

			da.Fill(ds)

			Return ds

		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			ds.Dispose()
			da.Dispose()
		End Try

	End Function

	Private Function GetShiftData(ByVal pShiftID As Int32, ByVal pShiftDate As Date, ByVal con As OracleConnection) As Boolean
		Dim cmd As New OracleCommand
		Dim r As System.Data.OracleClient.OracleString
		Dim shiftID As Int32
		Dim shiftName As String
		Dim shiftDate As Date

		Try

			With cmd
				.CommandType = CommandType.StoredProcedure
				.Connection = con
				.CommandText = "mesproc.machine_data.get_shift_data"
				.Parameters.Add("pnReportGroupId", OracleClient.OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnReportGroupId").Value = 873
				.Parameters.Add("pdEventTmstm", OracleType.DateTime).Direction = ParameterDirection.Input
				.Parameters("pdEventTmstm").Value = Now
				.Parameters.Add("pnDebug", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnDebug").Value = 1
				.Parameters.Add("pnShiftId", OracleType.Number).Direction = ParameterDirection.Output
				.Parameters("pnShiftId").Value = DBNull.Value
				.Parameters.Add("pdShiftDate", OracleType.DateTime).Direction = ParameterDirection.Output
				.Parameters("pdShiftDate").Value = DBNull.Value
				.Parameters.Add("pvShiftName", OracleType.VarChar, 30).Direction = ParameterDirection.Output
				.Parameters("pvShiftName").Value = DBNull.Value
				.Parameters.Add("pnSQLCode", OracleType.Number).Direction = ParameterDirection.Output
				.Parameters("pnSQLCode").Value = DBNull.Value
				.Parameters.Add("pvSQLMessage", OracleType.VarChar, 100).Direction = ParameterDirection.Output
				.Parameters("pvSQLMessage").Value = DBNull.Value
				.Parameters.Add("pnReturnCode", OracleType.Number).Direction = ParameterDirection.Output
				.Parameters("pnReturnCode").Value = DBNull.Value

				.ExecuteOracleNonQuery(r)

				shiftID = .Parameters("pnShiftId").Value
				shiftName = .Parameters("pvShiftName").Value
				shiftDate = .Parameters("pdShiftDate").Value

			End With

			If shiftID <> pShiftID OrElse shiftDate <> pShiftDate Then
				Return True
			Else
				Return False
			End If

		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			cmd.Dispose()
		End Try
	End Function

	Private Function UpDate(ByVal machineID As Long, ByVal mesPartID As Long, ByVal currentProduced As Integer, ByVal lCurrentShiftQty As Long, ByVal con As OracleConnection) As Boolean
		Dim cmd As New OracleCommand
		Dim r As System.Data.OracleClient.OracleString
		Dim lResetCounters As Int32

		Try
			With cmd
				.Connection = con
				.CommandType = CommandType.StoredProcedure
				.CommandText = "MESPROC.MACHINE_DATA.Update_Machine_Status"
				.Parameters.Add("pnMachineId", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnMachineID").Value = machineID
				.Parameters.Add("pnMESPartId", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnMESPartID").Value = mesPartID
				.Parameters.Add("pnToolId", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnToolID").Value = 0
				.Parameters.Add("pvLotNbr", OracleType.VarChar, 30).Direction = ParameterDirection.Input
				.Parameters("pvLotNbr").Value = "0"
				.Parameters.Add("pvOperatorId", OracleType.VarChar, 30).Direction = ParameterDirection.Input
				.Parameters("pvOperatorID").Value = Now.ToString
				.Parameters.Add("pnSpeed", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnSpeed").Value = 0
				.Parameters.Add("pnCurrentQty", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnCurrentQty").Value = currentProduced
				.Parameters.Add("pnShiftQty", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnShiftQty").Value = lCurrentShiftQty
				.Parameters.Add("pnContainerCount", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnContainerCount").Value = 0
				.Parameters.Add("pnScrapCount", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnScrapCount").Value = 0
				.Parameters.Add("pnRuntime", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnRuntime").Value = 0
				.Parameters.Add("pnIdletime", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnIdletime").Value = 0
				.Parameters.Add("pnDowntime", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnDowntime").Value = 0
				.Parameters.Add("pnShiftime", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnShiftime").Value = 0
				.Parameters.Add("pnInspectionRef", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnInspectionRef").Value = 0
				.Parameters.Add("pnFaultCount", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnFaultCount").Value = 0
				.Parameters.Add("pvStatus", OracleType.VarChar, 50).Direction = ParameterDirection.Input
				.Parameters("pvStatus").Value = "RUNNING"
				.Parameters.Add("pvModUserid", OracleType.VarChar, 30).Direction = ParameterDirection.Input
				.Parameters("pvModUserid").Value = "OperatorP19"
				.Parameters.Add("pdEventTime", OracleType.DateTime).Direction = ParameterDirection.Input
				.Parameters("pdEventTime").Value = Now
				.Parameters.Add("pnProductionRunId", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnProductionRunId").Value = DBNull.Value
				.Parameters.Add("pnScrapContainerCount", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnScrapContainerCount").Value = 0
				.Parameters.Add("pvMachineParmList", OracleType.VarChar, 100).Direction = ParameterDirection.Input
				.Parameters("pvMachineParmList").Value = DBNull.Value
				.Parameters.Add("pvMachineParmValues", OracleType.VarChar, 100).Direction = ParameterDirection.Input
				.Parameters("pvMachineParmValues").Value = DBNull.Value
				.Parameters.Add("pnUseCommitRollback", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnUseCommitRollback").Value = 0
				.Parameters.Add("pnForceNewHistory", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnForceNewHistory").Value = 0
				.Parameters.Add("pnDebug", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnDebug").Value = 1
				.Parameters.Add("pnResetCounters", OracleType.Number).Direction = ParameterDirection.Output
				.Parameters("pnResetCounters").Value = 0
				.Parameters.Add("pnSQLCode", OracleType.Number).Direction = ParameterDirection.Output
				.Parameters("pnSQLCode").Value = 0
				.Parameters.Add("pvSQLMessage", OracleType.VarChar, 100).Direction = ParameterDirection.Output
				.Parameters("pvSQLMessage").Value = ""
				.Parameters.Add("pnReturnCode", OracleType.Number).Direction = ParameterDirection.Output
				.Parameters("pnReturnCode").Value = 0
				.ExecuteOracleNonQuery(r)
				lResetCounters = .Parameters.Item("pnResetCounters").Value
				If lResetCounters > 0 Then
					Select Case lResetCounters
						Case 1						   'Shift Change
							lCurrentShiftQty = 1
							ResetCounters(con, mesPartID, machineID, currentProduced, lCurrentShiftQty)
						Case 2						   'Part change
							'ResetCounters(con, mesPartID, lMachineID, currentProduced, lCurrentShiftQty)
					End Select
				End If

				Dim s As String
				s = .Parameters.Item("pvSQLMessage").Value

			End With

			Return True

		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			cmd.Dispose()
		End Try
	End Function

	Private Sub ResetCounters(ByVal con As OracleConnection, ByVal mesPartID As Long, ByVal machineID As Long, ByVal currentProduced As Long, ByVal shiftQty As Long)
		Dim cmd As New OracleClient.OracleCommand

		Try
			'Open the connection if it's closed
			If con.State = ConnectionState.Closed Then con.Open()

			With cmd
				.Connection = con
				.CommandType = CommandType.StoredProcedure
				.CommandText = "MESPROC.MACHINE_DATA.Update_Machine_Status"
				.Parameters.Add("pnMachineId", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnMachineID").Value = machineID
				.Parameters.Add("pnMESPartId", OracleType.Number).Direction = ParameterDirection.Input
				If machineID = 0 Then
					.Parameters("pnMESPartID").Value = DBNull.Value
				Else
					.Parameters("pnMESPartID").Value = DBNull.Value
				End If
				.Parameters.Add("pnToolId", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnToolID").Value = 0
				.Parameters.Add("pvLotNbr", OracleType.VarChar, 30).Direction = ParameterDirection.Input
				.Parameters("pvLotNbr").Value = "0"
				.Parameters.Add("pvOperatorId", OracleType.VarChar, 30).Direction = ParameterDirection.Input
				.Parameters("pvOperatorID").Value = Now.ToString
				.Parameters.Add("pnSpeed", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnSpeed").Value = 0
				.Parameters.Add("pnCurrentQty", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnCurrentQty").Value = currentProduced
				.Parameters.Add("pnShiftQty", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnShiftQty").Value = shiftQty
				.Parameters.Add("pnContainerCount", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnContainerCount").Value = 0
				.Parameters.Add("pnScrapCount", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnScrapCount").Value = 0
				.Parameters.Add("pnRuntime", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnRuntime").Value = 0
				.Parameters.Add("pnIdletime", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnIdletime").Value = 0
				.Parameters.Add("pnDowntime", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnDowntime").Value = 0
				.Parameters.Add("pnShiftime", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnShiftime").Value = 0
				.Parameters.Add("pnInspectionRef", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnInspectionRef").Value = 0
				.Parameters.Add("pnFaultCount", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnFaultCount").Value = 0
				.Parameters.Add("pvStatus", OracleType.VarChar, 50).Direction = ParameterDirection.Input
				.Parameters("pvStatus").Value = DBNull.Value
				.Parameters.Add("pvModUserid", OracleType.VarChar, 30).Direction = ParameterDirection.Input
				.Parameters("pvModUserid").Value = "OperatorP19"
				.Parameters.Add("pdEventTime", OracleType.DateTime).Direction = ParameterDirection.Input
				.Parameters("pdEventTime").Value = Now
				.Parameters.Add("pnProductionRunId", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnProductionRunId").Value = DBNull.Value
				.Parameters.Add("pnScrapContainerCount", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnScrapContainerCount").Value = 0
				.Parameters.Add("pvMachineParmList", OracleType.VarChar, 100).Direction = ParameterDirection.Input
				.Parameters("pvMachineParmList").Value = "1"
				.Parameters.Add("pvMachineParmValues", OracleType.VarChar, 100).Direction = ParameterDirection.Input
				.Parameters("pvMachineParmValues").Value = "1"
				.Parameters.Add("pnUseCommitRollback", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnUseCommitRollback").Value = 1
				.Parameters.Add("pnForceNewHistory", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnForceNewHistory").Value = 0
				.Parameters.Add("pnDebug", OracleType.Number).Direction = ParameterDirection.Input
				.Parameters("pnDebug").Value = 1
				.Parameters.Add("pnResetCounters", OracleType.Number).Direction = ParameterDirection.Output
				.Parameters("pnResetCounters").Value = 0
				.Parameters.Add("pnSQLCode", OracleType.Number).Direction = ParameterDirection.Output
				.Parameters("pnSQLCode").Value = 0
				.Parameters.Add("pvSQLMessage", OracleType.VarChar, 100).Direction = ParameterDirection.Output
				.Parameters("pvSQLMessage").Value = ""
				.Parameters.Add("pnReturnCode", OracleType.Number).Direction = ParameterDirection.Output
				.Parameters("pnReturnCode").Value = 0
				.ExecuteNonQuery()
			End With

		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			cmd.Dispose()
		End Try
	End Sub
End Class
