<template>
  <transition name="slide">
    <div
      v-show="isOpen"
      ref="drawerRef"
      :class="{ 'drawer-open': isOpen }"
      :style="{ zIndex: zIndex }"
      class="drawer"
    >
      <div class="drawer-header" @mousedown="startDrag">
        <span>{{ viewTitle }} {{ viewMainTitle }}</span>
        <div>
          <el-button
            v-if="title == '车队监控' || title == '报警监控'"
            text
            @click="
              handleRouter(
                title == '车队监控'
                  ? '/monitor/car/index'
                  : '/monitor/alarmmdcs/index'
              )
            "
            >{{ $t("table.viewMore") }}</el-button
          >
          <el-button @click="handleClose">X</el-button>
        </div>
      </div>
      <div class="drawer-content container">
        <slot />
      </div>
    </div>
  </transition>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick, watch, watchEffect } from "vue";
import { useRouter } from "vue-router";
import { useMyI18n } from "@/plugins/i18n";
const router = useRouter();
const { t, locale } = useMyI18n();
const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  },
  zIndex: {
    type: Number,
    default: 1000
  },
  title: {
    type: String,
    default: "Drawer"
  },
  mainTitle: {
    type: String,
    default: ""
  }
});

let viewTitle = ref(props.title);
let viewMainTitle = ref(props.mainTitle);
watchEffect(() => {
  viewMainTitle.value = props.mainTitle;
});

watchEffect(() => {
  if (locale.value === "zh") {
    viewTitle.value = props.title;
  } else {
    switch (props.title) {
      case "任务监控":
        viewTitle.value = "Task Monitoring";
        break;
      case "报警监控":
        viewTitle.value = "Alarm monitoring";
        break;
      case "车队监控":
        viewTitle.value = "Fleet monitoring";
        break;
      case "车辆":
        viewTitle.value = "Car";
        break;
      case "站点":
        viewTitle.value = "Node";
        break;
      case "路线":
        viewTitle.value = "Line";
        break;
      default:
        viewTitle.value = props.title;
        break;
    }
  }
});

const emit = defineEmits(["update:modelValue"]);
const isOpen = ref(props.modelValue);
const drawerRef = ref(null);
let drawerEl = null;

let startX = 0;
let startY = 0;
let startLeft = 0;
let startTop = 0;
let isDragging = false;

watch(
  () => props.modelValue,
  newValue => {
    isOpen.value = newValue;
  }
);

const handleClose = () => {
  isOpen.value = false; // Set isOpen to false locally
  emit("update:modelValue", false); // Emit event to update parent component
};

const startDrag = event => {
  if (drawerEl) {
    startX = event.clientX;
    startY = event.clientY;
    startLeft = drawerEl.offsetLeft;
    startTop = drawerEl.offsetTop;

    isDragging = true;

    document.addEventListener("mousemove", onDrag);
    document.addEventListener("mouseup", stopDrag);
  }
};

const onDrag = event => {
  if (isDragging && drawerEl) {
    requestAnimationFrame(() => {
      const deltaX = event.clientX - startX;
      const deltaY = event.clientY - startY;
      drawerEl.style.left = `${startLeft + deltaX}px`;
      drawerEl.style.top = `${startTop + deltaY}px`;
    });
  }
};

const stopDrag = () => {
  if (isDragging) {
    isDragging = false;
    document.removeEventListener("mousemove", onDrag);
    document.removeEventListener("mouseup", stopDrag);
    if (drawerEl) {
      // Remove and re-add the class to trigger the animation
      drawerEl.classList.remove("drawer-open");
      void drawerEl.offsetWidth; // Trigger a reflow
      drawerEl.classList.add("drawer-open");
    }
  }
};

function handleRouter(routerLink: string) {
  // const router = useRouter();
  // router.push(routerLink);
  if (router) {
    router.push(routerLink);
  } else {
    console.error("Router is not available");
  }
}

watchEffect(() => {
  if (isOpen.value && drawerEl) {
    drawerEl.style.position = "fixed";
    drawerEl.classList.add("slide-enter");
  } else if (!isOpen.value && drawerEl) {
    console.log("移除弹框～～");
    // localStorage.removeItem("carCode");
  }
});

onMounted(() => {
  nextTick(() => {
    drawerEl = drawerRef.value;
    if (drawerEl) {
      drawerEl.style.position = "fixed";
      drawerEl.style.top = "50px";
      drawerEl.style.right = isOpen.value ? "0" : "350px";
      drawerEl.style.width = "350px";
      drawerEl.style.transition = "transform 0.3s ease, right 0.3s ease";
    }
  });
});

onUnmounted(() => {
  document.removeEventListener("mousemove", onDrag);
  document.removeEventListener("mouseup", stopDrag);
});
</script>

<style scoped>
* {
  user-select: none;
}

.drawer {
  position: fixed;
  width: 350px;
  right: 0;
  background: #d9d9d9;
}

.drawer-open {
  transform: translateX(100%);
}

.drawer-header {
  cursor: move;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #f5f5f5;
  padding: 10px;
}

.close-btn {
  cursor: pointer;
}

.slide-enter-active,
.slide-leave-active {
  transition: transform 0.3s ease;
}

.slide-enter {
  transform: translateX(100%);
}

.slide-enter-to {
  transform: translateX(95%);
}

.slide-leave-to {
  transform: translateX(110%);
}

.container {
  overflow: auto;
  -ms-overflow-style: none;
  scrollbar-width: none;
}

.container::-webkit-scrollbar {
  display: none;
}
</style>
