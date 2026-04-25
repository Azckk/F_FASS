<script setup lang="ts">
import { onMounted, ref, watchEffect } from "vue";
import { GetDictItemInLocal } from "@/utils/auth";
import { AddWork2 } from "@/api/mobile/padcall";
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
  dataList: Object as () => dataInterface[],
  HCGW: {},
  transformArrayData: {},
  tabsList: {}
});
let dataInfo = ref();
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

let kwlist = ref();
function handleChangeArea(e, item) {
  const selectedArea = props.transformArrayData[e];
  if (selectedArea) {
    item.filteredKwlist = selectedArea;
  } else {
    item.filteredKwlist = [];
  }
}
function handleChangeAreaHJ(e, item) {
  const selectedArea = props.transformArrayData[e];
  console.log(selectedArea);
  if (selectedArea) {
    item.filteredKwlistHJ = selectedArea;
  } else {
    item.filteredKwlist = [];
  }
}
async function sendWork(e, callMode, type) {
  if (!e.dic) e.dic = "";
  let data = {
    storageId: e.id,
    callMode,
    materialType: null,
    containerType: null,
    catrTypeId: "",
    srcStorageId: null,
    destStorageId: null
  };
  if (callMode == "EmptyFullExchange") {
    data.materialType = e.dic;
  } else {
    data.containerType = e.dic;
  }
  if (type == "fx") {
    data.srcStorageId = e.id;
    data.destStorageId = e.kwId;
  } else if (type == "hj") {
    data.srcStorageId = e.hjkwId;
    data.destStorageId = e.id;
  }
  console.log("接口传参...", data);

  if (!data.srcStorageId || !data.destStorageId) {
    ElMessage({
      message: "请选择库位起点或者终点",
      type: "warning"
    });
  } else {
    let res = await AddWork2(data);
    if (res.code === 200) {
      ElMessage({
        message: "呼叫成功",
        type: "success"
      });
    }
  }
}
onMounted(() => {
  getContainerType();
});
</script>

