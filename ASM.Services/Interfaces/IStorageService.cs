using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Services.Interfaces
{
    public interface IStorageService
    {
        public Task SendMessageAsync(string queueName, object item);
    }
}
