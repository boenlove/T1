Imports System.Drawing.Printing
Imports System.Data.OracleClient
Imports System.Configuration.ConfigurationSettings

Public Class ExceptionsHandler
    Inherits System.Windows.Forms.Form
    Private WithEvents Pdoc As New PrintDocument
    Public conf As New ConfigurationSettings
    Private PrinterID As String


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

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
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblPart As System.Windows.Forms.Label
    Friend WithEvents lblDestinationCode As System.Windows.Forms.Label
    Friend WithEvents lblLineID As System.Windows.Forms.Label
    Friend WithEvents lblSkidID As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtAdminCode As System.Windows.Forms.TextBox
    Friend WithEvents dgSerials As System.Windows.Forms.DataGrid
    Friend WithEvents lblLastSerialNumber As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents SerialCount As System.Windows.Forms.StatusBarPanel
    Friend WithEvents Printer As System.Windows.Forms.StatusBarPanel
    Friend WithEvents mnuUserFunctions As System.Windows.Forms.MenuItem
    Friend WithEvents mnuUserFuncSelectAll As System.Windows.Forms.MenuItem
    Friend WithEvents mnuUserFuncReprintLast As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminFunctions As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminUnlock As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminGotDloc As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminGotLabel As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminSendMessage As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminCritial As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminCriticalDeleteSelected As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminCriticalAbortSkid As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminCriticalSchedule As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminCriticalScheduleWhatsScheduled As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminCriticalScheduleDeleteScheduled As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminCriticalSelectPrinter As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminCriticalSelectPrinterOffline As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminCriticalSelectPrinterAttached As System.Windows.Forms.MenuItem
    Friend WithEvents dgSchedule As System.Windows.Forms.DataGrid
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem10 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem11 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem12 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem13 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem14 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem15 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem16 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem17 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem18 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem19 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem20 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem21 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem22 As System.Windows.Forms.MenuItem
    Friend WithEvents cnCurrentSkid As System.Data.OleDb.OleDbConnection
    Friend WithEvents Timer1 As System.Timers.Timer
    Friend WithEvents TimeClick As System.Windows.Forms.StatusBarPanel
    Friend WithEvents cntxSkid As System.Windows.Forms.ContextMenu
    Friend WithEvents cntxSchedule As System.Windows.Forms.ContextMenu
    Friend WithEvents cntxMnuScheduleHide As System.Windows.Forms.MenuItem
    Friend WithEvents cntxMnuDeleteSelectedSchedule As System.Windows.Forms.MenuItem
    Friend WithEvents cntxMnuDeleteFromSkid As System.Windows.Forms.MenuItem
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblUsesSkid As System.Windows.Forms.Label
    Friend WithEvents lblPlant As System.Windows.Forms.Label
    Friend WithEvents cntxMnuDeleteALLwithMatchingParams As System.Windows.Forms.MenuItem
    Friend WithEvents cntxMnuDeleteSchedulethisLineOnly As System.Windows.Forms.MenuItem
    Friend WithEvents lblMachID As System.Windows.Forms.Label
    Friend WithEvents mnuAdminCriticalDeleteScheduledforThisLine As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAdminCriticalScheduleDeleteALLMatchingSchedule As System.Windows.Forms.MenuItem
    Friend WithEvents cntxMnuReprintSelected As System.Windows.Forms.MenuItem
    Friend WithEvents mnuUserFunctReprintSkidSummary As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem23 As System.Windows.Forms.MenuItem
    Friend WithEvents miUnderhood As System.Windows.Forms.MenuItem
    Friend WithEvents miMid As System.Windows.Forms.MenuItem
    Friend WithEvents miLeft As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblPart = New System.Windows.Forms.Label
        Me.lblDestinationCode = New System.Windows.Forms.Label
        Me.lblLineID = New System.Windows.Forms.Label
        Me.lblSkidID = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtAdminCode = New System.Windows.Forms.TextBox
        Me.dgSerials = New System.Windows.Forms.DataGrid
        Me.cntxSkid = New System.Windows.Forms.ContextMenu
        Me.cntxMnuDeleteFromSkid = New System.Windows.Forms.MenuItem
        Me.cntxMnuReprintSelected = New System.Windows.Forms.MenuItem
        Me.lblLastSerialNumber = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuUserFunctions = New System.Windows.Forms.MenuItem
        Me.mnuUserFuncSelectAll = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.mnuUserFuncReprintLast = New System.Windows.Forms.MenuItem
        Me.mnuUserFunctReprintSkidSummary = New System.Windows.Forms.MenuItem
        Me.mnuAdminFunctions = New System.Windows.Forms.MenuItem
        Me.mnuAdminUnlock = New System.Windows.Forms.MenuItem
        Me.mnuAdminGotDloc = New System.Windows.Forms.MenuItem
        Me.mnuAdminGotLabel = New System.Windows.Forms.MenuItem
        Me.mnuAdminSendMessage = New System.Windows.Forms.MenuItem
        Me.MenuItem22 = New System.Windows.Forms.MenuItem
        Me.mnuAdminCritial = New System.Windows.Forms.MenuItem
        Me.mnuAdminCriticalDeleteSelected = New System.Windows.Forms.MenuItem
        Me.mnuAdminCriticalAbortSkid = New System.Windows.Forms.MenuItem
        Me.MenuItem20 = New System.Windows.Forms.MenuItem
        Me.mnuAdminCriticalSchedule = New System.Windows.Forms.MenuItem
        Me.mnuAdminCriticalScheduleWhatsScheduled = New System.Windows.Forms.MenuItem
        Me.mnuAdminCriticalScheduleDeleteScheduled = New System.Windows.Forms.MenuItem
        Me.mnuAdminCriticalDeleteScheduledforThisLine = New System.Windows.Forms.MenuItem
        Me.mnuAdminCriticalScheduleDeleteALLMatchingSchedule = New System.Windows.Forms.MenuItem
        Me.MenuItem21 = New System.Windows.Forms.MenuItem
        Me.mnuAdminCriticalSelectPrinter = New System.Windows.Forms.MenuItem
        Me.mnuAdminCriticalSelectPrinterOffline = New System.Windows.Forms.MenuItem
        Me.mnuAdminCriticalSelectPrinterAttached = New System.Windows.Forms.MenuItem
        Me.MenuItem23 = New System.Windows.Forms.MenuItem
        Me.miUnderhood = New System.Windows.Forms.MenuItem
        Me.miMid = New System.Windows.Forms.MenuItem
        Me.miLeft = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.MenuItem10 = New System.Windows.Forms.MenuItem
        Me.MenuItem11 = New System.Windows.Forms.MenuItem
        Me.MenuItem12 = New System.Windows.Forms.MenuItem
        Me.MenuItem13 = New System.Windows.Forms.MenuItem
        Me.MenuItem14 = New System.Windows.Forms.MenuItem
        Me.MenuItem15 = New System.Windows.Forms.MenuItem
        Me.MenuItem16 = New System.Windows.Forms.MenuItem
        Me.MenuItem17 = New System.Windows.Forms.MenuItem
        Me.MenuItem18 = New System.Windows.Forms.MenuItem
        Me.MenuItem19 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.TimeClick = New System.Windows.Forms.StatusBarPanel
        Me.SerialCount = New System.Windows.Forms.StatusBarPanel
        Me.Printer = New System.Windows.Forms.StatusBarPanel
        Me.dgSchedule = New System.Windows.Forms.DataGrid
        Me.cntxSchedule = New System.Windows.Forms.ContextMenu
        Me.cntxMnuDeleteSelectedSchedule = New System.Windows.Forms.MenuItem
        Me.cntxMnuDeleteSchedulethisLineOnly = New System.Windows.Forms.MenuItem
        Me.cntxMnuDeleteALLwithMatchingParams = New System.Windows.Forms.MenuItem
        Me.cntxMnuScheduleHide = New System.Windows.Forms.MenuItem
        Me.cnCurrentSkid = New System.Data.OleDb.OleDbConnection
        Me.Timer1 = New System.Timers.Timer
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblUsesSkid = New System.Windows.Forms.Label
        Me.lblPlant = New System.Windows.Forms.Label
        Me.lblMachID = New System.Windows.Forms.Label
        CType(Me.dgSerials, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TimeClick, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SerialCount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Printer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgSchedule, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Timer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'label1
        '
        Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label1.Location = New System.Drawing.Point(16, 8)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(96, 16)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Delphi Part"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(112, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Destination Code"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(208, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Line ID"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(296, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 16)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Skid ID"
        '
        'lblPart
        '
        Me.lblPart.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblPart.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPart.Location = New System.Drawing.Point(16, 32)
        Me.lblPart.Name = "lblPart"
        Me.lblPart.Size = New System.Drawing.Size(80, 24)
        Me.lblPart.TabIndex = 4
        Me.lblPart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDestinationCode
        '
        Me.lblDestinationCode.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblDestinationCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestinationCode.Location = New System.Drawing.Point(112, 32)
        Me.lblDestinationCode.Name = "lblDestinationCode"
        Me.lblDestinationCode.Size = New System.Drawing.Size(80, 24)
        Me.lblDestinationCode.TabIndex = 5
        Me.lblDestinationCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblLineID
        '
        Me.lblLineID.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblLineID.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLineID.Location = New System.Drawing.Point(208, 32)
        Me.lblLineID.Name = "lblLineID"
        Me.lblLineID.Size = New System.Drawing.Size(80, 24)
        Me.lblLineID.TabIndex = 6
        Me.lblLineID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSkidID
        '
        Me.lblSkidID.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblSkidID.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSkidID.Location = New System.Drawing.Point(296, 32)
        Me.lblSkidID.Name = "lblSkidID"
        Me.lblSkidID.Size = New System.Drawing.Size(80, 24)
        Me.lblSkidID.TabIndex = 7
        Me.lblSkidID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(528, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 16)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Admin Code"
        '
        'txtAdminCode
        '
        Me.txtAdminCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdminCode.Location = New System.Drawing.Point(536, 24)
        Me.txtAdminCode.Name = "txtAdminCode"
        Me.txtAdminCode.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.txtAdminCode.Size = New System.Drawing.Size(80, 24)
        Me.txtAdminCode.TabIndex = 5
        Me.txtAdminCode.Text = ""
        '
        'dgSerials
        '
        Me.dgSerials.CaptionText = "Current skid..."
        Me.dgSerials.ContextMenu = Me.cntxSkid
        Me.dgSerials.DataMember = ""
        Me.dgSerials.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgSerials.Location = New System.Drawing.Point(0, 64)
        Me.dgSerials.Name = "dgSerials"
        Me.dgSerials.ReadOnly = True
        Me.dgSerials.Size = New System.Drawing.Size(328, 256)
        Me.dgSerials.TabIndex = 12
        Me.dgSerials.Visible = False
        '
        'cntxSkid
        '
        Me.cntxSkid.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cntxMnuDeleteFromSkid, Me.cntxMnuReprintSelected})
        '
        'cntxMnuDeleteFromSkid
        '
        Me.cntxMnuDeleteFromSkid.Index = 0
        Me.cntxMnuDeleteFromSkid.Text = "Delete selected Serials from Skid"
        Me.cntxMnuDeleteFromSkid.Visible = False
        '
        'cntxMnuReprintSelected
        '
        Me.cntxMnuReprintSelected.Index = 1
        Me.cntxMnuReprintSelected.Text = "Reprint Selected only"
        '
        'lblLastSerialNumber
        '
        Me.lblLastSerialNumber.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblLastSerialNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastSerialNumber.Location = New System.Drawing.Point(400, 32)
        Me.lblLastSerialNumber.Name = "lblLastSerialNumber"
        Me.lblLastSerialNumber.Size = New System.Drawing.Size(120, 24)
        Me.lblLastSerialNumber.TabIndex = 29
        Me.lblLastSerialNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(416, 8)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(96, 16)
        Me.Label10.TabIndex = 28
        Me.Label10.Text = "Last Serial#"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuUserFunctions, Me.mnuAdminFunctions, Me.MenuItem2, Me.MenuItem3})
        '
        'mnuUserFunctions
        '
        Me.mnuUserFunctions.Index = 0
        Me.mnuUserFunctions.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuUserFuncSelectAll, Me.MenuItem4})
        Me.mnuUserFunctions.Text = "User Functions"
        '
        'mnuUserFuncSelectAll
        '
        Me.mnuUserFuncSelectAll.Index = 0
        Me.mnuUserFuncSelectAll.Text = "Select all on skid"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 1
        Me.MenuItem4.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuUserFuncReprintLast, Me.mnuUserFunctReprintSkidSummary})
        Me.MenuItem4.Text = "Reprint"
        '
        'mnuUserFuncReprintLast
        '
        Me.mnuUserFuncReprintLast.Index = 0
        Me.mnuUserFuncReprintLast.Text = "Reprint last"
        '
        'mnuUserFunctReprintSkidSummary
        '
        Me.mnuUserFunctReprintSkidSummary.Index = 1
        Me.mnuUserFunctReprintSkidSummary.Text = "Reprint Skid Summary"
        '
        'mnuAdminFunctions
        '
        Me.mnuAdminFunctions.Index = 1
        Me.mnuAdminFunctions.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAdminUnlock, Me.mnuAdminGotDloc, Me.mnuAdminGotLabel, Me.mnuAdminSendMessage, Me.MenuItem22, Me.mnuAdminCritial})
        Me.mnuAdminFunctions.Text = "Admin Functions"
        '
        'mnuAdminUnlock
        '
        Me.mnuAdminUnlock.Index = 0
        Me.mnuAdminUnlock.Text = "Admin Unlock"
        '
        'mnuAdminGotDloc
        '
        Me.mnuAdminGotDloc.Enabled = False
        Me.mnuAdminGotDloc.Index = 1
        Me.mnuAdminGotDloc.Text = "Got DLOC?"
        '
        'mnuAdminGotLabel
        '
        Me.mnuAdminGotLabel.Enabled = False
        Me.mnuAdminGotLabel.Index = 2
        Me.mnuAdminGotLabel.Text = "Got Labels?"
        '
        'mnuAdminSendMessage
        '
        Me.mnuAdminSendMessage.Enabled = False
        Me.mnuAdminSendMessage.Index = 3
        Me.mnuAdminSendMessage.Text = "Send Message to PC&L"
        '
        'MenuItem22
        '
        Me.MenuItem22.Index = 4
        Me.MenuItem22.Text = "-"
        '
        'mnuAdminCritial
        '
        Me.mnuAdminCritial.Enabled = False
        Me.mnuAdminCritial.Index = 5
        Me.mnuAdminCritial.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAdminCriticalDeleteSelected, Me.mnuAdminCriticalAbortSkid, Me.MenuItem20, Me.mnuAdminCriticalSchedule, Me.MenuItem21, Me.mnuAdminCriticalSelectPrinter, Me.MenuItem23})
        Me.mnuAdminCritial.Text = "Critical Functions"
        '
        'mnuAdminCriticalDeleteSelected
        '
        Me.mnuAdminCriticalDeleteSelected.Index = 0
        Me.mnuAdminCriticalDeleteSelected.Text = "Delete selected serial numbers from SKID"
        Me.mnuAdminCriticalDeleteSelected.Visible = False
        '
        'mnuAdminCriticalAbortSkid
        '
        Me.mnuAdminCriticalAbortSkid.Index = 1
        Me.mnuAdminCriticalAbortSkid.Text = "Abort skid"
        '
        'MenuItem20
        '
        Me.MenuItem20.Index = 2
        Me.MenuItem20.Text = "-"
        '
        'mnuAdminCriticalSchedule
        '
        Me.mnuAdminCriticalSchedule.Index = 3
        Me.mnuAdminCriticalSchedule.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAdminCriticalScheduleWhatsScheduled, Me.mnuAdminCriticalScheduleDeleteScheduled})
        Me.mnuAdminCriticalSchedule.Text = "Schedule"
        '
        'mnuAdminCriticalScheduleWhatsScheduled
        '
        Me.mnuAdminCriticalScheduleWhatsScheduled.Index = 0
        Me.mnuAdminCriticalScheduleWhatsScheduled.Text = "Show what's scheduled"
        '
        'mnuAdminCriticalScheduleDeleteScheduled
        '
        Me.mnuAdminCriticalScheduleDeleteScheduled.Enabled = False
        Me.mnuAdminCriticalScheduleDeleteScheduled.Index = 1
        Me.mnuAdminCriticalScheduleDeleteScheduled.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAdminCriticalDeleteScheduledforThisLine, Me.mnuAdminCriticalScheduleDeleteALLMatchingSchedule})
        Me.mnuAdminCriticalScheduleDeleteScheduled.Text = "Delete Schedule entries"
        '
        'mnuAdminCriticalDeleteScheduledforThisLine
        '
        Me.mnuAdminCriticalDeleteScheduledforThisLine.Index = 0
        Me.mnuAdminCriticalDeleteScheduledforThisLine.Text = "Delete Scheduled for this line only"
        '
        'mnuAdminCriticalScheduleDeleteALLMatchingSchedule
        '
        Me.mnuAdminCriticalScheduleDeleteALLMatchingSchedule.Index = 1
        Me.mnuAdminCriticalScheduleDeleteALLMatchingSchedule.Text = "Delete ALL Scheduled with matching parameters"
        '
        'MenuItem21
        '
        Me.MenuItem21.Index = 4
        Me.MenuItem21.Text = "-"
        '
        'mnuAdminCriticalSelectPrinter
        '
        Me.mnuAdminCriticalSelectPrinter.Index = 5
        Me.mnuAdminCriticalSelectPrinter.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAdminCriticalSelectPrinterOffline, Me.mnuAdminCriticalSelectPrinterAttached})
        Me.mnuAdminCriticalSelectPrinter.Text = "Select Printer"
        '
        'mnuAdminCriticalSelectPrinterOffline
        '
        Me.mnuAdminCriticalSelectPrinterOffline.Index = 0
        Me.mnuAdminCriticalSelectPrinterOffline.RadioCheck = True
        Me.mnuAdminCriticalSelectPrinterOffline.Text = "Off-line printer"
        '
        'mnuAdminCriticalSelectPrinterAttached
        '
        Me.mnuAdminCriticalSelectPrinterAttached.Index = 1
        Me.mnuAdminCriticalSelectPrinterAttached.RadioCheck = True
        Me.mnuAdminCriticalSelectPrinterAttached.Text = "Attached printer"
        '
        'MenuItem23
        '
        Me.MenuItem23.Index = 6
        Me.MenuItem23.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.miUnderhood, Me.miMid, Me.miLeft})
        Me.MenuItem23.Text = "Change Department"
        '
        'miUnderhood
        '
        Me.miUnderhood.Index = 0
        Me.miUnderhood.Text = "Underhood"
        '
        'miMid
        '
        Me.miMid.Index = 1
        Me.miMid.Text = "Mid"
        '
        'miLeft
        '
        Me.miLeft.Index = 2
        Me.miLeft.Text = "Left"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 2
        Me.MenuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem5})
        Me.MenuItem2.Text = "Help"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem6, Me.MenuItem7, Me.MenuItem8})
        Me.MenuItem1.Text = "User Functions"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 0
        Me.MenuItem6.Text = "Select all on Skid"
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 1
        Me.MenuItem7.Text = "Reprint last"
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 2
        Me.MenuItem8.Text = "Reprint selected only"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 1
        Me.MenuItem5.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem9, Me.MenuItem10, Me.MenuItem11, Me.MenuItem12, Me.MenuItem13})
        Me.MenuItem5.Text = "Admin Functions"
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 0
        Me.MenuItem9.Text = "Admin Unlock"
        '
        'MenuItem10
        '
        Me.MenuItem10.Index = 1
        Me.MenuItem10.Text = "Got DLOC"
        '
        'MenuItem11
        '
        Me.MenuItem11.Index = 2
        Me.MenuItem11.Text = "Got Labels"
        '
        'MenuItem12
        '
        Me.MenuItem12.Index = 3
        Me.MenuItem12.Text = "Send message to PC&L"
        '
        'MenuItem13
        '
        Me.MenuItem13.Index = 4
        Me.MenuItem13.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem14, Me.MenuItem15, Me.MenuItem16, Me.MenuItem19})
        Me.MenuItem13.Text = "Critical Functions"
        '
        'MenuItem14
        '
        Me.MenuItem14.Index = 0
        Me.MenuItem14.Text = "Delete serial from skid"
        '
        'MenuItem15
        '
        Me.MenuItem15.Index = 1
        Me.MenuItem15.Text = "Abort skid"
        '
        'MenuItem16
        '
        Me.MenuItem16.Index = 2
        Me.MenuItem16.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem17, Me.MenuItem18})
        Me.MenuItem16.Text = "Schedule"
        '
        'MenuItem17
        '
        Me.MenuItem17.Index = 0
        Me.MenuItem17.Text = "Show what's scheduled"
        '
        'MenuItem18
        '
        Me.MenuItem18.Index = 1
        Me.MenuItem18.Text = "Delete Scheduled entries"
        '
        'MenuItem19
        '
        Me.MenuItem19.Index = 3
        Me.MenuItem19.Text = "Select Printer"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 3
        Me.MenuItem3.Text = "Close"
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 336)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.TimeClick, Me.SerialCount, Me.Printer})
        Me.StatusBar1.ShowPanels = True
        Me.StatusBar1.Size = New System.Drawing.Size(720, 22)
        Me.StatusBar1.TabIndex = 33
        Me.StatusBar1.Text = "StatusBar1"
        '
        'TimeClick
        '
        Me.TimeClick.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.TimeClick.Text = "StatusBarPanel1"
        Me.TimeClick.Width = 99
        '
        'SerialCount
        '
        Me.SerialCount.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.SerialCount.Text = "8"
        Me.SerialCount.Width = 20
        '
        'Printer
        '
        Me.Printer.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.Printer.Text = "Local"
        Me.Printer.Width = 41
        '
        'dgSchedule
        '
        Me.dgSchedule.CaptionText = "Current schedule..."
        Me.dgSchedule.ContextMenu = Me.cntxSchedule
        Me.dgSchedule.DataMember = ""
        Me.dgSchedule.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgSchedule.Location = New System.Drawing.Point(328, 64)
        Me.dgSchedule.Name = "dgSchedule"
        Me.dgSchedule.ReadOnly = True
        Me.dgSchedule.Size = New System.Drawing.Size(384, 256)
        Me.dgSchedule.TabIndex = 35
        Me.dgSchedule.Visible = False
        '
        'cntxSchedule
        '
        Me.cntxSchedule.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cntxMnuDeleteSelectedSchedule, Me.cntxMnuScheduleHide})
        '
        'cntxMnuDeleteSelectedSchedule
        '
        Me.cntxMnuDeleteSelectedSchedule.Index = 0
        Me.cntxMnuDeleteSelectedSchedule.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.cntxMnuDeleteSchedulethisLineOnly, Me.cntxMnuDeleteALLwithMatchingParams})
        Me.cntxMnuDeleteSelectedSchedule.Text = "Delete Schedule entries "
        '
        'cntxMnuDeleteSchedulethisLineOnly
        '
        Me.cntxMnuDeleteSchedulethisLineOnly.Index = 0
        Me.cntxMnuDeleteSchedulethisLineOnly.Text = "Delete Scheduled for this line only"
        '
        'cntxMnuDeleteALLwithMatchingParams
        '
        Me.cntxMnuDeleteALLwithMatchingParams.Index = 1
        Me.cntxMnuDeleteALLwithMatchingParams.Text = "Delete ALL Scheduled with matching parameters"
        '
        'cntxMnuScheduleHide
        '
        Me.cntxMnuScheduleHide.Index = 1
        Me.cntxMnuScheduleHide.Text = "Hide Schedule "
        '
        'cnCurrentSkid
        '
        Me.cnCurrentSkid.ConnectionString = "Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Registry Path=;Jet OLEDB:Database L" & _
        "ocking Mode=1;Data Source=""C:\Thanas\mfg\Labeling\ICS\BECSkidTracker.mdb"";Mode=S" & _
        "hare Deny None;Jet OLEDB:Engine Type=5;Provider=""Microsoft.Jet.OLEDB.4.0"";Jet OL" & _
        "EDB:System database=;Jet OLEDB:SFP=False;persist security info=False;Extended Pr" & _
        "operties=;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:Encrypt Datab" & _
        "ase=False;Jet OLEDB:Create System Database=False;Jet OLEDB:Don't Copy Locale on " & _
        "Compact=False;User ID=Admin;Jet OLEDB:Global Bulk Transactions=1"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        Me.Timer1.SynchronizingObject = Me
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(16, 64)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(160, 16)
        Me.Label5.TabIndex = 36
        Me.Label5.Text = "These Controls not visible at runtime"
        Me.Label5.Visible = False
        '
        'lblUsesSkid
        '
        Me.lblUsesSkid.Location = New System.Drawing.Point(24, 80)
        Me.lblUsesSkid.Name = "lblUsesSkid"
        Me.lblUsesSkid.Size = New System.Drawing.Size(136, 16)
        Me.lblUsesSkid.TabIndex = 37
        Me.lblUsesSkid.Text = "NoSkidRequired"
        Me.lblUsesSkid.Visible = False
        '
        'lblPlant
        '
        Me.lblPlant.Location = New System.Drawing.Point(16, 96)
        Me.lblPlant.Name = "lblPlant"
        Me.lblPlant.Size = New System.Drawing.Size(136, 16)
        Me.lblPlant.TabIndex = 38
        Me.lblPlant.Text = "Plant"
        Me.lblPlant.Visible = False
        '
        'lblMachID
        '
        Me.lblMachID.Location = New System.Drawing.Point(16, 112)
        Me.lblMachID.Name = "lblMachID"
        Me.lblMachID.Size = New System.Drawing.Size(136, 16)
        Me.lblMachID.TabIndex = 39
        Me.lblMachID.Text = "MachId"
        Me.lblMachID.Visible = False
        '
        'ExceptionsHandler
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(720, 358)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblMachID)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.lblLastSerialNumber)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.dgSerials)
        Me.Controls.Add(Me.txtAdminCode)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblSkidID)
        Me.Controls.Add(Me.lblLineID)
        Me.Controls.Add(Me.lblDestinationCode)
        Me.Controls.Add(Me.lblPart)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.dgSchedule)
        Me.Controls.Add(Me.lblUsesSkid)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblPlant)
        Me.MaximizeBox = False
        Me.Menu = Me.MainMenu1
        Me.MinimizeBox = False
        Me.Name = "ExceptionsHandler"
        Me.Text = "ExceptionsHandler"
        CType(Me.dgSerials, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TimeClick, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SerialCount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Printer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgSchedule, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Timer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnAdmin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub DataView1_ListChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ListChangedEventArgs)

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        runningExceptions = 2
        Me.Close()
    End Sub

    Private Sub dgSerials_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles dgSerials.Navigate

    End Sub

    Private Sub txtAdminCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAdminCode.TextChanged

    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Public WriteOnly Property Part()
        Set(ByVal Value)
            lblPart.Text = CType(Value, String)
        End Set
    End Property
    Public WriteOnly Property LineID()
        Set(ByVal Value)
            lblLineID.Text = CType(Value, String)
        End Set
    End Property
    Public WriteOnly Property SkidId()
        Set(ByVal Value)
            lblSkidID.Text = CType(Value, String)
        End Set
    End Property
    Public WriteOnly Property DestinationCode()
        Set(ByVal Value)
            lblDestinationCode.Text = CType(Value, String)
        End Set
    End Property
    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

    End Sub
    Private Sub lblSkidID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblSkidID.Click

    End Sub

    Private Sub btnReprintLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)


    End Sub

    Private Sub btnPrintSkidSumm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub mnuUserFunctReprintSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxMnuReprintSelected.Click

        'David Maibor (DSM) select DEV/PRODUCTION as appropriate
        'Dim reprintReq As New localhostRequest.RequestLabel 'DSM: DEV. version
        'Dim Greeting As New localhostRequest.Cordial    'DSM: DEV. version
        Dim reprintReq As New dorme2.RequestLabel   'DSM: PRODUCTION version
        Dim Greeting As New dorme2.Cordial          'DSM: PRODUCTION version

        Dim serialnbr() As String
        Dim p As New RawPrinterHelper
        Dim label As String
        Dim i, j As Integer
        Dim wl As Integer


        Try
            If goappstate.BufferedLabelCount > 0 Then
                'if there are buffered labels, reprint them from the buffer
                'print this label and increment the pointer
