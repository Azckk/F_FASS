<script setup lang="ts">
import { onMounted, ref, onUnmounted, watchEffect } from "vue";
import { useHook } from "../utils/hook";
import { useI18n } from "vue-i18n";
const { t } = useI18n();
const props = defineProps({
  toggleFullscreen: Function,
  isFullscreen: Boolean
});
const { LogoutFn } = useHook(props.toggleFullscreen);
// const { BeforeData, AfterData } = useHook2();

defineOptions({
  name: ""
});
let time = ref("");
let aaaaa = ref("");
watchEffect(() => {
  if (props.isFullscreen) {
    aaaaa.value = t("screen.exit") + t("screen.fullScreen");
  } else {
    aaaaa.value = t("screen.fullScreen");
  }
});
let timer = null;
onMounted(() => {
  timer = setInterval(() => {
    time.value = new Date().toLocaleString();
  }, 1000);
});
onUnmounted(() => {
  clearInterval(timer);
  timer = null;
});
</script>

<template>
  <div id="header">
    <div class="time">{{ $t("screen.currentTime") }}：{{ time }}</div>
    <div class="title">
      <img src="/logo.png" alt="" /> {{ $t("title.title") }}
    </div>
    <div class="header-btn">
      <!-- <el-button size="small" color="#4BA0B5">数据</el-button> -->
      <!-- <el-button color="#904EE9" @click="BeforeData()">{{
        $t("screen.frontEnd")
      }}</el-button>
      <el-button color="#904EE9" @click="AfterData()">{{
        $t("screen.backstage")
      }}</el-button>
      <el-button color="#904EE9" @click="props.toggleFullscreen()">{{
        $t("screen.fullScreen")
      }}</el-button>
      <el-button color="#904EE9" @click="LogoutFn(props.isFullscreen)">{{
        $t("screen.exit")
      }}</el-button> -->
      <!-- <div class="btnStyle" @click="BeforeData()">
        {{ $t("screen.frontEnd") }}
      </div>
      <div class="btnStyle" @click="AfterData()">
        {{ $t("screen.backstage") }}
      </div> -->
      <div class="btnStyle" @click="props.toggleFullscreen()">
        {{ aaaaa }}
      </div>
      <div class="btnStyle" @click="LogoutFn(props.isFullscreen)">
        {{ $t("screen.exit") }}
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
#header {
  width: 100%;
  height: 90px;
  display: flex;
  justify-content: space-between;
  background-image: url("@/assets/screen/header2.png");
  background-size: 100%;
  background-repeat: no-repeat;
  background-position-x: 6px;
  margin: 0;
  margin-top: 10px;
  color: #fff;
  .time {
    margin-top: 50px;
    font-size: 24px;
    text-align: center;
    flex: 1;
  }
  .title {
    font-size: 26px;
    font-weight: 600;
    line-height: 3.5rem;
    text-align: center;
    justify-content: center;
    color: #fff;
    flex: 2;
    img {
      width: 154px;
      height: 33px;
      margin-right: 20px;
    }
  }
  .header-btn {
    flex: 1;
    margin-top: 50px;
    display: flex;
    justify-content: flex-end;
  }
}
.btnStyle {
  width: 84px;
  height: 30px;
  margin-right: 20px;
  background: linear-gradient(
    0deg,
    rgba(125, 53, 220, 0.4),
    rgba(136, 70, 223, 0.05)
  );
  border-radius: 4px;
  border: 2px solid #904ee9;
  text-align: center;
  font-weight: 600;
  font-size: 14px;
  color: #ffffff;
  line-height: 23px;
  cursor: pointer;
}
.btnStyle:hover {
  background: linear-gradient(
    0deg,
    rgba(125, 53, 220, 0.05),
    rgba(136, 70, 223, 0.05)
  );
}
</style>
