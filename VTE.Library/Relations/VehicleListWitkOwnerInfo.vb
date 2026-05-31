
<Serializable()> _
Public Class VehicleListWitkOwnerInfo
  Inherits ReadOnlyBase(Of VehicleListWitkOwnerInfo)

  Private _lastRegistrationNumber As String
  Public ReadOnly Property LastRegistration() As String
    Get
      Return _lastRegistrationNumber
    End Get
  End Property
  Private _customerSurname As String
  Public ReadOnly Property CustomerSurname() As String
    Get
      Return _customerSurname
    End Get
  End Property
  Private _customerFirstName As String
  Public ReadOnly Property CustomerFirstName() As String
    Get
      Return _customerFirstName
    End Get
  End Property
  Public ReadOnly Property Customer() As String
    Get
      Return _customerFirstName & " " & _customerSurname
    End Get
  End Property
  Private _id As Long
  Public ReadOnly Property IdVehicle() As Long
    Get
      Return _id
    End Get
  End Property
  Private _relationId As Long
  Public ReadOnly Property RelationId() As Long
    Get
      Return _relationId
    End Get
  End Property

  Private _mb As String
  Public ReadOnly Property MB() As String
    Get
      Return _mb
    End Get
  End Property
  Private _engineNumber As String
  Public ReadOnly Property EngineNumber() As String
    Get
      Return _engineNumber
    End Get
  End Property

  Private _shellNumber As String
  Public ReadOnly Property ShellNumber() As String
    Get
      Return _shellNumber
    End Get
  End Property
  Private _modelName As String
  Public ReadOnly Property ModelName() As String
    Get
      Return _modelName
    End Get
  End Property
  Private _companyName As String
  Public ReadOnly Property CompanyName() As String
    Get
      Return _companyName
    End Get
  End Property
  Private _companyTrademark As String
  Public ReadOnly Property CompanyTrademark() As String
    Get
      Return _companyTrademark
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _customersurname
  End Function

  Public Overrides Function ToString() As String
    Return _customersurname
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _customersurname = dr.GetString("CustomerSurname")
    _customerfirstname = dr.GetString("CustomerFirstName")
    _mb = dr.GetString("MB")
    _enginenumber = dr.GetString("EngineNumber")
    _shellnumber = dr.GetString("ShellNumber")
    _modelname = dr.GetString("ModelName")
    _companyname = dr.GetString("CompanyName")
    _companyTrademark = dr.GetString("CompanyTrademark")
    _id = dr.GetInt64("Id")
    _relationId = dr.GetInt64("RelationId")
        ' Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_id)
        _lastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
    
  End Sub

End Class