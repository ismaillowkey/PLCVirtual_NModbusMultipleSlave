using NModbus;
using System.Net.Sockets;
using System.Net;

namespace PLCVirtual_NModbusMultipleSlave
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int port = 502;
            Console.WriteLine($"Starting Modbus TCP Server with port:{port}");
            IPAddress address = new IPAddress(new byte[] { 0, 0, 0, 0 });

            // create and start the TCP slave
            TcpListener slaveTcpListener = new TcpListener(address, port);
            slaveTcpListener.Start();
            Console.WriteLine($"Started Modbus TCP Server with port:{port}");

            IModbusFactory factory = new ModbusFactory();
            IModbusSlaveNetwork network = factory.CreateSlaveNetwork(slaveTcpListener);

            var SlaveList = new List<IModbusSlave>();
            var CountSlave = 1;
            for (int i=1; i<=CountSlave; i++) 
            {
                Console.WriteLine($"adding slave with id:{i}");
                var slave = factory.CreateSlave((byte)i);
                SlaveList.Add(slave);
                network.AddSlave(slave);
            }


            Console.WriteLine("Starting modbus tcp slave");
            network.ListenAsync().GetAwaiter().GetResult();
            Console.WriteLine("Started modbus tcp slave");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
