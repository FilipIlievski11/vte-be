
<Serializable()> _
Public Class DocumentTypesOptionsInfo
  Inherits ReadOnlyBase(Of DocumentTypesOptionsInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _idDocumentTypes As Integer
  Public ReadOnly Property IdDocumentTypes() As Integer
    Get
      Return _idDocumentTypes
    End Get
  End Property
  Private _optionName As String
  Public ReadOnly Property OptionName() As String
    Get
      Return _optionName
    End Get
  End Property
  Private _isNewRegistration As Boolean
  Public ReadOnly Property IsNewRegistration() As Boolean
    Get
      Return _isNewRegistration
    End Get
  End Property
  Private _isTehnicalExamRquired As Boolean
  Public ReadOnly Property IsTehnicalExamRquired() As Boolean
    Get
      Return _isTehnicalExamRquired
    End Get
  End Property
  Private _relationDeleted As Boolean
  Public ReadOnly Property RelationDeleted() As Boolean
    Get
      Return _relationDeleted
    End Get
  End Property
  Private _vehicleDeleted As Boolean
  Public ReadOnly Property VehicleDeleted() As Boolean
    Get
      Return _vehicleDeleted
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
    _iddocumenttypes = dr.GetInt32("IdDocumentTypes")
    _optionname = dr.GetString("OptionName")
    _isnewregistration = dr.GetBoolean("IsNewRegistration")
    _istehnicalexamrquired = dr.GetBoolean("IsTehnicalExamRquired")
    _relationdeleted = dr.GetBoolean("RelationDeleted")
    _vehicledeleted = dr.GetBoolean("VehicleDeleted")
  End Sub

End Class