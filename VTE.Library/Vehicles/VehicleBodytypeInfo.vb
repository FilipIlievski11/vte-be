
<Serializable()> _
Public Class VehicleBodytypeInfo
  Inherits ReadOnlyBase(Of VehicleBodytypeInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _oldBodytypeDescription As String
  Public ReadOnly Property OldBodytypeDescription() As String
    Get
      Return _oldBodytypeDescription
    End Get
  End Property
  Private _bodytypeCode As String
  Public ReadOnly Property BodytypeCode() As String
    Get
      Return _bodytypeCode
    End Get
  End Property
  Private _bodytypeDescriprion As String
  Public ReadOnly Property BodytypeDescriprion() As String
    Get
      Return _bodytypeDescriprion
    End Get
  End Property
  Public ReadOnly Property BodytypeFull() As String
    Get
      Return _bodytypeCode & "-" & _bodytypeDescriprion
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
    _oldBodytypeDescription = dr.GetString("OldBodytypeDescription")
    _bodytypecode = dr.GetString("BodytypeCode")
    _bodytypedescriprion = dr.GetString("BodytypeDescriprion")
  End Sub

End Class