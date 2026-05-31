
<Serializable()> _
Public Class DocumentsTehnicalExamsReport
 Inherits Csla.BusinessBase(Of DocumentsTehnicalExamsReport)


#Region " Stored Procedures Names "
 Private Const spGetByID As String = "GetDocumentsTehnicalExamsReportByID"
 Private Const spGetAll As String = "GetDocumentsTehnicalExamsReports"
 Private Const spUpdate As String = "updateDocumentsTehnicalExamsReport"
 Private Const spAdd As String = "addDocumentsTehnicalExamsReport"
 Private Const spDelete As String = "deleteDocumentsTehnicalExamsReport"
 Private Const spGetChildDetails As String = "getDocumentsTehnicalExamsReportsDetailByIdTehnicalExamsReports"
 Private Const spGetChildVisualErrors As String = "getDocumentsTehnicalExamsReportsVisualErrorByIdDocumentsTehnicalExamsReports"
 Private Const spGetChildMeasuredValues As String = "getDocumentsTehnicalExamsReportsMeasuredValueByIdDocumentsTehnicalExamsReports"
 Private Const SpGetNumberForReport As String = "getPaymentDocumentsNumberByIdStationForTehExamReport"

#End Region 'Stored Procedures Names

#Region " Business Properties and Methods "
 'register properties
 Private Shared IdProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Long)("Id"))
 Private Shared IdCustomerVehicleRelationProperty As PropertyInfo(Of Long) = RegisterProperty(Of Long)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Long)("IdCustomerVehicleRelation"))
 Private Shared IdTypeOfTehnicalExamProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Integer)("IdTypeOfTehnicalExam"))
 Private Shared RegNumberProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of String)("RegNumber"))
 Private Shared MadeDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of SmartDate)("MadeDate", "MadeDate", New SmartDate(DateTime.Today, True)))
 Private Shared ValidTillDateProperty As PropertyInfo(Of SmartDate) = RegisterProperty(Of SmartDate)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of SmartDate)("ValidTillDate", "ValidTillDate", New SmartDate(DateTime.Today, True)))
 Private Shared IdOrganizationForTehnicalExamProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Integer)("IdOrganizationForTehnicalExam"))
 Private Shared IdFirsControlerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Integer)("IdFirsControler"))
 Private Shared IdSecondControlerProperty As PropertyInfo(Of Integer) = RegisterProperty(Of Integer)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Integer)("IdSecondControler"))
 Private Shared VehicleIsRightProperty As PropertyInfo(Of Boolean) = RegisterProperty(Of Boolean)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Boolean)("VehicleIsRight", "VehicleIsRight", True))
 Private Shared ExplanationNoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of String)("ExplanationNote"))
 Private Shared DriversWarningProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of String)("DriversWarning"))
 Private Shared NoteProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of String)("Note"))
 Private Shared axis1LeftProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis1Left"))
 Private Shared axis1RightProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis1Right"))
 Private Shared axis1GjProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis1Gj"))
 Private Shared axis1LeftPjProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis1LeftPj"))
 Private Shared axis1PNProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis1PN"))

 Private Shared axis2LeftProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis2Left"))
 Private Shared axis2RightProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis2Right"))
 Private Shared axis2GjProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis2Gj"))
 Private Shared axis2LeftPjProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis2LeftPj"))
 Private Shared axis2PNProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis2PN"))

 Private Shared axis3LeftProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis3Left"))
 Private Shared axis3RightProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis3Right"))
 Private Shared axis3GjProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis3Gj"))
 Private Shared axis3LeftPjProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis3LeftPj"))
 Private Shared axis3PNProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis3PN"))

 Private Shared axis4LeftProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis4Left"))
 Private Shared axis4RightProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis4Right"))
 Private Shared axis4GjProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis4Gj"))
 Private Shared axis4LeftPjProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis4LeftPj"))
 Private Shared axis4PNProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axis4PN"))

 Private Shared axisParkingLeftProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axisParkingLeft"))
 Private Shared axisParkingRightProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axisParkingRight"))
 Private Shared axisParkingGjProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axisParkingGj"))
 Private Shared axisParkingLeftPjProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axisParkingLeftPj"))
 Private Shared axisParkingPNProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("axisParkingPN"))
 Private Shared waightProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("waight"))
 Private Shared effectOfWorkingBreakEmptyProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("effectOfWorkingBreakEmpty"))
 Private Shared effectOfWorkingBreakFullProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("effectOfWorkingBreakFull"))
 Private Shared effectOfSecondaryBreakProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("effectOfSecondaryBreak"))
 Private Shared effectOfParkingBreakProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("effectOfParkingBreak"))
 Private Shared speedOfTurnsProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("speedOfTurns"))
 Private Shared COProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("CO"))
 Private Shared numEngineTurnsProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("numEngineTurns"))
 Private Shared cOPlusTurnsProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("cOPlusTurns"))
 Private Shared lambdaProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("lambda"))
 Private Shared pinpointsProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("pinpoints"))
 Private Shared noiseProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("noise"))
 Private Shared tempOfEngineOilProperty As PropertyInfo(Of Decimal) = RegisterProperty(Of Decimal)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of Decimal)("tempOfEngineOil"))
 Private Shared technicalChangesProperty As PropertyInfo(Of String) = RegisterProperty(Of String)(GetType(DocumentsTehnicalExamsReport), New PropertyInfo(Of String)("technicalChanges"))

 Private _lastChanged(7) As Byte

 Private Shared DetailsProperty As PropertyInfo(Of DocumentsTehnicalExamsReportsDetails) = _
   RegisterProperty(Of DocumentsTehnicalExamsReportsDetails)(GetType(DocumentsTehnicalExamsReport), _
   New PropertyInfo(Of DocumentsTehnicalExamsReportsDetails)("Details"))

 Private Shared VisualErrorsProperty As PropertyInfo(Of DocumentsTehnicalExamsReportsVisualErrors) = _
RegisterProperty(Of DocumentsTehnicalExamsReportsVisualErrors)(GetType(DocumentsTehnicalExamsReport), _
New PropertyInfo(Of DocumentsTehnicalExamsReportsVisualErrors)("VisualErrors"))

 <System.ComponentModel.DataObjectField(True, True)> _
