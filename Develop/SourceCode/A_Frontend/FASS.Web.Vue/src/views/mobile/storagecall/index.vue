<script setup lang="ts">
import { onMounted, ref, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import Header from "../home/components/header.vue";
import { reactive } from "vue";
import type { FormRules } from "element-plus";
const router = useRouter();
import { useHook } from "./utils/hook";
const formRef = ref();
const {
  query,
  areaList,
  MaterialList,
  modelList,
  storageList,
  loading,
  deviceDetection,
  AreaChange,
  handleSearch,
  handleConfirm,
  handleReset
} = useHook();

const formRules = reactive(<FormRules>{
  areaId: [{ required: true, message: "必填项", trigger: "blur" }],
  storageId: [{ required: true, message: "必填项", trigger: "blur" }],
  callMode: [{ required: true, message: "必填项", trigger: "blur" }],
  materialId: [{ required: true, message: "必填项", trigger: "blur" }]
});

// 用于存储序列号与类名的映射关系
const seriesClassMap = reactive({});
// 可用的样式类名数组
const availableClasses = ["red", "#905905", "skyblue", "pink"];
let classIndex = 0;

// 获取特定材料的类名
const getMaterialClass = material => {
  if (!material || !material.name) return "";

  // 提取材料名称中的序列号部分（假设格式类似）
  const matches = material.name.match(/^(?:[^-]*-){2}[^-]*/);
  if (!matches || !matches[1]) return "";

  const seriesCode = matches[1];

  // 如果该序列号尚未分配类名，且还有可用的类名
  if (!seriesClassMap[seriesCode] && classIndex < availableClasses.length) {
    seriesClassMap[seriesCode] = availableClasses[classIndex++];
  }

  // 返回该序列号对应的类名
  return { color: seriesClassMap[seriesCode] || "" };
};

watch(
  () => query.callMode,
  newValue => {
    if (newValue !== "EmptyOffline") {
      formRules.materialId[0].required = true;
    } else {
      formRules.materialId[0].required = false;
    }
  }
);

// EmptyOffline
// 除了放空桶之外，都要选择物料编码
</script>

<template>
  <div class="menuItem">
    <div style="height: 5rem"><Header class="header" /></div>
    <div :class="[deviceDetection() && 'flex-wrap']">
      <div
        :class="[deviceDetection() ? ['w-full', 'mt-2'] : 'w-[calc(100%)]']"
      />
      <el-card style="width: 100%; margin-top: -1.2rem">
        <template #header>
          <div class="card-header">
            <span class="text-xl font-bold">{{ $t("table.callarea") }}</span>
          </div>
        </template>
        <el-form
          ref="formRef"
          :model="query"
          :rules="formRules"
          class="search-form bg-bg_color w-[99/100] pl-8 pt-[12px] overflow-auto"
          label-position="top"
        >
          <el-form-item :label="$t('table.area')" prop="areaId">
            <el-select
              v-model="query.areaId"
              :placeholder="$t('table.pleaseSelect')"
              @change="AreaChange(query.areaId)"
            >
              <!-- <el-option label="不指定" value="" /> -->
              <el-option
                v-for="item in areaList"
                :key="item.id"
                :label="item.name"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item :label="$t('table.storage')" prop="storageId">
            <el-select
              v-model="query.storageId"
              :placeholder="$t('table.pleaseSelect')"
            >
              <!-- <el-option label="不指定" value="" /> -->
              <el-option
                v-for="item in storageList"
                :key="item.id"
                :label="item.name"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item :label="$t('table.callmodel')" prop="callMode">
            <el-select
              v-model="query.callMode"
              :placeholder="$t('table.pleaseSelect')"
            >
              <!-- <el-option label="不指定" value="" /> -->
              <el-option
                v-for="item in modelList"
                :key="item.id"
                :label="item.name"
                :value="item.code"
              />
            </el-select>
          </el-form-item>
          <el-form-item :label="$t('table.materialCode')" prop="materialId">
            <el-select
              v-model="query.materialId"
              :placeholder="$t('table.pleaseSelect')"
            >
              <!-- <el-option label="不指定" value="" /> -->
              <el-option
                v-for="item in MaterialList"
                :key="item.id"
                :label="item.name"
                :value="item.id"
              >
                <div class="flex items-center">
                  <el-tag
                    :color="getMaterialClass(item).color"
                    style="margin-right: 8px"
                    size="small"
                  />
                  <span :style="getMaterialClass(item)">{{ item.name }}</span>
                </div>
              </el-option>
            </el-select>
          </el-form-item>
          <el-form-item>
            <el-button
              type="primary"
              :loading="loading"
              @click="handleConfirm(formRef)"
            >
              {{ $t("buttons.pureConfirm") }}
            </el-button>
            <el-button @click="handleReset(formRef)">
              {{ $t("buttons.reset") }}
            </el-button>
          </el-form-item>
        </el-form>
        <!-- <p v-for="o in 4" :key="o" class="text item">
          {{ "List item " + o }}
        </p> -->
      </el-card>
    </div>
  </div>
</template>

<style scoped>
::v-deep(.el-card__header) {
  border-bottom: none;
}
.grow .main-content {
  margin: 0;
}
::v-deep(.el-card__body) {
  padding: 0;
}

.el-tag {
  border: none;
  aspect-ratio: 1;
}
</style>
