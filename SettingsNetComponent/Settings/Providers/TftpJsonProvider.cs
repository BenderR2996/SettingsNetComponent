using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tftp.Net;

namespace SettingsNetComponent
{
    /// <summary>
    /// Провайдер настроек в формате Json (работающий через tftp соединение)
    /// </summary>
    public class TftpJsonProvider : IJsonProvider
    {
        private readonly string tftpServerAddress;
        public TftpJsonProvider(Guid appId, IPAddress tftpServerIp)
        {
            this.AppId = appId;
            tftpServerAddress = tftpServerIp.ToString();
        }
        public Guid AppId { get; set; }

        private static AutoResetEvent TransferFinishedEvent = new AutoResetEvent(false);

        public string LoadJson()
        {
            var client = new TftpClient(tftpServerAddress);

            var transfer = client.Download($@"{AppId}.json");

            transfer.OnFinished += (t) => { TransferFinishedEvent.Set(); client = null; };
            transfer.OnError += (s, e) => { TransferFinishedEvent.Set(); };

            string content = "";
            using (var ms = new MemoryStream())
            {
                ms.Position = 0;
                transfer.Start(ms);
                TransferFinishedEvent.WaitOne();
                content = new StreamReader(new MemoryStream(ms.GetBuffer())).ReadToEnd();
            }


            return content;
        }
        public void SaveJson(string json)
        {
            var tmp = $@"{AppId}.json";
            File.WriteAllText(tmp, json);

            var client = new TftpClient(tftpServerAddress);
            var transfer = client.Upload(tmp);

            transfer.OnFinished += (t) => { TransferFinishedEvent.Set(); client = null; };
            transfer.OnError += (s, e) => { TransferFinishedEvent.Set(); };

            using (Stream stream = File.Open(tmp, FileMode.Open))
            {
                transfer.Start(stream);
                TransferFinishedEvent.WaitOne();
            }
            File.Delete(tmp);
        }
    }
}
