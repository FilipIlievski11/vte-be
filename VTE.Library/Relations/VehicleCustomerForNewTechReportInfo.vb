
<Serializable()> _
Public Class VehicleCustomerForNewTechReportInfo
    Inherits ReadOnlyBase(Of VehicleCustomerForNewTechReportInfo)

#Region " Business Properties and Methods "

    Private _id As Long
    Public ReadOnly Property Id() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _id
        End Get
    End Property

    Private _registrationnumber As String
    Public ReadOnly Property RegistrationNumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _registrationnumber
        End Get
    End Property

    Private _lastregistrationdate As DateTime
    Public ReadOnly Property LastRegistrationDate() As DateTime
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _lastregistrationdate
        End Get
    End Property

    Private _shellnumber As String
    Public ReadOnly Property ShellNumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _shellnumber
        End Get
    End Property

    Private _companyname As String
    Public ReadOnly Property CompanyName() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _companyname
        End Get
    End Property

    Private _modelname As String
    Public ReadOnly Property ModelName() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _modelname
        End Get
    End Property

    Private _customersurname As String
    Public ReadOnly Property CustomerSurname() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _customersurname
        End Get
    End Property

    Private _customerfirstname As String
    Public ReadOnly Property CustomerFirstName() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _customerfirstname
        End Get
    End Property

    Private _mb As String
    Public ReadOnly Property MB() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _mb
        End Get
    End Property

    Private _categorycode As String
    Public ReadOnly Property CategoryCode() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _categorycode
        End Get
    End Property

    Private _categoryname As String
    Public ReadOnly Property CategoryName() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _categoryname
        End Get
    End Property

    Private _iscompany As Boolean
    Public ReadOnly Property IsCompany() As Boolean
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _iscompany
        End Get
    End Property

    Private _streetname As String
    Public ReadOnly Property StreetName() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _streetname
        End Get
    End Property

    Private _livingaddressnumber As String
    Public ReadOnly Property LivingAddressNumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _livingaddressnumber
        End Get
    End Property

    Private _cityname As String
    Public ReadOnly Property CityName() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _cityname
        End Get
    End Property

    Private _yearofproduction As DateTime
    Public ReadOnly Property YearOfProduction() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return (_yearofproduction.Year).ToString
        End Get
    End Property

    Private _firstregistration As String
    Public ReadOnly Property FirstRegistration() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _firstregistration
        End Get
    End Property

    Private _firstregistrationdate As DateTime
    Public ReadOnly Property FirstRegistrationDate() As DateTime
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _firstregistrationdate
        End Get
    End Property

    Private _hologationsertificatenumber As String
    Public ReadOnly Property HologationSertificateNumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _hologationsertificatenumber
        End Get
    End Property

    Private _enginetypecode As String
    Public ReadOnly Property EngineTypeCode() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _enginetypecode
        End Get
    End Property

    Private _enginenumber As String
    Public ReadOnly Property EngineNumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _enginenumber
        End Get
    End Property
    Public ReadOnly Property EngineTypeNumber() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return (_enginetypecode & _enginenumber)
        End Get
    End Property
    Private _enginepoweroutput As Decimal
    Public ReadOnly Property EnginePowerOutPut() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _enginepoweroutput
        End Get
    End Property
    Public ReadOnly Property EnginePowerOutPutString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _enginepoweroutput & " (kW)"
        End Get
    End Property
    Public ReadOnly Property EnginePowerOutPutEmptywaight() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            If _emptywaight > 0 Then
                Return Math.Round((_enginepoweroutput / _emptywaight), 1)
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property EnginePowerOutPutEmptywaightString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            If _emptywaight > 0 Then
                Return Math.Round((_enginepoweroutput / _emptywaight), 1) & " (kW/kg)"
            Else
                Return 0 & " (kW/kg)"
            End If
        End Get
    End Property

    Private _engineworkingcapacity As Decimal
    Public ReadOnly Property EngineWorkingCapacity() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _engineworkingcapacity
        End Get
    End Property
    Public ReadOnly Property EngineWorkingCapacityString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _engineworkingcapacity & " (cm3)"
        End Get
    End Property

    Private _bodytypecode As String
    Public ReadOnly Property BodytypeCode() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _bodytypecode
        End Get
    End Property

    Private _bodytypedescriprion As String
    Public ReadOnly Property BodytypeDescriprion() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _bodytypedescriprion
        End Get
    End Property
    Public ReadOnly Property Bodytype() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _bodytypecode & "-" & _bodytypedescriprion
        End Get
    End Property

    Private _usedescription As String
    Public ReadOnly Property UseDescription() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _usedescription
        End Get
    End Property

    Private _emptywaight As Decimal
    Public ReadOnly Property EmptyWaight() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _emptywaight
        End Get
    End Property
    Public ReadOnly Property EmptyWaightString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _emptywaight & " (kg)"
        End Get
    End Property

    Private _maximunallowedwaight As Decimal
    Public ReadOnly Property MaximunAllowedWaight() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _maximunallowedwaight
        End Get
    End Property
    Public ReadOnly Property MaximunAllowedWaightString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _maximunallowedwaight & " (kg)"
        End Get
    End Property

    Private _primarycolorcode As String
    Public ReadOnly Property PrimaryColorCode() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _primarycolorcode
        End Get
    End Property

    Private _primarycolordescription As String
    Public ReadOnly Property PrimaryColorDescription() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _primarycolordescription
        End Get
    End Property

    Private _secondarycolorcode As String
    Public ReadOnly Property SecondaryColorCode() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _secondarycolorcode
        End Get
    End Property

    Private _secondarycolordescription As String
    Public ReadOnly Property SecondaryColorDescription() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _secondarycolordescription
        End Get
    End Property
    Public ReadOnly Property FullColor() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            If _secondarycolorcode IsNot Nothing AndAlso _secondarycolorcode <> String.Empty Then
                Return _primarycolorcode & "-" & _primarycolordescription _
                & "; " & _secondarycolorcode & "-" & _secondarycolordescription
            Else
                If _primarycolorcode IsNot Nothing AndAlso _primarycolorcode <> String.Empty Then
                    Return _primarycolorcode & "-" & _primarycolordescription
                Else
                    Return ""
                End If
            End If
        End Get
    End Property
    Private _numberofseats As Short
    Public ReadOnly Property NumberOfSeats() As Short
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _numberofseats
        End Get
    End Property

    Private _numberofstandingseats As Short
    Public ReadOnly Property NumberOfStandingSeats() As Short
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _numberofstandingseats
        End Get
    End Property

    Private _noisestatic As Decimal
    Public ReadOnly Property NoiseStatic() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _noisestatic
        End Get
    End Property
    Public ReadOnly Property NoiseStaticString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _noisestatic & " (dB(A))"
        End Get
    End Property

    Private _noisemovment As Decimal
    Public ReadOnly Property NoiseMovment() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _noisemovment
        End Get
    End Property
    Public ReadOnly Property NoiseMovmentString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _noisemovment & " (min-1)"
        End Get
    End Property

    Private _blackening As String
    Public ReadOnly Property Blackening() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _blackening
        End Get
    End Property
    Public ReadOnly Property BlackeningString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            If _blackening = String.Empty Then
                Return 0 & " (m-1)"
            Else
                Return _blackening & " (m-1)"
            End If

        End Get
    End Property

    Private _enginetorque As String
    Public ReadOnly Property EngineTorque() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _enginetorque
        End Get
    End Property
    Public ReadOnly Property EngineTorqueString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            If _enginetorque = String.Empty Then
                Return 0 & " (min-1)"
            Else
                Return _enginetorque & " (min-1)"
            End If

        End Get
    End Property

    Private _enginetorqueundergass As Decimal
    Public ReadOnly Property EngineTorqueUnderGass() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _enginetorqueundergass
        End Get
    End Property
    Public ReadOnly Property EngineTorqueUnderGassString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _enginetorqueundergass & " (min-1)"
        End Get
    End Property


    Private _idrelation As Long
    Public ReadOnly Property IdRelation() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idrelation
        End Get
    End Property
    Private _MaxSpeed As Decimal
    Public ReadOnly Property MaxSpeed() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _MaxSpeed
        End Get
    End Property
    Public ReadOnly Property MaxSpeedString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _MaxSpeed & " (km/h)"
        End Get
    End Property

    Private _TempOfEngineOil As Decimal
    Public ReadOnly Property TempOfEngineOil() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _TempOfEngineOil
        End Get
    End Property
    Public ReadOnly Property TempOfEngineOilString() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _TempOfEngineOil & " (oC)"
        End Get
    End Property

    Protected Overrides Function GetIdValue() As Object
        Return _id
    End Function

