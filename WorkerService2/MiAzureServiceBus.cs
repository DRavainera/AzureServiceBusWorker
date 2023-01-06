using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService2.Models;

namespace WorkerService2
{
    public class MiAzureServiceBus : IMiAzureServiceBus
    {
        private readonly ServiceBusClient _serviceBus;
        private readonly ApplicationDbContext _dbContext;

        public MiAzureServiceBus(ServiceBusClient serviceBus, ApplicationDbContext dbContext)
        {
            _serviceBus = serviceBus;
            _dbContext = dbContext;
        }

        public async Task GetQueues(CancellationToken cancellationToken)
        {
            bool valido = true;
            
            ServiceBusReceiver receiver = _serviceBus.CreateReceiver("myqueue");

            do
            {
                Console.WriteLine(valido);
                
                ServiceBusReceivedMessage receivedMessage = await receiver
                    .ReceiveMessageAsync(TimeSpan.FromMilliseconds(1000), cancellationToken);

                if (receivedMessage is not null)
                {
                    var jsonString = receivedMessage.Body.ToString();

                    Console.WriteLine(jsonString);

                    if (jsonString is not null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(jsonString);

                        if (json.Id is not null)
                        {
                            await receiver.CompleteMessageAsync(receivedMessage);

                            Producto producto = new Producto()
                            {
                                Id = 0,
                                Nombre = json.Nombre,
                                Stock = json.Stock,
                                Precio = json.Precio
                            };

                            await _dbContext.Producto.AddAsync(producto);

                            await _dbContext.SaveChangesAsync();
                        }
                        else valido = false;
                    }
                    else valido = false;
                }
                else valido = false;

            } while (valido == true);

            Console.WriteLine(valido);
        }
    }
}
