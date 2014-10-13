
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceEventLogging;
using ServiceEventLogging.Events;
using System.Diagnostics.Tracing;

namespace MvcShoppingCart.Logging
{
    public class CartServiceEventLogger : ServiceEventLogger
    {
        static CartServiceEventLogger eventLogger = new CartServiceEventLogger();

        private CartServiceEventLogger()
        {
        }

        public static CartServiceEventLogger Instance
        {
            get
            {
                return eventLogger;
            }
        }

        public void LogMethodStartEvent(Guid correlationId, string methodName, long milliseconds)
        {
            var startEvent = new ServiceEvent()
            {
                CorrelationId = correlationId,
                DebugInfo = string.Concat("Ticks:", milliseconds),
                Message = string.Concat(methodName, " Start Event"),
                EventId = EventId.MethodStartEventId
            };
            ServiceEventSource.EventSource.LogServiceEvent(startEvent.ToLogLine());
        }

        public void LogMethodEndEvent(Guid correlationId, string methodName, long milliseconds)
        {
            var startEvent = new ServiceEvent()
            {
                CorrelationId = correlationId,
                DebugInfo = string.Concat("Ticks:", milliseconds),
                Message = string.Concat(methodName, " End Event"),
                EventId = EventId.MethodEndEventId
            };
            ServiceEventSource.EventSource.LogServiceEvent(startEvent.ToLogLine());
        }

        public void LogInvalidCartItemEvent(Guid correlationId, string methodName)
        {
            ServiceEventSource.EventSource.LogServiceEvent(new ServiceEvent
            {
                
                CorrelationId = correlationId,
                EventId = EventId.InvalidCartEventId,
                EventLevel = EventLevel.Error,
                Message = methodName + " Invalid Cart Item"
            }.ToLogLine());
        }
    }
}