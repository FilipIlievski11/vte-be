
<Serializable()> _
Public Class VehicleCategoriesRelationInfo
  Inherits ReadOnlyBase(Of VehicleCategoriesRelationInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _idCategory As Integer
  Public ReadOnly Property IdCategory() As Integer
    Get
      Return _idCategory
    End Get
  End Property
  Private _idBodytype As Integer
  Public ReadOnly Property IdBodytype() As Integer
    Get
      Return _idBodytype
    End Get
  End Property
  Private _idUse As Integer
  Public ReadOnly Property IdUse() As Integer
    Get
      Return _idUse
    End Get
  End Property
  Private _IdVehicleCategoryForPayments As Integer
  Public ReadOnly Property IdVehicleCategoryForPayments() As Integer
    Get
      Return _IdVehicleCategoryForPayments
    End Get
  End Property
  Private _detailDescription As String
  Public ReadOnly Property DetailDescription() As String
    Get
      Return _detailDescription
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
    _idcategory = dr.GetInt32("IdCategory")
    _idbodytype = dr.GetInt32("IdBodytype")
    _idUse = dr.GetInt32("IdUse")
    _IdVehicleCategoryForPayments = dr.GetInt32("IdVehicleCategoryForPayments")
    _detaildescription = dr.GetString("DetailDescription")
  End Sub

End Class