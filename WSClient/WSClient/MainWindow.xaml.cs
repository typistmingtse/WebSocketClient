using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebSocketSharp;

namespace WSClient
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        WebSocket ws;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                //透過websocketclient傳送資料
                ws = new WebSocket(ConfigurationManager.AppSettings["WebSocketServer"] /*"ws://192.168.10.99:25000"*/);
                ws.Connect();
                if(ws.ReadyState == WebSocketState.Open)
                    ws.OnMessage += GetMessage;
                else
                {
                    switch (ws.ReadyState)
                    {
                        case WebSocketState.Closed:
                            MessageBox.Show("Websocket server closed");
                            break;
                        case WebSocketState.Closing:
                            MessageBox.Show("Websocket server closing");
                            break;
                        case WebSocketState.Connecting:
                            MessageBox.Show("Websocket server connecting");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GetMessage(object sender, MessageEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                lbxMessages.Items.Add(e.Data);
            }); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ws.Send(tbxMessage.Text);
        }
    }
}
