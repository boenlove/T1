Imports System.Web.Services
'Imports System.Data.OracleClient
Imports System.Text
Imports System.Configuration
Imports Oracle.ManagedDataAccess.Client

'Imports Oracle.ManagedDataAccess.Client

<System.Web.Services.WebService(Namespace:="http://tempuri.org/BecWebService/BecWebService")> _
Public Class BECDATALOOKUP
    Inherits System.Web.Services.WebService

#Region " Web Services Designer Generated Code "
    'Dim OracleDbType As Object
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

        '### DSM fix
        'Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()  '  GetConnectionString()
        Dim conString As String = Common.GetConnectionString()

        'Dim conString As String = "ddd"
        Dim con As New OracleConnection(conString)
        Dim da As OracleDataAdapter
        Dim ds As New DataSet
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
            con.Close()
            con.Dispose()
        End Try
    End Function

    <WebMethod()> _
    Public Function GetCurrentLotNbr(ByVal psMachineName As String) As String

        '### DSM fix
        'Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()   ' GetConnectionString()
        Dim conString As String = Common.GetConnectionString()

        'Dim conString As String = "ddd"
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
            con.Close()
            con.Dispose()
            cmd.Dispose()
            dr.Dispose()
        End Try

    End Function

    <WebMethod()> _
    Public Function GetMTMSLocation(ByVal psPartNbr As String) As DataSet

        '###DSM fix
        'Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()   ' GetConnectionString()
        Dim conString As String = Common.GetConnectionString()

        'Dim conString As String = "ddd"
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
            con.Close()
            con.Dispose()
            sql = Nothing
        End Try
    End Function ' GetMTMSLocation

    '*****************************************************************************
    '* Function...: GetPartNumber
    '* Purpose....: Get the part number associated with a broadcast code.
    '* Parameters.: psBroadcastCode (String)
    '*                Broadcast code
    '* Returns....: Part number
    '*****************************************************************************

    Public Function GetPartNumber(ByVal psBroadcastCode As String, ByRef cnOra As OracleConnection) As DataSet

        ' local variables
        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim sSQL As String
        Dim schema As String
        'Dim partNumber As String

        Try
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.Open()
            End If
            schema = GetSchemaName()
            ' define the SQL statement
            sSQL = "select p.part_nbr, p.mes_part_id, pr.rev_physical from "
            sSQL = sSQL & schema & "mes_part p, " & schema & "production_run pr "
            sSQL = sSQL & "where p.MES_PART_ID = pr.MES_PART_ID and pr.PROCESS_ID = '" & psBroadcastCode & "'"

            ' get the information
            With cmd
                .Connection = cnOra
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
            'because we pass an open connection into this, we don't close it
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try
    End Function ' GetPartNumber


    Public Function GetStandardPackDetails(ByVal broadCastCode As String, ByVal container As String, ByVal labelType As String, ByVal ECL As String, ByVal Dept As String, ByRef cnOra As OracleConnection) As DataSet

        ' local variables
        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim SQL As New System.Text.StringBuilder
        Dim sContainerType As String

        Try

            ' define the SQL statement
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.Open()
            End If

            Select Case labelType
                Case "ICS"
                    '10/17/05  PAW added this code for getting standard pack details, instead of the other, which was very poor on speed.
                    SQL.Append("SELECT PR.PRODUCTION_RUN_ID, PR.SYSTEM_FLAG, PR.mes_part_id, PR.PACKAGE_CODE, PR.STD_PACK_QTY AS STANDARD_PACK, ")
                    SQL.Append(" PR.TOTAL_PART_QTY AS BOXCOUNT, PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT, PR.CONTAINER_CODE, Pr.Report_Group_ID ")
                    SQL.Append(" FROM MESDBA.PRODUCTION_RUN PR ")
                    SQL.Append(" WHERE PR.PROCESS_ID = '" & broadCastCode & "' ")
                    SQL.Append(" AND PR.REV_PHYSICAL = '" & ECL & "' ")
                    If container = "Returnable" Then
                        Select Case Dept
                            Case "2341", "02341"
                                'Jerry bdu,uec,LE
                                'SQL.Append(" And pr.package_code = 'Y' And pr.report_group_id = 1125 ")
                                'SQL.Append(" And pr.package_code IN ( 'UC','BU','Y') And pr.report_group_id = 1125 ")
                                SQL.Append(" And pr.package_code IN ( 'UC','BU','Y','LE') And pr.report_group_id = 1125 ")
                            Case "2342", "02342"
                                SQL.Append(" And pr.package_code = 'Z' And pr.report_group_id = 1126 ")
                            Case "2343", "02343"
                                SQL.Append(" And pr.package_code = 'Z' And pr.report_group_id = 1127  ")
                                '## to DEBUG ##
                                'SQL.Append(" And pr.package_code = 'Y' And pr.report_group_id = 1125  ")
                        End Select
                    Else
                        Select Case Dept
                            Case "2341", "02341"
                                SQL.Append(" And pr.package_code = '4C' And pr.report_group_id = 1125 ")
                                'SQL.Append(" And pr.package_code IN ( 'UC','BU','4C') And pr.report_group_id = 1125 ")
                            Case "2342", "02342"
                                SQL.Append(" And pr.package_code = 'C' And pr.report_group_id = 1126 ")
                            Case "2343", "02343"
                                SQL.Append(" And pr.package_code = 'C' And pr.report_group_id = 1127 ")
                                '### FOR DEBUG PURPOSES ###
                                'SQL.Append(" And pr.package_code = '4C' And pr.report_group_id = 1125 ")
                        End Select
                    End If

                Case "MTMS"
                    SQL.Append("SELECT PR.PRODUCTION_RUN_ID, D.DEPT, PR.SYSTEM_FLAG, PR.mes_part_id, PR.PACKAGE_CODE, PR.STD_PACK_QTY AS STANDARD_PACK, ")
                    SQL.Append("PR.TOTAL_PART_QTY AS BOXCOUNT, PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT, PR.CONTAINER_CODE ")
                    SQL.Append("FROM MESDBA.PRODUCTION_RUN PR, MESDBA.VW_PACKAGE_INFORMATION VW, ")
                    SQL.Append("MESDBA.VW_DEPARTMENT D ")
                    SQL.Append("WHERE PR.PROCESS_ID = '" & broadCastCode & "' ")
                    SQL.Append("AND PR.SYSTEM_FLAG = 'M' AND D.ID = PR.REPORT_GROUP_ID ")
                    SQL.Append("AND PR.SYSTEM_FLAG = VW.SYSTEM_FLAG ")
                    SQL.Append("AND PR.REV_PHYSICAL = '" & ECL & "' ")
                    If container = "Returnable" Then
                        sContainerType = "R"
                    Else
                        sContainerType = "E"
                    End If
                    SQL.Append(" AND VW.RETURNABLE_EXPENDIBLE_FLAG = '" & sContainerType & "' ")
                    SQL.Append("AND VW.PKG_CNT_TYPE = PR.PACKAGE_CODE")
                Case "PILOT"
                    SQL.Append("SELECT PR.PRODUCTION_RUN_ID, PR.mes_part_id, PR.PACKAGE_CODE, PR.STD_PACK_QTY AS STANDARD_PACK, PR.TOTAL_PART_QTY AS BOXCOUNT, PR.STD_PACK_COMPLETE_QTY AS LAYER_COUNT, ")
                    SQL.Append("PR.SYSTEM_FLAG ")
                    SQL.Append("FROM MESDBA.PRODUCTION_RUN PR ")
                    SQL.Append("WHERE PR.PROCESS_ID = ")
                    SQL.Append(broadCastCode)
            End Select    ' psBroadcastLabelType


            ' get the information
            With cmd
                .Connection = cnOra
                .CommandType = CommandType.Text
                .CommandText = SQL.ToString
                .ExecuteNonQuery()
            End With

            'fill the dataset
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)

            Return ds
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            'because we pass in the open connection, we don't close it
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try

    End Function ' GetStandardPackDetails

    <WebMethod()> _
    Public Function GetStandardPackDetailsSat(ByVal broadCastCode As String, ByVal ECL As String) As DataSet
        ' local variables
        '### DSM fix
        'Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()   ' GetConnectionString()
        Dim conString As String = Common.GetConnectionString()

        'Dim conString As String = "ddd"
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
            con.Close()
            con.Dispose()
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try

    End Function ' GetStandardPackDetails

    <WebMethod()> _
    Public Function GetStandardPackDetailsMold(ByVal broadCastCode As String, ByVal ECL As String, ByVal systemFlag As String) As DataSet
        ' local variables
        '### DSM fix
        'Dim conString As String = mesCommon.DataAccess.GetOracleConnectionString()   ' GetConnectionString()
        Dim conString As String = Common.GetConnectionString()

        'Dim conString As String = "ddd"
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
            con.Close()
            con.Dispose()
            cmd.Dispose()
            ds.Dispose()
            da.Dispose()
        End Try

    End Function ' GetStandardPackDetails
    Public Function GetCustomerPNfromICStable(ByVal DelphiPN As String, ByVal Container_code As String, ByRef cnOra As OracleConnection) As String

        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim sSQL As String
        Dim schema As String = GetSchemaName()
        Dim sContainerType As String

        Try
            If cnOra.State = ConnectionState.Closed Then
                cnOra.Open()
            End If
            If Trim(Container_code) = "" Then
                sSQL = "Select Part_nbr,Customer_Part_Nbr, Destination from Pltfloor.Ics_label_data_vw where Part_nbr = '" & DelphiPN & "' and Destination is null and rownum =1"
            ElseIf Container_code.ToUpper = "<ANY>" Then
                sSQL = "Select Part_nbr,Customer_Part_Nbr, Destination from Pltfloor.Ics_label_data_vw where Part_nbr = '" & DelphiPN & "' and rownum =1"
            Else
                sSQL = "Select Part_nbr,Customer_Part_Nbr, Destination from Plantfloor.Ics_label_data_vw where Part_nbr = '" & DelphiPN & "' and Destination = '" & Container_code & "' and rownum =1"
            End If
            With cmd
                .CommandType = CommandType.Text
                .CommandText = sSQL
                .Connection = cnOra
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Return ds.Tables(0).Rows(0).Item("Customer_Part_Nbr")
                Else
                    Return "Failure: Getting Customer Part Nbr -ICS Data Not Available"
                End If
            Else
                Return "Failure: Getting Customer Part Nbr"
            End If
        Catch ex As Exception
            Return "Failure: " & ex.Message
        Finally
            cmd.Dispose()
            da.Dispose()
            ds.Dispose()
        End Try

    End Function

    Public Function ConvertToCustomerPartNumber(ByVal PartNumber As String, ByRef cnOra As OracleConnection) As String
        Dim oCommand As New OracleCommand

        Try
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.Open()
            End If

            With oCommand
                .Connection = cnOra
                .CommandType = CommandType.StoredProcedure
                .CommandText = "mitproc.gsd_read_prod_pkg.get_xref_part_proc"
                With .Parameters
                    .Add("P_PART_NBR", OracleDbType.Varchar2, 8).Value = PartNumber
                    .Add("P_UNIQUE_FEATURE", OracleDbType.Varchar2, 30).Value = ""
                    .Add("p_xref_busorg_svr_nbr", OracleDbType.Varchar2, 10).Value = ""
                    .Add("p_xref_busorg_id", OracleDbType.Varchar2, 10).Value = ""
                    .Add("p_xref_busorg_numbng_cd", OracleDbType.Varchar2, 10).Value = ""
                    .Add("p_xref_part_nbr", OracleDbType.Varchar2, 8)
                    .Add("p_xref_unique_feature", OracleDbType.Varchar2, 30)
                    .Add("p_err_num", OracleDbType.Int16)
                    .Add("p_err_msg", OracleDbType.Varchar2, 100)
                    .Add("p_err_proc", OracleDbType.Varchar2, 100)

                    Dim I As Integer
                    For I = 0 To 4
                        .Item(I).Direction = ParameterDirection.InputOutput
                    Next

                    For I = 5 To 9
                        .Item(I).Direction = ParameterDirection.Output
                    Next
                End With

                .ExecuteNonQuery()

                Return CStr(.Parameters("P_XREF_PART_NBR").Value)
            End With
        Catch ex As Exception
            Return "Failure: " & ex.Message
        Finally
            'because we pass in an open connection, we don't close it
            oCommand.Dispose()
        End Try

    End Function

    <WebMethod()> _
    Public Function FindBroadcastCodeInCntrs(ByVal broadCastCode As String) As DataSet

        ' local variables
        Dim sSQL As String
        Dim cnOra As New OracleConnection
        Dim cmd As New OracleCommand
        Dim da As OracleDataAdapter
        Dim ds As New DataSet
        Dim x As Integer
        Dim StdPackID As String
        Dim dsRet As New DataSet
        Dim dt As New DataTable

        Try

            'define the SQL statement
            sSQL = "SELECT SP.SERIAL_NBR, SP.STANDARD_PACK_ID "
            sSQL = sSQL & "FROM MESDBA.STANDARD_PACK SP, MESDBA.SERIALIZED_PRODUCT ZP "
            sSQL = sSQL & "WHERE ZP.STANDARD_PACK_ID = SP.STANDARD_PACK_ID AND ZP.SERIAL_NBR = '" & broadCastCode & "' AND "
            sSQL = sSQL & "SYSDATE - ZP.INSERT_TMSTM < 1"

            'Open the connection
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                cnOra.Open()
            End If

            ' get the information
            With cmd
                .Connection = cnOra
                .CommandType = CommandType.Text
                .CommandText = sSQL
            End With

            'Execute the query
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then

                Dim da2 As OracleDataAdapter
                Dim ds2 As New DataSet
                Dim cmd2 As New OracleCommand
                Dim sSQL2 As String

                'Ignore BECs that have been cleared for repacking

                StdPackID = ds.Tables(0).Rows(0).Item("STANDARD_PACK_ID")

                'define the SQL statement
                sSQL2 = "SELECT RL.SERIAL_NBR, RL.STANDARD_PACK_ID_ORIG "
                sSQL2 = sSQL2 & "FROM MESDBA.SERIALIZED_PRODUCT_RELABEL RL "
                sSQL2 = sSQL2 & "WHERE RL.STANDARD_PACK_ID_ORIG = " & StdPackID & " AND RL.SERIAL_NBR = '" & broadCastCode & "'"

                ' get the information
                With cmd2
                    .Connection = cnOra
                    .CommandType = CommandType.Text
                    .CommandText = sSQL
                End With

                cmd2.CommandText = sSQL2
                da2 = New OracleDataAdapter(cmd2)
                da2.Fill(ds2)

                If ds2.Tables(0).Rows.Count > 0 Then
                    'This serial is cleared for repacking - no alarm

                    dt.Columns.Add("SERIAL_NBR", GetType(System.String))
                    dt.Columns.Add("STANDARD_PACK_ID", GetType(System.Int64))
                    dsRet.Tables.Add(dt)
                    Return dsRet 'return empty dataset - no alarm
                Else
                    Return ds 'BEC has not been cleared for repacking
                End If

                ds2.Dispose()
                cmd2.Dispose()
                da2.Dispose()

            Else
                Return ds 'no past records of BEC found
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            ds.Dispose()
            da.Dispose()
            'ds2.Dispose()
            'da2.Dispose()
            dsRet.Dispose()
            dt.Dispose()
            cmd.Dispose()
            'cmd2.Dispose()
            cnOra.Close()
            cnOra.Dispose()
        End Try

    End Function

    <WebMethod()> _
    Public Function ProcessBroadcastCode(ByVal broadCastCode As String, ByVal Dept As String, ByRef Result As String, ByVal ContainerType As String, ByVal LabelType As String) As DataSet

        Dim dt As New DataTable
        Dim retv As String
        Dim DelphiPartNumber, ECL As String
        Dim MESPartID As Int64
        Dim PackageCode As String
        Dim StdPack As Int32
        Dim NumberOfBoxes As Integer
        Dim NumberOfLayersPerBox As Integer
        Dim ScheduleRequired As Boolean
        Dim CustomerPartNumber As String
        Dim ds As New DataSet
        Dim dsret As New DataSet
        Dim cnOra As New OracleConnection

        'Retrieve the part number and mespart id
        'Todo:  For the 900 release, we will condense these three calls into one.  Also, the broadcast code may not be necessary
        'at that time because it will be in the barcode
        Try
            'create a data row from the data table
            cnOra.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
            cnOra.Open()

            Dim dr As DataRow = dt.NewRow()
            ds = GetPartNumber(broadCastCode, cnOra)
            If ds.Tables(0).Rows.Count > 0 Then
                DelphiPartNumber = Convert.ToString(ds.Tables(0).Rows(0).Item("Part_Nbr"))
                CustomerPartNumber = GetCustomerPNfromICStable(DelphiPartNumber, "<ANY>", cnOra)
                If InStr(CustomerPartNumber, "Failure:") <> 0 Then
                    'todo MsgBox("Failed to Get Customer Part Number. Details: " & retv)
                    Throw New Exception("Failed to Cross Reference the Delphi Part Number to a Customer Part Number for Delphi Part Number: " & DelphiPartNumber)
                End If

                MESPartID = Convert.ToInt64(ds.Tables(0).Rows(0).Item("mes_part_id"))
                If Len(broadCastCode.Trim) = 5 Then
                    ECL = broadCastCode.Substring(3, 2)
                Else
                    ECL = Convert.ToString(ds.Tables(0).Rows(0).Item("Rev_Physical"))
                End If
            Else
                Throw New Exception("Broadcast Code not found.  Verify that a production run has been loaded for this Broadcast-Code:" & broadCastCode)
            End If
            ds.Clear()
            LogMsg("broadCastCode: Dept:", broadCastCode, Dept)
            LogMsg("ContainerType: LabelType:", ContainerType, LabelType)
            LogMsg("ECL:", ECL)

            ds = GetStandardPackDetails(broadCastCode, ContainerType, LabelType, ECL, Dept, cnOra)
            If ds.Tables(0).Rows.Count > 0 Then
                PackageCode = Convert.ToString(ds.Tables(0).Rows(0).Item("PACKAGE_CODE"))

                StdPack = Convert.ToInt32(ds.Tables(0).Rows(0).Item("STANDARD_PACK"))
                NumberOfBoxes = Convert.ToInt32(ds.Tables(0).Rows(0).Item("BOXCOUNT"))
                If (Dept = "2341" Or Dept = "02341") And Convert.ToString(ds.Tables(0).Rows(0).Item("PACKAGE_CODE")) = "Y" Then
                    NumberOfLayersPerBox = Convert.ToInt32(ds.Tables(0).Rows(0).Item("LAYER_COUNT")) * 2
                Else
                    NumberOfLayersPerBox = Convert.ToInt32(ds.Tables(0).Rows(0).Item("LAYER_COUNT"))
                End If
                If Convert.ToString(ds.Tables(0).Rows(0).Item("CONTAINER_CODE")) = "CDICS" Then
                    ScheduleRequired = True
                Else
                    ScheduleRequired = False
                End If

            Else
                Throw New Exception("No Information in Production Run table for this Broadcast Code, ECL, Package_Code and Department")
            End If

            dt.Columns.Add("DelphiPartNumber", GetType(System.String))
            dt.Columns.Add("ECL", GetType(System.String))
            dt.Columns.Add("MESPartID", GetType(System.Int64))
            dt.Columns.Add("PackageCode", GetType(System.String))
            dt.Columns.Add("Department", GetType(System.String))
            dt.Columns.Add("StdPack", GetType(System.Int64))
            dt.Columns.Add("NumberOfBoxes", GetType(System.Int64))
            dt.Columns.Add("NumberOfLayersPerBox", GetType(System.Int64))
            dt.Columns.Add("ScheduleRequired", GetType(System.Boolean))
            dt.Columns.Add("CustomerPartNumber", GetType(System.String))
            dt.Columns.Add("ProdRunID", GetType(System.Int64))

            ' populate the data row
            dr.Item("DelphiPartNumber") = DelphiPartNumber
            dr.Item("ECL") = ECL
            dr.Item("MESPartID") = MESPartID
            dr.Item("PackageCode") = PackageCode
            dr.Item("Department") = Dept
            dr.Item("StdPack") = StdPack
            dr.Item("NumberOfBoxes") = NumberOfBoxes
            dr.Item("NumberOfLayersPerBox") = NumberOfLayersPerBox
            dr.Item("ScheduleRequired") = ScheduleRequired
            dr.Item("CustomerPartNumber") = CustomerPartNumber
            dr.Item("ProdRunID") = Convert.ToInt64(ds.Tables(0).Rows(0).Item("PRODUCTION_RUN_ID"))
            dt.Rows.Add(dr)
            dsret.Tables.Add(dt)

            Result = "Success:"
            Return dsret

        Catch ex As Exception
            Result = "Failure in ProcessBroadCastCode =>" & ex.Message
        Finally
            If Not cnOra Is Nothing Then
                cnOra.Close()
                cnOra.Dispose()
                GC.Collect()
            End If
            ds.Dispose()
            dsret.Dispose()
        End Try
    End Function

    <WebMethod()> _
    Public Function FlagAllBECsInCntrOK2Relabel(ByVal StdPackID As String) As Boolean

        ' local variables
        Dim sSQL1 As String
        Dim sSQL2 As String
        Dim cnOra As New OracleConnection
        Dim cmd As New OracleCommand
        Dim da As OracleDataAdapter
        Dim ds As New DataSet
        Dim x As Integer

        Try

            'define the SQL statement
            sSQL1 = "SELECT * "
            sSQL1 = sSQL1 & "FROM MESDBA.SERIALIZED_PRODUCT ZP "
            sSQL1 = sSQL1 & "WHERE ZP.STANDARD_PACK_ID = " & StdPackID

            'Open the connection
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                cnOra.Open()
            End If

            ' get the information
            With cmd
                .Connection = cnOra
                .CommandType = CommandType.Text
                .CommandText = sSQL1
            End With

            'Execute the query
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                For x = 0 To ds.Tables(0).Rows.Count - 1

                    'define the SQL statement
                    sSQL2 = ""
                    sSQL2 = "INSERT INTO "
                    sSQL2 = sSQL2 & "MESDBA.SERIALIZED_PRODUCT_RELABEL"
                    sSQL2 = sSQL2 & "(SERIAL_NBR, STANDARD_PACK_ID_ORIG, INSERT_USERID) VALUES('"
                    sSQL2 = sSQL2 & ds.Tables(0).Rows(x).Item("SERIAL_NBR") & "', "
                    sSQL2 = sSQL2 & ds.Tables(0).Rows(x).Item("STANDARD_PACK_ID") & ", "
                    sSQL2 = sSQL2 & "'P23BECKiosk')"

                    cmd.CommandText = sSQL2
                    cmd.ExecuteNonQuery()
                Next
            End If

            'Return the boolean true if successful
            Return True

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            ds.Dispose()
            da.Dispose()
            cmd.Dispose()
            cnOra.Close()
            cnOra.Dispose()
        End Try

    End Function

    <WebMethod()> _
    Public Function FlagBECOK2Relabel(ByVal broadCastCode As String) As Boolean

        ' local variables
        Dim sSQL1 As String
        Dim sSQL2 As String
        Dim cnOra As New OracleConnection
        Dim cmd As New OracleCommand
        Dim da As OracleDataAdapter
        Dim ds As New DataSet
        Dim x As Integer

        Try

            'define the SQL statement
            sSQL1 = "SELECT * "
            sSQL1 = sSQL1 & "FROM MESDBA.SERIALIZED_PRODUCT ZP "
            sSQL1 = sSQL1 & "WHERE ZP.SERIAL_NBR = '" & broadCastCode & "'"

            'Open the connection
            If cnOra.State = System.Data.ConnectionState.Closed Then
                cnOra.ConnectionString = ConfigurationSettings.AppSettings.Get(Common.GetServerType & "OracleConnection")
                cnOra.Open()
            End If

            ' get the information
            With cmd
                .Connection = cnOra
                .CommandType = CommandType.Text
                .CommandText = sSQL1
            End With

            'Execute the query
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                For x = 0 To ds.Tables(0).Rows.Count - 1

                    'define the SQL statement
                    sSQL2 = ""
                    sSQL2 = "INSERT INTO "
                    sSQL2 = sSQL2 & "MESDBA.SERIALIZED_PRODUCT_RELABEL"
                    sSQL2 = sSQL2 & "(SERIAL_NBR, STANDARD_PACK_ID_ORIG, INSERT_USERID) VALUES('"
                    sSQL2 = sSQL2 & ds.Tables(0).Rows(x).Item("SERIAL_NBR") & "', "
                    sSQL2 = sSQL2 & ds.Tables(0).Rows(x).Item("STANDARD_PACK_ID") & ", "
                    sSQL2 = sSQL2 & "'P23BECKiosk')"

                    cmd.CommandText = sSQL2
                    cmd.ExecuteNonQuery()
                Next
            End If

            'Return the boolean true if successful
            Return True

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            ds.Dispose()
            da.Dispose()
            cmd.Dispose()
            cnOra.Close()
            cnOra.Dispose()
        End Try

    End Function

End Class
