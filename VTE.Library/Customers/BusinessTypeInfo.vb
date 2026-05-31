
<Serializable()> _
Public Class BusinessTypeInfo
  Inherits ReadOnlyBase(Of BusinessTypeInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _businessTypeCode As String
  Public ReadOnly Property BusinessTypeCode() As String
    Get
      Return _businessTypeCode
    End Get
  End Property
  Private _businessTypeDescription As String
  Public ReadOnly Property BusinessTypeDescription() As String
    Get
      Return _businessTypeDescription
    End Get
  End Property
  Public ReadOnly Property BusinessType() As String
    Get
      Return _businessTypeCode & "-" & _businessTypeDescription
    End Get
  End Property
  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal intId As Integer, ByVal strBusinessTypeCode As String, ByVal strBusinesTypeDescription As String)
    _id = 0
    _businessTypeCode = strBusinessTypeCode
    _businessTypeDescription = strBusinesTypeDescription
  End Sub

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
    _businesstypecode = dr.GetString("BusinessTypeCode")
    _businesstypedescription = dr.GetString("BusinessTypeDescription")
  End Sub

End Class