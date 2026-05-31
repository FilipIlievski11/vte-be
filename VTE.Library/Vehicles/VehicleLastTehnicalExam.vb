
<Serializable()> _
Public Class VehicleLastTehnicalExam
  Inherits Csla.BusinessBase(Of VehicleLastTehnicalExam)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetDocumentsTehnicalExamsReportByID"
  Private Const spGetAll As String = "GetDocumentsTehnicalExamsReports"
  Private Const spUpdate As String = "updateDocumentsTehnicalExamsReport"
  Private Const spAdd As String = "addDocumentsTehnicalExamsReport"
  Private Const spDelete As String = "deleteDocumentsTehnicalExamsReport"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(VehicleLastTehnicalExam), New PropertyInfo(Of Long)("Id"))
  Private Shared IdVehicleProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(VehicleLastTehnicalExam), New PropertyInfo(Of Long)("IdVehicle"))
  Private Shared IdCustomerProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(VehicleLastTehnicalExam), New PropertyInfo(Of Long)("IdCustomer"))
  Private Shared MadeDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(VehicleLastTehnicalExam), New PropertyInfo(Of SmartDate)("MadeDate", "MadeDate", New SmartDate(DateTime.Today, True)))
  Private Shared ValidTillDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(VehicleLastTehnicalExam), New PropertyInfo(Of SmartDate)("ValidTillDate", "ValidTillDate", New SmartDate(DateTime.Today, True)))
  Private Shared IdOrganizationForTehnicalExamProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleLastTehnicalExam), New PropertyInfo(Of Integer)("IdOrganizationForTehnicalExam"))
  Private Shared IdFirsControlerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleLastTehnicalExam), New PropertyInfo(Of Integer)("IdFirsControler"))
  Private Shared IdSecondControlerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(VehicleLastTehnicalExam), New PropertyInfo(Of Integer)("IdSecondControler"))
  Private Shared VehicleIsRightProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(VehicleLastTehnicalExam), New PropertyInfo(Of Boolean)("VehicleIsRight"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Long
    Get
      Return GetProperty(Of Long)(IdProperty)
    End Get
  End Property
  Public Property IdVehicle() As Long
    Get
      Return GetProperty(Of Long)(IdVehicleProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdVehicleProperty, value)
    End Set
  End Property
  Public Property IdCustomer() As Long
    Get
      Return GetProperty(Of Long)(IdCustomerProperty)
    End Get
    Set(ByVal value As Long)
      SetProperty(Of Long)(IdCustomerProperty, value)
    End Set
  End Property
  Public Property MadeDate() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(MadeDateProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(MadeDateProperty, value)
    End Set
  End Property
  Public Property ValidTillDate() As Date
    Get
      Return GetProperty(Of SmartDate, Date)(ValidTillDateProperty)
    End Get
    Set(ByVal value As Date)
      SetProperty(Of SmartDate, Date)(ValidTillDateProperty, value)
    End Set
  End Property
  Public Property IdOrganizationForTehnicalExam() As Integer
    Get
      Return GetProperty(Of Integer)(IdOrganizationForTehnicalExamProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdOrganizationForTehnicalExamProperty, value)
    End Set
  End Property
  Public Property IdFirsControler() As Integer
    Get
      Return GetProperty(Of Integer)(IdFirsControlerProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdFirsControlerProperty, value)
    End Set
  End Property
  Public Property IdSecondControler() As Integer
    Get
      Return GetProperty(Of Integer)(IdSecondControlerProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdSecondControlerProperty, value)
    End Set
  End Property
  Public Property VehicleIsRight() As Boolean
    Get
      Return GetProperty(Of Boolean)(VehicleIsRightProperty)
    End Get
    Set(ByVal value As Boolean)
      SetProperty(Of Boolean)(VehicleIsRightProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()
        Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicle") Then
            AuthorizationRules.AllowWrite("IdVehicle", roleName)
        Else
            AuthorizationRules.DenyWrite("IdVehicle", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomer") Then
            AuthorizationRules.AllowWrite("IdCustomer", roleName)
        Else
            AuthorizationRules.DenyWrite("IdCustomer", roleName)
        End If

        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("MadeDate") Then
            AuthorizationRules.AllowWrite("MadeDate", roleName)
        Else
            AuthorizationRules.DenyWrite("MadeDate", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ValidTillDate") Then
            AuthorizationRules.AllowWrite("ValidTillDate", roleName)
        Else
            AuthorizationRules.DenyWrite("ValidTillDate", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdOrganizationForTehnicalExam") Then
            AuthorizationRules.AllowWrite("IdOrganizationForTehnicalExam", roleName)
        Else
            AuthorizationRules.DenyWrite("IdOrganizationForTehnicalExam", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdFirsControler") Then
            AuthorizationRules.AllowWrite("IdFirsControler", roleName)
        Else
            AuthorizationRules.DenyWrite("IdFirsControler", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdSecondControler") Then
            AuthorizationRules.AllowWrite("IdSecondControler", roleName)
        Else
            AuthorizationRules.DenyWrite("IdSecondControler", roleName)
        End If
        If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("MadeDate") Then
            AuthorizationRules.AllowWrite("VehicleIsRight", roleName)
        Else
            AuthorizationRules.DenyWrite("VehicleIsRight", roleName)
        End If

    End Sub
    Public Shared Function CanGetObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("VehicleLastTehnicalExam")
    End Function

    Public Shared Function CanAddObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("VehicleLastTehnicalExam")
    End Function

    Public Shared Function CanEditObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("VehicleLastTehnicalExam")
    End Function

    Public Shared Function CanDeleteObject() As Boolean
        Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("VehicleLastTehnicalExam")
    End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' MadeDateProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, MadeDateProperty)
    ' ValidTillDateProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ValidTillDateProperty)
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewVehicleLastTehnicalExamChild() As VehicleLastTehnicalExam
        If Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a VehicleLastTehnicalExam")
        End If
        Return DataPortal.CreateChild(Of VehicleLastTehnicalExam)()
  End Function

  Friend Shared Function GetVehicleLastTehnicalExam(ByVal dr As SafeDataReader) As VehicleLastTehnicalExam
        If Not CanGetObject() Then
            Throw New System.Security.SecurityException("User not authorized to view a VehicleLastTehnicalExam")
        End If
        Return DataPortal.FetchChild(Of VehicleLastTehnicalExam)(dr)
  End Function

    Public Shared Sub DeleteVehicleGearBox(ByVal id As Integer)
        If Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a VehicleLastTehnicalExam")
        End If
        DataPortal.Delete(New SingleCriteria(Of VehicleLastTehnicalExam, Integer)(id))
    End Sub


    Public Overrides Function Save() As VehicleLastTehnicalExam
        If IsDeleted AndAlso Not CanDeleteObject() Then
            Throw New System.Security.SecurityException("User not authorized to remove a VehicleLastTehnicalExam")
        ElseIf IsNew AndAlso Not CanAddObject() Then
            Throw New System.Security.SecurityException("User not authorized to add a VehicleLastTehnicalExam")
        ElseIf Not CanEditObject() Then
            Throw New System.Security.SecurityException("User not authorized to update a VehicleLastTehnicalExam")
        End If
        Return MyBase.Save()
    End Function

  Private Sub New()
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Create "
  Protected Overloads Sub Child_Create()
  End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    Database.LogInfo("VehicleLastTehnicalExam.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Long)(IdVehicleProperty, dr.GetInt64("IdVehicle"))
      LoadProperty(Of Long)(IdCustomerProperty, dr.GetInt64("IdCustomer"))
      LoadProperty(Of SmartDate, Date?)(MadeDateProperty, dr.GetSmartDate("MadeDate", True))
      LoadProperty(Of SmartDate, Date?)(ValidTillDateProperty, dr.GetSmartDate("ValidTillDate", True))
      LoadProperty(Of Integer)(IdOrganizationForTehnicalExamProperty, dr.GetInt32("IdOrganizationForTehnicalExam"))
      LoadProperty(Of Integer)(IdFirsControlerProperty, dr.GetInt32("IdFirsControler"))
      LoadProperty(Of Integer)(IdSecondControlerProperty, dr.GetInt32("IdSecondControler"))
      LoadProperty(Of Boolean)(VehicleIsRightProperty, dr.GetBoolean("VehicleIsRight"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("VehicleLastTehnicalExam.Child_Fetch", ex)
      Throw New DbCslaException("VehicleLastTehnicalExam.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As Vehicle)
    Try
      Using cn As SqlConnection = ApplicationContext.LocalContext("cn")
        If cn.State = ConnectionState.Closed Then
          cn.ConnectionString = Database.VTEConnection
          cn.Open()
        End If
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spAdd
            'Smeni go Id so Parent.Id
            .Parameters.AddWithValue("@IdVehicle", parent.Id)
            .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
            .Parameters.AddWithValue("@MadeDate", ReadProperty(Of SmartDate)(MadeDateProperty).DBValue)
            .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
            .Parameters.AddWithValue("@IdOrganizationForTehnicalExam", ReadProperty(Of Integer)(IdOrganizationForTehnicalExamProperty))
            .Parameters.AddWithValue("@IdFirsControler", ReadProperty(Of Integer)(IdFirsControlerProperty))
            .Parameters.AddWithValue("@IdSecondControler", ReadProperty(Of Integer)(IdSecondControlerProperty))
            .Parameters.AddWithValue("@VehicleIsRight", ReadProperty(Of Boolean)(VehicleIsRightProperty))

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
      Database.LogException("VehicleLastTehnicalExam.Child_Insert", ex)
      Throw New DbCslaException("VehicleLastTehnicalExam.Child_Insert", ex)
    Finally
      Database.LogInfo("VehicleLastTehnicalExam.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Vehicle)
    Database.LogInfo("VehicleLastTehnicalExam.Child_Update", GetHashCode)
    Try
      Using cn As SqlConnection = ApplicationContext.LocalContext("cn")
        If cn.State = ConnectionState.Closed Then
          cn.ConnectionString = Database.VTEConnection
          cn.Open()
        End If
        Using cm As SqlCommand = cn.CreateCommand
          With cm
            .CommandType = CommandType.StoredProcedure
            .CommandText = spUpdate

            .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
            .Parameters.AddWithValue("@IdVehicle", parent.Id)
            .Parameters.AddWithValue("@IdCustomer", ReadProperty(Of Long)(IdCustomerProperty))
            .Parameters.AddWithValue("@MadeDate", ReadProperty(Of SmartDate)(MadeDateProperty).DBValue)
            .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
            .Parameters.AddWithValue("@IdOrganizationForTehnicalExam", ReadProperty(Of Integer)(IdOrganizationForTehnicalExamProperty))
            .Parameters.AddWithValue("@IdFirsControler", ReadProperty(Of Integer)(IdFirsControlerProperty))
            .Parameters.AddWithValue("@IdSecondControler", ReadProperty(Of Integer)(IdSecondControlerProperty))
            .Parameters.AddWithValue("@VehicleIsRight", ReadProperty(Of Boolean)(VehicleIsRightProperty))
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
      Database.LogException("VehicleLastTehnicalExam.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("VehicleLastTehnicalExam.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("VehicleLastTehnicalExam.Child_DeleteSelf", GetHashCode)
    Try
      Using cn As SqlConnection = Csla.ApplicationContext.LocalContext("cn")
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

      End Using
    Catch ex As Exception
      Database.LogException("VehicleLastTehnicalExam.Child_Fetch", ex)
      Throw New DbCslaException("VehicleLastTehnicalExam.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
