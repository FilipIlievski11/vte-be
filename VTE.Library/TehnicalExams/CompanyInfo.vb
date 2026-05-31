
<Serializable()> _
Public Class CompanyInfo
 Inherits ReadOnlyBase(Of CompanyInfo)

#Region " Business Properties and Methods "

 Private _id As Integer
 Public ReadOnly Property Id() As Integer
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _id
  End Get
 End Property

 Private _companyname As String
 Public ReadOnly Property Companyname() As String
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _companyname
  End Get
 End Property


 Protected Overrides Function GetIdValue() As Object
  Return _id
 End Function

#End Region

#Region " Factory Methods "

 Friend Shared Function GetCompanyInfo(ByVal dr As SafeDataReader) As CompanyInfo
  Return New CompanyInfo(dr)
 End Function

 Private Sub New(ByVal dr As SafeDataReader)
  Fetch(dr)
 End Sub


#End Region

#Region " Data Access - Fetch "

 Private Sub Fetch(ByVal dr As SafeDataReader)
  _id = dr.GetInt32("Id")
  _companyname = dr.GetString("CompanyName")

 End Sub

#End Region

End Class