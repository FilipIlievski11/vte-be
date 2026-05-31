Imports System.Collections
Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Xml

Public Class Installer

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent

    End Sub

    Private conStr As String = "packet size=4096;integrated security=SSPI;" + "data source=""(local)"";persist security info=False;" + "initial catalog=master"

    Public Overrides Sub Install(ByVal stateSaver As System.Collections.IDictionary)
        MyBase.Install(stateSaver)
        'If Context.Parameters("databaseServer").Length > 0 Then
        '    If Context.Parameters("userName").Length > 0 AndAlso Context.Parameters("userPass").Length > 0 Then
        '        conStr = GetLogin(Context.Parameters("databaseServer"), Context.Parameters("userName"), Context.Parameters("userPass"), "master")
        '    Else
        '        'so integrated security
        '        conStr = GetLogin(Context.Parameters("databaseServer"), "master")
        '    End If

        '    Dim rijndael As New RijndaelCryptography()
        '    rijndael.GenKey()
        '    rijndael.Encrypt(conStr)
        '    'save information in the state-saver IDictionary
        '    'to be used in the Uninstall method
        '    stateSaver.Add("key", rijndael.Key)
        '    stateSaver.Add("IV", rijndael.IV)
        '    stateSaver.Add("conStr", rijndael.Encrypted)
        'End If
        'Using sqlCon As New SqlConnection(conStr)
        '    sqlCon.Open()
        '    ExecuteSql(sqlCon)
        'End Using
        'StaviVoRegister()
    End Sub

    Public Overrides Sub Uninstall(ByVal savedState As System.Collections.IDictionary)
        MyBase.Uninstall(savedState)
        'If savedState.Contains("conStr") Then
        '    Dim rijndael As New RijndaelCryptography()

        '    rijndael.Key = CType((savedState("key")), Byte())
        '    rijndael.IV = CType(savedState("IV"), Byte())
        '    conStr = rijndael.Decrypt(CType(savedState("conStr"), Byte()))
        'End If

        'Dim sqlCon As New SqlConnection(conStr)

        'ExecuteDrop(sqlCon)
    End Sub

    Private Sub StaviVoRegister()
        Dim cry As New Crypt("VTEBSS")
        Dim crydata As New Crypt("VTEData")
        Dim userName As String = String.Empty
        Dim pass As String = String.Empty
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE", True).CreateSubKey("BSS", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS", True).CreateSubKey("VTE", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SERVER", Context.Parameters("databaseServer").ToString, Microsoft.Win32.RegistryValueKind.String)
        If Context.Parameters("userName").Length > 0 AndAlso Context.Parameters("userPass").Length > 0 Then
            userName = crydata.Encrypt(Context.Parameters("userName").ToString)
            pass = crydata.Encrypt(Context.Parameters("userPass").ToString)
            Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SERVER_IS", 0, Microsoft.Win32.RegistryValueKind.String)
            Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SERVER_USER_ID", userName, Microsoft.Win32.RegistryValueKind.String)
            Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SERVER_PWD", pass, Microsoft.Win32.RegistryValueKind.String)
        Else
            Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SERVER_IS", 1, Microsoft.Win32.RegistryValueKind.String)
            Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SERVER_USER_ID", userName, Microsoft.Win32.RegistryValueKind.String)
            Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SERVER_PWD", pass, Microsoft.Win32.RegistryValueKind.String)
        End If
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("AutmateProceses", "1", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("Company", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("DefaultCity", "1", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("DefaultRegistrationIssuer", "1", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("IdCommunity", "1", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("OdgovorenOrgan", "МВР", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("PaymentPrintOption", "1", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("RegistrationVlidNumOfMonths", "12", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("TrafficLicenceVlidNumOfMonths", "12", Microsoft.Win32.RegistryValueKind.String)
        If Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("KenoBingo").GetValue("SN", "") = "" Then
            Dim strTmp As String = cry.Encrypt(Format(Now.Date.AddDays(10), "dd/MM/yyyy")) & (cry.Encrypt(HardwareInfo.GetCPU) & cry.Encrypt(HardwareInfo.GetBIOS) & cry.Encrypt(HardwareInfo.GetMAC))
            Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("BSS").OpenSubKey("KenoBingo", True).SetValue("SN", strTmp, Microsoft.Win32.RegistryValueKind.String)
        End If

        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE", True).CreateSubKey("BSS", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("BSS", True).CreateSubKey("VTE", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)

        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("ApproveRequestAutomate", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("AutmateProceses", "1", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("BelButtonMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("BelLeftMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("BelRightMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("BelTopMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("Company", "1", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("Culture", "mk-MK", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("DefaultCity", "233", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("DefaultRegistrationIssuer", "1", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("Deponent", "СТБ", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("DoubleInvoice", "True", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("EDB", "100000005678", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("Fax", "", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("FiskalFolderPath", "C:\fiskal", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("FontSize", "8", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("IdCommunity", "21", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("KodNaStanica", "100", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("LogoPath", "D:\Doc Scans\E.jpg", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("MegunarodnaButtonMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("MegunarodnaLeftMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("MegunarodnaRightMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("MegunarodnaTopMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("OdgovorenOrgan", "МВР", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("OdobrenieButtonMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("OdobrenieLeftMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("OdobrenieRightMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("OdobrenieTopMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("PaymentPrintOption", "2", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("PictureServerPath", "D:\Proekti\Skenirani dokumneti", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("PlavButtonMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("PlavLeftMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("PlavRightMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("PlavTopMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("RegistrationVlidNumOfMonths", "12", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SaveLayout", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("Skin", "Office 2007 Blue", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SoobrakajnaButtonMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SoobrakajnaLeftMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SoobrakajnaRightMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("SoobrakajnaTopMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("StationAddress", "ул. бр.", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("Tel", "02/3296325", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("TrafficLicenceVlidNumOfMonths", "12", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("ZelenButtonMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("ZelenLeftMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("ZelenRightMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("ZelenTopMargin", "0", Microsoft.Win32.RegistryValueKind.String)
        Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BSS").OpenSubKey("VTE", True).SetValue("ZiroSmetka", "32310564163541", Microsoft.Win32.RegistryValueKind.String)

    End Sub

    Private Shared Function GetScript(ByVal name As String) As String
        Dim asm As Assembly = Assembly.GetExecutingAssembly()
        Dim str As Stream = asm.GetManifestResourceStream(asm.GetName().Name + "." + name)
        Dim reader As New StreamReader(str)
        Return reader.ReadToEnd()
    End Function

    Private Shared Function GetLogin(ByVal databaseServer As String, ByVal database As String) As String
        Return "server=" + databaseServer + ";database=" + database + ";Integrated security=true" '+ ";User ID=" + userName + ";Password=" + userPass
    End Function

    Private Shared Function GetLogin(ByVal databaseServer As String, ByVal userName As String, ByVal userPass As String, ByVal database As String) As String
        Return "server=" + databaseServer + ";database=" + database + ";Integrated security=false;User ID=" + userName + ";pwd=" + userPass
    End Function

    Private Shared Sub ExecuteSql(ByVal sqlCon As SqlConnection)


        Dim SqlLine As String()

        Dim regex As New Regex("^GO", RegexOptions.IgnoreCase Or RegexOptions.Multiline)

        Dim txtSQL As String = GetScript("sqlData.sql")

        SqlLine = regex.Split(txtSQL)

        Dim cmd As SqlCommand = sqlCon.CreateCommand()
        cmd.Connection = sqlCon

        For Each line As String In SqlLine
            If line.Length > 0 Then
                cmd.CommandText = line
                cmd.CommandType = CommandType.Text
                Try
                    cmd.ExecuteNonQuery()
                Catch generatedExceptionName As SqlException
                    'rollback
                    MsgBox(generatedExceptionName.Message.ToString())
                    ExecuteDrop(sqlCon)
                    Exit Try
                End Try
            End If
        Next
    End Sub

    Private Shared Sub ExecuteDrop(ByVal sqlCon As SqlConnection)
        If sqlCon.State <> ConnectionState.Closed Then
            sqlCon.Close()
        End If
        sqlCon.Open()
        Dim cmd As SqlCommand = sqlCon.CreateCommand()
        cmd.Connection = sqlCon
        cmd.CommandText = GetScript("Uninstall.sql")
        cmd.CommandType = CommandType.Text
        cmd.ExecuteNonQuery()
        sqlCon.Close()
    End Sub


End Class
