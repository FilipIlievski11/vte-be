
<Serializable()> _
Public Class RegistrationIssuerInfo
  Inherits ReadOnlyBase(Of RegistrationIssuerInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _issuerName As String
  Public ReadOnly Property IssuerName() As String
    Get
      Return _issuerName
    End Get
    End Property
    Private _IdCommunity As Integer
    Public ReadOnly Property IdCommunity() As Integer
        Get
            Return _IdCommunity
        End Get
    End Property

  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal id As Integer, ByVal issuerName As String)
    _id = id
    _issuerName = issuerName
  End Sub

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
        _issuerName = dr.GetString("IssuerName")
        _IdCommunity = dr.GetInt32("IdCommunity")
  End Sub

End Class