<script setup lang="ts">
import { reactive, ref, watchEffect, watch } from "vue";
import { useHook } from "./utils/hook";
import { formRules, formRules2 } from "./utils/rule";
const {
  formRef,
  TaskTemplateDataList,
  nodeDataList,
  carDataList,
  LogisticsRouteDataList,
  areaDataList,
  handleSendWorkFn,
  handleCarChange
} = useHook();

const formLabelAlign = reactive({
  taskTemplateId: "",
  carId: "",
  carTypeId: "",
  code: "",
  type: "",
  srcStorageId: "",
  destStorageId: "",
  srcAreaId: "",
  destAreaId: ""
});

const switchBtn = ref(true);
defineOptions({
  name: ""
});
// watchEffect(() => {
//   switchBtn.value;
//   formLabelAlign.taskTemplateId = "";
//   formLabelAlign.carId = "";
//   formLabelAlign.carTypeId = "";
//   formLabelAlign.code = "";
//   formLabelAlign.type = "";
//   formLabelAlign.srcStorageId = "";
//   formLabelAlign.destStorageId = "";
// });
watch(switchBtn, newValue => {
  formLabelAlign.taskTemplateId = "";
  formLabelAlign.carId = "";
  formLabelAlign.carTypeId = "";
  formLabelAlign.code = "";
  formLabelAlign.type = "";
  formLabelAlign.srcStorageId = "";
  formLabelAlign.destStorageId = "";
  formLabelAlign.srcAreaId = "";
  formLabelAlign.destAreaId = "";
});
</script>

<template>
  <div>
    <h5 class="mb-2">{{ $t("table.sendWork") }}</h5>
    <div>
      <el-radio-group
        v-model="switchBtn"
        aria-label="label position"
        size="small"
      >
        <el-radio-button :value="true">{{
          $t("menus.Task-Template")
        }}</el-radio-button>
        <!-- <el-radio-button :value="false">{{
          $t("menus.Task-LogisticsRoute")
        }}</el-radio-button> -->
      </el-radio-group>
    </div>
    <div style="margin: 20px" />
    <div v-if="switchBtn">
      <el-form
        ref="formRef"
        label-width="auto"
        :model="formLabelAlign"
        :rules="formRules"
        style="max-width: 600px"
      >
        <el-form-item :label="$t('table.templateType')" prop="taskTemplateId">
          <el-select
            v-model="formLabelAlign.taskTemplateId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in TaskTemplateDataList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          :label="$t('table.taskStartingPoint')"
          prop="srcStorageId"
        >
          <el-select
            v-model="formLabelAlign.srcStorageId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
            filterable
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in areaDataList"
              :key="item.id"
              :label="`${item.name} (${item.code})`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.taskEndingPoint')" prop="destStorageId">
          <el-select
            v-model="formLabelAlign.destStorageId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
            filterable
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in areaDataList"
              :key="item.id"
              :label="`${item.name} (${item.code})`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.designatedVehicle')" prop="carId">
          <el-select
            v-model="formLabelAlign.carId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
            filterable
            @change="handleCarChange"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in carDataList"
              :key="item.id"
              :label="`${item.code}(${item.name})`"
              :value="item.id"
              :data-carTypeId="item.carTypeId"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          ><div class="sendWorkBtn">
            <el-button
              type="primary"
              @click="handleSendWorkFn(formLabelAlign)"
              >{{ $t("table.sendWork") }}</el-button
            >
          </div>
        </el-form-item>
      </el-form>
    </div>
    <div v-if="!switchBtn">
      <el-form
        ref="formRef"
        label-width="auto"
        :model="formLabelAlign"
        :rules="formRules2"
        style="max-width: 600px"
      >
        <el-form-item
          :label="$t('menus.Flow-LogisticsRoute')"
          prop="taskTemplateId"
        >
          <el-select
            v-model="formLabelAlign.taskTemplateId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in LogisticsRouteDataList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.designatedVehicle')" prop="carId">
          <el-select
            v-model="formLabelAlign.carId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
            @change="handleCarChange"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in carDataList"
              :key="item.id"
              :label="`${item.code}(${item.name})`"
              :value="item.id"
              :data-carTypeId="item.carTypeId"
            />
          </el-select>
        </el-form-item>

        <el-form-item>
          <div class="sendWorkBtn">
            <el-button
              type="primary"
              @click="handleSendWorkFn(formLabelAlign)"
              >{{ $t("table.sendWork") }}</el-button
            >
          </div>
        </el-form-item>
      </el-form>
    </div>
    <!-- <div class="sendWorkBtn">
      <el-button type="primary" @click="handleSendWorkFn(formLabelAlign)">{{
        $t("table.sendWork")
      }}</el-button>
    </div> -->
  </div>
</template>

<style lang="scss" scoped>
.flex-grow {
  flex-grow: 1;
}
</style>
