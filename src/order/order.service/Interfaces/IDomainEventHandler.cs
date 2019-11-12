using System;
using System.Collections.Generic;
using System.Text;

namespace order.service.Interfaces
{
    public interface IDomainEventHandler
    {
        void Handler(string message);
    }
}
