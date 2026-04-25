<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { usePublicHooks } from "../hooks";
import { IconSelect } from "@/components/ReIcon";
import { useMyI18n } from "@/plugins/i18n";
import { GetDictItemInLocal } from "@/utils/auth";
const { locale } = useMyI18n();
const { switchStyle } = usePublicHooks();

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    parentId: "",
    parents: [],
    type: "",
    code: "",
    name: "",
    icon: "",
    method: "",
    target: "",
    address: "",
    sortNumber: 0,
    isEnable: true,
    remark: ""
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}
const typeList = ref([]);
const methodsList = ref([]);
onMounted(async () => {
  typeList.value = await GetDictItemInLocal("PermissionType");
  methodsList.value = await GetDictItemInLocal("PermissionMethod");
});

defineExpose({ getRef });
</script>

<template>
  <el-form
    ref="ruleFormRef"
    :model="newFormInline"
    :rules="formRules"
    label-width="100px"
  >
    <el-row :gutter="30">
      <re-col>
        <el-form-item :label="$t('table.superior')">
          <el-cascader
            v-model="newFormInline.parentId"
            class="w-full"
            :options="newFormInline.parents"
            :props="{
              value: 'id',
              label: 'name',
              emitPath: false,
              checkStrictly: true
            }"
            clearable
            filterable
            :placeholder="$t('table.pleaseSelect')"
          >
            <template #default="{ node, data }">
              <span>{{ data.name }}</span>
              <span v-if="!node.isLeaf"> ({{ data.children.length }}) </span>
            </template>
          </el-cascader>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.type')" prop="type">
          <el-select
            v-model="newFormInline.type"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in typeList"
              :key="item.id"
              :label="`${locale == 'zh' ? item.name : item.code}`"
              :value="item.code"
            />
          </el-select>
          <!-- <el-input
            v-model="newFormInline.type"
            clearable
            :placeholder="$t('table.type')"
          /> -->
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.code')" prop="code">
          <el-input
            v-model="newFormInline.code"
            clearable
            :placeholder="$t('table.code')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.name')" prop="name">
          <el-input
            v-model="newFormInline.name"
            clearable
            :placeholder="$t('table.name')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.icon')" prop="icon">
          <IconSelect v-model="newFormInline.icon" class="w-full" />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.method')" prop="method">
          <el-select
            v-model="newFormInline.method"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.method')"
          >
            <el-option
              v-for="item in methodsList"
              :key="item.id"
              :label="`${locale == 'zh' ? item.name : item.code}`"
              :value="item.code"
            />
          </el-select>
          <!-- <el-input
            v-model="newFormInline.method"
            clearable
            :placeholder="$t('table.method')"
          /> -->
          <!-- methodsList -->
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.target')" prop="target">
          <el-input
            v-model="newFormInline.target"
            clearable
            :placeholder="$t('table.target')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.address')" prop="address">
          <el-input
            v-model="newFormInline.address"
            clearable
            :placeholder="$t('table.address')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.sort')">
          <el-input-number
            v-model="newFormInline.sortNumber"
            :min="-99999"
            :max="99999"
            controls-position="right"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.isEnable')">
          <el-switch
            v-model="newFormInline.isEnable"
            inline-prompt
            :active-value="true"
            :inactive-value="false"
            :active-text="$t('table.yes')"
            :inactive-text="$t('table.no')"
            :style="switchStyle"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.remark')">
          <el-input
            v-model="newFormInline.remark"
            :placeholder="$t('table.remark')"
            type="textarea"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
