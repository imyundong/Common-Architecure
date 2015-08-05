Imports ServerPlatform.Library.Data
Imports ServerPlatform.Library.Utility
Imports System.Xml.Serialization

<Xml.Serialization.XmlRoot("AppConfig")>
Public Class CTaskScheduleConfig
    Inherits ServerPlatform.Library.Data.CApplicationConfig
    <XmlArrayItem("TaskGroup")>
    Property Tasks As New List(Of CTaskGroup)

    Property Source As String = "Data"
    Property Database As CDatabaseInfo

    Public Shared INVALID_DATETIME As Date = New DateTime(0)

    Public Enum CTaskStatus
        Stopped = 0
        Running = 1
    End Enum

    Public Class CTaskGroup
        <XmlElement("Task")>
        Property Tasks As New List(Of CTaskInfo)
        <Xml.Serialization.XmlAttribute()>
        Property Id As String = ""


        <Xml.Serialization.XmlAttribute()>
        Property Frequency As String
        ' Fixed:yyyy,mm,dd,HH,MM
        ' Interval: 30
        ' FixInterval : 30

        <XmlIgnore>
        Public Property NextRuntime As DateTime = INVALID_DATETIME
        <XmlIgnore>
        Public Property TaskFrequency As CTaskFrequency
        <XmlIgnore>
        Public Property TaskStatus As CTaskStatus = CTaskStatus.Stopped
        <XmlIgnore>
        Public Property LastRuntime As DateTime


        Public Function GetTaskFrequency() As CTaskFrequency
            If String.IsNullOrEmpty(Frequency) Then Return Nothing

            Dim Str() As String = Frequency.Split(":"c)
            If Str.Length <> 2 Then Return Nothing

            If Str(0).ToUpper = "INTERVAL" Then
                Dim Interval As Integer = 1
                Integer.TryParse(Str(1), Interval)

                If Interval <= 0 Then Return Nothing
                Return New CTaskFrequencyInterval(Interval)
            ElseIf Str(0).ToUpper = "FIXEDINTERVAL" Then
                Dim Interval As Integer = 1
                Integer.TryParse(Str(1), Interval)

                If Interval <= 0 Then Return Nothing
                Return New CTaskFrequencyFiexedInterval(Interval)
            ElseIf Str(0).ToUpper = "FIXED" Then
                Dim TempDate As String() = Str(1).Split(","c)
                If TempDate.Length <> 5 Then
                    Return Nothing
                Else
                    Dim TaskFrequencyFixed As New CTaskFrequencyFixed _
                        (TempDate(0), TempDate(1), TempDate(2), TempDate(3), TempDate(4))
                    If TaskFrequencyFixed.IsValid = False Then
                        Return Nothing
                    Else
                        Return TaskFrequencyFixed
                    End If
                End If
            Else
                Return Nothing
            End If
        End Function

        <Xml.Serialization.XmlAttribute()>
        Property Repeat As CTaskRepeat = CTaskRepeat.Repeat
    End Class
    Public Class CTaskInfo
        <Xml.Serialization.XmlAttribute()>
        Property TaskId As String

    End Class
    Public Enum CTaskRepeat As Integer
        Repeat = 0
        Once = 1
    End Enum

    Public MustInherit Class CTaskFrequency
        ' Frequency Category
        Public MustOverride ReadOnly Property Category As CFrequencyCategory
        ' IsValid
        Public MustOverride Property IsValid As Boolean
        ' GetNextRuntime
        Public MustOverride Function GetNextRuntime(ByVal CurrentDate As DateTime) As DateTime


        Public Shared Function GetTaskFrequency(ByVal Frequency As String) As CTaskFrequency
            Dim TempString As String() = Frequency.Split(":"c)

            If TempString.Length <> 2 Then
                Return Nothing
            End If

            If TempString(0).ToUpper = "INTERVAL" Then
                Dim Interval As Integer = 1
                Integer.TryParse(TempString(1), Interval)

                If Interval <= 0 Then Return Nothing
                Return New CTaskFrequencyInterval(Interval)
            ElseIf TempString(0).ToUpper = "FIXED" Then
                Dim TempDate As String() = TempString(1).Split(","c)
                If TempDate.Length <> 5 Then
                    Return Nothing
                Else
                    Dim TaskFrequencyFixed As New CTaskFrequencyFixed _
                        (TempDate(0), TempDate(1), TempDate(2), TempDate(3), TempDate(4))
                    If TaskFrequencyFixed.IsValid = False Then
                        Return Nothing
                    Else
                        Return TaskFrequencyFixed
                    End If
                End If
            Else
                Return Nothing
            End If

        End Function

        Public Enum CFrequencyCategory
            Interval = 0
            Fixed = 1
            FixedInterval = 2
        End Enum
    End Class

    Public Class CTaskFrequencyFixed
        Inherits CTaskFrequency

        ' Seconds
        Public Property Year As String = "*"
        Public Property Month As String = "*"
        Public Property Day As String = "*"
        Public Property Hour As String = "*"
        Public Property Minute As String = "*"

        Public Sub New(ByVal Year As String, ByVal Month As String, _
                       ByVal Day As String, ByVal Hour As String, ByVal Minute As String)

            Dim TempInt As Integer = 0
            ' Year
            If String.IsNullOrEmpty(Year) OrElse Year = "*" Then
                Me.Year = "*"
            ElseIf Integer.TryParse(Year, TempInt) Then
                Year = TempInt
            Else
                IsValid = False
                Exit Sub
            End If

            ' Month
            If String.IsNullOrEmpty(Month) OrElse Month = "*" Then
                Me.Month = "*"
            ElseIf Integer.TryParse(Month, TempInt) Then
                Month = TempInt
            Else
                IsValid = False
                Exit Sub
            End If

            ' Day
            If String.IsNullOrEmpty(Day) OrElse Day = "*" Then
                Me.Day = "*"
            ElseIf Integer.TryParse(Day, TempInt) Then
                Day = TempInt
            Else
                IsValid = False
                Exit Sub
            End If

            ' Hour
            If String.IsNullOrEmpty(Hour) OrElse Hour = "*" Then
                Me.Hour = "*"
            ElseIf Integer.TryParse(Hour, TempInt) Then
                Hour = TempInt
            Else
                IsValid = False
                Exit Sub
            End If

            ' Minute
            If String.IsNullOrEmpty(Minute) OrElse Minute = "*" Then
                Me.Minute = "*"
            ElseIf Integer.TryParse(Minute, TempInt) Then
                Minute = TempInt
            Else
                IsValid = False
                Exit Sub
            End If

            Dim TempYear As String
            If Me.Year = "*" Then TempYear = 1990 Else TempYear = Me.Year
            Dim TempMonth As String
            If Me.Month = "*" Then TempMonth = 1 Else TempMonth = Me.Month
            Dim TempDay As String
            If Me.Day = "*" Then TempDay = 1 Else TempDay = Me.Day
            Dim TempHour As String
            If Me.Hour = "*" Then TempHour = 0 Else TempHour = Me.Hour
            Dim TempMinute As String
            If Me.Minute = "*" Then TempMinute = 0 Else TempMinute = Me.Minute

            Dim TempDate As DateTime
            If IsValid = DateTime.TryParse(TempYear & "-" & TempMonth & "-" & TempDay & " " & _
                              TempHour & ":" & TempMinute & ":0", TempDate) Then
            End If

        End Sub

        Public Overrides Property IsValid As Boolean = True

        Public Overrides ReadOnly Property Category As CTaskFrequency.CFrequencyCategory
            Get
                Return CFrequencyCategory.Fixed
            End Get
        End Property

        Public Overrides Function GetNextRuntime(CurrentDate As Date) As Date
            If IsValid = False Then
                Return INVALID_DATETIME
            End If

            Dim NextRuntime As DateTime
            Dim TempDate As DateTime = New Date(CurrentDate.Year, CurrentDate.Month, _
                                                CurrentDate.Day, CurrentDate.Hour, CurrentDate.Minute, 0)

            Dim TempYear As Integer
            If Year = "*" Then TempYear = 0 Else TempYear = Year
            Dim TempMonth As Integer
            If Month = "*" Then TempMonth = 1 Else TempMonth = Month
            Dim TempDay As Integer
            If Day = "*" Then TempDay = 1 Else TempDay = Day
            Dim TempHour As Integer
            If Hour = "*" Then TempHour = 0 Else TempHour = Hour
            Dim TempMinute As Integer
            If Minute = "*" Then TempMinute = 0 Else TempMinute = Minute

            NextRuntime = New Date(TempYear, TempMonth, TempDay, TempHour, TempMinute, 0)

            ' Initial
            If Year = "*" Then
                If Month = "*" Then
                    If Day = "*" Then
                        If Hour = "*" Then
                            If Minute = "*" Then
                                Return CurrentDate.AddMinutes(1)
                            Else
                                If Minute > CurrentDate.Minute Then
                                    Return New Date(CurrentDate.Year, CurrentDate.Month, _
                                                    CurrentDate.Day, CurrentDate.Hour, Minute, 0)
                                ElseIf Minute < CurrentDate.Minute Then
                                    Return AddMinute(New Date _
                                                   (CurrentDate.Year, CurrentDate.Month, _
                                                    CurrentDate.Day, CurrentDate.Hour, Minute, 0))
                                Else
                                    Return AddMinute(CurrentDate)
                                End If
                            End If
                        Else
                            If Hour > CurrentDate.Hour Then
                                Return New Date(CurrentDate.Year, CurrentDate.Month, _
                                                CurrentDate.Day, Hour, TempMinute, 0)
                            ElseIf Hour < CurrentDate.Hour Then
                                Return AddDay(New Date _
                                               (CurrentDate.Year, CurrentDate.Month, _
                                                CurrentDate.Day, Hour, TempMinute, 0))
                            Else
                                Return AddMinute(CurrentDate)
                            End If
                        End If
                    Else

                        If Day > CurrentDate.Day Then
                            Return New Date(CurrentDate.Year, _
                                            CurrentDate.Month, Day, TempHour, TempMinute, 0)
                        ElseIf Day < CurrentDate.Day Then
                            Return AddMonth(New Date _
                                           (CurrentDate.Year, _
                                            CurrentDate.Month, Day, TempHour, TempMinute, 0))
                        Else
                            Return AddMinute(CurrentDate)
                        End If
                    End If
                Else
                    If Month > CurrentDate.Month Then
                        Return New Date(CurrentDate.Year, Month, TempDay, TempHour, TempMinute, 0)
                    ElseIf Month < CurrentDate.Month Then
                        Return AddYear(New Date _
                                       (CurrentDate.Year, Month, TempDay, TempHour, TempMinute, 0))
                    Else
                        Return AddMinute(CurrentDate)
                    End If
                End If
            Else
                If Year > CurrentDate.Year Then
                    Return New Date(Year, TempMonth, TempDay, TempHour, TempMinute, 0)
                ElseIf Year < CurrentDate.Year Then
                    Return Nothing
                Else
                    Return AddMinute(CurrentDate)
                End If
            End If

            Return NextRuntime
        End Function

        Private Function AddMinute(ByRef CurrentDate As DateTime) As DateTime
            If Minute = "*" And CurrentDate.Minute < 59 Then
                Return CurrentDate.AddMinutes(1)
            Else
                Return AddHour(CurrentDate)
            End If
        End Function

        Private Function AddHour(ByRef CurrentDate As DateTime) As DateTime
            If Hour = "*" And CurrentDate.Hour < 23 Then
                Return CurrentDate.AddHours(1)
            Else
                Return AddDay(CurrentDate)
            End If
        End Function

        Private Function AddDay(ByRef CurrentDate As DateTime) As DateTime
            If Day = "*" And CurrentDate.AddDays(1).Month = CurrentDate.Month Then
                Return CurrentDate.AddDays(1)
            ElseIf Day = "*" Then
                CurrentDate = CurrentDate.AddDays(-CurrentDate.Day + 1)
                Return AddMonth(CurrentDate)
            Else
                Return AddMonth(CurrentDate)
            End If
        End Function

        Private Function AddMonth(ByRef CurrentDate As DateTime) As DateTime
            If Month = "*" And CurrentDate.Month < 12 Then
                Return CurrentDate.AddMonths(1)
            Else
                Return AddYear(CurrentDate)
            End If
        End Function

        Private Function AddYear(ByRef CurrentDate As DateTime) As DateTime
            If Year <> "*" Then
                Return INVALID_DATETIME
            End If
            Return CurrentDate.AddYears(1)
        End Function

    End Class

    Public Class CTaskFrequencyInterval
        Inherits CTaskFrequency
        ' Seconds
        Public Property Interval As Integer = 0

        Public Sub New(ByVal Interval As Integer)
            Me.Interval = Interval
            If Interval <= 0 Then IsValid = False
        End Sub

        Public Overrides Property IsValid As Boolean = True

        Public Overrides ReadOnly Property Category As CTaskFrequency.CFrequencyCategory
            Get
                Return CFrequencyCategory.Interval
            End Get
        End Property

        Public Overrides Function GetNextRuntime(CurrentDate As Date) As DateTime
            If IsValid = False Then
                Return INVALID_DATETIME
            End If

            Dim TempDate As New DateTime(CurrentDate.Year, CurrentDate.Month, _
                                         CurrentDate.Day, CurrentDate.Hour, _
                                         CurrentDate.Minute, CurrentDate.Second)
            TempDate = TempDate.AddSeconds(Interval)

            Return TempDate
        End Function
    End Class

    Public Class CTaskFrequencyFiexedInterval
        Inherits CTaskFrequencyInterval

        Sub New(ByVal Interval As Integer)
            MyBase.New(Interval)
        End Sub

        Public Overrides ReadOnly Property Category As CTaskFrequency.CFrequencyCategory
            Get
                Return CFrequencyCategory.FixedInterval
            End Get
        End Property

    End Class

End Class