#End Region

#Region " Factory Methods "

    Friend Shared Function GetVehicleCustomerForNewTechReportInfo(ByVal dr As SafeDataReader) As VehicleCustomerForNewTechReportInfo
        Return New VehicleCustomerForNewTechReportInfo(dr)
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        Fetch(dr)
    End Sub


#End Region

#Region " Data Access - Fetch "

    Private Sub Fetch(ByVal dr As SafeDataReader)
        _id = dr.GetInt64("Id")
        _registrationnumber = dr.GetString("RegistrationNumber")
        _lastregistrationdate = dr.GetDateTime("LastRegistrationDate")
        _shellnumber = dr.GetString("ShellNumber")
        _companyname = dr.GetString("CompanyName")
        _modelname = dr.GetString("ModelName")
        _customersurname = dr.GetString("CustomerSurname")
        _customerfirstname = dr.GetString("CustomerFirstName")
        _mb = dr.GetString("MB")
        _categorycode = dr.GetString("CategoryCode")
        _categoryname = dr.GetString("CategoryName")
        _iscompany = dr.GetBoolean("IsCompany")
        _streetname = dr.GetString("StreetName")
        _livingaddressnumber = dr.GetString("LivingAddressNumber")
        _cityname = dr.GetString("CityName")
        _yearofproduction = dr.GetDateTime("YearOfProduction")
        _firstregistration = dr.GetString("FirstRegistration")
        _firstregistrationdate = dr.GetDateTime("FirstRegistrationDate")
        _hologationsertificatenumber = dr.GetString("HologationSertificateNumber")
        _enginetypecode = dr.GetString("EngineTypeCode")
        _enginenumber = dr.GetString("EngineNumber")
        _enginepoweroutput = dr.GetValue("EnginePowerOutPut")
        _engineworkingcapacity = dr.GetValue("EngineWorkingCapacity")
        _bodytypecode = dr.GetString("BodytypeCode")
        _bodytypedescriprion = dr.GetString("BodytypeDescriprion")
        _usedescription = dr.GetString("UseDescription")
        _emptywaight = dr.GetValue("EmptyWaight")
        _maximunallowedwaight = dr.GetValue("MaximunAllowedWaight")
        _primarycolorcode = dr.GetString("PrimaryColorCode")
        _primarycolordescription = dr.GetString("PrimaryColorDescription")
        _secondarycolorcode = dr.GetString("SecondaryColorCode")
        _secondarycolordescription = dr.GetString("SecondaryColorDescription")
        _numberofseats = dr.GetInt16("NumberOfSeats")
        _numberofstandingseats = dr.GetInt16("NumberOfStandingSeats")
        _noisestatic = dr.GetValue("NoiseStatic")
        _noisemovment = dr.GetValue("NoiseMovment")
        _blackening = dr.GetString("Blackening")
        _enginetorque = dr.GetString("EngineTorque")
        _enginetorqueundergass = dr.GetValue("EngineTorqueUnderGass")
        _idrelation = dr.GetInt64("IdRelation")
        _MaxSpeed = dr.GetValue("MaxSpeed")
        _TempOfEngineOil = dr.GetValue("TempOfEngineOil")
    End Sub

#End Region

End Class