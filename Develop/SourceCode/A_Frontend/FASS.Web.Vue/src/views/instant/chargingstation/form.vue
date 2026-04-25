<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

const selectGenderList = ref([]);
const selectAvatarList = ref([]);

const {
  nodeList,
  current,
  voltage,
  ChargingMode,
  carTypeList,
  carList,
  changeCarType
} = useHook();

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    nodeId: "",
    code: "",
    name: "",
    state: "",
    occupiedCarId:""
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {});
</script>

<template>
  <el-form
    ref="ruleFormRef"
    :model="newFormInline"
    :rules="formRules"
    label-width="100px"
  >
     <el-row :gutter="30">
      <re-col :value="8" :xs="24" :sm="24">
        <el-form-item :label="$t('table.chargingPileName')" prop="name">
          <el-input
            v-model="newFormInline.name"
            clearable
            :placeholder="$t('table.chargingPileName')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="8" :xs="24" :sm="24">
        <el-form-item :label="$t('table.chargingPileIP')" prop="ip">
          <el-input
            v-model="newFormInline.ip"
            clearable
            :placeholder="$t('table.chargingPileIP')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="8" :xs="24" :sm="24">
        <el-form-item :label="$t('table.port')" prop="port" :required="true">
          <el-input
            v-model="newFormInline.port"
            clearable
            :placeholder="$t('table.port')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="8" :xs="24" :sm="24">
        <el-form-item :label="$t('table.chargingMode')" :required="true">
          <el-select
            v-model="newFormInline.mode"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in ChargingMode"
              :label="item.name"
              :value="item.code"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="8" :xs="24" :sm="24">
        <el-form-item :label="$t('table.carType')">
          <el-select
            v-model="newFormInline.chargeCode"
            filterable
            class="!w-[100%]"
          :placeholder="$t('table.pleaseSelect')"
            @change="changeCarType"
          >
            <el-option
              v-for="item in carTypeList"
              :label="item.name"
              :value="item.code"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="8" :xs="24" :sm="24">
        <el-form-item :label="$t('table.siteId')" :required="true">
          <el-select  v-model="newFormInline.chargeId" filterable class="!w-[100%]" placeholder="请选择">
            <el-option v-for="item in nodeList" :label="item.code" :value="item.id" :key="item.id"/> 
          </el-select>
        </el-form-item :label="$t('table.siteId')" :required="true">
        
      </re-col>
      <re-col :value="8" :xs="24" :sm="24">
        <el-form-item :label="$t('table.current')" :required="true">
          <el-select
            v-model="newFormInline.current"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.current')"
          >
            <el-option
              v-for="item in current"
              :label="item.label"
              :value="item.value"
              :key="item.value"
            />
          </el-select>
        </el-form-item>
      </re-col>
      
      
      <!-- <re-col :value="8" :xs="24" :sm="24">
        <el-form-item label="备注">
          <el-input v-model="newFormInline.remark" clearable placeholder="备注" />
        </el-form-item>
      </re-col> -->
      <re-col :value="8" :xs="24" :sm="24">
        <el-form-item :label="$t('table.voltage')" prop="voltage" :required="true">
          <el-select
            v-model="newFormInline.voltage"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.voltage')"
          >
           <el-option
              v-for="item in voltage"
              :label="item.label"
              :value="item.value"
              :key="item.value"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="8" :xs="24" :sm="24">
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
      </re-col>
      <re-col :value="8" >
        <el-form-item :label="$t('table.isEnable')" prop="isEnable">
          <el-switch
            v-model="newFormInline.isOccupied"
            class="ml-2"
            style="
              --el-switch-on-color: #13ce66;
              --el-switch-off-color: #ff4949;
            "
          />
        </el-form-item>
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
