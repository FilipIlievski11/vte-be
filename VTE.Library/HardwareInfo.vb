Imports Microsoft.Win32
Imports System.Management
Imports System.IO
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System

Public Class HardwareInfo


    Public Shared Function GetMAC() As String
        Dim wmi = New ManagementClass()
        Dim obj As ManagementObject
        'Dim num As Integer
        'Dim temp As String

        Dim ID_MAC As String

        Try
            ID_MAC = ""
            wmi = New ManagementClass("Win32_NetworkAdapter")
            wmi.Path.RelativePath = "Win32_NetworkAdapter"
            For Each obj In wmi.GetInstances()

                If Not IsNothing(obj("MACAddress")) Then
                    ID_MAC = obj("MACAddress").ToString
                    Exit For
                End If
            Next

            If ID_MAC = "" Then
                wmi.Path.RelativePath = "Win32_NetworkAdapterConfiguration"
                For Each obj In wmi.GetInstances()
                    If Not IsNothing(obj("IPAddress")) AndAlso UBound(CType(obj("IPAddress"), String())) >= 0 Then
                        ID_MAC = obj("MACAddress").ToString
                    End If
                Next
            End If

        Catch ex As Exception
            'MsgBox(ex.Message)
            Return ""
        End Try
        Return ID_MAC
    End Function

    Public Shared Function GetHDD() As String
        Dim wmi = New ManagementClass()
        Dim obj, related As ManagementObject
        Dim cnt As Integer
        'Dim num As Integer
        Dim temp As String

        Dim ID_HDD As String
        Try
            cnt = 0

            ID_HDD = ""


            wmi = New ManagementClass("Win32_DiskDrive")
            wmi.Path.RelativePath = "Win32_DiskDrive"
            For Each obj In wmi.GetInstances()

                'If cnt > 0 And Not IsNothing(obj("")) Then
                '  Exit For
                'End If

                ' let's see if we can get a better Serial Number
                For Each related In obj.GetRelated("Win32_PhysicalMedia")
                    Dim i As Integer
                    Dim c As Char
                    Dim IsRaw As Boolean
                    Dim ValidHex As String = "0123456789abcdefABCDEF"

                    If Not IsNothing(related("SerialNumber")) Then
                        temp = related("SerialNumber").ToString
                        ' as usual, some sanity checking
                        If temp = "" Then
                            Exit For
                        End If

                        ' let's see if this is really an ASCII hex string
                        For Each c In temp.ToCharArray
                            If ValidHex.IndexOf(c) < 0 Then
                                IsRaw = True
                                Exit For
                            End If
                        Next

                        If Not IsRaw Then
                            ' convert a hex string into a string
                            Dim sb As New System.Text.StringBuilder

                            For i = 0 To temp.Length - 1 Step 2
                                sb.Append(Chr(CInt("&h" & temp.Substring(i, 2))))
                            Next
                            ID_HDD = Trim(sb.ToString)
                        Else
                            ID_HDD = Trim(related("SerialNumber").ToString)
                        End If
                    End If
                Next
                cnt += 1
            Next

        Catch ex As Exception
            'MsgBox(ex.Message)
            Return ""
        End Try
        Return ID_HDD
    End Function

    Public Shared Function GetBIOS() As String
        Dim wmi = New ManagementClass()
        Dim obj As ManagementObject
        Dim cnt As Integer
        'Dim num As Integer
        'Dim temp As String

        Dim ID_BIOS As String
        Try
            cnt = 0

            ID_BIOS = ""

            ' This is a bit different... we don't want multiple entries for PCs
            ' with dual processsors, instead we collapse the data into a single
            ' data row and add a CPU count.
            wmi = New ManagementClass("Win32_BIOS")
            wmi.Path.RelativePath = "Win32_BIOS"
            For Each obj In wmi.GetInstances()
                ' We do not count the hyperthreading CPUs as real CPUs
                ' BTW: I've found a few systems that can fool this simple test
                If cnt > 0 And Not IsNothing(obj("SerialNumber")) Then
                    Exit For
                End If
                ID_BIOS = obj("SerialNumber").ToString
                cnt += 1
            Next

        Catch ex As Exception
            'MsgBox(ex.Message)
            Return ""
        End Try
        Return ID_BIOS
    End Function

    Public Shared Function GetCPU() As String
        Dim wmi = New ManagementClass()
        Dim obj As ManagementObject
        Dim cnt As Integer
        'Dim num As Integer
        'Dim temp As String

        Dim ID_CPU As String
        Try
            cnt = 0

            ID_CPU = ""

            ' This is a bit different... we don't want multiple entries for PCs
            ' with dual processsors, instead we collapse the data into a single
            ' data row and add a CPU count.
            wmi = New ManagementClass("Win32_Processor")
            wmi.Path.RelativePath = "Win32_Processor"
            For Each obj In wmi.GetInstances()
                ' We do not count the hyperthreading CPUs as real CPUs
                ' BTW: I've found a few systems that can fool this simple test
                If cnt > 0 And Not IsNothing(obj("ProcessorID")) AndAlso obj("ProcessorID").ToString = "0000000000000000" Then
                    Exit For
                End If
                ID_CPU = obj("ProcessorID").ToString
                cnt += 1

            Next



        Catch ex As Exception
            'MsgBox(ex.Message)
            Return ""
        End Try
        Return ID_CPU
    End Function

End Class
