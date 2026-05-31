Imports System
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Registrator

Namespace MyXtraGrid
  Public Class MyGridControl
    Inherits GridControl
    Protected Overloads Overrides Function CreateDefaultView() As BaseView
      Return CreateView("MyGridView")
    End Function
    Protected Overloads Overrides Sub RegisterAvailableViewsCore(ByVal collection As InfoCollection)
      MyBase.RegisterAvailableViewsCore(collection)
      collection.Add(New MyGridViewInfoRegistrator())
    End Sub
  End Class
End Namespace