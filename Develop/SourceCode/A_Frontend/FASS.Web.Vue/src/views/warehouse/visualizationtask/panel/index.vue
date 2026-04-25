<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch, watchEffect, nextTick } from "vue";
import { useHook } from "./utils/hook";
import Menu from "../menu/index.vue";
defineOptions({
  name: ""
});

const props = defineProps([
  "tagListData",
  "dataList",
  "tagList",
  "handleMaterial",
  "handleContainer",
  "handleSetStartPoin"
]);
// 测试canvas fabric
const conRef = ref(null);

const carDataList = ref([
  // {
  //   x: 120,
  //   y: 40,
  //   offsetx: 0,
  //   offsety: 0,
  //   code: "1-1-1", // 文本name
  //   state: "FullContainer", //状态颜色
  //   data: {
  //     areaId: "3a1533b8-9c2c-7aa5-ea74-f9c27c82e98e",
  //     nodeId: "204",
  //     nodeCode: "204",
  //     code: "1001",
  //     name: "1001",
  //     type: "Default",
  //     state: "FullContainer",
  //     isLock: false,
  //     barcode: "1001",
  //     areaCode: "1001",
  //     containers: [
  //       {
  //         areaId: "3a1533b8-9c2c-7aa5-ea74-f9c27c82e98e",
  //         code: "1001",
  //         name: "",
  //         type: "Default",
  //         state: "FullMaterial",
  //         isLock: true,
  //         barcode: "",
  //         length: 0,
  //         width: 0,
  //         height: 0,
  //         areaCode: "1001",
  //         sortNumber: 0,
  //         isEnable: true,
  //         isDelete: false,
  //         createAt: "2024/09/24 09:01:37",
  //         updateAt: "2024/09/24 16:28:25",
  //         id: "3a1533b9-2b61-e12b-61f1-e8162021e0b3"
  //       }
  //     ],
  //     coordinate: [120, 40],
  //     materials: [],
  //     sortNumber: 0,
  //     isEnable: true,
  //     isDelete: false,
  //     createAt: "2024/09/24 09:01:25",
  //     updateAt: "2024/09/24 16:28:25",
  //     id: "3a1533b8-fc67-4638-1ee7-3fb43c7b321f"
  //   }
  // },
  {
    areaId: "3a141a4f-910f-7f5e-39d4-229b0690af88",
    nodeId: "1",
    nodeCode: "1",
    code: "1",
    name: "离心机台1",
    type: "Default",
    state: "NoneContainer",
    isLock: false,
    barcode: "",
    areaCode: "LX",
    containers: [],
    coordinate: [200, 20],
    // offsetCoordinate: [12, 23],
    // textCoordinate: [3, 4],
    coefficient: 2,
    sortNumber: 0,
    isEnable: true,
    isDelete: false,
    createAt: "2024/08/01 16:43:35",
    updateAt: "2024/08/21 14:58:01",
    id: "3a141f48-b641-1dda-f72a-b8260b559981"
  },
  {
    name: "1001",
    // offsetCoordinate: [12, 23],
    // textCoordinate: [3, 4],
    coordinate: [0, 0],
    // coefficient: 2,
    state: ""
  }
]);

const {
  isTrue,
  areaList,
  activeAreaId,
  coefficient,
  textX,
  textY,
  // dataList,
  cardStates,
  handleEdit,
  handleSave,
  canvasInit,
  handleGetList,
  GetAreaListFn,
  handleResetAll,
  handleZoomUp,
  handleZoomDown,
  updateColor
} = useHook();

watchEffect(() => {
  if (props.dataList) {
    cardStates.value = null;
  }
});
nextTick(() => {
  watchEffect(() => {
    if (props.dataList) {
      cardStates.value = null;
      handleGetList(activeAreaId.value);
    }
  });
});
const handleChange = (value: number) => {
  canvasInit();
};
// let timer = null;
// watchEffect(() => {
//   if (isTrue.value) {
//     clearInterval(timer);
//   } else {
//     timer = setInterval(() => {
//       updateColor();
//     }, 5000);
//   }
// });
onMounted(() => {
  GetAreaListFn();
  // timer = setInterval(() => {
  //   updateColor();
  // }, 5000);
});
// onUnmounted(() => {
//   // clearInterval(timer);
// });
</script>

