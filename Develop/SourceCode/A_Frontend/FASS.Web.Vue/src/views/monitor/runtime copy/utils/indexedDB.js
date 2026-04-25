// indexedDB.js

const dbName = "dataList";
let dbVersion = 2; // 初始版本号

// 打开数据库并创建对象仓库（如果不存在）
function openDatabase(storeName) {
  return new Promise((resolve, reject) => {
    const request = indexedDB.open(dbName, dbVersion);
    console.log("openDatabase", request);

    request.onerror = () => {
      console.error("Database error: ", request.error);
      reject(request.error);
    };

    request.onsuccess = event => {
      console.log("Database opened successfully");
      resolve(event.target.result);
    };

    request.onupgradeneeded = event => {
      const db = event.target.result;
      console.log("~~~~~~~~.", !db.objectStoreNames.contains(storeName));
      if (!db.objectStoreNames.contains(storeName)) {
        db.createObjectStore(storeName, { keyPath: "id", autoIncrement: true });
        console.log(`Created object store '${storeName}'`);
      }
    };
  });
}

// 存储数据到指定的对象仓库
export function setTasks(tasks, storeName) {
  return openDatabase(storeName).then(db => {
    return new Promise((resolve, reject) => {
      const transaction = db.transaction([storeName], "readwrite");
      const objectStore = transaction.objectStore(storeName);

      objectStore.clear().onsuccess = () => {
        for (const task of tasks) {
          objectStore.add(task);
        }
        resolve();
      };

      transaction.onerror = event => {
        console.error(
          `Set tasks error for store '${storeName}': `,
          event.target.errorCode
        );
        reject(event.target.errorCode);
      };
    });
  });
}

// 获取指定对象仓库中的所有数据
export function getTasks(storeName) {
  return openDatabase(storeName).then(db => {
    return new Promise((resolve, reject) => {
      const transaction = db.transaction([storeName], "readonly");
      const objectStore = transaction.objectStore(storeName);
      const request = objectStore.getAll();

      request.onerror = event => {
        console.error(
          `Get all tasks error for store '${storeName}': `,
          event.target.errorCode
        );
        reject(event.target.errorCode);
      };

      request.onsuccess = event => {
        resolve(event.target.result);
      };
    });
  });
}

// 删除整个数据库
export function deleteDatabase() {
  const request = indexedDB.deleteDatabase(dbName);

  return new Promise((resolve, reject) => {
    request.onerror = event => {
      console.error("Failed to delete database:", event.target.errorCode);
      reject(event.target.errorCode);
    };

    request.onsuccess = event => {
      console.log("Database deleted successfully.", event);
      resolve();
    };

    request.onblocked = event => {
      console.warn("Database delete operation blocked.", event);
    };
  });
}
