using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SolaERP.Application.Dtos;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;
using SolaERP.Application.UnitOfWork;
using SolaERP.Job.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailTemplateKey = SolaERP.Job.Enums.EmailTemplateKey;
using MailTopic = SolaERP.Job.Enums.MailTopic;

namespace SolaERP.Job
{
    public class BackgroundMailService : IBackgroundMailService
    {
        private readonly IConfiguration _configuration;
        public BackgroundMailService()
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
        }
        public async Task SendMailAsync(HashSet<RowInfo> rowInfos, Person person)
        {
            try
            {
                MailInfo emailModel = new MailInfo
                {
                    link = _configuration["Mail:ServerUrlUI"],
                    emailTemplateKey = EmailTemplateKey.ALL_TYPES_ALL_STATUSES.ToString(),
                    rowInfos = rowInfos,
                    referenceNo = "",
                    companyName = "Apertech",
                    persons = new List<Person> { person }
                };

                string topicName = "prod-mail-topic";

                var config = new ProducerConfig { BootstrapServers = _configuration["Mail:KafkaUrl"] };

                using (var producer = new ProducerBuilder<string, string>(config).Build())
                {
                    try
                    {
                        string jsonString = JsonConvert.SerializeObject(emailModel);
                        await producer.ProduceAsync(topicName, new Message<string, string> { Value = jsonString });

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to deliver message: {ex.Message} ");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error");
            }

        }

    }
}
