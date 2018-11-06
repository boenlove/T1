Option Explicit On 
Option Strict On

Public Class SetCount
	Inherits System.Windows.Forms.Form
    Private pPassCode As String
#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call

	End Sub

	Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
		MyBase.Dispose(disposing)
	End Sub

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	Friend WithEvents pOne As System.Windows.Forms.Button
	Friend WithEvents pZero As System.Windows.Forms.Button
	Friend WithEvents pNine As System.Windows.Forms.Button
	Friend WithEvents pEight As System.Windows.Forms.Button
	Friend WithEvents pSeven As System.Windows.Forms.Button
	Friend WithEvents pSix As System.Windows.Forms.Button
	Friend WithEvents pFive As System.Windows.Forms.Button
	Friend WithEvents pFour As System.Windows.Forms.Button
	Friend WithEvents pThree As System.Windows.Forms.Button
	Friend WithEvents pTwo As System.Windows.Forms.Button
	Friend WithEvents lblPart As System.Windows.Forms.Label
	Friend WithEvents ClearPartToScan As System.Windows.Forms.Button
	Friend WithEvents Cancel As System.Windows.Forms.Button
	Friend WithEvents OK As System.Windows.Forms.Button
	Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPassCode As System.Windows.Forms.TextBox
    Private pMinSetCount As Integer = 0
    Private pMaxSetCount As Integer = 0

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Cancel = New System.Windows.Forms.Button
        Me.pOne = New System.Windows.Forms.Button
        Me.pZero = New System.Windows.Forms.Button
        Me.pNine = New System.Windows.Forms.Button
        Me.pEight = New System.Windows.Forms.Button
        Me.pSeven = New System.Windows.Forms.Button
        Me.pSix = New System.Windows.Forms.Button
        Me.pFive = New System.Windows.Forms.Button
        Me.pFour = New System.Windows.Forms.Button
        Me.pThree = New System.Windows.Forms.Button
        Me.pTwo = New System.Windows.Forms.Button
        Me.lblPart = New System.Windows.Forms.Label
        Me.ClearPartToScan = New System.Windows.Forms.Button
        Me.OK = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtPassCode = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Cancel
        '
        Me.Cancel.Location = New System.Drawing.Point(372, 243)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(105, 57)
        Me.Cancel.TabIndex = 14
        Me.Cancel.Text = "Cancel"
        '
        'pOne
        '
        Me.pOne.Location = New System.Drawing.Point(33, 69)
        Me.pOne.Name = "pOne"
        Me.pOne.Size = New System.Drawing.Size(81, 66)
        Me.pOne.TabIndex = 13
        Me.pOne.Text = "1"
        '
        'pZero
        '
        Me.pZero.Location = New System.Drawing.Point(144, 333)
        Me.pZero.Name = "pZero"
        Me.pZero.Size = New System.Drawing.Size(81, 66)
        Me.pZero.TabIndex = 12
        Me.pZero.Text = "0"
        '
        'pNine
        '
        Me.pNine.Location = New System.Drawing.Point(249, 234)
        Me.pNine.Name = "pNine"
        Me.pNine.Size = New System.Drawing.Size(81, 66)
        Me.pNine.TabIndex = 11
        Me.pNine.Text = "9"
        '
        'pEight
        '
        Me.pEight.Location = New System.Drawing.Point(141, 234)
        Me.pEight.Name = "pEight"
        Me.pEight.Size = New System.Drawing.Size(81, 66)
        Me.pEight.TabIndex = 10
        Me.pEight.Text = "8"
        '
        'pSeven
        '
        Me.pSeven.Location = New System.Drawing.Point(33, 234)
        Me.pSeven.Name = "pSeven"
        Me.pSeven.Size = New System.Drawing.Size(81, 66)
        Me.pSeven.TabIndex = 9
        Me.pSeven.Text = "7"
        '
        'pSix
        '
        Me.pSix.Location = New System.Drawing.Point(249, 150)
        Me.pSix.Name = "pSix"
        Me.pSix.Size = New System.Drawing.Size(81, 66)
        Me.pSix.TabIndex = 8
        Me.pSix.Text = "6"
        '
        'pFive
        '
        Me.pFive.Location = New System.Drawing.Point(138, 150)
        Me.pFive.Name = "pFive"
        Me.pFive.Size = New System.Drawing.Size(81, 66)
        Me.pFive.TabIndex = 7
        Me.pFive.Text = "5"
        '
        'pFour
        '
        Me.pFour.Location = New System.Drawing.Point(33, 150)
        Me.pFour.Name = "pFour"
        Me.pFour.Size = New System.Drawing.Size(81, 66)
        Me.pFour.TabIndex = 6
        Me.pFour.Text = "4"
        '
        'pThree
        '
        Me.pThree.Location = New System.Drawing.Point(246, 69)
        Me.pThree.Name = "pThree"
        Me.pThree.Size = New System.Drawing.Size(81, 66)
        Me.pThree.TabIndex = 5
        Me.pThree.Text = "3"
        '
        'pTwo
        '
        Me.pTwo.Location = New System.Drawing.Point(138, 69)
        Me.pTwo.Name = "pTwo"
        Me.pTwo.Size = New System.Drawing.Size(81, 66)
        Me.pTwo.TabIndex = 4
        Me.pTwo.Text = "2"
        '
        'lblPart
        '
        Me.lblPart.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblPart.Location = New System.Drawing.Point(141, 27)
        Me.lblPart.Name = "lblPart"
        Me.lblPart.Size = New System.Drawing.Size(129, 24)
        Me.lblPart.TabIndex = 3
        Me.lblPart.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ClearPartToScan
        '
        Me.ClearPartToScan.Location = New System.Drawing.Point(290, 21)
        Me.ClearPartToScan.Name = "ClearPartToScan"
        Me.ClearPartToScan.Size = New System.Drawing.Size(120, 36)
        Me.ClearPartToScan.TabIndex = 2
        Me.ClearPartToScan.Text = "Clear Part to Scan"
        '
        'OK
        '
        Me.OK.Location = New System.Drawing.Point(372, 138)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(105, 57)
        Me.OK.TabIndex = 1
        Me.OK.Text = "OK"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(10, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(130, 18)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Qty Parts Scanned:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtPassCode
        '
        Me.txtPassCode.Location = New System.Drawing.Point(380, 110)
        Me.txtPassCode.Name = "txtPassCode"
        Me.txtPassCode.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.txtPassCode.TabIndex = 15
        Me.txtPassCode.Text = ""
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(370, 90)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 20)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Unlock Code"
        '
        'SetCount
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(502, 416)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtPassCode)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.ClearPartToScan)
        Me.Controls.Add(Me.lblPart)
        Me.Controls.Add(Me.pTwo)
        Me.Controls.Add(Me.pThree)
        Me.Controls.Add(Me.pFour)
        Me.Controls.Add(Me.pFive)
        Me.Controls.Add(Me.pSix)
        Me.Controls.Add(Me.pSeven)
        Me.Controls.Add(Me.pEight)
        Me.Controls.Add(Me.pNine)
        Me.Controls.Add(Me.pZero)
        Me.Controls.Add(Me.pOne)
        Me.Controls.Add(Me.Cancel)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SetCount"
        Me.Text = "SetCount"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Property minSetCount() As Integer
        Get
            minSetCount = pminSetCount
        End Get
        Set(ByVal Value As Integer)
            pMinSetCount = Value
        End Set
    End Property
    Public Property maxSetCount() As Integer
        Get
            maxSetCount = pMaxSetCount
        End Get
        Set(ByVal Value As Integer)
            pMaxSetCount = Value
        End Set
    End Property
    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        runningSetCount = 2
        Me.Close()
    End Sub

    Private Sub pOne_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pOne.Click
        lblPart.Text += "1"
    End Sub

    Private Sub pTwo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pTwo.Click
        lblPart.Text += "2"
    End Sub

    Private Sub pThree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pThree.Click
        lblPart.Text += "3"
    End Sub

    Private Sub pFour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pFour.Click
        lblPart.Text += "4"
    End Sub

    Private Sub pFive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pFive.Click
        lblPart.Text += "5"
    End Sub

    Private Sub pSix_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pSix.Click
        lblPart.Text += "6"
    End Sub

    Private Sub pSeven_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pSeven.Click
        lblPart.Text += "7"
    End Sub

    Private Sub pEight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pEight.Click
        lblPart.Text += "8"
    End Sub

    Private Sub pNine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pNine.Click
        lblPart.Text += "9"
    End Sub

    Private Sub pZero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pZero.Click
        lblPart.Text += "0"
    End Sub


    Private Sub ClearPartToScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearPartToScan.Click
        lblPart.Text = ""
    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Dim i As Int32 = 0

        Try
            If txtPassCode.Text.ToUpper <> pPassCode.ToUpper Then
                MsgBox("Must Enter Valid Unlock Code", MsgBoxStyle.ApplicationModal)
                Exit Sub
            End If

            If lblPart.Text = "" Then
                MsgBox("A Numeric Quantity is required.  Or Cancel the operation", MsgBoxStyle.ApplicationModal)
            Else
                If Convert.ToInt32(lblPart.Text) <= 1 Then
                    Dim ms As String
                    ms = "Must Set Count greater than 1.  (note: Setting to 1 is undefined without doing the Process BroadCast Code. If you must set to 1, use the Main form's RESET Button and Re-Start the process)"
                    MsgBox(ms, MsgBoxStyle.OKOnly Or MsgBoxStyle.ApplicationModal)
                    lblPart.Text = ""
                ElseIf Convert.ToInt32(lblPart.Text) < minSetCount Then
                    MsgBox("Can not set count less than: " & minSetCount, MsgBoxStyle.ApplicationModal)
                    lblPart.Text = ""
                Else
                    If CType(lblPart.Text, Integer) <= maxSetCount Then
                        i = CType(lblPart.Text, Integer)
                        goappstate.TotalPartsScanned = i
                        If goappstate.NextAction.StartsWith("Correct") Then
                            'don't change the next action, we are working through a correct layer, package etc. process
                        Else
                            goappstate.NextAction = "Now Scan Part " & CType(lblPart.Text, Integer) + 1 & " of " & goappstate.StdPack.ToString
                        End If
                        runningSetCount = 2
                        Me.Close()
                    Else
                        MsgBox("Cannot set to value larger than: " & maxSetCount, MsgBoxStyle.ApplicationModal)
                    End If
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.ApplicationModal)
        End Try
    End Sub

    Private Sub lblPart_RightToLeftChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblPart.RightToLeftChanged

    End Sub

    Private Sub lblPart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblPart.Click

    End Sub

    Private Sub SetCount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim conf As New ConfigurationSettings
        pPassCode = conf.Appsettings("//Properties/AdminCode")

        Me.AcceptButton = OK
    End Sub

    Protected Overrides Sub Finalize()

        MyBase.Finalize()
    End Sub
End Class
