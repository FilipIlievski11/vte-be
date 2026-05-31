Public Class uxDocTehnicalExamReportTouch
  '  Private WithEvents _TehReportsGroups As TehnicalExamVehiclePartsCategoryList
  '  Private WithEvents _tehReportsParts As TehnicalExamVehiclePartsList
  '  Private WithEvents kopce As DevExpress.XtraEditors.CheckButton
  '  Private WithEvents _docTehExamReport As DocumentsTehnicalExamsReport
  '  Private WithEvents _vehicleList As VehicleList
  '  Private WithEvents _customerList As CustomersList
  '  Private WithEvents _relation As CustomerVehiclesRelationsInfo
  '#Region "WinPart"

  '  Protected Overrides Function GetIdValue() As Object
  '    Return My.Resources.uxRequests
  '  End Function

  '  Public Overrides Function ToString() As String
  '    Return My.Resources.uxRequests
  '  End Function

  '#End Region
  '  Public ReadOnly Property Doc() As DocumentsTehnicalExamsReport
  '    Get
  '      Return _docTehExamReport
  '    End Get
  '  End Property

  '  Public Sub New(ByVal inDoc As DocumentsTehnicalExamsReport, ByVal inIdRelation As Long)

  '    ' This call is required by the Windows Form Designer.
  '    InitializeComponent()
  '    _docTehExamReport = inDoc
  '    If inDoc.IsNew Then
  '      _docTehExamReport.IdCustomerVehicleRelation = inIdRelation
  '    End If
  '    LoadList()
  '    ' Add any initialization after the InitializeComponent() call.
  '    _relation = CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsList. _
  '     GetInfoRelationById(_docTehExamReport.IdCustomerVehicleRelation)
  '    LookUpEditCustomer.EditValue = _relation.IdCustomer
  '    LookUpEditVehicle.EditValue = _relation.IdVehicle
  '  End Sub

  '  Private Sub LoadList()
  '    _TehReportsGroups = TehnicalExamVehiclePartsCategoryList.GetTehnicalExamVehiclePartsCategoryList
  '    _tehReportsParts = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsList
  '    _vehicleList = objVehicleList
  '    Me.VehicleListBindingSource.DataSource = _vehicleList
  '    _customerList = CustomersList.GetCustomersList
  '    Me.CustomersListBindingSource.DataSource = _customerList
  '  End Sub

  '  Private Sub uxDocTehnicalExamReportTouch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  '    LayoutControlItemParts.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
  '    LayoutControlItemCategory.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
  '    LoadPanelCategory()
  '  End Sub


  '  Private Sub LoadPanelCategory()

  '    Dim Sirina As Integer = Me.PanelCategory.Width / 4
  '    Const Visina As Integer = 100 'Me.PanelCategory.Height / (_TehReportsGroups.Count / 3 + 2)
  '    Dim i As Integer = 1
  '    Dim left As Integer = 0
  '    Dim top As Integer = 10
  '    For Each kategorija As TehnicalExamVehiclePartsCategoryInfo In _TehReportsGroups
  '      kopce = New DevExpress.XtraEditors.CheckButton
  '      AddHandler kopce.Click, AddressOf Kategoruja_Klick
  '      kopce.Parent = PanelCategory
  '      'kopce.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D
  '      kopce.Appearance.BackColor = Drawing.Color.Green
  '      kopce.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
  '      'kopce.
  '      kopce.Width = Sirina
  '      kopce.Height = Visina
  '      kopce.Name = "kopce" & kategorija.Id
  '      kopce.Text = kategorija.CategoryName
  '      Select Case (i Mod 3)
  '        Case 1
  '          left = 10
  '          kopce.Left = left
  '          kopce.Top = top
  '          left += (Sirina + Sirina / 2 - 10)
  '        Case 2
  '          kopce.Left = left
  '          kopce.Top = top
  '          left += (Sirina + Sirina / 2 - 10)
  '        Case 0
  '          kopce.Left = left
  '          kopce.Top = top
  '          top += (Visina + Visina / 2 - 10)
  '      End Select
  '      i += 1
  '    Next
  '  End Sub

  '  Private Sub Kategoruja_Klick(ByVal sender As Object, ByVal e As System.EventArgs)
  '    Dim SelectedCategoriId As Integer = Strings.Mid(sender.name, 6)
  '    kopce = sender
  '    kopce.Checked = False
  '    PanelCategory.Focus()
  '    OtvoriPanelParts(SelectedCategoriId)
  '  End Sub

  '  Private Sub Nepravilnost_Klick(ByVal sender As Object, ByVal e As System.EventArgs)
  '    kopce = sender
  '    If Not kopce.Checked Then
  '      kopce.Appearance.BackColor = Drawing.Color.LightCoral

  '    Else

  '      kopce.Appearance.BackColor = Drawing.Color.WhiteSmoke
  '    End If
  '  End Sub

  '  Private Sub OtvoriPanelParts(ByVal inId As Integer)
  '    LayoutControlItemCategory.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
  '    LayoutControlItemParts.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
  '    _tehReportsParts = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsListByCategory(inId)
  '    Dim i As Integer = 1
  '    Dim left As Integer = 0
  '    Dim top As Integer = 10
  '    Dim sirinaKopce As Integer = Me.PanelParts.Width / 5
  '    Dim visinaKopce As Integer = Me.PanelParts.Height / (_tehReportsParts.Count / 4 + 2)
  '    For Each part As TehnicalExamVehiclePartsInfo In _tehReportsParts
  '      kopce = New DevExpress.XtraEditors.CheckButton
  '      AddHandler kopce.Click, AddressOf Nepravilnost_Klick
  '      kopce.Parent = PanelParts
  '      kopce.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D
  '      kopce.Appearance.BackColor = Drawing.Color.WhiteSmoke
  '      kopce.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap

  '      kopce.Width = sirinaKopce
  '      kopce.Height = visinaKopce
  '      kopce.Name = "kopceNepravilnost" & part.Id
  '      kopce.Text = part.CodeAndDescription
  '      'kopce.Font.Size = 30 'System.Drawing.Font.Size(30)

  '      Select Case (i Mod 4)
  '        Case 1
  '          left = 10
  '          kopce.Left = left
  '          kopce.Top = top
  '          left += (sirinaKopce + sirinaKopce / 2 - 10)
  '        Case 2
  '          kopce.Left = left
  '          kopce.Top = top
  '          left += (sirinaKopce + sirinaKopce / 2 - 10)
  '        Case 3
  '          kopce.Left = left
  '          kopce.Top = top
  '          left += (sirinaKopce + sirinaKopce / 2 - 10)
  '        Case 0
  '          kopce.Left = left
  '          kopce.Top = top
  '          top += (visinaKopce + visinaKopce / 2 - 10)
  '      End Select
  '      i += 1
  '    Next
  '  End Sub

  '  Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
  '    If LayoutControlItemCategory.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always Then
  '      _docTehExamReport.Save()
  '      Me.Close()
  '    Else
  '      For Each btn As DevExpress.XtraEditors.CheckButton In Me.PanelParts.Controls
  '        If btn.Checked Then
  '          Dim detal As DocumentsTehnicalExamsReportsDetail = _docTehExamReport.Details.AddNew()
  '          Dim partId As Integer = Strings.Mid(btn.Name, 18)
  '          detal.DateEnter = Now
  '          detal.IdStatus = 3
  '          detal.IdTehnicalExamsReports = _docTehExamReport.Id
  '          detal.IdTehnicalExamVehivlePart = TehnicalExamVehiclePart.GetTehnicalExamVehiclePart(partId).Id
  '          _docTehExamReport.Save()
  '        End If
  '      Next
  '      Me.PanelParts.Controls.Clear()
  '      LayoutControlItemParts.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
  '      LayoutControlItemCategory.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
  '    End If
  '  End Sub
End Class
