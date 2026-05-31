Public Class dijChangeUserNameAndPass 
    Private WithEvents _user As User
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        LoadList()

        BindUI()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub LoadList()
        Try
            _user = User.GetUser(Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
        Catch ex As Exception
            MsgBox(My.Resources.NeNajaven)
        End Try

    End Sub
    Private Sub BindUI()
        _user.BeginEdit()
        Me.UserBindingSource.DataSource = _user
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSave.Click
        Using busy As New Splash(My.Resources.textSaveing)
            Try
                RebindUI(True, True)
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("")
            End Try

        End Using
    End Sub
    Private Sub RebindUI(ByVal saveObject As Boolean, ByVal rebind As Boolean)
        ' disable events
        Me.UserBindingSource.RaiseListChangedEvents = False
       
        Try
          
            UnbindBindingSource(Me.UserBindingSource, saveObject, True)

            ' save or cancel changes
            If saveObject Then
                _user.ApplyEdit()
                Try
                    'Dim tmp As Customer = _user.Clone
                    _user = _user.Save 'tmp.Save
                Catch ex As Csla.Validation.ValidationException
                    MsgBox(My.Resources.ValidationError) '("Some validation errors has occurred," + vbCrLf + " please check Broken rules collection on bottom for details")

                Catch ex As Csla.DataPortalException
                    MessageBox.Show(ex.BusinessException.ToString(), _
                      "Error saving", MessageBoxButtons.OK, _
                      MessageBoxIcon.Exclamation)

                Catch ex As Exception
                    MessageBox.Show(ex.ToString(), _
                      "Error Saving", MessageBoxButtons.OK, _
                      MessageBoxIcon.Exclamation)
                End Try
            Else
                _user.CancelEdit()
            End If
        Finally
            ' rebind UI if requested
            If rebind Then
                BindUI()
            End If

            ' restore events
            Me.UserBindingSource.RaiseListChangedEvents = True
            If rebind Then
                ' refresh the UI if rebinding
                Me.UserBindingSource.ResetBindings(False)
              
            End If

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


    'Private Sub dijChangeUserNameAndPass_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    FirstNameTextEdit.Focus()
    'End Sub
End Class