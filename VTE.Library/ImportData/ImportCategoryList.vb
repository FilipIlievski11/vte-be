Imports System.IO

<Serializable()> _
Public Class ImportCategoryList


  Inherits ReadOnlyListBase(Of ImportCategoryList, ImportCategoryInfo)

  Public Shared Function GetImportCategoryList(ByVal table As System.Data.DataTable) As ImportCategoryList
    Return DataPortal.Fetch(Of ImportCategoryList)(New FilterCriteria(table))
  End Function

  Private Sub New()

  End Sub

  <Serializable()> _
Private Class FilterCriteria
    Public _table As System.Data.DataTable

    Public Sub New(ByVal table As System.Data.DataTable)
      _table = table
    End Sub
  End Class

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As FilterCriteria)
    RaiseListChangedEvents = False
    IsReadOnly = False

    Using sr As System.Data.DataTableReader = New DataTableReader(criteria._table) '= New StreamReader(criteria._line)
      Dim poRed As DataRow = criteria._table.Rows.Item(0)
      Dim i As Integer = 0
      While poRed IsNot Nothing
        Me.Add(ImportCategoryInfo.GetPacientiInfo(poRed))
        i += 1
        poRed = criteria._table.Rows.Item(i)
      End While
    
      sr.Close()
    End Using

    IsReadOnly = True
    RaiseListChangedEvents = True


  End Sub
End Class

