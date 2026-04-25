<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { nameRules } from "./utils/rule";
import { FormNameItemProps } from "./utils/types";
import { GetDictItemInLocal } from "@/utils/auth";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

// const { AreaTypeList, AreaStateList } = useHook();

const props = withDefaults(defineProps<FormNameItemProps>(), {
  formNameInline: () => ({
    fileName: ""
  })
});
const newFormInline = ref(props.formNameInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });
</script>

<template>
  <el-form
    ref="ruleFormRef"
    :model="newFormInline"
    :rules="nameRules"
    label-width="100px"
  >
    <el-row :gutter="30">
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.fileName')" prop="fileName">
          <el-input
            v-model="newFormInline.fileName"
            clearable
            :placeholder="$t('table.fileName')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>

  <div class="margin-left: 40px; color: red; font-size: 12px;">
    示例:XXXX.json
  </div>
</template>
