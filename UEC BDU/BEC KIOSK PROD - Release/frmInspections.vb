Public Class frmContainmentLog
    Inherits System.Windows.Forms.Form
    Private myDataSet As DataSet
    Private my_dgridtxtboxcol As New DataGridTextBoxColumn
    Public CommandString As String
    Private conf As New ConfigurationSettings
    Private currentControl As String 'used to handle scanning the inspector ID
    Private stdpackQty As Int64



#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SetUp()
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents dgProblems As System.Windows.Forms.DataGrid
    Friend WithEvents cmbInspectionType As System.Windows.Forms.ComboBox
    Friend WithEvents cmdLineID As System.Windows.Forms.ComboBox
    Friend WithEvents txtPartNbr As System.Windows.Forms.TextBox
    Friend WithEvents txtECL As System.Windows.Forms.TextBox
    Friend WithEvents txtSerial As System.Windows.Forms.TextBox
    Friend WithEvents txtQtyScraped As System.Windows.Forms.TextBox
    Friend WithEvents txtInspectionDate As System.Windows.Forms.TextBox
    Friend WithEvents txtInspector1 As System.Windows.Forms.TextBox
    Friend WithEvents txtInspector3 As System.Windows.Forms.TextBox
    Friend WithEvents txtInspector2 As System.Windows.Forms.TextBox
    Friend WithEvents txtInspector4 As System.Windows.Forms.TextBox
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents tmrLookforScan As System.Windows.Forms.Timer
    Friend WithEvents txtQtyAccepted As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        Me.dgProblems = New System.Windows.Forms.DataGrid
        Me.cmbInspectionType = New System.Windows.Forms.ComboBox
        Me.cmdLineID = New System.Windows.Forms.ComboBox
        Me.txtPartNbr = New System.Windows.Forms.TextBox
        Me.txtECL = New System.Windows.Forms.TextBox
        Me.txtSerial = New System.Windows.Forms.TextBox
        Me.txtQtyScraped = New System.Windows.Forms.TextBox
        Me.txtInspectionDate = New System.Windows.Forms.TextBox
        Me.txtInspector1 = New System.Windows.Forms.TextBox
        Me.txtInspector3 = New System.Windows.Forms.TextBox
        Me.txtInspector2 = New System.Windows.Forms.TextBox
        Me.txtInspector4 = New System.Windows.Forms.TextBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tmrLookforScan = New System.Windows.Forms.Timer(Me.components)
        Me.txtQtyAccepted = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        CType(Me.dgProblems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(32, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Inspection Type"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(160, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 24)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Line ID"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(232, 128)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(184, 24)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Inspector3"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(40, 184)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(184, 24)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Inspector2"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(40, 128)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(184, 24)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Inspector1"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(400, 32)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(120, 24)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "ECL"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(280, 32)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 24)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Part Nbr"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(232, 184)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(184, 24)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Inspector4"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(200, 80)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(96, 24)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Qty Rejected"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(40, 80)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(144, 24)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Container Serial Nbr"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(392, 80)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(120, 24)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Inspection Date"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(0, 240)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(120, 24)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "PROBLEMS"
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(0, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(120, 16)
        Me.Label13.TabIndex = 12
        Me.Label13.Text = "INSPECTIONS"
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(560, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 32)
        Me.btnClose.TabIndex = 12
        Me.btnClose.Text = "&Save and Close"
        '
        'dgProblems
        '
        Me.dgProblems.DataMember = ""
        Me.dgProblems.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgProblems.Location = New System.Drawing.Point(8, 264)
        Me.dgProblems.Name = "dgProblems"
        Me.dgProblems.Size = New System.Drawing.Size(536, 168)
        Me.dgProblems.TabIndex = 11
        '
        'cmbInspectionType
        '
        Me.cmbInspectionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbInspectionType.Location = New System.Drawing.Point(24, 56)
        Me.cmbInspectionType.Name = "cmbInspectionType"
        Me.cmbInspectionType.Size = New System.Drawing.Size(128, 21)
        Me.cmbInspectionType.TabIndex = 14
        Me.cmbInspectionType.Text = "ComboBox1"
        '
        'cmdLineID
        '
        Me.cmdLineID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLineID.Location = New System.Drawing.Point(184, 56)
        Me.cmdLineID.Name = "cmdLineID"
        Me.cmdLineID.Size = New System.Drawing.Size(72, 21)
        Me.cmdLineID.TabIndex = 15
        Me.cmdLineID.Text = "ComboBox1"
        '
        'txtPartNbr
        '
        Me.txtPartNbr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartNbr.Location = New System.Drawing.Point(288, 56)
        Me.txtPartNbr.Name = "txtPartNbr"
        Me.txtPartNbr.Size = New System.Drawing.Size(120, 20)
        Me.txtPartNbr.TabIndex = 16
        Me.txtPartNbr.Text = ""
        Me.txtPartNbr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtECL
        '
        Me.txtECL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtECL.Location = New System.Drawing.Point(432, 56)
        Me.txtECL.Name = "txtECL"
        Me.txtECL.Size = New System.Drawing.Size(56, 20)
        Me.txtECL.TabIndex = 17
        Me.txtECL.Text = ""
        Me.txtECL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSerial
        '
        Me.txtSerial.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial.Location = New System.Drawing.Point(56, 104)
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.Size = New System.Drawing.Size(120, 20)
        Me.txtSerial.TabIndex = 18
        Me.txtSerial.TabStop = False
        Me.txtSerial.Text = ""
        Me.txtSerial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtQtyScraped
        '
        Me.txtQtyScraped.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQtyScraped.Location = New System.Drawing.Point(208, 104)
        Me.txtQtyScraped.Name = "txtQtyScraped"
        Me.txtQtyScraped.Size = New System.Drawing.Size(80, 20)
        Me.txtQtyScraped.TabIndex = 0
        Me.txtQtyScraped.Text = ""
        Me.txtQtyScraped.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtInspectionDate
        '
        Me.txtInspectionDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInspectionDate.Location = New System.Drawing.Point(408, 104)
        Me.txtInspectionDate.Name = "txtInspectionDate"
        Me.txtInspectionDate.Size = New System.Drawing.Size(80, 20)
        Me.txtInspectionDate.TabIndex = 6
        Me.txtInspectionDate.TabStop = False
        Me.txtInspectionDate.Text = ""
        Me.txtInspectionDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtInspector1
        '
        Me.txtInspector1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInspector1.Location = New System.Drawing.Point(40, 152)
        Me.txtInspector1.Name = "txtInspector1"
        Me.txtInspector1.Size = New System.Drawing.Size(184, 20)
        Me.txtInspector1.TabIndex = 2
        Me.txtInspector1.Text = ""
        Me.txtInspector1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtInspector3
        '
        Me.txtInspector3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInspector3.Location = New System.Drawing.Point(232, 152)
        Me.txtInspector3.Name = "txtInspector3"
        Me.txtInspector3.Size = New System.Drawing.Size(184, 20)
        Me.txtInspector3.TabIndex = 4
        Me.txtInspector3.Text = ""
        Me.txtInspector3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtInspector2
        '
        Me.txtInspector2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInspector2.Location = New System.Drawing.Point(40, 208)
        Me.txtInspector2.Name = "txtInspector2"
        Me.txtInspector2.Size = New System.Drawing.Size(184, 20)
        Me.txtInspector2.TabIndex = 3
        Me.txtInspector2.Text = ""
        Me.txtInspector2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtInspector4
        '
        Me.txtInspector4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInspector4.Location = New System.Drawing.Point(232, 208)
        Me.txtInspector4.Name = "txtInspector4"
        Me.txtInspector4.Size = New System.Drawing.Size(184, 20)
        Me.txtInspector4.TabIndex = 5
        Me.txtInspector4.Text = ""
        Me.txtInspector4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdCancel
        '
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(560, 40)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 32)
        Me.cmdCancel.TabIndex = 13
        Me.cmdCancel.Text = "&Cancel"
        '
        'tmrLookforScan
        '
        '
        'txtQtyAccepted
        '
        Me.txtQtyAccepted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQtyAccepted.Location = New System.Drawing.Point(304, 104)
        Me.txtQtyAccepted.Name = "txtQtyAccepted"
        Me.txtQtyAccepted.Size = New System.Drawing.Size(80, 20)
        Me.txtQtyAccepted.TabIndex = 1
        Me.txtQtyAccepted.Text = ""
        Me.txtQtyAccepted.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(296, 80)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(96, 24)
        Me.Label14.TabIndex = 20
        Me.Label14.Text = "Qty Accepted"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmContainmentLog
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(640, 442)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtQtyAccepted)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.txtInspector4)
        Me.Controls.Add(Me.txtInspector2)
        Me.Controls.Add(Me.txtInspector3)
        Me.Controls.Add(Me.txtInspector1)
        Me.Controls.Add(Me.txtInspectionDate)
        Me.Controls.Add(Me.txtQtyScraped)
        Me.Controls.Add(Me.txtSerial)
        Me.Controls.Add(Me.txtECL)
        Me.Controls.Add(Me.txtPartNbr)
        Me.Controls.Add(Me.cmdLineID)
        Me.Controls.Add(Me.cmbInspectionType)
        Me.Controls.Add(Me.dgProblems)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmContainmentLog"
        Me.Text = "Inspections"
        CType(Me.dgProblems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub

    Private Sub frmContainmentLog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        Dim ar
        Dim t As String, i As Int32

        'populate the combo box for inspection types
        t = conf.Appsettings("//ApplicationState/InspectionTypes")
        ar = Split(t, ";")

        For i = 0 To UBound(ar) - 1
            With cmbInspectionType.Items
                .Add(ar(i))
            End With
        Next i

        'populate combo for lines, which will be converted to machineIDs
        With cmdLineID.Items
            For i = 1 To 15
                If i = 11 Or i = 12 Or i = 13 Or i = 14 Then
                    'dont add these are invalid lines
                Else
                    .Add(i)
                End If
            Next
        End With

        txtQtyScraped.Text = 0
        txtInspectionDate.Text = Now.Date
        If Not IsNothing(CommandString) Then
            'notice the format this format of input this form is expecting, by the ar(i)
            ar = Split(CommandString, ",")
            cmbInspectionType.Text = ar(0) 'inspectiontype
            cmdLineID.Text = ar(1) 'lineID
            txtPartNbr.Text = ar(2) 'etc.
            txtECL.Text = ar(3)
            txtSerial.Text = ar(4)
            stdpackQty = ar(5)
            txtPartNbr.ReadOnly = True
            txtECL.ReadOnly = True
            txtSerial.ReadOnly = True
            txtInspectionDate.ReadOnly = True
            cmdLineID.Enabled = False
        End If

    End Sub
    Private Sub SetUp()
        Dim result As String
        Try
            ' Create a DataSet with two tables and one relation.
            myDataSet = MakeDataSet(result)
            ' Bind the DataGrid to the DataSet. The dataMember
            ' specifies that the Customers table should be displayed.
            dgProblems.SetDataBinding(myDataSet, "QUALITY_PROBLEM")
            Dim tsl As New DataGridTableStyle
            tsl.MappingName = "QUALITY_PROBLEM"
            tsl.AlternatingBackColor = Color.AliceBlue

            'add combo box
            Dim problemStyle As New DataGridControlColumn(GetType(ComboBox), "Text", "", 120, 22, 22)
            problemStyle.MappingName = "PROBLEM_CODE"
            problemStyle.HeaderText = "Problem Code"
            problemStyle.Width = 100
            tsl.GridColumnStyles.Add(problemStyle)
            ' Fill the list area of the combobox
            Dim cbo As ComboBox = CType(problemStyle.InnerControl, ComboBox)
            'populate problem codes combo box

            'cbo.Items.AddRange(New String() {100, 101, 102, 103, 104, 99})
            cbo.Items.AddRange(common.Quality_ProbCodes)

            'add the location box
            Dim txtLoc As New DataGridTextBoxColumn
            txtLoc.MappingName = "LOCATION_ON_PART"
            txtLoc.Width = 100
            txtLoc.HeaderText = "Location"
            tsl.GridColumnStyles.Add(txtLoc)

            'add the analysis box
            Dim txtAnalysis As New DataGridTextBoxColumn
            txtAnalysis.MappingName = "ANALYSIS"
            txtAnalysis.Width = 100
            txtAnalysis.HeaderText = "Analysis"
            tsl.GridColumnStyles.Add(txtAnalysis)

            'add the problem quantity box
            Dim txtProbQty As New DataGridTextBoxColumn
            txtProbQty.MappingName = "PROBLEM_QTY"
            txtProbQty.Width = 100
            txtProbQty.HeaderText = "Problem Qty"
            tsl.GridColumnStyles.Add(txtProbQty)

            dgProblems.TableStyles.Add(tsl)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Function MakeDataSet(ByRef result As String) As DataSet
        Dim mydataset As New DataSet
        Try
            'This function will return the dataset for the users so they don't have to make it.

            'The oracle tables have the following structures:
            '*****************************************************************************
            '                   : QUALITY_INSPECTION()
            'INSPECTION_ID:=> Numeric
            'INSPECTION_TYPE:=> String
            'STANDARD_PACK_ID:=> Numeric
            'PRODUCT_ID:=> Numeric
            'ACCEPTABLE_QTY:=> Numeric
            'DEFECTIVE_QTY:=> Numeric

            '*****************************************************************************
            '                   : QUALITY_INSPECTOR()
            'INSPECTION_ID:=> Numeric
            'INSPECTION_SEQUENCE:=> Numeric
            'ORG_EMP_ID:=> Numeric
            'MACHINE_ID:=> Numeric
            'INSPECTION_TMSTM:=> Date

            '*****************************************************************************
            '                   : QUALITY_PROBLEM()
            'INSPECTION_ID:=> Numeric
            'PROBLEM_CODE:=> String
            'LANGUAGE_CODE:=> String
            'LOCATION_ON_PART:=> String
            'ANALYSIS:=> String
            'PROBLEM_QTY:=> Numeric


            ' Create a local DataSet to hold and pass the data to oracle.
            mydataset = New DataSet("myDataSet")

            ' Create DataTables.
            Dim tInspection As New DataTable("QUALITY_INSPECTION")
            Dim tProblems As New DataTable("QUALITY_PROBLEM")
            Dim tInspectors As New DataTable("QUALITY_INSPECTOR")
            Dim tSerialProduct As New DataTable("SERIALIZED_PRODUCT")

            'create the inspectors table after the oracle structure
            Dim cInspSeq As New DataColumn("INSPECTION_SEQUENCE", GetType(Int64))
            Dim cORG_EMP_ID As New DataColumn("ORG_EMP_ID", GetType(Int64))
            Dim cMACHINE_ID As New DataColumn("MACHINE_ID", GetType(Int64))
            Dim cINSPECTION_TMSTM As New DataColumn("INSPECTION_TMSTM", GetType(Date))
            tInspectors.Columns.Add(cInspSeq)
            tInspectors.Columns.Add(cORG_EMP_ID)
            tInspectors.Columns.Add(cMACHINE_ID)
            tInspectors.Columns.Add(cINSPECTION_TMSTM)

            ' Create columns, and add to the Problems table.
            Dim cProblemCode As New DataColumn("PROBLEM_CODE", GetType(String))
            Dim cLocation As New DataColumn("LOCATION_ON_PART", GetType(String))
            Dim cAnalysis As New DataColumn("ANALYSIS", GetType(String))
            Dim cProblemQty As New DataColumn("PROBLEM_QTY", GetType(Int64))
            Dim cLang As New DataColumn("LANGUAGE_CODE", GetType(String))
            tProblems.Columns.Add(cProblemCode)
            tProblems.Columns.Add(cLocation)
            tProblems.Columns.Add(cAnalysis)
            tProblems.Columns.Add(cProblemQty)
            tProblems.Columns.Add(cLang)

            'create columns for inspection table
            Dim cInspectionType As New DataColumn("INSPECTION_TYPE", GetType(String))
            Dim cProductID As New DataColumn("PRODUCT_ID", GetType(String))
            Dim cStdPackID As New DataColumn("STANDARD_PACK_ID", GetType(String))
            Dim cSerialNbr As New DataColumn("SERIAL_NBR", GetType(String))
            Dim cQtyScraped As New DataColumn("DEFECTIVE_QTY", GetType(Int32))
            Dim cGoodQty As New DataColumn("ACCEPTABLE_QTY", GetType(Int32))
            tInspection.Columns.Add(cInspectionType)
            tInspection.Columns.Add(cProductID)
            tInspection.Columns.Add(cStdPackID)
            tInspection.Columns.Add(cQtyScraped)
            tInspection.Columns.Add(cGoodQty)
            tInspection.Columns.Add(cSerialNbr)

            'create columns for serialized_product table
            Dim cProd As New DataColumn("PRODUCT_ID", GetType(Int64))
            Dim cPProd As New DataColumn("PARENT_PRODUCT_ID", GetType(Int64))
            Dim cstid As New DataColumn("STANDARD_PACK_ID", GetType(Int64))
            Dim cMachID As New DataColumn("MACHINE_ID", GetType(Int64))
            Dim cDispc As New DataColumn("DISPOSITION_CODE", GetType(String))
            Dim cScrapC As New DataColumn("SCRAP_CODE", GetType(String))
            Dim cMesP As New DataColumn("MES_PART_ID", GetType(Int64))
            Dim cSN As New DataColumn("SERIAL_NBR", GetType(String))
            Dim cPrdTmstm As New DataColumn("PRODUCED_TMSTM", GetType(Date))
            Dim cScrap As New DataColumn("SCRAP_SYSTEM_ID", GetType(Int64))
            Dim cRevPhy As New DataColumn("REV_PHYSICAL", GetType(String))
            Dim cRevNonPhy As New DataColumn("REV_NON_PHYSICAL", GetType(String))
            Dim cInsUser As New DataColumn("INSERT_USERID", GetType(String))
            Dim cPartNbr As New DataColumn("Part_Nbr", GetType(String))

            tSerialProduct.Columns.Add(cProd)
            tSerialProduct.Columns.Add(cPProd)
            tSerialProduct.Columns.Add(cstid)
            tSerialProduct.Columns.Add(cMachID)
            tSerialProduct.Columns.Add(cDispc)
            tSerialProduct.Columns.Add(cScrapC)
            tSerialProduct.Columns.Add(cMesP)
            tSerialProduct.Columns.Add(cSN)
            tSerialProduct.Columns.Add(cPrdTmstm)
            tSerialProduct.Columns.Add(cScrap)
            tSerialProduct.Columns.Add(cRevPhy)
            tSerialProduct.Columns.Add(cRevNonPhy)
            tSerialProduct.Columns.Add(cInsUser)
            tSerialProduct.Columns.Add(cPartNbr)


            ' Add the tables to the DataSet.
            mydataset.Tables.Add(tProblems)
            mydataset.Tables.Add(tInspection)
            mydataset.Tables.Add(tInspectors)
            mydataset.Tables.Add(tSerialProduct)

            'return to sender
            result = "Success in MakeDataSet: " & Now.Now
            Return mydataset
        Catch ex As Exception
            result = "Failure in MakeDataSet: " & ex.Message
        Finally
            mydataset.Dispose()
        End Try
    End Function



    Private Sub cmdAddProb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        dgProblems.Focus()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        common.ProcessingContainment = False
        Me.Dispose()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim retv As String
        Dim ep As String
        Dim i As Int16

        'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
        'Dim reqlab As New localhostRequest.RequestLabel  'DSM: DEV version
        Dim reqlab As New dorme2.RequestLabel  'DSM: PRODUCTION version

        Try


            'first, put the inspection records into the inspections table
            ep = "Add inspection data to dataset"
            Dim dr As DataRow

            dr = myDataSet.Tables("QUALITY_INSPECTION").NewRow
            dr.Item("INSPECTION_TYPE") = cmbInspectionType.Text
            dr.Item("SERIAL_NBR") = txtSerial.Text
            dr.Item("Standard_Pack_ID") = "" 'set these to nothing because they are not used by kiosk
            dr.Item("Product_ID") = ""

            If IsNumeric(txtQtyAccepted.Text) Then
                dr.Item("ACCEPTABLE_QTY") = Val(txtQtyAccepted.Text)
            Else
                MsgBox("Must Enter Numeric value for Acceptable Qty: ")
                Exit Sub
            End If
            If IsNumeric(txtQtyScraped.Text) Then
                dr.Item("DEFECTIVE_QTY") = Val(txtQtyScraped.Text)
            Else
                MsgBox("Must Enter Numeric value for Rejected Qty: ")
                Exit Sub
            End If

            myDataSet.Tables("QUALITY_INSPECTION").Rows.Add(dr)

            If txtInspector1.Text = "" And txtInspector2.Text = "" And txtInspector3.Text = "" And txtInspector4.Text = "" Then
                MsgBox("Must save at least one Inspector", MsgBoxStyle.Information)
                Exit Sub
            Else
                'put data in the inspectors table
                ep = "Add Inspectors to dataset"
                For i = 1 To 4
                    Select Case i
                        Case 1
                            If Trim(txtInspector1.Text) = "" Then Exit For
                            If Not IsNumeric(txtInspector1.Text) Then
                                MsgBox("Must Enter Numeric InspectorID for Inspector #1")
                                Exit Sub
                            End If
                            dr = myDataSet.Tables("QUALITY_INSPECTOR").NewRow
                            dr.Item("ORG_EMP_ID") = txtInspector1.Text
                            dr.Item("INSPECTION_SEQUENCE") = i
                            dr.Item("MACHINE_ID") = Val(cmdLineID.Text)
                            dr.Item("INSPECTION_TMSTM") = CDate(txtInspectionDate.Text)
                        Case 2
                            If Trim(txtInspector2.Text) = "" Then Exit For
                            If Not IsNumeric(txtInspector2.Text) Then
                                MsgBox("Must Enter Numeric InspectorID for Inspector #2")
                                Exit Sub
                            End If
                            dr = myDataSet.Tables("QUALITY_INSPECTOR").NewRow
                            dr.Item("ORG_EMP_ID") = txtInspector2.Text
                            dr.Item("INSPECTION_SEQUENCE") = i
                            dr.Item("MACHINE_ID") = Val(cmdLineID.Text)
                            dr.Item("INSPECTION_TMSTM") = CDate(txtInspectionDate.Text)
                        Case 3
                            If Trim(txtInspector3.Text) = "" Then Exit For
                            If Not IsNumeric(txtInspector3.Text) Then
                                MsgBox("Must Enter Numeric InspectorID for Inspector #3")
                                Exit Sub
                            End If
                            dr = myDataSet.Tables("QUALITY_INSPECTOR").NewRow
                            dr.Item("ORG_EMP_ID") = txtInspector3.Text
                            dr.Item("INSPECTION_SEQUENCE") = i
                            dr.Item("MACHINE_ID") = Val(cmdLineID.Text)
                            dr.Item("INSPECTION_TMSTM") = CDate(txtInspectionDate.Text)
                        Case 4
                            If Trim(txtInspector4.Text) = "" Then Exit For
                            If Not IsNumeric(txtInspector4.Text) Then
                                MsgBox("Must Enter Numeric InspectorID for Inspector #4")
                                Exit Sub
                            End If
                            dr = myDataSet.Tables("QUALITY_INSPECTOR").NewRow
                            dr.Item("ORG_EMP_ID") = txtInspector4.Text
                            dr.Item("INSPECTION_SEQUENCE") = i
                            dr.Item("MACHINE_ID") = Val(cmdLineID.Text)
                            dr.Item("INSPECTION_TMSTM") = CDate(txtInspectionDate.Text)
                    End Select
                    myDataSet.Tables("QUALITY_INSPECTOR").Rows.Add(dr)
                Next
                'put the language code in for the problems because its required by mesdba
                For i = 0 To myDataSet.Tables("Quality_Problem").Rows.Count - 1
                    myDataSet.Tables("Quality_Problem").Rows(i).Item("Language_code") = "en-us"
                Next

                ep = "Calling SaveInspectionRecord web method"
                'myDataSet.WriteXml("C:\Inetpub\wwwroot\BECWebService_RFC100-03-124001\bin\xmlttt.xml")
                retv = reqlab.SaveInpectionRecord(myDataSet)
                If InStr(retv, "Failure") <> 0 Then
                    MsgBox("Failure Saving Inspection Record: " & retv)
                End If
                Me.Dispose()
            End If
            common.ProcessingContainment = False
        Catch ex As Exception
            MsgBox("Failure: " & ep & " => " & ex.Message)
        Finally
            reqlab.Dispose()
            myDataSet.Dispose()
        End Try
    End Sub

    Private Sub txtQtyScraped_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQtyScraped.TextChanged

    End Sub

    Private Sub txtInspector2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInspector2.TextChanged

    End Sub

    Private Sub txtInspector2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInspector2.Leave
        If txtInspector2.Text = "" Then
            dgProblems.Focus()
        End If
        tmrLookforScan.Enabled = False
    End Sub

    Private Sub txtInspector3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInspector3.TextChanged

    End Sub

    Private Sub txtInspector3_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInspector3.Leave
        If txtInspector3.Text = "" Then
            dgProblems.Focus()
        End If
        tmrLookforScan.Enabled = False
    End Sub

    Private Sub txtInspector1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInspector1.Leave

        tmrLookforScan.Enabled = False
    End Sub

    Private Sub cmbInspectionType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbInspectionType.SelectedIndexChanged

    End Sub

    Private Sub dgProblems_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles dgProblems.Navigate

    End Sub

    Private Sub txtInspector1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInspector1.TextChanged

    End Sub

    Private Sub txtInspector1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInspector1.GotFocus
        currentControl = "I1"
        common.ScannedForContainment = ""
        tmrLookforScan.Interval = 100
        tmrLookforScan.Enabled = True
    End Sub

    Private Sub txtInspector2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInspector2.GotFocus
        currentControl = "I2"
        common.ScannedForContainment = ""
        tmrLookforScan.Interval = 100
        tmrLookforScan.Enabled = True
    End Sub

    Private Sub txtInspector3_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInspector3.GotFocus
        currentControl = "I3"
        common.ScannedForContainment = ""
        tmrLookforScan.Interval = 100
        tmrLookforScan.Enabled = True
    End Sub

    Private Sub txtInspector4_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInspector4.GotFocus
        currentControl = "I4"
        common.ScannedForContainment = ""
        tmrLookforScan.Interval = 100
        tmrLookforScan.Enabled = True
    End Sub

    Private Sub txtInspector4_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInspector4.Leave
        tmrLookforScan.Enabled = False
    End Sub

    Private Sub tmrLookforScan_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrLookforScan.Tick
        If common.ScannedForContainment <> "" Then
            tmrLookforScan.Enabled = False
            Select Case currentControl
                Case "I1"
                    Me.txtInspector1.Text = common.ScannedForContainment
                    Me.txtInspector2.Focus()
                Case "I2"
                    Me.txtInspector2.Text = common.ScannedForContainment
                    Me.txtInspector3.Focus()
                Case "I3"
                    Me.txtInspector3.Text = common.ScannedForContainment
                    Me.txtInspector4.Focus()
                Case "I4"
                    Me.txtInspector4.Text = common.ScannedForContainment
                    dgProblems.Focus()
            End Select
            common.ScannedForContainment = ""
        End If
    End Sub

    Private Sub txtInspector4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInspector4.TextChanged

    End Sub

    Private Sub Label9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.Click

    End Sub

    Private Sub txtQtyScraped_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQtyScraped.LostFocus
        Try
            txtQtyAccepted.Text = stdpackQty - Val(txtQtyScraped.Text)
        Catch ex As Exception
        End Try
    End Sub
End Class
