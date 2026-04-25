<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";

const selectGenderList = ref([]);

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    systemName: "",
    name: "",
    sortNumber: 0,
    isEnable: true,
    isDelete: false,
    remark: "",
    extend: "",
    communicatioMode: "",
    url: "",
    method: "",
    headers: "",
    queryParams: "",
    responseParams: "",
    state: "",
    isRetransmit: true
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
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.systemName')" prop="systemName">
          <el-input
            v-model="newFormInline.systemName"
            clearable
            :placeholder="$t('table.systemName')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.interfaceName')" prop="name">
          <el-input v-model="newFormInline.name" clearable placeholder="名称" />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.communicationMode')"
          prop="communicatioMode"
        >
          <el-input
            v-model="newFormInline.communicatioMode"
            clearable
            :placeholder="$t('table.communicationMode')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.communicationAddress')" prop="url">
          <el-input
            v-model="newFormInline.url"
            clearable
            :placeholder="$t('table.communicationAddress')"
          />
        </el-form-item>
      </re-col>

      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.requestMethod')" prop="method">
          <el-input
            v-model="newFormInline.method"
            clearable
            :placeholder="$t('table.requestMethod')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.headers')">
          <el-input
            v-model="newFormInline.headers"
            clearable
            :placeholder="$t('table.headers')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.queryParams')">
          <el-input
            v-model="newFormInline.queryParams"
            clearable
            :placeholder="$t('table.queryParams')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.responseParams')" prop="responseParams">
          <el-input
            v-model="newFormInline.responseParams"
            clearable
            :placeholder="$t('table.responseParams')"
          />
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
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.sortNumber')" prop="sortNumber">
          <el-input
            v-model="newFormInline.sortNumber"
            clearable
            :placeholder="$t('table.sortNumber')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
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
      <!-- <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.availableTags')" prop="isEnable">
          <el-checkbox
            v-model="newFormInline.isEnable"
            :label="$t('table.availableTags')"
            size="large"
          />
        </el-form-item>
      </re-col> -->
    </el-row>
  </el-form>
</template>
