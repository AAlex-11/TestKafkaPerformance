Imports Confluent.Kafka

Public Interface IKafkaProducer
    Function SendOrderRequest(Num As Integer, Topic As String, Message As String) As Task(Of Boolean)
End Interface