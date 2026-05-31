Imports System.IO

Public Class AutoUpdate

  Public Function AutoUpdate(ByRef CommandLine As String, ByVal RemotePath As String) As Boolean
    Dim Key As String = "VERTESTAutoUpdate" ' ova e bilo koja unikatna niza od karakteri
    ' фајлот со информациите за update-от
    Dim sfile As String = "updateVERTEST.dat"
    ' асембли името
    Dim AssemblyName As String = _
    System.Reflection.Assembly.GetEntryAssembly.GetName.Name
    ' местото каде се наоѓаат фајловите во системот од каде треба да се превземат
    Dim RemoteUri As String = RemotePath & "/" & AssemblyName & "/"
    ' на командната линија се испишува клучот (така после се споредува)
    CommandLine = Replace(Microsoft.VisualBasic.Command(), Key, "")
    ' овде го споредува клучот
    If InStr(Microsoft.VisualBasic.Command(), Key) > 0 Then
      Try
        ' се брише ауто апдејт програмот затоа што не треба веќе
        System.IO.File.Delete(Application.StartupPath & "/SampleWebDownloader.exe")
      Catch ex As Exception
      End Try
      ' не требало апдејт но враќаме True бидејќки се е во ред
      Return True
    Else
      ' програмот за апдејт бил повикан од програмот
      Dim ret As Boolean = True ' Default – се е во ред може да продолжи програмот
      Try
        Dim myWebClient As New System.Net.WebClient  'the webclient
        ' Download the update info file to the memory,
        ' read and close the stream
        myWebClient.BaseAddress = ""

        myWebClient.Credentials = New System.Net.NetworkCredential("stefan", "DuleMule1.")

        ' тука се превзема локално фајлот за апдејт

        Dim uri As Uri = New Uri("ftp://" & RemoteUri & sfile)

        Dim newStream As Stream = myWebClient.OpenRead(uri)

        Dim file As StreamReader = New StreamReader(newStream)
        Dim Contents As String = file.ReadToEnd()
        file.Close()

        ' се чита цела содржина на фајлот и доколку е различна од празен стринг се продолжува
        If Contents <> "" Then
          ' Break the contents
          Dim x() As String = Split(Contents, "|")
          ' првиот параметар е верзијата на програмот. ако е 
          ' поголем од верзијата која го стартувала ауто апдејтот 
          ' значи има потреба за ауто апдејт
          Dim verzija As Integer = CInt(CStr(Application.ProductVersion).Replace(".", ""))
          If x(0) > verzija Then

            'овде се спремаат параметрите кои ќе се препратат на ауто апдејт програмот
            Dim arg As String = AssemblyName & "|" & _
            "ftp://" & RemoteUri & "|" & x(1) & "|" & Key & "|" & _
            Microsoft.VisualBasic.Command()

            ' се спушта програмот за земање на новите фајлови
            'MsgBox("ftp://" & RemoteUri & "autoupdate.exe")
            myWebClient.DownloadFile("ftp://" & RemoteUri & "SampleWebDownloader.exe", _
            Application.StartupPath & "/SampleWebDownloader.exe")

            'Се повикува програмот со сите параметри
            System.Diagnostics.Process.Start( _
            Application.StartupPath & "/SampleWebDownloader.exe", arg)

            ret = False

          End If
        End If
      Catch ex As Exception
        ' if there is an error return true,
        ' what means that the application
        ' should be closed

        ' something went wrong…
        MsgBox("Новата верзија не може да се превземе затоа што не може да се воспостави конекција со серверот. Ве молиме обидете се подоцна или обратете се во техничка подршка.")
        'Dim dij As New MessageBox("Новата верзија не може да се превземе затоа што не може да се воспостави конекција со серверот. Ве молиме обидете се подоцна или обратете се во техничка подршка.")
        'dij.ShowDialog()
        ret = True
        ''MsgBox(ex.Message)
      End Try

      Return ret
    End If
  End Function
End Class