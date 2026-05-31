
<Serializable()> _
Public Class ColorDetail
  Inherits Csla.BusinessBase(Of ColorDetail)

#Region " Stored Procedures Names "
  Private Const spGetByID As String = "GetColorsDetailByID"
  Private Const spGetAll As String = "GetColorsDetails"
  Private Const spUpdate As String = "updateColorsDetail"
  Private Const spAdd As String = "addColorsDetail"
  Private Const spDelete As String = "deleteColorsDetail"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
  'register properties
  Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(ColorDetail), New PropertyInfo(Of Integer)("Id"))
  Private Shared IdColorProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(ColorDetail), New PropertyInfo(Of Integer)("IdColor"))
  Private Shared ColorCodeProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(ColorDetail), New PropertyInfo(Of String)("ColorCode"))
  Private Shared ColorDescriptionProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(ColorDetail), New PropertyInfo(Of String)("ColorDescription"))

  Private _lastChanged(7) As Byte

  <System.ComponentModel.DataObjectField(True, True)> _
  Public ReadOnly Property Id() As Integer
    Get
      Return GetProperty(Of Integer)(IdProperty)
    End Get
  End Property
  Public Property IdColor() As Integer
    Get
      Return GetProperty(Of Integer)(IdColorProperty)
    End Get
    Set(ByVal value As Integer)
      SetProperty(Of Integer)(IdColorProperty, value)
    End Set
  End Property
  Public Property ColorCode() As String
    Get
      Return GetProperty(Of String)(ColorCodeProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(ColorCodeProperty, value)
    End Set
  End Property
  Public Property ColorDescription() As String
    Get
      Return GetProperty(Of String)(ColorDescriptionProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(ColorDescriptionProperty, value)
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Id.ToString
  End Function
#End Region 'Business Properties and Methods



#Region " Validation Rules "
  Protected Overrides Sub AddBusinessRules()
    ' ColorCodeProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ColorCodeProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(ColorCodeProperty, 50))
    ' ColorDescriptionProperty rules
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ColorDescriptionProperty)
    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs(ColorDescriptionProperty, 250))
  End Sub
#End Region ' Validation Rules

#Region " Factory Methods "

  Friend Shared Function NewColorDetailChild() As ColorDetail
    Return DataPortal.CreateChild(Of ColorDetail)()
  End Function

  Friend Shared Function GetColorDetail(ByVal dr As SafeDataReader) As ColorDetail
    Return DataPortal.FetchChild(Of ColorDetail)(dr)
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
    Database.LogInfo("ColorDetail.Child_Fetch", GetHashCode())
    Try

      LoadProperty(Of Integer)(IdProperty, dr.GetInt32("Id"))
      LoadProperty(Of Integer)(IdColorProperty, dr.GetInt32("IdColor"))
      LoadProperty(Of String)(ColorCodeProperty, dr.GetString("ColorCode"))
      LoadProperty(Of String)(ColorDescriptionProperty, dr.GetString("ColorDescription"))

      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)

      ValidationRules.CheckRules()

    Catch ex As Exception
      Database.LogException("ColorDetail.Child_Fetch", ex)
      Throw New DbCslaException("ColorDetail.Child_Fetch", ex)
    End Try

  End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

  Private Sub Child_Insert(ByVal parent As Color)
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
            .Parameters.AddWithValue("@IdColor", parent.Id)
            .Parameters.AddWithValue("@ColorCode", ReadProperty(Of String)(ColorCodeProperty))
            .Parameters.AddWithValue("@ColorDescription", ReadProperty(Of String)(ColorDescriptionProperty))

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
      End Using
    Catch ex As Exception
      Database.LogException("ColorDetail.Child_Insert", ex)
      Throw New DbCslaException("ColorDetail.Child_Insert", ex)
    Finally
      Database.LogInfo("ColorDetail.Child_Insert", GetHashCode)
    End Try

  End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

  Private Sub Child_Update(ByVal parent As Color)
    Database.LogInfo("ColorDetail.Child_Update", GetHashCode)
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

            .Parameters.AddWithValue("@Id", parent.Id)
            .Parameters.AddWithValue("@IdColor", ReadProperty(Of Integer)(IdColorProperty))
            .Parameters.AddWithValue("@ColorCode", ReadProperty(Of String)(ColorCodeProperty))
            .Parameters.AddWithValue("@ColorDescription", ReadProperty(Of String)(ColorDescriptionProperty))
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
      Database.LogException("ColorDetail.Child_Update", ex)
      If Not ex.Message.EndsWith("drug korisnik") Then
        Throw New DbCslaException("ColorDetail.Child_Update", ex)
      End If
    End Try
  End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

  Private Sub Child_DeleteSelf()

    Database.LogInfo("ColorDetail.Child_DeleteSelf", GetHashCode)
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
            .Parameters.AddWithValue("@id", ReadProperty(Of Integer)(IdProperty))
            .ExecuteNonQuery()
          End With
        End Using

      End Using
    Catch ex As Exception
      Database.LogException("ColorDetail.Child_Fetch", ex)
      Throw New DbCslaException("ColorDetail.Child_Fetch", ex)
    End Try
  End Sub

#End Region ' Data Access - Delete

#End Region ' Data Access

End Class
