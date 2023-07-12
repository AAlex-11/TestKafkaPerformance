Imports System.Net
Imports System.Text.Json
Imports System.Threading
Imports Confluent.Kafka
Imports Microsoft.Extensions.Logging

Public Class KafkaProducer
    Implements IKafkaProducer

    Private ReadOnly KafkaServer As String = "10.8.0.1"
    Private ReadOnly _Logger As ILogger(Of KafkaProducer)
    Private ReadOnly KafkaConfig As ProducerConfig
    Private ReadOnly Producer As IProducer(Of Confluent.Kafka.Null, String)
    Public Sub New(logger As ILogger(Of KafkaProducer))
        _Logger = logger
        KafkaConfig = New ProducerConfig With {
                                       .BootstrapServers = KafkaServer
                                       }
        Producer = New ProducerBuilder(Of Confluent.Kafka.Null, String)(KafkaConfig).Build()
    End Sub

    Public Async Function SendOrderRequest(Num As Integer, Topic As String, Message As String) As Task(Of Boolean) Implements IKafkaProducer.SendOrderRequest
        _Logger.LogInformation(Now)
        Try
            Dim Result As Task(Of DeliveryResult(Of Null, String)) = Producer.ProduceAsync(Topic, New Message(Of Null, String) With {
                        .Value = JsonSerializer.Serialize(New With {.Num = Num, .Txt = Message})
                    })
            Dim Tsk1 = Result.GetAwaiter
            Tsk1.OnCompleted(Sub() _Logger.LogInformation($"Delivery Timestamp: {Result.Result.Timestamp.UtcDateTime}, offset {Result.Result.Offset}, Num {Num}"))
            Producer.Flush()
            Await Result
            Return True

        Catch ex As Exception
            _Logger.LogInformation($"Error occured: {ex.Message}")
        End Try
        Return False
    End Function
End Class




