
<Serializable()> _
Public Class CustomerVehicleRelationTypeInfo
  Inherits ReadOnlyBase(Of CustomerVehicleRelationTypeInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _relationTypeName As String
  Public ReadOnly Property RelationTypeName() As String
    Get
      Return _relationTypeName
    End Get
  End Property
  Private _isCustomerOnly As Boolean
  Public ReadOnly Property IsCustomerOnly() As Boolean
    Get
      Return _isCustomerOnly
    End Get
  End Property
  Private _isOwner As Boolean
  Public ReadOnly Property IsOwner() As Boolean
    Get
      Return _isOwner
    End Get
  End Property
  Private _isAuthorized As Boolean
  Public ReadOnly Property IsAuthorized() As Boolean
    Get
      Return _isAuthorized
    End Get
  End Property
  Private _relationDescription As String
  Public ReadOnly Property RelationDescription() As String
    Get
      Return _relationDescription
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
    _relationtypename = dr.GetString("RelationTypeName")
    _iscustomeronly = dr.GetBoolean("IsCustomerOnly")
    _isowner = dr.GetBoolean("IsOwner")
    _isauthorized = dr.GetBoolean("IsAuthorized")
    _relationdescription = dr.GetString("RelationDescription")
  End Sub

End Class