using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MQCore {  
  public class Core {
    

    #region Properties
    public string sMessage { get; set; }
    public bool bError { get; set; }
    public List<string> lQueues { get; set; } = new List<string>();

    protected ConnectionFactory oFactory = null;
    protected IConnection iConn = null;
    protected IModel iChannel = null;
    protected IBasicProperties iProp = null;
    #endregion


    public void CreateQueue(string sQueue, string sHostName) {
      try {
        if (iChannel == null) {
          ConnectMQ(sHostName);
        }
        if (!lQueues.Contains(sQueue)) {          

          iChannel.QueueDeclare(sQueue, false, false, false, null);

          lQueues.Add(sQueue);
          sMessage = "Queue Created:" + sQueue;
        }        
      } catch (Exception oEx) {
        bError = true;
        sMessage = "Error On Queue Creation: " + oEx.Message;
      }
    }

    private void ConnectMQ(string sHostName) {
      try {

        oFactory = new ConnectionFactory() { HostName = sHostName};
        iConn = oFactory.CreateConnection();
        iChannel = iConn.CreateModel();
        iProp = iChannel.CreateBasicProperties();
        iProp.Persistent = true;
        iProp.DeliveryMode = 2;

        sMessage = "MQConnected";
      } catch (Exception oEx) {
        bError = true;
        sMessage = "Error On MQ Connection: " + oEx.Message;
      }                  
    }

    public string SendMessage(string sBody, string sQueue, string sHostName) {
      try {
        InitGlobal();
        if (iChannel == null) {
          ConnectMQ(sHostName);
        }
        iProp.Timestamp = new AmqpTimestamp((Int32)(DateTime.UtcNow.Subtract(DateTime.Now)).TotalSeconds);
        iProp.CorrelationId = System.Guid.NewGuid().ToString();
        iProp.MessageId = iProp.CorrelationId;
        iChannel.BasicPublish(exchange: "", routingKey: sQueue, basicProperties: iProp, body: Encoding.UTF8.GetBytes(sBody), mandatory: true);

        return "Message Successfully Sent on " + sQueue;
      } catch(Exception oEx) {
        bError = true;
        return "Error On Sending: " + oEx.Message;
      }
    }

    public string GetMessage(string sQueue, ref bool bRun, string sHostName) {
      try {
        InitGlobal();
        
        string sBody = "";
        while (bRun) {
          EventingBasicConsumer oConsumer = new EventingBasicConsumer(iChannel);
          var data = iChannel.BasicGet(sQueue, true);
          sBody = Encoding.UTF8.GetString(data.Body); 
          if (sBody.Length > 0) {
            return sBody;
          }
          System.Threading.Thread.Sleep(1000);
        }
        return "";
      } catch(Exception oEx) {
        bError = true;
        return "Error on reading queue: " + oEx.Message;
      }
    }
    #region util
    private void InitGlobal() {
      sMessage = "";
      bError = false;
    }

    private void ClearConnection() {
    oFactory = null;
    iConn = null;
    iChannel = null;
  }
    #endregion
  }
}
