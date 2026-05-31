
Public Class uxVehicleList

    Private WithEvents _vehicleList As VehiclesListShortListAll
  'Private WithEvents _vehicleListWithOwner As VehicleListWitkOwnerList
  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
    ' Me.rideLastRegDate.NullDate = New Csla.SmartDate(True).Date
    'Me.rideLastRegDate.NullText = String.Empty
  End Sub

#Region "PritisnatoKopce"

  Private Sub uxVehicleList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle

    End Select
  End Sub
  Private Sub uxVehicleCategorie_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")

    End Select
  End Sub
#End Region

#Region "WinPart"

  Protected Overrides Function GetIdValue() As Object
    Return My.Resources.uxVehiclesList
  End Function

  Public Overrides Function ToString() As String
    Return My.Resources.uxVehiclesList
  End Function

#End Region

  Private Sub uxVehicleList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      '_vehicleList = VehicleList.GetVehicleList
      'If _vehicleList IsNot Nothing Then
      '  Me.VehicleListBindingSource.DataSource = _vehicleList
      'End If
            _vehicleList = VehiclesListShortListAll.GetVehiclesListShortListAll
      If _vehicleList IsNot Nothing Then
        Me.VehicleListBindingSource.DataSource = _vehicleList
      End If

    Catch ex As Csla.DataPortalException
      MessageBox.Show(ex.BusinessException.ToString, _
        "Error loading", MessageBoxButtons.OK, _
        MessageBoxIcon.Exclamation)

    Catch ex As Exception
      MessageBox.Show(ex.ToString, _
        "Error loading", MessageBoxButtons.OK, _
        MessageBoxIcon.Exclamation)
    End Try
    VehicleListGridControl.ForceInitialize()
    VehicleListGridControl.Focus()
    Me.GridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
  End Sub



  Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
    ShowVehicle()
  End Sub

  Private Sub ShowVehicle()
    Dim par As MainForm = Me.ParentForm
    Dim vehic As Vehicle
    Using busy As New StatusBusy(My.Resources.LoadingData)
      Try
        If _vehicleList.Count > 0 Then
          Dim pomVehicleId As Long = Me._vehicleList.Item(Me.VehicleListBindingSource.Position).Id
          If pomVehicleId > 0 Then
            vehic = Vehicle.GetVehicle(pomVehicleId)
          Else
            MsgBox("Нема возило за тој сопственик")
            Exit Sub
          End If

        Else
          Exit Sub
        End If
        For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
          For Each ctl As Control In page.Controls
            If (TypeOf ctl Is uxVehicle) AndAlso (CType(ctl, uxVehicle).Vehicle.Id = vehic.Id) Then
              par.ShowWinPart(CType(ctl, uxVehicle))
              Exit Sub
            End If
          Next
        Next

        'Me.Close()
        par.AddWinPart(New uxVehicle(vehic))
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

  End Sub
  Private Sub AddVehicle()
    Dim par As MainForm = Me.ParentForm
    Dim vehic As Vehicle
    Using busy As New StatusBusy(My.Resources.LoadingData)
      Try

        vehic = Vehicle.NewVehicle
        par.AddWinPart(New uxVehicle(vehic))
        For Each page As DevExpress.XtraTab.XtraTabPage In par.tabMain.TabPages
          For Each ctl As Control In page.Controls
            If (TypeOf ctl Is uxVehicle) AndAlso (CType(ctl, uxVehicle).Vehicle.Id = vehic.Id) Then
              par.ShowWinPart(CType(ctl, uxVehicle))
              Exit Sub
            End If
          Next
        Next
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

  Private Sub HyperLinkEditShellNumber_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
  HyperLinkEditCustomer.DoubleClick, HyperLinkEditRegBr.DoubleClick, HyperLinkEditShelNum.DoubleClick
    ShowVehicle()

  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Me.Close()
  End Sub

  Private Sub GridView1_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridView1.FocusedColumnChanged
    If GridView1.FocusedColumn IsNot Nothing Then
      Select Case GridView1.FocusedColumn.Name
        Case "colLastRegistratinNumber", "colModelName", "colCompanyName", "colShellNumber"
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("en-US"))
        Case Else
          System.Windows.Forms.InputLanguage.CurrentInputLanguage = _
          System.Windows.Forms.InputLanguage.FromCulture(New System.Globalization.CultureInfo("mk-MK"))
      End Select
    End If
  End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        _vehicleList = VehiclesListShortListAll.GetVehiclesListShortListAll
        Me.VehicleListBindingSource.DataSource = _vehicleList
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Me.PrintableComponentLink1.ShowPreviewDialog(Me)
    End Sub
End Class
