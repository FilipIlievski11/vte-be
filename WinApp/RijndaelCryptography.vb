Imports System.IO
Imports System.Security.Cryptography
Imports System.Text


''' <summary>
''' Simple Rijndael implementation
''' </summary>
Friend Class RijndaelCryptography
  Private myRijndael As RijndaelManaged
  Private textConverter As ASCIIEncoding
  Private fromEncrypt As Byte()
  Private m_encrypted As Byte()
  Private toEncrypt As Byte()
  Private _key As Byte()
  Private _IV As Byte()

  Friend Property Key() As Byte()
    Get
      Return _key
    End Get
    Set(ByVal value As Byte())
      _key = value
    End Set
  End Property
  Friend Property IV() As Byte()
    Get
      Return _IV
    End Get
    Set(ByVal value As Byte())
      _IV = value
    End Set
  End Property
  Friend ReadOnly Property Encrypted() As Byte()
    Get
      Return m_encrypted
    End Get
  End Property

  Friend Sub New()
    myRijndael = New RijndaelManaged()
    myRijndael.Mode = CipherMode.CBC
    textConverter = New ASCIIEncoding()
  End Sub

  Friend Overridable Sub GenKey()
    'Create a new key and initialization vector.
    myRijndael.GenerateKey()
    myRijndael.GenerateIV()

    'Get the key and IV.
    _key = myRijndael.Key
    _IV = myRijndael.IV
  End Sub

  Friend Sub Encrypt(ByVal TxtToEncrypt As String)
    'Get an encryptor.
    Dim encryptor As ICryptoTransform = myRijndael.CreateEncryptor(_key, _IV)

    'Encrypt the data.
    Dim msEncrypt As New MemoryStream()
    Dim csEncrypt As New CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)

    'Convert the data to a byte array.
    toEncrypt = textConverter.GetBytes(TxtToEncrypt)

    'Write all data to the crypto stream and flush it.
    csEncrypt.Write(toEncrypt, 0, toEncrypt.Length)
    csEncrypt.FlushFinalBlock()

    'Get encrypted array of bytes.
    m_encrypted = msEncrypt.ToArray()
  End Sub

  Friend Function Decrypt(ByVal crypted As Byte()) As String
    'Get a decryptor.
    Dim decryptor As ICryptoTransform = myRijndael.CreateDecryptor(_key, _IV)

    'Decrypting the encrypted byte array.
    Dim msDecrypt As New MemoryStream(crypted)
    Dim csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)

    fromEncrypt = New Byte(crypted.Length - 1) {}

    'Read the data out of the crypto stream.
    csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length)

    'Convert the byte array into a string.
    Return textConverter.GetString(fromEncrypt)


  End Function
End Class