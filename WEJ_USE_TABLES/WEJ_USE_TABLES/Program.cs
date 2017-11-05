using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace WEJ_USE_TABLES
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            /*
             * To use the Azure storage emulator is mandatory to configure the following:
             * -Create a environment variable in the computer/server:
             *      AzureWebJobsEnv=Development 
             * -Add to the App.Config the following lines:
             *   <appSettings>
             *     <add key="StorageConnectionString" value="UseDevelopmentStorage=true" />
             *   </appSettings>
             * -When access to the Azure connection string, instead of using the global Azure connection string, user this other:
             *  CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
             */

            var config = new JobHostConfiguration();
            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            int MaxPollingIntervalMiliseconds = 1;
            string strMaxPollingIntervalMiliseconds = ConfigurationManager.AppSettings["MaxPollingIntervalMiliseconds"].ToString();
            int.TryParse(strMaxPollingIntervalMiliseconds, out MaxPollingIntervalMiliseconds);
            int BatchSize = 1;
            string strBatchSize = ConfigurationManager.AppSettings["BatchSize"].ToString();
            int.TryParse(strBatchSize, out BatchSize);

            config.Queues.BatchSize = BatchSize; //Number of messages to extract at once.
            config.Queues.MaxDequeueCount = 5; //Max retries before move the messate to the poison.
            config.Queues.MaxPollingInterval = TimeSpan.FromMilliseconds(MaxPollingIntervalMiliseconds); //Pooling request to the queue on miliseconds.
            //config.Queues.MaxPollingInterval = TimeSpan.FromSeconds(15);
            config.NameResolver = new QueueNameResolver();//Use this class to aovid using the real name of the queue and use the app.config key name.
                                                            //The app.confing key's name queue must be between %. See the Functions.cs
            
            //Starndar.
            //var host = new JobHost();
            //---------------------------
            //Use the config:
            var host = new JobHost(config);

#if DEBUG
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
#else

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
#endif
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = tableClient.GetTableReference("hello");
            CreateNewTable(cloudTable);



            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();



        }




        public static void CreateNewTable(CloudTable table)
        {
            if (!table.CreateIfNotExists())
            {
                Console.WriteLine("Table {0} already exists", table.Name);
                return;
            }
            Console.WriteLine("Table {0} created", table.Name);
        }

    }
}
