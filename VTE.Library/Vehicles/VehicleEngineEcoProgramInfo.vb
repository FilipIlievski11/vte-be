
<Serializable()> _
Public Class VehicleEngineEcoProgramInfo
  Inherits ReadOnlyBase(Of VehicleEngineEcoProgramInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _code As String
  Public ReadOnly Property Code() As String
    Get
      Return _code
    End Get
  End Property
  Private _ecoProgram As String
  Public ReadOnly Property EcoProgram() As String
    Get
      Return _ecoProgram
    End Get
  End Property
  Public ReadOnly Property EcoProgramAndTechincalDescription() As String
    Get
      Return _code & ", " & _ecoProgram & "-" & _techincalDescription
    End Get
  End Property
  Private _techincalDescription As String
  Public ReadOnly Property TechincalDescription() As String
    Get
      Return _techincalDescription
    End Get
  End Property
  Private _PercentForPayment As Decimal
  Public ReadOnly Property PercentForPayment() As Decimal
    Get
      Return _PercentForPayment
    End Get
  End Property

  Protected Overrides Function GetIdValue() As Object
    Return _id
  End Function

  Public Overrides Function ToString() As String
    Return _id
  End Function

  Friend Sub New(ByVal dr As SafeDataReader)
    _id = dr.GetInt32("Id")
    _code = dr.GetString("Code")
    _ecoprogram = dr.GetString("EcoProgram")
    _techincalDescription = dr.GetString("TechincalDescription")
    _PercentForPayment = dr.GetValue("PercentForPayment")
  End Sub

End Class