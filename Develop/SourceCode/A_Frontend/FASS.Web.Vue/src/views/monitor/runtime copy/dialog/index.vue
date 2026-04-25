<script setup lang="ts">
import { ref } from "vue";
defineOptions({
  name: ""
});
const props = defineProps({
  visible: {
    type: Boolean,
    default: true
  },
  msg: {
    type: String
    // required: true
  },
  onClose: {
    type: Function,
    required: true
  },
  event: {
    type: Object
  }
});
// console.log(props.msg);
const offsetX = ref(0);
const offsetY = ref(0);
const isDragging = ref(false);
const popup = ref(null);
function handleClose() {
  props.onClose();
}

const onMouseDown = event => {
  isDragging.value = true;
  offsetX.value = event.clientX - popup.value.getBoundingClientRect().left;
  offsetY.value = event.clientY - popup.value.getBoundingClientRect().top;
  document.addEventListener("mousemove", onMouseMove);
  document.addEventListener("mouseup", onMouseUp);
};

const onMouseMove = event => {
  if (isDragging.value) {
    popup.value.style.left = `${event.clientX - offsetX.value}px`;
    popup.value.style.top = `${event.clientY - offsetY.value}px`;
  }
};

const onMouseUp = () => {
  isDragging.value = false;
  document.removeEventListener("mousemove", onMouseMove);
  document.removeEventListener("mouseup", onMouseUp);
};
</script>

<template>
  <transition v-if="props.visible" name="slide">
    <div ref="popup" class="popup">
      <div class="header" @mousedown="onMouseDown">
        <span>{{ msg }}</span>
        <!-- <span class="close-btn" @click="handleClose">x</span> -->
        <el-button type="primary" class="close-btn" @click="handleClose"
          >X</el-button
        >
      </div>
      <div class="content">
        <slot name="info" :data="props.event"></slot>
      </div>
    </div>
  </transition>
</template>

<style lang="scss" scoped>
.header {
  cursor: move;
}
.popup {
  position: fixed;
  top: 100px;
  right: 20px;
  width: 400px;
  min-width: 300px;
  padding: 20px;
  background-color: white;
  border: 1px solid #ccc;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  transform: translateX(0%);
}
.slide-enter-active,
.slide-leave-active {
  transition: transform 0.5s;
}

.slide-enter-from,
.slide-leave-to {
  transform: translateX(100%);
}
.close-btn {
  position: absolute;
  top: 10px;
  right: 10px;
  cursor: pointer;
}
</style>
