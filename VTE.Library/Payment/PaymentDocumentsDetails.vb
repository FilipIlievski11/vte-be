
<Serializable()> _
Public Class PaymentDocumentsDetails
  Inherits Csla.BusinessListBase(Of PaymentDocumentsDetails, PaymentDocumentsDetail)

  Public Function GetItem(ByVal id As Integer) As PaymentDocumentsDetail
    For Each ch As PaymentDocumentsDetail In Me
      If ch.Id = id Then
        Return ch
      End If
    Next
    Return Nothing
  End Function

#Region " BindingList Overrides "

  Protected Overrides Function AddNewCore() As Object
    Dim item As PaymentDocumentsDetail = PaymentDocumentsDetail.NewPaymentDocumentsDetailChild()
    Me.Add(item)
    Return item
  End Function

#End Region ' BindingList Overrides


#Region " Authorization Rules "

  Public Shared Function CanGetObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanGet("PaymentDocumentsDetails")
  End Function

  Public Shared Function CanAddObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanAdd("PaymentDocumentsDetails")
  End Function

  Public Shared Function CanEditObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanEdit("PaymentDocumentsDetails")
  End Function

  Public Shared Function CanDeleteObject() As Boolean
    Return CType(Csla.ApplicationContext.User, VTE.Library.Security.VTEPrincipal).IsInObjectPrivilegesCanDelete("PaymentDocumentsDetails")
  End Function

#End Region ' Authorization Rules

#Region " Factory Methods "

  Friend Shared Function NewPaymentDocumentsDetails() As PaymentDocumentsDetails
    If Not CanAddObject() Then
      Throw New System.Security.SecurityException("User Not authorized to add a PaymentDocumentsDetails")
    End If
    Return DataPortal.CreateChild(Of PaymentDocumentsDetails)()
  End Function

  Friend Shared Function GetPaymentDocumentsDetails(ByVal dr As SafeDataReader) As PaymentDocumentsDetails
    If Not CanGetObject() Then
      Throw New System.Security.SecurityException("User Not authorized to view a PaymentDocumentsDetails")
    End If
    Return DataPortal.FetchChild(Of PaymentDocumentsDetails)(dr)
  End Function

  Private Sub New()
    AllowNew = True
  End Sub

#End Region ' Factory Methods

#Region " Data Access "

  Private Sub Child_Fetch(ByVal dr As SafeDataReader)
    RaiseListChangedEvents = False
    Database.LogInfo("PaymentDocumentsDetails.Child_Fetch", GetHashCode())
    Try
      While dr.Read()
        Me.Add(PaymentDocumentsDetail.GetPaymentDocumentsDetail(dr))
      End While
    Catch ex As Exception
      Database.LogException("PaymentDocumentsDetails.Child_Fetch", ex)
      Throw New DbCslaException("PaymentDocumentsDetails.Child_Fetch", ex)
    End Try
    RaiseListChangedEvents = True

  End Sub


