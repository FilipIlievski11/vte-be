Imports System
Imports VTE.Library
Public Class uxWinPart
  Inherits DevExpress.XtraEditors.XtraUserControl

  Private _tPage As DevExpress.XtraTab.XtraTabPage

  Public Property tPage() As DevExpress.XtraTab.XtraTabPage
    Get
      Return _tPage
    End Get
    Set(ByVal Value As DevExpress.XtraTab.XtraTabPage)
      _tPage = Value
    End Set
  End Property

  Private _isShowingBrokenRules As Boolean = False
  Public Property IsShowingBrokenRules() As Boolean
    Get
      Return _isShowingBrokenRules
    End Get
    Set(ByVal value As Boolean)
            _isShowingBrokenRules = value
            Try
                If value Then
                    Me.dpBrokenRules.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide
                Else
                    Me.dpBrokenRules.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden
                End If
            Catch ex As Exception

            End Try
            

        End Set
  End Property



  Protected Overridable Function GetIdValue() As Object

    Return Nothing

  End Function

  Public Overrides Function Equals(ByVal obj As Object) As Boolean

    If Me.DesignMode Then
      Return MyBase.Equals(obj)
    Else
      Dim id As Object = GetIdValue()
      If Me.GetType.Equals(obj.GetType) AndAlso id IsNot Nothing Then
        Return CType(obj, uxWinPart).GetIdValue.Equals(id)

      Else
        Return False
      End If
    End If

  End Function

  Public Overrides Function GetHashCode() As Integer

    Dim id As Object = GetIdValue()
    If id IsNot Nothing Then
      Return id.GetHashCode

    Else
      Return MyBase.GetHashCode
    End If

  End Function

  Public Overrides Function ToString() As String

    Dim id As Object = GetIdValue()
    If id IsNot Nothing Then
      Return id.ToString

    Else
      Return MyBase.ToString
    End If

  End Function

#Region " CloseWinPart "

  Public Event CloseWinPart As EventHandler

  Protected Sub Close()

    RaiseEvent CloseWinPart(Me, EventArgs.Empty)

  End Sub

#End Region

#Region " CurrentPrincipalChanged "

  Public Event CurrentPrincipalChanged As EventHandler

  Public Overridable Sub OnCurrentPrincipalChanged( _
    ByVal sender As Object, ByVal e As EventArgs)

    RaiseEvent CurrentPrincipalChanged(sender, e)

  End Sub

#End Region

#Region " KeyPress "
  Protected Event PritisnatoKopce As EventHandler(Of System.Windows.Forms.KeyPressEventArgs)

  Public Overridable Sub OnPritisnatoKopce( _
    ByVal sender As Object, _
    ByVal e As System.Windows.Forms.KeyPressEventArgs)

    RaiseEvent PritisnatoKopce(sender, e)

  End Sub

#End Region

#Region " Refresh Lists "
  Protected Event RefreshLists As EventHandler

  Public Overridable Sub OnRefreshLists(ByVal sender As Object, ByVal e As EventArgs)
    RaiseEvent RefreshLists(sender, e)
  End Sub
#End Region

#Region " Data binding helpers "

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

#End Region

#Region " Validation Helpers "
  Protected Sub ShowBrokenRules(ByVal message As String, Optional ByVal OnlyLabel As Boolean = False)

    If OnlyLabel Then
      Me.lblMessage.Text = message
    Else
      If (message.Trim <> String.Empty) Then
        Me.lblMessage.Text = message

        'Me.dpBrokenRules.Show()
      End If

    End If

  End Sub
#End Region


  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call

  End Sub



  Private Sub uxWinPart_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave
    Dim objOpcii As Options = Csla.ApplicationContext.LocalContext.Item("objOpcii")
    If objOpcii.SaveLayout Then
      SaveGridsLayout(Me)
    End If
  End Sub

  Private Sub uxWinPart_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
            Dim objOpcii As Options = Csla.ApplicationContext.LocalContext.Item("objOpcii")
      If objOpcii.SaveLayout Then
        RestoreGridsLayout(Me)
      End If
      RestoreFormsFont(Me)
    Catch ex As Exception
      SaveGridsLayout(Me)
    End Try
  End Sub

