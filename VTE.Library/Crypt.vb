Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

'
' A Class to encrypt/decrypt the connection strings in the config file.
' Note: I don't like "Base 64" string format, so I use hex strings instead
'
Public Class Crypt

    ' Hey... it's not like I'm trying to keep spies out... I'm just
    ' trying to keep the casual user from poking around.
    Private TheKey() As Byte = {&H0, &H11, &H22, &H33, &H44, &H55, &H66, &H77}
    Private Vector() As Byte = {&H88, &H99, &HAA, &HBB, &HCC, &HDD, &HEE, &HFF}
    Private BlockSize As Integer = 8

    Public Sub New()
        Dim AssmemblyName As String
        Dim SaltChars() As Char

        AssmemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
        SaltChars = AssmemblyName.ToCharArray
        For i As Integer = 0 To 4
            TheKey(i) = TheKey(i) And Convert.ToByte(SaltChars(i))
        Next
    End Sub

    Public Sub New(ByVal AssemblyName As String)
        Dim SaltChars() As Char

        SaltChars = AssemblyName.ToCharArray
        For i As Integer = 0 To 4
            TheKey(i) = TheKey(i) And Convert.ToByte(SaltChars(i))
        Next
    End Sub

    Public Function Encrypt(ByVal message As String) As String
        Dim des As New DESCryptoServiceProvider
        Dim out As MemoryStream
        Dim sb As New StringBuilder
        Dim in_buf(), out_buf() As Byte
        Dim i, in_len, out_len As Integer

        in_len = message.Length
        If in_len = 0 Then
            Return ""
        End If
        ReDim out_buf(in_len - 1 + BlockSize)

        ' put the cleartext into the input byte array
        in_buf = Encoding.ASCII.GetBytes(message.ToCharArray)

        ' prepare the output stream
        out = New MemoryStream(out_buf, True)

        ' create an cryptographic output stream
        Dim crStream As New CryptoStream(out, des.CreateEncryptor(TheKey, Vector), CryptoStreamMode.Write)
        crStream.Write(in_buf, 0, in_len)
        crStream.Close()

        ' get the ciphertext out of the buffer (as a hex string)
        out_len = ((in_len \ BlockSize) + 1) * BlockSize
        For i = 0 To out_len - 1
            sb.AppendFormat("{0:X2}", out_buf(i))
        Next
        Return sb.ToString
    End Function

    Public Function Decrypt(ByVal message As String, Optional ByVal bNothing As Boolean = True) As String
        Dim des As New DESCryptoServiceProvider
        Dim out As MemoryStream
        Dim buf As String
        Dim char_buf() As Char
        Dim in_buf(), out_buf() As Byte
        Dim i, in_len As Integer

        ' put the ciphertext into the input byte array
        in_len = message.Length
        If in_len = 0 Then
            Return ""
        End If
        ReDim in_buf(CInt(in_len / 2) - 1)
        ReDim out_buf(CInt(in_len / 2) - 1)

        Try
            char_buf = message.ToCharArray
            For i = 0 To in_len - 1 Step 2
                in_buf(i \ 2) = CByte("&h" & char_buf(i) & char_buf(i + 1))
            Next
        Catch ex As Exception
            ' This is a surprisingly common error, when you try to "cut-n-paste"
            ' the cipher text string into the designer and end up with an
            ' embedded CR or space.
            Return ""
        End Try

        ' prepare the output stream
        out = New MemoryStream(out_buf, True)

        ' create an cryptographic output stream
        Try
            Dim crStream As New CryptoStream(out, des.CreateDecryptor(TheKey, Vector), CryptoStreamMode.Write)
            crStream.Write(in_buf, 0, in_buf.Length)
            crStream.Close()
        Catch ex As System.Security.Cryptography.CryptographicException
            ' if decryption fails, then just return an empty string
            Return ""
        End Try

        ' get the cleartext out of the buffer
        char_buf = Encoding.ASCII.GetChars(out_buf, 0, CInt(in_len / 2))
        buf = char_buf

        If bNothing Then
            Dim bufpom As String = String.Empty

            For j As Integer = 0 To buf.Length - 1
                If buf.Chars(j) <> Nothing Then
                    bufpom &= buf.Chars(j)
                End If
            Next j
            buf = bufpom
        End If

        Return buf
    End Function
End Class
