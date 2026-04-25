<script setup lang="ts">
import { ref, onMounted, onUnmounted, watchEffect } from "vue";
defineOptions({
  name: ""
});

const isDragging = ref(false);
const startX = ref(0);
const scrollLeft = ref(0);
const containerRef = ref(null);
const hoveFlag = ref(false);

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

// 定义每个卡片的状态
const cardStates = ref<{ [key: string]: boolean }>({});

// 初始化卡片状态
const initCardStates = (dataList: any[]) => {
  cardStates.value = dataList.reduce((acc, item) => {
    acc[item.id] = false;
    return acc;
  }, {});
};

const showBtn = (itemId: string, show: boolean) => {
  cardStates.value[itemId] = show;
};
// Add global event listeners when the component is mounted
onMounted(() => {
  document.addEventListener("mousemove", drag);
  document.addEventListener("mouseup", stopDrag);
  initCardStates(props.dataList); // 添加或更新卡片状态
});

// Remove global event listeners when the component is unmounted
onUnmounted(() => {
  document.removeEventListener("mousemove", drag);
  document.removeEventListener("mouseup", stopDrag);
});
const props = defineProps([
  "tagListData",
  "dataList",
  "tagList",
  "handleMaterial",
  "handleContainer",
  "handleSetStartPoin"
]);
watchEffect(() => {
  console.log(props.dataList);
});
</script>

<template>
  <div>
    <div class="content container">
      <div v-for="item in props.dataList" :key="item.id">
        <div
          :class="
            item.state === 'NoneContainer' && !item.isLock ? 'font-b' : 'font-w'
          "
          class="contentBox storage"
          :style="{
            backgroundColor: item.isLock
              ? props.tagList.storage.color
              : item.containers?.length && item.materials?.length
                ? props.tagList.full.color
                : item.containers?.length && !item.materials?.length
                  ? props.tagList.empty.color
                  : !item.containers?.length && item.tags?.length
                    ? props.tagList.lid.color
                    : '#eee'
          }"
          @click="cardStates[item.id] = !cardStates[item.id]"
        >
          <!-- @mouseenter="cardStates[item.id] = true"
          @mouseleave="cardStates[item.id] = false" -->

          <el-dialog
            v-model="cardStates[item.id]"
            title=""
            top="30vh"
            width="40%"
          >
            <div class="dialog-centent">
              <div class="dialog-centent-item" @click="handleContainer(item)">
                <div class="dialog-centent-item-title">容器状态</div>
                <img
                  src="../../../../assets/visualizationtask/containerSetting.png"
                  alt=""
                />

                <el-button class="dialog-centent-item-title" type="info"
                  >设置</el-button
                >
              </div>
              <div class="dialog-centent-item" @click="handleMaterial(item)">
                <div class="dialog-centent-item-title">物料状态</div>
                <img
                  src="../../../../assets/visualizationtask/materialSetting.png"
                  alt=""
                />
                <el-button class="dialog-centent-item-title" type="info"
                  >设置</el-button
                >
              </div>
              <div
                class="dialog-centent-item"
                @click="handleSetStartPoin(item)"
              >
                <div class="dialog-centent-item-title">站点状态</div>
                <img
                  src="../../../../assets/visualizationtask/siteSetting.png"
                  alt=""
                />
                <el-button class="dialog-centent-item-title" type="info"
                  >设置</el-button
                >
              </div>
            </div>
          </el-dialog>

          <!-- <el-dialog
            v-model="cardStates[item.id]"
            title="设置"
            top="30vh"
            width="40%"
          >
            <div class="button-container">
              <el-button
                style="width: 33%"
                type="info"
                @click="handleContainer(item)"
                >设置容器</el-button
              >
              <el-button
                style="width: 33%"
                type="info"
                @click="handleMaterial(item)"
                >设置物料</el-button
              >
              <el-button
                style="width: 33%"
                type="info"
                @click="handleSetStartPoin(item)"
                >任务站点</el-button
              >
            </div>
          </el-dialog> -->
          <div v-if="!cardStates[item.id]">
            <!-- <div v-if="false"> -->
            <div>
              {{ item.name || $t("table.unnamed") }}
            </div>
            <div v-if="item?.containers && item?.containers.length">
              {{ item?.containers[0]?.name }}
            </div>
            <div v-if="item?.materials && item?.materials.length">
              {{ item?.materials[0]?.name }}
            </div>
            <div />
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
.dialog-centent {
  display: flex;
  padding: 10px;
  width: 100%;
  justify-content: center;
  .dialog-centent-item {
    width: 30%;
    // background-color: aqua;
    padding: 10px;
  }
  .dialog-centent-item-title {
    margin: 10px 0;
  }
}

.button-container {
  width: 100%;
  display: flex;
  //flex-direction: column;
  // align-items: center;
  text-align: center;
  gap: 10px; /* 增加按钮之间的间距 */
  padding: 10px; /* 添加内边距，使按钮不紧贴卡片边缘 */
}

.el-button {
  // border-radius: 8px; /* 圆角效果 */
  // box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); /* 添加阴影效果 */
  transition:
    background-color 0.3s,
    box-shadow 0.3s; /* 添加平滑过渡效果 */
  margin: 0;
  width: 85px;
  height: 30px;
  background: #ffffff;
  border-radius: 2px;
  color: #333333;
  font-size: 13px;
  // border: 1px solid #acacac;
}

.el-button:hover {
  color: #eee;
  background-color: #7710d6; /* 鼠标悬停时的背景颜色 */
  // box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3); /* 鼠标悬停时的阴影效果 */
}

.contentBox .text-sm {
  font-size: 12px;
  color: #666;
}

.storage {
  border-radius: 15px;
}
.content {
  width: 100%;
  height: 53vh;
  // background: #000000;
  display: flex;
  flex-wrap: wrap;
  overflow: scroll !important;
  align-content: flex-start;
}
.contentBox {
  // background: #42c457;
  width: 120px;
  height: 140px;
  margin: 10px;
  margin-bottom: 0;
  text-align: center;
  font-size: 14px;
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
  max-width: 3000px;
  overflow: auto; /* 允许滚动 */
  -ms-overflow-style: none; /* IE 和 Edge */
  scrollbar-width: none; /* Firefox */
}
.container::-webkit-scrollbar {
  display: none; /* Chrome, Safari 和 Opera */
}
.font-b {
  color: #333;
}
.font-w {
  color: #ffffff;
}
</style>
