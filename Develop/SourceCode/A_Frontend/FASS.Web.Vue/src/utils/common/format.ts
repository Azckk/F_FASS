Date.prototype.format = function (fmt) {
  var o = {
    "M+": this.getMonth() + 1,
    "d+": this.getDate(),
    "h+": this.getHours(),
    "m+": this.getMinutes(),
    "s+": this.getSeconds(),
    "q+": Math.floor((this.getMonth() + 3) / 3),
    S: this.getMilliseconds()
  };
  if (/(y+)/.test(fmt))
    fmt = fmt.replace(
      RegExp.$1,
      (this.getFullYear() + "").substr(4 - RegExp.$1.length)
    );
  for (var k in o)
    if (new RegExp("(" + k + ")").test(fmt))
      fmt = fmt.replace(
        RegExp.$1,
        RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length)
      );
  return fmt;
};

export function formatDateTime(str) {
  if (str === undefined || str === null || str === "") return "";
  var tm = new Date(str);
  return tm.format("yyyy-MM-dd hh:mm:ss");
}

export function formatTime(str) {
  if (str === undefined || str === null || str === "") return "";
  var tm = new Date(str);
  return tm.format("hh:mm:ss");
}

export function formatDate(str) {
  if (str === undefined || str === null || str === "") return "";
  var tm = new Date(str);
  return tm.format("yyyy-MM-dd");
}

export function formatShortDate(str) {
  if (str === undefined || str === null || str === "") return "";
  var tm = new Date(str);
  return tm.format("MM/dd");
}

export function formatMinuteSecond(str) {
  if (str === undefined || str === null || str === "") return "";
  var tm = new Date(str);
  return tm.format("mm:ss");
}

export function lowerFirstLetter(str) {
  if (str === undefined || str === null || str === "") return "";
  return str.charAt(0).toLowerCase() + str.slice(1);
}

export function GetContinue(created, cleared = null) {
  let msg = "";
  if (!cleared) {
    const targetDate = new Date(created); // 创建传入时间的日期对象
    const currentDate = new Date(); // 创建当前时间的日期对象
    // 计算时间间隔（毫秒数）
    const timeDifference = currentDate.getTime() - targetDate.getTime();
    // 计算间隔的小时、分钟和秒数
    const hours = Math.floor(timeDifference / (1000 * 60 * 60));
    const minutes = Math.floor(
      (timeDifference % (1000 * 60 * 60)) / (1000 * 60)
    );
    const seconds = Math.floor((timeDifference % (1000 * 60)) / 1000);
    msg = `${hours} 小时 ${minutes} 分 ${seconds} 秒`;
    return msg;
  } else {
    const targetDate = new Date(created); // 创建传入时间的日期对象
    const currentDate = new Date(cleared); // 创建当前时间的日期对象
    // 计算时间间隔（毫秒数）
    const timeDifference = currentDate.getTime() - targetDate.getTime();
    // 计算间隔的小时、分钟和秒数
    const hours = Math.floor(timeDifference / (1000 * 60 * 60));
    const minutes = Math.floor(
      (timeDifference % (1000 * 60 * 60)) / (1000 * 60)
    );
    const seconds = Math.floor((timeDifference % (1000 * 60)) / 1000);
    msg = `${hours} 小时 ${minutes} 分 ${seconds} 秒`;
    return msg;
  }
}
