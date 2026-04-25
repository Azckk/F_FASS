<script setup lang="ts">
import { ref, reactive, watchEffect, onMounted, watch, onUnmounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { useMyI18n } from "@/plugins/i18n";
import { message } from "@/utils/message";
import {
  getNodeList,
  getChargeList,
  ActionInit,
  ActionForceStop,
  // ActionRepair,
  ActionBlown,
  ActionCarGoToSomePlace,
  EnableNode,
  DisableNode,
  GetNodeInfo,
  ActionReStart,
  GetCarInfo,
  GetPcHardware,
  ActionStopReceiveTask,
  ActionGoToStandby,
  ActionGoToCharge,
  ActionStopOrStart
} from "@/api/monitor/caraction";
import { formRules } from "./rule";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import Pointer from "@iconify-icons/ep/pointer";
import { useMapStore } from "@/store/modules/map";
const { t, locale } = useMyI18n();
const props = defineProps({
  visible: {
    type: Boolean
    // default: true
  },
  msg: {
    type: String
    // required: true
  },
  onClose: {
    type: Function
    // required: true
  },
  data: {
    type: Object
  }
});
const dialogVisible = ref(false);
const formRef = ref();
const sendData = ref({
  side: "",
  carId: "",
  targetNodeId: Number(props.data.id) ? props.data.id : "请从地图选择站点"
});
const loading = ref(false);
const title = ref();
const carInfo = ref({
  code: "",
  name: "",
  type: "",
  ipAddress: "",
  port: 0,
  carTypeName: "",
  battery: 0,
  x: 0,
  y: 0,
  theta: 0,
  speed: 0,
  currNodeCode: "",
  nextNodeCode: "",
  isAlarm: false,
  trafficMessage: "",
  blockedBy: [],
  isBlockedBy: false,
  isOnline: true
});
const carPcInfo = ref({
  Cpu: "",
  Gpu: "",
  Tmp: "",
  Memory: "",
  NetW: ""
});
const store = useMapStore();
defineOptions({
  name: ""
});

watchEffect(async () => {
  // console.log("props.data", props.data.code.text);
  // console.log("props.data.id", props.data.id);
  if (props.data.code.text && props.msg == "车辆") {
    await getCarInfoList();
  }
});

const pagination = reactive({
  total: 0,
  pageSize: 9999,
  currentPage: 1,
  background: true
});
function handlePageParam() {
  return {
    pageParam: JSON.stringify({
      where: [],
      order: [{ field: "createAt", sequence: "DESC" }],
      number: pagination.currentPage,
      size: pagination.pageSize
    })
  };
}
const NodeState = ref();
async function getNodeStateFn() {
  let { data } = await GetNodeInfo({ nodeCode: props.data.code.text });
  NodeState.value = data.nodeState;
}
watchEffect(async () => {
  if (
    props.msg == "站点" &&
    (props.data.code.text || props.data.code.text == 0)
  ) {
    await getNodeStateFn();
  }
});
const popup = ref(null);

// 车辆-快捷操作
const handleClickCar = event => {
  let target = event.target;
  while (
    target &&
    target !== event.currentTarget &&
    !target.getAttribute("data-action")
  ) {
    target = target.parentNode;
  }
  if (target && target.getAttribute("data-action")) {
    const action = target.getAttribute("data-action");
    switch (action) {
      case "goWhere":
        goWhere();
        break;
      case "goCharging":
        goCharging();
        break;
      case "goStartPoint":
        goStartPoint();
        break;
      case "initialization":
        initialization();
        break;
      case "Pause/Resume":
        Resume();
        break;
      case "endTask":
        endTask();
        break;
      case "completelyOffline":
        completelyOffline();
        break;
      case "stopTask":
        stopTask();
        break;
      case "Restart":
        Restart();
        break;
      default:
        console.log("Unknown action:", action);
    }
  }
};

async function goWhere() {
  // console.log("goWhere");
  await getNodeListFn(); // 获取站点
  locale.value == "zh" ? (title.value = "去某地") : (title.value = "goWhere");
  dialogVisible.value = true;
}
// async function goCharging() {
//   // console.log("goCharging");
//   await getChargeListFn(); // 获取充电站点
//   locale.value == "zh"
//     ? (title.value = "去充电")
//     : (title.value = "goCharging");
//   dialogVisible.value = true;
// }

function goCharging() {
  // console.log("goStartPoint");
  ElMessageBox.confirm(t("buttons.goCharging"))
    .then(async () => {
      let res = await ActionGoToCharge({ carCode: props.data.code.text });
      if (res.code == 200 && res.data == "OK") {
        ElMessage({
          message: `${t("buttons.car")}${props.data.code.text}${t("buttons.goCharging")}！`,
          type: "success"
        });
      }
    })
    .catch(() => {
      // catch error
    });
}

// 去某处确认方法
async function sendGoWhere() {
  // console.log("发送去某地请求～～～", props.data);
  let data = JSON.stringify({
    id: props.data.id, // 站点id
    carCode: localStorage.getItem("carCode") // 车辆code
  });
  await formRef.value.validate(async valid => {
    // if (valid) {
    if (!Number(props.data.id)) return false;
    let res = await ActionCarGoToSomePlace({ Param: data });
    if (res.code == 200 && res.data == "OK") {
      message("发送成功！", { type: "success" });
    }
    dialogVisible.value = false;
    sendData.value.targetNodeId = "";
    // } else {
    //   // console.log("error submit!");
    // }
  });
}

function goStartPoint() {
  // console.log("goStartPoint");
  ElMessageBox.confirm(t("buttons.goStartPoint"))
    .then(async () => {
      let res = await ActionGoToStandby({ carCode: props.data.code.text });
      if (res.code == 200 && res.data == "OK") {
        ElMessage({
          message: `${t("buttons.car")}${props.data.code.text}${t("buttons.goStartPoint")}！`,
          type: "success"
        });
      }
    })
    .catch(() => {
      // catch error
    });
}
function initialization() {
  ElMessageBox.confirm(t("buttons.initialization"))
    .then(async () => {
      let res = await ActionInit({ carCode: props.data.code.text });
      if (res.code == 200 && res.data == "OK") {
        ElMessage({
          message: `${t("buttons.car")}${props.data.code.text}${t("buttons.initialization")}！`,
          type: "success"
        });
      }
    })
    .catch(() => {
      // catch error
    });
}
function Resume() {
  // console.log("暂停/恢复");
  ElMessageBox.confirm(t("buttons.Pause/Resume"))
    .then(async () => {
      let res = await ActionStopOrStart({ Param: props.data.code.text });
      if (res.code == 200 && res.data == "OK") {
        ElMessage({
          message: `${t("buttons.car")}${props.data.code.text}${t("buttons.Pause/Resume")}！`,
          type: "success"
        });
      }
    })
    .catch(() => {
      // catch error
    });
}
function endTask() {
  ElMessageBox.confirm(t("buttons.cancelTask"))
    .then(async () => {
      let res = await ActionForceStop({ carCode: props.data.code.text });
      if (res.code == 200 && res.data == "OK") {
        ElMessage({
          message: `${t("buttons.car")}${props.data.code.text}${t("buttons.cancelTask")}！`,
          type: "success"
        });
      }
    })
    .catch(() => {
      // catch error
    });
}
function completelyOffline() {
  ElMessageBox.confirm(t("buttons.completelyOffline"))
    .then(async () => {
      let res = await ActionBlown({ carCode: props.data.code.text });
      if (res.code == 200 && res.data == "OK") {
        ElMessage({
          message: `${t("buttons.car")}${props.data.code.text}${t("buttons.completelyOffline")}！`,
          type: "success"
        });
      }
    })
    .catch(() => {
      // catch error
    });
}
function stopTask() {
  ElMessageBox.confirm(t("buttons.stopTask"))
    .then(async () => {
      let res = await ActionStopReceiveTask({ carCode: props.data.code.text });
      if (res.code == 200 && res.data == "OK") {
        ElMessage({
          message: `${t("buttons.car")}${props.data.code.text}${t("buttons.stopTask")}！`,
          type: "success"
        });
      }
    })
    .catch(() => {
      // catch error
    });
}
function Restart() {
  console.log("Restart");
  ElMessageBox.confirm(t("buttons.restart"))
    .then(async () => {
      let res = await ActionReStart({ carCode: props.data.code.text });
      if (res.code == 200 && res.data == "OK") {
        ElMessage({
          message: `${t("buttons.car")}${props.data.code.text}${t("buttons.restart")}！`,
          type: "success"
        });
      }
    })
    .catch(() => {
      // catch error
    });
}

const nodeListData = ref();
async function getNodeListFn() {
  let pageParam = handlePageParam();
  let { data } = await getNodeList(pageParam);
  nodeListData.value = data.rows;
}
const chargeNodeListData = ref();
async function getChargeListFn() {
  let pageParam = handlePageParam();
  let { data } = await getChargeList(pageParam);
  chargeNodeListData.value = data.rows;
}
// 站点-快捷操作
function handleClickSite(event) {
  let target = event.target;
  while (
    target &&
    target !== event.currentTarget &&
    !target.getAttribute("data-action")
  ) {
    target = target.parentNode;
  }
  if (target && target.getAttribute("data-action")) {
    const action = target.getAttribute("data-action");
    switch (action) {
      case "isEnable":
        isEnableNode(0);
        break;
      case "isDisable":
        isEnableNode(1);
        break;
      default:
        console.log("Unknown action:", action);
    }
  }
}
async function isEnableNode(e) {
  if (!e) {
    ElMessageBox.confirm(t("buttons.siteIsDisable"))
      .then(async () => {
        let res = await DisableNode({ nodeCode: props.data.code.text });
        if (res.code == 200 && res.data == "OK") {
          getNodeStateFn();
          store.SET_ISENSBLE(true);
          // console.log("store.SET_ISENSBLE", store.GET_ISENSBLE);
          ElMessage({
            message: `${t("table.site")}${props.data.code.text}${t("buttons.disable")}！`,
            type: "success"
          });
        }
      })
      .catch(() => {
        // catch error
      });
  } else {
    ElMessageBox.confirm(t("buttons.siteIsEnabled"))
      .then(async () => {
        let res = await EnableNode({ nodeCode: props.data.code.text });
        if (res.code == 200 && res.data == "OK") {
          getNodeStateFn();
          store.SET_ISENSBLE(false);
          ElMessage({
            message: `${t("table.site")}${props.data.code.text}${t("buttons.enable")}！`,
            type: "success"
          });
        }
      })
      .catch(() => {
        // catch error
      });
  }
}
let text = "点击按钮获取";
async function getCarInfoList() {
  if (props.msg === "车辆") {
    const res = await GetCarInfo(props.data.code.text);
    carInfo.value = res.data;
    // console.log("props.data", props.data);
    carInfo.value.x = props.data.base.point.x;
    carInfo.value.y = props.data.base.point.y;
    carPcInfo.value = {
      Cpu: text,
      Gpu: text,
      Tmp: text,
      Memory: text,
      NetW: text
    };
  }
}

let btnText = "获取信息";
async function getCarPcInfo() {
  loading.value = true;
  btnText = "正在加载中...";
  const res = await GetPcHardware({ carCode: props.data.code.text });
  carPcInfo.value = res.data;
  btnText = "获取信息";
  loading.value = false;
}

onUnmounted(() => {
  store.SET_ISENSBLE(null);
});
</script>

<template>
  <div ref="popup" class="popup">
    <div class="content container">
      <div>
        <el-form :model="props.data" label-width="auto" label-position="left">
          <!-- <el-form-item>
            <span>{{ msg }} {{ data.code.text }}</span>
          </el-form-item> -->
          <!-- <el-divider>{{ msg }} {{ data.code.text }}</el-divider> -->
          <div v-if="msg == '站点'">
            <span v-if="props.data.type === 'NodeEllipse'" class="title">{{
              $t("table.quickOperation")
            }}</span>
            <div
              v-if="props.data.type === 'NodeEllipse'"
              class="operationBtn"
              @click="handleClickSite"
            >
              <el-button
                data-action="isEnable"
                color="#5A3092"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.disable") }}</el-button
              >
              <!-- <el-button
                data-action="暂无对应函数"
                color="#5A3092"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.clearMarker") }}</el-button
              > -->
              <el-button
                data-action="isDisable"
                color="#5A3092"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.enable") }}</el-button
              >
              <!-- <el-button
                data-action="暂无对应函数"
                color="#3EA5D0"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.showConflictPoints") }}</el-button
              >
              <el-button
                data-action="暂无对应函数"
                color="#3EA5D0"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.isLock") }}</el-button
              >
              <el-button
                data-action="暂无对应函数"
                color="#3EA5D0"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.isUnlock") }}</el-button
              > -->
            </div>
            <span class="title">{{ $t("table.siteInfo") }}</span>
            <el-form-item :label="$t('table.sitType')">
              <span>{{ data.type }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.siteLocation')">
              <!-- <span>
                x:{{ Math.round(data.data.nodePosition?.x) }} y:{{
                  Math.round(data.data.nodePosition?.y)
                }}</span
              > -->
              <span>
                x:{{ Number(data.data.nodePosition?.x).toFixed(3) }} y:{{
                  Number(data.data.nodePosition?.y).toFixed(3)
                }}
              </span>
            </el-form-item>
            <!-- <el-form-item :label="$t('table.siteRotation')">
              <span>{{}}</span>
            </el-form-item> -->
            <el-form-item :label="$t('table.siteStatus')">
              <span>{{ NodeState }}</span>
            </el-form-item>
            <!-- <el-form-item :label="$t('table.pathPlanningRules')">
              <span>{{}}</span>
            </el-form-item> -->
          </div>
          <div v-if="msg == '路线'">
            <span class="title">{{ $t("table.routeInfo") }}</span>
            <el-form-item label="id">
              <span>{{ data.id }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.startingPoint')">
              <span>{{ data.code.text.split("-")[0] }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.endPoint')">
              <span>{{ data.code.text.split("-")[1] }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.direction')">
              <span>{{
                !data.base.isOneway
                  ? $t("table.bidirectional2")
                  : $t("table.unidirectional")
              }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.pathSpeed')">
              <span>{{ data.data.maxSpeed }}</span>
            </el-form-item>
            <!-- <el-form-item :label="$t('table.pathAngle')">
              <span>{{}}</span>
            </el-form-item> -->
            <!-- <el-form-item :label="$t('table.forwardObstacleAvoidanceInterval')">
              <span>{{}}</span>
            </el-form-item>
            <el-form-item
              :label="$t('table.backwardObstacleAvoidanceInterval')"
            >
              <span>{{}}</span>
            </el-form-item> -->
          </div>
          <div v-if="msg == '车辆'">
            <span
              v-if="msg == '车辆' && carInfo.type == 'Fairyland.Pc'"
              class="title"
              >{{ $t("table.quickOperation") }}</span
            >
            <div
              v-if="msg == '车辆' && carInfo.type == 'Fairyland.Pc'"
              class="operationBtn"
              @click="handleClickCar"
            >
              <el-button
                data-action="goWhere"
                color="#5A3092"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.goWhere") }}</el-button
              >
              <el-button
                data-action="goCharging"
                color="#5A3092"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.goCharging") }}</el-button
              >
              <el-button
                data-action="goStartPoint"
                color="#5A3092"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.goStartPoint") }}</el-button
              >
              <br />
              <el-button
                data-action="initialization"
                color="#3EA5D0"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.initialization") }}</el-button
              >
              <el-button
                data-action="Pause/Resume"
                color="#3EA5D0"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.Pause/Resume") }}</el-button
              >
              <el-button
                data-action="endTask"
                color="#3EA5D0"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.endTask") }}</el-button
              ><br />
              <el-button
                data-action="completelyOffline"
                color="#FF8200"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.completelyOffline") }}</el-button
              >
              <el-button
                data-action="stopTask"
                color="#FF8200"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.stopTask") }}</el-button
              >
              <el-button
                data-action="Restart"
                color="#FF8200"
                style="width: 80px; height: 40px"
                >{{ $t("buttons.restart") }}</el-button
              >
            </div>
            <span class="title">{{ $t("table.carInfo") }}</span>
            <el-form-item :label="$t('table.name')">
              <span>{{ carInfo.name }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.coordinates')">
              <span
                >X:{{ carInfo.x.toFixed(3) }},Y:{{ carInfo.y.toFixed(3) }}</span
              >
            </el-form-item>
            <el-form-item :label="$t('table.type')">
              <span>{{ carInfo.type }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.alarmStatus')">
              <span>{{ !carInfo.isAlarm ? "正常" : "报警" }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.electricity')">
              <span>{{ carInfo.battery }}</span>
            </el-form-item>

            <el-form-item :label="$t('table.IPAddress')">
              <span>{{ carInfo.ipAddress }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.trafficState')">
              <span>{{ carInfo.trafficMessage }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.currentLockPoint')">
              <span>{{ carInfo.currNodeCode }}</span>
            </el-form-item>
            <el-form-item :label="$t('table.preLockPoint')">
              <span class="hide">{{ carInfo.nextNodeCode }}</span>
              <!-- <span class="hide"
                >测试多个点，123，123，123，123，12315631651561123，123，123，123，123，123，123，123，123，123，123，123，123</span
              > -->
            </el-form-item>
            <el-form v-if="carInfo.type == 'Fairyland.Pc'" style="width: 100%">
              <el-form-item label="">
                <el-button
                  text
                  bg
                  :loading="loading"
                  size="small"
                  :icon="useRenderIcon(Pointer)"
                  @click="getCarPcInfo()"
                  >{{ btnText }}</el-button
                >
              </el-form-item>
              <el-form-item label="CPU">
                <span>{{ carPcInfo.Cpu ? carPcInfo.Cpu : "" }}</span>
              </el-form-item>
              <el-form-item label="GPU">
                <span>{{ carPcInfo.Gpu ? carPcInfo.Gpu : "" }}</span>
              </el-form-item>
              <el-form-item :label="$t('table.memory')">
                <span>{{ carPcInfo.Memory ? carPcInfo.Memory : "" }}</span>
              </el-form-item>
              <el-form-item :label="$t('table.ethernet')">
                <span>{{ carPcInfo.NetW ? carPcInfo.NetW : "" }}</span>
              </el-form-item>
            </el-form>
          </div>
        </el-form>
        <!-- <el-divider>{{ msg }}{{ $t("table.property") }}</el-divider>
        <div>
          <span>{{ $t("table.attributeOne") }}</span>
        </div>
        <el-form>
          <el-form-item :label="$t('table.propertyName')">{{}}</el-form-item>
          <el-form-item :label="$t('table.attributeDescription')">
            {{}}</el-form-item
          >
          <el-form-item :label="$t('table.attributeParameters')">
            {{}}</el-form-item
          >
        </el-form> -->
      </div>
    </div>
  </div>
  <el-dialog
    v-model="dialogVisible"
    :title="title"
    width="250"
    style="margin-top: 50px"
    :destroy-on-close="true"
    :modal="false"
    :close-on-click-modal="false"
    draggable
    class="dialog-draggable"
  >
    <el-form
      v-if="title == '去某地' || title == 'goWhere'"
      ref="formRef"
      :rules="formRules"
      :model="sendData"
      label-width="auto"
    >
      <el-form-item :label="$t('table.site')" prop="targetNodeId">
        <span>{{
          Number(props.data.id) ? props.data.id : "请从地图选择站点"
        }}</span>

        <!-- <el-select
          v-model="sendData.targetNodeId"
          filterable
          class="!w-[100%]"
          :placeholder="$t('table.pleaseSelectASiteToGo')"
        >
          <el-option
            v-for="item in nodeListData"
            :key="item.id"
            :label="`${item.name}(${item.code})`"
            :value="item.code"
          />
        </el-select> -->
      </el-form-item>
    </el-form>
    <el-form
      v-if="title == '去充电' || title == 'goCharging'"
      ref="formRef"
      :rules="formRules"
      :model="sendData"
      label-width="auto"
    >
      <el-form-item :label="$t('table.site')" prop="targetNodeId">
        <el-select
          v-model="sendData.targetNodeId"
          filterable
          class="!w-[100%]"
          :placeholder="$t('table.pleaseSelectAChargingStation')"
        >
          <el-option
            v-for="item in chargeNodeListData"
            :key="item.id"
            :label="`${item.name}(${item.code})`"
            :value="item.code"
          />
        </el-select>
      </el-form-item>
    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button
          @click="
            dialogVisible = false;
            sendData.targetNodeId = '';
          "
          >{{ $t("buttons.cancel") }}</el-button
        >
        <el-button type="primary" @click="sendGoWhere">
          {{ $t("buttons.ok") }}
        </el-button>
        <!-- <el-button
          v-if="title == '去充电' || title == 'goCharging'"
          type="primary"
          @click="sendGoWhere"
        >
          {{ $t("buttons.ok") }}
        </el-button> -->
      </div>
    </template>
  </el-dialog>
</template>

<style lang="scss" scoped>
.popup {
  width: 350px;
  min-width: 300px;
  padding: 20px;
  background-color: white;
  border: 1px solid rgba(0, 0, 0, 0.1);
  border-top: none;
  // box-shadow: 20px 2px 10px 20px rgba(0, 0, 0, 0.5);
  transform: translateX(0%);

  .content {
    max-height: 700px;
    overflow: auto;
  }
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
  // cursor: pointer;
}
.operationBtn {
  display: flex;
  // margin-top: 10px;
  // height: 150px;
  flex-wrap: wrap;
  flex-direction: row;
  button {
    border-radius: 4px;
    margin-top: 10px;
    margin-right: 20px;
    margin-left: 0px;
    color: aliceblue;
  }
}
.quickOperationBtn {
  display: flex;
  overflow: hidden;
}
:deep(.el-form-item__content) {
  display: flex;
  justify-content: flex-end;
}
.el-form-item {
  margin: 0;
}
.hide {
  width: 100%;
  height: auto;
  text-align: right;
  word-break: break-word; // 解决英文不会自动换行问题啊阿啊阿啊阿啊阿啊阿
}
.title {
  font-size: 14px;
  color: #1a1a1a;
  font-family: Microsoft YaHei;
  font-weight: bold;
  font-size: 14px;
  // margin-top: 10px;
  // margin-bottom: 10px;
  display: inline-block;
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
:deep(.el-dialog) {
  // 向上移动
  --el-dialog-margin-top: 6vh;
  transform: translate(0px, -25px);
}
</style>
