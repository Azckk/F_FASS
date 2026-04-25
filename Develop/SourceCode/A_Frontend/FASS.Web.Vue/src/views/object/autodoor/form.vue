<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetListToSelect } from "@/api/base/node";

const selectGenderList = ref([]);

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    sortNumber: 0,
    isEnable: true,
    isDelete: true,
    remark: "",
    extend: "",
    type: "",
    code: "",
    name: "",
    prevNodeId: "",
    nextNodeId: "",
    isAdvance: true,
    state: ""
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
  const { data } = await GetListToSelect();
  nodeList.value = [...data];
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
          <el-input v-model="newFormInline.code" clearable placeholder="编码" />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.name')" prop="name">
          <el-input v-model="newFormInline.name" clearable placeholder="名称" />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.prevNodeId')" prop="prevNodeId">
          <el-select
            v-model="newFormInline.prevNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in nodeList"
              :key="item.id"
              :label="item.code"
              :value="item.id"
            />
          </el-select>
          <!-- <el-input
            v-model="newFormInline.prevNodeId"
            clearable
            :placeholder="$t('table.prevNodeId')"
          /> -->
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.nextNodeId')" prop="nextNodeId">
          <el-select
            v-model="newFormInline.nextNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in nodeList"
              :key="item.id"
              :label="item.code"
              :value="item.id"
            />
          </el-select>
          <!-- <el-input
            v-model="newFormInline.nextNodeId"
            clearable
            :placeholder="$t('table.nextNodeId')"
          /> -->
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.state')" prop="state">
          <el-input
            v-model="newFormInline.state"
            clearable
            :placeholder="$t('table.state')"
          />
        </el-form-item>
      </re-col>

      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.isAdvance')" prop="isAdvance">
          <el-switch
            v-model="newFormInline.isAdvance"
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
        <el-form-item :label="$t('table.isEnable')" prop="isEnable">
          <el-switch
            v-model="newFormInline.isEnable"
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
    </el-row>
  </el-form>
</template>
