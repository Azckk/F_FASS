<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetDictItemInLocal } from "@/utils/auth";
import { getWarehouseAreaList } from "@/api/warehouse/container";
const ContainerTypeList = ref([]);
const ContainerStateList = ref([]);
const WarehouseAreaList = ref([]);
const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    areaId: "",
    barcode: "",
    isLock: false,
    code: "",
    name: "",
    length: 0,
    width: 0,
    height: 0,
    type: "Default",
    state: "Default"
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  ContainerTypeList.value = await GetDictItemInLocal("ContainerType");
  ContainerStateList.value = await GetDictItemInLocal("ContainerState");
  const { data } = await getWarehouseAreaList();
  WarehouseAreaList.value = [...data];
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
        <el-form-item :label="$t('table.region')" prop="areaId">
          <el-select
            v-model="newFormInline.areaId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in WarehouseAreaList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
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
        <el-form-item :label="$t('table.type')" prop="type">
          <el-select
            v-model="newFormInline.type"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in ContainerTypeList"
              :key="item.code"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.status')" prop="state">
          <el-select
            v-model="newFormInline.state"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in ContainerStateList"
              :key="item.code"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
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
        <el-form-item :label="$t('table.barcode')" prop="barcode">
          <el-input
            v-model="newFormInline.barcode"
            :placeholder="$t('table.documentContent')"
            :autosize="{ minRows: 12, maxRows: 18 }"
            type="textarea"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.length')" prop="length">
          <el-input-number
            v-model="newFormInline.length"
            controls-position="right"
            clearable
            :placeholder="$t('table.length')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.width')" prop="width">
          <el-input-number
            v-model="newFormInline.width"
            controls-position="right"
            clearable
            :placeholder="$t('table.width')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.height')" prop="height">
          <el-input-number
            v-model="newFormInline.height"
            controls-position="right"
            clearable
            :placeholder="$t('table.height')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
