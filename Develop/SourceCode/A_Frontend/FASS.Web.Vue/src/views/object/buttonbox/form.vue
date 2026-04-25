<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
// import { GetListToSelect as gatChargeNodePage } from "@/api/data/charge";
import { GetListToSelect } from "@/api/data/chargingstation";
import { GetDictItemInLocal } from "@/utils/auth";

// const nodeList = ref([]);
const ChargingMode = ref([]);
const ChargingProtocol = ref([]);

// function handleChargeIdChange(value: string) {
//   const selectedItem = nodeList.value.find(item => item.id === value);
//   if (selectedItem) {
//     newFormInline.value.chargeCode = selectedItem.code;
//   } else {
//     newFormInline.value.chargeCode = "";
//   }
// }

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    code: "",
    id: "",
    name: "",
    ip: "",
    port: "",
    protocolType: "",
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
  // const { data } = await gatChargeNodePage();
  // nodeList.value = data;
  // const res = await GetListToSelect("ChargingMode");
  // ChargingMode.value = [...res.data];
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
        <el-form-item :label="$t('table.code')" prop="code">
          <el-input
            v-model="newFormInline.code"
            clearable
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.name')" prop="name">
          <el-input
            v-model="newFormInline.name"
            clearable
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item label="IP" prop="ip">
          <el-input
            v-model="newFormInline.ip"
            clearable
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>

      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.communicationMode')"
          prop="protocolType"
        >
          <el-select
            v-model="newFormInline.protocolType"
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
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.port')" prop="port" :required="true">
          <el-input
            v-model="newFormInline.port"
            clearable
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="8">
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
