import * as signalR from "@microsoft/signalr";

const hubUrl = `/messageHub/Monitor/AlarmHub`;

const connection = new signalR.HubConnectionBuilder()
  .withUrl(hubUrl)
  .configureLogging(signalR.LogLevel.Information)
  .build();

let isSendingUpdates = false;

async function start(): Promise<void> {
  try {
    await connection.start();
    console.log("SignalR Connected.");
    setupSignalRHandlers(data => {
      // console.log("处理报警数据:", data);
    });
  } catch (err) {
    console.log("Connection failed: ", err);
    setTimeout(start, 10000); // 连接失败后重试
  }
}

connection.onclose(async (error: Error | undefined) => {
  stopSendingUpdates();
  if (error) await start();
});

const connect = async (): Promise<void> => {
  await start();
};

async function send(methodName: string, ...args: any[]): Promise<void> {
  try {
    if (connection.state === signalR.HubConnectionState.Connected) {
      await connection.invoke(methodName);
    } else {
      console.warn("Connection is not in the 'Connected' state.");
    }
  } catch (err) {
    console.error("Send failed: ", err);
  }
}

const sendAlarm = async (methodName = "Update"): Promise<void> => {
  isSendingUpdates = true;
  await send(methodName);
};

const disconnect = async (callback): Promise<void> => {
  stopSendingUpdates();
  callback();
  try {
    if (connection.state !== signalR.HubConnectionState.Disconnected) {
      await connection.stop();
      console.log("链接已被关闭✋");
    } else {
      console.warn("Connection is already in the 'Disconnected' state.");
    }
  } catch (err) {
    console.error("Disconnect failed: ", err);
  }
};

const setupSignalRHandlers = callback => {
  connection.on("alarm", async (data: any) => {
    try {
      if (
        data &&
        (typeof data === "object" || typeof data === "string") &&
        isSendingUpdates
      ) {
        callback(data);
      } else {
        callback("");
        // console.warn("Received invalid alarm data:", data);
      }
    } catch (error) {
      console.error("Error in 'Alarm' handler:", error);
    }
  });
};

const stopSendingUpdates = () => {
  isSendingUpdates = false;
};

export {
  connect,
  sendAlarm,
  disconnect,
  setupSignalRHandlers,
  stopSendingUpdates
};