whichLabel:
                Try
                    wl = InputBox("Enter Number of label to reprint (enter 0 for Master Label, 1 for first, 2 for second, and so on", "Which Label to reprint")
                Catch
                    GoTo exithere
                End Try

                If wl < 0 Or wl > goappstate.BufferedLabelCount - 1 Then
                    If MsgBox("Must Enter Number between Zero and " & goappstate.BufferedLabelCount - 1 & vbCrLf & "Try Again?", MsgBoxStyle.Question Or MsgBoxStyle.YesNoCancel Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                        GoTo whichLabel
                    Else
                        GoTo exithere
                    End If
                End If
                Pdoc.PrinterSettings.PrinterName = "Generic"
                If Pdoc.PrinterSettings.IsValid Then
                    RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, goappstate.BufferedLabels(wl))
                    'If wl = (goappstate.BufferedLabelCount - 1) Then
                    '    'print the master label
                    '    RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, goappstate.BufferedLabels(0))
                    'End If
                End If
            Else
                'If lblLastSerial.Text.Length > 8 Then
                If dgSerials.VisibleRowCount > 0 Then
                    For i = 0 To dgSerials.VisibleRowCount - 1
                        If dgSerials.IsSelected(i) Then
                            ReDim Preserve serialnbr(j + 1)
                            j = j + 1
                            serialnbr(j) = dgSerials.Item(i, 2)
                        End If
                    Next
                    If j = 0 Then
                        'nothing is selected
                        MsgBox("Nothing Selected for Reprint", MsgBoxStyle.ApplicationModal)
                        Exit Sub
                    End If
                    Greeting.Password = "forgetaboutit"
                    Greeting.UserID = "delphikiosk"
                    reprintReq.CordialValue = Greeting
                    label = reprintReq.ReprintICS(serialnbr)

                    If InStr(label, "</?BWSError>") <> 0 Then
                        'to handle the error
                        MsgBox(Mid(label, 1, InStr(label, "</?BWSError>") - 1), MsgBoxStyle.ApplicationModal) 'Extract the error from front of the return value
                        label = Mid(label, InStr(label, "</?BWSError>") + 12) 'strip off the error stuff
                        'print the error label
                        If InStr(StatusBar1.Panels(2).Text, "Local Attached") <> 0 Then
                            Pdoc.PrinterSettings.PrinterName = "Generic"
                        Else
                            Pdoc.PrinterSettings.PrinterName = Trim(Mid(StatusBar1.Panels(2).Text, 9))
                        End If
                        If Pdoc.PrinterSettings.IsValid Then
                            RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, label)
                        End If
                    Else
                        Try
                            'first we must replace any <?CharXXXX> placeholders with the real thing. This is because xml does not support control characters
                            If InStr(label, "</?Serial>") <> 0 Then
                                goappstate.LastSerialNbr = Mid(label, 1, InStr(label, "</?Serial>") - 1) & ":" & Now.Now
                                label = Mid(label, InStr(label, "</?Serial>") + 10) 'grab everything except the serial header
                            Else
                                'the serial was not there, must be an error
                            End If
                            label = Replace(label, "<Char29>", "_1D") 'put the control character back in.
                            label = Replace(label, "<Char30>", "_1E") 'put the control character back in.
                            label = Replace(label, "<Char4>", "_04")
                            Pdoc.PrinterSettings.PrinterName = "Generic"
                            If Pdoc.PrinterSettings.IsValid Then
                                RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, label)
                            End If
                            'log the reprint request
                            'David Maibor (DSM) select DEV/PROD as appropriate
                            'reprintReq.EnterLog(localhostRequest.logTypes.Reprint.Reprint, "Reprint Requested at: " & Now.Date & " from Machine/Line: " & lblLineID.Text & " for: " & serialnbr(1))
                            reprintReq.EnterLog(dorme2.logTypes.Reprint.Reprint, "Reprint Requested at: " & Now.Now & " from Machine/Line: " & lblLineID.Text & " for: " & serialnbr(1))
                            MsgBox("Reprint Request was processed successfully", MsgBoxStyle.ApplicationModal)
                        Catch ex As Exception
                        End Try
                    End If
                Else
                    MsgBox("Nothing Selected for Reprint", MsgBoxStyle.ApplicationModal)
                End If
            End If
