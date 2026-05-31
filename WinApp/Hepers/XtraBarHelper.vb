Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.XtraBars
Imports DevExpress.LookAndFeel
Imports DevExpress.Skins

Namespace LookAndFeelMenu
  Class XtraBarsHelper
    'public static BarSubItem CreateLookAndFeelMenu(BarManager manager) {
    ' BarSubItem menu = new BarSubItem(manager, "&Look&&Feel");
    ' //BarButtonItem flat = new BarButtonItem(manager, "
    ' }


    Private m_miLookAndFeel As BarSubItem, miSkin As BarSubItem
    'CheckBarItem miAllowFormSkins;

    Private m_manager As BarManager
    Public ReadOnly Property Manager() As BarManager
      Get
        Return m_manager
      End Get
    End Property

    Private m_lookAndFeel As DefaultLookAndFeel
    Public ReadOnly Property LookAndFeel() As DefaultLookAndFeel
      Get
        Return m_lookAndFeel
      End Get
    End Property

    'public Form MenuForm {
    ' get {
    ' if(manager != null)
    ' return manager.Form as Form;
    ' return null;
    ' }
    ' }


    Public ReadOnly Property MILookAndFeel() As BarSubItem
      Get
        Return m_miLookAndFeel
      End Get
    End Property

    Public Sub New(ByVal manager As BarManager, ByVal lookAndFeel As DefaultLookAndFeel)
      If manager Is Nothing Then
        Throw New ArgumentNullException("manager")
      End If
      If lookAndFeel Is Nothing Then
        Throw New ArgumentNullException("lookAndFeel")
      End If

      Me.m_lookAndFeel = lookAndFeel
      Me.m_manager = manager

      'this.manager.Images = DevExpress.Utils.Controls.ImageHelper.CreateImageCollectionFromResources("DevExpress.Tutorials.MainDemo.menu.bmp", typeof(LookAndFeelMenu).Assembly, new Size(16, 16), Color.Magenta);
      Me.m_manager.ForceInitialize()
      SetupMenu()
    End Sub

    Private Sub SetupMenu()
            m_miLookAndFeel = New BarSubItem(Manager, "&Look And Feel")

            'trgnato izbor na skin
   m_miLookAndFeel.Visibility = BarItemVisibility.Always

      'miAllowFormSkins = new CheckBarItem(Manager, "Allow Form Skins", new ItemClickEventHandler(OnSwitchFormSkinStyle_Click));
      'miLookAndFeel.ItemLinks.Add(miAllowFormSkins);
      m_miLookAndFeel.ItemLinks.Add(New CheckBarItemWithStyle(Manager, "&Flat", New ItemClickEventHandler(AddressOf OnSwitchStyle_Click), ActiveLookAndFeelStyle.Flat, LookAndFeelStyle.Flat)).BeginGroup = True
      m_miLookAndFeel.ItemLinks.Add(New CheckBarItemWithStyle(Manager, "&Ultra Flat", New ItemClickEventHandler(AddressOf OnSwitchStyle_Click), ActiveLookAndFeelStyle.UltraFlat, LookAndFeelStyle.UltraFlat))
      m_miLookAndFeel.ItemLinks.Add(New CheckBarItemWithStyle(Manager, "&Style3D", New ItemClickEventHandler(AddressOf OnSwitchStyle_Click), ActiveLookAndFeelStyle.Style3D, LookAndFeelStyle.Style3D))
      m_miLookAndFeel.ItemLinks.Add(New CheckBarItemWithStyle(Manager, "&Office2003", New ItemClickEventHandler(AddressOf OnSwitchStyle_Click), ActiveLookAndFeelStyle.Office2003, LookAndFeelStyle.Office2003))
      m_miLookAndFeel.ItemLinks.Add(New CheckBarItemWithStyle(Manager, "&XP", New ItemClickEventHandler(AddressOf OnSwitchStyle_Click), ActiveLookAndFeelStyle.WindowsXP, LookAndFeelStyle.Skin))
      miSkin = New BarSubItem(Manager, "S&kin")
      AddHandler m_miLookAndFeel.Popup, AddressOf OnPopupLookAndFeel
      AddHandler miSkin.Popup, AddressOf OnPopupSkinNames
      For Each cnt As SkinContainer In SkinManager.[Default].Skins
        miSkin.ItemLinks.Add(New CheckBarItem(Manager, cnt.SkinName, New ItemClickEventHandler(AddressOf OnSwitchSkin), ActiveLookAndFeelStyle.Skin))
      Next

      m_miLookAndFeel.ItemLinks.Add(miSkin)

      If Manager.MainMenu IsNot Nothing Then
        Manager.MainMenu.ItemLinks.Add(m_miLookAndFeel)
      End If

      For Each item As BarItemLink In m_miLookAndFeel.ItemLinks
        Dim aItem As CheckBarItemWithStyle = TryCast(item.Item, CheckBarItemWithStyle)
        If aItem IsNot Nothing AndAlso aItem.LookAndFeelStyle = LookAndFeelStyle.Skin Then
          aItem.Enabled = DevExpress.Utils.WXPaint.Painter.ThemesEnabled
        End If
      Next
    End Sub

    'protected virtual void OnSwitchFormSkinStyle_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
    ' if(DevExpress.Skins.SkinManager.AllowFormSkins)
    ' DevExpress.Skins.SkinManager.DisableFormSkins();
    ' else
    ' DevExpress.Skins.SkinManager.EnableFormSkins();
    ' DevExpress.LookAndFeel.LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
    ' }


    Private ReadOnly Property UsingXP() As Boolean
      Get
        Return LookAndFeel.LookAndFeel.UseWindowsXPTheme AndAlso DevExpress.Utils.WXPaint.Painter.ThemesEnabled
      End Get
    End Property

    Private Function AvailableStyle(ByVal style As LookAndFeelStyle) As Boolean
      Return m_lookAndFeel.LookAndFeel.Style = style AndAlso Not UsingXP
    End Function

    Private Sub OnPopupSkinNames(ByVal sender As Object, ByVal e As EventArgs)
      Dim items As BarSubItem = TryCast(sender, BarSubItem)
      For Each item As BarItemLink In items.ItemLinks
        Dim aItem As CheckBarItem = TryCast(item.Item, CheckBarItem)
        If aItem IsNot Nothing Then
          aItem.Checked = AvailableStyle(LookAndFeelStyle.Skin) AndAlso LookAndFeel.LookAndFeel.SkinName = item.Caption
        End If
      Next
    End Sub

    Private Sub OnPopupLookAndFeel(ByVal sender As Object, ByVal e As EventArgs)
      Dim items As BarSubItem = TryCast(sender, BarSubItem)
      For Each item As BarItemLink In items.ItemLinks
        Dim aItem As CheckBarItemWithStyle = TryCast(item.Item, CheckBarItemWithStyle)
        If aItem IsNot Nothing Then
          If aItem.LookAndFeelStyle = LookAndFeelStyle.Skin Then
            aItem.Checked = UsingXP
          Else
            aItem.Checked = AvailableStyle(aItem.LookAndFeelStyle)
          End If
        End If
      Next
      'miAllowFormSkins.Checked = DevExpress.Skins.SkinManager.AllowFormSkins;
    End Sub

    Private Sub OnSwitchSkin(ByVal sender As Object, ByVal e As ItemClickEventArgs)
      OnSwitchStyle_Click(sender, e)
      If LookAndFeel IsNot Nothing Then
        LookAndFeel.LookAndFeel.SetSkinStyle(e.Item.Caption)
        uSettings.Skin = e.Item.Caption
        uSettings.Save()
        'Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("TAXI", True).SetValue("SKIN_NAME", e.Item.Caption, Microsoft.Win32.RegistryValueKind.String)
      End If
    End Sub

    Protected Overridable Sub OnSwitchStyle_Click(ByVal sender As Object, ByVal e As ItemClickEventArgs)
      Dim item As CheckBarItem = TryCast(e.Item, CheckBarItem)
      Dim wxp As Boolean = item.Style = ActiveLookAndFeelStyle.WindowsXP
      LookAndFeel.LookAndFeel.SetStyle(DirectCast(item.Style, LookAndFeelStyle), wxp, LookAndFeel.LookAndFeel.UseDefaultLookAndFeel, LookAndFeel.LookAndFeel.SkinName)

    End Sub

    Private Class OptionBarItem
      Inherits BarCheckItem
      Private optionItem As Boolean = False
      Public Sub New(ByVal manager As BarManager, ByVal text As String, ByVal handler As ItemClickEventHandler)
        Me.New(manager, text, handler, True)
      End Sub
      Public Sub New(ByVal manager As BarManager, ByVal text As String, ByVal handler As ItemClickEventHandler, ByVal optionItem As Boolean)
        Me.optionItem = optionItem
        Me.Manager = manager
        Me.Caption = text
        AddHandler Me.ItemClick, handler 'Me.ItemClick += handler
      End Sub
      Public ReadOnly Property IsOption() As Boolean
        Get
          Return Me.optionItem
        End Get
      End Property
    End Class
    Private Class CheckBarItem
      Inherits OptionBarItem
      Private m_style As ActiveLookAndFeelStyle
      Public Sub New(ByVal manager As BarManager, ByVal text As String, ByVal handler As ItemClickEventHandler)
        Me.New(manager, text, handler, ActiveLookAndFeelStyle.Flat)
      End Sub
      Public Sub New(ByVal manager As BarManager, ByVal text As String, ByVal handler As ItemClickEventHandler, ByVal style As ActiveLookAndFeelStyle)
        MyBase.New(manager, text, handler, False)
        Me.m_style = style
      End Sub
      Public ReadOnly Property Style() As ActiveLookAndFeelStyle
        Get
          Return m_style
        End Get
      End Property

    End Class
    Private Class CheckBarItemWithStyle
      Inherits CheckBarItem
      Private lfStyle As LookAndFeelStyle
      Public Sub New(ByVal manager As BarManager, ByVal text As String, ByVal handler As ItemClickEventHandler, ByVal style As ActiveLookAndFeelStyle, ByVal lfStyle As LookAndFeelStyle)
        MyBase.New(manager, text, handler, style)
        Me.lfStyle = lfStyle
      End Sub
      Public ReadOnly Property LookAndFeelStyle() As LookAndFeelStyle
        Get
          Return lfStyle
        End Get
      End Property
    End Class
  End Class
End Namespace