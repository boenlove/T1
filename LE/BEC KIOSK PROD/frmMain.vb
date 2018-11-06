Option Explicit On
Imports System.IO
Imports System.Configuration.ConfigurationSettings
Imports System.Drawing.Printing
Imports System.Web.Services
Imports System.Text
Imports Oracle.ManagedDataAccess.Client
Imports System.IO.Ports
Imports System.Configuration

Public Class frmMain
    Inherits System.Windows.Forms.Form
    Public Plant As String
    Public LastSerialNbr As String
    Private conf As New ConfigurationSettings
    Private BECSeries As String
    Private minSetCount As Integer
    Private maxSetCount As Integer

    Dim gbScannerEnabled As Boolean
    Dim DelayBetweenScans As Integer
    Dim LastScan As Date
    Dim setCount As Boolean
    Private WithEvents Pdoc As New PrintDocument
    Dim WithEvents frmExceptions As New ExceptionsHandler
    Dim Stx As String
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents test_SN As System.Windows.Forms.TextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    'used to buffer comm port input

    Private newSerialNumbers() As String

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        Try
            InitializeComponent()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblScannerStatus As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents optCardboard As System.Windows.Forms.RadioButton
    Friend WithEvents optReturnable As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblAction As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents optICS As System.Windows.Forms.RadioButton
    Friend WithEvents optMTMS As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmdCorrectProblem As System.Windows.Forms.Button
    Friend WithEvents cmdReset As System.Windows.Forms.Button
    Friend WithEvents cmdResetCount As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents label6 As System.Windows.Forms.Label
    Friend WithEvents lblLastScan As System.Windows.Forms.Label
    Friend WithEvents lblECL As System.Windows.Forms.Label
    Friend WithEvents tmrRefresh As System.Windows.Forms.Timer
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtAdminCode As System.Windows.Forms.TextBox
    Friend WithEvents txtCurrLayer As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalLayers As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtNumPackages As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblCustPartNbr As System.Windows.Forms.Label
    Friend WithEvents lblDelphiPart As System.Windows.Forms.TextBox
    Friend WithEvents cmdExceptionsHandler As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblLastSerial As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lblMachineID As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtPkgsPerSkid As System.Windows.Forms.TextBox
    Friend WithEvents txtDestinationCode As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtPackageCode As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtSkidPackageCount As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents chkScanOut As System.Windows.Forms.CheckBox
    Friend WithEvents lblDept As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lblRunsAt As System.Windows.Forms.Label
    Friend WithEvents PageSetupDialog1 As System.Windows.Forms.PageSetupDialog
    Friend WithEvents PageSetupDialog2 As System.Windows.Forms.PageSetupDialog
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtShipTo As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lblScannerStatus = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.optCardboard = New System.Windows.Forms.RadioButton()
        Me.optReturnable = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblAction = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.optICS = New System.Windows.Forms.RadioButton()
        Me.optMTMS = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdCorrectProblem = New System.Windows.Forms.Button()
        Me.cmdReset = New System.Windows.Forms.Button()
        Me.cmdResetCount = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.label6 = New System.Windows.Forms.Label()
        Me.lblLastScan = New System.Windows.Forms.Label()
        Me.lblCustPartNbr = New System.Windows.Forms.Label()
        Me.lblECL = New System.Windows.Forms.Label()
        Me.tmrRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.txtAdminCode = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtCurrLayer = New System.Windows.Forms.TextBox()
        Me.txtTotalLayers = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtNumPackages = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblDelphiPart = New System.Windows.Forms.TextBox()
        Me.cmdExceptionsHandler = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblLastSerial = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblMachineID = New System.Windows.Forms.Label()
        Me.txtPkgsPerSkid = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtDestinationCode = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtPackageCode = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtSkidPackageCount = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.chkScanOut = New System.Windows.Forms.CheckBox()
        Me.lblDept = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblRunsAt = New System.Windows.Forms.Label()
        Me.PageSetupDialog1 = New System.Windows.Forms.PageSetupDialog()
        Me.PageSetupDialog2 = New System.Windows.Forms.PageSetupDialog()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtShipTo = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.test_SN = New System.Windows.Forms.TextBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblScannerStatus
        '
        Me.lblScannerStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScannerStatus.ForeColor = System.Drawing.Color.Red
        Me.lblScannerStatus.Location = New System.Drawing.Point(246, 192)
        Me.lblScannerStatus.Name = "lblScannerStatus"
        Me.lblScannerStatus.Size = New System.Drawing.Size(126, 66)
        Me.lblScannerStatus.TabIndex = 1
        Me.lblScannerStatus.Text = "Scanner Disabled"
        Me.lblScannerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.optCardboard)
        Me.Panel1.Controls.Add(Me.optReturnable)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(12, 171)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 135)
        Me.Panel1.TabIndex = 3
        '
        'optCardboard
        '
        Me.optCardboard.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCardboard.Location = New System.Drawing.Point(40, 90)
        Me.optCardboard.Name = "optCardboard"
        Me.optCardboard.Size = New System.Drawing.Size(120, 24)
        Me.optCardboard.TabIndex = 1
        Me.optCardboard.Text = "Cardboard"
        '
        'optReturnable
        '
        Me.optReturnable.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optReturnable.Location = New System.Drawing.Point(40, 40)
        Me.optReturnable.Name = "optReturnable"
        Me.optReturnable.Size = New System.Drawing.Size(120, 24)
        Me.optReturnable.TabIndex = 0
        Me.optReturnable.Text = "Returnable"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(130, 23)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Container Type:"
        '
        'lblAction
        '
        Me.lblAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAction.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblAction.Location = New System.Drawing.Point(9, 6)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.Size = New System.Drawing.Size(591, 33)
        Me.lblAction.TabIndex = 5
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.optICS)
        Me.Panel2.Controls.Add(Me.optMTMS)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Location = New System.Drawing.Point(399, 174)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(200, 135)
        Me.Panel2.TabIndex = 7
        '
        'optICS
        '
        Me.optICS.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optICS.Location = New System.Drawing.Point(40, 90)
        Me.optICS.Name = "optICS"
        Me.optICS.Size = New System.Drawing.Size(104, 24)
        Me.optICS.TabIndex = 1
        Me.optICS.Text = "SAP"
        '
        'optMTMS
        '
        Me.optMTMS.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optMTMS.Location = New System.Drawing.Point(40, 40)
        Me.optMTMS.Name = "optMTMS"
        Me.optMTMS.Size = New System.Drawing.Size(104, 24)
        Me.optMTMS.TabIndex = 0
        Me.optMTMS.Text = "MTMS"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Label Type:"
        '
        'cmdCorrectProblem
        '
        Me.cmdCorrectProblem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCorrectProblem.Location = New System.Drawing.Point(441, 408)
        Me.cmdCorrectProblem.Name = "cmdCorrectProblem"
        Me.cmdCorrectProblem.Size = New System.Drawing.Size(171, 40)
        Me.cmdCorrectProblem.TabIndex = 9
        Me.cmdCorrectProblem.Text = "Correct Problem"
        Me.cmdCorrectProblem.Visible = False
        '
        'cmdReset
        '
        Me.cmdReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReset.Location = New System.Drawing.Point(441, 498)
        Me.cmdReset.Name = "cmdReset"
        Me.cmdReset.Size = New System.Drawing.Size(171, 45)
        Me.cmdReset.TabIndex = 11
        Me.cmdReset.Text = "Clear Screen and Start Over"
        '
        'cmdResetCount
        '
        Me.cmdResetCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdResetCount.Location = New System.Drawing.Point(441, 363)
        Me.cmdResetCount.Name = "cmdResetCount"
        Me.cmdResetCount.Size = New System.Drawing.Size(171, 30)
        Me.cmdResetCount.TabIndex = 13
        Me.cmdResetCount.TabStop = False
        Me.cmdResetCount.Text = "Reset Count"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(27, 333)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(124, 25)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Last Scan:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(-6, 393)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(161, 25)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Cust Part Nbr:"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label6.Location = New System.Drawing.Point(87, 438)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(63, 25)
        Me.label6.TabIndex = 19
        Me.label6.Text = "ECL:"
        '
        'lblLastScan
        '
        Me.lblLastScan.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastScan.ForeColor = System.Drawing.Color.Navy
        Me.lblLastScan.Location = New System.Drawing.Point(147, 330)
        Me.lblLastScan.Name = "lblLastScan"
        Me.lblLastScan.Size = New System.Drawing.Size(285, 35)
        Me.lblLastScan.TabIndex = 21
        '
        'lblCustPartNbr
        '
        Me.lblCustPartNbr.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustPartNbr.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblCustPartNbr.Location = New System.Drawing.Point(147, 390)
        Me.lblCustPartNbr.Name = "lblCustPartNbr"
        Me.lblCustPartNbr.Size = New System.Drawing.Size(230, 35)
        Me.lblCustPartNbr.TabIndex = 23
        '
        'lblECL
        '
        Me.lblECL.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblECL.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblECL.Location = New System.Drawing.Point(147, 435)
        Me.lblECL.Name = "lblECL"
        Me.lblECL.Size = New System.Drawing.Size(230, 35)
        Me.lblECL.TabIndex = 25
        '
        'tmrRefresh
        '
        Me.tmrRefresh.Interval = 50
        '
        'txtAdminCode
        '
        Me.txtAdminCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdminCode.Location = New System.Drawing.Point(513, 468)
        Me.txtAdminCode.Name = "txtAdminCode"
        Me.txtAdminCode.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtAdminCode.Size = New System.Drawing.Size(93, 22)
        Me.txtAdminCode.TabIndex = 12
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(426, 471)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(81, 18)
        Me.Label5.TabIndex = 28
        Me.Label5.Text = "AdminCode:"
        '
        'txtCurrLayer
        '
        Me.txtCurrLayer.BackColor = System.Drawing.SystemColors.Control
        Me.txtCurrLayer.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCurrLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrLayer.ForeColor = System.Drawing.Color.Blue
        Me.txtCurrLayer.Location = New System.Drawing.Point(204, 42)
        Me.txtCurrLayer.Name = "txtCurrLayer"
        Me.txtCurrLayer.ReadOnly = True
        Me.txtCurrLayer.Size = New System.Drawing.Size(87, 19)
        Me.txtCurrLayer.TabIndex = 30
        Me.txtCurrLayer.TabStop = False
        '
        'txtTotalLayers
        '
        Me.txtTotalLayers.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalLayers.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalLayers.ForeColor = System.Drawing.Color.Blue
        Me.txtTotalLayers.Location = New System.Drawing.Point(204, 66)
        Me.txtTotalLayers.Name = "txtTotalLayers"
        Me.txtTotalLayers.ReadOnly = True
        Me.txtTotalLayers.Size = New System.Drawing.Size(87, 19)
        Me.txtTotalLayers.TabIndex = 31
        Me.txtTotalLayers.TabStop = False
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(75, 42)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(126, 21)
        Me.Label7.TabIndex = 32
        Me.Label7.Text = "Current Layer:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(27, 66)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(174, 21)
        Me.Label8.TabIndex = 33
        Me.Label8.Text = "Layers per Package:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(15, 90)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(186, 21)
        Me.Label9.TabIndex = 35
        Me.Label9.Text = "Packages per Standard:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNumPackages
        '
        Me.txtNumPackages.BackColor = System.Drawing.SystemColors.Control
        Me.txtNumPackages.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtNumPackages.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumPackages.ForeColor = System.Drawing.Color.Blue
        Me.txtNumPackages.Location = New System.Drawing.Point(204, 90)
        Me.txtNumPackages.Name = "txtNumPackages"
        Me.txtNumPackages.ReadOnly = True
        Me.txtNumPackages.Size = New System.Drawing.Size(87, 19)
        Me.txtNumPackages.TabIndex = 34
        Me.txtNumPackages.TabStop = False
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(309, 66)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(123, 21)
        Me.Label10.TabIndex = 37
        Me.Label10.Text = "Delphi PN:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDelphiPart
        '
        Me.lblDelphiPart.BackColor = System.Drawing.SystemColors.Control
        Me.lblDelphiPart.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblDelphiPart.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDelphiPart.ForeColor = System.Drawing.Color.Blue
        Me.lblDelphiPart.Location = New System.Drawing.Point(438, 66)
        Me.lblDelphiPart.Name = "lblDelphiPart"
        Me.lblDelphiPart.ReadOnly = True
        Me.lblDelphiPart.Size = New System.Drawing.Size(153, 19)
        Me.lblDelphiPart.TabIndex = 36
        Me.lblDelphiPart.TabStop = False
        '
        'cmdExceptionsHandler
        '
        Me.cmdExceptionsHandler.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExceptionsHandler.Location = New System.Drawing.Point(441, 324)
        Me.cmdExceptionsHandler.Name = "cmdExceptionsHandler"
        Me.cmdExceptionsHandler.Size = New System.Drawing.Size(171, 30)
        Me.cmdExceptionsHandler.TabIndex = 42
        Me.cmdExceptionsHandler.Text = "Exceptions Handler"
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(612, 6)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(57, 27)
        Me.btnClose.TabIndex = 43
        Me.btnClose.Text = "&Close"
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(309, 42)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(123, 21)
        Me.Label11.TabIndex = 39
        Me.Label11.Text = "Last Serial#:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLastSerial
        '
        Me.lblLastSerial.BackColor = System.Drawing.SystemColors.Control
        Me.lblLastSerial.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblLastSerial.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastSerial.ForeColor = System.Drawing.Color.Blue
        Me.lblLastSerial.Location = New System.Drawing.Point(438, 42)
        Me.lblLastSerial.Name = "lblLastSerial"
        Me.lblLastSerial.ReadOnly = True
        Me.lblLastSerial.Size = New System.Drawing.Size(306, 19)
        Me.lblLastSerial.TabIndex = 38
        Me.lblLastSerial.TabStop = False
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(435, 552)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(171, 18)
        Me.Label12.TabIndex = 44
        Me.Label12.Text = "Last Build:  02/24/2006 8:36 AM"
        '
        'lblMachineID
        '
        Me.lblMachineID.BackColor = System.Drawing.SystemColors.Control
        Me.lblMachineID.Location = New System.Drawing.Point(426, 573)
        Me.lblMachineID.Name = "lblMachineID"
        Me.lblMachineID.Size = New System.Drawing.Size(174, 18)
        Me.lblMachineID.TabIndex = 45
        '
        'txtPkgsPerSkid
        '
        Me.txtPkgsPerSkid.BackColor = System.Drawing.SystemColors.Control
        Me.txtPkgsPerSkid.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPkgsPerSkid.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPkgsPerSkid.ForeColor = System.Drawing.Color.Blue
        Me.txtPkgsPerSkid.Location = New System.Drawing.Point(204, 114)
        Me.txtPkgsPerSkid.Name = "txtPkgsPerSkid"
        Me.txtPkgsPerSkid.ReadOnly = True
        Me.txtPkgsPerSkid.Size = New System.Drawing.Size(87, 19)
        Me.txtPkgsPerSkid.TabIndex = 46
        Me.txtPkgsPerSkid.TabStop = False
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(15, 114)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(186, 21)
        Me.Label13.TabIndex = 47
        Me.Label13.Text = "Packages per Skid:"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDestinationCode
        '
        Me.txtDestinationCode.BackColor = System.Drawing.SystemColors.Control
        Me.txtDestinationCode.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtDestinationCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDestinationCode.ForeColor = System.Drawing.Color.Blue
        Me.txtDestinationCode.Location = New System.Drawing.Point(438, 90)
        Me.txtDestinationCode.Name = "txtDestinationCode"
        Me.txtDestinationCode.ReadOnly = True
        Me.txtDestinationCode.Size = New System.Drawing.Size(153, 19)
        Me.txtDestinationCode.TabIndex = 48
        Me.txtDestinationCode.TabStop = False
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(309, 90)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(123, 21)
        Me.Label14.TabIndex = 49
        Me.Label14.Text = "Dest Code:"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPackageCode
        '
        Me.txtPackageCode.BackColor = System.Drawing.SystemColors.Control
        Me.txtPackageCode.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPackageCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPackageCode.ForeColor = System.Drawing.Color.Blue
        Me.txtPackageCode.Location = New System.Drawing.Point(438, 114)
        Me.txtPackageCode.Name = "txtPackageCode"
        Me.txtPackageCode.ReadOnly = True
        Me.txtPackageCode.Size = New System.Drawing.Size(153, 19)
        Me.txtPackageCode.TabIndex = 50
        Me.txtPackageCode.TabStop = False
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(309, 114)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(123, 21)
        Me.Label15.TabIndex = 51
        Me.Label15.Text = "Package Code:"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSkidPackageCount
        '
        Me.txtSkidPackageCount.BackColor = System.Drawing.SystemColors.Control
        Me.txtSkidPackageCount.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSkidPackageCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSkidPackageCount.ForeColor = System.Drawing.Color.Blue
        Me.txtSkidPackageCount.Location = New System.Drawing.Point(204, 138)
        Me.txtSkidPackageCount.Name = "txtSkidPackageCount"
        Me.txtSkidPackageCount.ReadOnly = True
        Me.txtSkidPackageCount.Size = New System.Drawing.Size(87, 19)
        Me.txtSkidPackageCount.TabIndex = 52
        Me.txtSkidPackageCount.TabStop = False
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(15, 138)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(186, 21)
        Me.Label16.TabIndex = 53
        Me.Label16.Text = "Skid Package Count:"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkScanOut
        '
        Me.chkScanOut.Appearance = System.Windows.Forms.Appearance.Button
        Me.chkScanOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkScanOut.Location = New System.Drawing.Point(243, 273)
        Me.chkScanOut.Name = "chkScanOut"
        Me.chkScanOut.Size = New System.Drawing.Size(129, 24)
        Me.chkScanOut.TabIndex = 54
        Me.chkScanOut.Text = "Scan Remove BEC"
        Me.chkScanOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDept
        '
        Me.lblDept.BackColor = System.Drawing.SystemColors.Control
        Me.lblDept.Location = New System.Drawing.Point(435, 594)
        Me.lblDept.Name = "lblDept"
        Me.lblDept.Size = New System.Drawing.Size(174, 18)
        Me.lblDept.TabIndex = 55
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(594, 114)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 56
        Me.Button1.Text = "Button1"
        '
        'lblRunsAt
        '
        Me.lblRunsAt.BackColor = System.Drawing.SystemColors.Control
        Me.lblRunsAt.Location = New System.Drawing.Point(435, 618)
        Me.lblRunsAt.Name = "lblRunsAt"
        Me.lblRunsAt.Size = New System.Drawing.Size(219, 18)
        Me.lblRunsAt.TabIndex = 57
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(309, 138)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(123, 21)
        Me.Label17.TabIndex = 59
        Me.Label17.Text = "Ship To:"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtShipTo
        '
        Me.txtShipTo.BackColor = System.Drawing.SystemColors.Control
        Me.txtShipTo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtShipTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShipTo.ForeColor = System.Drawing.Color.Blue
        Me.txtShipTo.Location = New System.Drawing.Point(432, 138)
        Me.txtShipTo.Name = "txtShipTo"
        Me.txtShipTo.ReadOnly = True
        Me.txtShipTo.Size = New System.Drawing.Size(153, 19)
        Me.txtShipTo.TabIndex = 60
        Me.txtShipTo.TabStop = False
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(649, 186)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(111, 24)
        Me.Button2.TabIndex = 61
        Me.Button2.Text = "Full BR Test"
        '
        'test_SN
        '
        Me.test_SN.Location = New System.Drawing.Point(659, 216)
        Me.test_SN.Name = "test_SN"
        Me.test_SN.Size = New System.Drawing.Size(216, 20)
        Me.test_SN.TabIndex = 62
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(766, 186)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(111, 24)
        Me.Button3.TabIndex = 63
        Me.Button3.Text = "Single BR Test"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(946, 678)
        Me.ControlBox = False
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.test_SN)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.txtShipTo)
        Me.Controls.Add(Me.txtSkidPackageCount)
        Me.Controls.Add(Me.txtPackageCode)
        Me.Controls.Add(Me.txtDestinationCode)
        Me.Controls.Add(Me.txtPkgsPerSkid)
        Me.Controls.Add(Me.lblLastSerial)
        Me.Controls.Add(Me.lblDelphiPart)
        Me.Controls.Add(Me.txtNumPackages)
        Me.Controls.Add(Me.txtTotalLayers)
        Me.Controls.Add(Me.txtCurrLayer)
        Me.Controls.Add(Me.txtAdminCode)
        Me.Controls.Add(Me.label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.lblRunsAt)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblDept)
        Me.Controls.Add(Me.chkScanOut)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.lblMachineID)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.cmdExceptionsHandler)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblECL)
        Me.Controls.Add(Me.lblCustPartNbr)
        Me.Controls.Add(Me.lblLastScan)
        Me.Controls.Add(Me.cmdResetCount)
        Me.Controls.Add(Me.cmdReset)
        Me.Controls.Add(Me.cmdCorrectProblem)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.lblAction)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblScannerStatus)
        Me.Name = "frmMain"
        Me.Text = "frmMain"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub PlaySound()
        Try
            Const FILE_NAME = "C:\windows\media\tada.wav"
            Dim SoundInst As New SoundClass
            SoundInst.PlaySoundFile(FILE_NAME)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim curlayer As Integer
        Dim curbox As Integer
        goappstate = New ApplicationState
        Dim ub As Integer
        Dim retv
        Dim ar() As String


        'DSM: select Dev. or Production service 
        'Dim rl As New localhostRequest.RequestLabel  'DSM: DEVELOPMENT version
        Dim rl As New dorme2.RequestLabel   'DSM: PRODUCTION version

        Try

            Me.WindowState = FormWindowState.Maximized
            'hit web service to determine if the webservice is production or not

            Dim t As String
            t = rl.RunsAt
            t = Mid(t, 1, InStr(t, ".") - 1)
            Me.lblRunsAt.Text = "Current Mode: " & t
            If t = "Production" Then Me.Button1.Visible = False
            rl.Dispose()

            If UBound(Diagnostics.Process.GetProcessesByName(Diagnostics.Process.GetCurrentProcess.ProcessName)) > 0 Then
                MsgBox("Application is Already Running", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly Or MsgBoxStyle.ApplicationModal)
                Application.Exit()
                Exit Sub
            Else
                'Carry on
            End If

            'set comm port stuff
            'MSComm.Settings = AppSettings("ScannerSettings")
            'MSComm.CommPort = AppSettings("ScannerCommPort")
            'MSComm.RThreshold = AppSettings("ScannerRThreshold")

            'MSComm.PortOpen = True
            'Serial Port Init  Jerry
            '***********************************************
            SerialPort1.PortName = ConfigurationManager.AppSettings("COMPORT") '"COM1"
            SerialPort1.BaudRate = Convert.ToInt16(ConfigurationManager.AppSettings("BAUDRATE")) '9600
            SerialPort1.Parity = Convert.ToInt16(ConfigurationManager.AppSettings("PARITY")) 'IO.Ports.Parity.None
            SerialPort1.DataBits = Convert.ToInt16(ConfigurationManager.AppSettings("DATABITS")) '8
            SerialPort1.StopBits = Convert.ToInt16(ConfigurationManager.AppSettings("STOPBITS")) 'IO.Ports.StopBits.One
            SerialPort1.RtsEnable = ConfigurationManager.AppSettings("RTSENABLE") 'True

            AddHandler SerialPort1.DataReceived, AddressOf DataReceivedHandler
            File.AppendAllText("log.txt", "Trying SerialPort1 open.." + " " + Now + Environment.NewLine)
            Try
                'jerry SerialPort1.Open()
            Catch ex As Exception
                MessageBox.Show("Failure Open Serial Port")
            End Try
            File.AppendAllText("log.txt", "SerialPort1 open success" + " " + Now + Environment.NewLine)

            lblAction.Text = goappstate.NextAction

            Me.Text = "GM T1 BEC Labeling System"  'jerry by Lance request 0510
            LastScan = goappstate.LastScan
            DelayBetweenScans = goappstate.DelayBetweenScans

            Plant = conf.Appsettings("//Properties/Plant")
            BECSeries = goappstate.BecSeries
            If BECSeries = "900" Then
                cmdResetCount.Enabled = False
                chkScanOut.Enabled = True
                chkScanOut.Checked = False
            Else
                cmdResetCount.Enabled = True
                chkScanOut.Checked = False
                chkScanOut.Enabled = False
            End If

            lblLastSerial.Text = conf.Appsettings("//ApplicationState/LastSerialNbr")
            lblMachineID.Text = conf.Appsettings("//Properties/MachineName")
            lblDept.Text = conf.Appsettings("//Properties/Department")
            If Len(lblMachineID.Text.Trim) = 0 Then
                MsgBox("Machine Name Not Set.  Set Appconfig file then restart", MsgBoxStyle.ApplicationModal)
                Application.Exit()
                Exit Sub
            End If

            If Plant = "23" Then
                Me.optMTMS.Enabled = False
            End If

            If goappstate.InProgress = True Or goappstate.NumberPkgsPerSkid > 1 Then
                'load the scanned serials
                Dim fr As System.IO.StreamReader
                Dim fw As System.IO.StreamWriter

                Dim line As String
                fr = New System.IO.StreamReader(Application.StartupPath & AppSettings("ScannedSerialsPath"))

                Do
                    line = fr.ReadLine()
                    If (line Is Nothing) Or line = "" Then
                    Else
                        ar = Split(line, "^")
                        Try
                            ub = UBound(goappstate.Scans)
                        Catch ex As Exception
                            ub = 0
                        End Try
                        ReDim Preserve goappstate.Scans(ub + 1)
                        ReDim Preserve goappstate.ScanTime(ub + 1)
                        goappstate.Scans(ub) = ar(0)
                        goappstate.ScanTime(ub) = ar(1)
                    End If
                Loop Until (line Is Nothing) Or line = ""
                fr.Close()
                'go ahead and erase the file
                fw = New System.IO.StreamWriter(Application.StartupPath & AppSettings("ScannedSerialsPath"))
                fw.Close()
                'end of load scanned serial

                'load scanremoved becs
                fr = New System.IO.StreamReader(Application.StartupPath & AppSettings("BufferedLabelsPath"))
                Do
                    line = fr.ReadLine()
                    If (line Is Nothing) Or line = "" Then
                    Else
                        Try
                            ub = UBound(goappstate.BufferedLabels)
                        Catch ex As Exception
                            ub = 0
                        End Try
                        ReDim Preserve goappstate.BufferedLabels(ub + 1)
                        goappstate.BufferedLabels(ub) = line
                    End If
                Loop Until (line Is Nothing) Or line = ""
                fr.Close()
                'go ahead and erase the file
                fw = New System.IO.StreamWriter(Application.StartupPath & AppSettings("BufferedLabelsPath"))
                fw.Close()


                'load the bufferedlabels
                fr = New System.IO.StreamReader(Application.StartupPath & AppSettings("ScanRemovedBECSPath"))
                Do
                    line = fr.ReadLine()
                    If (line Is Nothing) Or line = "" Then
                    Else
                        Try
                            ub = UBound(goappstate.RemovedBECs)
                        Catch
                            ub = 0
                        End Try
                        ReDim Preserve goappstate.RemovedBECs(ub + 1)
                        goappstate.RemovedBECs(ub) = line
                    End If
                Loop Until (line Is Nothing) Or line = ""
                fr.Close()
                'go ahead and erase the file
                fw = New System.IO.StreamWriter(Application.StartupPath & AppSettings("ScanRemovedBECSPath"))
                fw.Close()


                'jerry EnableScanner()
                If goappstate.ContainerType = "Returnable" Then
                    optReturnable.Checked = True
                    optCardboard.Enabled = False
                End If
                If goappstate.ContainerType = "Cardboard" Then
                    optCardboard.Checked = True
                    optReturnable.Enabled = False
                End If

                If goappstate.LabelType = "MTMS" Then
                    optMTMS.Checked = True
                    optICS.Enabled = False
                End If
                If goappstate.LabelType = "ICS" Then
                    optICS.Checked = True
                    optMTMS.Enabled = False
                End If
                refreshFormControls()
                If LayerProps("CurrentLayer", goappstate.TotalPartsScanned, "") Then
                End If
                Me.lblAction.Text = goappstate.NextAction
                Me.lblECL.Text = goappstate.ECL
                Me.lblDelphiPart.Text = goappstate.DelphiPartNumber
                Me.lblCustPartNbr.Text = goappstate.CustomerPartNumber
                Me.txtNumPackages.Text = goappstate.NumberOfBoxes
                Me.txtTotalLayers.Text = goappstate.NumberOfLayersPerBox
                Me.txtCurrLayer.Text = LayerProps("CurrentLayer", goappstate.TotalPartsScanned, "")
                If Me.txtCurrLayer.Text = "0" Then Me.txtCurrLayer.Text = ""

                If goappstate.TotalPartsScanned = (goappstate.StdPack / 2) Then
                    Me.lblAction.ForeColor = System.Drawing.Color.Red
                    PlayDaSound("CarHorn")
                ElseIf goappstate.TotalPartsScanned > (goappstate.StdPack / 2) Then
                    Me.lblAction.ForeColor = System.Drawing.Color.Red
                Else
                    Me.lblAction.ForeColor = System.Drawing.Color.ForestGreen
                End If
                If Plant = "23" Then
                    If goappstate.PackageCode = "Y" Then
                        Label7.Text = "Current Tray:"
                        Label8.Text = "Trays per Package:"
                    Else
                        Label7.Text = "Current Layer:"
                        Label8.Text = "Layers per Package:"
                    End If
                End If

                'This code gets the problem codes from Oracle for quality containment
                If conf.Appsettings("//ApplicationState/LogContainment") = "True" Then
                    'get problem codes for quality containment combo box
                    'todo put code from the webservice here
                    Dim ds As New DataSet
                    Dim result As String = ""
                    Dim i As Int16

                    ds = rl.GetQualityProblemCodes(result)
                    If InStr(result, "Failure") = 0 Then
                        For i = 0 To ds.Tables(0).Rows.Count - 1
                            ReDim Preserve Quality_ProbCodes(i)
                            Quality_ProbCodes(i) = ds.Tables(0).Rows(i).Item("Problem_code")
                        Next
                    Else
                        MsgBox("Failed to Get Problem Codes for Quality Inspection." & result & vbCrLf & "If you can't resolve, turn of containment logging in the application config and restart")
                        Me.Close()
                    End If
                End If

                Select Case goappstate.NextActionType
                    Case ApplicationState.ActionType.PROCESS_CORRECT_LAYER_COUNT, ApplicationState.ActionType.PROCESS_CORRECT_PACKAGE_COUNT, ApplicationState.ActionType.PROCESS_CORRECT_BOX_COUNT
                        DisableScanner()
                        cmdCorrectProblem.Visible = True
                        cmdCorrectProblem.Text = goappstate.NextAction
                        Me.lblAction.Text = goappstate.NextAction & " and press button"
                        Me.lblScannerStatus.Text = "Scanner Disabled"
                        Me.lblScannerStatus.ForeColor = System.Drawing.Color.Red
                    Case ApplicationState.ActionType.PROCESS_IS_LAYER_COMPLETE
                        If BECSeries = "900" Then
                            MsgBox("Layer is complete.", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                            goappstate.NextActionType = ApplicationState.ActionType.PROCESS_SCANPARTS_COMMAND
                            goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                            Me.lblAction.Text = goappstate.NextAction
                        Else
                            If IsComplete("Layer") Then
                                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_SCANPARTS_COMMAND
                                goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                                Me.lblAction.Text = goappstate.NextAction
                            Else
                                DisableScanner()
                                cmdCorrectProblem.Visible = True
                                goappstate.NextAction = "Correct Layer Count"
                                cmdCorrectProblem.Text = goappstate.NextAction
                                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_CORRECT_LAYER_COUNT
                                Me.lblAction.Text = goappstate.NextAction & " and press button"
                                Me.lblScannerStatus.Text = "Scanner Disabled"
                                Me.lblScannerStatus.ForeColor = System.Drawing.Color.Red
                            End If
                        End If
                    Case ApplicationState.ActionType.PROCESS_IS_BOX_COMPLETE
                        If BECSeries = "900" Then
                            MsgBox("Box is complete.", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                            ProcessBoxComplete()
                        Else
                            If IsComplete("Box") Then
                                ProcessBoxComplete()
                            Else
                                DisableScanner()
                                cmdCorrectProblem.Visible = True
                                retv = LayerProps("CurrentLayer", goappstate.TotalPartsScanned, "Correction")
                                goappstate.NextAction = "Correct Box Count"
                                cmdCorrectProblem.Text = goappstate.NextAction
                                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_IS_BOX_COMPLETE
                                Me.lblAction.Text = goappstate.NextAction & " and press button"
                                Me.lblScannerStatus.Text = "Scanner Disabled"
                                Me.lblScannerStatus.ForeColor = System.Drawing.Color.Red
                            End If
                        End If
                    Case ApplicationState.ActionType.PROCESS_IS_PACKAGE_COMPLETE
                        Dim b As Boolean = False
                        If goappstate.IsMultipleBoxes Then
                            b = ProcessPartScanMultipleBox()
                        Else
                            b = ProcessPartScanSingleBox()
                        End If
                        If b = True Then
                            If goappstate.NumberPkgsPerSkid > 1 Then
                                If (goappstate.SkidPackageCount = goappstate.NumberPkgsPerSkid) Then
                                    Reset(3) 'reset the skid also
                                Else
                                    Reset(1) 'reset but not the skid
                                End If
                            Else
                                Reset(0) 'no skid, just reset
                            End If
                        Else
                            DisableScanner()
                            cmdCorrectProblem.Visible = True
                            goappstate.NextAction = "Correct Package Count"
                            cmdCorrectProblem.Text = goappstate.NextAction
                            goappstate.NextActionType = ApplicationState.ActionType.PROCESS_IS_PACKAGE_COMPLETE
                            Me.lblAction.Text = goappstate.NextAction & " and press button"
                            Me.lblScannerStatus.Text = "Scanner Disabled"
                            Me.lblScannerStatus.ForeColor = System.Drawing.Color.Red
                        End If
                        'Jerry ***********************
                        'Update pData into DB
                        optCardboard.Enabled = True  'jerry 0510 to enable option after print label
                        optCardboard.Checked = False
                        optReturnable.Enabled = True
                        optReturnable.Checked = False

                    Case Else

                End Select
            Else
                If Plant = 23 Then
                    Me.optICS.Checked = True
                End If
            End If

        Catch ex As Exception
            MsgBox("Error in frmMain_Load: " & ex.Source & vbCrLf & ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub
    Private Sub optReturnable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optReturnable.CheckedChanged
        Try
            If goappstate.InProgress = False Then
                If optReturnable.Checked = True Then
                    goappstate.ContainerType = "Returnable"
                    If goappstate.LabelType = "" Then
                        goappstate.NextAction = "Choose ICS or MTMS"
                        Me.lblAction.Text = goappstate.NextAction
                    Else
                        If goappstate.InProgress = False Then

                            EnableScanner()

                            goappstate.NextAction = "Scan First Part"
                            lblAction.ForeColor = System.Drawing.Color.Green
                            goappstate.NextActionType = ApplicationState.ActionType.PROCESS_BROADCASTCODE
                            Me.lblAction.Text = goappstate.NextAction
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.ApplicationModal, "Option Returnable")
        End Try
    End Sub
    Private Sub optCardboard_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optCardboard.CheckedChanged
        Try
            If goappstate.InProgress = False Then
                If optCardboard.Checked = True Then
                    goappstate.ContainerType = "Cardboard"
                    If goappstate.LabelType = "" Then
                        goappstate.NextAction = "Choose ICS or MTMS"
                        Me.lblAction.Text = goappstate.NextAction
                    Else
                        If goappstate.InProgress = False Then
                            EnableScanner()
                            goappstate.NextAction = "Scan First Part"
                            lblAction.ForeColor = System.Drawing.Color.Green
                            goappstate.NextActionType = ApplicationState.ActionType.PROCESS_BROADCASTCODE
                            Me.lblAction.Text = goappstate.NextAction
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.ApplicationModal, "Option Cardboard")
        End Try
    End Sub
    Private Sub optMTMS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optMTMS.CheckedChanged
        Try
            If goappstate.InProgress = False Then

                If optMTMS.Checked = True Then
                    goappstate.LabelType = "MTMS"
                    If goappstate.ContainerType = "" Then
                        goappstate.NextAction = "Choose Container Type"
                        Me.lblAction.Text = goappstate.NextAction
                    Else
                        If goappstate.InProgress = False Then
                            goappstate.NextAction = "Scan First Part"
                            goappstate.NextActionType = ApplicationState.ActionType.PROCESS_BROADCASTCODE
                            Me.lblAction.Text = goappstate.NextAction
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.ApplicationModal, "Option MTMS")
        End Try
    End Sub
    Private Function BundleSerializedList() As String
        Dim ub As Int16, ub1 As Int16, j As Int16
        Dim sbuf As New StringBuilder
        Dim found As Boolean
        Dim i As Int16
        On Error Resume Next
        Err.Clear()
        ub = UBound(goappstate.Scans)
        If Err.Number <> 0 Then ub = -1

        Err.Clear()
        ub1 = UBound(goappstate.RemovedBECs)
        If Err.Number <> 0 Then ub1 = -1


        For i = 0 To ub
            If Not goappstate.Scans(i) Is Nothing Then
                found = False
                For j = 0 To ub1
                    If Not goappstate.RemovedBECs(j) Is Nothing Then
                        If goappstate.RemovedBECs(j) = goappstate.Scans(i) Then
                            found = True
                            Exit For
                        End If
                    End If
                Next j
                If Not found Then
                    'if the bec was not scan removed from the count, include it here
                    sbuf.Append(goappstate.Scans(i))
                    sbuf.Append("^")
                    sbuf.Append(goappstate.ScanTime(i))
                    sbuf.Append("^^")
                End If
            End If
        Next i
        Return sbuf.ToString
    End Function
    Public Sub EnableScanner()
        On Error Resume Next
        gbScannerEnabled = True
        'MSComm.PortOpen = True
        SerialPort1.Open()
        Application.DoEvents()
        Me.lblScannerStatus.Text = "Scanner Enabled"
        Me.lblScannerStatus.ForeColor = System.Drawing.Color.Green
        Me.Stx = "" 'clear out what was in the scanner buffer
    End Sub
    Public Sub DisableScanner()
        On Error Resume Next
        gbScannerEnabled = False
        'MSComm.PortOpen = False
        SerialPort1.Close()
        Application.DoEvents()
        Me.lblScannerStatus.Text = "Scanner Disabled"
        Me.lblScannerStatus.ForeColor = System.Drawing.Color.Red

    End Sub
    Private Sub optICS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optICS.CheckedChanged
        Try
            If goappstate.InProgress = False Then

                If optICS.Checked = True Then
                    goappstate.LabelType = "ICS"
                    If goappstate.ContainerType = "" Then
                        goappstate.NextAction = "Choose Container Type"
                        Me.lblAction.Text = goappstate.NextAction
                    Else
                        If goappstate.InProgress = False Then
                            'goAppState.NextAction = "Scan First Part"
                            'goAppState.NextActionType = ApplicationState.ActionType.PROCESS_BROADCASTCODE
                            'EnableScanner()
                            'Me.lblAction.Text = goAppState.NextAction
                        End If
                    End If
                End If
            Else
                goappstate.LabelType = "ICS"
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.ApplicationModal, "Option ICS")
        End Try
    End Sub
    Private Sub cmdCorrectProblem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCorrectProblem.Click
        Dim enableFlag As Boolean
        cmdCorrectProblem.Visible = False
        goappstate.NextActionType = ApplicationState.ActionType.PROCESS_SCANPARTS_COMMAND

        ProcessLastScan(enableFlag)
        If enableFlag Then
            EnableScanner()
        End If

    End Sub
    Public Sub refreshAction(ByVal Value As String)
        'jerry by lance request 0510
        If Value = "Choose ICS or MTMS" Then
            Me.lblAction.Text = ""
        Else
            Me.lblAction.Text = Value
        End If
        'Me.lblAction.Text = Value
    End Sub
    Private Sub Reset(ByVal Control As Int16)
        On Error Resume Next
        'Control = 0 implies we are not using a skid, so we reset accordingly
        'Control = 1 implies we are using a skid and don't want to erase that cached skid information
        'Control = 3 implies we are using a skid and want to clear it and start over.  This does not delete the skid form Oracle, that would need to be done using exceptions handler
        If Control = 0 Or Control = 1 Or Control = 3 Then
            optCardboard.Checked = False
            optReturnable.Checked = False
            optMTMS.Checked = False
            optICS.Checked = False
            goappstate.InProgress = False
            goappstate.LabelType = "            "
            goappstate.NextActionType = 1
            goappstate.NextAction = ""
            goappstate.BecSeries = ""
            BECSeries = ""
            lblAction.Text = ""

            Me.lblLastScan.Text = ""
            Me.optICS.Enabled = True
            If Plant = "23" Then
            Else
                Me.optMTMS.Enabled = True
            End If

            Me.txtCurrLayer.Text = ""

            DisableScanner()
            txtAdminCode.Text = ""
            Me.lblAction.Text = ""
            Me.cmdCorrectProblem.Visible = False
            Me.optICS.Checked = True
            Erase goappstate.Scans 'erase the scanned records
            Erase goappstate.ScanTime
            Erase goappstate.RemovedBECs 'erase the scan removed becs
            goappstate.TotalPartsScanned = 0
            Me.chkScanOut.Enabled = False
        End If
        If Control = 3 Or Control = 0 Then
            'wipe out everything
            Me.txtPackageCode.Text = ""
            goappstate.PackageCode = ""
            Me.txtDestinationCode.Text = ""
            goappstate.DestinationLocation = ""
            goappstate.DestinationCode = ""
            Me.txtPkgsPerSkid.Text = ""
            goappstate.NumberPkgsPerSkid = 0
            Me.txtSkidPackageCount.Text = ""
            goappstate.SkidPackageCount = 0
            Me.optCardboard.Enabled = True  'these should not be reset if control =1
            Me.optReturnable.Enabled = True
            Me.lblDelphiPart.Text = ""
            goappstate.DelphiPartNumber = ""
            Me.lblCustPartNbr.Text = ""
            goappstate.CustomerPartNumber = ""
            Me.lblECL.Text = ""
            goappstate.ECL = ""
            Me.txtNumPackages.Text = ""
            goappstate.NumberOfLayersPerBox = 0
            Me.txtTotalLayers.Text = ""
            goappstate.NumberOfBoxes = 0
            goappstate.ScheduleRequired = False
        End If
    End Sub
    Private Sub refreshFormControls()
        On Error Resume Next
        With goappstate
            Me.txtDestinationCode.Text = .DestinationCode
            Me.txtNumPackages.Text = .NumberOfBoxes
            Me.txtPackageCode.Text = .PackageCode
            Me.txtPkgsPerSkid.Text = .NumberPkgsPerSkid
            Me.txtTotalLayers.Text = .NumberOfLayersPerBox
            Me.lblDelphiPart.Text = .DelphiPartNumber
            Me.lblCustPartNbr.Text = .CustomerPartNumber
            Me.txtSkidPackageCount.Text = .SkidPackageCount
        End With
    End Sub
    Private Sub cmdResetCount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdResetCount.Click
        If BECSeries = "900" Then
            cmdResetCount.Enabled = False
            Exit Sub
        End If
        If goappstate.InProgress = True Then
            runningSetCount = 1
            ScannerEnabledWhenSetCountCalled = gbScannerEnabled
            DisableScanner()

            txtAdminCode.Text = ""
            DisableScanner()
            Dim o As New SetCount
            Dim i As String
            o.Tag = "Plant" & Plant
            If BECSeries = "800" Then
                o.minSetCount = minSetCount
                o.maxSetCount = maxSetCount
            End If
            o.ShowDialog()
            o = Nothing
            tmrRefresh.Enabled = True
        End If
    End Sub
    Private Sub frmMain_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        If goappstate.NextAction = "Choose ICS or MTMS" Then  'jerry by lance request 0510
            Me.lblAction.Text = ""
        Else
            Me.lblAction.Text = goappstate.NextAction
        End If
        'Me.lblAction.Text = goappstate.NextAction
    End Sub
    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Dim cs As Int16 = 0
        Dim retv As String
        Dim clearSchedule As Boolean

        '5/21/08 DSM: Activate DEVELOPMENT/PRODUCTION version as appropriate

        'DSM: DEVELOPMENT services
        'Dim greeting As New localhostRequest.Cordial
        'Dim webref As New localhostRequest.RequestLabel

        'DSM: PRODUCTION services
        Dim greeting As New dorme2.Cordial
        Dim webref As New dorme2.RequestLabel

        Dim i As Int16

        Try
            If MsgBox("Doing this will wipe out current operation, forcing to start over." & vbCrLf & "Are you sure you want to do this?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                'If Me.txtAdminCode.Text.ToUpper = conf.Appsettings("//Properties/AdminCode").ToUpper() Then  ' jerry fix 0510
                If Me.txtAdminCode.Text.ToUpper = conf.Appsettings("//Properties/AdminCode").ToUpper() Then
                    If goappstate.NumberPkgsPerSkid > 1 Then
                        If goappstate.PackageCode <> "4C" Then
                            'ask about the skid
                            If MsgBox("Do you wish to clear the current skid and start a new one? ", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                                clearSchedule = True
                                cs = 3
                            Else
                                clearSchedule = False
                                cs = 1
                            End If
                        Else
                            If MsgBox("Are you changing package type or Part Number?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                                clearSchedule = True
                                cs = 3
                            Else
                                clearSchedule = False
                                cs = 3
                            End If
                        End If
                    Else
                        If MsgBox("Are you changing package type or Part Number?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                            clearSchedule = True
                            cs = 0
                        Else
                            clearSchedule = False
                            cs = 0
                        End If
                    End If

                    If clearSchedule Then
                        Try
                            greeting.UserID = "delphikiosk"
                            greeting.Password = "forgetaboutit"
                            Application.DoEvents()
                            webref.CordialValue = greeting
                            i = 0
                            cmdReset.Tag = cmdReset.Text
                            cmdReset.Text = "Processing..."
                            cmdReset.Enabled = False
                            Application.DoEvents()

apportSkid:
                            If Not goappstate.ScheduleRequired Then
                                'there is not skid to abort, but there may be a serial number to unreserve
                                retv = webref.UnReserveSerials(goProperties.MachineName & "-RSV")
                            Else
                                retv = webref.AbortSkid(goappstate.ScheduleId, goappstate.CustomerPartNumber, goappstate.DestinationCode)
                                i = i + 1
                                If InStr(retv, "Failure") <> 0 Then
                                    If i < 3 Then
                                        GoTo apportSkid
                                    Else
                                        If MsgBox(retv & " Do you want to retry?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                                            GoTo apportSkid
                                        End If
                                    End If
                                End If
                            End If
                        Catch ex As Exception
                            MsgBox("Failure in Abort Skid: " & ex.Message, MsgBoxStyle.ApplicationModal)
                        Finally
                            webref.Dispose()
                            cmdReset.Text = cmdReset.Tag
                            cmdReset.Enabled = True
                        End Try
                    End If

                    Reset(cs) 'reset the form controls
                Else
                    MsgBox("Invalid Admin Code")
                End If
            End If
        Catch ex As Exception
            MsgBox("Failure in Clear Screen and Start over: " & ex.Message)
        End Try

    End Sub
    Private Sub tmrRefresh_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrRefresh.Tick
        On Error Resume Next
        Dim curlayer As Int16
        Dim curbox As Int16

        If runningExceptions = 2 Then
            'it was running, but has closed
            tmrRefresh.Enabled = False
            Application.DoEvents()
            If ScannerEnabledWhenExceptionsCalled Then

                EnableScanner()


            End If
            runningExceptions = 0
        End If


        If runningSetCount = 2 Then
            'set count has been closed
            Me.tmrRefresh.Enabled = False
            runningSetCount = 0
            Me.lblAction.Text = goappstate.NextAction
            Me.txtCurrLayer.Text = LayerProps("CurrentLayer", goappstate.TotalPartsScanned, "")
            If Me.txtCurrLayer.Text = "0" Then Me.txtCurrLayer.Text = ""
            If goappstate.TotalPartsScanned = (goappstate.StdPack / 2) Then
                Me.lblAction.ForeColor = System.Drawing.Color.Red
                PlayDaSound("CarHorn")
            ElseIf goappstate.TotalPartsScanned > (goappstate.StdPack / 2) Then
                Me.lblAction.ForeColor = System.Drawing.Color.Red
            Else
                Me.lblAction.ForeColor = System.Drawing.Color.ForestGreen
            End If
            If ScannerEnabledWhenSetCountCalled Then
                EnableScanner()
            End If
        End If

        If setCount = True Then
            setCount = False
            cmdCorrectProblem.Visible = False
        End If
    End Sub

    Public Function PlayDaSound(ByVal txtsound As String)
        Dim osnd As New SoundClass
        osnd.PlaySoundFile(Application.StartupPath & "\Sounds\" & txtsound & ".wav")
        osnd = Nothing

    End Function
    Private Class SoundClass

        Declare Auto Function PlaySound Lib "winmm.dll" (ByVal name As String, ByVal hmod As Integer, ByVal flags As Integer) As Integer
        Public Const SND_SYNC = &H0          ' play synchronously
        Public Const SND_ASYNC = &H1         ' play asynchronously
        Public Const SND_FILENAME = &H20000  ' name is file name
        Public Const SND_RESOURCE = &H40004  ' name is resource name or atom
        Public Sub PlaySoundFile(ByVal filename As String)
            ' Plays a sound from filename.
            Try
                PlaySound(filename, Nothing, SND_FILENAME Or SND_ASYNC)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
            End Try
        End Sub
    End Class

    Private Sub cmdExceptionsHandler_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExceptionsHandler.Click

        Try
            Me.cmdExceptionsHandler.Enabled = False
            Application.DoEvents()
            frmExceptions = New ExceptionsHandler
            frmExceptions.Part = lblDelphiPart.Text
            frmExceptions.LineID = conf.Appsettings("//Properties/MachineName")
            frmExceptions.lblPlant.Text = conf.Appsettings("//Properties/Plant")

            If goappstate.LastSerialNbr = "" Then
                frmExceptions.lblLastSerialNumber.Text = "N/A"
            Else
                If InStr(goappstate.LastSerialNbr, ":") <> 0 Then
                    frmExceptions.lblLastSerialNumber.Text = Mid(goappstate.LastSerialNbr, 1, InStr(goappstate.LastSerialNbr, ":") - 1)
                Else
                    frmExceptions.lblLastSerialNumber.Text = goappstate.LastSerialNbr
                End If
            End If
            runningExceptions = 1
            ScannerEnabledWhenExceptionsCalled = gbScannerEnabled
            DisableScanner()
            tmrRefresh.Enabled = True
            frmExceptions.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'make sure you save any data which has not been saved, such as scanned serials, buffered labels, etc
        On Error Resume Next
        btnClose.Text = "Closing"
        Application.DoEvents()

        Dim ub As Integer
        Dim i As Integer
        Dim fw As System.IO.StreamWriter
        Err.Clear()
        ub = UBound(goappstate.BufferedLabels)
        If Err.Number <> 0 Then ub = -1

        If ub > 0 Then
            fw = New System.IO.StreamWriter(Application.StartupPath & AppSettings("BufferedLabelsPath"))
            For i = 0 To ub
                If Not (goappstate.BufferedLabels(i) Is Nothing) Then
                    fw.WriteLine(goappstate.BufferedLabels(i))
                End If
            Next
            fw.Close()
        End If

        Err.Clear()
        ub = UBound(goappstate.Scans)
        If Err.Number = 0 Then
            fw = New System.IO.StreamWriter(Application.StartupPath & AppSettings("ScannedSerialsPath"))
            For i = 0 To ub
                If Not (goappstate.Scans(i) Is Nothing) Then
                    fw.WriteLine(goappstate.Scans(i) & "^" & goappstate.ScanTime(i) & "^^")
                End If
            Next
            fw.Close()
        End If

        Err.Clear()
        ub = UBound(goappstate.RemovedBECs)
        If Err.Number = 0 Then
            fw = New System.IO.StreamWriter(Application.StartupPath & AppSettings("ScanRemovedBECSPath"))
            For i = 0 To ub
                If Not (goappstate.RemovedBECs(i) Is Nothing) Then
                    fw.WriteLine(goappstate.RemovedBECs(i))
                End If
            Next
            fw.Close()
        End If
        Me.Close()
    End Sub
    Private Sub frmExceptions_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles frmExceptions.Closing
        Me.cmdExceptionsHandler.Enabled = True
    End Sub
    Public Function ProcessPartScanMultipleBox() As Boolean
        Dim iResponse As MsgBoxResult
        Dim retv As String

        If goappstate.IsFirstPartOfBox Then
            If Plant <> "23" Then
                ' if this is the first part of a box, then print a label for it now
                ' print a child label if it's ICS
                'If goappstate.LabelType = "ICS" Then
                '    PrintLabel("ICS", "InActive")
                'End If
            Else
                'plant 23 does not print the label on first part of box
                Return True
            End If
        End If

        'first check to see if the standard is complete
        If goappstate.TotalPartsScanned = goappstate.StdPack Then
            If BECSeries = "900" Then
                MsgBox("Package is complete", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                Dim b As Boolean = ProcessPackageComplete()
                '# to do
                'Process package complete
                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_BROADCASTCODE
                goappstate.NextAction = "Reset"
                Return True
            Else
                If IsComplete("Package") Then
                    Dim b As Boolean = ProcessPackageComplete()
                    '# to do
                    'Process package complete
                    goappstate.NextActionType = ApplicationState.ActionType.PROCESS_BROADCASTCODE
                    goappstate.NextAction = "Reset"
                    Return True
                Else
                    retv = LayerProps("CurrentLayer", goappstate.TotalPartsScanned, "Correction")
                    goappstate.NextAction = "Correct Package Count"
                    goappstate.NextActionType = ApplicationState.ActionType.PROCESS_CORRECT_PACKAGE_COUNT
                    DisableScanner()
                    Return False
                End If
            End If
        End If

        'if standard not complete, perhaps the box is complete
        If goappstate.CurrentBoxPartCount = goappstate.NumberOfPartsPerBox Then
            ' if this is not the last box...
            If goappstate.CurrentBoxCount <> goappstate.NumberOfBoxes Then
                If BECSeries = "900" Then
                    MsgBox("Box is complete.", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                    If ProcessBoxComplete() Then
                        goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                        Me.lblAction.Text = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                        Return True
                    End If
                Else
                    ' prepare for the next box
                    If IsComplete("Box") Then
                        If ProcessBoxComplete() Then
                            goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                            Me.lblAction.Text = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                            goappstate.NextActionType = ApplicationState.ActionType.PROCESS_SCANPARTS_COMMAND
                            Return True
                        End If
                    Else
                        retv = LayerProps("CurrentLayer", goappstate.TotalPartsScanned, "Correction")
                        goappstate.NextAction = "Correct Box Count"
                        goappstate.NextActionType = ApplicationState.ActionType.PROCESS_CORRECT_BOX_COUNT
                        DisableScanner()
                        Return False
                    End If
                End If
            End If
        End If

        'box is not complete, perhaps the layer is complete
        If LayerProps("IsCompleteLayer", goappstate.TotalPartsScanned, "") Then
            If BECSeries = "900" Then
                'don't ask
                MsgBox("Layer is complete", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                Me.lblAction.Text = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_SCANPARTS_COMMAND
                Return True
            Else
                If IsComplete("Layer") Then
                    goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                    Me.lblAction.Text = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                    goappstate.NextActionType = ApplicationState.ActionType.PROCESS_SCANPARTS_COMMAND
                    Return True
                Else
                    goappstate.NextAction = "Correct Layer Count"
                    goappstate.NextActionType = ApplicationState.ActionType.PROCESS_CORRECT_LAYER_COUNT
                    DisableScanner()
                    Return False
                End If
            End If
        End If

        goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
        Me.lblAction.Text = goappstate.NextAction
        Return True
    End Function
    Public Function ProcessBoxComplete() As Boolean

        Dim ep As String
        Try
            If goappstate.LabelType = "MTMS" Then
                PrintLabel("MTMS", "Inactive")
            Else
                ep = ""
                PrintLabel("ICS", "Active")
                'if containement logging feature is enabled, then open the containment form
                Dim sn As String
                ep = "opening containment log form"
                'If conf.Appsettings("//ApplicationState/LogContainment") = "True" Then 'jerry 0510
                If conf.Appsettings("//ApplicationState/LogContainment") = "True" Then
                    'LogContainment allows them to turn on this feature through the appconfig file
                    Dim frm As New frmContainmentLog
                    Dim ts As New StringBuilder

                    ts.Append("GP-12,")
                    ts.Append(conf.Appsettings("//Properties/LineNumber") & ",")
                    ts.Append(goappstate.DelphiPartNumber & ",")
                    ts.Append(goappstate.ECL & ",")
                    sn = goappstate.LastSerialNbr
                    If InStr(sn, ":") <> 0 Then
                        sn = Mid(sn, 1, InStr(sn, ":") - 1)
                    Else
                        'sn does not contain timestamp
                    End If

                    ts.Append(sn & ",")
                    ts.Append(goappstate.StdPack & ",")
                    frm.CommandString = ts.ToString
                    common.ProcessingContainment = True

                    EnableScanner()

                    frm.ShowDialog()
                End If
            End If

            Return True
        Catch ex As Exception
            MsgBox("Failure: " & ep & " > " & ex.Message, MsgBoxStyle.ApplicationModal)
            Return False
        End Try

    End Function
    Private Function LayerProps(ByVal Control As String, ByVal CurrScanned As Int32, ByVal Context As String) As Integer
        'this procedure calculates layer and box
        Dim l(1, 1) As Integer
        Dim b() As Integer
        Dim i As Integer
        Dim c As Integer
        Dim ppb As Integer
        Dim ppl As Integer
        Dim tl As Integer
        Dim lay As Integer
        Dim box As Integer
        Dim j As Integer
        Dim layersPerBox As Integer
        Dim NumBoxes As Integer
        Dim piecesPerStandard As Integer
        Dim curlayer As Int16
        Dim curbox As Int16
        Dim plc As Int16
        Dim cl As Integer
        Try
            'get pieces per layer
            ppb = goappstate.NumberOfPartsPerBox
            layersPerBox = goappstate.NumberOfLayersPerBox
            NumBoxes = goappstate.NumberOfBoxes
            piecesPerStandard = goappstate.StdPack

            ppl = ppb / layersPerBox

            tl = layersPerBox * NumBoxes

            ReDim b(NumBoxes)
            For i = 1 To NumBoxes
                b(i) = i * ppb
            Next i

            ReDim l(tl, 2)
            If goappstate.PackageCode = "4C" And BECSeries = "900" Then
                'this is special consideration 9 parts on first layer, 6 on the second layer of each box
                For i = 1 To tl
                    If i Mod 2 = 0 Then
                        j = j + 6
                        l(i, 1) = j
                    Else
                        j = j + 9
                        l(i, 1) = j
                    End If
                Next i
            Else
                For i = 1 To tl
                    l(i, 1) = i * ppl
                Next i
            End If

            c = 0
            For i = 1 To tl
                If c * ppl >= ppb Then
                    c = 1
                Else
                    c = c + 1
                End If
                l(i, 2) = c
            Next i

            For i = 1 To piecesPerStandard
                If CurrScanned < l(i, 1) Then
                    If CurrScanned > 0 Then
                        curlayer = l(i, 2)
                    Else
                        curlayer = 0
                    End If
                    ppl = l(i, 1)
                    'set these for 800 count setting restrictions
                    minSetCount = l(i - 1, 1) + 1
                    maxSetCount = l(i, 1) - 1
                    If minSetCount < 2 Then minSetCount = 2
                    If maxSetCount > goappstate.StdPack - 1 Then maxSetCount = goappstate.StdPack - 1
                    Exit For
                ElseIf CurrScanned = l(i, 1) Then
                    If CurrScanned > 0 Then
                        curlayer = l(i, 2)
                    Else
                        curlayer = 0
                    End If
                    ppl = l(i, 1)
                    'set these for 800 count setting restrictions
                    If Context = "Correction" Then
                        'this is set when we need to make a correction, from the "Is package Complete?"
                        minSetCount = l(i - 1, 1) + 1
                        maxSetCount = l(i, 1) - 1
                        If minSetCount < 2 Then minSetCount = 2
                        If maxSetCount > goappstate.StdPack - 1 Then maxSetCount = goappstate.StdPack - 1
                    Else
                        minSetCount = l(i, 1) + 1
                        Try
                            maxSetCount = l(i + 1, 1) - 1
                        Catch
                            maxSetCount = l(i, 1) - 1
                        End Try
                        If minSetCount < 2 Then minSetCount = 2
                        If maxSetCount > goappstate.StdPack - 1 Then maxSetCount = goappstate.StdPack - 1
                    End If
                    Exit For
                End If
            Next i
            For i = 1 To piecesPerStandard
                If CurrScanned <= b(i) Then
                    curbox = i
                    Exit For
                End If
            Next i

            If CurrScanned Mod (ppl) = 0 Then
                plc = ppl
                cl = True
            Else
                plc = CurrScanned - ppl
                cl = False
            End If


            Select Case Control
                Case "CurrentLayer"
                    Return curlayer
                Case "PartsPerLayer"
                    Return ppl
                Case "PartLayerCount"
                    Return plc
                Case "IsCompleteLayer"
                    Return cl
            End Select

        Catch ex As Exception
            MsgBox("Error Calculating CurrentLayer: " & ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Function
    Public Function ProcessPartScanSingleBox() As Boolean
        Dim sn As String
        Dim lc As String
        Dim retv

        If goappstate.TotalPartsScanned = goappstate.StdPack Then


            If BECSeries = "900" Then
                'don't ask
                MsgBox("Package is complete", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                Dim b As Boolean = ProcessPackageComplete()
                'Process package complete
                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_BROADCASTCODE
                goappstate.NextAction = "Reset"
                Return True
            Else
                If IsComplete("Package") Then
                    Dim b As Boolean = ProcessPackageComplete()
                    'Process package complete
                    goappstate.NextActionType = ApplicationState.ActionType.PROCESS_BROADCASTCODE
                    goappstate.NextAction = "Reset"
                    Return True
                Else
                    retv = LayerProps("CurrentLayer", goappstate.TotalPartsScanned, "Correction")
                    goappstate.NextAction = "Correct Package Count"
                    goappstate.NextActionType = ApplicationState.ActionType.PROCESS_CORRECT_PACKAGE_COUNT
                    DisableScanner()
                    Return False
                End If
            End If
        End If

        If LayerProps("PartLayerCount", goappstate.TotalPartsScanned, "") = LayerProps("PartsPerLayer", goappstate.TotalPartsScanned, "") Then

            If Plant = "23" And goappstate.PackageCode = "Y" Then
                lc = "Tray"
            Else
                lc = "Layer"
            End If
            If BECSeries = "900" Then
                MsgBox(lc & " is complete", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                Me.lblAction.Text = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_SCANPARTS_COMMAND
                Return True
            Else
                If IsComplete(lc) Then
                    goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                    Me.lblAction.Text = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                    goappstate.NextActionType = ApplicationState.ActionType.PROCESS_SCANPARTS_COMMAND
                    Return True
                Else
                    If lc = "Tray" Then
                        goappstate.NextAction = "Correct Tray Count"
                    Else
                        goappstate.NextAction = "Correct Layer Count"
                    End If
                    goappstate.NextActionType = ApplicationState.ActionType.PROCESS_CORRECT_LAYER_COUNT
                    Return False
                End If
            End If
        End If

        goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
        Me.lblAction.Text = goappstate.NextAction

        Return True
    End Function
    Private Sub ProcessLastScan(ByRef EnableScanner As Boolean)
        On Error Resume Next
        ' process the part just scanned
        EnableScanner = True

        If goappstate.IsMultipleBoxes Then
            If ProcessPartScanMultipleBox() Then
                'The package is complete, restart application
                If goappstate.NextAction = "Reset" Then
                    If goappstate.NumberPkgsPerSkid > 1 Then
                        If (goappstate.SkidPackageCount = goappstate.NumberPkgsPerSkid) Then
                            Reset(3) 'reset the skid also
                        Else
                            Reset(1) 'reset but not the skid
                        End If
                    Else
                        Reset(0) 'no skid, just reset
                    End If
                    DisableScanner()
                    EnableScanner = False
                End If
            Else
                Me.cmdCorrectProblem.Text = goappstate.NextAction
                Me.cmdCorrectProblem.Visible = True
                lblAction.Text = goappstate.NextAction & " and press button"
            End If
        Else
            If ProcessPartScanSingleBox() Then
                If goappstate.NextAction = "Reset" Then
                    If goappstate.NumberPkgsPerSkid > 1 Then
                        If goappstate.SkidPackageCount = goappstate.NumberPkgsPerSkid Then
                            Reset(3)
                        Else
                            Reset(1)
                        End If
                    Else
                        Reset(0)
                    End If
                    DisableScanner()
                    EnableScanner = False
                End If
            Else
                Me.cmdCorrectProblem.Text = goappstate.NextAction
                Me.cmdCorrectProblem.Visible = True
                lblAction.Text = goappstate.NextAction & " and press button"
            End If
        End If
    End Sub
    Public Function IsComplete(ByVal type As String) As Boolean
        Dim i As MsgBoxResult

        Select Case type.ToUpper
            Case "LAYER"
                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_IS_LAYER_COMPLETE
            Case "BOX"
                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_IS_BOX_COMPLETE
            Case "PACKAGE"
                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_IS_PACKAGE_COMPLETE
            Case "TRAY"
                goappstate.NextActionType = ApplicationState.ActionType.PROCESS_IS_TRAY_COMPLETE
        End Select
        i = MsgBox("Is " & type & " Complete?", MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.ApplicationModal, "Is " & type & " Complete")
        Select Case i
            Case MsgBoxResult.Yes
                Return True
            Case MsgBoxResult.No
                Return False
                setCount = False
        End Select
    End Function
    Public Function ProcessPackageComplete() As Boolean
        Dim ep As String

        Try
            ' lets see what label format we will be printing
            If goappstate.LabelType = "MTMS" Then
                If goappstate.IsMultipleBoxes Then
                    PrintLabel("MTMS", "Inactive")
                End If
                PrintLabel("MTMS Master", "Active")
            Else
                ' this is an ICS label
                PrintLabel("ICS", "Active")
                'if containement logging feature is enabled, then open the containment form
                Dim sn As String
                ep = "opening containment log form"
                If conf.Appsettings("//ApplicationState/LogContainment") = "True" Then
                    'LogContainment allows them to turn on this feature through the appconfig file
                    Dim frm As New frmContainmentLog
                    Dim ts As New StringBuilder

                    ts.Append("GP-12,")
                    ts.Append(conf.Appsettings("//Properties/LineNumber") & ",") 'jerry 0510
                    ts.Append(goappstate.DelphiPartNumber & ",")
                    ts.Append(goappstate.ECL & ",")
                    sn = goappstate.LastSerialNbr
                    If InStr(sn, ":") <> 0 Then
                        sn = Mid(sn, 1, InStr(sn, ":") - 1)
                    Else
                        'sn does not contain timestamp
                    End If

                    ts.Append(sn & ",")
                    ts.Append(goappstate.StdPack & ",")
                    frm.CommandString = ts.ToString
                    common.ProcessingContainment = True 'this is set so the scanner will work with the inspector ID

                    EnableScanner()

                    frm.ShowDialog()
                End If
            End If

            Return True
        Catch ex As Exception
            MsgBox("Failure: " & ep & " > " & ex.Message, MsgBoxStyle.ApplicationModal)
            Return False
        End Try



    End Function

    Public Function ProcessBroadcastCode(ByRef broadCastCode As String, ByVal useCache As Boolean) As String

        '5/21/08 DSM: Activate DEVELOPMENT/PRODUCTION version of services & class as appropriate

        '### DSM: DEVELOPMENT version ###
        'Dim pc As New localhostData.BECDATALOOKUP
        'Dim dest As New localhostRequest.clsDestination
        'Dim sd As New localhostRequest.RequestLabel

        '### DSM: PRODUCTION version ###
        Dim pc As New dorme1.BECDATALOOKUP
        Dim dest As New dorme2.clsDestination
        Dim sd As New dorme2.RequestLabel

        Dim ds As New DataSet
        Dim result As String
        Dim ecl As String
        Dim i As Int16
        Dim Dept As String
        Dim tmpBroadCast As String
        Dim ub As Integer

        Try
            'erase any buffered labels from previous request
            goappstate.BufferedLabelCount = 0
            Erase goappstate.BufferedLabels
            goappstate.BufferedLabelPointer = -1

            tmpBroadCast = broadCastCode.Replace("_", "").Replace(":", "")
            broadCastCode = broadCastCode.Replace("_", "").Replace(":", "")
            tmpBroadCast = broadCastCode
            If broadCastCode.Length >= 10 Then
                '900 series
                'The 1st 8 characters is the GM part number
                'Next 2 characters is the Delphi revision level
                'Next 4 characters is the Julian date
                'Next 2 characters is the line number
                'Next 6 characters is the hour-minute-second in which the part was built 
                ecl = Mid(broadCastCode, 9, 2)
                broadCastCode = Mid(broadCastCode, 1, 8)

                goappstate.BecSeries = "900"
                BECSeries = "900" 'local variable for speed
                'disable manual count setting
                Me.cmdResetCount.Enabled = False
                'enable the scan out feature
                Me.chkScanOut.Enabled = True
                Me.chkScanOut.Checked = False

            ElseIf broadCastCode.Length = 5 Then
                goappstate.BecSeries = "800"
                BECSeries = "800"
                cmdResetCount.Enabled = True
                Me.chkScanOut.Checked = False
                Me.chkScanOut.Enabled = False
            Else
                MsgBox(broadCastCode & " is Not a Valid Broadcast Code.", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                GoTo exithere
            End If

procBroadCode:
            Dept = goProperties.Department
            If Not useCache Then
                'MsgBox("Line 1986: Calling processbroad... " & broadCastCode & vbCrLf & Dept & vbCrLf & result & vbCrLf & goappstate.ContainerType & vbCrLf & goappstate.LabelType)

                ds = pc.ProcessBroadcastCode(broadCastCode, Dept, result, goappstate.ContainerType, goappstate.LabelType)

                i = i + 1

                If InStr(result, "Success") = 0 Then
                    MsgBox(result, MsgBoxStyle.Exclamation Or MsgBoxStyle.ApplicationModal)
                    Return "Error Processing BroadCast Code =>" & result
                End If

                If ds.Tables(0).Rows.Count > 0 Then
                    goappstate.DelphiPartNumber = Convert.ToString(ds.Tables(0).Rows(0).Item("DelphiPartNumber"))
                    goappstate.MESPartID = Convert.ToInt64(ds.Tables(0).Rows(0).Item("MESPartID"))
                    goappstate.BroadcastCode = broadCastCode
                    goappstate.ProductionRunNbr = Convert.ToInt64(ds.Tables(0).Rows(0).Item("ProdRunID"))
                    If BECSeries = "800" Then
                        goappstate.ECL = broadCastCode.Substring(3, 2)
                    ElseIf BECSeries = "900" Then
                        goappstate.ECL = ecl
                    End If
                Else
                    'MsgBox("Failed with the call at line 2000")
                    If i < 3 Then  'automatically retry 3 times
                        GoTo procBroadCode
                    Else
                        If MsgBox("Failed to Process BroadCast Code because: " & result & " do you wish to retry?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                            GoTo procBroadCode
                        Else
                            MsgBox("Verify that a production run exist for this Part,ECL and Package Type.", MsgBoxStyle.ApplicationModal)
                        End If
                    End If
                End If

                goappstate.PackageCode = Convert.ToString(ds.Tables(0).Rows(0).Item("PackageCode"))
                goappstate.Department = Convert.ToString(ds.Tables(0).Rows(0).Item("Department"))
                goappstate.StdPack = Convert.ToInt32(ds.Tables(0).Rows(0).Item("StdPack"))
                goappstate.NumberOfBoxes = Convert.ToInt32(ds.Tables(0).Rows(0).Item("NumberOfBoxes"))



                If (Plant = "23") And Convert.ToString(ds.Tables(0).Rows(0).Item("PackageCode")) = "Y" Then
                    goappstate.NumberOfLayersPerBox = Convert.ToInt32(ds.Tables(0).Rows(0).Item("NumberOfLayersPerBox"))
                Else
                    goappstate.NumberOfLayersPerBox = Convert.ToInt32(ds.Tables(0).Rows(0).Item("NumberOfLayersPerBox"))
                End If


                goappstate.ScheduleRequired = ds.Tables(0).Rows(0).Item("ScheduleRequired")
                goappstate.CustomerPartNumber = ds.Tables(0).Rows(0).Item("CustomerPartNumber")


                goappstate.StartTime = Now.Now
            End If
            If BECSeries = "900" Then 'add this scan to the list of scanned parts
                Try
                    ub = UBound(goappstate.Scans)
                Catch ex As Exception
                    ub = 0
                End Try
                ReDim Preserve goappstate.Scans(ub + 1)
                ReDim Preserve goappstate.ScanTime(ub + 1)
                goappstate.Scans(ub) = tmpBroadCast
                goappstate.ScanTime(ub) = Now.Now
            End If
            'msgBox("completed to line 2039")
            'next, we must go and get the skid information, etc
            dest.Part_nbr = goappstate.DelphiPartNumber
            dest.mes_part_id = goappstate.MESPartID
            dest.Package_Code = goappstate.PackageCode
            dest.mes_machine_id = goProperties.LineNumber
            dest.MachineName = goProperties.MachineName
            dest.Revision_Physical = goappstate.ECL
            If goappstate.ScheduleRequired Then
                dest.UseGeneric = False
            Else
                dest.UseGeneric = True
            End If
            dest.QtyPieces = goappstate.StdPack
            dest.ControlFlag = "Reserve"
            i = 0

getShipDest:
            'MsgBox("starting line 2049")

            If Not useCache Then

                result = sd.GetShipDestination_Webmethod(dest)
                'Dim retv As String
                'retv = ReserveSerials(dest, 1)

                i = i + 1
                If InStr(result, "Failure") <> 0 Then
                    If i < 3 Then
                        GoTo getShipDest 'try three times before giving a failure
                    Else
                        MsgBox("Failure getting a ship destination: " & result, MsgBoxStyle.ApplicationModal)
                        GoTo exithere
                    End If
                Else
                    '2/20/2006  with the addition of the auto-Generic logic, we also need to check the use_generic flag to determine if
                    '           if rolled to generic
                    If dest.UseGeneric Then
                        'it has rolled over to use generic
                        goappstate.ScheduleRequired = False
                        goappstate.DestinationCode = "Generic"
                        goappstate.DestinationLocation = ""
                    Else
                        goappstate.NumberPkgsPerSkid = dest.QtyPkgsperSkidReq
                        goappstate.SkidPackageCount = dest.QtyPkgsUsed
                        goappstate.DestinationCode = dest.CNTR
                        goappstate.DestinationLocation = dest.location
                        goappstate.ScheduleId = dest.ScheduleID
                    End If
                End If

                If dest.UseGeneric Then
                    'it has rolled over to use generic
                    goappstate.ScheduleRequired = False
                    goappstate.DestinationCode = "Generic"
                Else
                    goappstate.SkidPackageCount = 0
                    goappstate.DestinationLocation = dest.location ' DSM: Added for SAP call
                End If

            End If
            'MsgBox("starting line 2067")
            refreshFormControls() 'update what's on the form
            Return "Success"
exithere:
        Catch ex As Exception
            'MsgBox(ex.StackTrace)
            Return "Error Processing BroadCast Code =>" & ex.Message & Space(1) & ex.StackTrace
        Finally
            ds = Nothing
            sd = Nothing
        End Try
    End Function

    Private Function ReserveSerials(ByRef Destination As dorme2.clsDestination, ByVal HowMany As Int16) As String
        '2/14/2006  This function must reserve HowMany serials for printing.  It does this by running a sql statement and then verifying the
        '           results.  When the results don't comply, a rollback is done and the Failure is passed back to the caller
        '   It returns either a serial number, or a Failure statement such as FailedNotEnoughSerials of Failure...


        Dim cmd As New OracleCommand
        Dim sqli As New System.Text.StringBuilder
        Dim inSql As New System.Text.StringBuilder
        Dim en As Int16
        Dim ep As String
        Dim MyTran As OracleTransaction
        Dim cmd1 As New OracleCommand
        Dim da As New OracleDataAdapter
        Dim ds As New DataSet
        Dim cn As New OracleConnection()
        Dim destinationlocation As String
        Dim strconn As String
        strconn = "Data Source = "
        strconn &= "(DESCRIPTION = "
        strconn &= " (ADDRESS_LIST ="
        strconn &= " (ADDRESS = (PROTOCOL = TCP)(HOST =  uswana07.northamerica.delphiauto.net)(PORT = 1521))"
        strconn &= " )"
        strconn &= " (CONNECT_DATA ="
        strconn &= " (SERVICE_NAME = PLT23DV)"
        strconn &= " )"
        strconn &= ");"
        strconn &= "User Id=mesweb;"
        strconn &= "password=mesfull;"

        'If you do set up tnsnames.ora , you can use the following code:
        'strconn = "user id=your_userid;password=your_password;data source=your_oracle_server/your_oracle_service"

        cn.ConnectionString = strconn
        Try
            cn.Open()
            MyTran = cn.BeginTransaction
            cmd.Connection = cn
            cmd.Transaction = MyTran
            Dim lsSQl As String

            If Destination.UseGeneric Then
                'The will print call should already have a serial reserved for Generics.  Look for that one first, then reserve
                ep = "looking for previously reserved Generic Serial"
                sqli.Remove(0, sqli.Length)
                sqli.Append("SELECT SERIAL_NBR FROM pltfloor.ICS_LABEL_DATA_VW WHERE")
                en = 1
                sqli.Append(" PART_NBR = '" & Destination.Part_nbr & "'")
                en = 2
                sqli.Append(" AND QUANTITY = " & Destination.QtyPieces)
                sqli.Append(" AND Used_Status = '1'")
                en = 3
                sqli.Append(" AND Upper(Package_Code) = '" & UCase(Destination.Package_Code) & "'")
                en = 4
                sqli.Append(" AND Upper(ECL) = '" & UCase(Destination.Revision_Physical) & "'")
                sqli.Append(" AND DESTINATION is null ")
                sqli.Append(" AND (Sysdate - mod_tmstm <= 350)") 'make sure n
                en = 5
                sqli.Append(" and machine_name = '" & Destination.MachineName & "-RSV'")
                en = 6
                lsSQl = sqli.ToString
                With cmd
                    .CommandType = CommandType.Text
                    .CommandText = sqli.ToString
                    .ExecuteNonQuery()
                End With
                da = New OracleDataAdapter(cmd)
                da.Fill(ds)
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        Return ds.Tables(0).Rows(0).Item("serial_nbr")
                    Else
                        'no serial found
                    End If
                Else
                    'no serial found
                End If
            End If

            'at this point, one of two conditions exist.
            '1) not using a generic
            '2) using a generic, but did not find one reserved
            '   in Both cases we go through this next reserve serial logic
            'Beginning reserve logic:  this is the sub query
            ep = "building sub-query string for ReserveSerials"
            inSql.Remove(0, inSql.Length)
            inSql.Append("(SELECT SERIAL_NBR FROM pltfloor.ICS_LABEL_DATA_VW WHERE")
            en = 10
            inSql.Append(" PART_NBR = '" & Destination.Part_nbr & "'")
            en = 20
            inSql.Append(" AND QUANTITY = " & Destination.QtyPieces)
            inSql.Append(" AND Used_Status = '0'")
            en = 30
            inSql.Append(" AND Upper(Package_Code) = '" & UCase(Destination.Package_Code) & "'")
            en = 40
            inSql.Append(" AND Upper(ECL) = '" & UCase(Destination.Revision_Physical) & "'")
            If Destination.UseGeneric Then
                inSql.Append(" AND DESTINATION is null ")
            Else
                en = 50
                inSql.Append(" AND upper(DESTINATION) = '" & UCase(Trim$(Destination.CNTR)) & "' ")
            End If
            inSql.Append(" AND (Sysdate - mod_tmstm <= 350)") 'make sure n
            en = 60
            inSql.Append(" AND rownum < " & HowMany + 1 & ")")
            lsSQl = inSql.ToString

            'build main update sql
            sqli.Remove(0, sqli.Length)
            'DSM: don't have rights with TV10 to update ics_label_data_vw
            sqli.Append("Update snapmgr.ics_label_data set used_status = 1, ")
            'sqli.Append("Update pltfloor.ics_label_data_vw  set used_status = 1, ")

            sqli.Append(" machine_name = '")
            en = 70
            If Destination.UseGeneric Then
                sqli.Append(Destination.MachineName & "-RSV'")   'with generic, there will not be a 2-12 follow up and there is not a schedule ID, so we use machinename and .
            Else
                sqli.Append(Destination.ScheduleID & "' || '-' || rownum ")
            End If
            sqli.Append(" where serial_nbr in ")
            en = 80
            sqli.Append(inSql.ToString)
            lsSQl = sqli.ToString

            'reserve the labels
            With cmd
                .CommandType = CommandType.Text
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With

            'check to see if we successfully reserved enough
            With cmd
                sqli.Remove(0, sqli.Length)
                If Destination.UseGeneric Then
                    sqli.Append("Select serial_nbr from pltfloor.ics_label_data_vw where machine_name = '")
                    sqli.Append(Destination.MachineName & "-RSV' and used_status = 1 order by serial_nbr ")
                Else
                    sqli.Append("Select serial_nbr from pltfloor.ics_label_data_vw where machine_name like '")
                    sqli.Append(Destination.ScheduleID & "-%' and used_status = 1 order by serial_nbr ")
                End If
                lsSQl = sqli.ToString
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count < HowMany Then
                'we were not able to reserve all the labels required, we must assume they have run out.
                MyTran.Rollback()
                Return "FailureNotEnoughSerials"
            Else
                MyTran.Commit()
                Destination.SerialNbr = ds.Tables(0).Rows(0).Item("Serial_Nbr")

                'DSM: add embedding destination location in the destination object for SAP call
                destinationlocation = GetDestinationLocation(Destination.SerialNbr)

                If Mid(destinationlocation, 1, 7) = "Failure" Then
                    Destination.location = ""
                    Return destinationlocation
                Else
                    Destination.location = destinationlocation
                End If

                Return ds.Tables(0).Rows(0).Item("Serial_Nbr")
            End If

        Catch ex As Exception
            Return "Failure in ReserveSerials: " & ep & " at line marker " & en & " because " & ex.Message
        Finally
            cmd.Dispose()
            cmd1.Dispose()
            ds.Dispose()
            cn.Close()
            cn.Dispose()
        End Try
    End Function
    Private Function GetDestinationLocation(ByVal serialnbr As String) As String
        '4/16/08 DSM: created to get Destination Location (PKG_12Z_SEGM_NBR field from ICS_LABEL_DATA table)


        Dim cnOra As New OracleConnection()
        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Dim location As String = ""
        Dim sqli As New StringBuilder
        Dim strconn As String
        strconn = "Data Source = "
        strconn &= "(DESCRIPTION = "
        strconn &= " (ADDRESS_LIST ="
        strconn &= " (ADDRESS = (PROTOCOL = TCP)(HOST =  uswana07.northamerica.delphiauto.net)(PORT = 1521))"
        strconn &= " )"
        strconn &= " (CONNECT_DATA ="
        strconn &= " (SERVICE_NAME = PLT23DV)"
        strconn &= " )"
        strconn &= ");"
        strconn &= "User Id=mesweb;"
        strconn &= "password=mesfull;"

        'If you do set up tnsnames.ora , you can use the following code:
        'strconn = "user id=your_userid;password=your_password;data source=your_oracle_server/your_oracle_service"

        cnOra.ConnectionString = strconn
        Try
            cnOra.Open()
            sqli.Append("select PKG_12Z_SEGM_NBR from snapmgr.ics_label_data where serial_nbr = '" & serialnbr & "'")
            With cmd
                .Connection = cnOra
                .CommandType = CommandType.Text
                .CommandText = sqli.ToString
                .ExecuteNonQuery()
            End With
            da = New OracleDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("PKG_12Z_SEGM_NBR")) Then
                    location = ds.Tables(0).Rows(0).Item("PKG_12Z_SEGM_NBR")
                End If
            End If
            Return location
        Catch ex As Exception
            Return "Failure in GetDestinationLocation: " & ex.Message
        End Try

    End Function
    Private Sub NotifySAPofPrintedLabel(ByVal serial_nbr As String)
        '6/2/08: DSM modify to use new GetSAPLabelData in RequestLabel service, add package_type to call 
        '3/18/08: David Maibor (DSM)created to notify SAP of printed label by calling the SAP PostSerializedProduction web method
        'PostSerializedProduction parameters
        '1: strClientName - VARCHAR2(128 Bytes)
        '2: strPlantId - VARCHAR2(4 Bytes)
        '3: strMaterialNumber - VARCHAR2(18 Bytes)
        '4: strProductVersion - VARCHAR2(4 Bytes)
        '5: dblQuantity - NUMBER(13, 3)
        '6: datManufacturingDateTime - DATE
        '7: strOperatorId - VARCHAR2(12 Bytes)
        '8: datRequestDatetime - DATE
        '9: strSerialNumber - VARCHAR2(10 Bytes)
        '10: strStorageLocation - VARCHAR2(4 Bytes)
        '11: strShipToNumber - VARCHAR2(10 Bytes)
        '12: strContainerNumber - VARCHAR2(18 Bytes)
        '13: strLotNumber - VARCHAR2(10 Bytes)
        '14: strDestinationLocation - VARCHAR2(20 Bytes)
        '15: strCustomerName - VARCHAR2(40 Bytes)
        '16: strCustomerPlant - VARCHAR2(40 Bytes)
        '17: strAddressStreet - VARCHAR2(40 Bytes)
        '18: strAddressCity - VARCHAR2(40 Bytes)
        '19: strAddressState - VARCHAR2(3 Bytes)
        '20: strAddressPostalCode - VARCHAR2(10 Bytes)
        '21: strAddressCountry - VARCHAR2(10 Bytes)
        '22: strCustomerPartNumber - VARCHAR2(35 Bytes)
        '23: strPartDescription - VARCHAR2(15 Bytes)
        '24: dblGrossWeight - NUMBER (15,3)
        '25: strEngineeringChange - VARCHAR2(10 Bytes)
        '26: strContactPerson - VARCHAR2(16 Bytes)
        '27: strOptionalText1 - VARCHAR2(16 Bytes)
        '28: strOptionalText2 - VARCHAR2(16 Bytes)
        '29: strOptionalText3 - VARCHAR2(16 Bytes)
        '30: strOptionalText4 - VARCHAR2(16 Bytes)
        '31: strOptionalText5 - VARCHAR2(16 Bytes)
        '32: strDestination - ? string
        '33: strDunnscode - VARCHAR2(9 Bytes)
        '34: strLabelFile - VARCHAR2(16 Bytes)
        '35: iCopies - NUMBER(2)
        '36: strLabelType - VARCHAR2(3 Bytes)
        '37: iNumberOfBoxes - NUMBER(3)
        '38: strCountryOfOrigin - VARCHAR2(3 Bytes)
        '39: iTimeout - NUMBER(3)
        '40: fBufferOnSystemRException - BOOLEAN

        Const BUFFERONSYSTEMEXCEPTION = True
        Const CLIENTNAME = "Brookhaven"
        Const COUNTRYOFORIGIN = "US"
        Const COPIES = 2
        Const CUSTOMER = "GM2D_LABL"
        Const GENERIC = "PACKARD_B10"
        Const LABELTYPE = "STD"
        Const NUMBEROFBOXES_WITH_SERIALNUMBER = 1
        Const PLANTID = "FT23"
        Const PRODUCTVERSION = "0001"
        Const SAPFAILUREMSG = "There was a problem reporting this order to SAP. Please notify PC&L of the problem." & vbCrLf & vbCrLf & "The error is: "
        Const STORAGELOCATION = "0002"
        Const TIMEOUT = 30000


        Dim errmsg As String
        Dim labelformat As String
        Dim msg As String
        Dim original_serial_nbr As String = Mid(LastSerialNbr, 1, 10)
        Dim package_type As String 'DSM: added, 6/2/08
        Dim SAPInfo As DataSet
        Dim SAPreturnvalue As String = ""
        Dim use_generic As Boolean

        'Variables for SAP call
        Dim SAP_cust_address As String = ""
        Dim SAP_cust_city As String = ""
        Dim SAP_cust_cntry As String = ""
        Dim SAP_cust_name As String = ""
        Dim SAP_cust_state As String = ""
        Dim SAP_cust_zip As String = ""
        Dim SAP_destination_location As String = " "  ' AC 4/18/08 Remove "none" - should be blank
        Dim SAP_DunnsCode As String = " " ' AC  4/18/08 Added variable for Dunns Code
        Dim SAP_gross_wgt As Single
        Dim SAP_OPTIONAL_TEXT5 As String = "" 'DSM: for PLANT_DOCK_LINE_1 field in SAP Label_Data table 
        Dim SAP_part_description As String = ""
        Dim SAP_plant As String = PLANTID
        Dim SAP_quantity As Integer = goappstate.StdPack
        Dim SAP_shipto As String = ""
        Dim SAP_std_pack_cntr As String = ""

        'Determine if part is generic or customer specific
        If goappstate.ScheduleRequired Then
            use_generic = False
        Else
            use_generic = True
        End If

        '5/21/08 DSM: select Dev/Production service

        '*** DSM: RequestLabelsvs call ***
        'Dim RQLabelsvs As New localhostRequest.RequestLabel  'activate for DEVELOPMENT testing
        Dim RQLabelsvs As New dorme2.RequestLabel   'activate for PRODUCTION

        '*** DSM: SAP service call ***
        'Dim SAPsvs As New gold2.Service 'activate for DEVELOPMENT testing
        Dim SAPsvs As New dorme.Service 'activate for PRODUCTION

        Dim tmpstr As String

        '### Test
        'Try
        '    Dim retv As String
        '    package_type = "CARDBOARD"
        '    SAPInfo = RQLabelsvs.GetSAPLabelData(serial_nbr, package_type)
        '    retv = SAPInfo.Tables("BEC_Part_Cntr_Info").Rows(0).Item("result")
        '    If Mid(retv, 1, 7) = "failure" Then
        '        MsgBox(SAPFAILUREMSG & "Failure getting SAP info from the DB." & retv, MsgBoxStyle.OKOnly, "ERROR: NOTIFYING SAP")
        '    Else
        '        MsgBox(SAPFAILUREMSG & "Success getting SAP info from the DB.", MsgBoxStyle.OKOnly, "SUCCESS")
        '    End If
        'Catch ex As Exception
        '    MsgBox(SAPFAILUREMSG & ex.Message, MsgBoxStyle.OKOnly, "ERROR: NOTIFYING SAP")
        'End Try

        'Exit Sub

        Try
            'DSM: add package_type to GetSAPLabelData call
            If Me.optCardboard.Checked Then
                package_type = "CARDBOARD"
            Else
                package_type = "RETURNABLE"
            End If
            SAPInfo = RQLabelsvs.GetSAPLabelData(original_serial_nbr, package_type)
            msg = Mid(SAPInfo.Tables("BEC_Part_Cntr_Info").Rows(0).Item("result"), 1, 7)

            'Abort if 'failure' in getting SAP info
            If msg = "failure" Then
                MsgBox(SAPFAILUREMSG & "Failure getting SAP info from the DB." & vbCrLf & vbCrLf & msg, MsgBoxStyle.OkOnly, "ERROR: NOTIFYING SAP")
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox(SAPFAILUREMSG & ex.Message, MsgBoxStyle.OkOnly, "ERROR: NOTIFYING SAP")
            Exit Sub
        End Try


        If Len(goappstate.DestinationLocation) > 0 Then
            SAP_destination_location = goappstate.DestinationLocation
        End If

        'Extract info from SAP_label_data table, if there is data and this is for a customer specific part
        If SAPInfo.Tables("SAP_label_data").Rows.Count > 0 And Not use_generic Then

            If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_name")) Then
                SAP_cust_name = SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_name")
            End If

            If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("ship_to")) Then
                SAP_shipto = SAPInfo.Tables("SAP_label_data").Rows(0).Item("ship_to")
            End If

            'DSM: plant is hardcoded to 'FT23'
            'If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("plant")) Then
            '    SAP_plant = SAPInfo.Tables("SAP_label_data").Rows(0).Item("plant")
            'End If

            If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_address")) Then
                SAP_cust_address = SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_address")
            End If

            If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_city")) Then
                SAP_cust_city = SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_city")
            End If

            If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_state")) Then
                SAP_cust_state = SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_state")
            End If

            If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_zip_code")) Then
                SAP_cust_zip = SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_zip_code")
            End If

            If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_cntry")) Then
                SAP_cust_cntry = SAPInfo.Tables("SAP_label_data").Rows(0).Item("cust_cntry")
            End If

            ' AC 4/18/08 Added code to extract SAP_DunnsCode - Also added field in Web Service
            If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("DUNNS_CODE")) Then
                SAP_DunnsCode = SAPInfo.Tables("SAP_label_data").Rows(0).Item("DUNNS_CODE")
            End If

            'DSM: 4/23/08 Add code to extract SAP_destination from SAP Label_Data table, field: plant_dock_line_1
            If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("PLANT_DOCK_LINE_1")) Then
                'Truncate to 16 chars because SAP PostSerializedProduction only allows 16 chars for this field
                tmpstr = SAPInfo.Tables("SAP_label_data").Rows(0).Item("PLANT_DOCK_LINE_1")
                If tmpstr.Length > 16 Then
                    tmpstr = Mid(tmpstr, 1, 16)
                End If
                SAP_OPTIONAL_TEXT5 = tmpstr
            End If

            If Not IsDBNull(SAPInfo.Tables("SAP_label_data").Rows(0).Item("part_description")) Then
                'Truncate to 15 chars because SAP PostSerializedProduction only allows 15 chars for this field
                tmpstr = SAPInfo.Tables("SAP_label_data").Rows(0).Item("part_description")
                If tmpstr.Length > 15 Then
                    tmpstr = Mid(tmpstr, 1, 15)
                End If
                SAP_part_description = tmpstr
            End If

        End If


        'Extract BEC_Part_Cntr_info 
        If SAPInfo.Tables("BEC_Part_Cntr_Info").Rows.Count > 0 Then
            If Not IsDBNull(SAPInfo.Tables("BEC_Part_Cntr_Info").Rows(0).Item("std_pack_cntr")) Then
                SAP_std_pack_cntr = SAPInfo.Tables("BEC_Part_Cntr_Info").Rows(0).Item("std_pack_cntr")
            End If

            If Not IsDBNull(SAPInfo.Tables("BEC_Part_Cntr_Info").Rows(0).Item("weight")) Then
                SAP_gross_wgt = SAPInfo.Tables("BEC_Part_Cntr_Info").Rows(0).Item("weight")
            End If

            If Not IsDBNull(SAPInfo.Tables("BEC_Part_Cntr_Info").Rows(0).Item("quantity")) Then
                SAP_quantity = SAPInfo.Tables("BEC_Part_Cntr_Info").Rows(0).Item("quantity")
            End If
        Else
            MsgBox(SAPFAILUREMSG & "Unable to find SAP data in the BEC_Part_Cntr_Info table", MsgBoxStyle.OkOnly, "ERROR: NOTIFYING SAP")
            Exit Sub
        End If


        'Reduce by 1/4 for 4C   - Now handled by SAP Info DSM: 6/2/08
        'If goappstate.PackageCode = "4C" Then SAP_quantity = 15


        If goappstate.ScheduleRequired Then
            labelformat = CUSTOMER
        Else
            labelformat = GENERIC
            goappstate.DestinationCode = ""     'DLOC should be blank for genercis ACR 4/8
            goappstate.CustomerPartNumber = ""  'No Cust Part Number ACR 4/8
            SAP_shipto = ""                     'No Ship_To ACR 4/8
            SAP_cust_name = ""                  'ACR 4/20 blank the cust fields 
            SAP_cust_address = ""
            SAP_cust_city = ""
            SAP_cust_state = ""
            SAP_cust_zip = ""
        End If

hardcode:
        Try
            ' DSM: notify SAP via the PostSerializedProduction web method
            SAPreturnvalue = SAPsvs.PostSerializedProduction( _
                CLIENTNAME, _
                PLANTID, _
                goappstate.DelphiPartNumber, _
                PRODUCTVERSION, _
                SAP_quantity, _
                Now(), _
                conf.Appsettings("//Properties/MachineName"), _
                Now(), _
                serial_nbr, _
                STORAGELOCATION, _
                SAP_shipto, _
                SAP_std_pack_cntr, _
                "", _
                SAP_destination_location, _
                SAP_cust_name, _
                SAP_plant, _
                SAP_cust_address, _
                SAP_cust_city, _
                SAP_cust_state, _
                SAP_cust_zip, _
                SAP_cust_cntry, _
                goappstate.CustomerPartNumber, _
                SAP_part_description, _
                SAP_gross_wgt, _
                goappstate.ECL, _
                "", _
                "", _
                "", _
                "", _
                "", _
                SAP_OPTIONAL_TEXT5, _
                "", _
                SAP_DunnsCode, _
                labelformat, _
                COPIES, _
                LABELTYPE, _
                NUMBEROFBOXES_WITH_SERIALNUMBER, _
                COUNTRYOFORIGIN, _
                TIMEOUT, _
                BUFFERONSYSTEMEXCEPTION)

        Catch exSoapException As Protocols.SoapException
            msg = "Protocols.SoapException: " & exSoapException.Actor
            Select Case exSoapException.Actor
                Case "SapFatalException"
                    errmsg = exSoapException.Detail.SelectSingleNode("//Message").InnerText
                Case "SapConfigurationException"
                    errmsg = exSoapException.Detail.SelectSingleNode("//Message").InnerText
                Case "SapTransactionException"
                    errmsg = ""
                    For Each nodex As System.Xml.XmlNode In exSoapException.Detail.SelectSingleNode(("//StatusMessage"))
                        errmsg &= ": " & nodex.InnerText
                    Next
            End Select
            MsgBox(SAPFAILUREMSG & ": " & errmsg, MsgBoxStyle.OkOnly, "ERROR: NOTIFYING SAP")
        Catch ex As Exception
            Dim strX As String = ex.Message
            MsgBox(SAPFAILUREMSG & ": " & ex.Message, MsgBoxStyle.OkOnly, "ERROR: NOTIFYING SAP")
        End Try

    End Sub
    Public Sub PrintLabel(ByVal stdPackType As String, ByVal labelType As String)

        '6/4/08 DSM: add saving of label variable in the global variable, reprintlabel 
        '5/21/08 DSM: select DEVELOPMENT/PRODUCTION version of RequestLabel as appropriate
        'Mar. 12, 2008: David Maibor (DSM) added call to SAP web service after the Print commands 

        'Dim labelRequest As New localhostRequest.RequestLabel    'DSM: DEVELOPMENT version
        'Dim greeting As New localhostRequest.Cordial     'DSM: DEVELOPMENT version
        Dim labelRequest As New dorme2.RequestLabel 'DSM: PRODUCTION version
        Dim greeting As New dorme2.Cordial          'DSM: PRODUCTION version

        Dim label As String
        Dim dt As New DataTable
        Dim p As New RawPrinterHelper
        Dim i As Int16
        Dim ls As String

        'DSM added
        Dim charposition As Integer
        Dim labelchars As Integer
        Dim serial_nbr As String

        Try
            DisableScanner()
            reprintlabel = ""
            If goappstate.BufferedLabelCount > 0 And (goappstate.BufferedLabelPointer < (goappstate.BufferedLabelCount - 1)) Then
                'print this label and increment the pointer
                Pdoc.PrinterSettings.PrinterName = "Generic"
                If Pdoc.PrinterSettings.IsValid Then

                    'DSM:These are the 2nd/3rd/4th labels for an Underhood, 4-C package. 
                    p.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, goappstate.BufferedLabels(goappstate.BufferedLabelPointer + 1))

                    'DSM: save label just printed in reprintlabel global variable
                    reprintlabel = MakeReprintLabel(goappstate.BufferedLabels(goappstate.BufferedLabelPointer + 1))

                    'DSM: give SAP new SN generated from BecWebService
                    NotifySAPofPrintedLabel(newSerialNumbers(goappstate.BufferedLabelPointer))

                    goappstate.BufferedLabelPointer += 1
                    goappstate.SkidPackageCount += 1
                    refreshFormControls()
                    If goappstate.BufferedLabelPointer = (goappstate.BufferedLabelCount - 1) Then

                        'DSM: BufferedLabels(0) is the info for the Master Label. Master label is no longer printed for 4-C underhoods at the kiosk
                        'p.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, goappstate.BufferedLabels(0))

                        'This is the last label in the buffer, so let's save the serialized product list now
                        greeting.UserID = "delphikiosk"
                        greeting.Password = "forgetaboutit"
                        labelRequest.CordialValue = greeting
                        If InStr(goappstate.LastSerialNbr, ":") <> 0 Then
                            ls = Mid(goappstate.LastSerialNbr, 1, InStr(goappstate.LastSerialNbr, ":") - 1)
                        Else
                            ls = goappstate.LastSerialNbr
                        End If
                        If BECSeries = "900" Then labelRequest.saveSerializedProducts(ls, goProperties.LineNumber, BundleSerializedList)

                    End If
                End If
            Else
                'wipe out all traces of the buffered label    
                goappstate.BufferedLabelCount = 0
                Erase goappstate.BufferedLabels
                goappstate.BufferedLabelPointer = -1

                'request the label
                Me.lblAction.Text = "Requesting Label -please wait..."
                Application.DoEvents()

                'Add the columns
                dt.Columns.Add("Label Type", GetType(System.String))
                dt.Columns.Add("Part Number", GetType(System.String))
                dt.Columns.Add("Customer Code", GetType(System.String))
                dt.Columns.Add("Machine Name", GetType(System.String))
                dt.Columns.Add("Quantity", GetType(System.String))
                dt.Columns.Add("Start Time", GetType(System.String))
                dt.Columns.Add("End Time", GetType(System.String))
                dt.Columns.Add("Operator ID", GetType(System.String))
                dt.Columns.Add("Response Queue", GetType(System.String))
                dt.Columns.Add("Box Count", GetType(System.Int64))
                dt.Columns.Add("Disposition", GetType(System.String))
                dt.Columns.Add("Component Identifiers", GetType(System.String))
                dt.Columns.Add("Destination", GetType(System.String))
                dt.Columns.Add("Printer", GetType(System.String))
                dt.Columns.Add("ECL", GetType(System.String))
                dt.Columns.Add("Package Code", GetType(System.String))
                dt.Columns.Add("ScheduleRequired", GetType(System.Boolean))
                dt.Columns.Add("Department", GetType(System.String))
                dt.Columns.Add("SerializedBECs", GetType(System.String))
                dt.Columns.Add("ProdRunId", GetType(System.Int64))


                'create a data row from the data table
                Dim dr As DataRow = dt.NewRow()

                ' populate the data row
                dr.Item("Label Type") = stdPackType
                dr.Item("Part Number") = goappstate.DelphiPartNumber
                dr.Item("Customer Code") = goappstate.CustomerCode
                dr.Item("Machine Name") = goProperties.MachineName
                dr.Item("Quantity") = goappstate.StdPack

                dr.Item("Start Time") = goappstate.StartTime
                'dr.Item("End Time") = Now
                dr.Item("Operator ID") = goProperties.LineNumber
                'dr["Response Queue"]     = printQRecv
                dr("Box Count") = goappstate.NumberOfBoxes
                dr.Item("Disposition") = labelType
                dr.Item("Component Identifiers") = goappstate.CurrentLotNbr
                If stdPackType = "MTMS Master" Then
                    dr.Item("Destination") = goappstate.Location
                End If
                'dr.Item("Printer") = printer
                dr.Item("ECL") = goappstate.ECL
                dr.Item("Package Code") = goappstate.PackageCode
                dr.Item("ScheduleRequired") = goappstate.ScheduleRequired
                dr.Item("Department") = conf.Appsettings("//Properties/Department")
                dr.Item("ProdRunID") = Convert.ToInt64(goappstate.ProductionRunNbr)

                If BECSeries = "900" And goappstate.PackageCode <> "4C" Then
                    'add serialized BECS to print request for associating with the standard pack
                    'for 800 or underhood cardboard, we don't append the serialized list because we don't have all the scanned serials when the label is requested
                    dr.Item("SerializedBECs") = BundleSerializedList()
                End If

                ' //create a message object
                ' add the row to the table
                dt.Rows.Add(dr)

                'The DataTable is not INHERENTLY serializable
                'we have to put it in a dataset to pass to the Q
                Dim ds As New DataSet

                ds.Tables.Add(dt)
requestlabel:
                label = labelRequest.PrintLabel_P23(ds)
                If IsNothing(label) Then
                    If MsgBox("Failure Getting Label. Web Server not properly configured.  Retry?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                        GoTo requestlabel
                    End If
                Else
                    If InStr(label, "</?BWSError>") <> 0 Then
                        'to handle the error
                        If MsgBox(Mid(label, 1, InStr(label, "</?BWSError>") - 1) & vbCrLf & "Do you wish to resubmit the label request?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then 'Extract the error from front of the return value
                            GoTo requestlabel
                        Else
                            label = Mid(label, InStr(label, "</?BWSError>") + 12) 'strip off the error stuff
                            'print the error label
                            Pdoc.PrinterSettings.PrinterName = "Generic"
                            If Pdoc.PrinterSettings.IsValid Then
                                p.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, label)

                                'DSM: save label just printed in reprintlabel global variable
                                reprintlabel = MakeReprintLabel(goappstate.BufferedLabels(goappstate.BufferedLabelPointer + 1))

                            End If
                        End If
                    Else
                        Try
                            'first we must replace any <?CharXXXX> placeholders with the real thing. This is because xml does not support control characters
                            If InStr(label, "</?Serial>") <> 0 Then
                                LastSerialNbr = Mid(label, 1, InStr(label, "</?Serial>") - 1) & ":" & Now.Now 'Extract the serial number form the front of the label file the form is the_serial_number</?Serial>
                                Me.lblLastSerial.Text = LastSerialNbr
                                goappstate.LastSerialNbr = LastSerialNbr
                                label = Mid(label, InStr(label, "</?Serial>") + 10) 'grab everything except the serial header
                                label = Replace(label, "<Char29>", "_1D") 'put the control character back in.
                                label = Replace(label, "<Char30>", "_1E") 'put the control character back in.
                                label = Replace(label, "<Char4>", "_04")

                                If InStr(label, "<<myEndofLabel>>") <> 0 Then

                                    'DSM: add new serialnumbers created in BecWebService for the 2nd/3rd/4th 4C labels 
                                    'to the newSerialNumbers array and remove from the label file
                                    Erase newSerialNumbers
                                    newSerialNumbers = Split(label, "<<newSN>>") 'Split new ser. numbers into newSerialNumbers array
                                    charposition = InStr(label, "<<newSN>>")
                                    'Store the new ser. numbers in global variables
                                    label = Mid(label, 1, charposition - 1) 'Remove added chars in label for new ser. numbers

                                    'multiple label scenerio
                                    Erase goappstate.BufferedLabels
                                    goappstate.BufferedLabels = Split(label, "<<myEndofLabel>>")
                                    goappstate.BufferedLabelCount = UBound(goappstate.BufferedLabels)


                                    'print the first label
                                    Pdoc.PrinterSettings.PrinterName = "Generic"
                                    goappstate.BufferedLabelPointer = 1 '0 contains the Master Label
                                    If Pdoc.PrinterSettings.IsValid Then
                                        'DSM:This is the 1st label for an Underhood, 4-C package. Calculate serial_nbr for SAP, only send 8 digits
                                        serial_nbr = Mid(LastSerialNbr, 1, 10)
                                        p.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, goappstate.BufferedLabels(goappstate.BufferedLabelPointer))

                                        'DSM: save label just printed in reprintlabel global variable
                                        reprintlabel = MakeReprintLabel(goappstate.BufferedLabels(goappstate.BufferedLabelPointer))

                                        NotifySAPofPrintedLabel(serial_nbr)
                                    End If
                                Else
                                    Pdoc.PrinterSettings.PrinterName = "Generic"
                                    LogMsg("Print & NotifySAPofPrintedLabel Pdoc.PrinterSettings.IsValid:", Pdoc.PrinterSettings.IsValid)

                                    If Pdoc.PrinterSettings.IsValid Then
                                        p.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, label)

                                        'DSM: save label just printed in reprintlabel global variable
                                        reprintlabel = MakeReprintLabel(label)


                                        ' DSM: the program printed the label - now notify SAP of full 10 digit ser. no.
                                        serial_nbr = Mid(LastSerialNbr, 1, 10)
                                        NotifySAPofPrintedLabel(serial_nbr)
                                        LogMsg("NotifySAPofPrintedLabel Done")

                                    End If
                                End If
                            Else
                                'the serial was not there, must be an error
                            End If

                            If goappstate.NumberPkgsPerSkid > 1 Then
                                goappstate.SkidPackageCount += 1
                            End If
                            refreshFormControls()
                        Catch ex As Exception

                        End Try
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox("Error Printing Label: " & ex.Message, MsgBoxStyle.ApplicationModal)
        Finally
            p = Nothing
            labelRequest.Dispose()
            dt.Dispose()
        End Try
    End Sub

    Private Function MakeReprintLabel(ByVal label As String) As String
        '6/4/08 David Maibor (DSM) Change Dept. value in label file to add '-R' at end, signifying the label is reprinted

        Dim dept As Integer
        Dim findstring As String
        Dim newstring
        Dim returnstring As String

        dept = goappstate.Department
        findstring = "FD" & dept
        newstring = findstring & "-R"

        returnstring = label.Replace(findstring, newstring)
        MakeReprintLabel = returnstring


    End Function
    '*****************************************************************************
    '* Function...: RemoveAllCarriageReturns
    '* Purpose....: Removes all carriage returns from a string
    '* Parameters.: sBuffer (String)
    '*                String to be parsed
    '* Returns....: Parsed string
    '*****************************************************************************
    Public Function RemoveAllCarriageReturns(ByVal sBuffer As String) As String
        Dim sTemp As String

        sTemp = Replace(sBuffer, vbCr, "")
        Return Replace(sTemp, vbLf, "")

    End Function ' RemoveAllCarriageReturns
    Private Sub CalculateLayerBox(ByVal CurScanned As Integer, ByVal numboxes As Integer, ByVal layersPerBox As Integer, ByVal piecesPerStandard As Integer, ByRef curLayer As Integer, ByRef curBox As Integer)
        'this procedure calculates layer and box

        Dim l(1, 1) As Integer
        Dim b() As Integer
        Dim i As Integer
        Dim c As Integer
        Dim ppb As Integer
        Dim ppl As Integer
        Dim tl As Integer
        Dim lay As Integer
        Dim box As Integer
        Dim j As Integer

        'get pieces per layer
        ppb = piecesPerStandard / numboxes
        ppl = ppb / layersPerBox
        tl = layersPerBox * numboxes

        ReDim b(numboxes)
        For i = 1 To numboxes
            b(i) = i * ppb
        Next i

        ReDim l(tl, 2)
        If goappstate.PackageCode = "4C" And BECSeries = "900" Then
            'this is special consideration 9 parts on first layer, 6 on the second layer of each box
            For i = 1 To tl
                If i Mod 2 = 0 Then
                    j = j + 6
                    l(i, 1) = j
                Else
                    j = j + 9
                    l(i, 1) = j
                End If
            Next i
        Else
            For i = 1 To tl
                l(i, 1) = i * ppl
            Next i
        End If

        c = 0
        For i = 1 To tl
            If c * ppl >= ppb Then
                c = 1
            Else
                c = c + 1
            End If
            l(i, 2) = c
        Next i

        For i = 1 To piecesPerStandard
            If CurScanned <= l(i, 1) Then
                curLayer = l(i, 2)
                Exit For
            End If
        Next i

        For i = 1 To piecesPerStandard
            If CurScanned <= b(i) Then
                curBox = i
                Exit For
            End If
        Next i

    End Sub
    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

    End Sub

    'Private Sub MSComm_OnComm(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Select Case MSComm.CommEvent
    '        Case 2
    '            Application.DoEvents()
    '            tmrCommScanner.Interval = 100
    '            tmrCommScanner.Enabled = True
    '    End Select

    'End Sub
    Private Sub DataReceivedHandler(sender As Object, e As SerialDataReceivedEventArgs)
        Dim sp As SerialPort = CType(sender, SerialPort)

        Try
            Stx = Stx & sp.ReadExisting()
            If Stx.Length > 0 Then
                File.AppendAllText("log.txt", "Success Read Scan DataReceivedHandler" + " " + Now + Environment.NewLine)
                CommScannerFunc(Stx)

            Else
                File.AppendAllText("log.txt", "Failure read scan DataReceivedHandler" + " " + Now + Environment.NewLine)
                MessageBox.Show("Failure Read Scanner")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

        'Application.DoEvents()
        'tmrCommScanner.Interval = 100
        'tmrCommScanner.Enabled = True

    End Sub
    Public Sub CommScannerFunc(ByVal Stx As String)

        'tmrCommScanner.Enabled = False
        'tmrCommScanner.Interval = 101
        'jerry  Stx = Stx & MSComm.Input
        'File.AppendAllText("log.txt", "tmrCommScanner Stx:" + Stx + " " + Now + Environment.NewLine)
        'Stx = Stx & SerialPort1.ReadExisting
        'Stx = Stx & SerialPort1.ReadLine 
        Try


            DisableScanner()
            Application.DoEvents()
            File.AppendAllText("log.txt", "CommScannerFunc Stx & MSComm.Input:" + Stx + " " + Now + Environment.NewLine)

            If common.ProcessingContainment Then
                'containment form is being used, allow them to scan the operator ID
                If Stx.EndsWith(vbCrLf) Then
                    Stx = Stx.Remove(Stx.Length - 2, 2)
                    common.ScannedForContainment = Stx
                    Stx = ""
                End If
            Else
                'MsgBox("Since Last Scan: " & DateDiff(DateInterval.Second, LastScan, Now.Date))
                If DateDiff(DateInterval.Second, LastScan, Now.Now) < DelayBetweenScans Then
                    '8/15/05 buzzer removed per their request PlayDaSound("Buzzer")
                    MsgBox("Scanned too soon. Must wait", MsgBoxStyle.ApplicationModal)
                    Stx = ""
                    GoTo exithere
                Else
                    LastScan = Now.Now
                    PlayDaSound("windows xp information bar")
                    lblLastScan.Text = Stx
                End If

                If Stx.EndsWith(vbCrLf) Then
                    Stx = Stx.Remove(Stx.Length - 2, 2).Replace("_", "").Replace(":", "")
                    'Replace("_", "").Replace(":", "")
                    File.AppendAllText("log.txt", "ProcessCommInput Stx Input: " + Stx + " " + Now + Environment.NewLine)
                    ProcessCommInput(Stx)
                    Stx = ""
                Else
                    'MsgBox("@@ vbCrLf is missing ", MsgBoxStyle.ApplicationModal)
                    File.AppendAllText("log.txt", "You need Carriage Return Setting" + " " + Now + Environment.NewLine)

                    Stx = Stx.Remove(Stx.Length - 2, 2).Replace("_", "").Replace(":", "")
                    File.AppendAllText("log.txt", "ProcessCommInput Stx Input: " + Stx + " " + Now + Environment.NewLine)
                    ProcessCommInput(Stx)
                End If
            End If
exithere:

            'tmrCommScanner.Interval = 100
            If cmdCorrectProblem.Visible = False And (optCardboard.Checked Or optReturnable.Checked) Then

                EnableScanner()

            End If
            Application.DoEvents()

        Catch ex As Exception
            MessageBox.Show(ex.ToString())

        End Try
    End Sub
    Private Sub ProcessCommInput(ByRef Stx As String)
        Dim curlayer As Integer
        Dim curbox As Integer
        Dim EnableFlag As Boolean
        Dim valid As Boolean
        Dim retv As String
        Dim ttt As String
        Dim ub As Integer
        Dim cString As String
        cString = Stx.Replace("_", "").Replace(":", "")
        Stx = cString.ToString()
        'Dim serial As String      'BPD 9/14/11
        'Dim StdPackID As String   'BPD 9/25/11
        'Dim result As New DataSet 'BPD 9/25/11
        'Dim x As Integer          'BPD 9/25/11
        'Dim BCode As String       'BPD 9/25/11

        Try
            'Check to make sure a package type has been chosen
            If optReturnable.Checked = False And optCardboard.Checked = False Then
                MsgBox("You must select a package type", MsgBoxStyle.ApplicationModal, "Select Package Code")
                GoTo exithere
            End If

            'Check to make sure a label type has been chosen
            If optMTMS.Checked = False And optICS.Checked = False Then
                MsgBox("You must select a Label Type", MsgBoxStyle.ApplicationModal, "Select Label Type")
                GoTo exithere
            End If

            '9/9/05  PAW added this code to prevent switching between bec 900 and 800 in the middle of the run
            If BECSeries = "900" Then
                If Stx.Length < 8 Then
                    MsgBox("Problem:  Currently working on Series 900.  This scan is invalid", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                    GoTo exithere
                End If
            ElseIf BECSeries = "800" Then
                If Stx.Length > 5 Then
                    MsgBox("Invalid scan: 800 Series scans are five characters long", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                    GoTo exithere
                End If
            End If

            Try
                'This code is for 900 series

                If BECSeries = "900" Then
                    '900 series
                    '1 determine if it has been scanned.....
                    Dim i As Int32
                    Dim found As Boolean

                    Try
                        ub = UBound(goappstate.Scans)
                    Catch
                        ub = -1
                    End Try

                    If ub > 0 Then
                        'jerry 
                        For i = 0 To ub

                            If goappstate.Scans(i) = Stx Then
                                If Not Me.chkScanOut.Checked Then
                                    MsgBox("This BEC has already been scanned into this container.", MsgBoxStyle.ApplicationModal)
                                    GoTo exithere
                                End If
                                found = True
                                Exit For
                            End If
                        Next

                        '2 remove 
                        If Me.chkScanOut.Checked Then
                            'Decrement the total parts scanned by one
                            If found Then
                                found = False 'recycle this variable
                                Try
                                    ub = UBound(goappstate.RemovedBECs)
                                Catch
                                    ub = -1
                                End Try
                                For i = 0 To ub
                                    If goappstate.RemovedBECs(i) = Stx Then
                                        'error, already removed, can't remove again
                                        found = True
                                        Exit For
                                    End If
                                Next i
                                If Not found Then
                                    If MsgBox("This will remove the BEC from the count.  Are you sure?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                                        'ok to remove, or in essence, add to the removedbecs array
                                        If ub < 0 Then ub = 0
                                        ReDim Preserve goappstate.RemovedBECs(ub + 1)
                                        goappstate.RemovedBECs(ub) = Stx
                                        goappstate.TotalPartsScanned -= 1
                                        Me.lblAction.Text = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                                        goappstate.NextAction = "Now Scan Part " & (goappstate.TotalPartsScanned + 1) & " of " & goappstate.StdPack
                                        MsgBox("BEC: " & Stx & "  has been removed from the count", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                                    End If
                                Else
                                    MsgBox("Can Not Remove this BEC because it has already been removed once", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                                End If
                            Else
                                'not found, has never been scanned, don't remove.  Note: Currently we leave them in the scans even if 
                                '                                                        we scan remove them.  They will be removed from scans when label is requested
                                '                                                        so, if its' not in scans, it has never been scanned and cannot be removed
                                MsgBox("Can not remove this BEC from the scan count --because it was not been previously counted.", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                            End If 'if found
                            chkScanOut.Checked = False
                            GoTo exithere
                        End If ' if remove checked
                    End If 'ub > 0
                End If 'BECSeries was 900

                If Not HandlePreviouslyScannedBECs(Stx) Then GoTo exithere 'Check for duplicates across cntrs - added by BPD 9/14/2011

            Catch ex As Exception
                MsgBox("Error in ProcessCommInput: " & ex.Message, MsgBoxStyle.ApplicationModal)
                Throw New Exception(ex.Message)

            End Try

            Select Case goappstate.NextActionType
                Case ApplicationState.ActionType.PROCESS_BROADCASTCODE
                    ttt = Me.lblAction.Text
                    Me.lblAction.Text = "Processing Broadcast Code...."
                    Me.Refresh()

                    If Me.optICS.Checked Then
                        Me.optMTMS.Enabled = False
                    ElseIf Me.optMTMS.Checked Then
                        Me.optICS.Enabled = False
                    End If
                    If Me.optReturnable.Checked Then
                        Me.optCardboard.Enabled = False
                    ElseIf Me.optCardboard.Checked Then
                        Me.optReturnable.Enabled = False
                    End If

                    If goappstate.NumberPkgsPerSkid > 1 And (goappstate.SkidPackageCount < goappstate.NumberPkgsPerSkid) Then
                        If Len(Stx) >= 10 Then
                            If goappstate.BroadcastCode <> Mid(Stx, 1, 8) Then
                                MsgBox("Invalid Scan.  Switching Part Numbers in Middle of a Skid not allowed", MsgBoxStyle.ApplicationModal)
                                Me.lblAction.Text = ttt
                                GoTo exithere
                            End If
                        Else
                            '800
                            If goappstate.BroadcastCode <> Stx Then
                                MsgBox("Invalid Scan.  Switching Part Numbers in Middle of a Skid not allowed", MsgBoxStyle.ApplicationModal)
                                Me.lblAction.Text = ttt
                                GoTo exithere
                            End If
                        End If
                        retv = ProcessBroadcastCode(Stx, True)
                    Else
                        retv = ProcessBroadcastCode(Stx, False)
                    End If
                    If retv = "Success" Then
                        If Plant = "23" Then
                            If goappstate.PackageCode = "Y" Then
                                Label7.Text = "Current Tray:"
                                Label8.Text = "Trays per Package:"
                            Else
                                Label7.Text = "Current Layer:"
                                Label8.Text = "Layers per Package:"
                            End If
                        End If

                        goappstate.IsLastBox = False
                        goappstate.InProgress = True
                        'goAppState.StartTime = Now
                        goappstate.TotalPartsScanned = 1
                        If LayerProps("CurrentLayer", goappstate.TotalPartsScanned, "") Then
                        End If
                        goappstate.NextAction = "Now Scan Part 2 of " & goappstate.StdPack.ToString
                        goappstate.NextActionType = ApplicationState.ActionType.PROCESS_SCANPARTS_COMMAND
                        lblAction.Text = goappstate.NextAction
                        Me.lblDelphiPart.Text = goappstate.DelphiPartNumber
                        Me.lblCustPartNbr.Text = goappstate.CustomerPartNumber
                        Me.lblECL.Text = goappstate.ECL
                        Me.txtCurrLayer.Text = 1
                        Me.txtTotalLayers.Text = goappstate.NumberOfLayersPerBox
                        Me.txtNumPackages.Text = goappstate.NumberOfBoxes
                        'lock option buttons not currently set                        
                        Return
                    Else
                        'MsgBox("Failure Processing BroadCast Code for: " & Stx & vbCrLf & "Verify that PC&L has loaded a production run for this Part/ECL")
                        If goappstate.NumberPkgsPerSkid > 1 And (goappstate.SkidPackageCount < goappstate.NumberPkgsPerSkid) Then
                            Reset(3)
                        Else
                            Reset(0)
                        End If
                    End If

                Case ApplicationState.ActionType.PROCESS_SCANPARTS_COMMAND
                    If BECSeries = "900" Then
                        If goappstate.BroadcastCode = Mid(Stx, 1, 8) Then
                            valid = True
                            'add to scanned serials list
                            Try
                                ub = UBound(goappstate.Scans)
                            Catch ex As Exception
                                ub = 0
                            End Try
                            ReDim Preserve goappstate.Scans(ub + 1)
                            ReDim Preserve goappstate.ScanTime(ub + 1)
                            goappstate.Scans(ub) = Stx
                            goappstate.ScanTime(ub) = Now.Now
                        Else
                            valid = False
                        End If
                    Else
                        If goappstate.BroadcastCode = Stx Then
                            valid = True
                        Else
                            valid = False
                        End If
                    End If

                    If valid Then
                        goappstate.TotalPartsScanned += 1
                        Me.txtCurrLayer.Text = LayerProps("CurrentLayer", goappstate.TotalPartsScanned, "")
                        If Me.txtCurrLayer.Text = "0" Then Me.txtCurrLayer.Text = ""
                        If goappstate.TotalPartsScanned = (goappstate.StdPack / 2) Then
                            '8/3/05 added this to blow horn when they scan past the halfway point
                            Me.lblAction.ForeColor = System.Drawing.Color.Red
                            PlayDaSound("CarHorn")
                        ElseIf goappstate.TotalPartsScanned > (goappstate.StdPack / 2) Then
                            Me.lblAction.ForeColor = System.Drawing.Color.Red
                        Else
                            Me.lblAction.ForeColor = System.Drawing.Color.ForestGreen
                        End If
                        ProcessLastScan(EnableFlag)
                        Me.lblAction.Text = goappstate.NextAction
                    Else
                        Throw New Exception("Invalid Part: " & Stx)
                    End If
            End Select
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
        Finally
            Me.Cursor.Current = Cursors.Default
        End Try
exithere:
        If (optCardboard.Checked Or optReturnable.Checked) Then
        End If
    End Sub
    Private Sub Label12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label12.Click

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '5/21/08 DSM: select DEVELOPMENT/PRODUCTION version as appropriate
        'Dim labelRequest As New localhostRequest.RequestLabel    'DSM: DEVELOPMENT version
        Dim labelRequest As New dorme2.RequestLabel    'DSM: PRODUCTION version

        Dim label As String
        Dim dt As New DataTable
        Dim p As New RawPrinterHelper
        Dim stdPackType As String = "Active"
        Dim labelType As String = "ICS"

        Try
            Me.lblAction.Text = "Requesting Label -please wait..."
            Application.DoEvents()
            'Add the columns
            dt.Columns.Add("Label Type", GetType(System.String))
            dt.Columns.Add("Part Number", GetType(System.String))
            dt.Columns.Add("Customer Code", GetType(System.String))
            dt.Columns.Add("Machine Name", GetType(System.String))
            dt.Columns.Add("Quantity", GetType(System.String))
            dt.Columns.Add("Start Time", GetType(System.String))
            dt.Columns.Add("End Time", GetType(System.String))
            dt.Columns.Add("Operator ID", GetType(System.String))
            dt.Columns.Add("Response Queue", GetType(System.String))
            dt.Columns.Add("Box Count", GetType(System.Int64))
            dt.Columns.Add("Disposition", GetType(System.String))
            dt.Columns.Add("Component Identifiers", GetType(System.String))
            dt.Columns.Add("Destination", GetType(System.String))
            dt.Columns.Add("Printer", GetType(System.String))
            dt.Columns.Add("ECL", GetType(System.String))
            dt.Columns.Add("Package Code", GetType(System.String))
            dt.Columns.Add("ScheduleRequired", GetType(System.Boolean))
            dt.Columns.Add("Department", GetType(System.String))

            'create a data row from the data table
            Dim dr As DataRow = dt.NewRow()

            ' populate the data row
            dr.Item("Label Type") = stdPackType
            dr.Item("Part Number") = goappstate.DelphiPartNumber
            dr.Item("Customer Code") = goappstate.CustomerCode
            dr.Item("Machine Name") = goProperties.MachineName
            dr.Item("Quantity") = goappstate.StdPack
            dr.Item("Start Time") = goappstate.StartTime
            'dr.Item("End Time") = Now
            dr.Item("Operator ID") = goProperties.LineNumber
            'dr["Response Queue"]     = printQRecv
            dr("Box Count") = goappstate.NumberOfBoxes
            dr.Item("Disposition") = labelType
            dr.Item("Component Identifiers") = goappstate.CurrentLotNbr
            If stdPackType = "MTMS Master" Then
                dr.Item("Destination") = goappstate.Location
            End If
            'dr.Item("Printer") = printer
            dr.Item("ECL") = goappstate.ECL
            dr.Item("Package Code") = goappstate.PackageCode
            dr.Item("ScheduleRequired") = goappstate.ScheduleRequired
            dr.Item("Department") = conf.Appsettings("//Properties/Department")

            '      //create a message object
            ' add the row to the table
            dt.Rows.Add(dr)


            'The DataTable is not INHERENTLY serializable
            'we have to put it in a dataset to pass to the Q
            Dim ds As New DataSet

            ds.Tables.Add(dt)
            'labelRequest.Credentials = System.Net.CredentialCache.DefaultCredentials
            label = labelRequest.PrintLabel_P23(ds)
            If IsNothing(label) Then
                MsgBox("Failure Getting Label. Web Server not properly configured.", MsgBoxStyle.ApplicationModal)
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub
    Private Sub Label13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label13.Click

    End Sub
    Private Sub lblLastScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblLastScan.Click

    End Sub
    Private Sub chkScanOut_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkScanOut.CheckedChanged
        If chkScanOut.Checked Then
            If goappstate.TotalPartsScanned > 1 Then
                'ok
            Else
                MsgBox("Not allowed for the first part in the package. To remove the first BEC, Clear and start over", MsgBoxStyle.ApplicationModal)
                chkScanOut.Checked = False
            End If
        End If
    End Sub
    Protected Overrides Sub Finalize()

        MyBase.Finalize()
    End Sub
    Private Sub frmMain_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Leave

    End Sub
    Private Sub txtSkidPackageCount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSkidPackageCount.TextChanged

    End Sub
    Private Sub lblCustPartNbr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblCustPartNbr.Click

    End Sub
    Private Sub Button1_Click_3(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        '5/21/08 DSM: select DEV/PRODUCTION version as appropriate
        'Dim labelRequest As New localhostRequest.RequestLabel    'DSM: DEVELOPMENT version
        Dim labelRequest As New dorme2.RequestLabel     'DSM: PRODUCTION version

        Dim label As String
        Dim dt As New DataTable
        Dim p As New RawPrinterHelper
        Dim stdPackType As String = "Active"
        Dim labelType As String = "ICS"
        Dim ub As Integer


        Try
            GoTo testContainment

            Me.lblAction.Text = "Requesting Label -please wait..."
            Application.DoEvents()
            'Add the columns
            dt.Columns.Add("Label Type", GetType(System.String))
            dt.Columns.Add("Part Number", GetType(System.String))
            dt.Columns.Add("Customer Code", GetType(System.String))
            dt.Columns.Add("Machine Name", GetType(System.String))
            dt.Columns.Add("Quantity", GetType(System.String))
            dt.Columns.Add("Start Time", GetType(System.String))
            dt.Columns.Add("End Time", GetType(System.String))
            dt.Columns.Add("Operator ID", GetType(System.String))
            dt.Columns.Add("Response Queue", GetType(System.String))
            dt.Columns.Add("Box Count", GetType(System.Int64))
            dt.Columns.Add("Disposition", GetType(System.String))
            dt.Columns.Add("Component Identifiers", GetType(System.String))
            dt.Columns.Add("Destination", GetType(System.String))
            dt.Columns.Add("Printer", GetType(System.String))
            dt.Columns.Add("ECL", GetType(System.String))
            dt.Columns.Add("Package Code", GetType(System.String))
            dt.Columns.Add("ScheduleRequired", GetType(System.Boolean))
            dt.Columns.Add("Department", GetType(System.String))
            dt.Columns.Add("SerializedBECs", GetType(System.String))
            dt.Columns.Add("ProdRunId", GetType(System.Int64))

            'create a data row from the data table
            Dim dr As DataRow = dt.NewRow()

            ' populate the data row
            dr.Item("Label Type") = stdPackType
            dr.Item("Part Number") = goappstate.DelphiPartNumber
            dr.Item("Customer Code") = goappstate.CustomerCode
            dr.Item("Machine Name") = goProperties.MachineName
            dr.Item("Quantity") = goappstate.StdPack
            dr.Item("Start Time") = goappstate.StartTime
            'dr.Item("End Time") = Now
            dr.Item("Operator ID") = goProperties.LineNumber
            'dr["Response Queue"]     = printQRecv
            dr("Box Count") = goappstate.NumberOfBoxes
            dr.Item("Disposition") = labelType
            dr.Item("Component Identifiers") = goappstate.CurrentLotNbr
            If stdPackType = "MTMS Master" Then
                dr.Item("Destination") = goappstate.Location
            End If
            'dr.Item("Printer") = printer
            dr.Item("ECL") = goappstate.ECL
            dr.Item("Package Code") = goappstate.PackageCode
            dr.Item("ScheduleRequired") = goappstate.ScheduleRequired
            dr.Item("Department") = conf.Appsettings("//Properties/Department")
            dr.Item("SerializedBECs") = ""
            dr.Item("ProdRunID") = 0

            '      //create a message object
            ' add the row to the table
            dt.Rows.Add(dr)


            'The DataTable is not INHERENTLY serializable
            'we have to put it in a dataset to pass to the Q
            Dim ds As New DataSet

            ds.Tables.Add(dt)
requestlabel:
            label = labelRequest.PrintLabel_P23(ds)
            If IsNothing(label) Then
                If MsgBox("Failure Getting Label. Web Server not properly configured.  Retry?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                    GoTo requestlabel
                End If
            Else
                If InStr(label, "</?BWSError>") <> 0 Then
                    'to handle the error
                    If MsgBox(Mid(label, 1, InStr(label, "</?BWSError>") - 1) & vbCrLf & "Do you wish to resubmit the label request?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then 'Extract the error from front of the return value
                        GoTo requestlabel
                    Else
                        label = Mid(label, InStr(label, "</?BWSError>") + 12) 'strip off the error stuff
                        'print the error label
                        Pdoc.PrinterSettings.PrinterName = "Generic"
                        If Pdoc.PrinterSettings.IsValid Then
                            RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, label)
                        End If
                    End If
                Else
                    Try
                        'first we must replace any <?CharXXXX> placeholders with the real thing. This is because xml does not support control characters
                        If InStr(label, "</?Serial>") <> 0 Then
                            LastSerialNbr = Mid(label, 1, InStr(label, "</?Serial>") - 1) & ":" & Now.Now 'Extract the serial number form the front of the label file the form is the_serial_number</?Serial>
                            Me.lblLastSerial.Text = LastSerialNbr
                            goappstate.LastSerialNbr = LastSerialNbr
                            label = Mid(label, InStr(label, "</?Serial>") + 10) 'grab everything except the serial header
                            label = Replace(label, "<Char29>", "_1D") 'put the control character back in.
                            label = Replace(label, "<Char30>", "_1E") 'put the control character back in.
                            label = Replace(label, "<Char4>", "_04")

                            If InStr(label, "<<myEndofLabel>>") <> 0 Then
                                'multiple label scenerio
                                Erase goappstate.BufferedLabels
                                goappstate.BufferedLabels = Split(label, "<<myEndofLabel>>")
                                Try
                                    ub = UBound(goappstate.BufferedLabels)
                                Catch ex As Exception
                                    ub = 0
                                End Try
                                goappstate.BufferedLabelCount = ub
                                'print the error label
                                Pdoc.PrinterSettings.PrinterName = "Generic"
                                goappstate.BufferedLabelPointer = 1 '0 contains the Master Label
                                If Pdoc.PrinterSettings.IsValid Then
                                    RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, goappstate.BufferedLabels(goappstate.BufferedLabelPointer))
                                End If
                            Else
                                Pdoc.PrinterSettings.PrinterName = "Generic"
                                If Pdoc.PrinterSettings.IsValid Then
                                    RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, label)
                                End If
                            End If
                        Else
                            'the serial was not there, must be an error
                        End If

                        If goappstate.NumberPkgsPerSkid > 1 Then
                            goappstate.SkidPackageCount += 1
                        End If
                        refreshFormControls()
                    Catch ex As Exception

                    End Try
                End If
            End If
            GoTo exithere 'skip test containment

testContainment:
            'if containement logging feature is enabled, then open the containment form
            Dim ep As String
            Dim sn As String
            ep = "opening containment log form"
            If conf.Appsettings("//ApplicationState/LogContainment") = "True" Then
                'LogContainment allows them to turn on this feature through the appconfig file
                Dim frm As New frmContainmentLog
                Dim ts As New StringBuilder

                ts.Append("GP-12,")
                ts.Append(conf.Appsettings("//Properties/LineNumber") & ",")
                ts.Append(goappstate.DelphiPartNumber & ",")
                ts.Append(goappstate.ECL & ",")
                sn = goappstate.LastSerialNbr
                If InStr(sn, ":") <> 0 Then
                    sn = Mid(sn, 1, InStr(sn, ":") - 1)
                Else
                    'sn does not contain timestamp
                End If

                ts.Append(sn & ",")
                ts.Append(goappstate.StdPack & ",")
                frm.CommandString = ts.ToString
                common.ProcessingContainment = True 'this is set so the scanner will work with the inspector ID

                EnableScanner()

                frm.ShowDialog()
            End If
exithere:
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub

    Private Function HandlePreviouslyScannedBECs(ByVal BCode As String) As Boolean
        'BPD: 9/14/11 Check if this broadcast code appears in other containers
        'BPD: select Dev or Production service
        'Dim dl As New localhostData.BECDATALOOKUP  'BPD: DEVELOPMENT version
        Dim dl As New dorme1.BECDATALOOKUP          'BPD: PRODUCTION version

        Dim serial As String      'BPD 9/14/11
        Dim StdPackID As String   'BPD 9/25/11
        Dim result As New DataSet 'BPD 9/25/11
        Dim x As Integer          'BPD 9/25/11

        serial = ""
        StdPackID = ""

        result = dl.FindBroadcastCodeInCntrs(BCode)

        'Check to see if anything was returned and set the variables to their values
        If result.Tables(0).Rows.Count > 0 Then

            For x = 0 To result.Tables(0).Rows.Count - 1

                'Found a serial and standard pack id
                serial = result.Tables(0).Rows(x).Item("SERIAL_NBR")
                StdPackID = result.Tables(0).Rows(x).Item("STANDARD_PACK_ID")

                MsgBox("This BEC (" & BCode & ") has already been scanned and linked to serial number " & serial & ".", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
                'Application locks and prompts for password to continue.

                Dim sPwInput As String  ' BPD 9/14/11
                Dim sPassword As String ' BPD 9/14/11

                sPwInput = ""
                sPassword = conf.Appsettings("//Properties/DuplicateFoundCode")

                Do
                    sPwInput = InputBox("Please enter the password.", "BEC " & BCode & " is already in " & serial & ".", , , )
                Loop While sPwInput <> sPassword

                Dim Success As Boolean
                Dim IsRepack As MsgBoxResult
                Dim RepackAll As MsgBoxResult

AskIsRepack:    IsRepack = MsgBox("Is this BEC being re-packed?", MsgBoxStyle.YesNo + MsgBoxStyle.Question)

                If IsRepack = MsgBoxResult.Yes Then
AskRepackAll:       RepackAll = MsgBox("Do you want to re-pack every BEC in the container with serial number " & serial & "?", MsgBoxStyle.YesNo + MsgBoxStyle.Question)
                    'Add each unique BEC ID in the standard pack to the repack table so a repack is allowed
                    If RepackAll = MsgBoxResult.Yes Then

                        Success = dl.FlagAllBECsInCntrOK2Relabel(StdPackID)

                        If Success Then
                            MsgBox("Please have serial number " & serial & " deleted in SAP.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
                            Return True
                        End If
                    ElseIf RepackAll = MsgBoxResult.No Then
                        'only re-pack this BEC

                        Success = dl.FlagBECOK2Relabel(BCode)

                        If Success Then
                            MsgBox("Please have serial number " & serial & " deleted in SAP.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
                            Return True
                        End If
                    Else
                        GoTo AskRepackAll
                    End If

                ElseIf IsRepack = MsgBoxResult.No Then 'not a repack.  This BEC shouldn't be here
                    MsgBox("This BEC has already been counted towards a container.  The container with serial number " & serial & " needs to be broken down and sorted.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Problem Encountered")
                    Return False
                Else
                    GoTo AskIsRepack
                End If

            Next

        Else
            serial = ""
            StdPackID = ""
            Return True
        End If 'if found

        dl.Dispose()

    End Function

    'This was used in a test for detecting the re-scanning of BECs that are already in a container
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim x As String
        Dim y As String
        Dim z As String
        Dim a As String
        Dim sn As String
        'Dim path As String = "br.txt"
        Dim scannedval As String
        Dim nStdPack As Integer = goappstate.StdPack
        'Dim strMachineName As String = conf.Appsettings("//Properties/MachineName")
        Dim i As Integer
        ' Dim fw As System.IO.StreamWriter
        'x = "2279821301266103231425"
        'x = "23231596010088I2064120"
        'test_SN.Text = x
        scannedval = test_SN.Text.ToString().Replace("_", "").Replace(":", "")
        test_SN.Text = scannedval
        z = Mid(test_SN.Text.ToString(), 17, 6)


        y = Convert.ToDecimal(z)

        For i = 0 To nStdPack
            y = y + 1
            a = Mid(test_SN.Text.ToString(), 1, 15) + y
            ProcessCommInput(a)
        Next


        sn = goappstate.LastSerialNbr
        If InStr(sn, ":") <> 0 Then
            sn = Mid(sn, 1, InStr(sn, ":") - 1)
        Else
            'sn does not contain timestamp
        End If

        File.AppendAllText("br.txt", sn + " " + Now + Environment.NewLine)



    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ProcessCommInput(test_SN.Text)
        File.AppendAllText("log.txt", test_SN.Text + " " + Now + Environment.NewLine)
    End Sub
End Class

