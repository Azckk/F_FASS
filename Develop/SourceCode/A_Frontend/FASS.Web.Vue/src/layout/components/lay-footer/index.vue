<script lang="ts" setup>
import { getConfig } from "@/config";
import { ref, onMounted, watch, watchEffect, onBeforeUnmount } from "vue";
import { getAppName } from "@/api/user";
import { addDialog, closeDialog } from "@/components/ReDialog";
import dialog from "./dialog.vue";
import { useHook } from "./utils/hooks";
const { info, handleGlobalWarning, disconnect } = useHook();

const TITLE = getConfig("Title");
const AppName = ref();

const errMessage = ref();

const isActive = ref(false);
const getAppNameToShow = async () => {
  let { data } = await getAppName();
  AppName.value = data.appName;
};

watch(
  () => info.value,
  (newVal, oldVal) => {
    errMessage.value = newVal;
    if (
      newVal &&
      newVal.length > 0 &&
      !isActive.value &&
      oldVal.length !== newVal.length
    ) {
      isActive.value = true;
      onBaseClick();
    } else if (newVal.length === 0 && isActive.value) {
      isActive.value = false;
      closeDialog({}, 0);
    }
  },
  {
    deep: true
  }
);
let timer: ReturnType<typeof setInterval>;

function interval() {
  timer = setInterval(() => {
    if (errMessage.value && errMessage.value.length > 0 && !isActive.value) {
      isActive.value = true;
      onBaseClick();
      clearInterval(timer);
      timer = null;
    } else if (
      errMessage.value &&
      errMessage.value.length === 0 &&
      isActive.value
    ) {
      isActive.value = false;
      closeDialog({}, 0);
    }
  }, 1000 * 30);
}

onMounted(() => {
  getAppNameToShow();
  handleGlobalWarning();
  interval();
});
// 在组件卸载时清除定时器
onBeforeUnmount(() => {
  disconnect(() => {});
});

function onBaseClick() {
  addDialog({
    title: "警告",
    width: "20%",
    alignCenter: true,
    closeOnClickModal: false,
    // hideFooter: true,
    center: true,
    props: { info },
    footerButtons: [
      {
        label: "确认",
        color: "#5a3092",
        btnClick: ({ dialog: { options, index }, button }) => {
          console.log(options, index);
          console.log("info", errMessage.value);
          isActive.value = false;
          clearInterval(timer);
          interval();
          closeDialog(options, index);
        }
      }
    ],

    contentRenderer: () => dialog
  });
}
</script>

<template>
  <footer
    class="layout-footer text-[rgba(0,0,0,0.6)] dark:text-[rgba(220,220,242,0.8)]"
  >
    Copyright © 2024
    <a class="hover:text-primary" href="/"> &nbsp;{{ TITLE }} </a>
    <p>&emsp;{{ AppName }}</p>
  </footer>
</template>

<style lang="scss" scoped>
.layout-footer {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  padding: 0 0 8px;
  font-size: 14px;
}
</style>
