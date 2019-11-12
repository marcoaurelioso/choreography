using System;
using System.Collections.Generic;
using System.Text;

namespace payment.service.Interfaces
{
    public interface IDomainEventHandler
    {
        void Handler(string message);
    }
}
