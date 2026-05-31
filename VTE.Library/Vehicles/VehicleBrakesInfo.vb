
<Serializable()> _
Public Class VehicleBrakesInfo
  Inherits ReadOnlyBase(Of VehicleBrakesInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _breakesCode As Integer
  Public ReadOnly Property BreakesCode() As Integer
    Get
      Return _breakesCode
    End Get
  End Property
  Private _breakesDescription As String
  Public ReadOnly Property BreakesDescription() As String
    Get
      Return _breakesDescription
    End Get
  End Property
  Public ReadOnly Property Breakes() As String
    Get
      Return BreakesCode & "-" & BreakesDescription
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
    _breakescode = dr.GetInt32("BreakesCode")
    _breakesdescription = dr.GetString("BreakesDescription")
  End Sub

End Class