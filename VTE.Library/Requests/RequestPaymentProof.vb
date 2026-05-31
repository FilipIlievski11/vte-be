
<Serializable()> _
Public Class RequestPaymentProof
  Inherits Csla.BusinessBase(Of RequestPaymentProof)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetRequestPaymentProoByID"
  Private Const spGetAll As String = "GetRequestPaymentProof"
  Private Const spUpdate As String = "updateRequestPaymentProo"
  Private Const spAdd As String = "addRequestPaymentProo"
  Private Const spDelete As String = "deleteRequestPaymentProo"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(RequestPaymentProof), New PropertyInfo(Of Long)("Id"))
  Private Shared IdRequestProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(RequestPaymentProof), New PropertyInfo(Of Long)("IdRequest"))
  Private Shared IdPaymentProofProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(RequestPaymentProof), New PropertyInfo(Of Integer)("IdPaymentProof"))
  Private Shared PaymentProofProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(RequestPaymentProof), New PropertyInfo(Of String)("PaymentProof"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
    End Get
  End Property
  Public Property IdRequest() As Long
    Get
      Return GetProperty(Of Long)(IdRequestProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdRequestProperty, value)
    End Set
  End Property
  Public Property IdPaymentProof() As Integer
    Get
      Return GetProperty(Of Integer)(IdPaymentProofProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdPaymentProofProperty, value)
    End Set
  End Property
  Public Property PaymentProof() As String
    Get
      Return GetProperty(Of String)(PaymentProofProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(PaymentProofProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdRequest") Then
            AuthorizationRules.AllowWrite("IdRequest", roleName)
        Else
            AuthorizationRules.DenyWrite("IdRequest", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdPaymentProof") Then
            AuthorizationRules.AllowWrite("IdPaymentProof", roleName)
        Else
            AuthorizationRules.DenyWrite("IdPaymentProof", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("PaymentProof") Then
            AuthorizationRules.AllowWrite("PaymentProof", roleName)
        Else
            AuthorizationRules.DenyWrite("PaymentProof", roleName)
        End If
        
    End Sub


    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("RequestPaymentProof")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("RequestPaymentProof")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("RequestPaymentProof")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("RequestPaymentProof")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' PaymentProofProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(PaymentProofProperty, 50))

    ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                            New Validation.IntegerMinValueRuleArgs(IdPaymentProofProperty, 1))

  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewRequestPaymentProofChild() As RequestPaymentProof
        'If Not CanAddObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a RequestPaymentProof")
        'End If
        Return DataPortal.CreateChild(Of RequestPaymentProof)()
  End Function

  Friend Shared Function GetRequestPaymentProof(ByVal dr As SafeDataReader) As RequestPaymentProof
        'If Not CanGetObject() Then
        '    Throw New System.Security.SecurityException("User not authorized to add a RequestPaymentProof")
        'End If
        Return DataPortal.FetchChild(Of RequestPaymentProof)(dr)
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
    Database.LogInfo("RequestPaymentProof.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Long)(IdRequestProperty, dr.GetInt64("IdRequest"))
      LoadProperty(Of Integer)(IdPaymentProofProperty, dr.GetInt32("IdPaymentProof"))
      LoadProperty(Of String)(PaymentProofProperty, dr.GetString("PaymentProof"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("RequestPaymentProof.Child_Fetch", ex)
      Throw New DbCslaException("RequestPaymentProof.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As Request)
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
          .Parameters.AddWithValue("@IdRequest", parent.Id)
          .Parameters.AddWithValue("@IdPaymentProof", ReadProperty(Of Integer)(IdPaymentProofProperty))
          .Parameters.AddWithValue("@PaymentProof", ReadProperty(Of String)(PaymentProofProperty))

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

    Catch ex As Exception
      Database.LogException("RequestPaymentProof.Child_Insert", ex)
      Throw New DbCslaException("RequestPaymentProof.Child_Insert", ex)
    Finally
      Database.LogInfo("RequestPaymentProof.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Request)
    Database.LogInfo("RequestPaymentProof.Child_Update", GetHashCode)
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

          .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
          .Parameters.AddWithValue("@IdRequest", parent.Id)
          .Parameters.AddWithValue("@IdPaymentProof", ReadProperty(Of Integer)(IdPaymentProofProperty))
          .Parameters.AddWithValue("@PaymentProof", ReadProperty(Of String)(PaymentProofProperty))
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
      Database.LogException("RequestPaymentProof.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("RequestPaymentProof.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("RequestPaymentProof.Child_DeleteSelf", GetHashCode)
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
          .Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
          .ExecuteNonQuery()
        End With
      End Using


    Catch ex As Exception
      Database.LogException("RequestPaymentProof.Child_Fetch", ex)
      Throw New DbCslaException("RequestPaymentProof.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
