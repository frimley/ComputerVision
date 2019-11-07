using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SuperSight
{
    /// <summary>
    /// Simple configuration wrapper
    /// </summary>
    class Config
    {
        public static string RabbitMQServer { get { return GetAppKeyValue("RabbitMQServer"); } }
        public static int RabbitMQPort { get { return Convert.ToInt32(GetAppKeyValue("RabbitMQPort")); } }
        public static string RabbitMQUser { get { return GetAppKeyValue("RabbitMQUser"); } }
        public static string RabbitMQPassword { get { return GetAppKeyValue("RabbitMQPassword"); } }
        public static string RabbitMQExchangeFrameInput { get { return GetAppKeyValue("RabbitMQExchangeFrameInput"); } }
        public static string RabbitMQQueueFrameOutput { get { return GetAppKeyValue("RabbitMQQueueFrameOutput"); } }
        public static string RabbitMQVirtualHost { get { return GetAppKeyValue("RabbitMQVirtualHost"); } }

        private static string GetAppKeyValue(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
                return ConfigurationManager.AppSettings[key];
            else
                throw new ConfigurationErrorsException($"Configuration key {key} doesn't exist");
        }
    }
}
