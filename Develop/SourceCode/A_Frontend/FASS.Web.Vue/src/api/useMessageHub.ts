import * as signalR from "@microsoft/signalr";

const hubUrl = `/messageHub/Monitor/RuntimeHub`;

const connection = new signalR.HubConnectionBuilder()
  .withUrl(hubUrl)
  .configureLogging(signalR.LogLevel.Information)
  // .withAutomaticReconnect() // 添加自动重连功能
  .build();

let isSendingUpdates = false; // 标志位表示是否正在发送更新消息

async function start(): Promise<void> {
  console.log("重新链接？？");
  try {
    await connection.start();
    console.log("SignalR Connected.");
    // setupSignalRHandlers(); // 连接成功后立即设置处理程序
    startSendingUpdates(); // 开始发送更新消息
  } catch (err) {
    console.log("Connection failed: ", err);
    setTimeout(start, 10000); // 连接失败后 10 秒后重试
  }
}

connection.onclose(async (error: Error | undefined) => {
  // console.log("Connection closed with error: ", error);
  stopSendingUpdates();
  if (error) await start(); // 连接关闭时重新启动连接
});

const connect = async (): Promise<void> => {
  await start();
};

async function send(methodName: string): Promise<void> {
  try {
    if (connection.state === signalR.HubConnectionState.Connected) {
      await connection.invoke(methodName);
    } else {
      console.warn("Connection is not in the 'Connected' state.");
    }
  } catch (err) {
    // console.error("Send failed: ", err);
  }
}

// const disconnect = async (): Promise<void> => {
//   stopSendingUpdates();
//   if (connection.state === signalR.HubConnectionState.Connected) {
//     await connection.stop();
//   } else {
//     console.warn("Connection is not in the 'Connected' state.");
//   }
// };
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
  connection.on("Update", async (data: any) => {
    try {
      if (data && typeof data === "object" && isSendingUpdates) {
        // console.log("Received update:", data);
        callback(data);
      } else {
        console.warn("Received invalid update data:", data);
      }
    } catch (error) {
      console.error("Error in 'Update' handler:", error);
    }
  });
};

const startSendingUpdates = async () => {
  isSendingUpdates = true;
  await send("Update");
  console.log("开始发送更新消息");
};

const stopSendingUpdates = () => {
  isSendingUpdates = false;
};

export { connect, send, disconnect, setupSignalRHandlers, stopSendingUpdates };
