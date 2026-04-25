<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetListToSelect as gatChargeNodePage } from "@/api/data/charge";
import { GetListToSelect } from "@/api/data/chargingstation";
import { GetDictItemInLocal } from "@/utils/auth";

const nodeList = ref([]);
const ChargingMode = ref([]);
const ChargingProtocol = ref([]);

function handleChargeIdChange(value: string) {
  const selectedItem = nodeList.value.find(item => item.id === value);
  if (selectedItem) {
    newFormInline.value.chargeCode = selectedItem.code;
  } else {
    newFormInline.value.chargeCode = "";
  }
  console.log("newFormInline is", newFormInline.value);
}

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    nodeId: "",
    code: "",
    name: "",
    state: "",
    occupiedCarId: "",
    voltage: "",
    mode: "",
    ip: "",
    port: "",
    chargeCode: "",
    chargeId: "",
    protocol: "",
    current: "",
    isOccupied: false,
    isEnable: true
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  const { data } = await gatChargeNodePage();
  nodeList.value = data;
  const res = await GetListToSelect("ChargingMode");
  ChargingMode.value = [...res.data];
  ChargingProtocol.value = await GetDictItemInLocal("ProtocolType");
});
</script>

<template>
  <el-form
    ref="ruleFormRef"
    :model="newFormInline"
    :rules="formRules"
    label-width="100px"
  >
    <el-row :gutter="30">
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.chargingPileName')" prop="name">
          <el-input
            v-model="newFormInline.name"
            clearable
            :placeholder="$t('table.chargingPileName')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.chargingPileIP')" prop="ip">
          <el-input
            v-model="newFormInline.ip"
            clearable
            :placeholder="$t('table.chargingPileIP')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.port')" prop="port" :required="true">
          <el-input
            v-model="newFormInline.port"
            clearable
            :placeholder="$t('table.port')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.chargingMode')" prop="chargingMode">
          <el-select
            v-model="newFormInline.mode"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option label="--请选择--" value="" />
            <el-option
              v-for="item in ChargingMode"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.protocol')" prop="protocol">
          <el-select
            v-model="newFormInline.protocol"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in ChargingProtocol"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <!-- <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.carType')">
          <el-select
            v-model="newFormInline.chargeCode"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
            @change="changeCarType"
          >
            <el-option label="--请选择--" value="" />
            <el-option
              v-for="item in carTypeList"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col> -->
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.site')" prop="chargeId">
          <el-select
            v-model="newFormInline.chargeId"
            filterable
            class="!w-[100%]"
            placeholder="请选择"
            @change="handleChargeIdChange"
          >
            <el-option label="--请选择--" value="" />
            <el-option
              v-for="item in nodeList"
              :key="item.id"
              :label="item.code"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.current')" prop="current">
          <!-- <el-select
            v-model="newFormInline.current"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.current')"
          >
            <el-option
              v-for="item in current"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            />
          </el-select> -->
          <el-input
            v-model="newFormInline.current"
            clearable
            :placeholder="$t('table.current')"
          />
        </el-form-item>
      </re-col>

      <!-- <re-col :value="8" :xs="24" :sm="24">
        <el-form-item label="备注">
          <el-input v-model="newFormInline.remark" clearable placeholder="备注" />
        </el-form-item>
      </re-col> -->
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.voltage')" prop="voltage">
          <el-input
            v-model="newFormInline.voltage"
            clearable
            :placeholder="$t('table.voltage')"
          />
          <!-- <el-select
            v-model="newFormInline.voltage"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.voltage')"
          >
            <el-option
              v-for="item in voltage"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            />
          </el-select> -->
        </el-form-item>
      </re-col>
      <!-- <re-col :value="8" :xs="24" :sm="24">
        <el-form-item :label="$t('table.carCollection')" >
          <el-select
            v-model="newFormInline.occupiedCarId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.carCollection')"
          >
            <el-option
              v-for="item in carList"
              :label="item.name"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col> -->
      <re-col :value="8">
        <!-- <el-form-item :label="$t('table.isOccupied')" prop="isOccupied">
          <el-switch
            v-model="newFormInline.isOccupied"
            class="ml-2"
            style="
              --el-switch-on-color: #13ce66;
              --el-switch-off-color: #ff4949;
            "
          />
        </el-form-item> -->
        <el-form-item :label="$t('table.isEnable')" prop="isEnable">
          <el-switch
            v-model="newFormInline.isEnable"
            class="ml-2"
            style="
              --el-switch-on-color: #13ce66;
              --el-switch-off-color: #ff4949;
            "
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
