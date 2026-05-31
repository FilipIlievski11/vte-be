
<Serializable()> _
Public Class PaymentDocumentsRata
 Inherits Csla.BusinessBase(Of PaymentDocumentsRata)

#Region " Stored Procedures Names "
 Private Const SpZemiSite As String = "getPaymentDocumentsRata"
 Private Const SpZemiPoID As String = "getPaymentDocumentsRatById"
 Private Const SpSnimi As String = "updatePaymentDocumentsRat"
 Private Const SpDodadi As String = "addPaymentDocumentsRat"
 Private Const SpIzbrisi As String = "deletePaymentDocumentsRat"

#End Region


#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(PaymentDocumentsRata), New PropertyInfo(Of Long)("Id"))
 Private Shared IdPaymentDocumentsProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(PaymentDocumentsRata), New PropertyInfo(Of Long)("IdPaymentDocuments"))
 Private Shared PriceProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(PaymentDocumentsRata), New PropertyInfo(Of Decimal)("Price"))
 Private Shared PayedProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentDocumentsRata), New PropertyInfo(Of Boolean)("Payed"))
 Private Shared DatePayedProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(PaymentDocumentsRata), New PropertyInfo(Of SmartDate)("DatePayed", "DatePayed", New SmartDate(DateTime.Today, True)))
 Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentDocumentsRata), New PropertyInfo(Of String)("Note"))
 Private Shared IdOperatorProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentDocumentsRata), New PropertyInfo(Of Integer)("IdOperator"))
 Private Shared IdOrganizationProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentDocumentsRata), New PropertyInfo(Of Integer)("IdOrganization"))
 Private _lastChanged(7) As Byte

 <System.ComponentModel.DataObjectField(True, True)> _
 Public ReadOnly Property Id() As Long
  Get
   Return GetProperty(Of Long)(IdProperty)
  End Get
 End Property
 Public Property IdPaymentDocuments() As Long
  Get
   Return GetProperty(Of Long)(IdPaymentDocumentsProperty)
  End Get
  Set(ByVal value As Long)
   SetProperty(Of Long)(IdPaymentDocumentsProperty, value)
  End Set
 End Property

 Public Property Price() As Decimal
  Get
   Return FicalRound(GetProperty(Of Decimal)(PriceProperty))
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(PriceProperty, FicalRound(value))
  End Set
 End Property
 Public Property Payed() As Boolean
  Get
   Return GetProperty(Of Boolean)(PayedProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(PayedProperty, value)
  End Set
 End Property

 Public Property DatePayed() As Date
  Get
   Return GetProperty(Of SmartDate, Date)(DatePayedProperty)
  End Get
  Set(ByVal value As Date)
   SetProperty(Of SmartDate, Date)(DatePayedProperty, value)
  End Set
 End Property
 Public Property Note() As String
  Get
   Return GetProperty(Of String)(NoteProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(NoteProperty, value)
  End Set
 End Property
 Public Property IdOperator() As Integer
  Get
   Return GetProperty(Of Integer)(IdOperatorProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdOperatorProperty, value)
  End Set
 End Property

 Public Property IdOrganization() As Integer
  Get
   Return GetProperty(Of Integer)(IdOrganizationProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdOrganizationProperty, value)
  End Set
 End Property

 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function
#End Region 'Business Properties and Methods


#Region " Validation Rules "
 Protected Overrides Sub AddBusinessRules()
  ' NoteProperty rules
  ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(NoteProperty, 150))
  'ValidationRules.AddRule(Of PaymentDocumentsRata)(AddressOf CheckChild, PriceProperty)
 End Sub
 Private Shared Function CheckChild(Of T As PaymentDocumentsRata)(ByVal target As T, _
ByVal e As Csla.Validation.RuleArgs) As Boolean
  Dim vkupnoRati As Decimal = 0
  Dim vkupnoDetali As Decimal = 0
  Try
   Dim PayDocRati As PaymentDocumentsRati = CType(target.Parent, PaymentDocumentsRati)
   Dim PayDoc As PaymentDocument = PaymentDocument.GetPaymentDocument(PayDocRati.Item(0).IdPaymentDocuments)
   If PayDoc IsNot Nothing Then
    For Each rata As PaymentDocumentsRata In PayDoc.PaymentDocumentRati
     vkupnoRati += rata.Price
    Next
    For Each detal As PaymentDocumentsDetail In PayDoc.PaymentDocumentDetails
     vkupnoDetali += detal.Price
    Next
    If vkupnoRati > vkupnoDetali Then
     e.Description = "Збирот на ратите не смее да биде поголем од вкупната сметка"
     Return False
     Exit Function
    End If
   End If
  Catch ex As Exception
   Return True
  End Try
  Return True

 End Function

#End Region ' Validation Rules

#Region " Factory Methods "

 Friend Shared Function NewPaymentDocumentsRataChild() As PaymentDocumentsRata

  Return DataPortal.CreateChild(Of PaymentDocumentsRata)()
 End Function

 Friend Shared Function GetPaymentDocumentsRata(ByVal dr As SafeDataReader) As PaymentDocumentsRata

  Return DataPortal.FetchChild(Of PaymentDocumentsRata)(dr)
 End Function

 Public Shared Sub DeletePaymentDocumentsRata(ByVal id As Integer)

  DataPortal.Delete(New SingleCriteria(Of PaymentDocumentsRata, Integer)(id))
 End Sub

 Public Overrides Function Save() As PaymentDocumentsRata
  Return MyBase.Save()
 End Function

 Private Sub New()
 End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "
 <RunLocal()> _
 Protected Overloads Sub Child_Create()
  ValidationRules.CheckRules()
 End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

 Private Sub Child_Fetch(ByVal dr As SafeDataReader)
  Database.LogInfo("PaymentDocumentsRata.Child_Fetch", GetHashCode())
  Try

   LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
   LoadProperty(Of Long)(IdPaymentDocumentsProperty, dr.GetInt64("IdPaymentDocument"))
   LoadProperty(Of Decimal)(PriceProperty, dr.GetDecimal("Price"))
   LoadProperty(Of Boolean)(PayedProperty, dr.GetBoolean("Payed"))
   LoadProperty(Of SmartDate, Date?)(DatePayedProperty, dr.GetSmartDate("DatePayed", True))
   LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
   LoadProperty(Of Integer)(IdOperatorProperty, dr.GetInt32("IdOperator"))
   LoadProperty(Of Integer)(IdOrganizationProperty, dr.GetInt32("IdOrganization"))
   dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

   ValidationRules.CheckRules()

  Catch ex As Exception
   Database.LogException("PaymentDocumentsRata.Child_Fetch", ex)
   Throw New DbCslaException("PaymentDocumentsRata.Child_Fetch", ex)
  End Try

 End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

 Private Sub Child_Insert(ByVal parent As PaymentDocument)
  Try
   Using cn As SqlConnection = ApplicationContext.LocalContext("cn")
    If cn.State = ConnectionState.Closed Then
     cn.ConnectionString = Database.VTEConnection
     cn.Open()
    End If


    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = SpDodadi
      'Smeni go Id so Parent.Id
      .Parameters.AddWithValue("@IdPaymentDocument", parent.Id)
      .Parameters.AddWithValue("@Price", ReadProperty(Of Decimal)(PriceProperty))
      .Parameters.AddWithValue("@Payed", ReadProperty(Of Boolean)(PayedProperty))
      .Parameters.AddWithValue("@DatePayed", ReadProperty(Of SmartDate)(DatePayedProperty).DBValue)
      .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
      .Parameters.AddWithValue("@IdOperator", CInt(Csla.ApplicationContext.LocalContext("EmployeeID")))
      .Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
      Dim param As New SqlParameter("@newId", SqlDbType.Int)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)
      param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)

      .ExecuteNonQuery()

      LoadProperty(Of Long)(IdProperty, CInt(.Parameters("@newId").Value))
      _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
     End With
    End Using
    'update child objects
   End Using
  Catch ex As Exception
   Database.LogException("PaymentDocumentsRata.Child_Insert", ex)
   Throw New DbCslaException("PaymentDocumentsRata.Child_Insert", ex)
  Finally
   Database.LogInfo("PaymentDocumentsRata.Child_Insert", GetHashCode)
  End Try

 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Private Sub Child_Update(ByVal parent As PaymentDocument)
  Database.LogInfo("PaymentDocumentsDetail.Child_Update", GetHashCode)
  Try
   Using cn As SqlConnection = ApplicationContext.LocalContext("cn")
    If cn.State = ConnectionState.Closed Then
     cn.ConnectionString = Database.VTEConnection
     cn.Open()
    End If

    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = SpSnimi
      .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
      .Parameters.AddWithValue("@IdPaymentDocument", parent.Id)
      .Parameters.AddWithValue("@Price", ReadProperty(Of Decimal)(PriceProperty))
      .Parameters.AddWithValue("@Payed", ReadProperty(Of Boolean)(PayedProperty))
      .Parameters.AddWithValue("@DatePayed", ReadProperty(Of SmartDate)(DatePayedProperty).DBValue)
      .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
      .Parameters.AddWithValue("@IdOperator", CInt(Csla.ApplicationContext.LocalContext("EmployeeID")))
      .Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
      .Parameters.AddWithValue("@lastChanged", _lastChanged)
      Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)

      .ExecuteNonQuery()

      _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
     End With
    End Using
    'update child objects
   End Using
  Catch ex As Exception
   Database.LogException("PaymentDocumentsRata.Child_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DbCslaException("PaymentDocumentsRata.Child_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Private Sub Child_DeleteSelf()

  Database.LogInfo("PaymentDocumentsRata.Child_DeleteSelf", GetHashCode)
  Try
   Using cn As SqlConnection = Csla.ApplicationContext.LocalContext("cn")
    If cn.State = ConnectionState.Closed Then
     cn.ConnectionString = Database.VTEConnection
     cn.Open()
    End If


    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = SpIzbrisi
      .Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
      .ExecuteNonQuery()
     End With
    End Using

   End Using
  Catch ex As Exception
   Database.LogException("PaymentDocumentsRata.Child_Fetch", ex)
   Throw New DbCslaException("PaymentDocumentsRata.Child_Fetch", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