<template>
  <Menu :areaList="areaList" :handleGetList="handleGetList" />
  <div class="operation">
    <span v-if="isTrue">
      间距：<el-input-number
        v-model="coefficient"
        class="inputNumber"
        :step="0.05"
        @change="handleChange"
      />
      文本偏移：
      <el-input-number
        v-model="textY"
        class="inputNumber"
        :step="1"
        @change="handleChange"
      />
      <el-input-number
        v-model="textX"
        class="inputNumber"
        :step="1"
        @change="handleChange"
      />
    </span>
    <el-button
      class="btn"
      :class="isTrue ? 'bj' : ''"
      type="primary"
      @click="handleEdit"
      >{{ isTrue ? "放弃" : "编辑" }}</el-button
    >

    <el-button v-if="isTrue" class="btn save" type="primary" @click="handleSave"
      >保存</el-button
    >
    <el-button
      v-if="isTrue"
      class="btn init"
      type="danger"
      @click="handleResetAll"
      >初始化</el-button
    >
    <div :style="isTrue ? 'margin-top: 10px;' : 'display:inline-block;'">
      <el-button class="btn" type="primary" @click="handleZoomDown"
        >缩小</el-button
      >
      <el-button class="btn" type="primary" @click="handleZoomUp"
        >放大</el-button
      >
    </div>
    <el-button
      v-if="!isTrue"
      class="btn refresh"
      type="primary"
      @click="updateColor()"
      >刷新</el-button
    >
    <div id="main" ref="conRef" class="content container" />
  </div>
  <el-dialog v-model="cardStates" title="" top="30vh" width="40%">
    <div class="dialog-centent">
      <div class="dialog-centent-item" @click="handleContainer(cardStates)">
        <div class="dialog-centent-item-title">容器状态</div>
        <img
          src="../../../../assets/visualizationtask/containerSetting.png"
          alt=""
        />

        <el-button class="dialog-centent-item-title" type="info"
          >设置</el-button
        >
      </div>
      <div class="dialog-centent-item" @click="handleMaterial(cardStates)">
        <div class="dialog-centent-item-title">物料状态</div>
        <img
          src="../../../../assets/visualizationtask/materialSetting.png"
          alt=""
        />
        <el-button class="dialog-centent-item-title" type="info"
          >设置</el-button
        >
      </div>
      <div class="dialog-centent-item" @click="handleSetStartPoin(cardStates)">
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
</template>

<style lang="scss" scoped>
.dialog-centent {
  display: flex;
  padding: 10px;
  width: 100%;
  justify-content: center;
  .dialog-centent-item {
    text-align: center;
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
  // height: 53vh;
  height: calc(100vh - 350px);
  // background: #000000;
  // display: flex;
  // flex-wrap: wrap;
  overflow: scroll !important;
  // align-content: flex-start;
  display: grid;
  grid-template-columns: repeat(8, 12.3%);
}
.contentBox {
  // background: #42c457;
  width: 90%;
  height: 90%;
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
.list {
  width: 320px;
  height: 700px;
  background-color: #eee;
  position: fixed;
  top: 110px;
  left: 240px;
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
.inputNumber {
  width: 118px;
  height: 37px;
}
.btn {
  width: 80px;
  height: 37px;
}
.operation {
  position: relative;
}
.refresh {
  position: absolute;
  right: 10px;
}
.bj {
  background: rgba(255, 130, 0, 1);
  color: #ffffff;
  border-radius: 5px;
  margin-left: 10px;
}
.save {
  background: rgba(130, 182, 255, 1);
  color: #ffffff;
  border-radius: 5px;
  margin-left: 10px;
}
.init {
  background: rgba(62, 165, 208, 1);
  color: #ffffff;
  border-radius: 5px;
  margin-left: 10px;
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
