
<Serializable()> _
Public Class PaymentCataologInfo
 Inherits ReadOnlyBase(Of PaymentCataologInfo)

 'calculate fields
 Public ReadOnly Property PaymentName() As String
  Get
   If _vehicleCategoriesForPaymantCode.Contains("?") Then
    'ako e neodredeno(Samo za poslednoto)
    Return _categoryName
   Else
    Return _categoryName & " " & _itemName & " " & _
    String.Format(_prametarName, _parametarFrom, _parametarTo)
   End If


  End Get
 End Property

 Private _idPaymentCategory As Integer
 Public ReadOnly Property IdPaymentCategory() As Integer
  Get
   Return _idPaymentCategory
  End Get
 End Property
 Private _idPaymentItem As Integer
 Public ReadOnly Property IdPaymentItem() As Integer
  Get
   Return _idPaymentItem
  End Get
 End Property
 Private _idPaymentParametar As Integer
 Public ReadOnly Property IdPaymentParametar() As Integer
  Get
   Return _idPaymentParametar
  End Get
 End Property
 Private _idDDV As Integer
 Public ReadOnly Property IdDDV() As Integer
  Get
   Return _idDDV
  End Get
 End Property
 Private _idCalculationItem As Integer
 Public ReadOnly Property IdCalculationItem() As Integer
  Get
   Return _idCalculationItem
  End Get
 End Property
 Private _IdCommunity As Integer
 Public ReadOnly Property IdCommunity() As Integer
  Get
   Return _IdCommunity
  End Get
 End Property
 Private _IdCompany As Integer
 Public ReadOnly Property IdCompany() As Integer
  Get
   Return _IdCompany
  End Get
 End Property
 Private _dDVName As String
 Public ReadOnly Property DDVName() As String
  Get
   Return _dDVName
  End Get
 End Property
 Private _dDVValue As Decimal
 Public ReadOnly Property DDVValue() As Decimal
  Get
   Return _dDVValue
  End Get
 End Property
 Private _categoryName As String
 Public ReadOnly Property CategoryName() As String
  Get
   Return _categoryName
  End Get
 End Property

 Private _AllowDiscount As Boolean
 Public ReadOnly Property AllowDiscount() As Boolean
  Get
   Return _AllowDiscount
  End Get
 End Property
 Private _trigerdByRequest As Boolean
 Public ReadOnly Property TrigerdByRequest() As Boolean
  Get
   Return _trigerdByRequest
  End Get
 End Property
 Private _trigerdByTechnicalExam As Boolean
 Public ReadOnly Property TrigerdByTechnicalExam() As Boolean
  Get
   Return _trigerdByTechnicalExam
  End Get
 End Property
 Private _trigerdByTrafficLicence As Boolean
 Public ReadOnly Property TrigerdByTrafficLicence() As Boolean
  Get
   Return _trigerdByTrafficLicence
  End Get
 End Property
 Private _trigerdByPremisionForVehicle As Boolean
 Public ReadOnly Property TrigerdByPremisionForVehicle() As Boolean
  Get
   Return _trigerdByPremisionForVehicle
  End Get
 End Property
 Private _trigerdByInternationalDrivierLicence As Boolean
 Public ReadOnly Property TrigerdByInternationalDrivierLicence() As Boolean
  Get
   Return _trigerdByInternationalDrivierLicence
  End Get
 End Property
 Private _trigerdByIrregularTechnicalExam As Boolean
 Public ReadOnly Property TrigerdByIrregularTechnicalExam() As Boolean
  Get
   Return _trigerdByIrregularTechnicalExam
  End Get
 End Property
 Private _vehicleCategoriesForPaymantCode As String
 Public ReadOnly Property VehicleCategoriesForPaymantCode() As String
  Get
   Return _vehicleCategoriesForPaymantCode
  End Get
 End Property
 Private _vehicleCategoriesForPaymantName As String
 Public ReadOnly Property VehicleCategoriesForPaymantName() As String
  Get
   Return _vehicleCategoriesForPaymantName
  End Get
 End Property
 Private _idVehicleCategoryForPayments As Integer
 Public ReadOnly Property IdVehicleCategoryForPayments() As Integer
  Get
   Return _idVehicleCategoryForPayments
  End Get
 End Property
 Private _itemName As String
 Public ReadOnly Property ItemName() As String
  Get
   Return _itemName
  End Get
 End Property
 Private _prametarName As String
 Public ReadOnly Property PrametarName() As String
  Get
   Return _prametarName
  End Get
 End Property
 Private _vehicleField As String
 Public ReadOnly Property VehicleField() As String
  Get
   Return _vehicleField
  End Get
 End Property
 Private _parametarFrom As Single
 Public ReadOnly Property ParametarFrom() As Single
  Get
   Return _parametarFrom
  End Get
 End Property
 Private _parametarTo As Single
 Public ReadOnly Property ParametarTo() As Single
  Get
   If (_parametarTo = 0) AndAlso (_parametarFrom <> 0) Then
    Return Single.MaxValue
   Else
    Return _parametarTo
   End If

  End Get
 End Property
 Private _price As Decimal
 Public ReadOnly Property Price() As Decimal
  Get
   Return _price
  End Get

 End Property
 Private _isOptional As Boolean
 Public ReadOnly Property IsOptional() As Boolean
  Get
   Return _isOptional
  End Get
 End Property
 Private _VisibleOrder As Integer
 Public ReadOnly Property VisibleOrder() As Integer
  Get
   Return _VisibleOrder
  End Get
 End Property

 Protected Overrides Function GetIdValue() As Object
  Return _idPaymentCategory
 End Function

 Public Overrides Function ToString() As String
  Return _idPaymentCategory
 End Function

 Friend Sub New(ByVal dr As SafeDataReader)
  _idPaymentCategory = dr.GetInt32("IdPaymentCategory")
  _idPaymentItem = dr.GetInt32("IdPaymentItem")
  _idPaymentParametar = dr.GetInt32("IdPaymentParametar")
  _idDDV = dr.GetInt32("IdDDV")
  _idCalculationItem = dr.GetInt32("IdCalculationItem")
  _IdCommunity = dr.GetInt32("IdCommunity")
  _IdCompany = dr.GetInt32("IdCompany")
  _dDVName = dr.GetString("DDVName")
  _dDVValue = dr.GetDecimal("DDVValue")
  _categoryName = dr.GetString("CategoryName")
  _AllowDiscount = dr.GetBoolean("AllowDiscount")
  _trigerdByRequest = dr.GetBoolean("TrigerdByRequest")
  _trigerdByTechnicalExam = dr.GetBoolean("TrigerdByTechnicalExam")
  _trigerdByTrafficLicence = dr.GetBoolean("TrigerdByTrafficLicence")
  _trigerdByPremisionForVehicle = dr.GetBoolean("TrigerdByPremisionForVehicle")
  _trigerdByInternationalDrivierLicence = dr.GetBoolean("TrigerdByInternationalDrivierLicence")
  _trigerdByIrregularTechnicalExam = dr.GetBoolean("TrigerdByIrregularTechnicalExam")
  _vehicleCategoriesForPaymantCode = dr.GetString("VehicleCategoriesForPaymantCode")
  _vehicleCategoriesForPaymantName = dr.GetString("VehicleCategoriesForPaymantName")
  _idVehicleCategoryForPayments = dr.GetInt32("IdVehicleCategoryForPayments")
  _itemName = dr.GetString("ItemName")
  _prametarName = dr.GetString("PrametarName")
  _vehicleField = dr.GetString("VehicleField")
  _parametarFrom = dr.GetValue("ParametarFrom")
  _parametarTo = dr.GetValue("ParametarTo")
  _price = dr.GetDecimal("Price")
  _isOptional = dr.GetBoolean("IsOptional")
  _VisibleOrder = dr.GetInt32("VisibleOrder")
 End Sub

End Class