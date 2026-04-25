<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules, strategyRules } from "./utils/rule";
import { StrategyFormProps } from "./utils/types";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

const selectGenderList = ref([]);
const selectAvatarList = ref([]);

const props = withDefaults(defineProps<StrategyFormProps>(), {
  formInline: () => ({
    carMinBattery: "",
    carMaxBattery: "",
    carIdleChargeBattery: "",
    carIdleSecond: "",
    taskAvailableBattery: ""
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
    :rules="strategyRules"
    label-position="top"
    require-asterisk-position="right"
    label-width="120px"
  >
    <el-row :gutter="30">
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.minBattery2')"
          label-width="120px"
          prop="carMinBattery"
        >
          <el-input
            v-model="newFormInline.carMinBattery"
            clearable
            :placeholder="$t('table.minBattery2')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.maxBattery2')" prop="carMaxBattery">
          <el-input
            v-model="newFormInline.carMaxBattery"
            clearable
            :placeholder="$t('table.minBattery2')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.carIdleChargeBattery')"
          prop="carMaxBattery"
        >
          <el-input
            v-model="newFormInline.carIdleChargeBattery"
            clearable
            :placeholder="$t('table.carIdleChargeBattery')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.carIdleSecond2')" prop="carIdleSecond">
          <el-input
            v-model="newFormInline.carIdleSecond"
            clearable
            :placeholder="$t('table.carIdleSecond2')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.taskAvailableBattery')"
          prop="taskAvailableBattery"
        >
          <el-input
            v-model="newFormInline.taskAvailableBattery"
            clearable
            :placeholder="$t('table.taskAvailableBattery')"
          />
        </el-form-item>
      </re-col>

      <!-- <re-col :value="24">
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
      </re-col> -->
    </el-row>
  </el-form>
</template>
