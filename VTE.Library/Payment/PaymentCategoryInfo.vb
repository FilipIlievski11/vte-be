
<Serializable()> _
Public Class PaymentCategoryInfo
 Inherits ReadOnlyBase(Of PaymentCategoryInfo)

#Region " Business Properties and Methods "

 Private _id As Integer
 Public ReadOnly Property Id() As Integer
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _id
  End Get
 End Property

 Private _categoryname As String
 Public ReadOnly Property Categoryname() As String
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _categoryname
  End Get
 End Property

 Private _CommunityName As String
 Public ReadOnly Property CommunityName() As String
  <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
  Get
   Return _CommunityName
  End Get
 End Property

 Protected Overrides Function GetIdValue() As Object
  Return _id
 End Function



#End Region

#Region " Factory Methods "

 Friend Shared Function GetPaymentCategoryInfo(ByVal dr As SafeDataReader) As PaymentCategoryInfo
  Return New PaymentCategoryInfo(dr)
 End Function

 Private Sub New(ByVal dr As SafeDataReader)
  Fetch(dr)
 End Sub

#End Region

#Region " Data Access - Fetch "

 Private Sub Fetch(ByVal dr As SafeDataReader)
  _id = dr.GetInt32("Id")
  _categoryname = dr.GetString("CategoryName")
  _CommunityName = dr.GetString("CommunityName")
 End Sub

#End Region

End Class