<script setup lang="ts">
import { ref } from "vue";
import { useHook } from "../utils/hook";
import { computed } from "vue";
import { useGlobal } from "@pureadmin/utils";
const { $config } = useGlobal<GlobalPropertiesApi>();
const title = computed(() => {
  return $config.Title;
});
/** 获取`logo` */
function getLogo() {
  return new URL("/logo.svg", import.meta.url).href;
}
const { handleLogout, handleGoMenu } = useHook();
</script>
<template>
  <nav class="bg-gray-800 header-main">
    <div class="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
      <div
        class="header-container relative flex h-16 items-center justify-between"
      >
        <div
          class="leftBtn absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0"
        >
          <button
            type="button"
            class="relative rounded-full bg-gray-800 p-1 text-gray-400 hover:text-white focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800"
            @click="handleGoMenu"
          >
            <svg
              t="1714380713040"
              class="icon"
              viewBox="0 0 1024 1024"
              version="1.1"
              xmlns="http://www.w3.org/2000/svg"
              p-id="7439"
              width="32"
              height="32"
            >
              <path
                d="M1011.017143 560.091429L556.8 24.685714a57.6 57.6 0 0 0-89.6 0L12.982857 560.091429a57.6 57.6 0 0 0 44.8 93.805714c31.817143 0 77.348571 25.782857 77.348572 57.6v251.611428a57.6 57.6 0 0 0 57.6 57.6h102.948571a57.6 57.6 0 0 0 57.6-57.6v-171.154285a57.6 57.6 0 0 1 57.6-57.6h202.24a57.6 57.6 0 0 1 57.6 57.6v171.154285a57.6 57.6 0 0 0 57.6 57.6h102.948571a57.6 57.6 0 0 0 57.6-57.6V711.497143c0-31.817143 45.531429-57.6 77.348572-57.6a57.6 57.6 0 0 0 44.8-93.805714z"
                fill="#ffffff"
                p-id="7440"
              />
            </svg>
          </button>
        </div>
        <div class="titleContent">
          <div class="title">
            <span style="color: #ffffff; font-size: 1.5rem; font-weight: 600">
              <img :src="getLogo()" alt="" />
              {{ title }}
            </span>
          </div>
        </div>
        <div
          class="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0"
        >
          <button
            type="button"
            class="relative rounded-full bg-gray-800 p-1 text-gray-400 hover:text-white focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800"
            @click="handleLogout"
          >
            <svg
              t="1714380760111"
              class="icon"
              viewBox="0 0 1024 1024"
              version="1.1"
              xmlns="http://www.w3.org/2000/svg"
              p-id="8520"
              width="32"
              height="32"
            >
              <path
                d="M0 192v640c0 70.7 57.3 128 128 128h352c17.7 0 32-14.3 32-32s-14.3-32-32-32H128c-35.3 0-64-28.7-64-64V192c0-35.3 28.7-64 64-64h352c17.7 0 32-14.3 32-32s-14.3-32-32-32H128C57.3 64 0 121.3 0 192z"
                p-id="8521"
                fill="#ffffff"
              />
              <path
                d="M1013.3 535.7L650.9 863.3c-41.1 37.2-106.9 8-106.9-47.5V685c0-4.4-3.6-8-8-8H224c-17.7 0-32-14.3-32-32V379c0-17.7 14.3-32 32-32h312c4.4 0 8-3.6 8-8V208.1c0-55.5 65.8-84.7 106.9-47.5l362.4 327.6c14.1 12.8 14.1 34.8 0 47.5z"
                p-id="8522"
                fill="#ffffff"
              />
            </svg>
          </button>
        </div>
      </div>
    </div>
  </nav>
</template>
<style scoped>
.bg-gray-800 {
  background-color: #541087;
  margin: 0;
  padding: 0;
}
.titleContent {
  display: flex;
}
.title {
  text-align: center;
  background-color: #541087;
  img {
    width: 29px;
    height: 20px;
  }
}
.titleLeft {
  height: 6rem;
  width: 6rem;
  border-top: 3rem solid transparent;
  border-bottom: 3rem solid transparent;
  border-left: 3rem solid transparent;
  border-right: 3rem solid #541087;
}
.titleRight {
  height: 6rem;
  width: 6.2rem;
  border-top: 3rem solid transparent;
  border-bottom: 3rem solid transparent;
  border-left: 3rem solid #541087;
  border-right: 3rem solid transparent;
}
.titleLeft span {
  font-size: 1.5rem;
  font-weight: 600;
  color: #ffffff;
}
.title span {
  font-size: 1.5rem;
  font-weight: 600;
  color: #ffffff;
}
.lg\:px-8 {
  /* padding: 0;
    margin: 0; */
}
.max-w-7xl {
  max-width: 999rem;
}
.header-main {
  height: 4rem;
}
@media (max-width: 640px) {
  .header-container {
    display: flex;
    flex-direction: row;
    justify-content: space-evenly;
    align-items: center;
  }
  .leftBtn {
    left: 0;
  }
}
</style>
