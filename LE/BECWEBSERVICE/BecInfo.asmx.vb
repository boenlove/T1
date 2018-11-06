Imports System.Web.Services
Imports System.Data.OracleClient
Imports System.Text
Imports System.Configuration

<System.Web.Services.WebService(Namespace:="http://tempuri.org/BecWebService/BecWebService")> _
Public Class BecInfo
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

	<WebMethod()> _
	Public Function GetDate() As Date
		Return Now
	End Function

	<WebMethod()> _
	Public Function RetrieveReleaseInformation(ByVal machineName As String, ByVal mesPartID As Long) As DataSet
		Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()		'  GetConnectionString()
		Dim con As New OracleConnection(conString)
		Dim da As OracleDataAdapter
        Dim ds As New Data.DataSet
        Dim sql As New StringBuilder
		Dim schema As String = GetSchemaName()

		Try
			' SQL to see if a part number has been released to the machine
			sql.Append("SELECT SETUP.DESCR, PV.VALUE ")
			sql.Append("From ")
			sql.Append(schema & "MACHINE M, ")
			sql.Append(schema & "PROCESS_CONFIG PC, ")
			sql.Append(schema & "SETUP_PARAMETER SETUP, ")
			sql.Append(schema & "PROCESS_VALUE PV ")
			sql.Append("Where ")
			sql.Append("M.MACHINE_NAME = '" & machineName.ToUpper & "' AND PC.MES_PART_ID = " & mesPartID & " AND ")
			sql.Append("PC.MACHINE_GROUP_NAME = M.MACHINE_GROUP_NAME AND ")
			sql.Append("PV.PROCESS_CONFIG_ID = PC.PROCESS_CONFIG_ID And SETUP.PARAMETER_ID = PV.PARAMETER_ID")

			'Check if the connection is open and open it if the state is closed
			If con.State = ConnectionState.Closed Then con.Open()

			da = New OracleDataAdapter(sql.ToString, con)
			da.Fill(ds)
            
            Return ds

        Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			da.Dispose()
			ds.Dispose()
			con.Dispose()
		End Try
	End Function

	<WebMethod()> _
	Public Function GetCurrentLotNbr(ByVal psMachineName As String) As String
		Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()		 ' GetConnectionString()
		Dim con As New OracleConnection(conString)
		Dim dr As OracleDataReader
		Dim cmd As New OracleCommand
		Dim sSQL As String
		Dim sCurrentLotNbr As String
		Dim schema As String = GetSchemaName()

		Try
			sSQL = "SELECT CL.LOT_NBR From " & schema & "MACHINE_CURRENT_LOT CL, " & schema & "MACHINE M "
			sSQL = sSQL & "Where M.MACHINE_NAME = '" & psMachineName.ToUpper & "' AND M.MACHINE_ID = CL.MACHINE_ID"

			'Open the connection if it is closed
			If con.State = ConnectionState.Closed Then con.Open()

			'Command object properties
			With cmd
				.Connection = con
				.CommandType = CommandType.Text
				.CommandText = sSQL
			End With

			'Execute the reader
			dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

			'Check to see if anything was returned and set the variable to the value
			If dr.HasRows Then
				While (dr.Read())
					sCurrentLotNbr = dr.Item("LOT_NBR")
				End While
			Else
				sCurrentLotNbr = ""
			End If

			'Return the value or an empty string if nothing was returned from the database
			Return sCurrentLotNbr

		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			con.Dispose()
			cmd.Dispose()
			dr.Dispose()
		End Try

	End Function

	<WebMethod()> _
	Public Function GetMTMSLocation(ByVal psPartNbr As String) As DataSet
		Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()		 ' GetConnectionString()
		Dim con As New OracleConnection(conString)
		Dim da As OracleDataAdapter
		Dim sql As New StringBuilder
		Dim schema As String = GetSchemaName()
		Dim ds As New DataSet

		Try

			' define the SQL statement
			sql.Append("SELECT IL.NAME ")
			sql.Append("FROM ")
			sql.Append(schema & "INVENTORY_LOCATION IL, ")
			sql.Append(schema & "PART_LOCATION PL, ")
			sql.Append(schema & "MES_PART MP ")
			sql.Append("WHERE IL.INVENTORY_LOCATION_ID = PL.INVENTORY_LOCATION_ID AND ")
			sql.Append("MP.MES_PART_ID = PL.MES_PART_ID AND ")
			sql.Append("MP.PART_NBR = '" & psPartNbr & "'")

			'Open the connection if it is closed
			If con.State = ConnectionState.Closed Then con.Open()

			da = New OracleDataAdapter(sql.ToString, con)
			'Fill the dataset
			da.Fill(ds)

			Return ds

		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			da.Dispose()
			ds.Dispose()
			con.Dispose()
			sql = Nothing
		End Try
	End Function	' GetMTMSLocation

	'*****************************************************************************
	'* Function...: GetPartNumber
	'* Purpose....: Get the part number associated with a broadcast code.
	'* Parameters.: psBroadcastCode (String)
	'*                Broadcast code
	'* Returns....: Part number
	'*****************************************************************************
	<WebMethod()> _
	Public Function GetPartNumber(ByVal psBroadcastCode As String) As DataSet

		' local variables
		Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()		'  GetConnectionString()
		Dim con As New OracleConnection(conString)
		Dim cmd As New OracleCommand
		Dim ds As New DataSet
		Dim da As OracleDataAdapter
		Dim sSQL As String
		Dim schema As String = GetSchemaName()
		'Dim partNumber As String

		Try

			' define the SQL statement
			sSQL = "select p.part_nbr, p.mes_part_id from "
			sSQL = sSQL & schema & "mes_part p, " & schema & "production_run pr "
			sSQL = sSQL & "where p.MES_PART_ID = pr.MES_PART_ID and pr.PROCESS_ID = '" & psBroadcastCode & "'"

			'Open the connection
			If con.State = ConnectionState.Closed Then con.Open()

			' get the information
			With cmd
				.Connection = con
				.CommandType = CommandType.Text
				.CommandText = sSQL
				.ExecuteNonQuery()
			End With

			'fill the dataset
			da = New OracleDataAdapter(cmd)
			da.Fill(ds)

			Return ds

		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			con.Dispose()
			cmd.Dispose()
			ds.Dispose()
			da.Dispose()
		End Try
	End Function	' GetPartNumber

	<WebMethod()> _
	Public Function GetStandardPackDetails(ByVal broadCastCode As String, _
	  ByVal container As String, _
	  ByVal labelType As String, _
	  ByVal ECL As String) As DataSet

		' local variables
		Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()		 ' GetConnectionString()
		Dim con As New OracleConnection(conString)
		Dim cmd As New OracleCommand
		Dim ds As New DataSet
		Dim da As OracleDataAdapter
		Dim sSQL As String
		Dim schema As String = GetSchemaName()
		Dim sContainerType As String

		Try

			' define the SQL statement
			Select Case labelType
				Case "ICS"
					sSQL = "SELECT PR.PRODUCTION_RUN_ID, D.DEPT, PR.SYSTEM_FLAG, PR.mes_part_id, PR.PACKAGE_CODE, PR.STD_PACK_QTY AS STANDARD_PACK, "
					sSQL = sSQL & "PR.TOTAL_PART_QTY AS BOXCOUNT, PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT, PR.CONTAINER_CODE "
					sSQL = sSQL & "FROM MESDBA.PRODUCTION_RUN PR, MESDBA.VW_PACKAGE_INFORMATION VW, "
					sSQL = sSQL & "MESDBA.VW_DEPARTMENT D "
					sSQL = sSQL & "WHERE PR.PROCESS_ID = '" & broadCastCode & "' "
					sSQL = sSQL & "AND PR.SYSTEM_FLAG = 'I' AND D.ID = PR.REPORT_GROUP_ID "
					sSQL = sSQL & "AND PR.SYSTEM_FLAG = VW.SYSTEM_FLAG "
					sSQL = sSQL & "AND PR.REV_PHYSICAL = '" & ECL & "' "
					If container = "Returnable" Then
						sContainerType = "R"
					Else
						sContainerType = "E"
					End If
					sSQL = sSQL & " AND VW.RETURNABLE_EXPENDIBLE_FLAG = '" & sContainerType & "' "
					sSQL = sSQL & "AND VW.PKG_CNT_TYPE = PR.PACKAGE_CODE"

				Case "MTMS"
					sSQL = "SELECT PR.PRODUCTION_RUN_ID, D.DEPT, PR.SYSTEM_FLAG, PR.mes_part_id, PR.PACKAGE_CODE, PR.STD_PACK_QTY AS STANDARD_PACK, "
					sSQL = sSQL & "PR.TOTAL_PART_QTY AS BOXCOUNT, PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT, PR.CONTAINER_CODE "
					sSQL = sSQL & "FROM MESDBA.PRODUCTION_RUN PR, MESDBA.VW_PACKAGE_INFORMATION VW, "
					sSQL = sSQL & "MESDBA.VW_DEPARTMENT D "
					sSQL = sSQL & "WHERE PR.PROCESS_ID = '" & broadCastCode & "' "
					sSQL = sSQL & "AND PR.SYSTEM_FLAG = 'M' AND D.ID = PR.REPORT_GROUP_ID "
					sSQL = sSQL & "AND PR.SYSTEM_FLAG = VW.SYSTEM_FLAG "
					sSQL = sSQL & "AND PR.REV_PHYSICAL = '" & ECL & "' "
					If container = "Returnable" Then
						sContainerType = "R"
					Else
						sContainerType = "E"
					End If
					sSQL = sSQL & " AND VW.RETURNABLE_EXPENDIBLE_FLAG = '" & sContainerType & "' "
					sSQL = sSQL & "AND VW.PKG_CNT_TYPE = PR.PACKAGE_CODE"
				Case "PILOT"
					sSQL = "SELECT PR.PRODUCTION_RUN_ID, PR.mes_part_id, PR.PACKAGE_CODE, PR.STD_PACK_QTY AS STANDARD_PACK, PR.TOTAL_PART_QTY AS BOXCOUNT, PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT, " & _
					  "PR.SYSTEM_FLAG " & _
					 "FROM MESDBA.PRODUCTION_RUN PR " & _
					 "WHERE PR.PROCESS_ID = " & broadCastCode
			End Select			 ' psBroadcastLabelType

			'Open the connection
			If con.State = ConnectionState.Closed Then con.Open()

			' get the information
			With cmd
				.Connection = con
				.CommandType = CommandType.Text
				.CommandText = sSQL
				.ExecuteNonQuery()
			End With

			'fill the dataset
			da = New OracleDataAdapter(cmd)
			da.Fill(ds)

			Return ds
		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			con.Dispose()
			cmd.Dispose()
			ds.Dispose()
			da.Dispose()
		End Try

	End Function	' GetStandardPackDetails

	<WebMethod()> _
	   Public Function GetStandardPackDetailsSat(ByVal broadCastCode As String, ByVal ECL As String) As DataSet
		' local variables
		Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()		 ' GetConnectionString()
		Dim con As New OracleConnection(conString)
		Dim cmd As New OracleCommand
		Dim ds As New DataSet
		Dim da As OracleDataAdapter
		Dim sSQL As String
		Dim schema As String = GetSchemaName()
		Dim sContainerType As String

		Try

			' define the SQL statement
			sSQL = "SELECT D.DEPT, PR.SYSTEM_FLAG, PR.mes_part_id, PR.PACKAGE_CODE, "
			sSQL = sSQL & "PR.STD_PACK_QTY AS STANDARD_PACK, PR.TOTAL_PART_QTY AS BOXCOUNT, "
			sSQL = sSQL & "PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT "
			sSQL = sSQL & "FROM MESDBA.PRODUCTION_RUN PR, MESDBA.VW_DEPARTMENT D "
			sSQL = sSQL & "WHERE PR.PROCESS_ID = '" & broadCastCode & "' AND "
			sSQL = sSQL & "D.ID = PR.REPORT_GROUP_ID AND PR.REV_PHYSICAL = '" & ECL & "'"

			'Open the connection
			If con.State = ConnectionState.Closed Then con.Open()

			' get the information
			With cmd
				.Connection = con
				.CommandType = CommandType.Text
				.CommandText = sSQL
				.ExecuteNonQuery()
			End With

			'fill the dataset
			da = New OracleDataAdapter(cmd)
			da.Fill(ds)

			Return ds
		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			con.Dispose()
			cmd.Dispose()
			ds.Dispose()
			da.Dispose()
		End Try

	End Function	' GetStandardPackDetails

	<WebMethod()> _
	   Public Function GetStandardPackDetailsMold(ByVal broadCastCode As String, ByVal ECL As String, ByVal systemFlag As String) As DataSet
		' local variables
		Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()		 ' GetConnectionString()
		Dim con As New OracleConnection(conString)
		Dim cmd As New OracleCommand
		Dim ds As New DataSet
		Dim da As OracleDataAdapter
		Dim sSQL As String
		Dim schema As String = GetSchemaName()
		Dim sContainerType As String

		Try

			' define the SQL statement
			sSQL = "SELECT D.DEPT, PR.SYSTEM_FLAG, PR.mes_part_id, PR.PACKAGE_CODE, "
			sSQL = sSQL & "PR.STD_PACK_QTY AS STANDARD_PACK, PR.TOTAL_PART_QTY AS BOXCOUNT, "
			sSQL = sSQL & "PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT "
			sSQL = sSQL & "FROM MESDBA.PRODUCTION_RUN PR, MESDBA.VW_DEPARTMENT D "
			sSQL = sSQL & "WHERE PR.PROCESS_ID = '" & broadCastCode & "' AND "
			sSQL = sSQL & "D.ID = PR.REPORT_GROUP_ID AND PR.REV_PHYSICAL = '" & ECL & "'" & " AND PR.SYSTEM_FLAG = '" & systemFlag & "'"

			'Open the connection
			If con.State = ConnectionState.Closed Then con.Open()

			' get the information
			With cmd
				.Connection = con
				.CommandType = CommandType.Text
				.CommandText = sSQL
				.ExecuteNonQuery()
			End With

			'fill the dataset
			da = New OracleDataAdapter(cmd)
			da.Fill(ds)

			Return ds
		Catch ex As Exception
			Throw New Exception(ex.Message)
		Finally
			con.Dispose()
			cmd.Dispose()
			ds.Dispose()
			da.Dispose()
		End Try

	End Function	' GetStandardPackDetails


End Class
