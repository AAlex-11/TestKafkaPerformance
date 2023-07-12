Imports Microsoft

Public Class RandomString
    Shared RND As Random = New Random()
    Const Alf As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
    Public Shared Function GetRandomString(Len As Integer) As String
        Return New String(Enumerable.Repeat(Alf, Len).Select(Function(s) s(Random.Shared.Next(s.Length))).ToArray())
    End Function

End Class
