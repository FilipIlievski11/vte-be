
<Serializable()> _
Public Class VehicleSupportingInfo
  Inherits ReadOnlyBase(Of VehicleSupportingInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _supportingCode As Integer
  Public ReadOnly Property SupportingCode() As Integer
    Get
      Return _supportingCode
    End Get
  End Property
  Private _supportingDescription As String
  Public ReadOnly Property SupportingDescription() As String
    Get
      Return _supportingDescription
    End Get
  End Property
  Public ReadOnly Property Supporting() As String
    Get
      Return SupportingCode & "-" & SupportingDescription
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
    _supportingcode = dr.GetInt32("SupportingCode")
    _supportingdescription = dr.GetString("SupportingDescription")
  End Sub

End Class