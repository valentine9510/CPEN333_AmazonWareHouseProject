using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;
using AmazoomClassLibrary;
using System.Runtime.Serialization.Formatters.Binary;

namespace AmazoomWebServer
{
    public class ServerCommThread
    {
        private string WebServerPipeHandle;

        public ServerCommThread(string pipehandle)
        {
            WebServerPipeHandle = pipehandle;
        }

        public void Execute()
        {
            AnonymousPipeClientStream WebServerPipe = new AnonymousPipeClientStream(PipeDirection.Out, WebServerPipeHandle);
            BinaryFormatter StreamWriter = new BinaryFormatter();
            while (true)
            {
                if (Globals.ReturnQueueCount() != 0)
                {
                    Order Sale = Globals.TakeFromQueue();
                    ProcessCommunication.SyncPipeClient(WebServerPipe);
                    StreamWriter.Serialize(WebServerPipe, Sale);
                }
            } 
        }
    }
}
