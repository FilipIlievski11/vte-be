

<Serializable()> _
Public Class CustomersSearchInfo
 Inherits ReadOnlyBase(Of CustomersSearchInfo)

 Private _id As Long
 Public ReadOnly Property Id() As Long
  Get
   Return _id
  End Get
 End Property
 Private _mb As String
 Public ReadOnly Property MB() As String
  Get
   Return _mb
  End Get
 End Property
 Private _customerSurname As String
 Public ReadOnly Property CustomerSurname() As String
  Get
   Return _customerSurname
  End Get
 End Property

 Protected Overrides Function GetIdValue() As Object
  Return _id
 End Function
 Public Overrides Function ToString() As String
  Return _id
 End Function

 Friend Sub New(ByVal dr As SafeDataReader)
  _id = dr.GetInt64("Id")
  _mb = dr.GetString("MB") & " " & dr.GetString("Ime")

 End Sub


End Class
