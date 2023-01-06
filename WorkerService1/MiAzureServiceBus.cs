using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService1.Models;

namespace WorkerService1
{
    public class MiAzureServiceBus : IMiAzureServiceBus
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ApplicationDbContext _dbContext;

        public MiAzureServiceBus(ServiceBusClient serviceBusClient, ApplicationDbContext dbContext)
        {
            _serviceBusClient = serviceBusClient;
            _dbContext = dbContext;
        }

        public async Task SendMessageAsync(Producto producto)
        {
            ServiceBusSender sender = _serviceBusClient.CreateSender("myqueue");

            var body = System.Text.Json.JsonSerializer.Serialize(producto);

            var mensaje = new ServiceBusMessage(body);

            await sender.SendMessageAsync(mensaje);
        }

        public async Task GetNewData(CancellationToken cancellationToken)
        {
            var producto = await _dbContext.Producto.FirstOrDefaultAsync();

            await SendMessageAsync(producto);
        }
    }
}
