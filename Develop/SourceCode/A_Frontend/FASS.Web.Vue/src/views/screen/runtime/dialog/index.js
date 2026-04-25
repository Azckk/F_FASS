import { createApp, h } from "vue";
import MessageBox from "./index.vue";
import ElementPlus from "element-plus";
import "element-plus/dist/index.css";
import router from "@/router"; // 假设你的路由文件是 router.js

let activeDialogs = []; // 用于保存所有活动弹窗的销毁函数

function checkClassName(className) {
  const elements = document.getElementsByTagName("*");
  for (let i = 0; i < elements.length; i++) {
    if (elements[i].classList.contains(className)) {
      return true;
    }
  }
  return false;
}

export default function showMsg(msg, className, info, event) {
  if (checkClassName(className)) return;

  const div = document.createElement("div");
  document.body.appendChild(div);
  div.classList.add(className);

  const app = createApp({
    setup() {
      const close = () => {
        if (document.body.contains(div)) {
          app.unmount();
          document.body.removeChild(div);
        }
      };
      return () =>
        h(
          MessageBox,
          { msg, visible: true, onClose: close, event },
          {
            info: () => h(info)
          }
        );
    }
  });
  app.use(ElementPlus);
  app.mount(div);

  // 保存销毁函数到数组中
  activeDialogs.push(() => {
    if (document.body.contains(div)) {
      app.unmount();
      document.body.removeChild(div);
    }
  });
}

// 导出一个函数用于销毁所有弹窗
export function destroyAllDialogs() {
  activeDialogs.forEach(destroy => destroy());
  activeDialogs = [];
}

// 添加全局前置守卫
router.beforeEach((to, from, next) => {
  destroyAllDialogs(); // 在路由变化时销毁所有弹窗
  next();
});
