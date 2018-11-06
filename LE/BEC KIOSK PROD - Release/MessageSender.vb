Public Class MessageSender
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents txtComments As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnSend = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtComments
        '
        Me.txtComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.Location = New System.Drawing.Point(16, 32)
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.Size = New System.Drawing.Size(472, 216)
        Me.txtComments.TabIndex = 0
        Me.txtComments.Text = ""
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(496, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(56, 24)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "&Close"
        '
        'btnSend
        '
        Me.btnSend.Location = New System.Drawing.Point(496, 72)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(56, 24)
        Me.btnSend.TabIndex = 2
        Me.btnSend.Text = "Send"
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(496, 40)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(56, 24)
        Me.btnClear.TabIndex = 3
        Me.btnClear.Text = "Clear"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(152, 24)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Message..."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'MessageSender
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(560, 270)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtComments)
        Me.Name = "MessageSender"
        Me.Text = "MessageSender"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        If MsgBox("Erase Message?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            txtComments.Text = ""
        End If
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click

        Try
            If Len(Trim(txtComments.Text)) > 0 Then
                If MsgBox("Are you sure you want to send this message to PC&L?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Exit Sub
                End If

                'David Maibor (DSM) select DEV/PRODUCTION version as appropriate
                'Dim webref As New localhostRequest.RequestLabel  'DSM: DEV version
                'Dim greeting As New localhostRequest.Cordial 'DSM: DEV version
                Dim webref As New dorme2.RequestLabel  'DSM: PRODUCTION version
                Dim greeting As New dorme2.Cordial 'DSM: PRODUCTION version

                btnSend.Enabled = False
                greeting.Password = "forgetaboutit"
                greeting.UserID = "delphikiosk"
                webref.CordialValue = greeting
                webref.SendEmail("Message from BEC SPL labeling", "normal", txtComments.Text, "PCL")
                MsgBox("Message Sent")
                Me.Close()
            End If
        Catch ex As Exception
            MsgBox("Failed to Send message: >>" & ex.Message)
        End Try
    End Sub
End Class
