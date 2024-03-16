using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Receiver
{
    static void Main(string[] args)
    {
        const int listenPort = 11001;
        const int senderPort = 11000; // Assuming the sender's listening port for acknowledgments

        using (UdpClient receiver = new UdpClient(listenPort))
        {
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            IPEndPoint senderEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), senderPort);

            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for messages...");
                    byte[] bytes = receiver.Receive(ref groupEP);

                    string receivedData = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    Console.WriteLine($"Received: {receivedData}");

                    // Send acknowledgment
                    string confirmationMessage = "Message received";
                    byte[] confirmBytes = Encoding.UTF8.GetBytes(confirmationMessage);
                    receiver.Send(confirmBytes, confirmBytes.Length, senderEP);

                    Console.WriteLine("Confirmation sent.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}