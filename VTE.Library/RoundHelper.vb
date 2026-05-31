Module RoundHelper
  Public Function FicalRound(ByVal value As Double) As Double
    Select Case Math.Round(value - Math.Truncate(value), 2)
            'Case 0
            '  Return Math.Truncate(value)
            '  'vrati go celobrojniot pogolem
            'Case 0.25 To 0.75
            '  Return Math.Truncate(value) + 0.5
            'Case Is > 0.75
            '  Return Math.Truncate(value) + 1
            'Case Is < 0.25
            '          Return Math.Truncate(value)
            Case 0 To 0.49
                Return Math.Truncate(value)
            Case Is > 0.49
                Return Math.Truncate(value) + 1
        End Select
  End Function
End Module