#Region "Save/Restore layout "
  Private Shared Sub SaveGridsLayout(ByVal parent As Control)
    On Error Resume Next

    'svriti gi site kontroli
    For Each ctrControl As Control In parent.Controls
      'proveri dali e GridControl
      If ctrControl.GetType() Is GetType(DevExpress.XtraGrid.GridControl) Then
        For Each vw As DevExpress.XtraGrid.Views.Grid.GridView _
                       In CType(ctrControl, DevExpress.XtraGrid.GridControl).Views
          vw.SaveLayoutToXml(My.Application.Info.DirectoryPath & "\" & GetParentName(ctrControl) & ctrControl.Name & ".xml")
        Next
      End If
      'proveri dali e MyGridControl
      If ctrControl.GetType() Is GetType(MyXtraGrid.MyGridControl) Then
        For Each vw As MyXtraGrid.MyGridView _
               In CType(ctrControl, MyXtraGrid.MyGridControl).Views
          vw.SaveLayoutToXml(My.Application.Info.DirectoryPath & "\" & GetParentName(ctrControl) & ctrControl.Name & ".xml")
        Next
      End If
      'proveri dali e Layout
      If ctrControl.GetType() Is GetType(DevExpress.XtraLayout.LayoutControl) Then
        'CType(ctrControl, DevExpress.XtraLayout.LayoutControl).SaveLayoutToRegistry("HKEY_LOCAL_MACHINE\SOFTWARE\BSS\KNIGOVODSTVO\XML")
        Select Case CType(ctrControl, DevExpress.XtraLayout.LayoutControl).Name
          Case "RequestChooserLayoutControl", "RequestEditLayoutControl"
            'ne pravi nisto
          Case Else
            CType(ctrControl, DevExpress.XtraLayout.LayoutControl).SaveLayoutToXml( _
            My.Application.Info.DirectoryPath & "\" & GetParentName(ctrControl) & ctrControl.Name & ".xml")
        End Select

      End If
      'proveri dali e DataLayout
      If ctrControl.GetType() Is GetType(DevExpress.XtraDataLayout.DataLayoutControl) Then
        'CType(ctrControl, DevExpress.XtraLayout.LayoutControl).SaveLayoutToRegistry("HKEY_LOCAL_MACHINE\SOFTWARE\BSS\KNIGOVODSTVO\XML")
        CType(ctrControl, DevExpress.XtraDataLayout.DataLayoutControl).SaveLayoutToXml( _
        My.Application.Info.DirectoryPath & "\" & GetParentName(ctrControl) & ctrControl.Name & ".xml")
      End If
      'PivodGrid
      If ctrControl.GetType() Is GetType(DevExpress.XtraPivotGrid.PivotGridControl) Then
        CType(ctrControl, DevExpress.XtraPivotGrid.PivotGridControl).SaveLayoutToXml( _
        My.Application.Info.DirectoryPath & "\" & GetParentName(ctrControl) & ctrControl.Name & ".xml")
      End If


      'otidi vo rekurzija ako ima deca
      If ctrControl.HasChildren Then
        SaveGridsLayout(ctrControl)
      End If


    Next
  End Sub

  Private Shared Sub RestoreGridsLayout(ByVal parent As Control)
    'svriti gi site kontroli
    For Each ctrControl As Control In parent.Controls
      'proveri dali e GridControl
      If ctrControl.GetType() Is GetType(DevExpress.XtraGrid.GridControl) Then
        For Each vw As DevExpress.XtraGrid.Views.Grid.GridView _
               In CType(ctrControl, DevExpress.XtraGrid.GridControl).Views
          vw.RestoreLayoutFromXml(My.Application.Info.DirectoryPath & "\" & GetParentName(ctrControl) & ctrControl.Name & ".xml")
        Next
      End If
      'proveri dali e MyGridControl
      If ctrControl.GetType() Is GetType(MyXtraGrid.MyGridControl) Then
        For Each vw As MyXtraGrid.MyGridView _
               In CType(ctrControl, MyXtraGrid.MyGridControl).Views
          vw.RestoreLayoutFromXml(My.Application.Info.DirectoryPath & "\" & GetParentName(ctrControl) & ctrControl.Name & ".xml")
        Next
      End If
      'proveri dali e Layout
      If ctrControl.GetType() Is GetType(DevExpress.XtraLayout.LayoutControl) Then
        'CType(ctrControl, DevExpress.XtraLayout.LayoutControl).RestoreLayoutFromRegistry("HKEY_LOCAL_MACHINE\SOFTWARE\BSS\KNIGOVODSTVO\XML")
        CType(ctrControl, DevExpress.XtraLayout.LayoutControl).RestoreLayoutFromXml( _
        My.Application.Info.DirectoryPath & "\" & GetParentName(ctrControl) & ctrControl.Name & ".xml")
      End If
      'proveri dali e DataLayout
      If ctrControl.GetType() Is GetType(DevExpress.XtraDataLayout.DataLayoutControl) Then
        'CType(ctrControl, DevExpress.XtraLayout.LayoutControl).RestoreLayoutFromRegistry("HKEY_LOCAL_MACHINE\SOFTWARE\BSS\KNIGOVODSTVO\XML")
        CType(ctrControl, DevExpress.XtraDataLayout.DataLayoutControl).RestoreLayoutFromXml( _
        My.Application.Info.DirectoryPath & "\" & GetParentName(ctrControl) & ctrControl.Name & ".xml")
      End If
      'proveri dali e PivodGrid
      If ctrControl.GetType() Is GetType(DevExpress.XtraPivotGrid.PivotGridControl) Then
        CType(ctrControl, DevExpress.XtraPivotGrid.PivotGridControl).RestoreLayoutFromXml( _
        My.Application.Info.DirectoryPath & "\" & GetParentName(ctrControl) & ctrControl.Name & ".xml")
      End If
      'Font
      If ctrControl.GetType() Is GetType(DevExpress.XtraLayout.LayoutControl) Then
        Dim Size As Integer = CType(Csla.ApplicationContext.LocalContext("uSettings"), UserSettings).FormsFornt

        CType(ctrControl, DevExpress.XtraLayout.LayoutControl).Appearance.Control.Font = New Font("Tahoma", Size)
        CType(ctrControl, DevExpress.XtraLayout.LayoutControl).Root.AppearanceItemCaption.Font = New Font("Tahoma", Size)
      End If

      'otidi vo rekurzija ako ima deca
      If ctrControl.HasChildren Then
        RestoreGridsLayout(ctrControl)
      End If
    Next
  End Sub
  Private Shared Function GetParentName(ByVal ctr As Control) As String

    If ctr.Parent IsNot Nothing Then
      If TypeOf ctr Is uxWinPart Then
        Return CType(ctr, uxWinPart).Name
      Else
        Return GetParentName(ctr.Parent)
      End If

    End If
    Return ""
  End Function

  Private Shared Sub RestoreFormsFont(ByVal parent As Control)
    'svriti gi site kontroli
    For Each ctrControl As Control In parent.Controls

      'Font
      If ctrControl.GetType() Is GetType(DevExpress.XtraLayout.LayoutControl) Then
        Dim Size As Integer = CType(Csla.ApplicationContext.LocalContext("uSettings"), UserSettings).FormsFornt
        'CType(ctrControl, DevExpress.XtraLayout.LayoutControl).Font = New Font("Tahoma", Size)

        CType(ctrControl, DevExpress.XtraLayout.LayoutControl).Font = New Font("Tahoma", Size)
        CType(ctrControl, DevExpress.XtraLayout.LayoutControl).Root.AppearanceItemCaption.Font = New Font("Tahoma", Size)
        For Each it As Control In ctrControl.Controls
          it.Font = New Font("Tahoma", Size)
        Next
      End If

      'otidi vo rekurzija ako ima deca
      If ctrControl.HasChildren Then
        RestoreFormsFont(ctrControl)
      End If
    Next
  End Sub

#End Region



End Class
