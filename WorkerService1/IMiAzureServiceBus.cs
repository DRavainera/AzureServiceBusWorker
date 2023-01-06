using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService1.Models;

namespace WorkerService1
{
    public interface IMiAzureServiceBus
    {
        public Task GetNewData(CancellationToken cancellationToken);

        public Task SendMessageAsync(Producto producto);
    }
}
