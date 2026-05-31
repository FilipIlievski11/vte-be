<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uxCountries
    Inherits VTE.BaseParts.uxWinPart

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
  Me.components = New System.ComponentModel.Container
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uxCountries))
  Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl
  Me.UxKopcinja1 = New VTE.BaseParts.uxKopcinja
  Me.CountriesGridControl = New DevExpress.XtraGrid.GridControl
  Me.CountriesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
  Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
  Me.colId = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCountryName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCountryShortName = New DevExpress.XtraGrid.Columns.GridColumn
  Me.colCitizenship = New DevExpress.XtraGrid.Columns.GridColumn
  Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
  Me.BindingSourceRefresh1 = New Csla.Windows.BindingSourceRefresh(Me.components)
  Me.DxErrorProvider1 = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
  CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SplitContainerControl1.SuspendLayout()
  CType(Me.CountriesGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.CountriesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).BeginInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
  Me.SuspendLayout()
  '
  'SplitContainerControl1
  '
  Me.SplitContainerControl1.AccessibleDescription = Nothing
  Me.SplitContainerControl1.AccessibleName = Nothing
  resources.ApplyResources(Me.SplitContainerControl1, "SplitContainerControl1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainerControl1, False)
  Me.SplitContainerControl1.Horizontal = False
  Me.SplitContainerControl1.Name = "SplitContainerControl1"
  Me.SplitContainerControl1.Panel1.Controls.Add(Me.UxKopcinja1)
  resources.ApplyResources(Me.SplitContainerControl1.Panel1, "SplitContainerControl1.Panel1")
  Me.SplitContainerControl1.Panel2.Controls.Add(Me.CountriesGridControl)
  resources.ApplyResources(Me.SplitContainerControl1.Panel2, "SplitContainerControl1.Panel2")
  Me.SplitContainerControl1.SplitterPosition = 62
  '
  'UxKopcinja1
  '
  Me.UxKopcinja1.AccessibleDescription = Nothing
  Me.UxKopcinja1.AccessibleName = Nothing
  resources.ApplyResources(Me.UxKopcinja1, "UxKopcinja1")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UxKopcinja1, False)
  Me.UxKopcinja1.BackgroundImage = Nothing
  Me.UxKopcinja1.Name = "UxKopcinja1"
  '
  'CountriesGridControl
  '
  Me.CountriesGridControl.AccessibleDescription = Nothing
  Me.CountriesGridControl.AccessibleName = Nothing
  resources.ApplyResources(Me.CountriesGridControl, "CountriesGridControl")
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.CountriesGridControl, False)
  Me.CountriesGridControl.BackgroundImage = Nothing
  Me.CountriesGridControl.DataSource = Me.CountriesBindingSource
  Me.CountriesGridControl.EmbeddedNavigator.AccessibleDescription = Nothing
  Me.CountriesGridControl.EmbeddedNavigator.AccessibleName = Nothing
  Me.CountriesGridControl.EmbeddedNavigator.Anchor = CType(resources.GetObject("CountriesGridControl.EmbeddedNavigator.Anchor"), System.Windows.Forms.AnchorStyles)
  Me.CountriesGridControl.EmbeddedNavigator.BackgroundImage = Nothing
  Me.CountriesGridControl.EmbeddedNavigator.BackgroundImageLayout = CType(resources.GetObject("CountriesGridControl.EmbeddedNavigator.BackgroundImageLayout"), System.Windows.Forms.ImageLayout)
  Me.CountriesGridControl.EmbeddedNavigator.ImeMode = CType(resources.GetObject("CountriesGridControl.EmbeddedNavigator.ImeMode"), System.Windows.Forms.ImeMode)
  Me.CountriesGridControl.EmbeddedNavigator.TextLocation = CType(resources.GetObject("CountriesGridControl.EmbeddedNavigator.TextLocation"), DevExpress.XtraEditors.NavigatorButtonsTextLocation)
  Me.CountriesGridControl.EmbeddedNavigator.ToolTip = resources.GetString("CountriesGridControl.EmbeddedNavigator.ToolTip")
  Me.CountriesGridControl.EmbeddedNavigator.ToolTipIconType = CType(resources.GetObject("CountriesGridControl.EmbeddedNavigator.ToolTipIconType"), DevExpress.Utils.ToolTipIconType)
  Me.CountriesGridControl.EmbeddedNavigator.ToolTipTitle = resources.GetString("CountriesGridControl.EmbeddedNavigator.ToolTipTitle")
  Me.CountriesGridControl.Font = Nothing
  Me.CountriesGridControl.MainView = Me.GridView1
  Me.CountriesGridControl.Name = "CountriesGridControl"
  Me.CountriesGridControl.UseEmbeddedNavigator = True
  Me.CountriesGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
  '
  'CountriesBindingSource
  '
  Me.CountriesBindingSource.AllowNew = True
  Me.CountriesBindingSource.DataSource = GetType(VTE.Library.Country)
  Me.BindingSourceRefresh1.SetReadValuesOnChange(Me.CountriesBindingSource, False)
  '
  'GridView1
  '
  Me.GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
  Me.GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  Me.GridView1.Appearance.Row.Options.UseTextOptions = True
  Me.GridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
  Me.GridView1.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
  resources.ApplyResources(Me.GridView1, "GridView1")
  Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colId, Me.colCountryName, Me.colCountryShortName, Me.colCitizenship})
  Me.GridView1.GridControl = Me.CountriesGridControl
  Me.GridView1.Name = "GridView1"
  Me.GridView1.OptionsNavigation.AutoFocusNewRow = True
  Me.GridView1.OptionsNavigation.EnterMoveNextColumn = True
  Me.GridView1.OptionsView.ShowAutoFilterRow = True
  '
  'colId
  '
  resources.ApplyResources(Me.colId, "colId")
  Me.colId.FieldName = "Id"
  Me.colId.Name = "colId"
  Me.colId.OptionsColumn.ReadOnly = True
  '
  'colCountryName
  '
  resources.ApplyResources(Me.colCountryName, "colCountryName")
  Me.colCountryName.FieldName = "CountryName"
  Me.colCountryName.Name = "colCountryName"
  '
  'colCountryShortName
  '
  resources.ApplyResources(Me.colCountryShortName, "colCountryShortName")
  Me.colCountryShortName.FieldName = "CountryShortName"
  Me.colCountryShortName.Name = "colCountryShortName"
  '
  'colCitizenship
  '
  resources.ApplyResources(Me.colCitizenship, "colCitizenship")
  Me.colCitizenship.FieldName = "Citizenship"
  Me.colCitizenship.Name = "colCitizenship"
  '
  'DxErrorProvider1
  '
  Me.DxErrorProvider1.ContainerControl = Me
  Me.DxErrorProvider1.DataSource = Me.CountriesBindingSource
  '
  'uxCountries
  '
  Me.AccessibleDescription = Nothing
  Me.AccessibleName = Nothing
  Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
  resources.ApplyResources(Me, "$this")
  Me.BackgroundImage = Nothing
  Me.Controls.Add(Me.SplitContainerControl1)
  Me.Name = "uxCountries"
  Me.Controls.SetChildIndex(Me.SplitContainerControl1, 0)
  CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.SplitContainerControl1.ResumeLayout(False)
  CType(Me.CountriesGridControl, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.CountriesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.BindingSourceRefresh1, System.ComponentModel.ISupportInitialize).EndInit()
  CType(Me.DxErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
  Me.ResumeLayout(False)

 End Sub
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents UxKopcinja1 As VTE.BaseParts.uxKopcinja
  Friend WithEvents CountriesBindingSource As System.Windows.Forms.BindingSource
  Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
  Friend WithEvents BindingSourceRefresh1 As Csla.Windows.BindingSourceRefresh
  Friend WithEvents DxErrorProvider1 As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
  Friend WithEvents CountriesGridControl As DevExpress.XtraGrid.GridControl
  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCountryName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCountryShortName As DevExpress.XtraGrid.Columns.GridColumn
  Friend WithEvents colCitizenship As DevExpress.XtraGrid.Columns.GridColumn

End Class
