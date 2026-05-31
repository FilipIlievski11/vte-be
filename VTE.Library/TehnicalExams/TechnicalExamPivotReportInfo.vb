
<Serializable()> _
Public Class TechnicalExamPivotReportInfo
    Inherits ReadOnlyBase(Of TechnicalExamPivotReportInfo)

#Region " Business Properties and Methods "

    Private _idtechexam As Long
    Public ReadOnly Property Idtechexam() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idtechexam
        End Get
    End Property

    Private _regnumber As String
    Public ReadOnly Property Regnumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _regnumber
        End Get
    End Property

    Private _madedate As Date
    Public ReadOnly Property Madedate() As Date
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _madedate
        End Get
    End Property

    Private _validtilldate As Date
    Public ReadOnly Property Validtilldate() As Date
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _validtilldate
        End Get
    End Property

    Private _idfirscontroler As Integer
    Public ReadOnly Property Idfirscontroler() As Integer
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idfirscontroler
        End Get
    End Property

    Private _idsecondcontroler As Integer
    Public ReadOnly Property Idsecondcontroler() As Integer
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idsecondcontroler
        End Get
    End Property

    Private _vehicleisright As Boolean
    Public ReadOnly Property Vehicleisright() As Boolean
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _vehicleisright
        End Get
    End Property

    Private _explanationnote As String
    Public ReadOnly Property Explanationnote() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _explanationnote
        End Get
    End Property

    Private _driverswarning As String
    Public ReadOnly Property Driverswarning() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _driverswarning
        End Get
    End Property

    Private _note As String
    Public ReadOnly Property Note() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _note
        End Get
    End Property

    Private _description As String
    Public ReadOnly Property Description() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _description
        End Get
    End Property

    Private _shellnumber As String
    Public ReadOnly Property Shellnumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _shellnumber
        End Get
    End Property

    Private _idvehicle As Long
    Public ReadOnly Property Idvehicle() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idvehicle
        End Get
    End Property

    Protected Overrides Function GetIdValue() As Object
        Return _idtechexam
    End Function

    Private _lastRegistrationNumber As String
    Public ReadOnly Property LastRegistration() As String
        Get
            Return _lastRegistrationNumber
        End Get
    End Property

  Private _vehiclePart As String
  Public ReadOnly Property VehiclePart() As String
    Get
      Return _vehiclePart
    End Get
  End Property

    Public ReadOnly Property VehicleIsRightMK() As String
        Get
            If _vehicleisright Then
                Return "ДА"
            Else
                Return "НЕ"
            End If
        End Get
  End Property

  Public ReadOnly Property VehicleIsRightMK2() As String
    Get
      If _vehicleisright AndAlso _vehiclePart = "-" Then
        Return "ДА"
      Else
        Return "НЕ"
      End If
    End Get
  End Property
    Private _firstControler As String
    Public ReadOnly Property FirstControler() As String
        Get
            Return _firstControler
        End Get
    End Property
    Private _secondControler As String
    Public ReadOnly Property SecondControler() As String
        Get
            Return _secondControler
        End Get
    End Property
    Private _CustomerSurname As String
    Public ReadOnly Property CustomerSurname() As String
        Get
            Return _CustomerSurname
        End Get
    End Property
    Private _CustomerFirstName As String
    Public ReadOnly Property CustomerFirstName() As String
        Get
            Return _CustomerFirstName
        End Get
    End Property
    Public ReadOnly Property CustomerFullName() As String
        Get
            Return _CustomerFirstName & " " & _CustomerSurname
        End Get
    End Property


    Private _BodytypeCode As String
    Public ReadOnly Property BodytypeCode() As String
        Get
            Return _BodytypeCode
        End Get
    End Property
    Private _BodytypeDescriprion As String
    Public ReadOnly Property BodytypeDescriprion() As String
        Get
            Return _BodytypeDescriprion
        End Get
    End Property
    Private _CategoryCode As String
    Public ReadOnly Property CategoryCode() As String
        Get
            Return _CategoryCode
        End Get
    End Property
    Private _CategoryName As String
    Public ReadOnly Property CategoryName() As String
        Get
            Return _CategoryName
        End Get
    End Property
    Public ReadOnly Property Category() As String
        Get
            Return _CategoryCode & "-" & _CategoryName
        End Get
    End Property
    Public ReadOnly Property Bodytype() As String
        Get
            Return _BodytypeCode & "-" & _BodytypeDescriprion
        End Get
    End Property
#End Region

#Region " Factory Methods "

    Friend Shared Function GetTechnicalExamPivotReportInfo(ByVal dr As SafeDataReader) As TechnicalExamPivotReportInfo
        Return New TechnicalExamPivotReportInfo(dr)
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        Fetch(dr)
    End Sub


#End Region

#Region " Data Access - Fetch "

    Private Sub Fetch(ByVal dr As SafeDataReader)
        _idtechexam = dr.GetInt64("IdTechExam")
        _regnumber = dr.GetString("RegNumber")
        _madedate = dr.GetDateTime("MadeDate")
        _validtilldate = dr.GetDateTime("ValidTillDate")
        _idfirscontroler = dr.GetInt32("IdFirsControler")
        _idsecondcontroler = dr.GetInt32("IdSecondControler")
        _vehicleisright = dr.GetBoolean("VehicleIsRight")
        _explanationnote = dr.GetString("ExplanationNote")
        _driverswarning = dr.GetString("DriversWarning")
        _note = dr.GetString("Note")
        _description = dr.GetString("Description")
        _shellnumber = dr.GetString("ShellNumber")
        _idvehicle = dr.GetInt64("IdVehicle")
        _CustomerSurname = dr.GetString("CustomerSurname")
        _CustomerFirstName = dr.GetString("CustomerFirstName")
        'Dim regInfo As registrationBasicInfo = Vehicle.GetVehicleLastRegistration(_idvehicle)
    _lastRegistrationNumber = dr.GetString("LastRegistratinNumber") 'regInfo.RegistrationNumber
    _vehiclePart = dr.GetString("VehiclePart1") & "-" & dr.GetString("VehiclePart2")
        '_dateOfLastRegistration = regInfo.RegistrationDate
        '_lastRegistrationPlace = regInfo.RegistrationPlace
        '_lastRegistrationValidTill = regInfo.DateRegistrationValidTill
        Dim objUsersList As UsersList = Csla.ApplicationContext.LocalContext.Item("objUsersList")
        If _idfirscontroler > 0 Then
            Try
                Dim firstC As UsersInfo = objUsersList.getInfoById(_idfirscontroler)
                _firstControler = firstC.FirstName & " " & firstC.SureName
            Catch ex As Exception
                _firstControler = ""
            End Try
            
        Else
            _firstControler = ""
        End If
        If _idsecondcontroler > 0 Then
            Try
                Dim secondC As UsersInfo = objUsersList.getInfoById(_idsecondcontroler)
                _secondControler = secondC.FirstName & " " & secondC.SureName
            Catch ex As Exception
                _secondControler = ""
            End Try
           
        Else
            _secondControler = ""
        End If
        _BodytypeCode = dr.GetString("BodytypeCode")
        _BodytypeDescriprion = dr.GetString("BodytypeDescriprion")
        _CategoryCode = dr.GetString("CategoryCode")
        _CategoryName = dr.GetString("CategoryName")
    End Sub

#End Region

End Class