Imports Microsoft.Win32
Public Class Login
 Private _VerifyIdentityOnly As Boolean = False
 Private _idEmployee As Integer = 0
 Public ReadOnly Property VerifyIdentityEmployeeId() As Integer
  Get
   Return _idEmployee
  End Get
 End Property

 Public Sub New()
  MyBase.New()
  'This call is required by the Windows Form Designer.
  InitializeComponent()
  'Zastita()
  'Add any initialization after the InitializeComponent() call

 End Sub

 'se koristi samo da proveri dali postoi toj user, 
 'ne go menuva momentalniot user
 Public Sub New(ByVal VerifyIdentityOnly As Boolean)
  MyBase.New()
  'This call is required by the Windows Form Designer.
  InitializeComponent()
  'Zastita()
  'Add any initialization after the InitializeComponent() call
  _VerifyIdentityOnly = VerifyIdentityOnly
 End Sub


 Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
  Try
    If Not _VerifyIdentityOnly Then
      VTE.Library.Security.VTEPrincipal.Login(Me.UsernameTextBox.Text, Me.PasswordTextBox.Text)
      If Not Csla.ApplicationContext.User.Identity.IsAuthenticated Then
        MsgBox("Wrong username or password")
        UsernameTextBox.Focus()
        Exit Sub
      End If
    Else
      _idEmployee = VTE.Library.Security.VTEPrincipal.VerifyOtherIdentity(Me.UsernameTextBox.Text, Me.PasswordTextBox.Text)
      If _idEmployee = 0 Then
        MsgBox("Wrong username or password")
        UsernameTextBox.Focus()
        Exit Sub
      End If
    End If
    Me.DialogResult = Windows.Forms.DialogResult.OK
    Me.Close()
  Catch ex As Exception
    System.IO.File.WriteAllText("C:\Users\FilipIlievski\Desktop\oldapp-error.txt", "Login click: " & ex.ToString())
    MsgBox("Login error - check Desktop\oldapp-error.txt")
  End Try
 End Sub


 Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
  'End
  Me.Close()
 End Sub

 Private Sub Login_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

  Select Case Asc(e.KeyChar)
   Case 13
    e.Handled = True
    SendKeys.Send("{TAB}")
  End Select
 End Sub

 Private Sub Login_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load, Me.Activated, Me.GotFocus
  Me.UsernameTextBox.Text = ""
  Me.PasswordTextBox.Text = ""
  Me.UsernameTextBox.Focus()

 End Sub
 Private Sub Zastita()

  UsernameTextBox.Text = ""
  PasswordTextBox.Text = ""
  Try
   Dim cry As New Crypt("VTEBSS")
   Dim sn As String = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE").GetValue("SN", "")
   Dim hinfo As String = (cry.Encrypt(HardwareInfo.GetCPU) & cry.Encrypt(HardwareInfo.GetBIOS) & cry.Encrypt(HardwareInfo.GetMAC))
   Dim dato As String = cry.Decrypt(Strings.Left(sn, 32))

   If Strings.Right(sn, sn.Length - 32) = (cry.Encrypt(HardwareInfo.GetCPU) & cry.Encrypt(HardwareInfo.GetBIOS) & cry.Encrypt(HardwareInfo.GetMAC)) Then
    If CDate(dato) < Now.Date Then
     MsgBox(My.Resources.EndTest, MsgBoxStyle.Information, My.Resources.Information)
     '     MsgBox("������ ���� ��������", MsgBoxStyle.Information, "����������")
     End
    End If

   Else
    'Console.WriteLine(Strings.Right((cry.Encrypt(HardwareInfo.GetCPU) & cry.Encrypt(HardwareInfo.GetBIOS) & cry.Encrypt(HardwareInfo.GetHDD) & cry.Encrypt(HardwareInfo.GetMAC)), sn.Length - 32))
    MsgBox(My.Resources.ProblemPC, MsgBoxStyle.Information, My.Resources.Information)
    ' MsgBox("������� ��", MsgBoxStyle.Information, "����������")
    End
   End If

  Catch ex As Exception
   MsgBox(ex.Message)
   End
  End Try
 End Sub
End Class