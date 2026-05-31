<Serializable()> _
Public Class VehicleFieldList
  Inherits NameValueListBase(Of String, String)

#Region " Business Methods "

  Public Shared Function DefaultField() As Integer

    Dim list As VehicleFieldList = GetList()
    If list.Count > 0 Then
      Return list.Items(0).Key

    Else
      Throw New NullReferenceException( _
        "No roles available; default role can not be returned")
    End If

  End Function

#End Region

#Region " Factory Methods "

  Private Shared mList As VehicleFieldList

  Public Shared Function EmptyList() As VehicleFieldList
    Return New VehicleFieldList
  End Function

  Public Shared Function GetList() As VehicleFieldList

    If mList Is Nothing Then
      mList = DataPortal.Fetch(Of VehicleFieldList) _
        (New Criteria(GetType(VehicleFieldList)))
    End If
    Return mList

  End Function

  Public Shared Function GetListByIdCategory(ByVal idCategory As Integer) As VehicleFieldList
    Return DataPortal.Fetch(Of VehicleFieldList)(New SingleCriteria(Of VehicleFieldList, Integer)(idCategory))

  End Function
  ''' <summary>
  ''' Clears the in-memory RoleList cache
  ''' so the list of roles is reloaded on
  ''' next request.
  ''' </summary>
  Public Shared Sub InvalidateCache()

    mList = Nothing

  End Sub

  Private Sub New()
    ' require use of factory methods
  End Sub

#End Region

#Region " Data Access "

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of VehicleFieldList, Integer))
    Me.RaiseListChangedEvents = False
    Using cn As SqlConnection = Database.VTE_SqlConnection
      Using cm As SqlCommand = cn.CreateCommand
        cm.CommandType = CommandType.StoredProcedure
        cm.CommandText = "getVehicleRequiredFieldByIdCategory"
        cm.Parameters.AddWithValue("@id", CInt(criteria.Value))
        Using dr As New SafeDataReader(cm.ExecuteReader)
          IsReadOnly = False
          With dr
            While .Read()
              Me.Add(New NameValuePair( _
                .GetString("FieldName"), .GetString("FieldName")))
            End While
          End With
          IsReadOnly = True
        End Using
      End Using
    End Using
    Me.RaiseListChangedEvents = True
  End Sub

  Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

    Me.RaiseListChangedEvents = False
    IsReadOnly = False
    Dim res As New Resources.ResourceManager("VTE.Library.VehicleFieldsResource", GetType(VehicleFieldList).Assembly)

    'nema 
    Me.Add(New NameValuePair("Null", "[Нема]"))

    For Each prop As System.Reflection.PropertyInfo In GetType(Vehicle).GetProperties

      If prop.CanWrite Then
        Select Case prop.Name
          Case "IdVehicleCategoryForPayments"
          Case Else
            Me.Add(New NameValuePair( _
              prop.Name, res.GetString(prop.Name)))
        End Select
      End If
      'If prop.FriendlyName <> String.Empty Then

      'End If

    Next
    'dodadi i za Nosivost oti e readonly
    'Me.Add(New NameValuePair("CarringCapacity", "Носивост"))

    IsReadOnly = True

    Me.RaiseListChangedEvents = True

  End Sub

#End Region

End Class
