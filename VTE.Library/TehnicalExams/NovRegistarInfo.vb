
<Serializable()> _
Public Class NovRegistarInfo
  Inherits ReadOnlyBase(Of NovRegistarInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _result As String
  Public ReadOnly Property Result() As String
    Get
      Return _result
    End Get
  End Property
  Private _regNumber As String
  Public ReadOnly Property RegNumber() As String
    Get
      Return _regNumber
    End Get
  End Property
  Private _lastRegistratinNumber As String
  Public ReadOnly Property LastRegistratinNumber() As String
    Get
      Return _lastRegistratinNumber
    End Get
  End Property
  Private _shellNumber As String
  Public ReadOnly Property ShellNumber() As String
    Get
      Return _shellNumber
    End Get
  End Property
  Private _vehicleMaker As String
  Public ReadOnly Property VehicleMaker() As String
    Get
      Return _vehicleMaker
    End Get
  End Property
  Private _vehicleModel As String
  Public ReadOnly Property VehicleModel() As String
    Get
      Return _vehicleModel
    End Get
  End Property
  Private _categoryName As String
  Public ReadOnly Property CategoryName() As String
    Get
      Return _categoryName
    End Get
  End Property
  Private _customerName As String
  Public ReadOnly Property CustomerName() As String
    Get
      Return _customerName
    End Get
  End Property
  Private _tehnicalExamsType As String
  Public ReadOnly Property TehnicalExamsType() As String
    Get
      Return _tehnicalExamsType
    End Get
  End Property
  Private _vehicleIsRight As Boolean
  Public ReadOnly Property VehicleIsRight() As Boolean
    Get
      Return _vehicleIsRight
    End Get
  End Property
  Private _driversWarning As String
  Public ReadOnly Property DriversWarning() As String
    Get
      If _result <> String.Empty Then
        Return My.Resources.Yes & " " & _driversWarning
      Else
        Return _driversWarning
      End If
    End Get
  End Property
  Private _madeDate As Date
  Public ReadOnly Property MadeDate() As Date
    Get
      Return _madeDate
    End Get
  End Property
  Private _note As String
  Public ReadOnly Property Note() As String
    Get
      Return _note
    End Get
  End Property
  Public ReadOnly Property ZavrsenaPostapka() As String
    Get
      If _result = String.Empty Then
        If _vehicleIsRight Then
          Return My.Resources.Yes
        Else
          Return My.Resources.No
        End If
      Else
        If _vehicleIsRight Then
          Return My.Resources.No & "/" & My.Resources.Yes
        Else
          Return My.Resources.No
        End If
      End If
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("id")
    _result = dr.GetString("Result")
    _regnumber = dr.GetString("RegNumber")
    _lastregistratinnumber = dr.GetString("LastRegistratinNumber")
    _shellnumber = dr.GetString("ShellNumber")
    _vehiclemaker = dr.GetString("VehicleMaker")
    _vehiclemodel = dr.GetString("VehicleModel")
    _categoryname = dr.GetString("CategoryName")
    _customername = dr.GetString("CustomerName")
    _tehnicalexamstype = dr.GetString("TehnicalExamsType")
    _vehicleisright = dr.GetBoolean("VehicleIsRight")
    _driverswarning = dr.GetString("DriversWarning")
    _madedate = dr.GetDateTime("MadeDate")
    _note = dr.GetString("Note")
  End Sub

End Class