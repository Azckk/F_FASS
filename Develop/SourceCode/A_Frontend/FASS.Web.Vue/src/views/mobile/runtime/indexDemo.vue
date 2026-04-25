<template>
  <div>
    <div class="control-btn">
      <el-button type="primary" @click="toggleDrawer(1)"
        >Open Drawer 1</el-button
      >
      <el-button type="primary" @click="toggleDrawer(2)"
        >Open Drawer 2</el-button
      >
      <el-button type="primary" @click="toggleDrawer(3)"
        >Open Drawer 3</el-button
      >
    </div>

    <SlideDrawer
      v-model="drawer1Open"
      :z-index="zIndex1"
      :is-active="activeDrawer === 1"
      @activate="setActiveDrawer(1)"
      @mouseenter="bringToFront(1)"
    >
      <span>Content for Drawer 1</span>
    </SlideDrawer>

    <SlideDrawer
      v-model="drawer2Open"
      :z-index="zIndex2"
      :is-active="activeDrawer === 2"
      @activate="setActiveDrawer(2)"
      @mouseenter="bringToFront(2)"
    >
      <span>Content for Drawer 2</span>
    </SlideDrawer>

    <SlideDrawer
      v-model="drawer3Open"
      :z-index="zIndex3"
      :is-active="activeDrawer === 3"
      @activate="setActiveDrawer(3)"
      @mouseenter="bringToFront(3)"
    >
      <span>Content for Drawer 3</span>
    </SlideDrawer>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import SlideDrawer from "./DraggableDrawer.vue";

const drawer1Open = ref(false);
const drawer2Open = ref(false);
const drawer3Open = ref(false);

const activeDrawer = ref(null);
const baseZIndex = 1000;

const zIndex1 = ref(baseZIndex);
const zIndex2 = ref(baseZIndex);
const zIndex3 = ref(baseZIndex);

const toggleDrawer = (drawerNumber: number) => {
  // Reset all zIndex values to baseZIndex
  zIndex1.value = baseZIndex;
  zIndex2.value = baseZIndex;
  zIndex3.value = baseZIndex;

  // Toggle the drawer state
  if (drawerNumber === 1) {
    if (!drawer1Open.value) {
      bringToFront(1);
    }
    drawer1Open.value = !drawer1Open.value;
  }
  if (drawerNumber === 2) {
    if (!drawer2Open.value) {
      bringToFront(2);
    }
    drawer2Open.value = !drawer2Open.value;
  }
  if (drawerNumber === 3) {
    if (!drawer3Open.value) {
      bringToFront(3);
    }
    drawer3Open.value = !drawer3Open.value;
  }
};

const bringToFront = (drawerNumber: number) => {
  zIndex1.value = baseZIndex;
  zIndex2.value = baseZIndex;
  zIndex3.value = baseZIndex;

  if (drawerNumber === 1) zIndex1.value = baseZIndex + 3;
  if (drawerNumber === 2) zIndex2.value = baseZIndex + 3;
  if (drawerNumber === 3) zIndex3.value = baseZIndex + 3;
};

const setActiveDrawer = (drawerNumber: number) => {
  activeDrawer.value = drawerNumber;
};
</script>

<style scoped>
.control-btn {
  position: absolute;
  z-index: 1000; /* Ensure buttons are above the drawers */
}
</style>
