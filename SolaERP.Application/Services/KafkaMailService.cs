using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class KafkaMailService : IKafkaMailService
    {
        private readonly IConfiguration _configuration;
        public KafkaMailService()
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
        }

        public async Task SendMail(EmailTemplate emailTemplateKey, ApproveStatus approveStatus, List<Person> persons, int sequence, string referenceNo)
        {
            KafkaEmail emailModel = new KafkaEmail
            {
                ApproveStatus = approveStatus,
                Link = _configuration["Mail:ServerUrlUI"],
                EmailTemplateKey = emailTemplateKey.ToString(),
                Persons = persons,
                Sequence = sequence,
                ReferenceNo = referenceNo,
                CompanyName = "Apertech"
            };

            string topicName = MailTopic.commerceTopic.ToString();

            var config = new ProducerConfig { BootstrapServers = "38.242.216.187:9092" };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                try
                {
                    string jsonString = JsonConvert.SerializeObject(emailModel);
                    var deliveryResult = await producer.ProduceAsync(topicName, new Message<string, string> { Value = jsonString });
                }
                catch (ProduceException<string, string> ex)
                {
                    Console.WriteLine($"Failed to deliver message: {ex.Message} [{ex.Error.Code}]");
                }
            }
        }

    }
}
