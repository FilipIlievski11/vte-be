Public Class rptBaranjeZelen
  Private WithEvents _stampaDocument As DocumentTypesOptionsInfo
  Private WithEvents _stampaVehicle As VehicleInfo
  Private WithEvents _stampaCustomer As CustomersInfo
  Public Sub New(ByVal inDocument As DocumentTypesOptionsInfo, ByVal inVehicle As VehicleInfo, ByVal inCustomer As CustomersInfo)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    _stampaDocument = inDocument
    Me.BindingSourceDocumentTypeOptions.DataSource = _stampaDocument
    _stampaCustomer = inCustomer
    _stampaVehicle = inVehicle

    Me.BindingSourceVehicle.DataSource = _stampaVehicle
    Me.BindingSourceCustomer.DataSource = _stampaCustomer
    Me.BindingSourceBodyType.DataSource = VehicleBodytypeList.GetVehicleBodytypeList
    Me.BindingSourceVehicleModel.DataSource = VehicleModelList.GetVehicleModelList
    Me.BindingSourceColors.DataSource = ColorsList.GetColorsList
    Me.BindingSourceEngineType.DataSource = VehicleEngineTypeList.GetVehicleEngineTypeList
  End Sub

End Class