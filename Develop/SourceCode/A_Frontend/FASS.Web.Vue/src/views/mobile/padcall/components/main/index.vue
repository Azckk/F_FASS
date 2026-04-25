<script setup lang="ts">
import { onMounted, ref, watchEffect, watch } from "vue";
import { GetDictItemInLocal } from "@/utils/auth";
import { AddWork } from "@/api/mobile/padcall";
import { ElMessage } from "element-plus";
interface dataInterface {
  id: string;
  name: string;
  value: string;
  type: string;
  state: string;
  dic?: string;
  sortNumber: number;
}
const props = defineProps({
  dataList: Object as () => dataInterface[]
});
let dataInfo = ref();
// watch(props.dataList, () => {
//   dataInfo =
//   props.dataList.sort((a, b) => a.sortNumber - b.sortNumber);
// })
watchEffect(() => {
  dataInfo.value = props.dataList;
  if (dataInfo.value) {
    dataInfo.value.sort((a, b) => a.sortNumber - b.sortNumber);
  }

});
let options = [];
let optionsValue;
async function getContainerType() {
  // options = await GetDictItemInLocal("ContainerType");
  options = await GetDictItemInLocal("MaterialType");

  optionsValue = options[0];
}
function handleChange(e) {
  console.log(e);
  // console.log(id);
}
async function sendWork(e, callMode) {
  console.log("任务类型++++", callMode);

  if (!e.dic) e.dic = "";
  let data = {
    storageId: e.id,
    callMode,
    materialType: null,
    containerType: null,

    catrTypeId: ""
  };
  if (callMode == "EmptyFullExchange") {
    data.materialType = e.dic;
  } else {
    data.containerType = e.dic;
  }

  console.log("接口传参...", data);

  let res = await AddWork(data);
  if (res.code === 200) {
    ElMessage({
      message: "呼叫成功",
      type: "success"
    });
  }
}
onMounted(() => {
  getContainerType();
});
</script>

<template>
  <div class="container">
    <!-- <div class="kw">
      <h5>线边库位1</h5>
      <img src="" alt="" />
      <div class="btn">
        <el-button color="#b088f1" type="primary">取空容器</el-button>
        <el-button color="#b088f1" type="primary">送满料</el-button>
      </div>
      <div>
        <el-button>取满料</el-button>
        <el-button>送空容器</el-button>
      </div>
      <div>
        <el-button>呼叫</el-button>
      </div>
    </div> -->
    <el-card v-for="item in dataInfo" :key="item.id" class="card" style="">
      <template #header>
        <div class="card-header">
          <span style="height: 32px; line-height: 32px">{{ item.name }}</span>
          <el-select
            v-if="item.type !== 'Call' && item.type !== 'Fetch'"
            v-model="item.dic"
            :value="
              item.dic ||
              (item.dic = (
                options.find(opt => opt.code === 'Default') || {}
              ).code)
            "
            placeholder=""
            style="width: 100px"
            @change="handleChange"
          >
            <el-option
              v-for="i in options"
              :key="i.id"
              :label="i.name"
              :value="i.code"
            />
          </el-select>
        </div>
      </template>
      <div class="imgBox">
        <img
          v-if="item.state === 'EmptyContainer'"
          src="@/assets/mobile/padCall/矢量智能对象.png"
          alt=""
        />
        <img
          v-if="item.state === 'FullContainer'"
          src="@/assets/mobile/padCall/矢量智能对象(1).png"
          alt=""
        />
        <img
          v-if="item.state === 'NoneContainer'"
          src="@/assets/mobile/padCall/矢量智能对象(2).png"
          alt=""
        />
      </div>
      <div>
        <div v-if="item.type === 'Put'" class="btn">
          <!-- <el-button type="primary" @click="sendWork(item, 'EmptyOffline')"
            >取空容器</el-button
          >
          <el-button type="primary" @click="sendWork(item, 'FullOnline')"
            >呼叫满料</el-button
          > -->
          <el-button type="primary" @click="sendWork(item, 'EmptyFullExchange')"
            >取空放满
          </el-button>
        </div>
        <div v-else-if="item.type === 'Fetch'" class="btn">
          <el-button type="primary" @click="sendWork(item, 'FullOffline')"
            >送走满料</el-button
          >
          <el-button type="primary" @click="sendWork(item, 'EmptyOnline')"
            >送空容器</el-button
          >
        </div>
        <div v-else-if="item.type === 'Call'" class="btn">
          <el-button type="primary" @click="sendWork(item, 'Call')"
            >呼叫</el-button
          >
        </div>
      </div>
    </el-card>
  </div>
</template>

<style lang="scss" scoped>
.container {
  display: flex;
  flex-wrap: wrap;
  height: calc(100vh - 306px);
  overflow-x: auto;
}
.card {
  // width: 250px;
  width: 24%;
  height: 240px;
  margin-right: 5px;
  margin-bottom: 12px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
:deep(.el-card__header) {
  padding: 2px 5px;
  // height: 40px;
  // line-height: 40px;/
}
:deep(.el-card__body) {
  padding: 10px;
  // height: 40px;
  // line-height: 40px;/
}

.kw {
  height: 200px;
  width: 170px;
  background-color: skyblue;
}
.btn {
  width: 100%;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-around;
  button {
    width: 80px;
    height: 40px;
    color: #fff;
    margin: 0;
    margin-top: -10px;
    background: linear-gradient(0deg, #5c068c, #af50e4);
  }
}
.imgBox {
  height: 150px;
  display: flex;
  justify-content: center;
  img {
    height: 85%;
  }
}
</style>
