<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetDictItemInLocal } from "@/utils/auth";
import { getNodeList, getWarehouseAreaList } from "@/api/warehouse/storage";
// const { switchStyle } = usePublicHooks();

const StorageTypeList = ref([]);
const StorageStateList = ref([]);
const nodeList = ref([]);
const WarehouseAreaList = ref([]);

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    areaId: "",
    nodeId: "",
    nodeCode: "",
    isLock: false,
    code: "",
    name: "",
    type: "Default",
    state: "Default",
    barcode: "",
    isEnable: true,
    sortNumber: 999
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

function warehouseNodeIdChange(value: string) {
  const selectedItem = nodeList.value.find(item => item.id === value);
  if (selectedItem) {
    newFormInline.value.nodeCode = selectedItem.code;
  } else {
    newFormInline.value.nodeCode = "";
  }
  console.log("newFormInline is", newFormInline.value);
}

defineExpose({ getRef });

onMounted(async () => {
  StorageTypeList.value = await GetDictItemInLocal("StorageType");
  StorageStateList.value = await GetDictItemInLocal("StorageState");
  const { data } = await getNodeList();
  nodeList.value = [...data];
  const res = await getWarehouseAreaList();
  WarehouseAreaList.value = [...res.data];
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
        <el-form-item :label="$t('table.site')" prop="nodeId">
          <el-select
            v-model="newFormInline.nodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
            @change="warehouseNodeIdChange"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in nodeList"
              :key="item.id"
              :label="item.code"
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
        <el-form-item :label="$t('table.name')">
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
              v-for="item in StorageTypeList"
              :label="item.name"
              :value="item.code"
              :key="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.sort')">
          <el-input-number
            v-model="newFormInline.sortNumber"
            :min="0"
            :max="999"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.isLock')">
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
        <el-form-item :label="$t('table.isEnable')">
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

      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.status')" prop="state">
          <el-select
            v-model="newFormInline.state"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in StorageStateList"
              :label="item.name"
              :value="item.code"
              :key="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.barcode')">
          <el-input
            v-model="newFormInline.barcode"
            :placeholder="$t('table.barcode')"
            :autosize="{ minRows: 12, maxRows: 18 }"
            type="textarea"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
