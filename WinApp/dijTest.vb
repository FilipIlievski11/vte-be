Imports System.Data
Imports System.Data.OleDb

Public Class dijTest

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
    'Me.PaymentCataologListBindingSource.DataSource = objPaymentCatalogList
  End Sub

  'Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
  '  Dim excelConnetion As String = _
  '    "Provider=Microsoft.Jet.OLEDB.4.0;" & _
  '    "Data Source=" & "c:\STP2007.xls" & ";" & _
  '    "Extended Properties=Excel 8.0;"

  '  Dim Proizveduvac As VehicleMaker
  '  Dim Model As VehicleModel

  '  Using cn As New OleDbConnection(excelConnetion)
  '    cn.Open()
  '    Using cm As OleDbCommand = cn.CreateCommand
  '      cm.CommandText = "SELECT * FROM [" & "stp2007" & "$]"
  '      Using dr As OleDbDataReader = cm.ExecuteReader
  '        While dr.Read
  '          'nov Proizveduvac
  '          If dr(0).ToString <> String.Empty Then
  '            Proizveduvac = VehicleMaker.NewVehicleMaker
  '            Proizveduvac.IdCountry = 2
  '            Proizveduvac.CompanyName = dr(0).ToString
  '            Proizveduvac = Proizveduvac.Save

  '          Else
  '            'dodadi model na toj proizveduvac
  '            If dr(3).ToString <> String.Empty Then
  '              Model = VehicleModel.NewVehicleModel
  '              Model.IdVehicleMaker = Proizveduvac.Id
  '              Model.ModelName = dr(3).ToString
  '              Model.Save()
  '            End If
  '          End If
  '          Me.ListBoxControl1.Items.Add(dr(0).ToString & " - " & dr(3).ToString)
  '        End While
  '      End Using
  '    End Using
  '  End Using

  'End Sub

  Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
    'FontDialog1.ShowDialog()
    'Dim font As Font = FontDialog1.Font
    'Console.WriteLine(font.Name)
    'Console.WriteLine(font.Size)
    'Console.WriteLine(font.Bold)
    'Console.WriteLine(font.Italic)


    'gumi
    'Dim excelConnetion As String = _
    '  "Provider=Microsoft.Jet.OLEDB.4.0;" & _
    '  "Data Source=" & "c:\CENOVNIK_gumi.xls" & ";" & _
    '  "Extended Properties=Excel 8.0;"


    'Using cn As New OleDbConnection(excelConnetion)
    '  cn.Open()
    '  Using cm As OleDbCommand = cn.CreateCommand
    '    cm.CommandText = "SELECT * FROM [" & "KONECNA VERZIJA" & "$]"
    '    Using dr As OleDbDataReader = cm.ExecuteReader
    '      While dr.Read
    '        If dr(0).ToString <> String.Empty Then
    '          Dim guma As VehicleTireType = VehicleTireType.NewVehicleTireType
    '          guma.Dimenzions = dr(4).ToString
    '          guma.Seria = dr(2).ToString
    '          Dim tmp As String = String.Empty
    '          For i As Integer = 0 To 7
    '            tmp = tmp & dr(i).ToString
    '          Next
    '          guma.TireType = tmp.Trim
    '          guma.Note = dr(8).ToString & " " & dr(9).ToString
    '          guma.Save()
    '        End If
    '      End While
    '    End Using

    '  End Using
    'End Using
    'MsgBox(ToLat("Нешто на килрилица, Чч Ѓѓ Жж Шш "))
    DodadiOpstiniNaseleniMesta()
    MsgBox("OK")
  End Sub

  Private Sub DodadiOpstiniNaseleniMesta()
    Dim excelConnetion As String = _
      "Provider=Microsoft.Jet.OLEDB.4.0;" & _
      "Data Source=" & "D:\downloads\SpisokOpstiniNM_ZaWEB1.xls" & ";" & _
      "Extended Properties=Excel 8.0;"

    Using cn As New OleDbConnection(excelConnetion)
      cn.Open()
      Using cm As OleDbCommand = cn.CreateCommand
        cm.CommandText = "SELECT * FROM [" & "Sheet1" & "$]"
        Using dr As OleDbDataReader = cm.ExecuteReader
          While dr.Read
            'indeksi
            '0 Матичен број на општина
            '1 Назив на општина
            '2 Матичен број на населено место
            '3 Назив на населено место
            '4 Град (г) / Село (с)
            If dr(0).ToString <> String.Empty Then
              'ako postoi opstinata
              Dim IdOpstina As Integer = Community.Exists(dr(1))
              If Not (IdOpstina <> 0) Then
                'dodadi nova opstina
                Dim opstina As Community = Community.NewCommunity
                opstina.CommunityCode = dr(0).ToString
                opstina.CommunityName = dr(1).ToString
                If opstina.CommunityName.Contains("Велес") Then
                  opstina.RegistrationCode = "VE"
                End If
                opstina = opstina.Save
                IdOpstina = opstina.Id

              End If
              'ako ne postoi naselenoto mesto dodadi novo naseleno mesto
              If City.Exists(dr(4).ToString & "." & dr(3).ToString) = 0 Then
                Dim naselenoMesto As City = City.NewCity
                naselenoMesto.CityName = dr(4).ToString & "." & dr(3).ToString
                naselenoMesto.CityZip = 0
                naselenoMesto.IdCountry = 77 'makedonija
                naselenoMesto.IdCommunityCode = IdOpstina
                naselenoMesto = naselenoMesto.Save

              End If
            End If
          End While
        End Using

      End Using
    End Using

  End Sub

End Class