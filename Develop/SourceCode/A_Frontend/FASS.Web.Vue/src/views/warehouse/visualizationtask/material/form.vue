<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetDictItemInLocal } from "@/utils/auth";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

const MaterialTypeList = ref([]);
const MaterialStateList = ref([]);
// const { MaterialTypeList, MaterialStateList, nodeList, WarehouseAreaList } =
//   useHook();

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    code: "",
    name: "",
    type: "Default",
    state: "Default",
    isLock: true,
    barcode: "", //条码
    batch: "", //批次
    spec: "", //规格
    unit: "", //单位
    quantity: "1" //数量
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  MaterialTypeList.value = await GetDictItemInLocal("MaterialType");
  MaterialStateList.value = await GetDictItemInLocal("MaterialState");
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
      <!-- <re-col :value="24" :xs="24" :sm="24">
        <el-form-item label="区域" prop="areaId">
          <el-select
            v-model="newFormInline.areaId"
            filterable
            class="!w-[100%]"
            placeholder="请选择"
          >
            <el-option label="--请选择--" value="" />
            <el-option
              v-for="item in WarehouseAreaList"
              :label="item.name"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col> -->
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
            :placeholder="$t('table.type')"
          >
            <el-option
              v-for="item in MaterialTypeList"
              :label="item.name"
              :value="item.code"
              :key="item.code"
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
            :placeholder="$t('table.status')"
          >
            <!-- ContainerStateList -->
            <el-option
              v-for="item in MaterialStateList"
              :label="item.name"
              :value="item.code"
              :key="item.code"
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
        <el-form-item :label="$t('table.batch')" prop="batch">
          <el-input
            v-model="newFormInline.batch"
            clearable
            :placeholder="$t('table.batch')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.spec')" prop="spec">
          <el-input
            v-model="newFormInline.spec"
            clearable
            :placeholder="$t('table.spec')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.unit')" prop="unit">
          <el-input
            v-model="newFormInline.unit"
            clearable
            :placeholder="$t('table.unit')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.quantity')" prop="quantity">
          <el-input
            v-model="newFormInline.quantity"
            clearable
            :placeholder="$t('table.quantity')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
