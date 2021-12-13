﻿using Confluent.Kafka.Admin;
using Confluent.Kafka;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace TimeOff.Core
{
    public static class KafkaHelper
    {
        public static async Task CreateTopicAsync(AdminClientConfig config, string topicName, int partitionCount)
        {
            using var adminClient = new AdminClientBuilder(config).Build();
            try
            {
                await adminClient.CreateTopicsAsync(new[]
                {
                    new TopicSpecification
                    {
                        Name = topicName,
                        ReplicationFactor = 1,
                        NumPartitions = partitionCount
                    }
                });
            }
            catch (CreateTopicsException e) when (e.Results.Select(r => r.Error.Code)
                                                      .Any(el => el == ErrorCode.TopicAlreadyExists))
            {
                Console.WriteLine($"Topic {e.Results[0].Topic} already exists");
            }
        }
    }
}