
<Serializable()> _
Public Class UsersInfo
  Inherits ReadOnlyBase(Of UsersInfo)

  Private _id As Long
  Public ReadOnly Property ID() As Long
    Get
      Return _id
    End Get
  End Property

  Public ReadOnly Property FullName() As String
    Get
            Return _firstName & " " & _sureName
    End Get
  End Property
    Public ReadOnly Property FullNameAndStation() As String
        Get
            Dim stanica As String = ""
            If _idStation > 0 Then
                Dim listaStanici As TehnicalExamOrganizationsList = Csla.ApplicationContext.LocalContext("objTehExamOrganizations")
                stanica = listaStanici.GetTehnicalExamOrganizationsInfoById(_idStation).Station
                Return _firstName & " " & _sureName & " - " & stanica
            Else
                Return _firstName & " " & _sureName
            End If

        End Get
    End Property
  'Private _idRole As Integer
  'Public ReadOnly Property IdRole() As Integer
  '  Get
  '    Return _idRole
  '  End Get
  'End Property
    Private _idDataBase As Integer
    Public ReadOnly Property IdDataBase() As Integer
        Get
            Return _idDataBase
        End Get
    End Property
    'Private _userFullName As String
    'Public ReadOnly Property UserFullName() As String
    '  Get
    '    Return _userFullName
    '  End Get
    'End Property
    'Private _userName As String
    'Public ReadOnly Property UserName() As String
    '  Get
    '    Return _userName
    '  End Get
    'End Property
    'Private _userPass As String
    'Public ReadOnly Property UserPass() As String
    '  Get
    '    Return _userPass
    '  End Get
    'End Property
    Private _firstName As String
    Public ReadOnly Property FirstName() As String
        Get
            Return _firstName
        End Get
    End Property
    Private _sureName As String
    Public ReadOnly Property SureName() As String
        Get
            Return _sureName
        End Get
    End Property
    Private _address As String
    Public ReadOnly Property Address() As String
        Get
            Return _address
        End Get
    End Property
    Private _eMBG As String
    Public ReadOnly Property EMBG() As String
        Get
            Return _eMBG
        End Get
    End Property
    Private _bLK As String
    Public ReadOnly Property BLK() As String
        Get
            Return _bLK
        End Get
    End Property
    Private _idStation As Integer
    Public ReadOnly Property IdStation() As Integer
        Get
            Return _idStation
        End Get
    End Property
    'Private _dateOfBirth As Date
    'Public ReadOnly Property DateOfBirth() As Date
    '  Get
    '    Return _dateOfBirth
    '  End Get
    'End Property
    'Private _dateOfHireing As Date
    'Public ReadOnly Property DateOfHireing() As Date
    '  Get
    '    Return _dateOfHireing
    '  End Get
    'End Property
    'Private _rFID As String
    'Public ReadOnly Property RFID() As String
    '  Get
    '    Return _rFID
    '  End Get
    'End Property
    Protected Overrides Function GetIdValue() As Object
        Return _id
    End Function

    Public Overrides Function ToString() As String
        Return _id
    End Function

    Friend Sub New(ByVal dr As SafeDataReader)
        _id = dr.GetInt64("ID")
        _idStation = dr.GetInt32("IdStation")
        _idDataBase = dr.GetInt32("IdDataBase")
        _firstName = dr.GetString("FirstName")
        _sureName = dr.GetString("SureName")
        _address = dr.GetString("Address")
        _eMBG = dr.GetString("EMBG")
        _bLK = dr.GetString("BLK")
    End Sub
    Friend Sub New(ByVal intId As Long, ByVal idStation As Integer, ByVal IdDataBase As Integer, ByVal strFirstName As String, ByVal strsureName As String, _
                   ByVal inAddress As String, ByVal inEMBG As Integer, ByVal inBLK As String)
        _id = intId
        _idStation = idStation
        _idDataBase = IdDataBase
        _firstName = strFirstName
        _sureName = strsureName
        _address = inAddress
        _eMBG = inEMBG
        _bLK = inBLK
    End Sub
End Class