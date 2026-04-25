<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

const selectGenderList = ref([]);
const selectAvatarList = ref([]);

const { carList } = useHook();

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    fromCarId: "",
    toCarId: "",
    count: 0,
    isMutual: true,
    IsFinish: true
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
      <re-col :value="24" :xs="24" :sm="24" id="form-list-lable">
        <el-form-item :label="$t('table.controlOfVehicles')" prop="fromCarId">
          <el-select
            :required="true"
            v-model="newFormInline.fromCarId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in carList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24" id="form-list-lable">
        <el-form-item :label="$t('table.regulatedvehicle')" prop="toCarId">
          <el-select
            v-model="newFormInline.toCarId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in carList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24" id="form-list-lable">
        <el-form-item :label="$t('table.regulatoryStatistics')" prop="count">
          <el-input-number
            class="w-[100%]"
            :min="-99999"
            :max="99999"
            controls-position="right"
            v-model="newFormInline.count"
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.bidirectional')" prop="sortNumber">
          <el-switch
            v-model="newFormInline.isMutual"
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
        <el-form-item :label="$t('table.effectiveness')" prop="sortNumber">
          <el-switch
            v-model="newFormInline.IsFinish"
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

<style scoped>
/* 确保表单项标签具有相对定位 */
#form-list-lable {
  position: relative;
  padding-right: 20px; /* 预留空间给右侧的星标 */
}

/* 将星标移动到右侧 */
#form-item-lable::after {
  content: "*";
  color: red;
  margin-left: 8px; /* 调整星标和文本的间距 */
  position: absolute; /* 使星标绝对定位 */
  left: 100px; /* 调整这个值以满足你的需求 */
  top: 8px;
}
</style>