Public ReadOnly Property Id() As Long
  Get
   Return GetProperty(Of Long)(IdProperty)
  End Get
 End Property
 Public ReadOnly Property Details() As DocumentsTehnicalExamsReportsDetails
  Get
   If Not FieldManager.FieldExists(DetailsProperty) Then
    SetProperty(Of DocumentsTehnicalExamsReportsDetails) _
    (DetailsProperty, DocumentsTehnicalExamsReportsDetails.NewDocumentsTehnicalExamsReportsDetails)
   End If
   Return GetProperty(Of DocumentsTehnicalExamsReportsDetails)(DetailsProperty)
  End Get
 End Property
 Public ReadOnly Property VisualErrors() As DocumentsTehnicalExamsReportsVisualErrors
  Get
   If Not FieldManager.FieldExists(VisualErrorsProperty) Then
    SetProperty(Of DocumentsTehnicalExamsReportsVisualErrors) _
    (VisualErrorsProperty, DocumentsTehnicalExamsReportsVisualErrors.NewDocumentsTehnicalExamsReportsVisualErrors)
   End If
   Return GetProperty(Of DocumentsTehnicalExamsReportsVisualErrors)(VisualErrorsProperty)
  End Get
 End Property

 Public Property IdCustomerVehicleRelation() As Long
  Get
   Return GetProperty(Of Long)(IdCustomerVehicleRelationProperty)
  End Get
  Set(ByVal value As Long)
   SetProperty(Of Long)(IdCustomerVehicleRelationProperty, value)
  End Set
 End Property

 Public Property IdTypeOfTehnicalExam() As Integer
  Get
   Return GetProperty(Of Integer)(IdTypeOfTehnicalExamProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdTypeOfTehnicalExamProperty, value)
  End Set
 End Property

 Public Property RegNumber() As String
  Get
   Return GetProperty(Of String)(RegNumberProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(RegNumberProperty, value)
  End Set
 End Property

 Public Property MadeDate() As Date
  Get
   Return GetProperty(Of SmartDate, Date)(MadeDateProperty)
  End Get
  Set(ByVal value As Date)
   SetProperty(Of SmartDate, Date)(MadeDateProperty, value)
   Dim examType As TehnicalExamsTypesInfo = _
   CType(Csla.ApplicationContext.LocalContext("objTehnicalExamsTypesList"),  _
   TehnicalExamsTypesList).GetInfoById(ReadProperty(Of Integer)(IdTypeOfTehnicalExamProperty))
   If examType IsNot Nothing Then
    SetProperty(Of SmartDate, Date)(ValidTillDateProperty, value.AddDays(examType.ValidNumOfDays))
   Else
    SetProperty(Of SmartDate, Date)(ValidTillDateProperty, value.AddYears(1))
   End If
  End Set
 End Property
 Public Property ValidTillDate() As Date
  Get
   Return GetProperty(Of SmartDate, Date)(ValidTillDateProperty)
  End Get
  Set(ByVal value As Date)
   SetProperty(Of SmartDate, Date)(ValidTillDateProperty, value)
  End Set
 End Property

 Public Property IdOrganizationForTehnicalExam() As Integer
  Get
   Return GetProperty(Of Integer)(IdOrganizationForTehnicalExamProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdOrganizationForTehnicalExamProperty, value)
  End Set
 End Property
 Public Property IdFirsControler() As Integer
  Get
   Return GetProperty(Of Integer)(IdFirsControlerProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdFirsControlerProperty, value)
  End Set
 End Property
 Public Property IdSecondControler() As Integer
  Get
   Return GetProperty(Of Integer)(IdSecondControlerProperty)
  End Get
  Set(ByVal value As Integer)
   SetProperty(Of Integer)(IdSecondControlerProperty, value)
  End Set
 End Property
 Public Property VehicleIsRight() As Boolean
  Get
   Return GetProperty(Of Boolean)(VehicleIsRightProperty)
  End Get
  Set(ByVal value As Boolean)
   SetProperty(Of Boolean)(VehicleIsRightProperty, value)
  End Set
 End Property
 Public Property ExplanationNote() As String
  Get
   Return GetProperty(Of String)(ExplanationNoteProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(ExplanationNoteProperty, value)
  End Set
 End Property
 Public Property DriversWarning() As String
  Get
   Return GetProperty(Of String)(DriversWarningProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(DriversWarningProperty, value)
  End Set
 End Property
 Public Property Note() As String
  Get
   Return GetProperty(Of String)(NoteProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(NoteProperty, value)
  End Set
 End Property

 Public Property Axis1Left() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis1LeftProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis1LeftProperty, value)
  End Set
 End Property
 Public Property Axis1Right() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis1RightProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis1RightProperty, value)
  End Set
 End Property
 Public Property Axis1Gj() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis1GjProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis1GjProperty, value)
  End Set
 End Property
 Public Property Axis1LeftPj() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis1LeftPjProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis1LeftPjProperty, value)
  End Set
 End Property
 Public Property Axis1PN() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis1PNProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis1PNProperty, value)
  End Set
 End Property

 Public Property Axis2Left() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis2LeftProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis2LeftProperty, value)
  End Set
 End Property
 Public Property Axis2Right() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis2RightProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis2RightProperty, value)
  End Set
 End Property
 Public Property Axis2Gj() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis2GjProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis2GjProperty, value)
  End Set
 End Property
 Public Property Axis2LeftPj() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis2LeftPjProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis2LeftPjProperty, value)
  End Set
 End Property
 Public Property Axis2PN() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis2PNProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis2PNProperty, value)
  End Set
 End Property

 Public Property Axis3Left() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis3LeftProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis3LeftProperty, value)
  End Set
 End Property
 Public Property Axis3Right() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis3RightProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis3RightProperty, value)
  End Set
 End Property
 Public Property Axis3Gj() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis3GjProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis3GjProperty, value)
  End Set
 End Property
 Public Property Axis3LeftPj() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis3LeftPjProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis3LeftPjProperty, value)
  End Set
 End Property
 Public Property Axis3PN() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis3PNProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis3PNProperty, value)
  End Set
 End Property

 Public Property Axis4Left() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis4LeftProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis4LeftProperty, value)
  End Set
 End Property
 Public Property Axis4Right() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis4RightProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis4RightProperty, value)
  End Set
 End Property
 Public Property Axis4Gj() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis4GjProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis4GjProperty, value)
  End Set
 End Property
 Public Property Axis4LeftPj() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis4LeftPjProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis4LeftPjProperty, value)
  End Set
 End Property
 Public Property Axis4PN() As Decimal
  Get
   Return GetProperty(Of Decimal)(axis4PNProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axis4PNProperty, value)
  End Set
 End Property

 Public Property AxisParkingLeft() As Decimal
  Get
   Return GetProperty(Of Decimal)(axisParkingLeftProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axisParkingLeftProperty, value)
  End Set
 End Property
 Public Property AxisParkingRight() As Decimal
  Get
   Return GetProperty(Of Decimal)(axisParkingRightProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axisParkingRightProperty, value)
  End Set
 End Property
 Public Property AxisParkingGj() As Decimal
  Get
   Return GetProperty(Of Decimal)(axisParkingGjProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axisParkingGjProperty, value)
  End Set
 End Property
 Public Property AxisParkingLeftPj() As Decimal
  Get
   Return GetProperty(Of Decimal)(axisParkingLeftPjProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axisParkingLeftPjProperty, value)
  End Set
 End Property
 Public Property AxisParkingPN() As Decimal
  Get
   Return GetProperty(Of Decimal)(axisParkingPNProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(axisParkingPNProperty, value)
  End Set
 End Property
 Public Property Waight() As Decimal
  Get
   Return GetProperty(Of Decimal)(waightProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(waightProperty, value)
  End Set
 End Property
 Public Property EffectOfWorkingBreakEmpty() As Decimal
  Get
   Return GetProperty(Of Decimal)(effectOfWorkingBreakEmptyProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(effectOfWorkingBreakEmptyProperty, value)
  End Set
 End Property
 Public Property EffectOfWorkingBreakFull() As Decimal
  Get
   Return GetProperty(Of Decimal)(effectOfWorkingBreakFullProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(effectOfWorkingBreakFullProperty, value)
  End Set
 End Property
 Public Property EffectOfSecondaryBreak() As Decimal
  Get
   Return GetProperty(Of Decimal)(effectOfSecondaryBreakProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(effectOfSecondaryBreakProperty, value)
  End Set
 End Property
 Public Property EffectOfParkingBreak() As Decimal
  Get
   Return GetProperty(Of Decimal)(effectOfParkingBreakProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(effectOfParkingBreakProperty, value)
  End Set
 End Property
 Public Property SpeedOfTurns() As Decimal
  Get
   Return GetProperty(Of Decimal)(speedOfTurnsProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(speedOfTurnsProperty, value)
  End Set
 End Property
 Public Property CO() As Decimal
  Get
   Return GetProperty(Of Decimal)(COProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(COProperty, value)
  End Set
 End Property
 Public Property NumEngineTurns() As Decimal
  Get
   Return GetProperty(Of Decimal)(numEngineTurnsProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(numEngineTurnsProperty, value)
  End Set
 End Property
 Public Property COPlusTurns() As Decimal
  Get
   Return GetProperty(Of Decimal)(cOPlusTurnsProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(cOPlusTurnsProperty, value)
  End Set
 End Property
 Public Property Lambda() As Decimal
  Get
   Return GetProperty(Of Decimal)(lambdaProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(lambdaProperty, value)
  End Set
 End Property

 Public Property Pinpoints() As Decimal
  Get
   Return GetProperty(Of Decimal)(pinpointsProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(pinpointsProperty, value)
  End Set
 End Property
 Public Property Noise() As Decimal
  Get
   Return GetProperty(Of Decimal)(noiseProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(noiseProperty, value)
  End Set
 End Property
 Public Property TempOfEngineOil() As Decimal
  Get
   Return GetProperty(Of Decimal)(tempOfEngineOilProperty)
  End Get
  Set(ByVal value As Decimal)
   SetProperty(Of Decimal)(tempOfEngineOilProperty, value)
  End Set
 End Property

 Public Property TechnicalChanges() As String
  Get
   Return GetProperty(Of String)(technicalChangesProperty)
  End Get
  Set(ByVal value As String)
   SetProperty(Of String)(technicalChangesProperty, value)
  End Set
 End Property

 Public Overrides Function ToString() As String
  Return Id.ToString
 End Function

 Private Sub DocumentsTehnicalExamsReport_ChildChanged(ByVal sender As Object, ByVal e As Csla.Core.ChildChangedEventArgs) Handles Me.ChildChanged
  Try
   If TypeOf (e.ChildObject) Is DocumentsTehnicalExamsReportsDetails Then
    Dim boolIsOK As Boolean = True
    If Me.Details.Count > 0 Then
     For Each child As DocumentsTehnicalExamsReportsDetail In Me.Details
      If child.IdStatus <> 1 Then
       boolIsOK = False
       Exit For
      End If
     Next
     Me.VehicleIsRight = boolIsOK
    Else
     Me.VehicleIsRight = True
    End If
   End If
        Catch ex As Exception
  End Try
 End Sub

#End Region 'Business Properties and Methods

#Region " Authorization Rules "

 Protected Overrides Sub AddAuthorizationRules()
  Dim roleName As String = CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).GetCurrrentRoleName
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdVehicle") Then
   AuthorizationRules.AllowWrite("IdVehicle", roleName)
  Else
   AuthorizationRules.DenyWrite("IdVehicle", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdVehicle")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdCustomer") Then
   AuthorizationRules.AllowWrite("IdCustomer", roleName)
  Else
   AuthorizationRules.DenyWrite("IdCustomer", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdCustomer")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("MadeDate") Then
   AuthorizationRules.AllowWrite("MadeDate", roleName)
  Else
   AuthorizationRules.DenyWrite("MadeDate", roleName)
  End If
  'AuthorizationRules.AllowWrite("MadeDate")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("ValidTillDate") Then
   AuthorizationRules.AllowWrite("ValidTillDate", roleName)
  Else
   AuthorizationRules.DenyWrite("ValidTillDate", roleName)
  End If
  'AuthorizationRules.AllowWrite("ValidTillDate")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdOrganizationForTehnicalExam") Then
   AuthorizationRules.AllowWrite("IdOrganizationForTehnicalExam", roleName)
  Else
   AuthorizationRules.DenyWrite("IdOrganizationForTehnicalExam", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdOrganizationForTehnicalExam")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdFirsControler") Then
   AuthorizationRules.AllowWrite("IdFirsControler", roleName)
  Else
   AuthorizationRules.DenyWrite("IdFirsControler", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdFirsControler")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("IdSecondControler") Then
   AuthorizationRules.AllowWrite("IdSecondControler", roleName)
  Else
   AuthorizationRules.DenyWrite("IdSecondControler", roleName)
  End If
  'AuthorizationRules.AllowWrite("IdSecondControler")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("VehicleIsRight") Then
   AuthorizationRules.AllowWrite("VehicleIsRight", roleName)
  Else
   AuthorizationRules.DenyWrite("VehicleIsRight", roleName)
  End If
  'AuthorizationRules.AllowWrite("VehicleIsRight")
  If CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInFieldsPrivilegesCanWrite("Active") Then
   AuthorizationRules.AllowWrite("Active", roleName)
  Else
   AuthorizationRules.DenyWrite("Active", roleName)
  End If
  'AuthorizationRules.AllowWrite("Active")
 End Sub



 Public Shared Function CanGetObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("DocumentsTehnicalExamsReport")
 End Function

 Public Shared Function CanAddObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("DocumentsTehnicalExamsReport")
 End Function

 Public Shared Function CanEditObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("DocumentsTehnicalExamsReport")
 End Function

 Public Shared Function CanDeleteObject() As Boolean
  Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("DocumentsTehnicalExamsReport")
 End Function

#End Region ' Authorization Rules

#Region " Validation Rules "
 Protected Overrides Sub AddBusinessRules()
  ' MadeDateProperty rules
  'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, MadeDateProperty)
  '' ValidTillDateProperty rules
  'ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, ValidTillDateProperty)

  ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                          New Csla.Validation.IntegerMinValueRuleArgs(IdCustomerVehicleRelationProperty, 1))
  ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                          New Csla.Validation.IntegerMinValueRuleArgs(IdOrganizationForTehnicalExamProperty, 1))
  ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
                          New Csla.Validation.IntegerMinValueRuleArgs(IdTypeOfTehnicalExamProperty, 1))
  'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
  '                          New Csla.Validation.IntegerMinValueRuleArgs(IdFirsControlerProperty, 1))
  'ValidationRules.AddRule(AddressOf MyCommonRules.ForeignIdSelect, _
  '                          New Csla.Validation.IntegerMinValueRuleArgs(IdSecondControlerProperty, 1))

  ValidationRules.AddRule(Of DocumentsTehnicalExamsReport)(AddressOf MadeDateGTValidTillDate, MadeDateProperty)
  ValidationRules.AddRule(Of DocumentsTehnicalExamsReport)(AddressOf MadeDateGTValidTillDate, ValidTillDateProperty)
  ValidationRules.AddDependentProperty(MadeDateProperty, ValidTillDateProperty, True)
  'ValidationRules.AddRule(Of DocumentsTehnicalExamsReport)(AddressOf RazlicniControlori, IdFirsControlerProperty)
  'ValidationRules.AddRule(Of DocumentsTehnicalExamsReport)(AddressOf RazlicniControlori, IdSecondControlerProperty)
  ' ValidationRules.AddDependentProperty(IdFirsControlerProperty, IdSecondControlerProperty, True)

 End Sub

 Private Shared Function MadeDateGTValidTillDate(Of T As DocumentsTehnicalExamsReport)(ByVal target As T, _
 ByVal e As Csla.Validation.RuleArgs) As Boolean

  If target.MadeDate > target.ValidTillDate Then
   e.Description = "Датумот на прегледот мора да е поголем од датумот на истекување"
   Return False
  Else
   Return True
  End If

 End Function
 Private Shared Function RazlicniControlori(Of T As DocumentsTehnicalExamsReport)(ByVal target As T, _
ByVal e As Csla.Validation.RuleArgs) As Boolean

  If target.IdFirsControler = target.IdSecondControler Then
   e.Description = "Мора да се потпишат 2 контролори"
   Return False
  Else
   Return True
  End If

 End Function
#End Region ' Validation Rules

#Region " Factory Methods "

 Private Sub New()
  ' require use of factory method 
 End Sub

 Public Shared Function NewDocumentsTehnicalExamsReport() As DocumentsTehnicalExamsReport
  If Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a DocumentsTehnicalExamsReport")
  End If
  Return DataPortal.Create(Of DocumentsTehnicalExamsReport)()
 End Function

 Public Shared Function GetDocumentsTehnicalExamsReport(ByVal id As Long) As DocumentsTehnicalExamsReport
  If Not CanGetObject() Then
   Throw New System.Security.SecurityException("User not authorized to view a DocumentsTehnicalExamsReport")
  End If
  Return DataPortal.Fetch(New SingleCriteria(Of DocumentsTehnicalExamsReport, Integer)(id))
 End Function

 Public Shared Sub DeleteDocumentsTehnicalExamsReport(ByVal id As Long)
  If Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a DocumentsTehnicalExamsReport")
  End If
  DataPortal.Delete(New SingleCriteria(Of DocumentsTehnicalExamsReport, Integer)(id))
 End Sub

 Public Overrides Function Save() As DocumentsTehnicalExamsReport
  If IsDeleted AndAlso Not CanDeleteObject() Then
   Throw New System.Security.SecurityException("User not authorized to remove a DocumentsTehnicalExamsReport")
  ElseIf IsNew AndAlso Not CanAddObject() Then
   Throw New System.Security.SecurityException("User not authorized to add a DocumentsTehnicalExamsReport")
  ElseIf Not CanEditObject() Then
   Throw New System.Security.SecurityException("User not authorized to update a DocumentsTehnicalExamsReport")
  End If

  Dim result As DocumentsTehnicalExamsReport = MyBase.Save

  OnDocumentsTehnicalExamsReportSaved(Me, New Csla.Core.SavedEventArgs(result))

  Return result
 End Function



#End Region ' Factory Methods

#Region " Child Factory Methods "

 Friend Shared Function NewDocumentsTehnicalExamsReportChild() As DocumentsTehnicalExamsReport
  Return DataPortal.CreateChild(Of DocumentsTehnicalExamsReport)()
 End Function

 Friend Shared Function GetDocumentsTehnicalExamsReport(ByVal dr As SafeDataReader) As DocumentsTehnicalExamsReport
  Return DataPortal.FetchChild(Of DocumentsTehnicalExamsReport)(dr)
 End Function

#End Region 'Child Factory Methods

#Region " Data Access "

#Region " Data Access - Create "

 <RunLocal()> _
 Private Overloads Sub DataPortal_Create()
  'LoadProperty(Of Integer)(IdFirsControlerProperty, Csla.ApplicationContext.LocalContext("EmployeeID"))
  'LoadProperty(Of Integer)(IdTypeOfTehnicalExamProperty, 1)
  Me.IdOrganizationForTehnicalExam = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id
  'Me.MadeDate = Now.Date
  'Me.ValidTillDate = Now.AddYears(1).Date
  'LoadProperty(Of String)(RegNumberProperty, DocumentsTehnicalExamsReport.GetRegNumber())
  ValidationRules.CheckRules()
 End Sub

#End Region ' Data Access - Create

#Region " Data Access - Fetch "

 Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of DocumentsTehnicalExamsReport, Integer))
  Database.LogInfo("DocumentsTehnicalExamsReport.DataPortal_Fetch", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand()
     cm.CommandType = CommandType.StoredProcedure
     cm.CommandText = spGetByID
     cm.Parameters.AddWithValue("@Id", criteria.Value)
     Using dr As New SafeDataReader(cm.ExecuteReader)
      dr.Read()

      LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
      LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
      LoadProperty(Of Integer)(IdTypeOfTehnicalExamProperty, dr.GetInt32("IdTypeOfTehnicalExam"))
      LoadProperty(Of String)(RegNumberProperty, dr.GetString("RegNumber"))
      LoadProperty(Of SmartDate, Date?)(MadeDateProperty, dr.GetSmartDate("MadeDate", True))
      LoadProperty(Of SmartDate, Date?)(ValidTillDateProperty, dr.GetSmartDate("ValidTillDate", True))
      LoadProperty(Of Integer)(IdOrganizationForTehnicalExamProperty, dr.GetInt32("IdOrganizationForTehnicalExam"))
      LoadProperty(Of Integer)(IdFirsControlerProperty, dr.GetInt32("IdFirsControler"))
      LoadProperty(Of Integer)(IdSecondControlerProperty, dr.GetInt32("IdSecondControler"))
      LoadProperty(Of Boolean)(VehicleIsRightProperty, dr.GetBoolean("VehicleIsRight"))
      LoadProperty(Of String)(ExplanationNoteProperty, dr.GetString("ExplanationNote"))
      LoadProperty(Of String)(DriversWarningProperty, dr.GetString("DriversWarning"))
      LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
      LoadProperty(Of Decimal)(axis1LeftProperty, dr.GetValue("axis1Left"))
      LoadProperty(Of Decimal)(axis1RightProperty, dr.GetValue("axis1Right"))
      LoadProperty(Of Decimal)(axis1GjProperty, dr.GetValue("axis1Gj"))
      LoadProperty(Of Decimal)(axis1LeftPjProperty, dr.GetValue("axis1LeftPj"))
      LoadProperty(Of Decimal)(axis1PNProperty, dr.GetValue("axis1PN"))
      LoadProperty(Of Decimal)(axis2LeftProperty, dr.GetValue("axis2Left"))
      LoadProperty(Of Decimal)(axis2RightProperty, dr.GetValue("axis2Right"))
      LoadProperty(Of Decimal)(axis2GjProperty, dr.GetValue("axis2Gj"))
      LoadProperty(Of Decimal)(axis2LeftPjProperty, dr.GetValue("axis2LeftPj"))
      LoadProperty(Of Decimal)(axis2PNProperty, dr.GetValue("axis2PN"))
      LoadProperty(Of Decimal)(axis3LeftProperty, dr.GetValue("axis3Left"))
      LoadProperty(Of Decimal)(axis3RightProperty, dr.GetValue("axis3Right"))
      LoadProperty(Of Decimal)(axis3GjProperty, dr.GetValue("axis3Gj"))
      LoadProperty(Of Decimal)(axis3LeftPjProperty, dr.GetValue("axis3LeftPj"))
      LoadProperty(Of Decimal)(axis3PNProperty, dr.GetValue("axis3PN"))
      LoadProperty(Of Decimal)(axis4LeftProperty, dr.GetValue("axis4Left"))
      LoadProperty(Of Decimal)(axis4RightProperty, dr.GetValue("axis4Right"))
      LoadProperty(Of Decimal)(axis4GjProperty, dr.GetValue("axis4Gj"))
      LoadProperty(Of Decimal)(axis4LeftPjProperty, dr.GetValue("axis4LeftPj"))
      LoadProperty(Of Decimal)(axis4PNProperty, dr.GetValue("axis4PN"))

      LoadProperty(Of Decimal)(axisParkingLeftProperty, dr.GetValue("axisParkingLeft"))
      LoadProperty(Of Decimal)(axisParkingRightProperty, dr.GetValue("axisParkingRight"))
      LoadProperty(Of Decimal)(axisParkingGjProperty, dr.GetValue("axisParkingGj"))
      LoadProperty(Of Decimal)(axisParkingLeftPjProperty, dr.GetValue("axisParkingLeftPj"))
      LoadProperty(Of Decimal)(axisParkingPNProperty, dr.GetValue("axisParkingPN"))
      LoadProperty(Of Decimal)(waightProperty, dr.GetValue("waight"))
      LoadProperty(Of Decimal)(effectOfWorkingBreakEmptyProperty, dr.GetValue("effectOfWorkingBreakEmpty"))
      LoadProperty(Of Decimal)(effectOfWorkingBreakFullProperty, dr.GetValue("effectOfWorkingBreakFull"))
      LoadProperty(Of Decimal)(effectOfSecondaryBreakProperty, dr.GetValue("effectOfSecondaryBreak"))
      LoadProperty(Of Decimal)(effectOfParkingBreakProperty, dr.GetValue("effectOfParkingBreak"))
      LoadProperty(Of Decimal)(speedOfTurnsProperty, dr.GetValue("speedOfTurns"))
      LoadProperty(Of Decimal)(COProperty, dr.GetValue("CO"))
      LoadProperty(Of Decimal)(numEngineTurnsProperty, dr.GetValue("numEngineTurns"))
      LoadProperty(Of Decimal)(cOPlusTurnsProperty, dr.GetValue("cOPlusTurns"))
      LoadProperty(Of Decimal)(lambdaProperty, dr.GetValue("lambda"))
      LoadProperty(Of Decimal)(pinpointsProperty, dr.GetValue("pinpoints"))
      LoadProperty(Of Decimal)(noiseProperty, dr.GetValue("noise"))
      LoadProperty(Of Decimal)(tempOfEngineOilProperty, dr.GetValue("tempOfEngineOil"))

      LoadProperty(Of String)(technicalChangesProperty, dr.GetString("technicalChanges"))
      dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
     End Using
    End Using
    Using cm1 As SqlCommand = cn.CreateCommand
     cm1.CommandType = CommandType.StoredProcedure
     cm1.CommandText = spGetChildDetails
     cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))

     Using drc As New SafeDataReader(cm1.ExecuteReader)
      LoadProperty(Of DocumentsTehnicalExamsReportsDetails) _
      (DetailsProperty, DocumentsTehnicalExamsReportsDetails.GetDocumentsTehnicalExamsReportsDetails(drc))
     End Using
    End Using
    Using cm1 As SqlCommand = cn.CreateCommand
     cm1.CommandType = CommandType.StoredProcedure
     cm1.CommandText = spGetChildVisualErrors
     cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))

     Using drc As New SafeDataReader(cm1.ExecuteReader)
      LoadProperty(Of DocumentsTehnicalExamsReportsVisualErrors) _
      (VisualErrorsProperty, DocumentsTehnicalExamsReportsVisualErrors.GetDocumentsTehnicalExamsReportsVisualErrors(drc))
     End Using
    End Using

   End Using

  Catch ex As Exception
   Database.LogException("DocumentsTehnicalExamsReport.DataPortal_Fetch", ex)
   Throw New DbCslaException("DocumentsTehnicalExamsReport.DataPortal_Fetch", ex)
  End Try

 End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

 Protected Overrides Sub DataPortal_Insert()
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm1 As SqlCommand = cn.CreateCommand
     Dim _docNum As Integer = 0
     Dim _idStation As Integer = CType(Csla.ApplicationContext.LocalContext("objCurentUser"), UsersInfo).IdStation
     ' Dim _stanica As TehnicalExamOrganization
     With cm1
      .CommandType = CommandType.StoredProcedure
      .CommandText = SpGetNumberForReport

      .Parameters.AddWithValue("@IdStation", _idStation)
      ' _stanica = TehnicalExamOrganization.GetTehnicalExamOrganization(_idStation)
      _docNum = cm1.ExecuteScalar
     End With

     Me.RegNumber = _idStation & "-" & _docNum & "/" & Now.Year


    End Using
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spAdd

      .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
      .Parameters.AddWithValue("@IdTypeOfTehnicalExam", ReadProperty(Of Integer)(IdTypeOfTehnicalExamProperty))
      .Parameters.AddWithValue("@RegNumber", ReadProperty(Of String)(RegNumberProperty))
      .Parameters.AddWithValue("@MadeDate", ReadProperty(Of SmartDate)(MadeDateProperty).DBValue)
      .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
      .Parameters.AddWithValue("@IdOrganizationForTehnicalExam", ReadProperty(Of Integer)(IdOrganizationForTehnicalExamProperty))
      .Parameters.AddWithValue("@IdFirsControler", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
      .Parameters.AddWithValue("@IdSecondControler", ReadProperty(Of Integer)(IdSecondControlerProperty))
      .Parameters.AddWithValue("@VehicleIsRight", ReadProperty(Of Boolean)(VehicleIsRightProperty))
      .Parameters.AddWithValue("@ExplanationNote", ReadProperty(Of String)(ExplanationNoteProperty))
      .Parameters.AddWithValue("@DriversWarning", ReadProperty(Of String)(DriversWarningProperty))
      .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
      .Parameters.AddWithValue("@axis1Left", ReadProperty(Of Decimal)(axis1LeftProperty))
      .Parameters.AddWithValue("@axis1Right", ReadProperty(Of Decimal)(axis1RightProperty))
      .Parameters.AddWithValue("@axis1Gj", ReadProperty(Of Decimal)(axis1GjProperty))
      .Parameters.AddWithValue("@axis1LeftPj", ReadProperty(Of Decimal)(axis1LeftPjProperty))
      .Parameters.AddWithValue("@axis1PN", ReadProperty(Of Decimal)(axis1PNProperty))
      .Parameters.AddWithValue("@axis2Left", ReadProperty(Of Decimal)(axis2LeftProperty))
      .Parameters.AddWithValue("@axis2Right", ReadProperty(Of Decimal)(axis2RightProperty))
      .Parameters.AddWithValue("@axis2Gj", ReadProperty(Of Decimal)(axis2GjProperty))
      .Parameters.AddWithValue("@axis2LeftPj", ReadProperty(Of Decimal)(axis2LeftPjProperty))
      .Parameters.AddWithValue("@axis2PN", ReadProperty(Of Decimal)(axis2PNProperty))
      .Parameters.AddWithValue("@axis3Left", ReadProperty(Of Decimal)(axis3LeftProperty))
      .Parameters.AddWithValue("@axis3Right", ReadProperty(Of Decimal)(axis3RightProperty))
      .Parameters.AddWithValue("@axis3Gj", ReadProperty(Of Decimal)(axis3GjProperty))
      .Parameters.AddWithValue("@axis3LeftPj", ReadProperty(Of Decimal)(axis3LeftPjProperty))
      .Parameters.AddWithValue("@axis3PN", ReadProperty(Of Decimal)(axis3PNProperty))
      .Parameters.AddWithValue("@axis4Left", ReadProperty(Of Decimal)(axis4LeftProperty))
      .Parameters.AddWithValue("@axis4Right", ReadProperty(Of Decimal)(axis4RightProperty))
      .Parameters.AddWithValue("@axis4Gj", ReadProperty(Of Decimal)(axis4GjProperty))
      .Parameters.AddWithValue("@axis4LeftPj", ReadProperty(Of Decimal)(axis4LeftPjProperty))
      .Parameters.AddWithValue("@axis4PN", ReadProperty(Of Decimal)(axis4PNProperty))
      .Parameters.AddWithValue("@axisParkingLeft", ReadProperty(Of Decimal)(axisParkingLeftProperty))
      .Parameters.AddWithValue("@axisParkingRight", ReadProperty(Of Decimal)(axisParkingRightProperty))
      .Parameters.AddWithValue("@axisParkingGj", ReadProperty(Of Decimal)(axisParkingGjProperty))
      .Parameters.AddWithValue("@axisParkingLeftPj", ReadProperty(Of Decimal)(axisParkingLeftPjProperty))
      .Parameters.AddWithValue("@axisParkingPN", ReadProperty(Of Decimal)(axisParkingPNProperty))
      .Parameters.AddWithValue("@waight", ReadProperty(Of Decimal)(waightProperty))
      .Parameters.AddWithValue("@effectOfWorkingBreakEmpty", ReadProperty(Of Decimal)(effectOfWorkingBreakEmptyProperty))
      .Parameters.AddWithValue("@effectOfWorkingBreakFull", ReadProperty(Of Decimal)(effectOfWorkingBreakFullProperty))
      .Parameters.AddWithValue("@effectOfSecondaryBreak", ReadProperty(Of Decimal)(effectOfSecondaryBreakProperty))
      .Parameters.AddWithValue("@effectOfParkingBreak", ReadProperty(Of Decimal)(effectOfParkingBreakProperty))
      .Parameters.AddWithValue("@speedOfTurns", ReadProperty(Of Decimal)(speedOfTurnsProperty))
      .Parameters.AddWithValue("@CO", ReadProperty(Of Decimal)(COProperty))
      .Parameters.AddWithValue("@numEngineTurns", ReadProperty(Of Decimal)(numEngineTurnsProperty))
      .Parameters.AddWithValue("@cOPlusTurns", ReadProperty(Of Decimal)(cOPlusTurnsProperty))
      .Parameters.AddWithValue("@lambda", ReadProperty(Of Decimal)(lambdaProperty))
      .Parameters.AddWithValue("@pinpoints", ReadProperty(Of Decimal)(pinpointsProperty))
      .Parameters.AddWithValue("@noise", ReadProperty(Of Decimal)(noiseProperty))
      .Parameters.AddWithValue("@tempOfEngineOil", ReadProperty(Of Decimal)(tempOfEngineOilProperty))
      .Parameters.AddWithValue("@technicalChanges", ReadProperty(Of String)(technicalChangesProperty))
      Dim param As New SqlParameter("@newId", SqlDbType.Int)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)
      param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)

      .ExecuteNonQuery()

      LoadProperty(Of Long)(IdProperty, CInt(.Parameters("@newId").Value))
      _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
     End With

    End Using

    Using cm1 As SqlCommand = cn.CreateCommand
     cm1.CommandType = CommandType.StoredProcedure
     cm1.CommandText = "TehnicalExamPassed"
     cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
     cm1.Parameters.AddWithValue("@IdTehnicalExam", ReadProperty(Of Long)(IdProperty))
     cm1.Parameters.AddWithValue("@IdTehnicalExamType", ReadProperty(Of Integer)(IdTypeOfTehnicalExamProperty))
     cm1.ExecuteNonQuery()
    End Using

    'update child objects

    AddDeptsToCustomer(cn)

    FieldManager.UpdateChildren(Me)

    If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
     ApplicationContext.LocalContext.Remove("cn")
    End If


   End Using
  Catch ex As Exception
   Database.LogException("DocumentsTehnicalExamsReport.DataPortal_Insert", ex)
   Throw New DbCslaException("DocumentsTehnicalExamsReport.DataPortal_Insert", ex)
  Finally
   Database.LogInfo("DocumentsTehnicalExamsReport.DataPortal_Insert", GetHashCode())
  End Try
 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Protected Overrides Sub DataPortal_Update()
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spUpdate

      .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
      .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
      .Parameters.AddWithValue("@IdTypeOfTehnicalExam", ReadProperty(Of Integer)(IdTypeOfTehnicalExamProperty))
      .Parameters.AddWithValue("@RegNumber", ReadProperty(Of String)(RegNumberProperty))
      .Parameters.AddWithValue("@MadeDate", ReadProperty(Of SmartDate)(MadeDateProperty).DBValue)
      .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
      .Parameters.AddWithValue("@IdOrganizationForTehnicalExam", ReadProperty(Of Integer)(IdOrganizationForTehnicalExamProperty))
      .Parameters.AddWithValue("@IdFirsControler", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
      .Parameters.AddWithValue("@IdSecondControler", ReadProperty(Of Integer)(IdSecondControlerProperty))
      .Parameters.AddWithValue("@VehicleIsRight", ReadProperty(Of Boolean)(VehicleIsRightProperty))
      .Parameters.AddWithValue("@ExplanationNote", ReadProperty(Of String)(ExplanationNoteProperty))
      .Parameters.AddWithValue("@DriversWarning", ReadProperty(Of String)(DriversWarningProperty))
      .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
      .Parameters.AddWithValue("@axis1Left", ReadProperty(Of Decimal)(axis1LeftProperty))
      .Parameters.AddWithValue("@axis1Right", ReadProperty(Of Decimal)(axis1RightProperty))
      .Parameters.AddWithValue("@axis1Gj", ReadProperty(Of Decimal)(axis1GjProperty))
      .Parameters.AddWithValue("@axis1LeftPj", ReadProperty(Of Decimal)(axis1LeftPjProperty))
      .Parameters.AddWithValue("@axis1PN", ReadProperty(Of Decimal)(axis1PNProperty))
      .Parameters.AddWithValue("@axis2Left", ReadProperty(Of Decimal)(axis2LeftProperty))
      .Parameters.AddWithValue("@axis2Right", ReadProperty(Of Decimal)(axis2RightProperty))
      .Parameters.AddWithValue("@axis2Gj", ReadProperty(Of Decimal)(axis2GjProperty))
      .Parameters.AddWithValue("@axis2LeftPj", ReadProperty(Of Decimal)(axis2LeftPjProperty))
      .Parameters.AddWithValue("@axis2PN", ReadProperty(Of Decimal)(axis2PNProperty))
      .Parameters.AddWithValue("@axis3Left", ReadProperty(Of Decimal)(axis3LeftProperty))
      .Parameters.AddWithValue("@axis3Right", ReadProperty(Of Decimal)(axis3RightProperty))
      .Parameters.AddWithValue("@axis3Gj", ReadProperty(Of Decimal)(axis3GjProperty))
      .Parameters.AddWithValue("@axis3LeftPj", ReadProperty(Of Decimal)(axis3LeftPjProperty))
      .Parameters.AddWithValue("@axis3PN", ReadProperty(Of Decimal)(axis3PNProperty))
      .Parameters.AddWithValue("@axis4Left", ReadProperty(Of Decimal)(axis4LeftProperty))
      .Parameters.AddWithValue("@axis4Right", ReadProperty(Of Decimal)(axis4RightProperty))
      .Parameters.AddWithValue("@axis4Gj", ReadProperty(Of Decimal)(axis4GjProperty))
      .Parameters.AddWithValue("@axis4LeftPj", ReadProperty(Of Decimal)(axis4LeftPjProperty))
      .Parameters.AddWithValue("@axis4PN", ReadProperty(Of Decimal)(axis4PNProperty))
      .Parameters.AddWithValue("@axisParkingLeft", ReadProperty(Of Decimal)(axisParkingLeftProperty))
      .Parameters.AddWithValue("@axisParkingRight", ReadProperty(Of Decimal)(axisParkingRightProperty))
      .Parameters.AddWithValue("@axisParkingGj", ReadProperty(Of Decimal)(axisParkingGjProperty))
      .Parameters.AddWithValue("@axisParkingLeftPj", ReadProperty(Of Decimal)(axisParkingLeftPjProperty))
      .Parameters.AddWithValue("@axisParkingPN", ReadProperty(Of Decimal)(axisParkingPNProperty))
      .Parameters.AddWithValue("@waight", ReadProperty(Of Decimal)(waightProperty))
      .Parameters.AddWithValue("@effectOfWorkingBreakEmpty", ReadProperty(Of Decimal)(effectOfWorkingBreakEmptyProperty))
      .Parameters.AddWithValue("@effectOfWorkingBreakFull", ReadProperty(Of Decimal)(effectOfWorkingBreakFullProperty))
      .Parameters.AddWithValue("@effectOfSecondaryBreak", ReadProperty(Of Decimal)(effectOfSecondaryBreakProperty))
      .Parameters.AddWithValue("@effectOfParkingBreak", ReadProperty(Of Decimal)(effectOfParkingBreakProperty))
      .Parameters.AddWithValue("@speedOfTurns", ReadProperty(Of Decimal)(speedOfTurnsProperty))
      .Parameters.AddWithValue("@CO", ReadProperty(Of Decimal)(COProperty))
      .Parameters.AddWithValue("@numEngineTurns", ReadProperty(Of Decimal)(numEngineTurnsProperty))
      .Parameters.AddWithValue("@cOPlusTurns", ReadProperty(Of Decimal)(cOPlusTurnsProperty))
      .Parameters.AddWithValue("@lambda", ReadProperty(Of Decimal)(lambdaProperty))
      .Parameters.AddWithValue("@pinpoints", ReadProperty(Of Decimal)(pinpointsProperty))
      .Parameters.AddWithValue("@noise", ReadProperty(Of Decimal)(noiseProperty))
      .Parameters.AddWithValue("@tempOfEngineOil", ReadProperty(Of Decimal)(tempOfEngineOilProperty))
      .Parameters.AddWithValue("@technicalChanges", ReadProperty(Of String)(technicalChangesProperty))

      .Parameters.AddWithValue("@lastChanged", _lastChanged)
      Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)

      .ExecuteNonQuery()

      _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
     End With

    End Using

    Using cm1 As SqlCommand = cn.CreateCommand
     cm1.CommandType = CommandType.StoredProcedure
     cm1.CommandText = "TehnicalExamPassed"
          cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
          cm1.Parameters.AddWithValue("@IdTehnicalExam", ReadProperty(Of Long)(IdProperty))
          cm1.Parameters.AddWithValue("@IdTehnicalExamType", ReadProperty(Of Integer)(IdTypeOfTehnicalExamProperty))

          cm1.ExecuteNonQuery()
    End Using
    'update child objects
    FieldManager.UpdateChildren(Me)
    If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
     ApplicationContext.LocalContext.Remove("cn")
    End If
   End Using
  Catch ex As Exception
   Database.LogException("DocumentsTehnicalExamsReport.DataPortal_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DBConcurrencyException("DocumentsTehnicalExamsReport.DataPortal_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Protected Overrides Sub DataPortal_DeleteSelf()
  DataPortal_Delete(New SingleCriteria(Of DocumentsTehnicalExamsReport, Integer)(Id))
 End Sub

 Private Overloads Sub DataPortal_Delete(ByVal criteria As SingleCriteria(Of DocumentsTehnicalExamsReport, Integer))
  Database.LogInfo("DocumentsTehnicalExamsReport.DataPortal_Delete", GetHashCode())
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spDelete
      .Parameters.AddWithValue("@id", criteria.Value)
      .ExecuteNonQuery()
     End With
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("DocumentsTehnicalExamsReport.DataPortal_Delete", ex)
   Throw New DbCslaException("DocumentsTehnicalExamsReport.DataPortal_Delete", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access

#Region " Child Data Access "

#Region " Data Access - Fetch "

 Private Sub Child_Fetch(ByVal dr As SafeDataReader)
  Database.LogInfo("DocumentsTehnicalExamsReport.Child_Fetch", GetHashCode())
  Try
   LoadProperty(Of Long)(IdProperty, dr.GetInt64("Id"))
   LoadProperty(Of Long)(IdCustomerVehicleRelationProperty, dr.GetInt64("IdCustomerVehicleRelation"))
   LoadProperty(Of Integer)(IdTypeOfTehnicalExamProperty, dr.GetInt32("IdTypeOfTehnicalExam"))
   LoadProperty(Of String)(RegNumberProperty, dr.GetString("RegNumber"))
   LoadProperty(Of SmartDate, Date?)(MadeDateProperty, dr.GetSmartDate("MadeDate", True))
   LoadProperty(Of SmartDate, Date?)(ValidTillDateProperty, dr.GetSmartDate("ValidTillDate", True))
   LoadProperty(Of Integer)(IdOrganizationForTehnicalExamProperty, dr.GetInt32("IdOrganizationForTehnicalExam"))
   LoadProperty(Of Integer)(IdFirsControlerProperty, dr.GetInt32("IdFirsControler"))
   LoadProperty(Of Integer)(IdSecondControlerProperty, dr.GetInt32("IdSecondControler"))
   LoadProperty(Of Boolean)(VehicleIsRightProperty, dr.GetBoolean("VehicleIsRight"))
   LoadProperty(Of String)(ExplanationNoteProperty, dr.GetString("ExplanationNote"))
   LoadProperty(Of String)(DriversWarningProperty, dr.GetString("DriversWarning"))
   LoadProperty(Of String)(NoteProperty, dr.GetString("Note"))
   dr.GetBytes("LastChanged", 0, _lastChanged, 0, 8)
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm1 As SqlCommand = cn.CreateCommand
     cm1.CommandType = CommandType.StoredProcedure
     cm1.CommandText = spGetChildDetails
     cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))

     Using drc As New SafeDataReader(cm1.ExecuteReader)
      LoadProperty(Of DocumentsTehnicalExamsReportsDetails) _
      (DetailsProperty, DocumentsTehnicalExamsReportsDetails.GetDocumentsTehnicalExamsReportsDetails(drc))
     End Using
    End Using
    Using cm1 As SqlCommand = cn.CreateCommand
     cm1.CommandType = CommandType.StoredProcedure
     cm1.CommandText = spGetChildVisualErrors
     cm1.Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))

     Using drc As New SafeDataReader(cm1.ExecuteReader)
      LoadProperty(Of DocumentsTehnicalExamsReportsVisualErrors) _
      (VisualErrorsProperty, DocumentsTehnicalExamsReportsVisualErrors.GetDocumentsTehnicalExamsReportsVisualErrors(drc))
     End Using
    End Using
   End Using
  Catch ex As Exception
   Database.LogException("DocumentsTehnicalExamsReport.Child_Fetch", ex)
   Throw New DbCslaException("DocumentsTehnicalExamsReport.Child_Fetch", ex)
  End Try

 End Sub

#End Region 'Data Access - Fetch

#Region " Data Access - Insert "

 Private Sub Child_Insert()
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm1 As SqlCommand = cn.CreateCommand
     Dim _docNum As Integer = 0
     Dim _idStation As Integer = CType(Csla.ApplicationContext.LocalContext("objCurentUser"), UsersInfo).IdStation
     With cm1
      .CommandType = CommandType.StoredProcedure
      .CommandText = SpGetNumberForReport

      .Parameters.AddWithValue("@IdStation", _idStation)

      _docNum = cm1.ExecuteScalar
     End With

     Me.RegNumber = _idStation & "-" & _docNum & "/" & Now.Year


    End Using
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spAdd
      .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
      .Parameters.AddWithValue("@IdTypeOfTehnicalExam", ReadProperty(Of Integer)(IdTypeOfTehnicalExamProperty))
      .Parameters.AddWithValue("@RegNumber", ReadProperty(Of String)(RegNumberProperty))
      .Parameters.AddWithValue("@MadeDate", ReadProperty(Of SmartDate)(MadeDateProperty).DBValue)
      .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
      .Parameters.AddWithValue("@IdOrganizationForTehnicalExam", ReadProperty(Of Integer)(IdOrganizationForTehnicalExamProperty))
      .Parameters.AddWithValue("@IdFirsControler", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
      .Parameters.AddWithValue("@IdSecondControler", ReadProperty(Of Integer)(IdSecondControlerProperty))
      .Parameters.AddWithValue("@VehicleIsRight", ReadProperty(Of Boolean)(VehicleIsRightProperty))
      .Parameters.AddWithValue("@ExplanationNote", ReadProperty(Of String)(ExplanationNoteProperty))
      .Parameters.AddWithValue("@DriversWarning", ReadProperty(Of String)(DriversWarningProperty))
      .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))

      .Parameters.AddWithValue("@axis1Left", ReadProperty(Of Decimal)(axis1LeftProperty))
      .Parameters.AddWithValue("@axis1Right", ReadProperty(Of Decimal)(axis1RightProperty))
      .Parameters.AddWithValue("@axis1Gj", ReadProperty(Of Decimal)(axis1GjProperty))
      .Parameters.AddWithValue("@axis1LeftPj", ReadProperty(Of Decimal)(axis1LeftPjProperty))
      .Parameters.AddWithValue("@axis1PN", ReadProperty(Of Decimal)(axis1PNProperty))
      .Parameters.AddWithValue("@axis2Left", ReadProperty(Of Decimal)(axis2LeftProperty))
      .Parameters.AddWithValue("@axis2Right", ReadProperty(Of Decimal)(axis2RightProperty))
      .Parameters.AddWithValue("@axis2Gj", ReadProperty(Of Decimal)(axis2GjProperty))
      .Parameters.AddWithValue("@axis2LeftPj", ReadProperty(Of Decimal)(axis2LeftPjProperty))
      .Parameters.AddWithValue("@axis2PN", ReadProperty(Of Decimal)(axis2PNProperty))
      .Parameters.AddWithValue("@axis3Left", ReadProperty(Of Decimal)(axis3LeftProperty))
      .Parameters.AddWithValue("@axis3Right", ReadProperty(Of Decimal)(axis3RightProperty))
      .Parameters.AddWithValue("@axis3Gj", ReadProperty(Of Decimal)(axis3GjProperty))
      .Parameters.AddWithValue("@axis3LeftPj", ReadProperty(Of Decimal)(axis3LeftPjProperty))
      .Parameters.AddWithValue("@axis3PN", ReadProperty(Of Decimal)(axis3PNProperty))
      .Parameters.AddWithValue("@axis4Left", ReadProperty(Of Decimal)(axis4LeftProperty))
      .Parameters.AddWithValue("@axis4Right", ReadProperty(Of Decimal)(axis4RightProperty))
      .Parameters.AddWithValue("@axis4Gj", ReadProperty(Of Decimal)(axis4GjProperty))
      .Parameters.AddWithValue("@axis4LeftPj", ReadProperty(Of Decimal)(axis4LeftPjProperty))
      .Parameters.AddWithValue("@axis4PN", ReadProperty(Of Decimal)(axis4PNProperty))
      .Parameters.AddWithValue("@axisParkingLeft", ReadProperty(Of Decimal)(axisParkingLeftProperty))
      .Parameters.AddWithValue("@axisParkingRight", ReadProperty(Of Decimal)(axisParkingRightProperty))
      .Parameters.AddWithValue("@axisParkingGj", ReadProperty(Of Decimal)(axisParkingGjProperty))
      .Parameters.AddWithValue("@axisParkingLeftPj", ReadProperty(Of Decimal)(axisParkingLeftPjProperty))
      .Parameters.AddWithValue("@axisParkingPN", ReadProperty(Of Decimal)(axisParkingPNProperty))
      .Parameters.AddWithValue("@waight", ReadProperty(Of Decimal)(waightProperty))
      .Parameters.AddWithValue("@effectOfWorkingBreakEmpty", ReadProperty(Of Decimal)(effectOfWorkingBreakEmptyProperty))
      .Parameters.AddWithValue("@effectOfWorkingBreakFull", ReadProperty(Of Decimal)(effectOfWorkingBreakFullProperty))
      .Parameters.AddWithValue("@effectOfSecondaryBreak", ReadProperty(Of Decimal)(effectOfSecondaryBreakProperty))
      .Parameters.AddWithValue("@effectOfParkingBreak", ReadProperty(Of Decimal)(effectOfParkingBreakProperty))
      .Parameters.AddWithValue("@speedOfTurns", ReadProperty(Of Decimal)(speedOfTurnsProperty))
      .Parameters.AddWithValue("@CO", ReadProperty(Of Decimal)(COProperty))
      .Parameters.AddWithValue("@numEngineTurns", ReadProperty(Of Decimal)(numEngineTurnsProperty))
      .Parameters.AddWithValue("@cOPlusTurns", ReadProperty(Of Decimal)(cOPlusTurnsProperty))
      .Parameters.AddWithValue("@lambda", ReadProperty(Of Decimal)(lambdaProperty))
      .Parameters.AddWithValue("@pinpoints", ReadProperty(Of Decimal)(pinpointsProperty))
      .Parameters.AddWithValue("@noise", ReadProperty(Of Decimal)(noiseProperty))
      .Parameters.AddWithValue("@tempOfEngineOil", ReadProperty(Of Decimal)(tempOfEngineOilProperty))
      .Parameters.AddWithValue("@technicalChanges", ReadProperty(Of String)(technicalChangesProperty))

      Dim param As New SqlParameter("@newId", SqlDbType.Int)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)
      param = New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)

      .ExecuteNonQuery()

      LoadProperty(Of Long)(IdProperty, CInt(.Parameters("@newId").Value))
      _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
     End With
    End Using
    Try
     Using cm1 As SqlCommand = cn.CreateCommand
      cm1.CommandType = CommandType.StoredProcedure
      cm1.CommandText = "TehnicalExamPassed"
            cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
            cm1.Parameters.AddWithValue("@IdTehnicalExam", ReadProperty(Of Long)(IdProperty))
            cm1.Parameters.AddWithValue("@IdTehnicalExamType", ReadProperty(Of Integer)(IdTypeOfTehnicalExamProperty))

            cm1.ExecuteNonQuery()
     End Using
    Catch ex As Exception

    End Try
    AddDeptsToCustomer(cn)
    FieldManager.UpdateChildren(Me)
    If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
     ApplicationContext.LocalContext.Remove("cn")
    End If

   End Using
  Catch ex As Exception
   Database.LogException("DocumentsTehnicalExamsReport.Child_Insert", ex)
   Throw New DbCslaException("DocumentsTehnicalExamsReport.Child_Insert", ex)
  Finally
   Database.LogInfo("DocumentsTehnicalExamsReport.Child_Insert", GetHashCode)
  End Try

 End Sub
