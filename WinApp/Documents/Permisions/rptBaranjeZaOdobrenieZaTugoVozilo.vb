Public Class rptBaranjeZaOdobrenieZaTugoVozilo
  '  Private WithEvents _printDocPermission As PrintDocumentPermisionForCustomerList
  'Private WithEvents _relationOwner As CustomerVehiclesRelation
  'Public Sub New(ByVal inIdDocPermission As Long)

  '  ' This call is required by the Windows Form Designer.
  '  InitializeComponent()


  '  _printDocPermission = PrintDocumentPermisionForCustomerList.GetPrintDocumentPermisionForCustomerList(inIdDocPermission)
  '  Me.BindingSourcePrintDocumentPermissionById.DataSource = _printDocPermission
  '  _relationOwner = CustomerVehiclesRelation.GetCustomerVehiclesRelation(_printDocPermission.Item(0).IdCustomerVehicleRelationOwner)
  '  lblOwner.Text = CustomersList.GetCustomersList.GetCustomersListById(_relationOwner.IdCustomer).Name
  '  lblOdgovorenOrgan.Text = objOpcii.OdgovorenOrgan
  '  ' Add any initialization after the InitializeComponent() call.

  '  Try
  '    lblRegNum.Text = Vehicle.GetVehicle(_relationOwner.IdVehicle).LastRegistation
  '  Catch ex As Exception
  '    MessageBox.Show("Возилото не е регистрирано")
  '  End Try
  'End Sub

  Private WithEvents _printDocPermission As printPermisionList
  'Private WithEvents _relationOwner As CustomerVehiclesRelation
  Public Sub New(ByVal inIdDocPermission As Long)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    Me.PrintingSystem.ShowMarginsWarning = False

    _printDocPermission = printPermisionList.GetprintPermisionList(inIdDocPermission)
    Me.BindingSourcePrintDocumentPermissionById.DataSource = _printDocPermission
    '_relationOwner = CustomerVehiclesRelation.GetCustomerVehiclesRelation(_printDocPermission.Item(0).IdCustomerVehicleRelationOwner)
    'lblOwner.Text = CustomersList.GetCustomersList.GetCustomersListById(_relationOwner.IdCustomer).Name
    lblOdgovorenOrgan.Text = objTehExamOrganizations.GetTehnicalExamOrganizationsInfoById(objCurentTehExamOrganization.Id).OrganizationAndStationName
    ' Add any initialization after the InitializeComponent() call.

    Try
      Dim pomCustomer As CustomersInfo = CustomersList.GetCustomersList.GetCustomersListById(_printDocPermission.Item(0).IdCustomer)
      lblAdresa.Text = (StreetsList.GetStreetsList.GetStreetInfo(pomCustomer.IdLivingAddress).StreetName & " " _
      & pomCustomer.LivingAddressNumber & " " & CityList.GetCityList.GetCityListById(pomCustomer.IdLivingCity).CityName)
      ' lblRegNum.Text = Vehicle.GetVehicle(_relationOwner.IdVehicle).LastRegistation
    Catch ex As Exception
      MessageBox.Show("Возилото не е регистрирано")
    End Try
  End Sub
End Class