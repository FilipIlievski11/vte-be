Public Class uxTouchTehnicalCheckReport
  Private WithEvents _tehExamVejiclePartsList As TehnicalExamVehiclePartsList
  Private WithEvents _tehnicalExamVehicleParts As TehnicalExamVehicleParts
  Private WithEvents _relationsList As CustomerVehiclesRelationsList

  Private WithEvents kopce As DevExpress.XtraEditors.CheckButton

#Region " KeyPress "

  Private Sub uxTehnicalExamVehicleParts_PritisnatoKopce(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.PritisnatoKopce
    Select Case Asc(e.KeyChar)
      Case 13
        SendKeys.Send("{TAB}")
    End Select
  End Sub

#End Region

#Region " WinPart Code "

  Protected Overrides Function GetIdValue() As Object

    Return My.Resources.uxTehnicalExamVehicleParts

  End Function

  Public Overrides Function ToString() As String

    Return My.Resources.uxTehnicalExamVehicleParts

  End Function
#End Region

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()
    LoadList()
    ' Add any initialization after the InitializeComponent() call.

  End Sub

  Private Sub LoadList()
    _relationsList = CustomerVehiclesRelationsList.GetCustomerVehiclesRelationsList
    Me.CustomerVehiclesRelationsListBindingSource.DataSource = _relationsList
  End Sub

  Private Sub uxTouchTehnicalCheckReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    _tehExamVejiclePartsList = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsList
    LoadPanels()
    For Each layItem As DevExpress.XtraLayout.LayoutControlItem In LayoutControlGroup1.Items
      If layItem.Name.Contains("LayoutItemchildPanel") Then
        
        If layItem.Name = "LayoutItemchildPanel0" Then
          layItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        Else
          layItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
      End If
    Next
  End Sub

  Private Sub LoadPanels()
    'za site podkategorii
    Dim pomPanel As DevExpress.XtraEditors.PanelControl
    For Each child As TehnicalExamVehiclePartsInfo In _tehExamVejiclePartsList
      pomPanel = New DevExpress.XtraEditors.PanelControl
      pomPanel.Name = "childPanel" & child.Id
      Dim pomlayoutItemC As DevExpress.XtraLayout.LayoutControlItem = LayoutControl1.AddItem()
      pomlayoutItemC.Name = "LayoutItem" & pomPanel.Name
      pomlayoutItemC.Control = pomPanel
      pomlayoutItemC.TextVisible = False
      'pomlayoutItemC.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
      Dim pomLista As TehnicalExamVehiclePartsList = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsListByCategory(child.Id)
      Dim Sirina As Integer = pomPanel.Width / 4
      Const Visina As Integer = 50 'Me.PanelCategory.Height / (_TehReportsGroups.Count / 3 + 2)
      Dim i As Integer = 1
      Dim left As Integer = 0
      Dim top As Integer = 10
      For Each childInPanel As TehnicalExamVehiclePartsInfo In pomLista
        kopce = New DevExpress.XtraEditors.CheckButton
        AddHandler kopce.Click, AddressOf Kategoruja_Klick
        kopce.Parent = pomPanel
        'kopce.ButtonStyle = DevExpr ess.XtraEditors.Controls.BorderStyles.Style3D
        kopce.Appearance.BackColor = Drawing.Color.Green
        kopce.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap

        kopce.Width = Sirina
        kopce.Height = Visina
        kopce.Name = pomPanel.Name & "kopce" & childInPanel.Id
        kopce.Text = childInPanel.Description
        Select Case (i Mod 3)
          Case 1
            left = 10
            kopce.Left = left
            kopce.Top = top
            left += (Sirina + Sirina / 2 - 10)
          Case 2
            kopce.Left = left
            kopce.Top = top
            left += (Sirina + Sirina / 2 - 10)
          Case 0
            kopce.Left = left
            kopce.Top = top
            top += (Visina + Visina / 2 - 10)
        End Select
        i += 1
      Next
      'pomPanel.Visible = False
    Next
    'za osnovnata kategorija
    Dim pomPanelOsn As DevExpress.XtraEditors.PanelControl = New DevExpress.XtraEditors.PanelControl
    pomPanelOsn.Name = "childPanel0"
    pomPanelOsn.Visible = True
    Dim pomlayoutItem As DevExpress.XtraLayout.LayoutControlItem = LayoutControl1.AddItem()
    pomlayoutItem.Name = "LayoutItem" & pomPanelOsn.Name
    pomlayoutItem.Control = pomPanelOsn
    pomlayoutItem.TextVisible = False
    pomlayoutItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
    Dim pomListaOsn As TehnicalExamVehiclePartsList = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsListByCategory(0)
    Dim SirinaOsn As Integer = pomPanelOsn.Width / 4
    Const VisinaOsn As Integer = 50 'Me.PanelCategory.Height / (_TehReportsGroups.Count / 3 + 2)
    Dim iOsn As Integer = 1
    Dim leftOsn As Integer = 0
    Dim topOsn As Integer = 10
    For Each childInPanel As TehnicalExamVehiclePartsInfo In pomListaOsn
      kopce = New DevExpress.XtraEditors.CheckButton
      AddHandler kopce.Click, AddressOf Kategoruja_Klick
      kopce.Parent = pomPanelOsn
      'kopce.ButtonStyle = DevExpr ess.XtraEditors.Controls.BorderStyles.Style3D
      kopce.Appearance.BackColor = Drawing.Color.Green
      kopce.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
      'kopce.
      kopce.Width = SirinaOsn
      kopce.Height = VisinaOsn
      kopce.Name = pomPanelOsn.Name & "kopce" & childInPanel.Id
      kopce.Text = childInPanel.Description
      Select Case (iOsn Mod 3)
        Case 1
          leftOsn = 10
          kopce.Left = leftOsn
          kopce.Top = topOsn
          leftOsn += (SirinaOsn + SirinaOsn / 2 - 10)
        Case 2
          kopce.Left = leftOsn
          kopce.Top = topOsn
          leftOsn += (SirinaOsn + SirinaOsn / 2 - 10)
        Case 0
          kopce.Left = leftOsn
          kopce.Top = topOsn
          topOsn += (VisinaOsn + VisinaOsn / 2 - 10)
      End Select
      iOsn += 1
    Next

  End Sub

  Private Sub Kategoruja_Klick(ByVal sender As Object, ByVal e As System.EventArgs)
    kopce = sender
   
    Dim narednaKategorija As TehnicalExamVehiclePartsList = _
    TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsListByCategory(CType(Mid(kopce.Name, 17), Integer))
    If narednaKategorija.Count > 0 Then
      For Each layItem As DevExpress.XtraLayout.LayoutControlItem In LayoutControlGroup1.Items
        If layItem.Name.Contains("LayoutItemchildPanel") Then
          'Dim pomint As Integer
          'Try
          '  pomint = Mid(kopce.Name, 11, 2)
          'Catch ex As Exception
          '  pomint = Mid(kopce.Name, 11, 1)
          'End Try

          If layItem.Name = "LayoutItemchildPanel" & Mid(kopce.Name, 17) Then
            layItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
          Else
            layItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
          End If
        End If
      Next
    End If
    'If Not kopce.Checked Then
    '  kopce.Appearance.BackColor = Drawing.Color.LightCoral
    'Else
    '  kopce.Appearance.BackColor = Drawing.Color.WhiteSmoke
    'End If
  End Sub

  Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
    Dim pom As Integer
    Dim nazadPanel As Integer
    For Each layItem As DevExpress.XtraLayout.LayoutControlItem In LayoutControlGroup1.Items
      If layItem.Name.Contains("LayoutItemchildPanel") AndAlso layItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always Then
        pom = Mid(layItem.Name, 21)
        Exit For
      End If
    Next
    Try
      nazadPanel = TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsListById(pom).IdCategoryVehicleParts
    Catch ex As Exception
      Me.Close()
    End Try
    For Each layItem As DevExpress.XtraLayout.LayoutControlItem In LayoutControlGroup1.Items
      If layItem.Name.Contains("LayoutItemchildPanel") Then
        If layItem.Name = "LayoutItemchildPanel" & nazadPanel Then
          layItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        Else
          layItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
      End If
    Next
  End Sub
End Class
