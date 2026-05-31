<Serializable()> _
Public Class CustomerFinanceDepInfo
 Inherits ReadOnlyBase(Of CustomerFinanceDepInfo)

 Private _id As Long
 Public ReadOnly Property Id() As Long
  Get
   Return _id
  End Get
 End Property

 Private _idCustomerVehicleRelation As Long
 Public ReadOnly Property IdCustomerVehicleRelation() As Long
  Get
   Return _idCustomerVehicleRelation
  End Get
 End Property

 Private _idDocument As Long
 Private _idDocumentTehnicalExam As Long
 Private _idDocumentsTrafficLicences As Long
 Private _idDocumentIternationalDriveingLicence As Long
 Private _idDocumentPermisions As Long
 Private _idPriceCatalog As Integer
 Public ReadOnly Property IdPriceCatalog() As Integer
  Get
   Return _idPriceCatalog
  End Get
 End Property
 Private _note As String
 Public ReadOnly Property Note() As String
  Get
   Return _note
  End Get
 End Property
 Private _price As Decimal
 Public ReadOnly Property Price() As Decimal
  Get
   Return _price
  End Get
 End Property
 Private _payed As Boolean
 Public ReadOnly Property Payed() As Boolean
  Get
   Return _payed
  End Get
 End Property

 Public ReadOnly Property VehicleDisplay() As String
  Get
   If _idVehicle = 0 Then
    Return My.Resources.Nema
   Else
    Return _shellNumber & " (" & _vehicleMaker & ", " & _vehicleModel & ") " & _vehicleLastRegistrationNumber
   End If

  End Get
 End Property

 Public ReadOnly Property CustomerDisplay() As String
  Get
   If _customerSurname = String.Empty Then
    Return _customerFirstName
   Else
    Return _customerSurname & ", " & _customerFirstName
   End If
  End Get
 End Property

 Private _idCustomer As Integer
 Private _customerSurname As String
 Private _customerFirstName As String
 Private _idVehicle As Integer


 Private _shellNumber As String
 Private _vehicleModel As String
 Private _vehicleMaker As String


 Private _vehicleLastRegistrationNumber As String
 Public ReadOnly Property VehicleLastRegistrationNumber() As String
  Get
   Return _vehicleLastRegistrationNumber
  End Get
 End Property

 Private _vehicleLastRegistrationDate As Date
 Public ReadOnly Property VehicleLastRegistrationDate() As Date
  Get
   Return _vehicleLastRegistrationDate
  End Get
 End Property



 Protected Overrides Function GetIdValue() As Object
  Return _id
 End Function

 Public Overrides Function ToString() As String
  Return _id
 End Function
#Region " Factory Methods "

 Friend Shared Function GetCustomerFinanceDepInfo(ByVal dr As SafeDataReader) As CustomerFinanceDepInfo
  Return New CustomerFinanceDepInfo(dr)
 End Function

 Private Sub New(ByVal dr As SafeDataReader)
  Fetch(dr)
 End Sub


#End Region
 Friend Sub Fetch(ByVal dr As SafeDataReader)
  _id = dr.GetInt64("Id")
  _idCustomerVehicleRelation = dr.GetInt64("IdCustomerVehicleRelation")
  _idDocument = dr.GetInt64("IdDocument")
  _idDocumentTehnicalExam = dr.GetInt64("IdDocumentTehnicalExam")
  _idDocumentsTrafficLicences = dr.GetInt64("IdDocumentsTrafficLicences")
  _idDocumentIternationalDriveingLicence = dr.GetInt64("IdDocumentIternationalDriveingLicence")
  _idDocumentPermisions = dr.GetInt64("IdDocumentPermisions")

  _idVehicle = dr.GetInt64("IdVehicle")
  If _idVehicle = 0 Then
   _shellNumber = My.Resources.Nema
   _vehicleModel = My.Resources.Nema
   _vehicleMaker = My.Resources.Nema

   _vehicleLastRegistrationNumber = My.Resources.Nema
   _vehicleLastRegistrationDate = DateTime.MinValue

  Else
   _shellNumber = dr.GetString("ShellNumber")
   _vehicleModel = dr.GetString("VehicleModel")
   _vehicleMaker = dr.GetString("VehicleMaker")
   ' Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_id)
   '_vehicleLastRegistrationNumber = regInfo.RegistrationNumber
   '_vehicleLastRegistrationDate = regInfo.RegistrationDate
   _vehicleLastRegistrationNumber = dr.GetString("LastRegistratinNumber")
   _vehicleLastRegistrationDate = dr.GetDateTime("LastRegistrationMakeDate")

  End If

  _idCustomer = dr.GetInt64("IdCustomer")
  _customerSurname = dr.GetString("CustomerSurname")
  _customerFirstName = dr.GetString("CustomerFirstName")

  _idPriceCatalog = dr.GetInt32("IdPriceCatalog")

  _note = dr.GetString("Note")
  _price = dr.GetDecimal("Price")
  _payed = dr.GetBoolean("Payed")
 End Sub

End Class
