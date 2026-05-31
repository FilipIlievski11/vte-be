Public Class NumberToText


    Public Shared Function ConvertNum(ByVal Input As Long) As String 'Call this function passing the number you desire to be changed
        Dim output As String = Nothing
        If Input < 1000 Then
            output = FindNumber(Input) 'if its less than 1000 then just look it up
        Else
            Dim nparts() As String 'used to break the number up into 3 digit parts
            Dim n As String = Input 'string version of the number
            Dim i As Long = Input.ToString.Length 'length of the string to help break it up

            Do Until i - 3 <= 0
                n = n.Insert(i - 3, ",") 'insert commas to use as splitters
                i = i - 3 'this insures that we get the correct number of parts
            Loop
            nparts = n.Split(",") 'split the string into the array

            i = Input.ToString.Length 'return i to initial value for reuse
            Dim p As Integer = 0 'p for parts, used for finding correct suffix
            For Each s As String In nparts
                Dim x As Long = CLng(s) 'x is used to compare the part value to other values
                p = p + 1
                If p = nparts.Length Then 'if p = number of elements in the array then we need to do something different
                    If x <> 0 Then
                        If CLng(s) < 100 Then
                            output = output & " и " & FindNumber(CLng(s)) ' look up the number, no suffix 
                        Else                                                ' required as this is the last part
                            output = output & " " & FindNumber(CLng(s))
                        End If
                    End If
                Else 'if its not the last element in the array
                    If x <> 0 Then
                        If output = Nothing Then 'we have to check this so we don't add a leading space
                            If FindNumber(CLng(s)) = "еден" AndAlso FindSuffix(i, CLng(s)) = "илјади" Then
                                output = output & "илјада"
                            Else
                                output = output & FindNumber(CLng(s)) & " " & FindSuffix(i, CLng(s)) 'look up the number and suffix
                            End If


                        Else 'spaces must go in the right place
                            output = output & " " & FindNumber(CLng(s)) & " " & FindSuffix(i, CLng(s)) 'look up the snumber and suffix
                        End If
                    End If
                End If
                i = i - 3 'reduce the suffix counter by 3 to step down to the next suffix
            Next
        End If
        Return output
    End Function

    Private Shared Function FindNumber(ByVal Number As Long) As String
        Dim Words As String = Nothing
        Dim Digits() As String = {"нула", "еден", "две", "три", "четири", "пет", "шест", "седум", _
      "осум", "девет", "десет"}
        Dim Teens() As String = {"", "единаесет", "дванаесет", "тринаесет", "четрнаесет", "петнаесет", "шеснаесет", "седумнаесет", _
       "осумнаесет", "деветнаесет"}

        If Number < 11 Then
            Words = Digits(Number)

        ElseIf Number < 20 Then
            Words = Teens(Number - 10)

        ElseIf Number = 20 Then
            Words = "дваесет"

        ElseIf Number < 30 Then
            Words = "дваесет и " & Digits(Number - 20)

        ElseIf Number = 30 Then
            Words = "триесет"

        ElseIf Number < 40 Then
            Words = "триесет и " & Digits(Number - 30)

        ElseIf Number = 40 Then
            Words = "четириесет"

        ElseIf Number < 50 Then
            Words = "четириесет и " & Digits(Number - 40)

        ElseIf Number = 50 Then
            Words = "педесет"

        ElseIf Number < 60 Then
            Words = "педесет и " & Digits(Number - 50)

        ElseIf Number = 60 Then
            Words = "шеесет"

        ElseIf Number < 70 Then
            Words = "шеесет и " & Digits(Number - 60)

        ElseIf Number = 70 Then
            Words = "седумдесет"

        ElseIf Number < 80 Then
            Words = "седумдесет и " & Digits(Number - 70)

        ElseIf Number = 80 Then
            Words = "осумдесет"

        ElseIf Number < 90 Then
            Words = "осумдесет и " & Digits(Number - 80)

        ElseIf Number = 90 Then
            Words = "деведесет"

        ElseIf Number < 100 Then
            Words = "деведесет и " & Digits(Number - 90)

        ElseIf Number = 100 Then
            Words = "сто "

        ElseIf Number > 100 AndAlso Number < 200 Then
            Words = "сто " & FindNumber(Number - 100)

       
        ElseIf Number < 1000 Then
            Words = Number.ToString
            Words = Words.Insert(1, ",")
            Dim wparts As String() = Words.Split(",")
            Words = FindNumber(wparts(0)) & "стотини"
            Dim n As String = FindNumber(wparts(1))
            If CLng(wparts(1)) <> 0 Then
                Words = Words & " " & n
            End If
        End If

        Return Words
    End Function

    Private Shared Function FindSuffix(ByVal Length As Long, ByVal l As Long) As String
        Dim word As String

        If l <> 0 Then
            If Length > 12 Then
                word = "трилиони"
            ElseIf Length > 9 Then
                word = "билиони"
            ElseIf Length > 6 Then
                word = "милиони"
            ElseIf Length > 3 Then
                word = "илјади"
            ElseIf Length > 2 Then
                word = "стотини"
            Else
                word = ""
            End If
        Else
            word = ""
        End If

        Return word
    End Function

End Class