exithere:
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
        Finally
            mnuUserFuncReprintLast.Enabled = True
        End Try
    End Sub

    Private Sub mnuAdminUnlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminUnlock.Click

        If mnuAdminUnlock.Text = "Lock Admin" Then
            mnuAdminCriticalAbortSkid.Enabled = False
            mnuAdminGotDloc.Enabled = False
            mnuAdminGotLabel.Enabled = False
            mnuAdminCriticalSelectPrinter.Enabled = False
            mnuAdminCriticalSchedule.Enabled = False
            mnuAdminSendMessage.Enabled = False
            mnuAdminCritial.Enabled = False
            txtAdminCode.Text = ""
            mnuAdminUnlock.Text = "Unlock Admin"
        Else
            If txtAdminCode.Text = conf.Appsettings("//Properties/ExceptionsAdminCode") Then
                If mnuUserFuncSelectAll.Enabled = True Then
                    'if there is not a skid, dont enable
                    mnuAdminCriticalAbortSkid.Enabled = True
                Else
                    mnuAdminCriticalAbortSkid.Enabled = False
                End If
                mnuAdminGotDloc.Enabled = True
                mnuAdminGotLabel.Enabled = True
                mnuAdminCriticalSelectPrinter.Enabled = True
                mnuAdminCriticalSchedule.Enabled = True
                mnuAdminSendMessage.Enabled = True
                mnuAdminCritial.Enabled = True

                mnuAdminUnlock.Text = "Lock Admin"
            Else
                MsgBox("Incorrect Admin Code", MsgBoxStyle.ApplicationModal)
            End If
        End If

    End Sub

    Private Sub mnuUserFuncSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUserFuncSelectAll.Click
        Dim i As Integer
        On Error Resume Next
        Application.DoEvents()
        If dgSerials.VisibleRowCount > 0 Then
            If mnuUserFuncSelectAll.Text = "Select all on skid" Then
                For i = 0 To dgSerials.VisibleRowCount - 1
                    dgSerials.Select(i)
                Next
                mnuUserFuncSelectAll.Text = "Unselect all on skid"
            Else
                For i = 0 To dgSerials.VisibleRowCount - 1
                    dgSerials.UnSelect(i)
                Next
                mnuUserFuncSelectAll.Text = "Select all on skid"
            End If
        End If
    End Sub

    Private Sub ExceptionsHandler_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            StatusBar1.Panels(0).Text = Now.Now
            StatusBar1.Panels(2).Text = "Printer: Local Attached"
            PrinterID = "Generic"

            'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
            'Dim wr As New localhostRequest.RequestLabel  'DSM: DEV version
            'Dim dest As New localhostRequest.clsDestination  'DSM: DEV version
            Dim wr As New dorme2.RequestLabel  'DSM: PRODUCTION version
            Dim dest As New dorme2.clsDestination  'DSM: PRODUCTION version

            Dim retv As String
            Dim rs As DataSet


            dest.MachineName = lblLineID.Text
            dest.ControlFlag = "peekonly"
            rs = wr.GetSkidRecords(dest.MachineName)
            retv = wr.GetShipDestination_Webmethod(dest)
            lblMachID.Text = wr.GetMachineID(lblLineID.Text)

            If rs.Tables(0).Rows.Count = 0 Then
                'no skid associated with this
                StatusBar1.Panels(1).Text = "No Skid"
                mnuUserFuncSelectAll.Enabled = False
                '6/5/08, DSM: remove reprint function per Lance Adams
                'mnuUserFunctReprintSelected.Enabled = True
                mnuAdminCriticalAbortSkid.Enabled = False
                mnuAdminCriticalDeleteSelected.Enabled = False
                lblDestinationCode.Text = dest.CNTR 'from getshipdest
                lblPart.Text = dest.Part_nbr 'ditto

            Else
                dgSerials.DataSource = rs.Tables(0)
                dgSerials.Visible = True
                StatusBar1.Panels(1).Text = "Skid Serial Count: " & rs.Tables(0).Rows.Count
                lblDestinationCode.Text = rs.Tables(0).Rows(0).Item("Container_Code")
                lblSkidID.Text = rs.Tables(0).Rows(0).Item("Schedule_ID")
                lblDestinationCode.Text = dest.CNTR
                lblPart.Text = dest.Part_nbr
            End If
        Catch ex As Exception
            MsgBox("Error Loading Exception handler: => " & ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub

    Private Sub mnuUserFuncReprintLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUserFuncReprintLast.Click

        '6/4/08 David Maibor: just send the global variable reprintlabel to the printer, if it has data
        'David Maibor (DSM) select DEV/PRODUCTION as appropriate
        'Dim reprintReq As New localhostRequest.RequestLabel  'DSM: DEV version
        'Dim Greeting As New localhostRequest.Cordial 'DSM: DEV version
        Dim reprintReq As New dorme2.RequestLabel  'DSM: PRODUCTION version
        Dim Greeting As New dorme2.Cordial 'DSM: PRODUCTION version

        Dim serialnbr() As String
        Dim p As New RawPrinterHelper
        Dim label As String

        'DSM: quick fix to simply reprint label without going to BecWebservice
        Pdoc.PrinterSettings.PrinterName = PrinterID
        If Pdoc.PrinterSettings.IsValid And Len(reprintlabel) > 0 Then
            RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, reprintlabel)
            MsgBox("The last label has been reprinted", MsgBoxStyle.OKOnly, "Last Label Reprinted")
        Else
            MsgBox("Cannot reprint the label, please notify the system administrator", MsgBoxStyle.OKOnly, "Problem Reprinting Label")
        End If
        Exit Sub


        Try
            Application.DoEvents()
            If goappstate.BufferedLabelCount > 0 And (goappstate.BufferedLabelPointer < (goappstate.BufferedLabelCount - 1)) Then
                'if there are buffered labels, reprint them from the buffer
                'print this label and increment the pointer
                Pdoc.PrinterSettings.PrinterName = PrinterID
                If Pdoc.PrinterSettings.IsValid Then
                    RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, goappstate.BufferedLabels(goappstate.BufferedLabelPointer))
                    If goappstate.BufferedLabelPointer = (goappstate.BufferedLabelCount - 1) Then
                        'print the master label
                        RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, goappstate.BufferedLabels(0))
                    End If
                End If
            Else
                'buffered labels not available
                If Len(lblLastSerialNumber.Text) > 8 Then
                    mnuUserFuncReprintLast.Enabled = False
                    Application.DoEvents()


                    'If lblLastSerial.Text.Length > 8 Then
                    ReDim serialnbr(1)
                    serialnbr(1) = lblLastSerialNumber.Text
                    Greeting.Password = "forgetaboutit"
                    Greeting.UserID = "delphikiosk"
                    reprintReq.CordialValue = Greeting
                    label = reprintReq.ReprintICS(serialnbr)

                    '*********************************
                    If InStr(label, "</?BWSError>") <> 0 Then
                        'to handle the error
                        MsgBox(Mid(label, 1, InStr(label, "</?BWSError>") - 1), MsgBoxStyle.ApplicationModal) 'Extract the error from front of the return value
                        label = Mid(label, InStr(label, "</?BWSError>") + 12) 'strip off the error stuff
                        'print the error label
                        If InStr(StatusBar1.Panels(2).Text, "Local Attached") <> 0 Then
                            Pdoc.PrinterSettings.PrinterName = PrinterID
                        Else
                            Pdoc.PrinterSettings.PrinterName = Trim(Mid(StatusBar1.Panels(2).Text, 9))
                        End If
                        If Pdoc.PrinterSettings.IsValid Then
                            RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, label)
                        End If
                    Else
                        Try
                            'first we must replace any <?CharXXXX> placeholders with the real thing. This is because xml does not support control characters
                            If InStr(label, "</?Serial>") <> 0 Then
                                goappstate.LastSerialNbr = Mid(label, 1, InStr(label, "</?Serial>") - 1) & ":" & Now.Now
                                label = Mid(label, InStr(label, "</?Serial>") + 10) 'grab everything except the serial header
                            Else
                                'the serial was not there, must be an error
                            End If
                            label = Replace(label, "<Char29>", "_1D") 'put the control character back in.
                            label = Replace(label, "<Char30>", "_1E") 'put the control character back in.
                            label = Replace(label, "<Char4>", "_04")
                            Pdoc.PrinterSettings.PrinterName = PrinterID
                            If Pdoc.PrinterSettings.IsValid Then
                                RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, label)
                            End If
                            'log the reprint request
                            'David Maibor (DSM) select DEV/PRODUCTION as appropriate
                            'reprintReq.EnterLog(localhostRequest.logTypes.Reprint, "Reprint Requested at: " & Now.Date & " from Machine/Line: " & lblLineID.Text & " for: " & serialnbr(1))  'DSM: DEV version
                            reprintReq.EnterLog(dorme2.logTypes.Reprint, "Reprint Requested at: " & Now.Now & " from Machine/Line: " & lblLineID.Text & " for: " & serialnbr(1))    'DSM: PRODUCTION version
                            MsgBox("Reprint Request was processed successfully", MsgBoxStyle.ApplicationModal)
                        Catch ex As Exception
                        End Try
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
        Finally
            mnuUserFuncReprintLast.Enabled = True
        End Try
    End Sub

    Private Sub mnuClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        runningExceptions = 2
        Me.Close()
    End Sub

    Private Sub mnuAdminGotDloc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminGotDloc.Click
        'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
        'Dim webrequest As New localhostRequest.RequestLabel  'DEV version
        Dim webrequest As New dorme2.RequestLabel  'PRODUCTION version

        Dim dloc As String
        Dim ar() As String
        Try
            Application.DoEvents()
            If lblDestinationCode.Text.Trim = "" Or lblPart.Text.Trim = "" Then
                'don't do it
                Dim part As String
                Dim dest As String
                part = InputBox("Enter Part Nbr:")
                If Trim(part) = "" Then GoTo exithere

                dest = UCase(InputBox("Enter CNTR Code: (example AR for Arlington)"))
                If Trim(dest) = "" Then GoTo exithere

                dloc = webrequest.GotDLOC(part, dest)
                If InStr(dloc, "Failure") Then
                    MsgBox("Failure Getting DLOC for Part:" & part & " CNTR:" & dest & vbCrLf & "(Remember when using this screen, enter the Delphi Part Number, not the customer part number)", MsgBoxStyle.ApplicationModal)
                Else
                    dloc = dloc.Replace(";", vbCrLf)
                    MsgBox(dloc, MsgBoxStyle.ApplicationModal)
                End If
            Else
                dloc = webrequest.GotDLOC(lblPart.Text, lblDestinationCode.Text)
                If InStr(dloc, "Failure") Then
                    MsgBox("Failure Getting DLOC: " & dloc, MsgBoxStyle.ApplicationModal)
                Else
                    dloc = dloc.Replace(";", vbCrLf)
                    MsgBox(dloc, MsgBoxStyle.ApplicationModal)
                End If
            End If
