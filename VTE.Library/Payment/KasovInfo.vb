

<Serializable()> _
Public Class KasovInfo
    Inherits ReadOnlyBase(Of KasovInfo)

#Region " Business Properties and Methods "

    Private _price As Double
    Public ReadOnly Property Price() As Double
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _price
        End Get
    End Property

    Private _idPriceCatalog As Integer
    Public ReadOnly Property idPriceCatalog() As Integer
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idPriceCatalog
        End Get
    End Property

    Private _paymanetCategory As String
    Public ReadOnly Property paymanetCategory() As String
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _paymanetCategory
        End Get
    End Property

    Private _idOperator As Integer
    Public ReadOnly Property IdOperator() As Integer
        <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Get
            Return _idOperator
        End Get
    End Property
    'Protected Overrides Function GetIdValue() As Object
    '    Return _id
    'End Function



#End Region

#Region " Factory Methods "

    Friend Shared Function GetKasovInfo(ByVal dr As SafeDataReader) As KasovInfo
        Return New KasovInfo(dr)
    End Function

    Private Sub New(ByVal dr As SafeDataReader)
        Fetch(dr)
    End Sub


#End Region

#Region " Data Access - Fetch "

    Private Sub Fetch(ByVal dr As SafeDataReader)
        _price = dr.GetDecimal("Price")
        _idPriceCatalog = dr.GetInt32("IdPriceCatalog")
        _idOperator = dr.GetInt32("IdOperator")
        If _idPriceCatalog > 0 Then
            Dim pInfo As PaymentCataologInfo = _
            CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
            PaymentCataologList).GetInfo(_idPriceCatalog)
            ' _priceName = pInfo.PaymentName
            _paymanetCategory = pInfo.CategoryName
        Else
            ' _priceName = My.Resources.Rata
            _paymanetCategory = My.Resources.Rata
        End If

    End Sub

#End Region


End Class
