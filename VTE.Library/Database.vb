Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Csla
Imports Csla.Data
Imports System.Configuration
Imports System.IO
Imports System.ComponentModel
Imports System.Diagnostics

<Assembly: log4net.Config.XmlConfigurator(Watch:=True)> 

<Serializable()> _
Partial Public NotInheritable Class Database
  Private Sub New()
  End Sub

#Region "Log4Net"
  Private Shared ReadOnly _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
  Public Shared ReadOnly Property Log() As log4net.ILog
    Get
      Return _log
    End Get
  End Property

  Public Shared Sub LogException(ByVal s As String, ByVal ex As Exception)
    '_log.Error(s, ex)
    If _log.IsErrorEnabled Then

      _log.[Error](s, ex)
    End If
    Dim i As Integer = 0
    Console.WriteLine("Error - {0}", s)
    While ex IsNot Nothing
      Console.WriteLine("{0}{1} - {2}", "".PadLeft(System.Threading.Interlocked.Increment(i) * 2), ex.[GetType]().ToString(), ex.Message)
      ex = ex.InnerException
    End While
  End Sub

  Private Shared _LoggingInfo As Boolean = True
  Public Shared Property LoggingInfo() As Boolean
    Get
      Return _LoggingInfo
    End Get
    Set(ByVal value As Boolean)
      _LoggingInfo = value
    End Set
  End Property

  Shared _CurrentProcess As System.Diagnostics.Process = System.Diagnostics.Process.GetCurrentProcess()
  Public Shared Sub LogInfo(ByVal s As String, ByVal hashCode As Integer)
    If _LoggingInfo AndAlso _log.IsInfoEnabled Then
      _log.InfoFormat("{0} MB [{1}] {2}", _CurrentProcess.WorkingSet64 / 1000000, hashCode, s)
    End If
  End Sub
  Public Shared Sub LogInfoFormat(ByVal format As String, ByVal ParamArray objs As Object())
    If _log.IsInfoEnabled Then
      _log.InfoFormat("{0} MB {1}", _CurrentProcess.WorkingSet64 / 1000000, String.Format(format, objs))
    End If
  End Sub


#End Region

  Public Shared ReadOnly Property SecurityConnection() As String
        Get
            Return "Data Source=195.26.159.162,7899;Initial Catalog=VTESecurity;User ID=testapp1;Password=2_Snegot"
        End Get
  End Property
  Public Shared ReadOnly Property SecurityConnection_SqlConnection() As SqlConnection
    Get
      Dim strConn As String = SecurityConnection
      ' If failure - Fail (Don't try to catch)
      ' Attempt to make a connection 
      Try
        Dim cn As New SqlConnection(strConn)
        cn.Open()
        Return cn
      Catch exsql As SqlException
        Const strAttachError As String = "An attempt to attach an auto-named database for file "
        If exsql.Message.StartsWith(strAttachError) Then
          ' Check to see if the file is missing
          Dim sFile As String = exsql.Message.Substring(strAttachError.Length)
          sFile = sFile.Substring(0, sFile.IndexOf(" failed"))
          '	"An attempt to attach an auto-named database for file <mdf file> failed"
          If strConn.ToLower().IndexOf("user instance=true") < 0 Then
            Throw New ApplicationException("Connection String missing attribute: User Instance=True")
          End If
          If System.IO.File.Exists(sFile) Then
            Throw New ApplicationException("Database file " + sFile + " Cannot be opened" & Chr(13) & "" & Chr(10) & "", exsql)
          Else
            Throw New FileNotFoundException("Database file " + sFile + " Not Found", exsql)
          End If
        Else
          Throw New ApplicationException("Failure on Connect", exsql)
        End If
      Catch ex As Exception
        ' Throw Application Exception on Failure
        LogException("Connection Error", ex)
        Throw New ApplicationException("Failure on Connect", ex)
      End Try
    End Get
  End Property

  Public Shared ReadOnly Property VTEConnection() As String
    Get
      Return "Data Source=195.26.159.162,7899;Initial Catalog=VTEZVV;User ID=testapp1;Password=2_Snegot"
    End Get
  End Property
  Public Shared ReadOnly Property VTE_SqlConnection() As SqlConnection
    Get
      Dim strConn As String = VTEConnection
      ' If failure - Fail (Don't try to catch)
      ' Attempt to make a connection 
      Try
        Dim cn As New SqlConnection(strConn)
        cn.Open()
        Return cn
      Catch exsql As SqlException
        Const strAttachError As String = "An attempt to attach an auto-named database for file "
        If exsql.Message.StartsWith(strAttachError) Then
          ' Check to see if the file is missing
          Dim sFile As String = exsql.Message.Substring(strAttachError.Length)
          sFile = sFile.Substring(0, sFile.IndexOf(" failed"))
          '	"An attempt to attach an auto-named database for file <mdf file> failed"
          If strConn.ToLower().IndexOf("user instance=true") < 0 Then
            Throw New ApplicationException("Connection String missing attribute: User Instance=True")
          End If
          If System.IO.File.Exists(sFile) Then
            Throw New ApplicationException("Database file " + sFile + " Cannot be opened" & Chr(13) & "" & Chr(10) & "", exsql)
          Else
            Throw New FileNotFoundException("Database file " + sFile + " Not Found", exsql)
          End If
        Else
          Throw New ApplicationException("Failure on Connect", exsql)
        End If
      Catch ex As Exception
        ' Throw Application Exception on Failure
        LogException("Connection Error", ex)
        Throw New ApplicationException("Failure on Connect", ex)
      End Try
    End Get
  End Property

  Public Shared Sub PurgeData()
    Try
      Dim cn As SqlConnection = VTE_SqlConnection
      Dim cmd As New SqlCommand("purgeData", cn)
      cmd.CommandType = CommandType.StoredProcedure
      cmd.CommandTimeout = 0
      cmd.ExecuteNonQuery()
    Catch ex As Exception
      LogException("Purge Error", ex)
      Throw New ApplicationException("Failure on Purge", ex)
    End Try
  End Sub
End Class

Public Class DbCslaException
  Inherits Exception
  Friend Sub New(ByVal message As String, ByVal innerException As Exception)
    MyBase.New(message, innerException)


  End Sub
  Friend Sub New(ByVal message As String)
    MyBase.New(message)


  End Sub
  Friend Sub New()
    MyBase.New()


  End Sub
End Class
' Class


'Public Module Database





'  Private _connection As String
'  Public Property CompactPayrollConnection() As String
'    Get
'      Return _connection 'ConnectionStrings("").ConnectionString
'    End Get
'    Set(ByVal value As String)
'      _connection = value
'    End Set
'  End Property

'  Private _securityConnection As String
'  Public Property SecurityConnection() As String
'    Get
'      Return _securityConnection 'ConnectionStrings("").ConnectionString
'    End Get
'    Set(ByVal value As String)
'      _securityConnection = value
'    End Set
'  End Property

'End Module