exithere:
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub

    Private Sub mnuAdminGotLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminGotLabel.Click

        'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
        'Dim WebRequest As New localhostRequest.RequestLabel  'DSM: DEV version
        'Dim Greeting As New localhostRequest.Cordial 'DSM: DEV version
        Dim WebRequest As New dorme2.RequestLabel  'DSM: PRODUCTION version
        Dim Greeting As New dorme2.Cordial 'DSM: PRODUCTION version

        Dim Labels As String

        Try
            Application.DoEvents()
            Greeting.Password = "forgetaboutit"
            Greeting.UserID = "delphikiosk"
            WebRequest.CordialValue = Greeting
            If lblPart.Text = "" Then
                Dim part As String
                Dim dest As String
                part = InputBox("Enter Part Nbr:")
                If Trim(part) = "" Then GoTo exithere
                dest = UCase(InputBox("Enter CNTR code: (Example AR for Arlington)"))
                Labels = WebRequest.GotLabels(part, dest)
                If InStr(Labels, "Failure") Then
                    MsgBox("Failed to determine label availability for PartNbr: " & part & " CNTR: " & dest & " >>" & Labels, MsgBoxStyle.ApplicationModal)
                Else
                    Labels = Labels.Replace("VBCRLF", vbCrLf)
                    MsgBox(Labels, MsgBoxStyle.ApplicationModal, "ICS Labels Available ")
                End If
            Else
                Labels = WebRequest.GotLabels(lblPart.Text, lblDestinationCode.Text)
                If InStr(Labels, "Failure") Then
                    MsgBox("Failed to determine label availability for PartNbr: " & lblPart.Text & " CNTR: " & lblDestinationCode.Text & " >>" & Labels, MsgBoxStyle.ApplicationModal)
                Else
                    Labels = Labels.Replace("VBCRLF", vbCrLf)
                    MsgBox(Labels, MsgBoxStyle.ApplicationModal, "ICS Labels Available ")
                End If
            End If