#End Region ' Data Access

  '  Private Sub PaymentDocumentsDetails_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles Me.ListChanged
  '    Dim boolReset As Boolean = False
  '    Me.RaiseListChangedEvents = False
  '    'za crven krst
  '    Dim sumaZaCrv As Double = 0
  '    Dim sumaZaCrvPomosna As Double = 0
  '    Dim sumaZaRSBSP As Double = 0
  '    Dim pomChildInZaRSBS As PaymentDocumentsDetail = Nothing
  '    Dim objCurentOptions As TehnicalExamOrganizationsInfo = _
  '    CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)
  '    Dim objPriceCatalog As PaymentCataologList = CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"), PaymentCataologList)

  '    Dim infoCrvenKrst As PaymentCataologInfo = objPriceCatalog.GetCrventKrstInfo
  '    Dim infoRSBSP As PaymentCataologInfo = objPriceCatalog.GetRSBSPInfo

  '    For Each childIn As PaymentDocumentsDetail In Me

  '      If (childIn.IdPriceCatalog <> 0) AndAlso (Not childIn.PrePayed) Then 'AndAlso '(Not childIn.PrePayed) ThenAndAlso childIn.IdPriceCatalog <> infoCrvenKrst.IdPaymentParametar
  '        If infoCrvenKrst IsNot Nothing Then
  '          Try


  '            If (objPriceCatalog.GetInfoByIdPaymentParametar(childIn.IdPriceCatalog).TrigerdByTechnicalExam) Then 'or (objPriceCatalog.GetInfoByIdPaymentParametar(childIn.IdPriceCatalog).TrigerdByRequest  Then
  '              If childIn.IdPriceCatalog <> infoCrvenKrst.IdPaymentParametar Then
  '                sumaZaCrv += childIn.Price
  '              End If
  '            Else
  '              If (objPriceCatalog.GetInfoByIdPaymentParametar(childIn.IdPriceCatalog).TrigerdByRequest) Then
  '                If childIn.IdPriceCatalog <> infoCrvenKrst.IdPaymentParametar Then
  '                  sumaZaCrvPomosna += childIn.Price
  '                End If
  '              End If

  '            End If
  '          Catch ex As Exception

  '          End Try
  '        End If
  '      End If
  '      If objCurentOptions.CalculatePercentOfTeh AndAlso childIn.IdPriceCatalog > 0 AndAlso pomChildInZaRSBS Is Nothing Then

  '        Dim info As PaymentCataologInfo = (objPriceCatalog.GetInfoByIdPaymentParametar(childIn.IdPriceCatalog))
  '        Dim name As String = objPriceCatalog.GetInfoByIdPaymentCategory(objCurentOptions.idTehnicalExam).CategoryName
  '        If info.CategoryName = name Then
  '          ' If info.IdPaymentCategory = objCurentOptions.idTehnicalExam Then
  '          If info.Price > 0 Then
  '            pomChildInZaRSBS = childIn
  '            pomChildInZaRSBS.Price = childIn.Price
  '            ' pomChildInZaRSBS.Price = info.Price
  '          End If

  '        End If
  '      End If
  '    Next
  '    If sumaZaCrv > 0 Then
  '      sumaZaCrv += sumaZaCrvPomosna
  '      Dim idDetal As Integer = 0
  '      Dim newDetal As PaymentDocumentsDetail
  '      For Each childIn As PaymentDocumentsDetail In Me
  '        If (childIn.IdPriceCatalog = infoCrvenKrst.IdPaymentParametar) Then
  '          newDetal = childIn
  '          Exit For
  '        End If
  '      Next
  '      'ako e storno ne dodavaj nov
  '      If CType(Me.Parent, PaymentDocument).Storno Then
  '        GoTo Izlez
  '      End If
  '      'dodadi nov ili izmeni
  '      If newDetal Is Nothing Then
  '        newDetal = Me.AddNew
  '        boolReset = True
  '      End If


  '      newDetal.Ddv = 0
  '      newDetal.IdPriceCatalog = infoCrvenKrst.IdPaymentParametar

  '      newDetal.Price = FicalRound(sumaZaCrv * infoCrvenKrst.Price / 100) + CType(Me.Parent, PaymentDocument).Polisa * 0.01
  '      newDetal.Note = "Автоматска пресметка"

  '    End If

  '    For Each childIn As PaymentDocumentsDetail In Me
  '      '      If (infoRSBSP IsNot Nothing) AndAlso (childIn.IdPriceCatalog = infoRSBSP.IdPaymentParametar) Then
  '      If (infoRSBSP Is Nothing) Then
  '        GoTo Izlez
  '      Else
  '        Me.RaiseListChangedEvents = False
  '        If pomChildInZaRSBS IsNot Nothing AndAlso (childIn.IdPriceCatalog = infoRSBSP.IdPaymentParametar) Then
  '          childIn.Price = FicalRound(pomChildInZaRSBS.PriceWithoutTax * objCurentOptions.PercentOfTeh / 100)
  '          pomChildInZaRSBS = Nothing
  '          GoTo Izlez
  '        End If
  '      End If
  '    Next
  '    If pomChildInZaRSBS IsNot Nothing AndAlso pomChildInZaRSBS.Price > 0 Then

  '      Dim newDetal2 As PaymentDocumentsDetail
  '      newDetal2 = Me.AddNew
  '      newDetal2.IdPriceCatalog = infoRSBSP.IdPaymentParametar
  '      newDetal2.Price = FicalRound(pomChildInZaRSBS.PriceWithoutTax * objCurentOptions.PercentOfTeh / 100)
  '      newDetal2.Ddv = infoRSBSP.DDVValue
  '    End If
  'Izlez:

  '    Me.RaiseListChangedEvents = True
  '    If boolReset Then
  '      Me.ResetBindings()
  '    End If




  '  End Sub
  Private Sub PaymentDocumentsDetails_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles Me.ListChanged
    Dim boolReset As Boolean = False
    Me.RaiseListChangedEvents = False
        'za crven krst
        Dim sumadetail As Double = 0
    Dim sumaZaCrv As Double = 0
    Dim sumaZaCrvPomosna As Double = 0
        Dim sumaZaRSBSP As Double = 0
    Dim pomChildInZaRSBS As PaymentDocumentsDetail = Nothing
    Dim objCurentOptions As TehnicalExamOrganizationsInfo = _
    CType(Csla.ApplicationContext.LocalContext.Item("objCurentTehExamOrganization"), TehnicalExamOrganizationsInfo)
    Dim objPriceCatalog As PaymentCataologList = CType(Csla.ApplicationContext.LocalContext("objPaymentCatalogList"), PaymentCataologList)

    Dim infoCrvenKrst As PaymentCataologInfo = objPriceCatalog.GetCrventKrstInfo
    Dim infoRSBSP As PaymentCataologInfo = objPriceCatalog.GetRSBSPInfo

    For Each childIn As PaymentDocumentsDetail In Me
            If (childIn.IdPriceCatalog <> 0) AndAlso (Not childIn.PrePayed) Then 'AndAlso '(Not childIn.PrePayed) ThenAndAlso childIn.IdPriceCatalog <> infoCrvenKrst.IdPaymentParametar
                If infoCrvenKrst IsNot Nothing Then
                    Try
                        If (objPriceCatalog.GetInfoByIdPaymentParametar(childIn.IdPriceCatalog).TrigerdByTechnicalExam) Then 'or (objPriceCatalog.GetInfoByIdPaymentParametar(childIn.IdPriceCatalog).TrigerdByRequest  Then
                            If childIn.IdPriceCatalog <> infoCrvenKrst.IdPaymentParametar Then
                                sumaZaCrv += childIn.Price
                            End If
                        Else
                            If (objPriceCatalog.GetInfoByIdPaymentParametar(childIn.IdPriceCatalog).TrigerdByRequest) Then
                                If childIn.IdPriceCatalog <> infoCrvenKrst.IdPaymentParametar Then
                                    sumaZaCrvPomosna += childIn.Price
                                End If
                            End If

                        End If
                    Catch ex As Exception

                    End Try
                End If
            End If
            If objCurentOptions.CalculatePercentOfTeh AndAlso childIn.IdPriceCatalog > 0 Then 'AndAlso pomChildInZaRSBS Is Nothing Then
                Dim info As PaymentCataologInfo = (objPriceCatalog.GetInfoByIdPaymentParametar(childIn.IdPriceCatalog))
                Dim name As String = objPriceCatalog.GetInfoByIdPaymentCategory(objCurentOptions.idTehnicalExam).CategoryName
                If info.Price > 0 AndAlso info.CategoryName.Contains("јавни патишта") Then
                    sumadetail = info.Price
                End If
                If info.CategoryName = name AndAlso info.Price > 0 Then
                    ' sumaZaRSBSP += (info.Price * (1 - info.DDVValue / 100) * objCurentOptions.PercentOfTeh / 100)
                    sumaZaRSBSP += FicalRound(DDVPresmetki.DanocnaOsnovica(info.Price, info.DDVValue, 1)) * objCurentOptions.PercentOfTeh / 100

                    'Else
                    '            If info.Price > 0 AndAlso info.CategoryName.Contains("јавни патишта") Then
                    '                sumaZaRSBSP += (info.Price * (1 - info.DDVValue / 100)) * 1 / 100
                    '            End If
                End If
            End If
        Next
    If sumaZaCrv > 0 Then
      sumaZaCrv += sumaZaCrvPomosna
      Dim idDetal As Integer = 0
      Dim newDetal As PaymentDocumentsDetail
      For Each childIn As PaymentDocumentsDetail In Me
        If (childIn.IdPriceCatalog = infoCrvenKrst.IdPaymentParametar) Then
          newDetal = childIn
          Exit For
        End If
      Next
      'ako e storno ne dodavaj nov
      If CType(Me.Parent, PaymentDocument).Storno Then
        GoTo Izlez
      End If
      'dodadi nov ili izmeni
      If newDetal Is Nothing Then
        newDetal = Me.AddNew
        boolReset = True
      End If
      newDetal.Ddv = 0
      newDetal.IdPriceCatalog = infoCrvenKrst.IdPaymentParametar
            newDetal.Price = FicalRound(sumaZaCrv * infoCrvenKrst.Price / 100) + CType(Me.Parent, PaymentDocument).Polisa * 0.01    
            newDetal.Price += sumadetail
            newDetal.Note = "Автоматска пресметка"
    End If

    'For Each childIn As PaymentDocumentsDetail In Me
    '  '      If (infoRSBSP IsNot Nothing) AndAlso (childIn.IdPriceCatalog = infoRSBSP.IdPaymentParametar) Then
    '  If (infoRSBSP Is Nothing) Then
    '    GoTo Izlez
    '  Else
    '    Me.RaiseListChangedEvents = False
    '    If pomChildInZaRSBS IsNot Nothing AndAlso (childIn.IdPriceCatalog = infoRSBSP.IdPaymentParametar) Then
    '      childIn.Price = FicalRound(pomChildInZaRSBS.PriceWithoutTax * objCurentOptions.PercentOfTeh / 100)
    '      pomChildInZaRSBS = Nothing
    '      GoTo Izlez
    '    End If
    '  End If
    'Next
    'If pomChildInZaRSBS IsNot Nothing AndAlso pomChildInZaRSBS.Price > 0 Then
    '  Dim newDetal2 As PaymentDocumentsDetail
    '  newDetal2 = Me.AddNew
    '  newDetal2.IdPriceCatalog = infoRSBSP.IdPaymentParametar
    '  newDetal2.Price = FicalRound(pomChildInZaRSBS.PriceWithoutTax * objCurentOptions.PercentOfTeh / 100)
    '  newDetal2.Ddv = infoRSBSP.DDVValue
    'End If
    If sumaZaRSBSP > 0 Then
      Dim idDetal As Integer = 0
      Dim newDetal2 As PaymentDocumentsDetail
      For Each childIn As PaymentDocumentsDetail In Me
        If (childIn.IdPriceCatalog = infoRSBSP.IdPaymentParametar) Then
          newDetal2 = childIn
          Exit For
        End If
      Next
      'ako e storno ne dodavaj nov
      If CType(Me.Parent, PaymentDocument).Storno Then
        GoTo Izlez
      End If
      'dodadi nov ili izmeni
      If newDetal2 Is Nothing Then
        newDetal2 = Me.AddNew
        boolReset = True
      End If
      newDetal2.Ddv = 0
      newDetal2.IdPriceCatalog = infoRSBSP.IdPaymentParametar
      newDetal2.Price = FicalRound(sumaZaRSBSP)

    End If
Izlez:
    Me.RaiseListChangedEvents = True
    If boolReset Then
      Me.ResetBindings()
    End If

  End Sub
End Class
