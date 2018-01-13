using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;

namespace PartialFoods.Services.InventoryServer
{

    public class KafkaReleasedConsumer
    {
        private string topic;
        private Dictionary<string, object> config;
        private InventoryReleasedEventProcessor eventProcessor;

        public KafkaReleasedConsumer(string topic, Dictionary<string, object> config, InventoryReleasedEventProcessor eventProcessor)
        {
            this.topic = topic;
            this.config = config;
            this.eventProcessor = eventProcessor;
        }

        public void Consume()
        {
            Task.Run(() =>
            {
                Console.WriteLine($"Starting Kafka subscription to {topic}");
                using (var consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8)))
                {
                    //consumer.Assign(new List<TopicPartitionOffset> { new TopicPartitionOffset(topic, 0, 0) });
                    consumer.Subscribe(new[] { topic });

                    while (true)
                    {
                        Message<Null, string> msg;
                        if (consumer.Consume(out msg, TimeSpan.FromSeconds(1)))
                        {
                            string rawJson = msg.Value;
                            try
                            {
                                InventoryReleasedEvent evt = JsonConvert.DeserializeObject<InventoryReleasedEvent>(rawJson);
                                eventProcessor.HandleInventoryReleasedEvent(evt);
                                var committedOffsets = consumer.CommitAsync(msg).Result;
                                if (committedOffsets.Error.HasError)
                                {
                                    Console.WriteLine($"Failed to commit offsets : {committedOffsets.Error.Reason}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                                Console.WriteLine($"Failed to handle inventory released event : ${ex.ToString()}");
                            }
                        }
                    }
                }
            });
        }
    }
}