exithere:
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub

    Private Sub mnuAdminSendMessage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminSendMessage.Click
        Try
            Dim m As New MessageSender
            m.ShowDialog()
        Catch ex As Exception
            MsgBox("Failure: " & ex.Message, MsgBoxStyle.ApplicationModal)
        End Try

    End Sub

    Private Sub mnuAdminCriticalDeleteSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminCriticalDeleteSelected.Click, cntxMnuDeleteFromSkid.Click
        Dim i As Integer
        Dim del() As String

        'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
        'Dim greeting As New localhostRequest.Cordial     'DSM: DEV version
        Dim greeting As New dorme2.Cordial     'DSM: PRODUCTION version

        Dim retv As String
        Dim f As Integer

        Try
            Application.DoEvents()
            If mnuAdminUnlock.Text = "Admin Unlock" Then
                MsgBox("Requires Admin unlock to procede", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
                Exit Sub
            End If

            'DSM: select DEV/PRODUCTION version as appropriate
            'Dim webref As New localhostRequest.RequestLabel  'DSM: DEV version
            Dim webref As New dorme2.RequestLabel  'DSM: PRODUCTION version

            greeting.UserID = "delphikiosk"
            greeting.Password = "forgetaboutit"
            webref.CordialValue = greeting

            If dgSerials.VisibleRowCount > 1 Then

                For i = 0 To dgSerials.VisibleRowCount - 1
                    If dgSerials.IsSelected(i) Then
                        ReDim Preserve del(i + 1)
                        del(i) = dgSerials.Item(i, 2)
                        f = f + 1
                    End If
                Next
                If f = 0 Then
                    MsgBox("Nothing Selected for Delete", MsgBoxStyle.ApplicationModal)
                    Exit Sub
                End If

                dgSerials.Tag = dgSerials.CaptionText
                dgSerials.CaptionText = "Processing delete, please wait..."
                retv = webref.DeleteICSBuffer(del)
                dgSerials.CaptionText = dgSerials.Tag
                MsgBox(retv, MsgBoxStyle.ApplicationModal)
                refreshSkid()
            Else
                MsgBox("Nothing Selected", MsgBoxStyle.ApplicationModal)
            End If

        Catch ex As Exception
            MsgBox("Failure: " & ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub
    Private Sub refreshSkid()

        'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
        'Dim dest As New localhostRequest.clsDestination  'DSM: DEV version
        'Dim wr As New localhostRequest.RequestLabel  'DSM: DEV version
        Dim dest As New dorme2.clsDestination  'DSM: PRODUCTION version
        Dim wr As New dorme2.RequestLabel  'DSM: PRODUCTION version

        Dim rs As DataSet

        Try
            dest.MachineName = lblLineID.Text
            dest.ControlFlag = "peekonly"
            rs = wr.GetSkidRecords(dest.MachineName)
            If rs.Tables(0).Rows.Count = 0 Then
                'no skid associated with this
                StatusBar1.Panels(1).Text = "No Skid"
                mnuUserFuncSelectAll.Enabled = False
                '6/5/08, DSM: remove reprint function per Lance Adams
                'mnuUserFunctReprintSelected.Enabled = True
                mnuAdminCriticalAbortSkid.Enabled = False
                mnuAdminCriticalDeleteSelected.Enabled = False
                lblDestinationCode.Text = dest.CNTR 'from getshipdest
                lblPart.Text = dest.Part_nbr 'ditto

            Else
                dgSerials.DataSource = rs.Tables(0)
                dgSerials.Visible = True
                StatusBar1.Panels(1).Text = "Skid Serial Count: " & rs.Tables(0).Rows.Count
                lblDestinationCode.Text = rs.Tables(0).Rows(0).Item("Container_Code")
                lblSkidID.Text = rs.Tables(0).Rows(0).Item("Schedule_ID")
                lblDestinationCode.Text = dest.CNTR
                lblPart.Text = dest.Part_nbr
            End If

        Catch ex As Exception
        Finally
            wr.Dispose()
            rs.Dispose()
        End Try
    End Sub

    Private Sub mnuAdminCriticalAbortSkid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminCriticalAbortSkid.Click
        Dim retv As String

        'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
        'Dim greeting As New localhostRequest.Cordial 'DSM: DEV version
        'Dim webref As New localhostRequest.RequestLabel  'DSM: DEV version
        Dim greeting As New dorme2.Cordial 'DSM: PRODUCTION version
        Dim webref As New dorme2.RequestLabel  'DSM: PRODUCTION version

        Try
            greeting.UserID = "delphikiosk"
            greeting.Password = "forgetaboutit"
            Application.DoEvents()
            webref.CordialValue = greeting
            retv = webref.AbortSkid(lblSkidID.Text, lblPart.Text, lblDestinationCode.Text)
            MsgBox(retv, MsgBoxStyle.ApplicationModal)
            If InStr(retv, "Failure") = 0 Then MsgBox("Note: If you are aborting this skid because of a Part/CNTR error, you may need to clear any others from the schedule", MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox("Failure in Abort Skid: " & ex.Message, MsgBoxStyle.ApplicationModal)
        Finally
            webref.Dispose()
            runningExceptions = 2
            Me.Close()
        End Try

    End Sub

    Private Sub mnuAdminCriticalScheduleWhatsScheduled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminCriticalScheduleWhatsScheduled.Click

        'David Maibor (DSM) select DEV/PRODUCTIN version as appropriate
        'Dim webreq As New localhostRequest.RequestLabel  'DSM: DEV version
        'Dim greeting As New localhostRequest.Cordial 'DSM: DEV version
        Dim webreq As New dorme2.RequestLabel  'DSM: PRODUCTION version
        Dim greeting As New dorme2.Cordial 'DSM: PRODUCTION version

        Dim ds As New DataSet

        Try
            Application.DoEvents()
            dgSchedule.DataSource = Nothing
            dgSchedule.Visible = True
            dgSchedule.BringToFront()
            dgSchedule.CaptionText = "Processing, please wait..."
            greeting.UserID = "delphikiosk"
            greeting.Password = "forgetaboutit"
            webreq.CordialValue = greeting

            Dim pn As String
            If lblPart.Text.Trim = "" Then
                pn = InputBox("Enter a Part Nbr: ")
                If pn.Length <> 8 Then
                    MsgBox("Invalid Part Nbr", MsgBoxStyle.ApplicationModal)
                    dgSchedule.Visible = False
                    mnuAdminCriticalScheduleDeleteScheduled.Enabled = False
                    GoTo exithere
                End If
            Else
                pn = lblPart.Text
            End If

            ds = webreq.GetWhatsScheduled(pn, "")
            If ds Is Nothing Then
                MsgBox("Nothing Scheduled for PartNbr: " & pn, MsgBoxStyle.ApplicationModal)
                dgSchedule.Visible = False
            Else
                dgSchedule.DataSource = ds.Tables(0)
                dgSchedule.CaptionText = "Current ICS Schedule"
                mnuAdminCriticalScheduleDeleteScheduled.Enabled = True
            End If

exithere:
        Catch ex As Exception
            MsgBox("Failure: " & ex.Message, MsgBoxStyle.ApplicationModal)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try

    End Sub

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        runningExceptions = 2
        Me.Close()
    End Sub

    Private Sub dgSchedule_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles dgSchedule.Navigate

    End Sub

    Private Sub dgSchedule_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgSchedule.MouseUp

    End Sub

    Private Sub mnuAdminCriticalScheduleDeleteScheduled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminCriticalScheduleDeleteScheduled.Click, cntxMnuDeleteSelectedSchedule.Click

    End Sub

    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem6.Click
        MsgBox("Selects all items in the data grid for the current skid", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem7.Click
        MsgBox("Re-prints the last requested label, and logs a record of the reprint request", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem8.Click
        MsgBox("Reprints all the serials which have been selected on the skid data grid, and logs a record of the reprint request", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem9.Click
        MsgBox("User must enter the Admin unlock code then select this.  If correct unlock code is entered, will unlock admin functions", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem10.Click
        MsgBox("Determines if there is DLOC information available for a particular Part, CNTR combination.  If so, displays that DLOC information", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem11.Click
        MsgBox("Determines if labels are available for the Part, CNTR, Package code, ECL and displays this information", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem5.Click

    End Sub

    Private Sub MenuItem12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem12.Click
        MsgBox("Email your comments, and critical information to PC&L", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem14.Click
        MsgBox("Delete selected serial numbers from the current skid.  This is useful if the skid gets out of sequence", MsgBoxStyle.Exclamation Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem15.Click
        MsgBox("Abort the skid releases the current skid and allows to start over. It will send a message to PC&L.  Use sparingly.", MsgBoxStyle.Exclamation Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem17.Click
        MsgBox("Show what's scheduled for the current part number, in the data grid.", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem18.Click
        MsgBox("Delete one or more scheduled entries sending a notification to PC&L.  This is useful when system problems are preventing from labeling a particular Part, CNTR, Package Code, ECL combination.  Use very sparingly", MsgBoxStyle.Exclamation Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub MenuItem19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem19.Click
        MsgBox("Allows to send next print request to an off-line printer. This is for emergency use. Caution must be used that your labels are not mixed with others at the off-line printer.", MsgBoxStyle.Exclamation Or MsgBoxStyle.ApplicationModal)
    End Sub

    Private Sub ContextMenu1_Popup(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Timer1_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer1.Elapsed
        On Error Resume Next
        StatusBar1.Panels(0).Text = Now.Now
    End Sub

    Private Sub mnuAdminCriticalSelectPrinterOffline_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminCriticalSelectPrinterOffline.Click
        Try
            StatusBar1.Tag = StatusBar1.Panels(2).Text
            StatusBar1.Panels(2).Text = "Getting Printer ID...wait"

            'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
            'Dim webreq As New localhostRequest.RequestLabel  'DSM: DEV version
            Dim webreq As New dorme2.RequestLabel  'DSM: PRODUCTION version

            Dim retv As String
            retv = webreq.GetOfflinePrinterName(goProperties.Department)
            If InStr(retv, "Failure") <> 0 Then
                PrinterID = "Generic"
                Throw New Exception(retv)
            Else
                StatusBar1.Panels(2).Text = "Printer: " & retv
                StatusBar1.Panels(2).Width = Len("Printer: " & retv)
                PrinterID = retv
            End If
        Catch ex As Exception
            MsgBox("Failure getting Offline printer ID: Check web service" & ex.Message Or MsgBoxStyle.ApplicationModal)
            StatusBar1.Panels(2).Text = StatusBar1.Tag
        End Try
    End Sub

    Private Sub StatusBar1_PanelClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.StatusBarPanelClickEventArgs) Handles StatusBar1.PanelClick

    End Sub

    Private Sub mnuAdminFunctions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminFunctions.Click

    End Sub

    Private Sub mnuAdminCriticalSelectPrinterAttached_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminCriticalSelectPrinterAttached.Click
        PrinterID = "Generic"
        StatusBar1.Panels(2).Text = "Printer: Local Attached"
    End Sub


    Private Sub cntxSchedule_Popup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxSchedule.Popup

    End Sub

    Private Sub cntxMnuScheduleHide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxMnuScheduleHide.Click
        dgSchedule.Visible = False
        mnuAdminCriticalScheduleDeleteScheduled.Enabled = False
    End Sub

    Protected Overrides Sub Finalize()
        runningExceptions = 2
        MyBase.Finalize()
    End Sub



    Private Sub cntxMnuDeleteALLwithMatchingParams_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxMnuDeleteALLwithMatchingParams.Click, mnuAdminCriticalScheduleDeleteALLMatchingSchedule.Click
        Dim i As Integer
        Dim s As New System.Text.StringBuilder
        Dim retv As String
        Try
            Application.DoEvents()
            If dgSchedule.VisibleRowCount > 1 Then
                For i = 0 To dgSchedule.VisibleRowCount - 1
                    If dgSchedule.IsSelected(i) Then
                        s.Append("ALL SCHEDULE ENTRIES FOR THESE WILL BE DELETED!  ARE YOU SURE?" & vbCrLf)
                        s.Append("Part Nbr: " & dgSchedule.Item(i, 0) & vbCrLf)
                        s.Append("ECL: " & dgSchedule.Item(i, 1) & vbCrLf)
                        s.Append("CNTR: " & dgSchedule.Item(i, 2) & vbCrLf)
                        s.Append("Package Code:" & dgSchedule.Item(i, 4) & vbCrLf)
                        If MsgBox(s.ToString, MsgBoxStyle.Exclamation Or MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then

                            'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
                            'Dim greeting As New localhostRequest.Cordial 'DSM: DEV version
                            'Dim webreq As New localhostRequest.RequestLabel  'DSM: DEV version
                            Dim greeting As New dorme2.Cordial 'DSM: PRODUCTION version
                            Dim webreq As New dorme2.RequestLabel  'DSM: PRODUCTION version

                            dgSchedule.Tag = dgSchedule.CaptionText
                            dgSchedule.CaptionText = "Deleting items from schedule..."
                            greeting.UserID = "delphikiosk"
                            greeting.Password = "forgetaboutit"
                            webreq.CordialValue = greeting
                            retv = webreq.DeleteScheduledRecords(dgSchedule.Item(i, 0), dgSchedule.Item(i, 1), dgSchedule.Item(i, 2), dgSchedule.Item(i, 4), "", False)
                            dgSchedule.CaptionText = dgSchedule.CaptionText
                            s.Remove(0, s.Length)
                            s.Append("Partnbr: " & dgSchedule.Item(i, 0) & Space(2))
                            s.Append("ECL: " & dgSchedule.Item(i, 1) & Space(2))
                            s.Append("CNTR: " & dgSchedule.Item(i, 2) & Space(2))
                            s.Append("Package Code:" & dgSchedule.Item(i, 4) & Space(2))

                            webreq.CordialValue = greeting
                            retv = webreq.SendEmail("BEC Labeling - ALL SCHEDULE ENTRIES FOR THESE ARE DELETED!", "high", s.ToString, "PCL")
                            dgSchedule.Visible = False
                            refreshSkid()
                            MsgBox("Deleted Schedule Entries as requested and sent notification to PC&L", MsgBoxStyle.ApplicationModal)
                            Exit For
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox("Failure: " & ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub



    Private Sub cntxMnuDeleteSchedulethisLineOnly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxMnuDeleteSchedulethisLineOnly.Click, mnuAdminCriticalDeleteScheduledforThisLine.Click
        Dim s As New System.Text.StringBuilder

        'David Maibor (DSM) select DEV/PRODUCTION vesion as appropriate
        'Dim webreq As New localhostRequest.RequestLabel  'DSM: DEV version
        'Dim greeting As New localhostRequest.Cordial 'DSM: DEV version
        Dim webreq As New dorme2.RequestLabel   'DSM: PRODUCTION version
        Dim greeting As New dorme2.Cordial      'DSM: PRODUCTION version

        Dim retv As String
        Dim i As Integer
        Dim fnd As Boolean
        Try

            Application.DoEvents()
            If dgSchedule.VisibleRowCount > 0 Then
                If MsgBox("Are you sure you want to delete Schedule entries for this Line?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal, "Input required") = MsgBoxResult.Yes Then
                    Do While True
                        If dgSchedule.Item(i, 5) & "" = "" Then
                            'nothing in that cell
                        Else
                            If CLng(dgSchedule.Item(i, 5)) = CLng(lblMachID.Text) Then 'only allow for the current line
                                fnd = True
                                dgSchedule.Tag = dgSchedule.CaptionText
                                dgSchedule.CaptionText = "Deleting items from schedule..."
                                greeting.UserID = "delphikiosk"
                                greeting.Password = "forgetaboutit"
                                webreq.CordialValue = greeting
                                retv = webreq.DeleteScheduledRecords("", "", "", "", lblMachID.Text, True) 'delete only for this machine
                                dgSchedule.CaptionText = dgSchedule.CaptionText
                                s.Remove(0, s.Length)
                                s.Append("Partnbr: " & dgSchedule.Item(i, 0) & Space(2))
                                s.Append("ECL: " & dgSchedule.Item(i, 1) & Space(2))
                                s.Append("CNTR: " & dgSchedule.Item(i, 2) & Space(2))
                                s.Append("Package Code:" & dgSchedule.Item(i, 4) & Space(2))

                                webreq.CordialValue = greeting
                                retv = webreq.SendEmail("BEC Labeling - SCHEDULE ENTRY DELETED!", "high", s.ToString, "PCL")
                                dgSchedule.Visible = False
                                refreshSkid()
                                MsgBox("Deleted Schedule Entries for this line only", MsgBoxStyle.ApplicationModal)
                                Exit Do
                            End If
                        End If
                        i = i + 1
                    Loop
                End If
            End If
            If fnd = False Then
                MsgBox("No Candidates for deletion found.  Verify the selected record is owned by this Line", MsgBoxStyle.Information Or MsgBoxStyle.ApplicationModal)
            End If
        Catch ex As Exception
            If InStr(ex.Message, "Specified argument was out of the range of valid values") <> 0 Then
                MsgBox("Scheduled Record for this line not found: lineID =>" & lblMachID.Text, MsgBoxStyle.ApplicationModal)
            Else
                MsgBox("Failure deleting: =>" & ex.Message, MsgBoxStyle.ApplicationModal)
            End If
        End Try

    End Sub

    Private Sub mnuAdminCriticalDeleteScheduledforThisLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminCriticalDeleteScheduledforThisLine.Click

    End Sub

    Private Sub mnuAdminCriticalScheduleDeleteALLMatchingSchedule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminCriticalScheduleDeleteALLMatchingSchedule.Click

    End Sub

    Private Sub cntxSkid_Popup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxSkid.Popup

    End Sub

    Private Sub cntxMnuReprintSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntxMnuReprintSelected.Click

    End Sub

    Private Sub mnuUserFunctions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUserFunctions.Click

    End Sub


    Private Sub mnuUserFunctReprintSkidSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUserFunctReprintSkidSummary.Click

        'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
        'Dim skidsum As New localhostRequest.RequestLabel 'DSM: DEV version
        Dim skidsum As New dorme2.RequestLabel 'DSM: PRODUCTION version

        Dim p As New RawPrinterHelper
        Dim label As String
        Dim sn As Long

        Try
getserial:
            Try
                sn = CLng(InputBox("Enter any Serial Number from the Skid: ", "Need some information"))
            Catch ex As Exception
                If MsgBox("Invalid Serial Number  Try Again?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.ApplicationModal) = MsgBoxResult.Yes Then
                    GoTo getserial
                Else
                    GoTo exithere
                End If
            End Try

            label = skidsum.PrintSkidSummary(sn)
            Pdoc.PrinterSettings.PrinterName = PrinterID
            If Pdoc.PrinterSettings.IsValid Then
                RawPrinterHelper.SendStringToPrinter(Pdoc.PrinterSettings.PrinterName, label)
            Else
                MsgBox("Printer Generic is not valid", MsgBoxStyle.ApplicationModal)
            End If
exithere:
        Catch ex As Exception
            MsgBox("Error in PrintSkidSummary: " & ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub

    Private Sub mnuAdminCriticalSelectPrinter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdminCriticalSelectPrinter.Click

    End Sub

    Private Sub miUnderhood_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miUnderhood.Click
        conf.Appsettings("//Properties/Department") = "02341"
        abortSkidSilent()
        Dim frm As New frmMain
        frm.btnClose.PerformClick()
        MsgBox("Department Change Made. Please Re-Open BEC SPL Application")
        Application.Exit()
    End Sub

    Private Sub miMid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miMid.Click
        conf.Appsettings("//Properties/Department") = "02342"
        abortSkidSilent()
        Dim frm As New frmMain
        frm.btnClose.PerformClick()
        MsgBox("Department Change Made. Please Re-Open BEC SPL Application")
        Application.Exit()
    End Sub

    Private Sub miLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miLeft.Click
        conf.Appsettings("//Properties/Department") = "02343"
        abortSkidSilent()
        Dim frm As New frmMain
        frm.btnClose.PerformClick()
        MsgBox("Department Change Made. Please Re-Open BEC SPL Application")
        Application.Exit()
    End Sub

    Function abortSkidSilent() As String()
        Dim retv As String

        'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
        'Dim greeting As New localhostRequest.Cordial    'DSM: DEV version
        'Dim webref As New localhostRequest.RequestLabel 'DSM: DEV version
        Dim greeting As New dorme2.Cordial  'DSM: PRODUCTION version
        Dim webref As New dorme2.RequestLabel   'DSM: PRODUCTION version

        Try
            greeting.UserID = "delphikiosk"
            greeting.Password = "forgetaboutit"
            Application.DoEvents()
            webref.CordialValue = greeting
            retv = webref.AbortSkid(lblSkidID.Text, lblPart.Text, lblDestinationCode.Text)

        Catch ex As Exception

        Finally
            webref.Dispose()
            runningExceptions = 2
            Me.Close()
        End Try
    End Function

End Class
