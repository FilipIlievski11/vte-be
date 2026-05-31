Public Class ImportCategoryInfo
  Inherits ReadOnlyBase(Of ImportCategoryInfo)
  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _oldCategoryName As String
  Public ReadOnly Property OldCategoryName() As String
    Get
      Return _oldCategoryName
    End Get
  End Property
  Private _categoryCode As String
  Public ReadOnly Property CategoryCode() As String
    Get
      Return _categoryCode
    End Get
  End Property
  Private _categoryName As String
  Public ReadOnly Property CategoryName() As String
    Get
      Return _categoryName
    End Get
  End Property
  Private _mKSJUS As String
  Public ReadOnly Property MKSJUS() As String
    Get
      Return _mKSJUS
    End Get
  End Property
  Private _iSO As String
  Public ReadOnly Property ISO() As String
    Get
      Return _iSO
    End Get
  End Property

  Private _mKSJUSDescription As String
  Public ReadOnly Property MKSJUSDescription() As String
    Get
      Return _mKSJUSDescription
    End Get
  End Property
  Private _picturePath As String
  Public ReadOnly Property PicturePath() As String
    Get
      Return _picturePath
    End Get
  End Property

  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Friend Shared Function GetPacientiInfo(ByVal line As System.Data.DataRow) As ImportCategoryInfo
    Return New ImportCategoryInfo(line)
  End Function

  Friend Sub New(ByVal line As System.Data.DataRow)
    Fetch(line)
  End Sub

  Friend Sub New()

  End Sub
  Private Sub Fetch(ByVal line As System.Data.DataRow)
    _oldCategoryName = line.Item(0)
    _categoryCode = line.Item(2)
    _categoryName = line.Item(3)
    _mKSJUS = line.Item(9)
    _iSO = line.Item(10)
    _mKSJUSDescription = line.Item(11)
    _picturePath = ""
  End Sub
End Class