<template>
  <div class="container">
    <el-card v-for="item in dataInfo" :key="item.id" class="card" style="">
      <template #header>
        <div class="card-header">
          <span style="height: 32px; line-height: 32px; font-size: 32px">{{
            item.name
          }}</span>
        </div>
      </template>
      <div class="imgBox">
        <div v-if="item.type === 'Put'" style="width: 100%">
          <div style="display: flex; height: 100%">
            <div class="discharged" style="">
              <div class="dischargedTitle">
                <span style="margin-left: 5px; color: #5c068c">放行</span>
              </div>

              <div style="display: flex; margin-bottom: 5px">
                <div class="dischargedLabel" style="">
                  <img
                    src="@/assets/mobile/customizedpadcall/start.png"
                    alt=""
                    style="width: 18px; height: 22px; margin-right: 5px"
                  />
                  起点
                </div>
                <div class="tagSty">当前库位</div>
              </div>

              <div class="call">
                <!-- <div style="width: 40%">终点</div> -->
                <div class="dischargedLabel">
                  <img
                    src="@/assets/mobile/customizedpadcall/stop.png"
                    alt=""
                    style="width: 18px; height: 22px; margin-right: 5px"
                  />
                  终点
                </div>
                <div class="callSelect">
                  <div>
                    <!-- 第一个下拉列表 -->
                    <el-select
                      v-model="item.fxDic"
                      placeholder="请选择库区"
                      size="large"
                      @change="handleChangeArea($event, item)"
                    >
                      <el-option
                        v-for="area in tabsList"
                        :key="area.id"
                        :label="area[0].areaName"
                        :value="area[0].areaName"
                        style="height: 100%"
                      />
                    </el-select>
                  </div>

                  <div>
                    <!-- 第二个下拉列表 -->
                    <el-select
                      v-model="item.kwId"
                      placeholder="请选择库位"
                      size="large"
                      style="margin-top: 10px; width: 100%"
                    >
                      <el-option
                        v-for="kw in item.filteredKwlist"
                        :key="kw.id"
                        :label="kw.name"
                        :value="kw.id"
                      />
                    </el-select>
                  </div>
                </div>
              </div>
              <div class="btn" style="text-align: center">
                <el-button
                  type="primary"
                  style="margin-top: 10px; width: 144px"
                  @click="sendWork(item, 'EmptyOffline', 'fx')"
                  >放行</el-button
                >
              </div>
            </div>
            <div style="width: 50%; padding-left: 5px">
              <div style="border-left: 5px solid rgba(92, 6, 140, 0.35)">
                <span style="margin-left: 5px; color: #5c068c">呼叫</span>
              </div>

              <div style="display: flex">
                <div class="dischargedLabel">
                  <img
                    src="@/assets/mobile/customizedpadcall/start.png"
                    alt=""
                    style="width: 18px; height: 22px; margin-right: 5px"
                  />
                  起点
                </div>
                <div class="callSelect">
                  <!-- 第一个下拉列表 -->
                  <el-select
                    v-model="item.hjDic"
                    placeholder="请选择库区"
                    size="large"
                    @change="handleChangeAreaHJ($event, item)"
                  >
                    <el-option
                      v-for="area in tabsList"
                      :key="area.id"
                      :label="area[0].areaName"
                      :value="area[0].areaName"
                    />
                  </el-select>

                  <!-- 第二个下拉列表 -->
                  <el-select
                    v-model="item.hjkwId"
                    placeholder="请选择库位"
                    size="large"
                    style="margin-top: 10px"
                  >
                    <el-option
                      v-for="kw in item.filteredKwlistHJ"
                      :key="kw.id"
                      :label="kw.name"
                      :value="kw.id"
                    />
                  </el-select>
                </div>
              </div>

              <div style="display: flex; margin-top: 5px">
                <div class="dischargedLabel">
                  <img
                    src="@/assets/mobile/customizedpadcall/stop.png"
                    alt=""
                    style="width: 18px; height: 22px; margin-right: 5px"
                  />
                  终点
                </div>
                <div class="tagSty">当前库位</div>
              </div>
              <div class="btn" style="text-align: center">
                <el-button
                  type="primary"
                  style="margin-top: 10px; width: 144px"
                  @click="sendWork(item, 'EmptyOffline', 'hj')"
                  >呼叫</el-button
                >
              </div>
            </div>
          </div>
        </div>
        <!-- <div v-else>
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
        </div> -->
      </div>
      <div>
        <div v-if="item.type === 'Put'" class="btn">
          <!-- <el-button type="primary" @click="sendWork(item, 'EmptyFullExchange')"
            >取空放满
          </el-button> -->
        </div>
        <div v-else-if="item.type === 'Fetch'" class="btn">
          <el-button type="primary" @click="sendWork(item, 'FullOffline', '')"
            >送走满料</el-button
          >
          <el-button type="primary" @click="sendWork(item, 'EmptyOnline', '')"
            >送空容器</el-button
          >
        </div>
        <div v-else-if="item.type === 'Call'" class="btn">
          <el-button type="primary" @click="sendWork(item, 'Call', '')"
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
  align-content: flex-start;
  height: calc(100vh - 306px);
  font-size: 18px;
}
.card {
  // width: 250px;
  width: 48%;
  height: 340px;
  margin-right: 5px;
  margin-bottom: 12px;
  color: #262626;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
:deep(.el-card__header) {
  padding: 2px 5px;
  // border: none;
  // height: 40px;
  // line-height: 40px;/
}
:deep(.el-card__body) {
  padding: 10px;
  height: 100%;
  // line-height: 40px;/
}
:deep(.el-select__wrapper) {
  height: 50px;
  font-size: 18px;
}
:deep(.el-select) {
  border: 2px solid #722ed1;
  border-radius: 5px;
  box-sizing: border-box;
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
    height: 70px;
    font-size: 28px;
    color: #fff;
    margin: 0;
    margin-top: -10px;
    background: linear-gradient(0deg, #5c068c, #af50e4);
  }
}
.imgBox {
  height: 100%;
  width: 100%;
  display: flex;
  justify-content: center;
  img {
    height: 85%;
  }
}
.tagSty {
  width: 60%;
  height: 40px;
  background: #d7d7d7;
  padding-left: 15px;
  line-height: 40px;
  border-radius: 4px;
  font-size: 15px;
}

.dischargedTitle {
  border-left: 5px solid rgba(92, 6, 140, 0.35);
}
.discharged {
  width: 50%;
  border-right: 1px solid #ccc;
  height: 100%;
}
.dischargedLabel {
  width: 70px;
  font-size: 18px;
  margin-top: 3px;
  display: flex;
  margin-top: 10px;
}

.call {
  display: flex;
}
.callSelect {
  width: 60%;
}
</style>
