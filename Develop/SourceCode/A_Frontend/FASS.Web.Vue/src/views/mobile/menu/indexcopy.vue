<script setup lang="ts">
import { ref } from "vue";
import { useRouter, useRoute } from "vue-router";
const router = useRouter();
import { useHook } from "./utils/hook";
const { handleRouter } = useHook();
</script>

<template>
  <div class="bg-white main-content">
    <div
      class="mx-auto max-w-2xl px-4 py-16 sm:px-6 sm:py-24 lg:max-w-7xl lg:px-8"
    >
      <h2 class="text-2xl font-bold tracking-tight text-gray-900">
        {{ $t("mobile.taskProcess") }}
      </h2>
      <div
        class="mt-6 grid grid-cols-1 gap-x-6 gap-y-10 sm:grid-cols-2 lg:grid-cols-3 xl:gap-x-8"
      >
        <div
          class="group relative"
          @click="handleRouter('/mobile/flowcartasksingle/index')"
        >
          <div
            class="img-content aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80"
          >
            <img
              src="@/assets/mobile/menu/flow-car-task-single.png"
              alt=""
              class="imgs h-full w-full object-cover object-center lg:h-full lg:w-full"
            />
            <p class="text-content text-xl font-medium text-gray-900">
              {{ $t("mobile.singleTasks") }}
            </p>
          </div>
        </div>
        <div
          class="group relative"
          @click="handleRouter('/mobile/flowcartaskdouble/index')"
        >
          <div
            class="img-content aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80"
          >
            <img
              src="@/assets/mobile/menu/flow-car-task-double.png"
              alt=""
              class="imgs h-full w-full object-cover object-center lg:h-full lg:w-full"
            />
            <p class="text-content text-xl font-medium text-gray-900">
              {{ $t("mobile.doubleTasks") }}
            </p>
          </div>
        </div>
        <div
          class="group relative"
          @click="handleRouter('/mobile/flowcartasktemplate/index')"
        >
          <div
            class="img-content aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80"
          >
            <img
              src="@/assets/mobile/menu/flow-car-task-template.png"
              alt=""
              class="imgs h-full w-full object-cover object-center lg:h-full lg:w-full"
            />
            <p class="text-content text-xl font-medium text-gray-900">
              {{ $t("mobile.templateTasks") }}
            </p>
          </div>
        </div>
      </div>
    </div>
    <!--  -->
    <div
      class="mx-auto max-w-2xl px-4 py-16 sm:px-6 sm:py-24 lg:max-w-7xl lg:px-8"
    >
      <h2 class="text-2xl font-bold tracking-tight text-gray-900">
        {{ $t("mobile.warehousingData") }}
      </h2>
      <div
        class="mt-6 grid grid-cols-1 gap-x-6 gap-y-10 sm:grid-cols-2 lg:grid-cols-3 xl:gap-x-8"
      >
        <div
          class="group relative"
          @click="handleRouter('/mobile/warehousedatastorage/index')"
        >
          <div
            class="img-content aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80"
          >
            <img
              src="@/assets/mobile/menu/warehouse-data-storage.png"
              alt=""
              class="imgs h-full w-full object-cover object-center lg:h-full lg:w-full"
            />
            <p class="text-content text-xl font-medium text-gray-900">
              {{ $t("mobile.storageData") }}
            </p>
          </div>
        </div>
        <div
          class="group relative"
          @click="handleRouter('/mobile/warehousedatacontainer/index')"
        >
          <div
            class="img-content aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80"
          >
            <img
              src="@/assets/mobile/menu/warehouse-data-container.png"
              alt=""
              class="imgs h-full w-full object-cover object-center lg:h-full lg:w-full"
            />
            <p class="text-content text-xl font-medium text-gray-900">
              {{ $t("mobile.containerData") }}
            </p>
          </div>
        </div>
        <div
          class="group relative"
          @click="handleRouter('/mobile/warehousedatamaterial/index')"
        >
          <div
            class="img-content aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80"
          >
            <img
              src="@/assets/mobile/menu/warehouse-data-material.png"
              alt=""
              class="imgs h-full w-full object-cover object-center lg:h-full lg:w-full"
            />
            <p class="text-content text-xl font-medium text-gray-900">
              {{ $t("mobile.materialData") }}
            </p>
          </div>
        </div>
      </div>
    </div>
    <!--  -->
    <div
      class="mx-auto max-w-2xl px-4 py-16 sm:px-6 sm:py-24 lg:max-w-7xl lg:px-8"
    >
      <h2 class="text-2xl font-bold tracking-tight text-gray-900">
        {{ $t("mobile.warehousingTasks") }}
      </h2>
      <div
        class="mt-6 grid grid-cols-1 gap-x-6 gap-y-10 sm:grid-cols-2 lg:grid-cols-3 xl:gap-x-8"
      >
        <div
          class="group relative"
          @click="handleRouter('/mobile/warehouseworkareaa/index')"
        >
          <div
            class="img-content aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80"
          >
            <img
              src="@/assets/mobile/menu/warehouse-area-a.png"
              alt=""
              class="imgs h-full w-full object-cover object-center lg:h-full lg:w-full"
            />
            <p class="text-content text-xl font-medium text-gray-900">
              {{ $t("mobile.missionAreaA") }}
            </p>
          </div>
        </div>
        <div
          class="group relative"
          @click="handleRouter('/mobile/warehouseworkareab/index')"
        >
          <div
            class="img-content aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80"
          >
            <img
              src="@/assets/mobile/menu/warehouse-area-b.png"
              alt=""
              class="imgs h-full w-full object-cover object-center lg:h-full lg:w-full"
            />
            <p class="text-content text-xl font-medium text-gray-900">
              {{ $t("mobile.missionAreaB") }}
            </p>
          </div>
        </div>
        <div
          class="group relative"
          @click="handleRouter('/mobile/warehouseworkareac/index')"
        >
          <div
            class="img-content aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80"
          >
            <img
              src="@/assets/mobile/menu/warehouse-area-c.png"
              alt=""
              class="imgs h-full w-full object-cover object-center lg:h-full lg:w-full"
            />
            <p class="text-content text-xl font-medium text-gray-900">
              {{ $t("mobile.missionAreaC") }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.bg-white {
  height: 100%;
}

.text-content {
  z-index: 100;
  position: relative;
  bottom: 1rem;
  text-align: center;
  color: #333;
}

.img-content {
  display: flex;
  flex-direction: column;
  align-items: center; /* 水平居中 */
  justify-content: center; /* 垂直居中 */
}

.imgs {
  width: 80%;
}
.grow .main-content {
  margin: 0;
}
</style>
