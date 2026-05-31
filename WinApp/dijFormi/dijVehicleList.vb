Public Class dijVehicleList 

    Private WithEvents _vehicleList As VehiclesListShortListAll '= objVehicleList

  Private _vehicle As VehicleInfo
  Public ReadOnly Property Vehicle() As VehicleInfo
    Get
      Return _vehicle
    End Get
  End Property
  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.

        _vehicleList = VehiclesListShortListAll.GetVehiclesListShortListAll  'VehicleList.GetVehicleList

  End Sub

    Public Sub New(ByVal vehicleList As VehiclesListShortListAll)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _vehicleList = vehicleList

    End Sub

  Private Sub dijVehicleList_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

  End Sub

  Private Sub dijVehicleList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
    End Select
  End Sub

  'Public Sub New(ByVal strFilter As String)

  '  ' This call is required by the Windows Form Designer.
  '  InitializeComponent()

  '  ' Add any initialization after the InitializeComponent() call.
  '  _vehicleList = VehicleList.GetVehicleList
  '  Me.VehicleListBindingSource.DataSource = _vehicleList
  '  GridView1.Columns("Id").FilterInfo = New DevExpress.XtraGrid.Columns.ColumnFilterInfo(strFilter)
  'End Sub
  Private Sub dijVehicleList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Me.VehicleListBindingSource.DataSource = _vehicleList
    'System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
    'System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
    VehicleListGridControl.ForceInitialize()
    VehicleListGridControl.Focus()
    'Me.GridView1.FocusedColumn = colRegistrationNumber
    Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle

  End Sub

  Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
    ShowVehicle()
  End Sub

  Private Sub ShowVehicle()
    If Me.VehicleListBindingSource.Position >= 0 Then
            _vehicle = VehicleList.GetVehicleById _
            (_vehicleList.Item(Me.VehicleListBindingSource.Position).Id) '_vehicleList.Item(Me.VehicleListBindingSource.Position)
      Me.DialogResult = Windows.Forms.DialogResult.OK
      Me.Close()
    End If
  End Sub
  Private Sub AddVehicle()
    Dim par As MainForm = Me.ParentForm
    Dim vehicle As Vehicle
    Using busy As New StatusBusy(My.Resources.LoadingData)
      Try

        vehicle = VTE.Library.Vehicle.NewVehicle
        'For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
        '  For Each ctl As Control In page.Controls
        '    If (TypeOf ctl Is uxVehicle) Then
        'par.ShowWinPart(CType(ctl, uxVehicle))
        '      Exit Sub
        '    End If
        '  Next
        'Next
        par.ShowWinPart(New uxVehicle(vehicle))
      Catch ex As Csla.DataPortalException
        MessageBox.Show(ex.BusinessException.ToString, _
          "Error loading", MessageBoxButtons.OK, _
          MessageBoxIcon.Exclamation)
      Catch ex As Exception
        MessageBox.Show(ex.ToString, _
          "Error loading", MessageBoxButtons.OK, _
          MessageBoxIcon.Exclamation)
      End Try
    End Using

   
    'Me.Close()

  End Sub

  Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
    AddVehicle()
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub

  Private Sub HyperLinkShellNumber_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles HyperLinkShellNumber.DoubleClick
    ShowVehicle()
  End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If GridView1.FocusedColumn IsNot Nothing Then
      Select Case GridView1.FocusedColumn.Name
        Case "colRegistrationNumber", "colShellNumber", "colModelName", "colVehiceMaker"
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
        Case Else
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
      End Select
    End If
  End Sub

  Private Sub rihleCurrentRegistationNumber_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles rihleCurrentRegistationNumber.DoubleClick
    ShowVehicle()
  End Sub

  Private Sub dijVehicleList_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
    System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
    System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
  End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        _vehicleList = VehiclesListShortListAll.GetVehiclesListShortListAll
        Me.VehicleListBindingSource.DataSource = _vehicleList
    End Sub
End Class