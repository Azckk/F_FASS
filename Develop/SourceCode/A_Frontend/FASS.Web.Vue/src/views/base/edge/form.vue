<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { getNodeList } from "@/api/data/avoid";
import { GetDictItemInLocal } from "@/utils/auth";
import { FormProps } from "./utils/types";
const nodeList = ref([]);
const EdgeOrientationTypeList = ref([]);
const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    kind: "",
    type: "",
    code: "",
    name: "",
    isLock: false,
    isOneway: false,
    sequenceId: "",
    released: false,
    startNodeCode: "",
    endNodeCode: "",
    maxSpeed: null,
    maxHeight: null,
    minHeight: null,
    orientation: null,
    orientationType: "",
    rotationAllowed: "",
    maxRotationSpeed: null,
    startNodeId: "",
    endNodeId: "",
    length: null,
    direction: "",
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
  const { data } = await getNodeList();
  nodeList.value = [...data];
  EdgeOrientationTypeList.value = await GetDictItemInLocal(
    "EdgeOrientationType"
  );
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
        <el-form-item :abel="$t('table.classification')" prop="kind">
          <el-input
            v-model="newFormInline.kind"
            clearable
            :placeholder="$t('table.classification')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.type')" prop="type">
          <el-input
            v-model="newFormInline.type"
            clearable
            :placeholder="$t('table.type')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.code')" prop="code">
          <el-input
            v-model="newFormInline.code"
            clearable
            :placeholder="$t('table.code')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.name')" prop="name">
          <el-input
            v-model="newFormInline.name"
            clearable
            :placeholder="$t('table.name')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.isLock')" prop="isLock">
          <el-switch
            v-model="newFormInline.isLock"
            class="ml-2"
            active-text="是"
            inactive-text="否"
            inline-prompt
            style="
              --el-switch-on-color: #13ce66;
              --el-switch-off-color: #ff4949;
            "
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.isOneway')" prop="isOneway">
          <el-switch
            v-model="newFormInline.isOneway"
            class="ml-2"
            active-text="是"
            inactive-text="否"
            inline-prompt
            style="
              --el-switch-on-color: #13ce66;
              --el-switch-off-color: #ff4949;
            "
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.sequenceId')" prop="sequenceId">
          <el-input
            v-model="newFormInline.sequenceId"
            clearable
            :placeholder="$t('table.sequenceId')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.release')" prop="released">
          <el-switch
            v-model="newFormInline.released"
            class="ml-2"
            active-text="是"
            inactive-text="否"
            inline-prompt
            style="
              --el-switch-on-color: #13ce66;
              --el-switch-off-color: #ff4949;
            "
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.startingPointLandmark')"
          prop="released"
        >
          <el-select
            v-model="newFormInline.startNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.startingPointLandmark')" value="" />
            <el-option
              v-for="item in nodeList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.endLandmark')" prop="released">
          <el-select
            v-model="newFormInline.endNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in nodeList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.maxSpeed')" prop="maxCar">
          <el-input-number
            v-model="newFormInline.maxHeight"
            :placeholder="$t('table.maxSpeed')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.maxHeight')" prop="maxHeight">
          <el-input-number
            v-model="newFormInline.maxHeight"
            :placeholder="$t('table.maxHeight')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.minHeight')" prop="minHeight">
          <el-input-number
            v-model="newFormInline.minHeight"
            :placeholder="$t('table.minHeight')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.routeDirection')" prop="orientation">
          <el-input-number
            v-model="newFormInline.orientation"
            :placeholder="$t('table.routeDirection')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.directionType')" prop="orientationType">
          <el-select
            v-model="newFormInline.orientationType"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.directionType')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in EdgeOrientationTypeList"
              :label="item.name"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.crossDirection')" prop="direction">
          <el-input
            v-model="newFormInline.direction"
            :placeholder="$t('table.crossDirection')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.allowRotation')" prop="rotationAllowed">
          <el-switch
            v-model="newFormInline.rotationAllowed"
            class="ml-2"
            active-text="是"
            inactive-text="否"
            inline-prompt
            style="
              --el-switch-on-color: #13ce66;
              --el-switch-off-color: #ff4949;
            "
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.maxSteering')" prop="maxRotationSpeed">
          <el-input-number
            v-model="newFormInline.maxRotationSpeed"
            :placeholder="$t('table.maxSteering')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.routeLength')" prop="length">
          <el-input-number
            v-model="newFormInline.length"
            :placeholder="$t('table.routeLength')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
