Imports System
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Base.Handler
Imports DevExpress.XtraGrid.Views.Base.ViewInfo
Imports DevExpress.XtraGrid.Registrator

Namespace MyXtraGrid
  Public Class MyGridViewInfoRegistrator
    Inherits GridInfoRegistrator
    Public Overloads Overrides ReadOnly Property ViewName() As String
      Get
        Return "MyGridView"
      End Get
    End Property
    Public Overloads Overrides Function CreateView(ByVal grid As GridControl) As BaseView
      Return New MyGridView(TryCast(grid, GridControl))
    End Function
  End Class
End Namespace