#End Region 'Data Access - Insert

#Region " Data Access - Update "

 Private Sub Child_Update()
  Database.LogInfo("DocumentsTehnicalExamsReport.Child_Update", GetHashCode)
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    ApplicationContext.LocalContext("cn") = cn
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spUpdate

      .Parameters.AddWithValue("@Id", ReadProperty(Of Long)(IdProperty))
      .Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
      .Parameters.AddWithValue("@IdTypeOfTehnicalExam", ReadProperty(Of Integer)(IdTypeOfTehnicalExamProperty))
      .Parameters.AddWithValue("@RegNumber", ReadProperty(Of String)(RegNumberProperty))
      .Parameters.AddWithValue("@MadeDate", ReadProperty(Of SmartDate)(MadeDateProperty).DBValue)
      .Parameters.AddWithValue("@ValidTillDate", ReadProperty(Of SmartDate)(ValidTillDateProperty).DBValue)
      .Parameters.AddWithValue("@IdOrganizationForTehnicalExam", ReadProperty(Of Integer)(IdOrganizationForTehnicalExamProperty))
      .Parameters.AddWithValue("@IdFirsControler", Csla.ApplicationContext.LocalContext.Item("EmployeeID"))
      .Parameters.AddWithValue("@IdSecondControler", ReadProperty(Of Integer)(IdSecondControlerProperty))
      .Parameters.AddWithValue("@VehicleIsRight", ReadProperty(Of Boolean)(VehicleIsRightProperty))
      .Parameters.AddWithValue("@ExplanationNote", ReadProperty(Of String)(ExplanationNoteProperty))
      .Parameters.AddWithValue("@DriversWarning", ReadProperty(Of String)(DriversWarningProperty))
      .Parameters.AddWithValue("@Note", ReadProperty(Of String)(NoteProperty))
      .Parameters.AddWithValue("@axis1Left", ReadProperty(Of Decimal)(axis1LeftProperty))
      .Parameters.AddWithValue("@axis1Right", ReadProperty(Of Decimal)(axis1RightProperty))
      .Parameters.AddWithValue("@axis1Gj", ReadProperty(Of Decimal)(axis1GjProperty))
      .Parameters.AddWithValue("@axis1LeftPj", ReadProperty(Of Decimal)(axis1LeftPjProperty))
      .Parameters.AddWithValue("@axis1PN", ReadProperty(Of Decimal)(axis1PNProperty))
      .Parameters.AddWithValue("@axis2Left", ReadProperty(Of Decimal)(axis2LeftProperty))
      .Parameters.AddWithValue("@axis2Right", ReadProperty(Of Decimal)(axis2RightProperty))
      .Parameters.AddWithValue("@axis2Gj", ReadProperty(Of Decimal)(axis2GjProperty))
      .Parameters.AddWithValue("@axis2LeftPj", ReadProperty(Of Decimal)(axis2LeftPjProperty))
      .Parameters.AddWithValue("@axis2PN", ReadProperty(Of Decimal)(axis2PNProperty))
      .Parameters.AddWithValue("@axis3Left", ReadProperty(Of Decimal)(axis3LeftProperty))
      .Parameters.AddWithValue("@axis3Right", ReadProperty(Of Decimal)(axis3RightProperty))
      .Parameters.AddWithValue("@axis3Gj", ReadProperty(Of Decimal)(axis3GjProperty))
      .Parameters.AddWithValue("@axis3LeftPj", ReadProperty(Of Decimal)(axis3LeftPjProperty))
      .Parameters.AddWithValue("@axis3PN", ReadProperty(Of Decimal)(axis3PNProperty))
      .Parameters.AddWithValue("@axis4Left", ReadProperty(Of Decimal)(axis4LeftProperty))
      .Parameters.AddWithValue("@axis4Right", ReadProperty(Of Decimal)(axis4RightProperty))
      .Parameters.AddWithValue("@axis4Gj", ReadProperty(Of Decimal)(axis4GjProperty))
      .Parameters.AddWithValue("@axis4LeftPj", ReadProperty(Of Decimal)(axis4LeftPjProperty))
      .Parameters.AddWithValue("@axis4PN", ReadProperty(Of Decimal)(axis4PNProperty))
      .Parameters.AddWithValue("@axisParkingLeft", ReadProperty(Of Decimal)(axisParkingLeftProperty))
      .Parameters.AddWithValue("@axisParkingRight", ReadProperty(Of Decimal)(axisParkingRightProperty))
      .Parameters.AddWithValue("@axisParkingGj", ReadProperty(Of Decimal)(axisParkingGjProperty))
      .Parameters.AddWithValue("@axisParkingLeftPj", ReadProperty(Of Decimal)(axisParkingLeftPjProperty))
      .Parameters.AddWithValue("@axisParkingPN", ReadProperty(Of Decimal)(axisParkingPNProperty))
      .Parameters.AddWithValue("@waight", ReadProperty(Of Decimal)(waightProperty))
      .Parameters.AddWithValue("@effectOfWorkingBreakEmpty", ReadProperty(Of Decimal)(effectOfWorkingBreakEmptyProperty))
      .Parameters.AddWithValue("@effectOfWorkingBreakFull", ReadProperty(Of Decimal)(effectOfWorkingBreakFullProperty))
      .Parameters.AddWithValue("@effectOfSecondaryBreak", ReadProperty(Of Decimal)(effectOfSecondaryBreakProperty))
      .Parameters.AddWithValue("@effectOfParkingBreak", ReadProperty(Of Decimal)(effectOfParkingBreakProperty))
      .Parameters.AddWithValue("@speedOfTurns", ReadProperty(Of Decimal)(speedOfTurnsProperty))
      .Parameters.AddWithValue("@CO", ReadProperty(Of Decimal)(COProperty))
      .Parameters.AddWithValue("@numEngineTurns", ReadProperty(Of Decimal)(numEngineTurnsProperty))
      .Parameters.AddWithValue("@cOPlusTurns", ReadProperty(Of Decimal)(cOPlusTurnsProperty))
      .Parameters.AddWithValue("@lambda", ReadProperty(Of Decimal)(lambdaProperty))
      .Parameters.AddWithValue("@pinpoints", ReadProperty(Of Decimal)(pinpointsProperty))
      .Parameters.AddWithValue("@noise", ReadProperty(Of Decimal)(noiseProperty))
      .Parameters.AddWithValue("@tempOfEngineOil", ReadProperty(Of Decimal)(tempOfEngineOilProperty))
      .Parameters.AddWithValue("@technicalChanges", ReadProperty(Of String)(technicalChangesProperty))
      .Parameters.AddWithValue("@lastChanged", _lastChanged)
      Dim param As New SqlParameter("@newLastChanged", SqlDbType.Timestamp)
      param.Direction = ParameterDirection.Output
      .Parameters.Add(param)

      .ExecuteNonQuery()

      _lastChanged = CType(.Parameters("@newLastChanged").Value, Byte())
     End With
    End Using

    Using cm1 As SqlCommand = cn.CreateCommand
     cm1.CommandType = CommandType.StoredProcedure
     cm1.CommandText = "TehnicalExamPassed"
          cm1.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
          cm1.Parameters.AddWithValue("@IdTehnicalExam", ReadProperty(Of Long)(IdProperty))
          cm1.Parameters.AddWithValue("@IdTehnicalExamType", ReadProperty(Of Integer)(IdTypeOfTehnicalExamProperty))
          cm1.ExecuteNonQuery()
    End Using
    'update child objects
    FieldManager.UpdateChildren(Me)
    If ApplicationContext.ExecutionLocation = ExecutionLocations.Client Then
     ApplicationContext.LocalContext.Remove("cn")
    End If

   End Using
  Catch ex As Exception
   Database.LogException("DocumentsTehnicalExamsReport.Child_Update", ex)
   If Not ex.Message.EndsWith("drug korisnik") Then
    Throw New DbCslaException("DocumentsTehnicalExamsReport.Child_Update", ex)
   End If
  End Try
 End Sub
