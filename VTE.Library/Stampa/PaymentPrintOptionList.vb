<Serializable()> _
Public Class PaymentPrintOptionList
  Inherits NameValueListBase(Of Integer, String)

  Private Sub New()

  End Sub

  Public Shared Function GetPaymentPrintOptionList() As PaymentPrintOptionList
    Return DataPortal.Fetch(Of PaymentPrintOptionList)()
  End Function

  Private Overloads Sub DataPortal_Fetch()
    Me.RaiseListChangedEvents = False
    Me.IsReadOnly = False

    Me.Add(New NameValuePair(1, "Основна"))
    Me.Add(New NameValuePair(2, "Компактна"))

    Me.IsReadOnly = True
    Me.RaiseListChangedEvents = True
  End Sub

End Class


Public Enum PaymentPrintOption
  Osnovna = 1
  Kompaktna = 2

End Enum