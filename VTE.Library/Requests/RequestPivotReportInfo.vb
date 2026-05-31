<Serializable()> _
Public Class RequestPivotReportInfo
    Inherits ReadOnlyBase(Of RequestPivotReportInfo)

    Private _IdVehicle As Long
    Public ReadOnly Property IdVehicle() As Long
        Get
            Return _idVehicle
        End Get
    End Property

    Private _ShellNumber As String
    Public ReadOnly Property ShellNumber() As String
        Get
            Return _ShellNumber
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

    Private _DateCreated As Date
    Public ReadOnly Property DateCreated() As Date
        Get
            Return _DateCreated
        End Get
    End Property

    Private _DateEnded As Date
    Public ReadOnly Property DateEnded() As Date
        Get
            Return _DateEnded
        End Get
    End Property

    Private _RequestId As Long
    Public ReadOnly Property RequestId() As Long
        Get
            Return _RequestId
        End Get
    End Property

    Private _Note As String
    Public ReadOnly Property Note() As String
        Get
            Return _Note
        End Get
    End Property

    Private _TypeName As String
    Public ReadOnly Property TypeName() As String
        Get
            Return _TypeName
        End Get
    End Property

    Public ReadOnly Property Status() As String
        Get
            If _DateEnded = CType("01.01.0001 00:00:00", Date) Then
                Return "Отворено"
            Else
                Return "Затворено"
            End If
        End Get
    End Property
    Private _RegNumber As String

    Public ReadOnly Property RegNumber() As String
        Get
            Return _RegNumber
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
    Public ReadOnly Property Customer() As String
        Get
            Return _CustomerFirstName & " " & _CustomerSurname
        End Get
    End Property
    Protected Overrides Function GetIdValue() As Object
        Return _IdVehicle
    End Function

    Public Overrides Function ToString() As String
        Return _IdVehicle
    End Function

    Friend Sub New(ByVal dr As SafeDataReader)
       
        _IdVehicle = dr.GetInt64("IdVehicle")
        _ShellNumber = dr.GetString("ShellNumber")
        _CategoryCode = dr.GetString("CategoryCode")
        _CategoryName = dr.GetString("CategoryName")
        _RequestId = dr.GetInt64("RequestId")
        _DateCreated = dr.GetDateTime("DateCreated")
        _DateEnded = dr.GetDateTime("DateEnded")
        _Note = dr.GetString("Note")
        _TypeName = dr.GetString("TypeName")
        _RegNumber = dr.GetString("RegNumber")
        _CustomerFirstName = dr.GetString("CustomerFirstName")
        _CustomerSurname = dr.GetString("CustomerSurname")

    End Sub

End Class

