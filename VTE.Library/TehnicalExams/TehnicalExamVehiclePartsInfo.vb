
<Serializable()> _
Public Class TehnicalExamVehiclePartsInfo
  Inherits ReadOnlyBase(Of TehnicalExamVehiclePartsInfo)

  Private _id As Integer
  Public ReadOnly Property Id() As Integer
    Get
      Return _id
    End Get
  End Property
  Private _idCategoryVehicleParts As Integer
  Public ReadOnly Property IdCategoryVehicleParts() As Integer
    Get
      Return _idCategoryVehicleParts
    End Get
  End Property
  Private _code As String
  Public ReadOnly Property Code() As String
    Get
      Return _code
    End Get
  End Property
  Private _description As String
  Public ReadOnly Property Description() As String
    Get
      Return _description
    End Get
  End Property
  Public ReadOnly Property CodeAndDescription() As String
    Get
      Return _code & "-" & _description
    End Get
  End Property
    'Private Function FullDescriptionFunc(ByVal info As TehnicalExamVehiclePartsInfo, ByVal pomIn As String) As String
    '    Dim pom As String = pomIn
    '    If info.IdCategoryVehicleParts > 0 Then
    '        Dim infoDete As TehnicalExamVehiclePartsInfo = _
    '        TehnicalExamVehiclePartsList.GetTehnicalExamVehiclePartsList. _
    '        GetTehnicalExamVehiclePartsListById(info.IdCategoryVehicleParts)
    '        pom = infoDete.FullDescription & pom
    '    Else
    '        pom = _description
    '    End If
    '    Return pom
    'End Function
    ''Private _fullDescription As String
    'Public ReadOnly Property FullDescription()
    '    Get
    '        Return FullDescriptionFunc(Me, _description) '_fullDescription
    '    End Get
    'End Property


    Private _picturePath As String
    Public ReadOnly Property PicturePath() As String
        Get
            Return _picturePath
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
        _idCategoryVehicleParts = dr.GetInt32("IdCategoryVehicleParts")
        _code = dr.GetString("Code")
        _description = dr.GetString("Description")
        _picturePath = dr.GetString("PicturePath")
        ' _fullDescription = FullDescriptionFunc(Me)
    End Sub

End Class