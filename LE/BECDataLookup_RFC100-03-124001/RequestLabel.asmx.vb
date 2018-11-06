Option Explicit On 

Imports System.Web.Services
Imports System.Data.OracleClient

<System.Web.Services.WebService(Namespace := "http://tempuri.org/BecWebService/RequestLabel")> _
Public Class RequestLabel
	Inherits System.Web.Services.WebService

	Public Enum StandardPackTypes
		ICS = 0
		MTMS
		ICS_MASTER
		MTMS_MASTER
	End Enum

	Private Structure MessageStructure
		Public PartNumber, Destination, OperatorId, PackageCode, LotNumber, ComponentIdentifiers() As String
		Public Machine, ResponseQueue, SerialNumber, Department, PrinterName, ECL As String
		Public StartTime, EndTime As Date
		Public Quantity, BoxCount As Integer
		Public LabelType As mesLabeling.Labeling.LabelTypes
		Public StandardPackType As StandardPackTypes
	End Structure

	Private moCN As OracleConnection
	Private moTransAction As OracleTransaction
	Const LOG_SOURCE = "BecWebService"


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
Public Function PrintLabel(ByVal dsRequest As DataSet) As String
		Dim oStandardPack As mesStandardPack.StandardPack
		Dim serialNumber As String
		Dim stMessage As MessageStructure
		Dim labelFile As String

		Try
			'Open up a Connection to the DataBase
			If moCN Is Nothing Then
				moCN = New OracleConnection(mesCommon.DataAccess.GetOracleConnectionString)			   'Common.GetConnectionString)
				moCN.Open()
			End If

			'This decodes the message from the queue and returns the variable in the structure
			Me.DecodeMessage(dsRequest.Tables(0), stMessage)

			Dim sMessage As New Text.StringBuilder
			With sMessage
				.Append("Request for ")
				.Append(stMessage.LabelType.ToString)
				.Append("; ")
				.Append(stMessage.StandardPackType.ToString)
				.Append("; ")
				.Append(stMessage.PartNumber)
				.Append("; ")
				.Append(stMessage.Destination)
				.Append("; ")
				.Append(stMessage.Department)
				.Append("; ")
				.Append(stMessage.Machine)
				.Append("; ")
				.Append(stMessage.EndTime.ToString)
				.Append("; ")
				.Append(stMessage.OperatorId)
			End With
			mesCommon.Logging.LogEvent(LOG_SOURCE, sMessage.ToString, EventLogEntryType.Information)

			'Start a Database TransAction
			moTransAction = moCN.BeginTransaction

			'Create and Print the Standard Pack
			Select Case stMessage.StandardPackType
				Case StandardPackTypes.ICS, StandardPackTypes.ICS_MASTER
					stMessage.Destination = CovertToDestinationCode(CInt(stMessage.Destination))
					If stMessage.PackageCode = Nothing Then
						stMessage.PackageCode = DeterminePackageCode(stMessage.PartNumber, stMessage.Destination)
					End If

					If stMessage.PackageCode Is Nothing Then
						sMessage = New Text.StringBuilder
						With sMessage
							.Append(mesCommon.Constants.RECORD_NOT_FOUND)
							.Append(" Package Code on Part = '")
							.Append(stMessage.PartNumber)
							.Append("'; CustomerCode = '")
							.Append(stMessage.Destination)
							.Append("' in the ICS Label Data Table.")
						End With
						Throw New ApplicationException(sMessage.ToString)
					End If

					'Need to Determine for Plant 12 if the Label Type should be ICS or ICS Internal
					'If the Box count is 1 then it is ICS else it is internal
					stMessage.BoxCount = mesStandardPack.MasterICSStandardPack.DetermineBoxCount(stMessage.PackageCode)

					If stMessage.BoxCount = 1 Then
						oStandardPack = New mesStandardPack.ICSStandardPack(moCN, moTransAction)
					ElseIf stMessage.StandardPackType = StandardPackTypes.ICS Then
						Dim oICSInternalStandardPack As New mesStandardPack.InternalICSStandardPack(moCN, moTransAction)
						oICSInternalStandardPack.NumberOfBoxesInMaster = stMessage.BoxCount
						oStandardPack = oICSInternalStandardPack
					ElseIf stMessage.Department.StartsWith("19") Then
						Dim oICSMasterPack As New mesStandardPack.Plant19MasterICSStandardPack(moCN, Me.moTransAction)
						oStandardPack = oICSMasterPack
					Else
						Dim oICSMasterPack As New mesStandardPack.MasterICSStandardPack(moCN, moTransAction)
						oStandardPack = oICSMasterPack
					End If
				Case StandardPackTypes.MTMS
					'Need to override the default of ICS
					If stMessage.Destination = "6" Then stMessage.Destination = ""

					If stMessage.LabelType = mesLabeling.Labeling.LabelTypes.Active Then
						oStandardPack = New mesStandardPack.MTMSStandardPack(moCN, moTransAction)
					Else
						Dim oMTMSInternalStandardPack As New mesStandardPack.InternalMTMSStandardPack(moCN, moTransAction)
						oMTMSInternalStandardPack.NumberOfBoxesInMaster = stMessage.BoxCount
						oStandardPack = oMTMSInternalStandardPack
					End If
				Case StandardPackTypes.MTMS_MASTER
					Dim oMTMSMasterStandardPack As New mesStandardPack.MasterMTMSStandardPack(moCN, moTransAction)
					oMTMSMasterStandardPack.TotalNumberOfStandardPacksInMaster = stMessage.BoxCount
					oStandardPack = oMTMSMasterStandardPack
			End Select

			With oStandardPack
				.Add()
				.PartNumber = stMessage.PartNumber

				'Only set the ECL if it was passed in.  If it is not passed the system will
				'get the latest.
				If Not stMessage.ECL Is Nothing Then .RevisionLevel = stMessage.ECL

				'The Package Code only needs set in ICS; MTMS retrieves it automatically
				If .LabelType = mesCommon.Enumerations.LabelingSystemTypes.ICS Or _
				 .LabelType = mesCommon.Enumerations.LabelingSystemTypes.ICS_Internal Or _
				 .LabelType = mesCommon.Enumerations.LabelingSystemTypes.ICS_MasterPack Then

					.PackageCode = stMessage.PackageCode
				Else
					'Quantity only needs to be set for MTMS; ICS retieves it automatically
					.Quantity = stMessage.Quantity
				End If

				.Destination = stMessage.Destination
				.Department = stMessage.Department
				.Machine = stMessage.Machine
				.TimeProduced = stMessage.EndTime
				.UserId = stMessage.OperatorId
				.LottNumber = stMessage.LotNumber
				.ComponentIdentifier = stMessage.ComponentIdentifiers
				.Update()
				stMessage.SerialNumber = .SerialNumber
				'.PrintLabel(.SerialNumber, stMessage.LabelType)
				labelFile = .PrinterStream(.SerialNumber, stMessage.LabelType, mesLabeling.Printer.PrinterTypes.Zebra, mesLabeling.Printer.PrinterDensities.TwoHundred)
			End With

			'Commit the Transaction
			moTransAction.Commit()
			moTransAction = Nothing

			sMessage = New Text.StringBuilder
			With sMessage
				.Append("Serial Number ")
				.Append(stMessage.SerialNumber)
				.Append(" sent to Printer ")
				.Append(stMessage.Machine)
			End With

			mesCommon.Logging.LogEvent(LOG_SOURCE, sMessage.ToString, EventLogEntryType.Information)

			Return labelFile

		Catch ex As Exception When ex.Message = "Mainframe Error: S/N ALREADY PRODUCED"
			mesCommon.Logging.LogEvent(LOG_SOURCE, stMessage.SerialNumber & " " & ex.Message, EventLogEntryType.Error)

			oStandardPack.ChangeDisposition(serialNumber, mesStandardPack.StandardPack.DispositionCodes.UNKNOWN, "Plant12ControlUnit")

			moTransAction.Commit()

		Catch ex As Exception
			If Not moTransAction Is Nothing Then moTransAction.Rollback()

			mesCommon.Logging.LogEvent(ex)
			Throw New Exception(ex.Message)

		Finally
			moTransAction = Nothing
			If Not moCN Is Nothing AndAlso moCN.State = ConnectionState.Open Then moCN.Close()
			moCN = Nothing
			oStandardPack = Nothing

		End Try
	End Function

	Private Sub DecodeMessage(ByVal dtInfo As DataTable, ByRef Message As MessageStructure)
		With dtInfo
			If .Columns.Contains("Disposition") Then
				Select Case CStr(.Rows(0)("Disposition")).ToUpper
					Case "INACTIVE"
						Message.LabelType = mesLabeling.Labeling.LabelTypes.InActive
						'TODO: Additional Dispositions
					Case "ACTIVE"
						Message.LabelType = mesLabeling.Labeling.LabelTypes.Active
					Case "QUARANTINE"
						Message.LabelType = mesLabeling.Labeling.LabelTypes.Containment
					Case "SCRAP"
						Message.LabelType = mesLabeling.Labeling.LabelTypes.Scrap
					Case "REPRINT"
						Message.LabelType = mesLabeling.Labeling.LabelTypes.Reprint
					Case Else
						Throw New ApplicationException("Invalid Label Label Type of " & CStr(.Rows(0)("Disposition")))
				End Select
			Else
				Message.LabelType = mesLabeling.Labeling.LabelTypes.InActive
			End If

			'Determine the Label Type
			Select Case CStr(.Rows(0)("Label Type")).ToUpper
				Case "GOOD", "ICS"
					Message.StandardPackType = StandardPackTypes.ICS
				Case "MTMS"
					Message.StandardPackType = StandardPackTypes.MTMS
				Case "MTMS MASTER"
					Message.StandardPackType = StandardPackTypes.MTMS_MASTER
				Case "ICS MASTER"
					Message.StandardPackType = StandardPackTypes.ICS_MASTER
				Case Else
					Throw New ApplicationException("Invalid Standard Pack Type of " & CStr(.Rows(0)("Label Type")) & _
					 " was received.")
			End Select

			Message.PartNumber = CStr(.Rows(0)("Part Number"))

			'Determine the Machine; If the request came from Final Assembly then the row
			'will contain the "Tester Id" column; if it came from the molding area, it will
			'contain "Machine Name" column
			If .Columns.Contains("Machine Name") Then
				If IsDBNull("Machine Name") Then
					Message.Machine = Nothing
				Else
					Message.Machine = CStr(.Rows(0)("Machine Name"))
				End If
			Else
				'Dim oMachine As New mesMachine.FinalAssemblyBoard(moCN)
				'oMachine.RetrieveItemByTester(CInt(.Rows(0)("Tester Id")))
				'Message.Machine = oMachine.Name
			End If

			'Convert the Machine to a department
			Message.Department = DetermineDepartment(Message.Machine)

			Message.Quantity = CInt(.Rows(0)("Quantity"))

			Try
				Message.StartTime = CDate(.Rows(0)("Start Time"))
			Catch ex As Exception
				Message.StartTime = Now
			End Try

			Try
				Message.EndTime = CDate(.Rows(0)("End Time"))
			Catch ex As Exception
				Message.EndTime = Now
			End Try

			If .Columns.Contains("Operator Id") AndAlso Not IsDBNull(.Rows(0)("Operator Id")) Then
				Message.OperatorId = CStr(.Rows(0)("Operator Id"))
			Else
				Message.OperatorId = ""
			End If

			If .Columns.Contains("Customer Code") AndAlso Not IsDBNull(.Rows(0)("Customer Code")) AndAlso _
			 CStr(.Rows(0)("Customer Code")).Trim <> "" Then
				Message.Destination = CStr(.Rows(0)("Customer Code"))
			Else
				'this is default for ICS standard Label format
				'this is needed to set for Plants 19 & 46
				Message.Destination = "6"
			End If

			If .Columns.Contains("Destination") AndAlso Not IsDBNull(.Rows(0)("Destination")) AndAlso _
			 CStr(.Rows(0)("Destination")).Trim <> "" Then Message.Destination = CStr(.Rows(0)("Destination"))

			If .Columns.Contains("Box Count") Then
				Try
					Message.BoxCount = CInt(.Rows(0)("Box Count"))
				Catch ex As Exception
					Message.BoxCount = 1
				End Try
			Else
				Message.BoxCount = 1
			End If

			If .Columns.Contains("Lot Number") AndAlso Not IsDBNull(.Rows(0)("Lot Number")) Then
				Message.LotNumber = CStr(.Rows(0)("Lot Number"))
			Else
				Message.LotNumber = Nothing
			End If

			If .Columns.Contains("Component Identifiers") AndAlso Not IsDBNull(.Rows(0)("Component Identifiers")) Then
				Message.ComponentIdentifiers = CStr(.Rows(0)("Component Identifiers")).Split(","c)
				If Message.ComponentIdentifiers.GetUpperBound(0) = 0 And Message.ComponentIdentifiers(0).Trim = "" Then
					Message.ComponentIdentifiers = Nothing
				End If
			Else
				Message.ComponentIdentifiers = Nothing
			End If

			If Not .Columns.Contains("Response Queue") OrElse IsDBNull(.Rows(0)("Response Queue")) Then
				Message.ResponseQueue = ""
			Else
				Message.ResponseQueue = CStr(.Rows(0)("Response Queue")).Trim
			End If

			If .Columns.Contains("Printer") AndAlso Not IsDBNull(.Rows(0)("Printer")) AndAlso _
			 CStr(.Rows(0)("Printer")) <> "" Then
				Message.PrinterName = CStr(.Rows(0)("Printer"))
			Else
				Message.PrinterName = Nothing
			End If

			If .Columns.Contains("Package Code") AndAlso Not IsDBNull(.Rows(0)("Package Code")) AndAlso _
			 CStr(.Rows(0)("Package Code")) <> "" Then
				Message.PackageCode = CStr(.Rows(0)("Package Code"))
			Else
				Message.PackageCode = Nothing
			End If

			If .Columns.Contains("ECL") AndAlso Not IsDBNull(.Rows(0)("ECL")) AndAlso _
			 CStr(.Rows(0)("ECL")) <> "" Then
				Message.ECL = CStr(.Rows(0)("ECL"))
			Else
				Message.ECL = Nothing
			End If
		End With
	End Sub

	Private Function DetermineDepartment(ByRef MachineName As String) As String
		Dim oCommand As New OracleCommand("SELECT MG_DEPT FROM MESDBA.MACHINE WHERE MACHINE_NAME = '" & _
		 MachineName & "'", moCN, moTransAction)

		Return CStr(oCommand.ExecuteScalar)

		oCommand.Dispose()

	End Function

	Private Function CovertToDestinationCode(ByVal DestinationValue As Integer) As String
		Dim oCodes As New mesPlant12Destinations.DestinationCodes(moCN, moTransAction)

		oCodes.RetrieveItem(CLng(DestinationValue))

		Try
			Dim sCode As String = oCodes.DestinationCode

			'Can remove this as soon as the database is updated.
			If sCode.ToUpper = "NULL" Then
				Return Nothing
			Else
				Return sCode
			End If
		Catch ex As Exception
			Return Nothing
		Finally
			oCodes = Nothing
		End Try
	End Function

	Private Function DeterminePackageCode(ByRef PartNumber As String, ByRef Destination As String) As String
		Dim sSQL As New Text.StringBuilder

		With sSQL
			.Append("SELECT UNIQUE(PACKAGE_CODE) FROM MESDBA.VW_ICS_LABEL_DATA WHERE PART_NBR = '")
			.Append(PartNumber)
			.Append("' AND DESTINATION ")
			If Destination Is Nothing OrElse Destination.Trim = "" Then
				.Append("IS NULL")
			Else
				.Append("= '")
				.Append(Destination)
				.Append("'")
			End If
		End With

		Dim oCommand As New OracleCommand(sSQL.ToString, moCN, moTransAction)

		Return CStr(oCommand.ExecuteScalar)

		sSQL = Nothing
	End Function
End Class
