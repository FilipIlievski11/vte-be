Module MainModul
 Public WithEvents uSettings As UserSettings
 Public WithEvents objOpcii As Options
 ' Public WithEvents objVehicleListShort As VehiclesListShortListAll
 Public WithEvents objUsersList As UsersList
 Public WithEvents objUsersListByStation As UsersList
 Public WithEvents objCurentUser As UsersInfo
 Public WithEvents objPaymentCatalogList As PaymentCataologList
 Public WithEvents objDDVList As DDVList 'stavi reset vo forma
 Public WithEvents objTehnicalExamsTypesList As TehnicalExamsTypesList
 Public WithEvents objRequestTypeList As RequestTypeList
 Public WithEvents objVehicleEngineEcoProgramList As VehicleEngineEcoProgramList
 Public WithEvents objPaymentTypeList As PaymentTypeList
 Public WithEvents objCommunityList As CommunitiesList
 'Public WithEvents objCustomersListShort As CustomersListShort
 'Public WithEvents objRlationList As CustomerVehiclesRelationsList
 'Public WithEvents objPaymentCategories As PaymentCategories
 Public WithEvents objTehExamOrganizations As TehnicalExamOrganizationsList
 Public WithEvents objCurentTehExamOrganization As TehnicalExamOrganizationsInfo
 Public WithEvents objVehicleCategoryForPaymentsList As VehicleCategoryForPaymentsList
 Public WithEvents objVehicleFieldList As VehicleFieldList
 'Public WithEvents objCalculationItemList As CalculationItemList
 Public WithEvents objVehicleUseList As VehicleUseList
 Public WithEvents objRegistrationIssuerList As RegistrationIssuerList
 Public WithEvents objVehicleSupportingList As VehicleSupportingList
 Public WithEvents objVehicleGearBoxList As VehicleGearBoxList
 Public WithEvents objVehicleBrakesList As VehicleBrakesList
 Public WithEvents objVehicleEnginePowerSourceTypeList As VehicleEnginePowerSourceTypeList
 Public WithEvents objCountriesList As CountriesList
 Public WithEvents objCityList As CityList
 Public WithEvents objColorsList As ColorsList
 Public WithEvents objVehicleCategoryList As VehicleCategoryList
 Public WithEvents objVehicleBodytypeList As VehicleBodytypeList
 Public WithEvents objOwnershipProofList As DocumentVehicleOwnershipProofList
 Public WithEvents objPaymentProofList As DocumentPaymentProofList
 Public WithEvents objDriveingLicenceCtegoryList As DriveingLicenceCtegoryList
  Public WithEvents objCompanyList As CompanyList
  Public WithEvents objVehicleParts As TehnicalExamVehiclePartsList
 'za brisenje


 Public insMainForm As MainForm

 Sub main()
  '  objOpcii = Options.GetOptions
  'Csla.ApplicationContext.LocalContext.Add("objOpcii", objOpcii)
  'MsgBox("se pusta")

  'Using sp As New Splash("Проверувам конекции")
  '  SetupConnectionStrings()
  'End Using
  'Login.ShowDialog()

  Try
    uSettings = UserSettings.GetUserSettings
  Catch
  End Try
  Try
    ChangeCulture(If(uSettings IsNot Nothing, uSettings.Culture, "mk-MK"))
  Catch
  End Try

  Try
    insMainForm = New MainForm
  Catch ex As Exception
    MsgBox("MainForm constructor error: " & ex.ToString())
    End
  End Try
  Try
   insMainForm.ShowDialog()
  Catch ex As Exception
   System.IO.File.WriteAllText(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "oldapp-error.txt"), ex.ToString())
   MsgBox("Error written to Desktop\oldapp-error.txt")
  End Try

  End

 End Sub


 Public Sub Kraj()
  End
 End Sub


 Public Sub SetupConnectionStrings()

  Dim cry As New Crypt("SecurityConnection")
  Dim bOpen As Boolean = False
  Dim strConn As String = ""
  Dim I As Integer = 1

  'tuka stavi gi i enkriptiraj gi
  Try
   Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath)
   'Load the file info
   Dim xml = XElement.Load(config.FilePath)
   'Get the first config section (first connection string info)
   'Dim connStrXML = xml.Descendants("connectionStrings").Elements().First
   ''Modify the existing connection string information
   'connStrXML.SetAttributeValue("connectionString", cry.Encrypt("Data Source=mainframe;Initial Catalog=PayrollSecurity;Integrated Security=false;User ID=sa;PWD=1_samsung"))

   'cry = New Crypt("VTEConnection")
   'connStrXML = xml.Descendants("connectionStrings").Elements().Last()
   'connStrXML.SetAttributeValue("connectionString", cry.Encrypt("Data Source=mainframe;Initial Catalog=VTE;Integrated Security=false;User ID=sa;PWD=1_samsung"))


   'Saving config at the same place
   xml.Save(config.FilePath)

   'Dim oSection As ConnectionStringsSection = oConf.GetSection("connectionStrings")
   'oSection.ConnectionStrings("SecurityConnection").ConnectionString = cry.Encrypt("Data Source=.\SQLEXPRESS;Initial Catalog=PayrollSecurity;Integrated Security=True")
   'cry = New Crypt("CompactPayrollConnection")
   'oSection.ConnectionStrings("CompactPayroll").ConnectionString = cry.Encrypt("Data Source=.\SQLEXPRESS;Initial Catalog=PayrollSecurity;Integrated Security=True")


   'oConf.Save(ConfigurationSaveMode.Full)

  Catch ex As Exception
   MsgBox(ex.Message)
  End Try

  'ConfigurationManager.ConnectionStrings.


 End Sub

 Public Sub ChangeCulture(ByVal strCulture As String)
  Dim cul As New System.Globalization.CultureInfo(strCulture)
  System.Threading.Thread.CurrentThread.CurrentCulture = cul
  System.Threading.Thread.CurrentThread.CurrentUICulture = cul
  My.Resources.Culture = cul
 End Sub

End Module