#End Region 'Data Access - Update

#Region " Data Access - Delete "

 Private Sub Child_DeleteSelf()

  Database.LogInfo("DocumentsTehnicalExamsReport.Child_DeleteSelf", GetHashCode)
  Try
   Using cn As SqlConnection = Database.VTE_SqlConnection
    Using cm As SqlCommand = cn.CreateCommand
     With cm
      .CommandType = CommandType.StoredProcedure
      .CommandText = spDelete
      .Parameters.AddWithValue("@id", ReadProperty(Of Long)(IdProperty))
      .ExecuteNonQuery()
     End With
    End Using

   End Using
  Catch ex As Exception
   Database.LogException("DocumentsTehnicalExamsReport.Child_Fetch", ex)
   Throw New DbCslaException("DocumentsTehnicalExamsReport.Child_Fetch", ex)
  End Try
 End Sub

#End Region ' Data Access - Delete

#End Region 'Chlild Data Access

#Region " Payment "
 Private Sub AddDeptsToCustomer(ByVal cn As SqlConnection)
  'ova e ako tehnickiot ne e izvrsen tuka
  Dim objCurentTehExamStation As TehnicalExamOrganizationsInfo = CType(Csla.ApplicationContext.LocalContext("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)

  If Not ReadProperty(Of Integer)(IdOrganizationForTehnicalExamProperty) = objCurentTehExamStation.Id Then
   Exit Sub
  End If

  Try
   ' Dim objRlationList As CustomerVehiclesRelationsList = Csla.ApplicationContext.LocalContext("objRlationList")
   'zemi go voziloto od relacijata
   Dim tmpRelacija As CustomerVehiclesRelation = _
   CustomerVehiclesRelation.GetCustomerVehiclesRelation( _
ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
   Dim tmpVehicleId As Long = tmpRelacija.IdVehicle
   Dim vInfo As Vehicle = Vehicle.GetVehicle(tmpVehicleId)
   '  Dim objIsuerList As RegistrationIssuerList
   Dim idCommunityLastReg As Integer = 0

   Try
    Dim CitiesI As CityInfo = _
CType(Csla.ApplicationContext.LocalContext.Item("objCityList"), CityList). _
GetCityListById(Customer.GetCustomer(tmpRelacija.IdCustomer).IdLivingCity)
    'objIsuerList = CType(Csla.ApplicationContext.LocalContext.Item("objRegistrationIssuerList"), RegistrationIssuerList)
    idCommunityLastReg = (CitiesI.IdCommunity) 'objIsuerList.GetRegistrationIssuerInfo(vInfo.LastIdRegistrationIssuer).IdCommunity

   Catch ex As Exception
    ' objIsuerList = RegistrationIssuerList.GetRegistrationIssuerList 'CType(Csla.ApplicationContext.LocalContext.Item("objRegistrationIssuerList"), RegistrationIssuerList)
    idCommunityLastReg = 0 'objIsuerList.GetRegistrationIssuerInfo(vInfo.LastIdRegistrationIssuer).IdCommunity
   End Try
   'filtriraj go katalogot so paramaetri:
   '1. TrigerdBy? (Request)
   '2. IdVehicleCategoryForPayments (od vozilito)
   '3. prebaraj dali konkretnoto vozilo spaga vo nekoi od tie

   'za neredoven i redoven thenicki
   Dim pCatalog As PaymentCataologList
   Dim delitel As Single = 1
   If Me.IdTypeOfTehnicalExam > 1 Then
    'tuka dodadi delitel

    Dim objTehnicalExamTypes As TehnicalExamsTypesList = Csla.ApplicationContext.LocalContext("objTehnicalExamsTypesList")
    'delitel = objTehnicalExamTypes.GetTehnicalExamsTypesListById(Me.IdTypeOfTehnicalExam).ValidNumOfDays _
    '          / objTehnicalExamTypes.GetTehnicalExamsTypesListById(1).ValidNumOfDays
    delitel = objTehnicalExamTypes.GetTehnicalExamsTypesListById(Me.IdTypeOfTehnicalExam).PercentOfFullExam / 100
    pCatalog = _
    CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
    PaymentCataologList).GetPaymentForDepts("TrigerdByIrregularTechnicalExam", vInfo)
   Else
    pCatalog = _
    CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"),  _
    PaymentCataologList).GetPaymentForDepts("TrigerdByTechnicalExam", vInfo)
   End If


   'zapamti gi site plakanje vo listata
   If delitel > 0 Then
    For Each pInfo As PaymentCataologInfo In pCatalog
     'vo sluaj poedinecno(so formula)
     Dim pomDelitel As Single = 1 ' delitel
     If (pInfo.ParametarFrom = 0) AndAlso (pInfo.ParametarTo = 0) Then

      If pInfo.IdCommunity > 0 Then
       If idCommunityLastReg = pInfo.IdCommunity Then
        Using cm As SqlCommand = cn.CreateCommand
         cm.CommandType = CommandType.StoredProcedure
         cm.CommandText = "addCustomerFinancialStatFromTehnicalExam"
         cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
         cm.Parameters.AddWithValue("@IdDocumentTehnicalExam", ReadProperty(Of Long)(IdProperty))
         If Me.IdTypeOfTehnicalExam > 1 Then

          cm.Parameters.AddWithValue("@note", "по нередовен технички преглед бр." & ReadProperty(Of Long)(IdProperty))
         Else
          cm.Parameters.AddWithValue("@note", "по технички преглед бр." & ReadProperty(Of Long)(IdProperty))
         End If
         cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)

         If pInfo.PaymentName.Contains("животна средина") Then
          If vInfo.IdEngineEcoProgram > 0 Then
           Dim ecoPercent As Decimal = _
           CType(Csla.ApplicationContext.LocalContext("objVehicleEngineEcoProgramList"),  _
           VehicleEngineEcoProgramList).GetInfoById(vInfo.IdEngineEcoProgram).PercentForPayment
           cm.Parameters.AddWithValue("@Price", pInfo.Price * ecoPercent / 100)
          Else
           cm.Parameters.AddWithValue("@Price", pInfo.Price)
          End If
         Else
          If (pInfo.PaymentName.Contains("Технички преглед")) Then
           pomDelitel = delitel
          Else
           pomDelitel = 1
          End If
          If pInfo.VehicleField = "Null" Then
           'fiksno

           cm.Parameters.AddWithValue("@Price", pInfo.Price * pomDelitel)
          Else
           'presmetlivo
           cm.Parameters.AddWithValue("@Price", (pInfo.Price * CInt(CallByName(vInfo, pInfo.VehicleField, CallType.Get))) * pomDelitel)
          End If

         End If
         cm.Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
         cm.ExecuteNonQuery()

        End Using
       End If
      Else

       Using cm As SqlCommand = cn.CreateCommand
        cm.CommandType = CommandType.StoredProcedure
        cm.CommandText = "addCustomerFinancialStatFromTehnicalExam"
        cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
        cm.Parameters.AddWithValue("@IdDocumentTehnicalExam", ReadProperty(Of Long)(IdProperty))
        If Me.IdTypeOfTehnicalExam > 1 Then

         cm.Parameters.AddWithValue("@note", "по нередовен технички преглед бр." & ReadProperty(Of Long)(IdProperty))
        Else
         cm.Parameters.AddWithValue("@note", "по технички преглед бр." & ReadProperty(Of Long)(IdProperty))
        End If
        cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)

        If pInfo.PaymentName.Contains("животна средина") Then
         If vInfo.IdEngineEcoProgram > 0 Then
          Dim ecoPercent As Decimal = _
          CType(Csla.ApplicationContext.LocalContext("objVehicleEngineEcoProgramList"),  _
          VehicleEngineEcoProgramList).GetInfoById(vInfo.IdEngineEcoProgram).PercentForPayment
          cm.Parameters.AddWithValue("@Price", pInfo.Price * ecoPercent / 100)
         Else
          cm.Parameters.AddWithValue("@Price", pInfo.Price)
         End If
        Else
         If (pInfo.PaymentName.Contains("Технички преглед")) Then
          pomDelitel = delitel
         Else
          pomDelitel = 1
         End If
         If pInfo.VehicleField = "Null" Then
          'fiksno

          cm.Parameters.AddWithValue("@Price", pInfo.Price * pomDelitel)
         Else
          'presmetlivo
          cm.Parameters.AddWithValue("@Price", (pInfo.Price * CInt(CallByName(vInfo, pInfo.VehicleField, CallType.Get))) * pomDelitel)
         End If

        End If
        cm.Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
        cm.ExecuteNonQuery()

       End Using
      End If
     Else
      If pInfo.IdCommunity > 0 Then
       If idCommunityLastReg = pInfo.IdCommunity Then
        Using cm As SqlCommand = cn.CreateCommand
         cm.CommandType = CommandType.StoredProcedure
         cm.CommandText = "addCustomerFinancialStatFromTehnicalExam"
         cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
         cm.Parameters.AddWithValue("@IdDocumentTehnicalExam", ReadProperty(Of Long)(IdProperty))

         If Me.IdTypeOfTehnicalExam > 1 Then
          cm.Parameters.AddWithValue("@note", "по нередовен технички преглед бр." & ReadProperty(Of Long)(IdProperty))
         Else
          cm.Parameters.AddWithValue("@note", "по технички преглед бр." & ReadProperty(Of Long)(IdProperty))
         End If
         cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)

         'pazi tuka
         If pInfo.PaymentName.Contains("животна средина") Then
          If vInfo.IdEngineEcoProgram > 0 Then
           Dim ecoPercent As Decimal = _
           CType(Csla.ApplicationContext.LocalContext("objVehicleEngineEcoProgramList"),  _
           VehicleEngineEcoProgramList).GetInfoById(vInfo.IdEngineEcoProgram).PercentForPayment
           cm.Parameters.AddWithValue("@Price", pInfo.Price * ecoPercent / 100)
          Else
           cm.Parameters.AddWithValue("@Price", pInfo.Price)
          End If
         Else
          If (pInfo.PaymentName.Contains("Технички преглед")) Then
           pomDelitel = delitel
          Else
           pomDelitel = 1
          End If
          cm.Parameters.AddWithValue("@Price", pInfo.Price * pomDelitel)
         End If
         cm.Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
         cm.ExecuteNonQuery()
        End Using
       End If
      Else
       Using cm As SqlCommand = cn.CreateCommand
        cm.CommandType = CommandType.StoredProcedure
        cm.CommandText = "addCustomerFinancialStatFromTehnicalExam"
        cm.Parameters.AddWithValue("@IdCustomerVehicleRelation", ReadProperty(Of Long)(IdCustomerVehicleRelationProperty))
        cm.Parameters.AddWithValue("@IdDocumentTehnicalExam", ReadProperty(Of Long)(IdProperty))

        If Me.IdTypeOfTehnicalExam > 1 Then
         cm.Parameters.AddWithValue("@note", "по нередовен технички преглед бр." & ReadProperty(Of Long)(IdProperty))
        Else
         cm.Parameters.AddWithValue("@note", "по технички преглед бр." & ReadProperty(Of Long)(IdProperty))
        End If
        cm.Parameters.AddWithValue("@IdPriceCatalog", pInfo.IdPaymentParametar)

        'pazi tuka
        If pInfo.PaymentName.Contains("животна средина") Then
         If vInfo.IdEngineEcoProgram > 0 Then
          Dim ecoPercent As Decimal = _
          CType(Csla.ApplicationContext.LocalContext("objVehicleEngineEcoProgramList"),  _
          VehicleEngineEcoProgramList).GetInfoById(vInfo.IdEngineEcoProgram).PercentForPayment
          cm.Parameters.AddWithValue("@Price", pInfo.Price * ecoPercent / 100)
         Else
          cm.Parameters.AddWithValue("@Price", pInfo.Price)
         End If
        Else
         If (pInfo.PaymentName.Contains("Технички преглед")) Then
          pomDelitel = delitel
         Else
          pomDelitel = 1
         End If
         cm.Parameters.AddWithValue("@Price", pInfo.Price * pomDelitel)
        End If
        cm.Parameters.AddWithValue("@IdOrganization", CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo).Id)
        cm.ExecuteNonQuery()
       End Using
      End If
     End If
    Next

   End If
   'MsgBox("da")
  Catch ex As Exception
   MsgBox(ex.Message)
  End Try

 End Sub
