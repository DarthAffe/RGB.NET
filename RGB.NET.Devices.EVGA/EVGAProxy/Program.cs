using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace EVGAProxy
{
    public class Program
    {
        static EVGA64Proxy prox;

        //I did the most offensively crappy IPC I could, because it was also the most quickiest to implement with the lowiest overhead
        private static byte[] DoMsg(byte[] msg)
        {
            byte[] resp = null;
            switch (msg[0])
            {
                //1 is get device count
                case 1:
                    resp = new byte[16];
                    resp[0] = 2;
                    resp[1] = (byte)prox.GetNumberOfDevices();
                    break;
                //2 is get number of devices response
                //5 is get led count for device
                case 5:
                    resp = new byte[16];
                    resp[0] = 6;
                    resp[1] = (byte)prox.GetLedCount(msg[1]);
                    break;
                //6 is get led count response
                //10 is set led
                case 10:
                    prox.SetColor(msg[1], msg[2], msg[3], msg[4], msg[5], msg[6]);
                    break;
            }

            return resp;
        }
        public static void Main(string[] args)
        {
            EVGA64Proxy.Log("Starting");
            //I wanna do some async because it's cool, so. yeah.  Wait.
            Main().Wait();
        }

        //the whole half assed idea of this function is try to get a connection and hold it, or just die.
        //  not very resilient, but may help processes from sticking around when they aren't wanted.
        public static async Task Main()
        {
            try
            {
                prox = new EVGA64Proxy();
                Queue<byte> queue = new Queue<byte>();
                EVGA64Proxy.Log("Starting pipe server");
                NamedPipeServerStream nps = new NamedPipeServerStream("evgargbled", PipeDirection.InOut);
                EVGA64Proxy.Log("Waiting for connection");
                nps.WaitForConnection();
                EVGA64Proxy.Log("Got connection");
                while (true)
                {
                    if (!nps.IsConnected)
                    {
                        EVGA64Proxy.Log("Not connected!  exiting.");
                        Environment.Exit(0);
                        return;
                    }
                    byte[] magic = new byte[1];
                    try
                    {
                        int read = await nps.ReadAsync(magic, 0, 1);
                        if (read != 1)
                        {
                            continue;
                        }
                        queue.Enqueue(magic[0]);
                        if (queue.Count > 4)
                        {
                            queue.Dequeue();
                        }
                        if (queue.SequenceEqual(new byte[] { 0xB, 0xE, 0xE, 0xF }))
                        {
                            queue.Clear();
                            byte[] bmsg = new byte[16];
                            //each message is 16 bytes just to keep things simple
                            read = await nps.ReadAsync(bmsg, 0, 16);
                            if (read != 16)
                            {
                                //something went wrong
                                continue;
                            }
                            byte[] resp = DoMsg(bmsg);
                            if (resp != null)
                            {
                                byte[] bMagic = new byte[] { 0xB, 0xE, 0xE, 0xF };
                                nps.Write(bMagic, 0, 4);
                                nps.Write(resp, 0, resp.Length);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EVGA64Proxy.Log($"Failed reading message: {ex.Message}");
                        //failed reading, so die
                        Environment.Exit(0);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //going to exit anyways, log it and let nature take its course
                EVGA64Proxy.Log($"Exception in main: {ex.Message}");
            }
        }            
    }
}
