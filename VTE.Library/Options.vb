
<Serializable()> _
Public Class Options
    Inherits Csla.BusinessBase(Of Options)

#Region " Business Properties and Methods "
 
    'margini
    'zelen
    Private Shared ZelenTopMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("ZelenTopMargin", "ZelenTopMargin", 0))
    Private Shared ZelenLeftMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("ZelenLeftMargin", "ZelenLeftMargin", 0))
    Private Shared ZelenRightMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("ZelenRightMargin", "ZelenRightMargin", 0))
    Private Shared ZelenButtonMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("ZelenButtonMargin", "ZelenButtonMargin", 0))

    'Plav
    Private Shared PlavTopMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("PlavTopMargin", "PlavTopMargin", 0))
    Private Shared PlavLeftMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("PlavLeftMargin", "PlavLeftMargin", 0))
    Private Shared PlavRightMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("PlavRightMargin", "PlavRightMargin", 0))
    Private Shared PlavButtonMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("PlavButtonMargin", "PlavButtonMargin", 0))

    'bel
    Private Shared BelTopMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("BelTopMargin", "BelTopMargin", 0))
    Private Shared BelLeftMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("BelLeftMargin", "BelLeftMargin", 0))
    Private Shared BelRightmarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("BelRightmargin", "BelRightmargin", 0))
    Private Shared BelButtonMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("BelButtonMargin", "BelButtonMargin", 0))

    'Odobrenie
    Private Shared OdobrenieTopMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("OdobrenieTopMargin", "OdobrenieTopMargin", 0))
    Private Shared OdobrenieLeftMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("OdobrenieLeftMargin", "OdobrenieLeftMargin", 0))
    Private Shared OdobrenieRightmarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("OdobrenieRightmargin", "OdobrenieRightmargin", 0))
    Private Shared OdobrenieButtonMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("OdobrenieButtonMargin", "OdobrenieButtonMargin", 0))

    'Soobrakajna
    Private Shared SoobrakajnaTopMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("SoobrakajnaTopMargin", "SoobrakajnaTopMargin", 0))
    Private Shared SoobrakajnaLeftMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("SoobrakajnaLeftMargin", "SoobrakajnaLeftMargin", 0))
    Private Shared SoobrakajnaRightmarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("SoobrakajnaRightmargin", "SoobrakajnaRightmargin", 0))
    Private Shared SoobrakajnaButtonMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("SoobrakajnaButtonMargin", "SoobrakajnaButtonMargin", 0))

    'Megunarodna
    Private Shared MegunarodnaTopMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("MegunarodnaTopMargin", "MegunarodnaTopMargin", 0))
    Private Shared MegunarodnaLeftMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("MegunarodnaLeftMargin", "MegunarodnaLeftMargin", 0))
    Private Shared MegunarodnaRightmarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("MegunarodnaRightmargin", "MegunarodnaRightmargin", 0))
    Private Shared MegunarodnaButtonMarginProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("MegunarodnaButtonMargin", "MegunarodnaButtonMargin", 0))

    Private Shared SaveLayoutProperty As PropertyInfo(Of Boolean) _
 = RegisterProperty(Of Boolean)(GetType(Options), New PropertyInfo(Of Boolean)("SaveLayout"))
    Private Shared FiskalFolderPathProperty As PropertyInfo(Of String) _
     = RegisterProperty(Of String)(GetType(Options), _
     New PropertyInfo(Of String)("FiskalFolderPath", "FiskalFolderPath"))
    Private Shared DuplaSmetkaProperty As PropertyInfo(Of Boolean) = _
    RegisterProperty(Of Boolean)(GetType(Options), New PropertyInfo(Of Boolean)("DoubleInvoice"))


    Public Property MegunarodnaButtonMargin() As Integer
        Get
            Return GetProperty(Of Integer)(MegunarodnaButtonMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MegunarodnaButtonMarginProperty, value)
        End Set
    End Property

    Public Property MegunarodnaRightmargin() As Integer
        Get
            Return GetProperty(Of Integer)(MegunarodnaRightmarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MegunarodnaRightmarginProperty, value)
        End Set
    End Property

    Public Property MegunarodnaLeftMargin() As Integer
        Get
            Return GetProperty(Of Integer)(MegunarodnaLeftMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MegunarodnaLeftMarginProperty, value)
        End Set
    End Property

    Public Property MegunarodnaTopMargin() As Integer
        Get
            Return GetProperty(Of Integer)(MegunarodnaTopMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(MegunarodnaTopMarginProperty, value)
        End Set
    End Property


    Public Property SoobrakajnaButtonMargin() As Integer
        Get
            Return GetProperty(Of Integer)(SoobrakajnaButtonMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(SoobrakajnaButtonMarginProperty, value)
        End Set
    End Property

    Public Property SoobrakajnaRightmargin() As Integer
        Get
            Return GetProperty(Of Integer)(SoobrakajnaRightmarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(SoobrakajnaRightmarginProperty, value)
        End Set
    End Property

    Public Property SoobrakajnaLeftMargin() As Integer
        Get
            Return GetProperty(Of Integer)(SoobrakajnaLeftMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(SoobrakajnaLeftMarginProperty, value)
        End Set
    End Property

    Public Property SoobrakajnaTopMargin() As Integer
        Get
            Return GetProperty(Of Integer)(SoobrakajnaTopMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(SoobrakajnaTopMarginProperty, value)
        End Set
    End Property

    Public Property OdobrenieButtonMargin() As Integer
        Get
            Return GetProperty(Of Integer)(OdobrenieButtonMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(OdobrenieButtonMarginProperty, value)
        End Set
    End Property

    Public Property OdobrenieRightmargin() As Integer
        Get
            Return GetProperty(Of Integer)(OdobrenieRightmarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(OdobrenieRightmarginProperty, value)
        End Set
    End Property

    Public Property OdobrenieLeftMargin() As Integer
        Get
            Return GetProperty(Of Integer)(OdobrenieLeftMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(OdobrenieLeftMarginProperty, value)
        End Set
    End Property

    Public Property OdobrenieTopMargin() As Integer
        Get
            Return GetProperty(Of Integer)(OdobrenieTopMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(OdobrenieTopMarginProperty, value)
        End Set
    End Property

    Public Property BelButtonMargin() As Integer
        Get
            Return GetProperty(Of Integer)(BelButtonMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(BelButtonMarginProperty, value)
        End Set
    End Property

    Public Property BelRightmargin() As Integer
        Get
            Return GetProperty(Of Integer)(BelRightmarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(BelRightmarginProperty, value)
        End Set
    End Property

    Public Property BelLeftMargin() As Integer
        Get
            Return GetProperty(Of Integer)(BelLeftMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(BelLeftMarginProperty, value)
        End Set
    End Property

    Public Property BelTopMargin() As Integer
        Get
            Return GetProperty(Of Integer)(BelTopMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(BelTopMarginProperty, value)
        End Set
    End Property


    Public Property PlavButtonMargin() As Integer
        Get
            Return GetProperty(Of Integer)(PlavButtonMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(PlavButtonMarginProperty, value)
        End Set
    End Property

    Public Property PlavRightMargin() As Integer
        Get
            Return GetProperty(Of Integer)(PlavRightMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(PlavRightMarginProperty, value)
        End Set
    End Property

    Public Property PlavLeftMargin() As Integer
        Get
            Return GetProperty(Of Integer)(PlavLeftMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(PlavLeftMarginProperty, value)
        End Set
    End Property


    Public Property PlavTopMargin() As Integer
        Get
            Return GetProperty(Of Integer)(PlavTopMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(PlavTopMarginProperty, value)
        End Set
    End Property

    Public Property ZelenTopMargin() As Integer
        Get
            Return GetProperty(Of Integer)(ZelenTopMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(ZelenTopMarginProperty, value)
        End Set
    End Property

    Public Property ZelenLeftMargin() As Integer
        Get
            Return GetProperty(Of Integer)(ZelenLeftMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(ZelenLeftMarginProperty, value)
        End Set
    End Property

    Public Property ZelenRightMargin() As Integer
        Get
            Return GetProperty(Of Integer)(ZelenRightMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(ZelenRightMarginProperty, value)
        End Set
    End Property

    Public Property ZelenButtonMargin() As Integer
        Get
            Return GetProperty(Of Integer)(ZelenButtonMarginProperty)
        End Get
        Set(ByVal value As Integer)
            SetProperty(Of Integer)(ZelenButtonMarginProperty, value)
        End Set
    End Property


    Public Property FiskalFolderPath() As String
        Get
            Return GetProperty(Of String)(FiskalFolderPathProperty)
        End Get
        Set(ByVal value As String)
            SetProperty(Of String)(FiskalFolderPathProperty, value)
        End Set
    End Property

 


    Public Property SaveLayout() As Boolean
        Get
            Return ReadProperty(Of Boolean)(SaveLayoutProperty)
        End Get
        Set(ByVal value As Boolean)
            SetProperty(Of Boolean)(SaveLayoutProperty, value)
        End Set
    End Property

  
    'Public Property KodNaStanica() As String
    '    Get
    '        Return ReadProperty(Of String)(KodNaStanicaProperty)
    '    End Get
    '    Set(ByVal value As String)
    '        SetProperty(Of String)(KodNaStanicaProperty, value)
    '    End Set
    'End Property
   
    Public Property DuplaSmetka() As Boolean
        Get
            Return ReadProperty(Of Boolean)(DuplaSmetkaProperty)
        End Get
        Set(ByVal value As Boolean)
            SetProperty(Of Boolean)(DuplaSmetkaProperty, value)
        End Set
    End Property

   
#End Region

#Region " Factory Methods "

    Private Sub New()
        ' require use of factory method 
    End Sub

    Public Shared Function NewOptions() As Options
        Return DataPortal.Create(Of Options)()
    End Function

    Public Shared Function GetOptions() As Options
        Return DataPortal.Fetch(Of Options)()
    End Function

    'Public Shared Sub DeleteOptions()
    '  DataPortal.Delete(New SingleCriteria(Of Options, ))
    'End Sub
    Public Overrides Function Save() As Options
        Return MyBase.Save()
    End Function
#End Region ' Factory Methods

#Region " Validation Rules "
    Protected Overrides Sub AddBusinessRules()
        'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
        '                        New Validation.IntegerMinValueRuleArgs(CompanyProperty, 1))
        'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
        '                        New Validation.IntegerMinValueRuleArgs(DefaultCityProperty, 1))
        'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
        '                        New Validation.IntegerMinValueRuleArgs(DefaultRegistrationIssuerProperty, 1))
        'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
        '                        New Validation.IntegerMinValueRuleArgs(IdCommunityProperty, 1))
    End Sub
#End Region

    <RunLocal()> _
    Private Overloads Sub DataPortal_Fetch()
        'proveri dali postoi kluc vo HKEY_CURRENT_USER
        Dim regVersion As Microsoft.Win32.RegistryKey
        regVersion = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BSS\\VTE\\", True)
        If regVersion Is Nothing Then
            regVersion = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BSS\\VTE\\", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
        End If

        'margin
        'zelen
        LoadProperty(Of Integer)(ZelenButtonMarginProperty, CType(regVersion.GetValue("ZelenButtonMargin", "0"), Integer))
        LoadProperty(Of Integer)(ZelenRightMarginProperty, CType(regVersion.GetValue("ZelenRightMargin", "0"), Integer))
        LoadProperty(Of Integer)(ZelenLeftMarginProperty, CType(regVersion.GetValue("ZelenLeftMargin", "0"), Integer))
        LoadProperty(Of Integer)(ZelenTopMarginProperty, CType(regVersion.GetValue("ZelenTopMargin", "0"), Integer))
        'plav
        LoadProperty(Of Integer)(PlavButtonMarginProperty, CType(regVersion.GetValue("PlavButtonMargin", "0"), Integer))
        LoadProperty(Of Integer)(PlavRightMarginProperty, CType(regVersion.GetValue("PlavRightMargin", "0"), Integer))
        LoadProperty(Of Integer)(PlavLeftMarginProperty, CType(regVersion.GetValue("PlavLeftMargin", "0"), Integer))
        LoadProperty(Of Integer)(PlavTopMarginProperty, CType(regVersion.GetValue("PlavTopMargin", "0"), Integer))
        'bel
        LoadProperty(Of Integer)(BelButtonMarginProperty, CType(regVersion.GetValue("BelButtonMargin", "0"), Integer))
        LoadProperty(Of Integer)(BelRightmarginProperty, CType(regVersion.GetValue("BelRightMargin", "0"), Integer))
        LoadProperty(Of Integer)(BelLeftMarginProperty, CType(regVersion.GetValue("BelLeftMargin", "0"), Integer))
        LoadProperty(Of Integer)(BelTopMarginProperty, CType(regVersion.GetValue("BelTopMargin", "0"), Integer))
        'Odobrenie
        LoadProperty(Of Integer)(OdobrenieButtonMarginProperty, CType(regVersion.GetValue("OdobrenieButtonMargin", "0"), Integer))
        LoadProperty(Of Integer)(OdobrenieRightmarginProperty, CType(regVersion.GetValue("OdobrenieRightMargin", "0"), Integer))
        LoadProperty(Of Integer)(OdobrenieLeftMarginProperty, CType(regVersion.GetValue("OdobrenieLeftMargin", "0"), Integer))
        LoadProperty(Of Integer)(OdobrenieTopMarginProperty, CType(regVersion.GetValue("OdobrenieTopMargin", "0"), Integer))
        'Megunarodna
        LoadProperty(Of Integer)(MegunarodnaButtonMarginProperty, CType(regVersion.GetValue("MegunarodnaButtonMargin", "0"), Integer))
        LoadProperty(Of Integer)(MegunarodnaRightmarginProperty, CType(regVersion.GetValue("MegunarodnaRightMargin", "0"), Integer))
        LoadProperty(Of Integer)(MegunarodnaLeftMarginProperty, CType(regVersion.GetValue("MegunarodnaLeftMargin", "0"), Integer))
        LoadProperty(Of Integer)(MegunarodnaTopMarginProperty, CType(regVersion.GetValue("MegunarodnaTopMargin", "0"), Integer))
        'Soobrakajna
        LoadProperty(Of Integer)(SoobrakajnaButtonMarginProperty, CType(regVersion.GetValue("SoobrakajnaButtonMargin", "0"), Integer))
        LoadProperty(Of Integer)(SoobrakajnaRightmarginProperty, CType(regVersion.GetValue("SoobrakajnaRightMargin", "0"), Integer))
        LoadProperty(Of Integer)(SoobrakajnaLeftMarginProperty, CType(regVersion.GetValue("SoobrakajnaLeftMargin", "0"), Integer))
        LoadProperty(Of Integer)(SoobrakajnaTopMarginProperty, CType(regVersion.GetValue("SoobrakajnaTopMargin", "0"), Integer))

        'LoadProperty(Of String)(PictureServerPathProperty, CType(regVersion.GetValue("PictureServerPath", "c:"), String))
        'LoadProperty(Of String)(LogoPathProperty, CType(regVersion.GetValue("LogoPath", "c:"), String))
        If regVersion.GetValue("SaveLayout", "0") = 0 Then
            LoadProperty(Of Boolean)(SaveLayoutProperty, False)
        Else
            LoadProperty(Of Boolean)(SaveLayoutProperty, True)
        End If
        LoadProperty(Of String)(FiskalFolderPathProperty, CType(regVersion.GetValue("FiskalFolderPath", "c:"), String))
        If regVersion.GetValue("DoubleInvoice", "0") = 0 Then
            LoadProperty(Of Boolean)(DuplaSmetkaProperty, False)
        Else
            LoadProperty(Of Boolean)(DuplaSmetkaProperty, True)
        End If

     

    End Sub
    <RunLocal()> _
    Protected Overrides Sub DataPortal_Update()
        'proveri dali postoi kluc vo HKEY_CURRENT_USER
        'Dim regVersion As Microsoft.Win32.RegistryKey
        'regVersion = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BSS\\VTE\\", True)
        'If regVersion Is Nothing Then
        '  Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BSS\\VTE\\", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
        'End If
        Dim regVersion As Microsoft.Win32.RegistryKey
        regVersion = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BSS\\VTE\\", True)
        If regVersion Is Nothing Then
            regVersion = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BSS\\VTE\\", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
        End If
        If SaveLayout = False Then
            regVersion.SetValue("SaveLayout", "0", Microsoft.Win32.RegistryValueKind.String)
        Else
            regVersion.SetValue("SaveLayout", "1", Microsoft.Win32.RegistryValueKind.String)
        End If
        'If ApproveRequestAutomate = False Then
        '    regVersion.SetValue("ApproveRequestAutomate", "0", Microsoft.Win32.RegistryValueKind.String)
        'Else
        '    regVersion.SetValue("ApproveRequestAutomate", "1", Microsoft.Win32.RegistryValueKind.String)
        'End If
        'If NewTechnicalExamReport = False Then
        '    regVersion.SetValue("NewTechnicalExamReport", "0", Microsoft.Win32.RegistryValueKind.String)
        'Else
        '    regVersion.SetValue("NewTechnicalExamReport", "1", Microsoft.Win32.RegistryValueKind.String)
        'End If

        'regVersion.SetValue("IdCommunity", ReadProperty(Of Integer)(IdCommunityProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("TrafficLicenceVlidNumOfMonths", ReadProperty(Of Integer)(TrafficLicenceVlidNumOfMonthsProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("RegistrationVlidNumOfMonths", ReadProperty(Of Integer)(RegistrationVlidNumOfMonthsProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("Company", ReadProperty(Of Integer)(CompanyProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("PictureServerPath", ReadProperty(Of String)(PictureServerPathProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("LogoPath", ReadProperty(Of String)(LogoPathProperty), Microsoft.Win32.RegistryValueKind.String)

        'regVersion.SetValue("OdgovorenOrgan", ReadProperty(Of String)(OdgovorenOrganProperty), Microsoft.Win32.RegistryValueKind.String)
        '' regVersion.SetValue("KodNaStanica", ReadProperty(Of String)(KodNaStanicaProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("Sekretar", ReadProperty(Of String)(SekretarProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("FiskalFolderPath", ReadProperty(Of String)(FiskalFolderPathProperty), Microsoft.Win32.RegistryValueKind.String)

        'regVersion.SetValue("PaymentPrintOption", ReadProperty(Of Integer)(PaymentPrintOptionProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("DefaultRegistrationIssuer", ReadProperty(Of Integer)(DefaultRegistrationIssuerProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("DefaultCity", ReadProperty(Of Integer)(DefaultCityProperty), Microsoft.Win32.RegistryValueKind.String)
        'If AutmateProceses = False Then
        '    regVersion.SetValue("AutmateProceses", "0", Microsoft.Win32.RegistryValueKind.String)
        'Else
        '    regVersion.SetValue("AutmateProceses", "1", Microsoft.Win32.RegistryValueKind.String)
        'End If

        'regVersion.SetValue("ZiroSmetka", ReadProperty(Of String)(ZiroSmetkaProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("Deponent", ReadProperty(Of String)(DeponentProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("StationAddress", ReadProperty(Of String)(StationAddressProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("EDB", ReadProperty(Of String)(EDBProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("Tel", ReadProperty(Of String)(TelProperty), Microsoft.Win32.RegistryValueKind.String)
        'regVersion.SetValue("Fax", ReadProperty(Of String)(FaxProperty), Microsoft.Win32.RegistryValueKind.String)
        If DuplaSmetka = False Then
            regVersion.SetValue("DoubleInvoice", "0", Microsoft.Win32.RegistryValueKind.String)
        Else
            regVersion.SetValue("DoubleInvoice", "1", Microsoft.Win32.RegistryValueKind.String)
        End If
        ' regVersion.SetValue("DoubleInvoice", ReadProperty(Of Boolean)(DuplaSmetkaProperty), Microsoft.Win32.RegistryValueKind.String)
        'margini
        'zelen
        regVersion.SetValue("ZelenButtonMargin", ReadProperty(Of Integer)(ZelenButtonMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("ZelenLeftMargin", ReadProperty(Of Integer)(ZelenLeftMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("ZelenRightMargin", ReadProperty(Of Integer)(ZelenRightMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("ZelenTopMargin", ReadProperty(Of Integer)(ZelenTopMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        'plav
        regVersion.SetValue("PlavButtonMargin", ReadProperty(Of Integer)(PlavButtonMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("PlavLeftMargin", ReadProperty(Of Integer)(PlavLeftMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("PlavRightMargin", ReadProperty(Of Integer)(PlavRightMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("PlavTopMargin", ReadProperty(Of Integer)(PlavTopMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        'Bel
        regVersion.SetValue("BelButtonMargin", ReadProperty(Of Integer)(BelButtonMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("BelLeftMargin", ReadProperty(Of Integer)(BelLeftMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("BelRightMargin", ReadProperty(Of Integer)(BelRightmarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("BelTopMargin", ReadProperty(Of Integer)(BelTopMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        'Soobrakajna
        regVersion.SetValue("SoobrakajnaButtonMargin", ReadProperty(Of Integer)(SoobrakajnaButtonMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("SoobrakajnaLeftMargin", ReadProperty(Of Integer)(SoobrakajnaLeftMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("SoobrakajnaRightMargin", ReadProperty(Of Integer)(SoobrakajnaRightmarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("SoobrakajnaTopMargin", ReadProperty(Of Integer)(SoobrakajnaTopMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        'Megunarodna
        regVersion.SetValue("MegunarodnaButtonMargin", ReadProperty(Of Integer)(MegunarodnaButtonMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("MegunarodnaLeftMargin", ReadProperty(Of Integer)(MegunarodnaLeftMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("MegunarodnaRightMargin", ReadProperty(Of Integer)(MegunarodnaRightmarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("MegunarodnaTopMargin", ReadProperty(Of Integer)(MegunarodnaTopMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        'Odobrenie
        regVersion.SetValue("OdobrenieButtonMargin", ReadProperty(Of Integer)(OdobrenieButtonMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("OdobrenieLeftMargin", ReadProperty(Of Integer)(OdobrenieLeftMarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("OdobrenieRightMargin", ReadProperty(Of Integer)(OdobrenieRightmarginProperty), Microsoft.Win32.RegistryValueKind.String)
        regVersion.SetValue("OdobrenieTopMargin", ReadProperty(Of Integer)(OdobrenieTopMarginProperty), Microsoft.Win32.RegistryValueKind.String)

        'regVersion.SetValue("Valuta", ReadProperty(Of Integer)(ValutaProperty), Microsoft.Win32.RegistryValueKind.String)
    End Sub


End Class

