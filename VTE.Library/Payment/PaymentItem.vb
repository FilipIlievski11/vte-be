
<Serializable()> _
Public Class PaymentItem
  Inherits Csla.BusinessBase(Of PaymentItem)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetPaymentItemByID"
  Private Const spGetAll As String = "GetPaymentItems"
  Private Const spUpdate As String = "updatePaymentItem"
  Private Const spAdd As String = "addPaymentItem"
  Private Const spDelete As String = "deletePaymentItem"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentItem), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdPymentCategoryProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentItem), New PropertyInfo(Of Integer)("IdPymentCategory"))
  Private Shared IdVehicleCategoryForPaymentsProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(PaymentItem), New PropertyInfo(Of Integer)("IdVehicleCategoryForPayments"))
  Private Shared ItemNameProperty As PropertyInfo(Of String) = _
    RegisterProperty(Of String)(GetType(PaymentItem), _
    New PropertyInfo(Of String)("ItemName"))
  Private Shared ParametarsProperty As PropertyInfo(Of PaymentItemParametars) = _
    RegisterProperty(Of PaymentItemParametars)(GetType(PaymentItem), _
    New PropertyInfo(Of PaymentItemParametars)("Parametars"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdPymentCategory() As Integer
    Get
      Return GetProperty(Of Integer)(IdPymentCategoryProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdPymentCategoryProperty, value)
    End Set
  End Property
  Public Property IdVehicleCategoryForPayments() As Integer
    Get
      Return GetProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty, value)
    End Set
  End Property
  Public Property ItemName() As String
    Get
      Return GetProperty(Of String)(ItemNameProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(ItemNameProperty, value)
    End Set
  End Property

  Public ReadOnly Property Parametars() As PaymentItemParametars
    Get
      If Not FieldManager.FieldExists(ParametarsProperty) Then
        SetProperty(Of PaymentItemParametars)(ParametarsProperty, PaymentItemParametars.NewPaymentItemParametars)
      End If
      Return GetProperty(Of PaymentItemParametars)(ParametarsProperty)
    End Get
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function



#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdPymentCategory") Then
            AuthorizationRules.AllowWrite("IdPymentCategory", roleName)
        Else
            AuthorizationRules.DenyWrite("IdPymentCategory", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicleCategoryForPayments") Then
            AuthorizationRules.AllowWrite("IdVehicleCategoryForPayments", roleName)
        Else
            AuthorizationRules.DenyWrite("IdVehicleCategoryForPayments", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ItemName") Then
            AuthorizationRules.AllowWrite("ItemName", roleName)
        Else
            AuthorizationRules.DenyWrite("ItemName", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Parametars") Then
            AuthorizationRules.AllowWrite("Parametars", roleName)
        Else
            AuthorizationRules.DenyWrite("Parametars", roleName)
        End If

    End Sub



    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentItem")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentItem")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentItem")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentItem")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' ItemNameProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ItemNameProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(ItemNameProperty, 250))
    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Csla.Validation.IntegerMinValueRuleArgs(IdVehicleCategoryForPaymentsProperty, 1))

  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

    Friend Shared Function NewPaymentItemChild() As PaymentItem
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a PaymentItem")
        'End If
        Return DataPortal.CreateChild(Of PaymentItem)()
    End Function

  Friend Shared Function GetPaymentItem(ByVal dr As SafeDataReader) As PaymentItem
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to view a PaymentItem")
        'End If
        Return DataPortal.FetchChild(Of PaymentItem)(dr)
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
    Database.LogInfo("PaymentItem.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdPymentCategoryProperty, dr.GetInt32("IdPymentCategory"))
      LoadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty, dr.GetInt32("IdVehicleCategoryForPayments"))
      LoadProperty(Of String)(ItemNameProperty, dr.GetString("ItemName"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      Using cn As SqlConnection = Database.VTE_SqlConnection
        Using cm As SqlCommand = cn.CreateCommand
          cm.CommandType = CommandType.StoredProcedure
          cm.CommandText = "getPaymentItemParametarByIdPaymentItem"
          cm.Parameters.AddWithValue("@id", ReadProperty(Of Integer)(IdProperty))
          Using drc As New SafeDataReader(cm.ExecuteReader)
            LoadProperty(Of PaymentItemParametars) _
            (ParametarsProperty, PaymentItemParametars.GetPaymentItemParametars(drc))

          End Using
        End Using
      End Using

    Catch ex As Exception
      Database.LogException("PaymentItem.Child_Fetch", ex)
      Throw New DbCslaException("PaymentItem.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As PaymentCategorie)
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
          .Parameters.AddWithValue("@IdPymentCategory", parent.Id)
          .Parameters.AddWithValue("@IdVehicleCategoryForPayments", ReadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty))
          .Parameters.AddWithValue("@ItemName", ReadProperty(Of String)(ItemNameProperty))

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
      FieldManager.UpdateChildren(Me)
 
    Catch ex As Exception
      Database.LogException("PaymentItem.Child_Insert", ex)
      Throw New DbCslaException("PaymentItem.Child_Insert", ex)
    Finally
      Database.LogInfo("PaymentItem.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As PaymentCategorie)
    Database.LogInfo("PaymentItem.Child_Update", GetHashCode)
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
          .Parameters.AddWithValue("@IdPymentCategory", parent.Id)
          .Parameters.AddWithValue("@IdVehicleCategoryForPayments", ReadProperty(Of Integer)(IdVehicleCategoryForPaymentsProperty))
          .Parameters.AddWithValue("@ItemName", ReadProperty(Of String)(ItemNameProperty))
          .Parameters.AddWithValue("@lastChanged", _lastChanged)
          Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
          param.Direction = ParameterDirection.Output
          .Parameters.Add(param)

          .ExecuteNonQuery()

          _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
        End With
      End Using
      'update child objects
      FieldManager.UpdateChildren(Me)
    Catch ex As Exception
      Database.LogException("PaymentItem.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("PaymentItem.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("PaymentItem.Child_DeleteSelf", GetHashCode)
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
      Database.LogException("PaymentItem.Child_Fetch", ex)
      Throw New DbCslaException("PaymentItem.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
