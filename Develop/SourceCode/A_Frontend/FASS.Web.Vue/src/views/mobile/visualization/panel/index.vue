<script setup lang="ts">
import { ref, onMounted, onUnmounted } from "vue";
defineOptions({
  name: ""
});

const isDragging = ref(false);
const startX = ref(0);
const scrollLeft = ref(0);
const containerRef = ref(null);

const startDrag = event => {
  isDragging.value = true;
  startX.value = event.pageX - containerRef.value.offsetLeft;
  scrollLeft.value = containerRef.value.scrollLeft;
  containerRef.value.style.cursor = "grabbing";
};

const drag = event => {
  if (!isDragging.value) return;
  const x = event.pageX - containerRef.value.offsetLeft;
  const walk = (x - startX.value) * 2; // 2 is a sensitivity factor
  containerRef.value.scrollLeft = scrollLeft.value - walk;
};

const stopDrag = () => {
  if (isDragging.value) {
    isDragging.value = false;
    containerRef.value.style.cursor = "grab";
  }
};

// Add global event listeners when the component is mounted
onMounted(() => {
  document.addEventListener("mousemove", drag);
  document.addEventListener("mouseup", stopDrag);
});

// Remove global event listeners when the component is unmounted
onUnmounted(() => {
  document.removeEventListener("mousemove", drag);
  document.removeEventListener("mouseup", stopDrag);
});
const props = defineProps([
  "tagListData",
  "dataList",
  "handleUpdate",
  "tagList"
]);
</script>

<template>
  <div>
    <div class="content container">
      <div v-for="item in props.dataList" :key="item.id">
        <div
          :class="item.state"
          class="contentBox storage"
          :style="{
            // backgroundColor: item.tags[0]?.colour || '#eee'
            backgroundColor:
              item.containers?.length && item.materials?.length
                ? props.tagList.full.color
                : item.containers?.length && !item.materials?.length
                  ? props.tagList.empty.color
                  : !item.containers?.length && item.tags?.length
                    ? props.tagList.lid.color
                    : !item.containers?.length && !item.tags?.length
                      ? props.tagList.storage.color
                      : '#eee'
          }"
          @click="handleUpdate(item)"
        >
          <div>
            <div>
              {{ item.name || $t("table.unnamed") }}
            </div>
            <div>
              {{ item.containers[0]?.name || "" }}
            </div>
            <div
              v-if="item?.materials && item?.materials.length"
              class="text-sm"
            >
              {{ item?.materials[0]?.name }}
            </div>
            <div>
              <!-- <el-button @click="handleUpdate(item)">{{
                item.tags[0]?.name ? item.tags[0]?.name : $t("buttons.addTags")
              }}</el-button> -->
            </div>
          </div>
        </div>
      </div>
    </div>
    <div
      ref="containerRef"
      class="examples"
      @mousedown="startDrag"
      @mousemove="drag"
      @mouseup="stopDrag"
    >
      <!-- <div class="item" v-for="(item, index) in props.tagListData" :key="index">
        <div class="red itemBox" :style="{ backgroundColor: item.colour }" />
        <span>{{ item.name }}</span>
      </div> -->
    </div>
  </div>
</template>

<style lang="scss" scoped>
.storage {
  border-radius: 10px;
}
.content {
  width: 100%;
  height: 70vh;
  // background: #000000;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(80px, 1fr));
}
.contentBox {
  width: 80px;
  height: 150px;
  margin: 10px;
  margin-bottom: 0;
  text-align: center;
  font-size: 12px;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  background-color: #eee;
}
.red {
  background: #ff0000;
}
.yellow {
  background: yellow;
}
.blue {
  background: blue;
}
.seagreen {
  background: seagreen;
}
.examples {
  // position: relative;
  // bottom: 0px;
  margin: 0 auto;
  padding: 0 5px;
  // width: 600px;
  // border: 1px solid #ccc;
  overflow: hidden;
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  white-space: nowrap; /* 防止内容换行 */
  cursor: grab; /* 鼠标指针样式 */
  // 禁止文字被鼠标选中
  moz-user-select: -moz-none;
  -moz-user-select: none;
  -o-user-select: none;
  -khtml-user-select: none;
  -webkit-user-select: none;
  -ms-user-select: none;
  user-select: none;
  .item {
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
    text-align: center;
    font-size: 14px;
    margin: 14px;
    .itemBox {
      margin: 6px;
      width: 20px;
      height: 20px;
    }
  }
}
.examples:active {
  cursor: grabbing; /* 鼠标按下时指针样式 */
}
.red {
  background: #ff0000;
}
.yellow {
  background: yellow;
}
.blue {
  background: blue;
}
.seagreen {
  background: skyblue;
}
/* 对于 Webkit 浏览器 (Chrome, Safari) */
.container {
  overflow: auto; /* 允许滚动 */
  -ms-overflow-style: none; /* IE 和 Edge */
  scrollbar-width: none; /* Firefox */
}
.container::-webkit-scrollbar {
  display: none; /* Chrome, Safari 和 Opera */
}
</style>
