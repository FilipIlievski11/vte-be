

<Serializable()> _
Public Class VehicleCategoryInfo
  Inherits ReadOnlyBase(Of VehicleCategoryInfo)

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

  Public ReadOnly Property Category() As String
    Get
      Return _categoryCode & "-" & _categoryName
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

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
    _oldcategoryname = dr.GetString("OldCategoryName")
    _categorycode = dr.GetString("CategoryCode")
    _categoryname = dr.GetString("CategoryName")
    _mksjus = dr.GetString("MKSJUS")
    _iso = dr.GetString("ISO")
    _mksjusdescription = dr.GetString("MKSJUSDescription")
    _picturepath = dr.GetString("PicturePath")
  End Sub

End Class
