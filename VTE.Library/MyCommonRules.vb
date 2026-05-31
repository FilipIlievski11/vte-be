Module MyCommonRules
  Public Function ForeignIdSelect(ByVal target As Object, ByVal e As Validation.RuleArgs) As Boolean
    Dim value As Integer = CInt(CallByName(target, e.PropertyName, CallType.Get))
    Dim args As Csla.Validation.DecoratedRuleArgs = DirectCast(e, Csla.Validation.DecoratedRuleArgs)
    Dim min As Integer = CInt(args("MinValue"))
    If value >= min Then
      Return True

    Else
      e.Description = String.Format("Please select {0} from the list", e.PropertyName)
      Return False
    End If
  End Function
End Module
