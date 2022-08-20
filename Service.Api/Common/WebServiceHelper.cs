using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Service.Api.Common
{
    public class WebServiceHelper
    {
        private readonly ClientWebSocket _webSocket;
        private readonly CancellationToken _cancellation;
        private readonly string _wsUrl;

        public WebServiceHelper(string wsUrl)
        {
            this._webSocket = new ClientWebSocket();
            this._cancellation = CancellationToken.None;
            this._wsUrl = wsUrl;
        }

        public async Task<T> Post<T>()
            where T : class
        {
            T result = null;
            try
            {
                await Open();

                if (_webSocket.State == WebSocketState.Open)
                {
                    ArraySegment<byte> bytesReceive = new ArraySegment<byte>(new byte[1024]);
                    WebSocketReceiveResult receiveResult = await _webSocket.ReceiveAsync(bytesReceive, _cancellation);
                    string responseBody = Encoding.UTF8.GetString(bytesReceive.Array, 0, receiveResult.Count);
                    result = JsonExtensions.Deserialize<T>(responseBody);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task Open()
        {
            if (_webSocket.State != WebSocketState.Open)
            {
                await _webSocket.ConnectAsync(new Uri(_wsUrl), _cancellation);
            }
        }

        public async Task Close()
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "over", _cancellation);
            }
        }
    }
}