#End Region

 '#Region " Reg Number "

 '    Public Shared Function GetRegNumber() As String

 '        Dim result As RegNumberCommand
 '        result = DataPortal.Execute(Of RegNumberCommand)(New RegNumberCommand())
 '        Return result.RegNumber

 '    End Function

 '    <Serializable()> _
 '    Private Class RegNumberCommand
 '        Inherits CommandBase

 '        Private _RegNumber As String

 '        Public ReadOnly Property RegNumber()
 '            Get
 '                Return _RegNumber
 '            End Get
 '        End Property

 '        Public Sub New()
 '            _RegNumber = 1
 '        End Sub

 '        Protected Overrides Sub DataPortal_Execute()
 '            Dim result As String = String.Empty

 '            Using cn As SqlConnection = Database.VTE_SqlConnection
 '                Using cm As SqlCommand = cn.CreateCommand
 '                    cm.CommandType = CommandType.Text
 '                    cm.CommandText = "declare @pom bigint set @pom = (select max(id)FROM DocumentsTehnicalExamsReports WHERE DocumentsTehnicalExamsReports.Active=1) SELECT (RegNumber)AS MAXRegNumber FROM DocumentsTehnicalExamsReports Where DocumentsTehnicalExamsReports.id = @pom"
 '                    '"SELECT MAX(RegNumber) AS MAXRegNumber FROM DocumentsTehnicalExamsReports WHERE DocumentsTehnicalExamsReports.Active=1"
 '                    Using dr As New SafeDataReader(cm.ExecuteReader)
 '                        If dr.Read() Then
 '                            If dr.GetString("MAXRegNumber") <> String.Empty Then
 '                                result = dr.GetString("MAXRegNumber") + 1
 '                                _RegNumber = result

 '                            End If
 '                        End If
 '                    End Using
 '                End Using
 '            End Using

 '        End Sub

 '    End Class

 '#End Region

#Region " Readonlylist refresh "
 Public Shared Event DocumentsTehnicalExamsReportSaved As EventHandler(Of Csla.Core.SavedEventArgs)
 Protected Shared Sub OnDocumentsTehnicalExamsReportSaved(ByVal sender As DocumentsTehnicalExamsReport, ByVal e As Csla.Core.SavedEventArgs)
  RaiseEvent DocumentsTehnicalExamsReportSaved(sender, e)
 End Sub
#End Region
 'Public Shared Sub ZatvoriValidnostNaTehnicki(ByVal inId As Long)
 '    Using cn As SqlConnection = Database.VTE_SqlConnection
 '        Using cm As SqlCommand = cn.CreateCommand()
 '            cm.CommandType = CommandType.StoredProcedure
 '            cm.CommandText = "ZatvoriPostoeckiTeh"
 '            cm.Parameters.AddWithValue("@id", inId)
 '            cm.ExecuteNonQuery()
 '        End Using
 '    End Using
 'End Sub

End Class
