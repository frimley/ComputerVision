using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace SuperSight.ServiceAggregator
{
    class Config
    {
        public static string RabbitMQServer { get { return GetAppKeyValue("RabbitMQServer"); } }
        public static int RabbitMQPort { get { return Convert.ToInt32(GetAppKeyValue("RabbitMQPort")); } }
        public static string RabbitMQUser { get { return GetAppKeyValue("RabbitMQUser"); } }
        public static string RabbitMQPassword { get { return GetAppKeyValue("RabbitMQPassword"); } }
        public static string RabbitMQVirtualHost { get { return GetAppKeyValue("RabbitMQVirtualHost"); } }

        public static string RabbitMQQueueFrameSource { get { return GetAppKeyValue("RabbitMQQueueFrameSource"); } }
        public static string RabbitMQQueueFrameFaceAgeGender { get { return GetAppKeyValue("RabbitMQQueueFrameFaceAgeGender"); } }
        public static string RabbitMQQueueFrameDogBreed { get { return GetAppKeyValue("RabbitMQQueueFrameDogBreed"); } }
        public static string RabbitMQExchangeFrameComplete { get { return GetAppKeyValue("RabbitMQExchangeFrameComplete"); } }

        private static string GetAppKeyValue(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
                return ConfigurationManager.AppSettings[key];
            else
                throw new ConfigurationErrorsException($"Configuration key {key} doesn't exist");
        }
    }
}
