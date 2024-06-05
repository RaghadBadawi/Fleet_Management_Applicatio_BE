//using System;
//using System.Collections.Concurrent;
//using System.Linq;
//using System.Net.WebSockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.WebSockets;

//namespace TProject2.App_Start
//{
//    public class WebSocketHandler : IHttpHandler
//    {
//        private static readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

//        public bool IsReusable => false;

//        public void ProcessRequest(HttpContext context)
//        {
//            if (context.IsWebSocketRequest)
//            {
//                context.AcceptWebSocketRequest(HandleWebSocketAsync);
//            }
//            else
//            {
//                context.Response.StatusCode = 400;
//            }
//        }

//        private async Task HandleWebSocketAsync(AspNetWebSocketContext context)
//        {
//            var socket = context.WebSocket;
//            var id = Guid.NewGuid().ToString();
//            _sockets.TryAdd(id, socket);

//            var buffer = new byte[1024 * 4];
//            WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

//            while (!result.CloseStatus.HasValue)
//            {
//                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
//            }

//            _sockets.TryRemove(id, out _);
//            await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
//        }

//        public static async Task BroadcastAsync(string message)
//        {
//            var buffer = Encoding.UTF8.GetBytes(message);
//            var tasks = _sockets.Values.Select(socket => socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None));
//            await Task.WhenAll(tasks);
//        }
//    }
//}