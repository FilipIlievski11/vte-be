
<Serializable()> _
Public Class DocumentTypesOptionsDetailsInfo
  Inherits ReadOnlyBase(Of DocumentTypesOptionsDetailsInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _idDocumentTypesOptions As Integer
  Public ReadOnly Property IdDocumentTypesOptions() As Integer
    Get
      Return _idDocumentTypesOptions
    End Get
  End Property
  Private _name As String
  Public ReadOnly Property Name() As String
    Get
      Return _name
    End Get
  End Property
  Private _isVehicleDeleted As Boolean
  Public ReadOnly Property IsVehicleDeleted() As Boolean
    Get
      Return _isVehicleDeleted
    End Get
  End Property
  Private _isNewCustomer As Boolean
  Public ReadOnly Property IsNewCustomer() As Boolean
    Get
      Return _isNewCustomer
    End Get
  End Property
  Private _isRelationDeleted As Boolean
  Public ReadOnly Property IsRelationDeleted() As Boolean
    Get
      Return _isRelationDeleted
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
    _iddocumenttypesoptions = dr.GetInt32("IdDocumentTypesOptions")
    _name = dr.GetString("Name")
    _isVehicleDeleted = dr.GetBoolean("IsVehicleDeleted")
    _isNewCustomer = dr.GetBoolean("IsNewCustomer")
    _isRelationDeleted = dr.GetBoolean("IsRelationDeleted")
  End Sub

End Class