
<Serializable()> _
Public Class VehicleCategoryForPaymentsInfo
  Inherits ReadOnlyBase(Of VehicleCategoryForPaymentsInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _code As String
  Public ReadOnly Property Code() As String
    Get
      Return _code
    End Get
  End Property
  Public ReadOnly Property CodeAndName() As String
    Get
      Return _code & ". " & _name
    End Get
  End Property
  Private _name As String
  Public ReadOnly Property Name() As String
    Get
      Return _name
    End Get
  End Property
  Private _zelenMap As Integer
  Public ReadOnly Property ZelenMap() As Integer
    Get
      Return _zelenMap
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
    _code = dr.GetString("Code")
    _name = dr.GetString("Name")
    _zelenMap = dr.GetInt32("ZelenMap")
  End Sub

End Class