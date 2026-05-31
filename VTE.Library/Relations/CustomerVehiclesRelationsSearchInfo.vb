
<Serializable()> _
Public Class CustomerVehiclesRelationsSearchInfo
 Inherits ReadOnlyBase(Of CustomerVehiclesRelationsSearchInfo)

#Region " Calculated Fields "

 Private _customerDuisplay As String = String.Empty

 Public ReadOnly Property CustomerName() As String
  Get
   Return _customerDuisplay
  End Get
 End Property

#End Region

 Private _id As Long
 Public ReadOnly Property Id() As Long
  Get
   Return _id
  End Get
 End Property
 
 Private _idCustomer As Long
 Public ReadOnly Property IdCustomer() As Long
  Get
   Return _idCustomer
  End Get
 End Property

 Private _idVehicle As Long
 Public ReadOnly Property IdVehicle() As Long
  Get
   Return _idVehicle
  End Get
 End Property

 Private _customerSurname As String = String.Empty
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

 Private _mb As String
 Public ReadOnly Property MB() As String
  Get
   Return _mb
  End Get
 End Property

 Private _shellNumber As String
 Public ReadOnly Property ShellNumber() As String
  Get
   Return _shellNumber
  End Get
 End Property

 Private _lastRegistrationNumber As String
 Public ReadOnly Property LastRegistration() As String
  Get
   Return _lastRegistrationNumber
  End Get
 End Property
 Private _status As String
 Public ReadOnly Property Status() As String
  Get
   Return _status
  End Get
 End Property
 Protected Overrides Function GetIdValue() As Object
  Return _id
 End Function

 Public Overrides Function ToString() As String
  Return _id
 End Function

 Friend Sub New(ByVal dr As SafeDataReader)
  _id = dr.GetInt64("Id")
  _idCustomer = dr.GetInt64("IdCustomer")
  _idVehicle = dr.GetInt64("IdVehicle")
  _customerSurname = dr.GetString("CustomerSurname")
  _customerFirstName = dr.GetString("CustomerFirstName")
  _mb = dr.GetString("MB")
  _shellNumber = dr.GetString("ShellNumber") & " " & dr.GetString("RegistrationNumber")
  _lastRegistrationNumber = dr.GetString("RegistrationNumber")
  _status = dr.GetString("Status")
  If _customerSurname <> String.Empty Then
   _customerDuisplay = _customerSurname & " " & _customerFirstName & " " & _mb
  Else
   _customerDuisplay = _customerFirstName & " " & _mb
  End If
 End Sub


End Class
