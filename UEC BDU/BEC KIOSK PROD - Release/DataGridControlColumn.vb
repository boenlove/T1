Imports System
Imports System.Data
Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel
Imports System.Reflection

' A DataGridColumnStyle-derived class that can host any control.

Public Class DataGridControlColumn
    Inherits DataGridColumnStyle

    ' This is the control hosted in this column style.
    Private ctrl As Control
    ' The bound property
    Private boundProperty As PropertyInfo
    ' The event that fires when the bound property changes.
    Private boundPropertyChangedEvent As EventInfo

    ' preferred size of this column
    Private preferredWidth As Integer
    Private preferredHeight As Integer
    Private minimumHeight As Integer

    ''Dim timePicker As New DateTimePicker

    ' This is True if the user is editing the hosted control.
    Private isEditing As Boolean

    ' The constructor 

    Public Sub New(ByVal ctrlType As Type, ByVal boundPropertyName As String, ByVal boundEventName As String, ByVal preferredWidth As Integer, ByVal preferredHeight As Integer, ByVal minimumHeight As Integer)
        ' Create an instance of the specified control.
        Me.ctrl = CType(Activator.CreateInstance(ctrlType), Control)
        ctrl.Visible = False

        ' Get a reference to the PropertyInfo describing the bound property.
        Me.boundProperty = ctrlType.GetProperty(boundPropertyName)
        If Me.boundProperty Is Nothing Then
            Throw New ArgumentException("Invalid bound property name")
        End If

        ' if the event name is nothing or null string, derive the event
        ' name from the bound property name
        If boundEventName Is Nothing OrElse boundEventName.Length = 0 Then
            boundEventName = boundPropertyName & "Changed"
        End If

        ' Get a reference to the event that fires when the bound property changes
        Me.boundPropertyChangedEvent = ctrlType.GetEvent(boundEventName)
        If Me.boundPropertyChangedEvent Is Nothing Then
            Throw New ArgumentException("Invalid bound event name")
        End If

        ' Remember preferred size
        Me.preferredWidth = preferredWidth
        Me.preferredHeight = preferredHeight
        Me.minimumHeight = minimumHeight
    End Sub

    ' initiates a request to interrupt an edit procedure.

    Protected Overrides Sub Abort(ByVal rowNum As Integer)
        isEditing = False
        ' Remove the event handler.
        Me.boundPropertyChangedEvent.RemoveEventHandler(ctrl, New EventHandler(AddressOf Control_PropertyChanged))
        ' Refresh the data grid.
        Invalidate()
    End Sub

    ' Initiates a request to complete an editing procedure.

    Protected Overrides Function Commit(ByVal dataSource As CurrencyManager, ByVal rowNum As Integer) As Boolean
        ctrl.Bounds = Rectangle.Empty
        ' Create an event handler for the bound property
        Me.boundPropertyChangedEvent.AddEventHandler(ctrl, New EventHandler(AddressOf Control_PropertyChanged))

        If isEditing Then
            isEditing = False
            Try
                ' Retrieve the value of the bound property.
                Dim value As Object = Me.boundProperty.GetValue(ctrl, Nothing)
                ' Use it as the value for this cell.
                SetColumnValueAtRow(dataSource, rowNum, value)
            Catch
                ' Ignore errors.
            End Try
            Invalidate()
        End If
        Return True
    End Function

    ' Prepares the cell for editing a value.

    Protected Overloads Overrides Sub Edit(ByVal dataSource As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        ' Retrieve the value from the underlying data source.
        Dim value As Object = GetColumnValueAtRow(dataSource, rowNum)
        ' Assign the value to the control.
        If IsDBNull(value) Then
            'Me.boundProperty.SetValue(ctrl, "", Nothing)
        Else
            Me.boundProperty.SetValue(ctrl, value, Nothing)
        End If

        If cellIsVisible Then
            ctrl.Bounds = New Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4)
            ' Add an event handler for the bound property.
            Me.boundPropertyChangedEvent.AddEventHandler(ctrl, New EventHandler(AddressOf Control_PropertyChanged))
            ' Repaint this portion of the datagrid.
            ctrl.Visible = True
            DataGridTableStyle.DataGrid.Invalidate(bounds)
        Else
            ctrl.Visible = True
        End If

    End Sub

    ' gets the width and height of the specified value. The width and height are used when the user navigates to DataGridTableStyle using the DataGridColumnStyle.

    Protected Overrides Function GetPreferredSize(ByVal g As Graphics, ByVal value As Object) As Size
        Return New Size(Me.preferredWidth, Me.preferredHeight)
    End Function

    ' gets the minimum height of a row.

    Protected Overrides Function GetMinimumHeight() As Integer
        Return Me.minimumHeight
    End Function

    ' gets the height used for automatically resizing columns.

    Protected Overrides Function GetPreferredHeight(ByVal g As Graphics, ByVal value As Object) As Integer
        Return Me.preferredHeight
    End Function

    ' paints the column in a System.Windows.Forms.DataGrid control.
    ' ( three overrides )

    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal dataSource As CurrencyManager, ByVal rowNum As Integer)
        Paint(g, bounds, dataSource, rowNum, False)
    End Sub

    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal dataSource As CurrencyManager, ByVal rowNum As Integer, ByVal alignToRight As Boolean)
        Paint(g, bounds, dataSource, rowNum, Brushes.Red, Brushes.Blue, alignToRight)
    End Sub

    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal dataSource As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)
        ' Retrieve the value from the data source.
        Dim value As Object = GetColumnValueAtRow(dataSource, rowNum)
        Dim rect As Rectangle = bounds

        g.FillRectangle(backBrush, rect)
        rect.Offset(0, 2)
        rect.Height -= 2
        g.DrawString(value.ToString, Me.DataGridTableStyle.DataGrid.Font, foreBrush, RectangleF.FromLTRB(rect.X, rect.Y, rect.Right, rect.Bottom))
    End Sub

    ' Sets the System.Windows.Forms.DataGrid for the column.

    Protected Overrides Sub SetDataGridInColumn(ByVal value As System.Windows.Forms.DataGrid)
        MyBase.SetDataGridInColumn(value)
        If Not (ctrl.Parent Is Nothing) Then
            ctrl.Parent.Controls.Remove(ctrl)
        End If
        If Not (value Is Nothing) Then
            value.Controls.Add(ctrl)
        End If
    End Sub

    ' This event fires when the bound property changes.

    Private Sub Control_PropertyChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.isEditing = True
        MyBase.ColumnStartedEditing(ctrl)
    End Sub

    ' This property exposes the inner control

    Public ReadOnly Property InnerControl() As Control
        Get
            Return ctrl
        End Get
    End Property
End Class

