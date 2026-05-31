Imports System
Namespace MyXtraGrid
  Public Class MyGridView
    Inherits DevExpress.XtraGrid.Views.Grid.GridView
    Public Sub New()
      Me.New(Nothing)
    End Sub
    Public Sub New(ByVal grid As DevExpress.XtraGrid.GridControl)
      ' put your initialization code here
      MyBase.New(grid)
    End Sub
    Protected Overloads Overrides ReadOnly Property ViewName() As String
      Get
        Return "MyGridView"
      End Get
    End Property
    Protected Overloads Overrides Sub SetFilterRowValue(ByVal column As DevExpress.XtraGrid.Columns.GridColumn, ByVal _value As Object)
      '_value <> DBNull.Value AndAlso
      If _value IsNot Nothing AndAlso column.OptionsFilter.AutoFilterCondition <> DevExpress.XtraGrid.Columns.AutoFilterCondition.Equals Then
        If _value.ToString().Length > 0 AndAlso _value.ToString()(0) <> "%"c Then
          _value = "%" & _value.ToString()
        End If
      End If
      MyBase.SetFilterRowValue(column, _value)
    End Sub

    Protected Overloads Overrides Function GetFilterRowValue(ByVal column As DevExpress.XtraGrid.Columns.GridColumn) As Object
      Dim value As Object = MyBase.GetFilterRowValue(column)
      If column.OptionsFilter.AutoFilterCondition <> DevExpress.XtraGrid.Columns.AutoFilterCondition.Equals Then
        If TypeOf value Is String Then
          value = value.ToString().Replace("%", "")
        End If
      End If
      Return value
    End Function


  End Class
End Namespace
