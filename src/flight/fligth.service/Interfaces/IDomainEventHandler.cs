using System;
using System.Collections.Generic;
using System.Text;

namespace fligth.service.Interfaces
{
    public interface IDomainEventHandler
    {
        void Handler(string message);
    }
}
