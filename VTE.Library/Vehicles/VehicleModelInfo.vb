
<Serializable()> _
Public Class VehicleModelInfo
  Inherits ReadOnlyBase(Of VehicleModelInfo)

  Public ReadOnly Property MakerModel() As String
    Get
      Return _vehicleMaker & ", " & _modelName
    End Get
  End Property


  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _idVehicleMaker As Integer
  Public ReadOnly Property IdVehicleMaker() As Integer
    Get
      Return _idVehicleMaker
    End Get
  End Property

  Private _vehicleMaker As String
  Public ReadOnly Property VehicleMaker() As String
    Get
      Return _vehicleMaker
    End Get
  End Property

  Private _modelName As String
  Public ReadOnly Property ModelName() As String
    Get
      Return _modelName
    End Get
  End Property
  Private _yearOfBeginingProduction As Date
  Public ReadOnly Property YearOfBeginingProduction() As Date
    Get
      Return _yearOfBeginingProduction
    End Get
  End Property
  Private _yearOfEndingProduction As Date
  Public ReadOnly Property YearOfEndingProduction() As Date
    Get
      Return _yearOfEndingProduction
    End Get
  End Property
  Public ReadOnly Property YearOfProduction() As String
    Get
      Return YearOfBeginingProduction.Year & "-" & YearOfEndingProduction.Year
    End Get
  End Property

  Private _idCountry As Integer
  Public ReadOnly Property IdCountry() As Integer
    Get
      Return _idCountry
    End Get
  End Property

  Private _countryName As String
  Public ReadOnly Property CountryName() As String
    Get
      Return _countryName
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
    _idvehiclemaker = dr.GetInt32("IdVehicleMaker")
    ' _idbodytypedefault = dr.GetInt32("IdBodytypeDefault")
    _modelname = dr.GetString("ModelName")
    _yearofbeginingproduction = dr.GetDateTime("YearOfBeginingProduction")
    _yearOfEndingProduction = dr.GetDateTime("YearOfEndingProduction")
    _vehicleMaker = dr.GetString("VehicleMaker")
    _idCountry = dr.GetInt32("IdCountry")
    _countryName = dr.GetString("CountryName")
  End Sub

End Class