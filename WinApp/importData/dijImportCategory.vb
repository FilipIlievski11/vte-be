Imports System.Data.OleDb
Public Class dijImportCategory
  Private WithEvents _colors As Colors
  'Private WithEvents tabela As DataTable = New DataTable
  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    _colors = Colors.GetColors

    ' Add any initialization after the InitializeComponent() call.

  End Sub

  Private Sub BindUI()
    Me.ColorsBindingSource.DataSource = _colors
  End Sub
  Private Sub parseXLSOleDD(ByVal strPath As String)
    Dim connectionString = _
      "Provider=Microsoft.Jet.OLEDB.4.0;" & _
      "Data Source=" & strPath & ";" & _
      "Extended Properties=Excel 8.0;"
    Using cn As New OleDbConnection(connectionString)
      cn.Open()
      Using cnSQL As New SqlClient.SqlConnection(Database.VTEConnection)
        cnSQL.Open()
        Using cm As OleDbCommand = cn.CreateCommand
          cm.CommandType = CommandType.Text
          cm.CommandText = "SELECT * FROM [каталог на бои$]"
          Using dr As OleDbDataReader = cm.ExecuteReader
            While dr.Read
              'ako SID e broj (praznite meseta se drugi redovi)
              If IsNumeric(dr(0)) Then
                ExecuteInsert(cnSQL, dr)
              End If
            End While
          End Using
        End Using
        'za vtorite strani
        Using cm As OleDbCommand = cn.CreateCommand
          cm.CommandType = CommandType.Text
          cm.CommandText = "SELECT * FROM [каталог на бои 2$]"
          Using dr As OleDbDataReader = cm.ExecuteReader
            While dr.Read
              'ako SID e broj (praznite meseta se drugi redovi)
              If IsNumeric(dr(0)) Then
                ExecuteInsert(cnSQL, dr)
              End If
            End While
          End Using
        End Using

      End Using
    End Using
  End Sub

  Private Sub ExecuteInsert(ByVal cn As SqlClient.SqlConnection, ByVal drInput As OleDbDataReader)

    Using cm As SqlClient.SqlCommand = cn.CreateCommand()
      cm.CommandType = CommandType.StoredProcedure
      cm.CommandText = "addColor"

      'cm.Parameters.AddWithValue("@ID_TSA_PERSON_TYPE", intPersonType)
      cm.Parameters.AddWithValue("@ColorCode", drInput(0))
      'If (Not IsDBNull(drInput(1))) AndAlso (drInput(1) = "YES") Then
      '  cm.Parameters.AddWithValue("@CLEARED", True)
      'Else
      '  cm.Parameters.AddWithValue("@CLEARED", False)
      'End If
      cm.Parameters.AddWithValue("@ColorDescription", drInput(1))
      cm.Parameters.AddWithValue("@NewColorEffects", drInput(2))
      cm.Parameters.AddWithValue("@NewColorCode", drInput(3))
      cm.Parameters.AddWithValue("@NewColorDarkness", drInput(4))
      Dim param As New SqlClient.SqlParameter("@newId", SqlDbType.Int)
      param.Direction = ParameterDirection.Output
      cm.Parameters.Add(param)
      param = New SqlClient.SqlParameter("@newLastChanged", SqlDbType.Timestamp)
      param.Direction = ParameterDirection.Output
      cm.Parameters.Add(param)

      cm.ExecuteNonQuery()

      'LoadProperty(Of Integer)(IdProperty, CInt(.Parameters("@newId").Value))
      '_lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())

      ''Dim paramPK As New SqlClient.SqlParameter("@new_ID", SqlDbType.Int)
      'paramPK.Direction = ParameterDirection.Output
      'cm.Parameters.Add(paramPK)
      'Dim paramTS As New SqlClient.SqlParameter("@new_POSLEDNA_IZMENA", SqlDbType.Timestamp)
      'paramTS.Direction = ParameterDirection.Output
      'cm.Parameters.Add(paramTS)

      'Dim ouptutID = CType(cm.Parameters("@new_ID").Value, Long)
      'Dim ouptutposlednaIzmena = CType(cm.Parameters("@new_POSLEDNA_IZMENA").Value, Byte())
    End Using

  End Sub

  Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
    Try
      Me.OpenFileDialog1.Filter = "excel files only (*.xls)| *.xls"
      If Not (Me.OpenFileDialog1.ShowDialog = DialogResult.Cancel) Then
        Dim pateka As String = OpenFileDialog1.FileName
        Me.txtPath.Text = pateka
      Else
        Me.txtPath.Text = String.Empty
      End If

    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub

  Protected Sub UnbindBindingSource( _
    ByVal source As BindingSource, ByVal apply As Boolean, ByVal isRoot As Boolean)

    Dim current As System.ComponentModel.IEditableObject = _
            TryCast(source.Current, System.ComponentModel.IEditableObject)
    If isRoot Then
      source.DataSource = Nothing
    End If
    If current IsNot Nothing Then
      If apply Then
        current.EndEdit()
      Else
        current.CancelEdit()
      End If
    End If

  End Sub
  Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
    ' stop the flow of events
    Me.ColorsBindingSource.RaiseListChangedEvents = False
    ' commit edits in memory
    UnbindBindingSource(Me.ColorsBindingSource, saveObject, True)
    Try
      ' save or cancel changes
      If saveObject Then
        Try
          _colors = _colors.Save
        Catch ex As Csla.DataPortalException
          MessageBox.Show(ex.BusinessException.ToString, _
            "Error saving", MessageBoxButtons.OK, _
            MessageBoxIcon.Exclamation)

        Catch ex As Exception
          MessageBox.Show(ex.ToString, _
            "Error saving", MessageBoxButtons.OK, _
            MessageBoxIcon.Exclamation)
        End Try
      Else
        _colors = Nothing
        Try
          _colors = Colors.GetColors
        Catch ex As Csla.DataPortalException
          MessageBox.Show(ex.BusinessException.ToString, _
            "Error saving", MessageBoxButtons.OK, _
            MessageBoxIcon.Exclamation)

        Catch ex As Exception
          MessageBox.Show(ex.ToString, _
            "Error saving", MessageBoxButtons.OK, _
            MessageBoxIcon.Exclamation)
        End Try
      End If
    Finally
      Me.ColorsBindingSource.DataSource = _colors
      Me.ColorsBindingSource.RaiseListChangedEvents = True
      If rebind Then
        Me.ColorsBindingSource.ResetBindings(False)
      End If

    End Try
  End Sub

  Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
    ' parseModifyXLSOleDD(txtPath.Text, 3, "Selectee")

    Console.WriteLine("Vcitaj lista:" & Now)
    _colors = Colors.GetColors
    BindUI()
    'RebindUI(True, True)
  End Sub
  Private Sub ImportOnly(ByVal strPath As String)
    Try
      'Using busy As New uxStatus("Importing...")
      'iscisti ja skors bazata
      'Console.WriteLine("pocna brisi baza vo:" & Now)
      'Using cn As SqlConnection = Database.VTE_SqlConnection
      '  cn.Open()
      '  Using cm As SqlClient.SqlCommand = cn.CreateCommand
      '    cm.CommandType = CommandType.StoredProcedure
      '    cm.CommandText = "TSA_LIST_DELETE_PO_ID_PERSON_TYPE"
      '    cm.Parameters.AddWithValue("@personType", intPersonType)
      '    cm.ExecuteNonQuery()
      '  End Using
      'End Using
      parseXLSOleDD(strPath)
      Console.WriteLine("Vcitaj lista:" & Now)
      _colors = Colors.GetColors
      BindUI()
      Console.WriteLine("Zavrsi vo:" & Now)
      'End Using
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try
  End Sub
End Class