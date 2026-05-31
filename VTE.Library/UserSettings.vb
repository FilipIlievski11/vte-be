Imports System.Drawing

<Serializable()> _
Public Class UserSettings
  Inherits BusinessBase(Of UserSettings)

#Region " Business Properties and Methods "
  Private Shared SkinPorperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(UserSettings), New PropertyInfo(Of String)("Skin", "Office 2007 Blue"))
  Private Shared CultureProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(UserSettings), New PropertyInfo(Of String)("Culture", "mk-MK"))
  Private Shared FormsForntProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(UserSettings), New PropertyInfo(Of String)("FormsForntProperty"))

  Public Property Skin() As String
    Get
      Return ReadProperty(Of String)(SkinPorperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(SkinPorperty, value)
    End Set
  End Property

  Public Property Culture() As String
    Get
      Return ReadProperty(Of String)(CultureProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(CultureProperty, value)
    End Set
  End Property

  Public Property FormsFornt() As String
    Get
      Return ReadProperty(Of String)(FormsForntProperty)
    End Get
    Set(ByVal value As String)
      SetProperty(Of String)(FormsForntProperty, value)
    End Set
  End Property

#End Region


#Region " Factory Methods "

  Private Sub New()
    ' require use of factory method 
  End Sub

  Public Shared Function NewUserSettings() As UserSettings
    Return DataPortal.Create(Of UserSettings)()
  End Function

  Public Shared Function GetUserSettings() As UserSettings
        Return DataPortal.Fetch(Of UserSettings)()
  End Function

  Public Shared Sub DeleteUserSettings(ByVal id As Integer)
    DataPortal.Delete(New SingleCriteria(Of UserSettings, Integer)(id))
  End Sub
    

#End Region ' Factory Methods
    <RunLocal()> _
    Private Overloads Sub DataPortal_Fetch()
        'proveri dali postoi kluc vo HKEY_CURRENT_USER
        Dim regVersion As Microsoft.Win32.RegistryKey
        regVersion = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BSS\\VTE\\", True)
        If regVersion Is Nothing Then
            regVersion = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BSS\\VTE\\", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
        End If

        LoadProperty(Of String)(SkinPorperty, regVersion.GetValue("Skin", "Office 2007 Blue"))
        LoadProperty(Of String)(CultureProperty, regVersion.GetValue("Culture", "mk-MK"))
        LoadProperty(Of String)(FormsForntProperty, regVersion.GetValue("FontSize", "8"))

    End Sub

    <RunLocal()> _
    Protected Overrides Sub DataPortal_Update()
        'proveri dali postoi kluc vo HKEY_CURRENT_USER
        Dim regVersion As Microsoft.Win32.RegistryKey
        regVersion = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BSS\\VTE\\", True)
        If regVersion Is Nothing Then
            Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BSS\\VTE\\", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
        End If
        regVersion.SetValue("Skin", ReadProperty(Of String)(SkinPorperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("Culture", ReadProperty(Of String)(CultureProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("FontSize", ReadProperty(Of String)(FormsForntProperty), Microsoft.Win32.RegistryValueKind.String)
    End Sub

End Class
