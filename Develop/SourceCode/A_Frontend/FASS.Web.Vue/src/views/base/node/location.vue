<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { GetExtends } from "@/api/base/node/index";
import { formRules } from "./utils/rule";
import { locationFormProps } from "./utils/types";

const props = withDefaults(defineProps<locationFormProps>(), {
  formInline: () => ({
    x: "",
    y: "",
    theta: "",
    allowedDeviationXY: "",
    allowedDeviationTheta: "",
    mapId: "",
    sequenceId: ""
  })
});
const newFormInline = ref(props.formInline);
const ruleFormRef = ref();
// let selectGenderList = ref();

function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  // selectGenderList.value = (await GetExtends()).data;
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
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.coordinatesX')" prop="x">
          <el-input
            v-model="newFormInline.x"
            clearable
            :placeholder="$t('table.coordinatesX')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.coordinatesY')" prop="y">
          <el-input
            v-model="newFormInline.y"
            clearable
            :placeholder="$t('table.coordinatesY')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.angle')" prop="theta">
          <el-input
            v-model="newFormInline.theta"
            clearable
            :placeholder="$t('table.angle')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.accuracyCoordinates')"
          prop="allowedDeviationXY"
        >
          <el-input
            v-model="newFormInline.allowedDeviationXY"
            clearable
            :placeholder="$t('table.accuracyCoordinates')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.accuracyAngle')"
          prop="allowedDeviationTheta"
        >
          <el-input
            v-model="newFormInline.allowedDeviationTheta"
            clearable
            :placeholder="$t('table.accuracyAngle')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.mapNo')" prop="mapId">
          <el-input
            v-model="newFormInline.mapId"
            clearable
            :placeholder="$t('table.mapNo')"
          />
        </el-form-item>
      </re-col>

      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.mapdescription')" prop="">
          <el-input
            type="textarea"
            v-model="newFormInline.sequenceId"
            clearable
            :placeholder="$t('table.mapdescription')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
