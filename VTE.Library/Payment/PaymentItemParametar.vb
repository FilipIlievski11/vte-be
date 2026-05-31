
<Serializable()> _
Public Class PaymentItemParametar
  Inherits Csla.BusinessBase(Of PaymentItemParametar)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetPaymentItemParametarByID"
  Private Const spGetAll As String = "GetPaymentItemParametars"
  Private Const spUpdate As String = "updatePaymentItemParametar"
  Private Const spAdd As String = "addPaymentItemParametar"
  Private Const spDelete As String = "deletePaymentItemParametar"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentItemParametar), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdPaymentItemProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentItemParametar), New PropertyInfo(Of Integer)("IdPaymentItem"))
  Private Shared PrametarNameProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentItemParametar), New PropertyInfo(Of String)("PrametarName"))
  Private Shared VehicleFieldProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(PaymentItemParametar), New PropertyInfo(Of String)("VehicleField"))
  Private Shared ParametarFromProperty As PropertyInfo(Of Single) = RegisterProperty(Of Single)(GetType(PaymentItemParametar), New PropertyInfo(Of Single)("ParametarFrom"))
  Private Shared ParametarToProperty As PropertyInfo(Of Single) = RegisterProperty(Of Single)(GetType(PaymentItemParametar), New PropertyInfo(Of Single)("ParametarTo"))
  Private Shared PriceProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(PaymentItemParametar), New PropertyInfo(Of Decimal)("Price"))
  Private Shared IsOptionalProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(PaymentItemParametar), New PropertyInfo(Of Boolean)("IsOptional"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdPaymentItem() As Integer
    Get
      Return GetProperty(Of Integer)(IdPaymentItemProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdPaymentItemProperty, value)
    End Set
  End Property
  Public Property PrametarName() As String
    Get
      Return GetProperty(Of String)(PrametarNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PrametarNameProperty, value)
    End Set
  End Property
  Public Property VehicleField() As String
    Get
      Return GetProperty(Of String)(VehicleFieldProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(VehicleFieldProperty, value)
    End Set
  End Property
  Public Property ParametarFrom() As Single
    Get
      Return GetProperty(Of Single)(ParametarFromProperty)
    End Get
    Set(ByVal value As Single)
      SetProperty(Of Single)(ParametarFromProperty, value)
    End Set
  End Property
  Public Property ParametarTo() As Single
    Get
      Return GetProperty(Of Single)(ParametarToProperty)
    End Get
    Set(ByVal value As Single)
      SetProperty(Of Single)(ParametarToProperty, value)
    End Set
  End Property
  Public Property Price() As Decimal
    Get
      Return GetProperty(Of Decimal)(PriceProperty)
    End Get
    Set(ByVal value As Decimal)
      SetProperty(Of Decimal)(PriceProperty, value)
    End Set
  End Property
  Public Property IsOptional() As Boolean
    Get
      Return GetProperty(Of Boolean)(IsOptionalProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(IsOptionalProperty, value)
    End Set
  End Property

  Public Property PriceWithoutTax() As Decimal
    Get

      Dim ddvList As DDVList = Csla.ApplicationContext.LocalContext("objDDVList")
      Dim DDV As Decimal = ddvList.GetInfo(Csla.ApplicationContext.LocalContext("CategorieDDV")).DDVValue
      Return Math.Round(DDVPresmetki.DanocnaOsnovica(GetProperty(Of Decimal)(PriceProperty), DDV, 1), 2)
    End Get
    Set(ByVal value As Decimal)
      Dim ddvList As DDVList = Csla.ApplicationContext.LocalContext("objDDVList")
      Dim DDV As Decimal = ddvList.GetInfo(Csla.ApplicationContext.LocalContext("CategorieDDV")).DDVValue
      Dim vrednost As Decimal = Math.Round(value * (1 + DDV / 100), 2)
      SetProperty(Of Decimal)(PriceProperty, vrednost)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdPaymentItem") Then
            AuthorizationRules.AllowWrite("IdPaymentItem", roleName)
        Else
            AuthorizationRules.DenyWrite("IdPaymentItem", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PrametarName") Then
            AuthorizationRules.AllowWrite("PrametarName", roleName)
        Else
            AuthorizationRules.DenyWrite("PrametarName", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("VehicleField") Then
            AuthorizationRules.AllowWrite("VehicleField", roleName)
        Else
            AuthorizationRules.DenyWrite("VehicleField", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ParametarFrom") Then
            AuthorizationRules.AllowWrite("ParametarFrom", roleName)
        Else
            AuthorizationRules.DenyWrite("ParametarFrom", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ParametarTo") Then
            AuthorizationRules.AllowWrite("ParametarTo", roleName)
        Else
            AuthorizationRules.DenyWrite("ParametarTo", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Price") Then
            AuthorizationRules.AllowWrite("Price", roleName)
        Else
            AuthorizationRules.DenyWrite("Price", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IsOptional") Then
            AuthorizationRules.AllowWrite("IsOptional", roleName)
        Else
            AuthorizationRules.DenyWrite("IsOptional", roleName)
        End If
    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentItemParametar")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentItemParametar")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentItemParametar")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentItemParametar")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' PrametarNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, PrametarNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PrametarNameProperty, 250))
    ' VehicleFieldProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, VehicleFieldProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(VehicleFieldProperty, 150))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewPaymentItemParametarChild() As PaymentItemParametar
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a PaymentItemParametar")
        'End If
        Return DataPortal.CreateChild(Of PaymentItemParametar)()
  End Function

  Friend Shared Function GetPaymentItemParametar(ByVal dr As SafeDataReader) As PaymentItemParametar
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a PaymentItemParametar")
        'End If
        Return DataPortal.FetchChild(Of PaymentItemParametar)(dr)
  End Function

  Private Sub New()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "
  Protected Overloads Sub Child_Create()
    ValidationRules.CheckRules()
  End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("PaymentItemParametar.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdPaymentItemProperty, dr.GetInt32("IdPaymentItem"))
      LoadProperty(Of String)(PrametarNameProperty, dr.GetString("PrametarName"))
      LoadProperty(Of String)(VehicleFieldProperty, dr.GetString("VehicleField"))
      LoadProperty(Of Single)(ParametarFromProperty, dr.GetValue("ParametarFrom"))
      LoadProperty(Of Single)(ParametarToProperty, dr.GetValue("ParametarTo"))
      LoadProperty(Of Decimal)(PriceProperty, dr.GetDecimal("Price"))
      LoadProperty(Of Boolean)(IsOptionalProperty, dr.GetBoolean("IsOptional"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("PaymentItemParametar.Child_Fetch", ex)
      Throw New DbCslaException("PaymentItemParametar.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As PaymentItem)
    Try
      Dim cn As SqlConnection = ApplicationContext.LocalContext("cn")
      If cn.State = ConnectionState.Closed Then
        cn.ConnectionString = Database.VTEConnection
        cn.Open()
      End If
      Using cm As SqlCommand = cn.CreateCommand
        With cm
          .CommandType = CommandType.StoredProcedure
          .CommandText = spAdd
          'Smeni go Id so Parent.Id
          .Parameters.AddWithValue("@IdPaymentItem", parent.Id)
          .Parameters.AddWithValue("@PrametarName", ReadProperty(Of String)(PrametarNameProperty))
          .Parameters.AddWithValue("@VehicleField", ReadProperty(Of String)(VehicleFieldProperty))
          .Parameters.AddWithValue("@ParametarFrom", ReadProperty(Of Single)(ParametarFromProperty))
          .Parameters.AddWithValue("@ParametarTo", ReadProperty(Of Single)(ParametarToProperty))
          .Parameters.AddWithValue("@Price", ReadProperty(Of Decimal)(PriceProperty))
          .Parameters.AddWithValue("@IsOptional", ReadProperty(Of Boolean)(IsOptionalProperty))

          Dim param As New SqlParameter("@newId", SqlDbType.Int)
          param.Direction = ParameterDirection.Output
          .Parameters.Add(param)
          param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
          param.Direction = ParameterDirection.Output
          .Parameters.Add(param)

          .ExecuteNonQuery()

          LoadProperty(Of Integer)(IdProperty, CInt(.Parameters("@newId").Value))
          _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
        End With
      End Using
      'update child objects

    Catch ex As Exception
      Database.LogException("PaymentItemParametar.Child_Insert", ex)
      Throw New DbCslaException("PaymentItemParametar.Child_Insert", ex)
    Finally
      Database.LogInfo("PaymentItemParametar.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As PaymentItem)
    Database.LogInfo("PaymentItemParametar.Child_Update", GetHashCode)
    Try
      Dim cn As SqlConnection = ApplicationContext.LocalContext("cn")
      If cn.State = ConnectionState.Closed Then
        cn.ConnectionString = Database.VTEConnection
        cn.Open()
      End If
      Using cm As SqlCommand = cn.CreateCommand
        With cm
          .CommandType = CommandType.StoredProcedure
          .CommandText = spUpdate

          .Parameters.AddWithValue("@Id", ReadProperty(Of Integer)(IdProperty))
          .Parameters.AddWithValue("@IdPaymentItem", parent.Id)
          .Parameters.AddWithValue("@PrametarName", ReadProperty(Of String)(PrametarNameProperty))
          .Parameters.AddWithValue("@VehicleField", ReadProperty(Of String)(VehicleFieldProperty))
          .Parameters.AddWithValue("@ParametarFrom", ReadProperty(Of Single)(ParametarFromProperty))
          .Parameters.AddWithValue("@ParametarTo", ReadProperty(Of Single)(ParametarToProperty))
          .Parameters.AddWithValue("@Price", ReadProperty(Of Decimal)(PriceProperty))
          .Parameters.AddWithValue("@IsOptional", ReadProperty(Of Boolean)(IsOptionalProperty))
          .Parameters.AddWithValue("@lastChanged", _lastChanged)
          Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
          param.Direction = ParameterDirection.Output
          .Parameters.Add(param)

          .ExecuteNonQuery()

          _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
        End With
      End Using
      'update child objects

    Catch ex As Exception
      Database.LogException("PaymentItemParametar.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("PaymentItemParametar.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("PaymentItemParametar.Child_DeleteSelf", GetHashCode)
    Try
      Dim cn As SqlConnection = Csla.ApplicationContext.LocalContext("cn")
      If cn.State = ConnectionState.Closed Then
        cn.ConnectionString = Database.VTEConnection
        cn.Open()
      End If
      Using cm As SqlCommand = cn.CreateCommand
        With cm
          .CommandType = CommandType.StoredProcedure
          .CommandText = spDelete
          .Parameters.AddWithValue("@id", ReadProperty(Of Integer)(IdProperty))
          .ExecuteNonQuery()
        End With
      End Using


    Catch ex As Exception
      Database.LogException("PaymentItemParametar.Child_Fetch", ex)
      Throw New DbCslaException("PaymentItemParametar.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
