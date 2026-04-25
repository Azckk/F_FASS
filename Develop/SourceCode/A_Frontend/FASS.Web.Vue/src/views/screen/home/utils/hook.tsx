import { onMounted } from "vue";
import {
  // GetDictItem,
  // GetHome,
  // GetPermission,
  GetUser
  // Logout
} from "@/api/screen/home";
// import { getData } from "@/api/screen/data";
import { message } from "@/utils/message";
import { ElMessageBox } from "element-plus";
import { useUserStore } from "@/store/modules/user";
const store = useUserStore();

export function useHook(init?: Function) {
  async function testFn() {
    let res = await GetUser();
    console.log(res);
  }
  async function LogoutFn(isFullscreen, cancel = undefined) {
    ElMessageBox.confirm("是否确认退出？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        // await Logout();
        store.logOut();
        if (isFullscreen) init(); //退出之后需要退出全屏
        message("成功退出！", { type: "success" });
      })
      .catch(() => cancel?.());
  }
  onMounted(async () => {});

  return {
    testFn,
    LogoutFn
  };
}
