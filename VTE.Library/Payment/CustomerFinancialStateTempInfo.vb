
<Serializable()> _
Public Class CustomerFinancialStateTempInfo
    Inherits ReadOnlyBase(Of CustomerFinancialStateTempInfo)

#Region " Business Properties and Methods "

    Private _id As Integer
    Public ReadOnly Property Id() As Integer
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _id
        End Get
    End Property

    Private _idvehicle As Long
    Public ReadOnly Property Idvehicle() As Long
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idvehicle
        End Get
    End Property

    Private _idpricecatalog As Integer
    Public ReadOnly Property Idpricecatalog() As Integer
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idpricecatalog
        End Get
    End Property

    Private _price As Decimal
    Public ReadOnly Property Price() As Decimal
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _price
        End Get
    End Property

  

    Protected Overrides Function GetIdValue() As Object
        Return _id
    End Function



#End Region

#Region " Factory Methods "

    Friend Shared Function GetCustomerFinancialStateTempInfo(ByVal dr As SafeDataReader) As CustomerFinancialStateTempInfo
        Return New CustomerFinancialStateTempInfo(dr)
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        Fetch(dr)
    End Sub


#End Region

#Region " Data Access - Fetch "

    Private Sub Fetch(ByVal dr As SafeDataReader)
        _id = dr.GetInt32("Id")
        _idvehicle = dr.GetInt64("IdVehicle")
        _idpricecatalog = dr.GetInt32("IdPriceCatalog")
        _price = dr.GetDecimal("Price")

    End Sub

#End Region

End Class
