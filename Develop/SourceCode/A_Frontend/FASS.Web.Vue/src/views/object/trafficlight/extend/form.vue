<script setup lang="ts">
import { ref, onMounted, handleError } from "vue";
import ReCol from "@/components/ReCol";
import Fold from "@iconify-icons/ep/fold";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { getNodeList as gatChargeNodePage } from "@/api/data/charge";

// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { operationProps } from "./utils/types";
const props = withDefaults(defineProps<operationProps>(), {
  formInline: () => ({
    openCloseSignal: "",
    station: "",
    trafficLightId: "",
    remark: ""
  })
});
const newFormInline = ref(props.formInline);
const nodeList = ref([]);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  console.log("props传值", props.formInline);
  const { data } = await gatChargeNodePage();
  nodeList.value = data;
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
        <el-form-item :label="$t('通讯位地址')" prop="openCloseSignal">
          <el-input
            v-model="newFormInline.openCloseSignal"
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.siteId')" prop="station">
          <el-select
            v-model="newFormInline.station"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in nodeList"
              :key="item.id"
              :label="item.code"
              :value="item.code"
            />
          </el-select>
          <!-- <el-input v-model="newFormInline.prevNodeId" clearable placeholder="请选择" /> -->
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('描述')" prop="remark">
          <el-input
            v-model="newFormInline.remark"
            clearable
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
