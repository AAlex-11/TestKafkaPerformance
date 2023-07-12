Imports System
Imports System.Threading
Imports Confluent.Kafka
Imports Confluent.Kafka.ConfigPropertyNames
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging


Module Program
    Sub Main(args As String())
        Dim Num As Integer
        'setup our DI
        Dim ServiceProvider = New ServiceCollection().
            AddLogging(Function(loggingBuilder) loggingBuilder.ClearProviders().AddConsole().SetMinimumLevel(LogLevel.Debug)).
            AddSingleton(Of IKafkaProducer, KafkaProducer)().
            BuildServiceProvider()
        'ref to console logging
        Dim Logger = ServiceProvider.GetService(Of ILoggerFactory)().CreateLogger(Of IKafkaProducer)
        Logger.LogDebug($"Start.")
        'ref to Kafka producer
        Dim KProducer = ServiceProvider.GetService(Of IKafkaProducer)
        For i As Integer = 0 To 100
            Interlocked.Increment(Num)
            Dim X = New Threading.Thread(Sub()
                                             KProducer.SendOrderRequest(Num, "TstTopic", $"{RandomString.GetRandomString(100000)}")
                                         End Sub)
            X.Start()
            'KProducer.SendOrderRequest("TstTopic", $"{RandomString.GetRandomString(100000)}")
        Next
        Logger.LogDebug("Finish.")
    End Sub

End Module
