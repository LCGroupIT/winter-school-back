using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ApiService
{
    [EventSource(Name = "MyCompany-DataServiceApplication-ApiService")]
    internal sealed class ServiceEventSource : EventSource
    {
        public static readonly ServiceEventSource Current = new ServiceEventSource();

        static ServiceEventSource()
        {
            // Обходной путь для проблемы, при которой действия трассировки событий Windows не отслеживаются до инициализации инфраструктуры задач.
            // Эта проблема будет устранена в платформе .NET Framework 4.6.2.
            Task.Run(() => { });
        }

        //Конструктор экземпляра является частным для принудительного применения одноэлементной семантики.
        private ServiceEventSource() : base() { }

        #region Ключевые слова
        // Ключевые слова событий можно использовать для категоризации событий. 
        // Каждое ключевое слово является одноразрядным флагом. Одно событие можно сопоставить с несколькими ключевыми словами (посредством свойства EventAttribute.Keywords).
        // Ключевые слова должны быть определены в качестве открытого класса с именем Keywords внутри использующего их EventSource.
        public static class Keywords
        {
            public const EventKeywords Requests = (EventKeywords)0x1L;
            public const EventKeywords ServiceInitialization = (EventKeywords)0x2L;
        }
        #endregion

        #region События
        // Определите метод экземпляра для каждого события, которое надо записывать, и примените к нему атрибут [Event].
        // Имя метода является именем события.
        // Передайте все параметры, которые нужно записать вместе с событием (разрешены только целочисленные типы-примитивы, DateTime, GUID и строки).
        // Каждая реализация метода события должна проверять, включен ли источник события, а в случае положительного результата вызывать метод WriteEvent() для порождения события.
        // Число и типы аргументов, передаваемых в каждый метод события, должны в точности соответствовать параметрам, передаваемым в WriteEvent().
        // Укажите атрибут [NonEvent] для всех методов, которые не определяют событие.
        // Дополнительные сведения см. по адресу https://msdn.microsoft.com/ru-ru/library/system.diagnostics.tracing.eventsource.aspx.

        [NonEvent]
        public void Message(string message, params object[] args)
        {
            if (this.IsEnabled())
            {
                string finalMessage = string.Format(message, args);
                Message(finalMessage);
            }
        }

        private const int MessageEventId = 1;
        [Event(MessageEventId, Level = EventLevel.Informational, Message = "{0}")]
        public void Message(string message)
        {
            if (this.IsEnabled())
            {
                WriteEvent(MessageEventId, message);
            }
        }

        [NonEvent]
        public void ServiceMessage(ServiceContext serviceContext, string message, params object[] args)
        {
            if (this.IsEnabled())
            {

                string finalMessage = string.Format(message, args);
                ServiceMessage(
                    serviceContext.ServiceName.ToString(),
                    serviceContext.ServiceTypeName,
                    GetReplicaOrInstanceId(serviceContext),
                    serviceContext.PartitionId,
                    serviceContext.CodePackageActivationContext.ApplicationName,
                    serviceContext.CodePackageActivationContext.ApplicationTypeName,
                    serviceContext.NodeContext.NodeName,
                    finalMessage);
            }
        }

        // События, возникающие очень часто, может оказаться удобнее порождать с помощью API WriteEventCore.
        // Это обеспечивает более эффективную обработку параметров, однако требует явного выделения структуры EventData и применения небезопасного кода.
        // Чтобы включить эту ветвь кода, определите символ условной компиляции UNSAFE и включите поддержку небезопасного кода в свойствах проекта.
        private const int ServiceMessageEventId = 2;
        [Event(ServiceMessageEventId, Level = EventLevel.Informational, Message = "{7}")]
        private
#if UNSAFE
        unsafe
#endif
        void ServiceMessage(
            string serviceName,
            string serviceTypeName,
            long replicaOrInstanceId,
            Guid partitionId,
            string applicationName,
            string applicationTypeName,
            string nodeName,
            string message)
        {
#if !UNSAFE
            WriteEvent(ServiceMessageEventId, serviceName, serviceTypeName, replicaOrInstanceId, partitionId, applicationName, applicationTypeName, nodeName, message);
#else
            const int numArgs = 8;
            fixed (char* pServiceName = serviceName, pServiceTypeName = serviceTypeName, pApplicationName = applicationName, pApplicationTypeName = applicationTypeName, pNodeName = nodeName, pMessage = message)
            {
                EventData* eventData = stackalloc EventData[numArgs];
                eventData[0] = new EventData { DataPointer = (IntPtr) pServiceName, Size = SizeInBytes(serviceName) };
                eventData[1] = new EventData { DataPointer = (IntPtr) pServiceTypeName, Size = SizeInBytes(serviceTypeName) };
                eventData[2] = new EventData { DataPointer = (IntPtr) (&replicaOrInstanceId), Size = sizeof(long) };
                eventData[3] = new EventData { DataPointer = (IntPtr) (&partitionId), Size = sizeof(Guid) };
                eventData[4] = new EventData { DataPointer = (IntPtr) pApplicationName, Size = SizeInBytes(applicationName) };
                eventData[5] = new EventData { DataPointer = (IntPtr) pApplicationTypeName, Size = SizeInBytes(applicationTypeName) };
                eventData[6] = new EventData { DataPointer = (IntPtr) pNodeName, Size = SizeInBytes(nodeName) };
                eventData[7] = new EventData { DataPointer = (IntPtr) pMessage, Size = SizeInBytes(message) };

                WriteEventCore(ServiceMessageEventId, numArgs, eventData);
            }
#endif
        }

        private const int ServiceTypeRegisteredEventId = 3;
        [Event(ServiceTypeRegisteredEventId, Level = EventLevel.Informational, Message = "Service host process {0} registered service type {1}", Keywords = Keywords.ServiceInitialization)]
        public void ServiceTypeRegistered(int hostProcessId, string serviceType)
        {
            WriteEvent(ServiceTypeRegisteredEventId, hostProcessId, serviceType);
        }

        private const int ServiceHostInitializationFailedEventId = 4;
        [Event(ServiceHostInitializationFailedEventId, Level = EventLevel.Error, Message = "Service host initialization failed", Keywords = Keywords.ServiceInitialization)]
        public void ServiceHostInitializationFailed(string exception)
        {
            WriteEvent(ServiceHostInitializationFailedEventId, exception);
        }

        // Пара событий, использующих одинаковый префикс имени с суффиксом Start/Stop, которая неявно задает границы действия трассировки событий.
        // Эти действия могут автоматически выбираться средствами отладки и профилирования, которые способны вычислять их время выполнения, дочерние действия
        // и другую статистику.
        private const int ServiceRequestStartEventId = 5;
        [Event(ServiceRequestStartEventId, Level = EventLevel.Informational, Message = "Service request '{0}' started", Keywords = Keywords.Requests)]
        public void ServiceRequestStart(string requestTypeName)
        {
            WriteEvent(ServiceRequestStartEventId, requestTypeName);
        }

        private const int ServiceRequestStopEventId = 6;
        [Event(ServiceRequestStopEventId, Level = EventLevel.Informational, Message = "Service request '{0}' finished", Keywords = Keywords.Requests)]
        public void ServiceRequestStop(string requestTypeName, string exception = "")
        {
            WriteEvent(ServiceRequestStopEventId, requestTypeName, exception);
        }
        #endregion

        #region Частные методы
        private static long GetReplicaOrInstanceId(ServiceContext context)
        {
            StatelessServiceContext stateless = context as StatelessServiceContext;
            if (stateless != null)
            {
                return stateless.InstanceId;
            }

            StatefulServiceContext stateful = context as StatefulServiceContext;
            if (stateful != null)
            {
                return stateful.ReplicaId;
            }

            throw new NotSupportedException("Context type not supported.");
        }
#if UNSAFE
        private int SizeInBytes(string s)
        {
            if (s == null)
            {
                return 0;
            }
            else
            {
                return (s.Length + 1) * sizeof(char);
            }
        }
#endif
        #endregion
    }
}
