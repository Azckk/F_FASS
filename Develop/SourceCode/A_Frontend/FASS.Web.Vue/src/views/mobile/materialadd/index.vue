<script setup lang="ts">
import { ref, watch } from "vue";
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
  // modelList,
  storageList,
  loading,
  deviceDetection,
  AreaChange,
  handleSearch,
  handleConfirm,
  handleReset,
  containerListData
} = useHook();

const formRules = reactive(<FormRules>{
  name: [{ required: true, message: "必填项", trigger: "blur" }],
  // code: [{ required: true, message: "必填项", trigger: "blur" }],
  barcode: [{ required: true, message: "必填项", trigger: "blur" }]
});

const modelList = ref([]);

// watch(
//   () => query.callMode,
//   newValue => {
//     if (newValue !== "EmptyOffline") {
//       formRules.materialId[0].required = true;
//     } else {
//       formRules.materialId[0].required = false;
//     }
//   }
// );

watch(
  () => query.area,
  newValue => {
    query.callMode = ""; //切换库区清空容器值
    modelList.value = containerListData.value.filter(item => {
      //筛选库区对应容器
      return item.areaCode === newValue;
    });
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
            <span class="text-xl font-bold">{{ $t("物料登记") }}</span>
          </div>
        </template>
        <el-form
          ref="formRef"
          :model="query"
          :rules="formRules"
          class="search-form bg-bg_color w-[99/100] pl-8 pt-[12px] overflow-auto"
          label-position="top"
        >
          <el-form-item :label="$t('物料名称')" prop="name">
            <el-input
              v-model="query.name"
              filterable
              class="!w-[200px]"
              :placeholder="$t('table.pleaseEnter')"
            >
              <!-- <el-option label="不指定" value="" /> -->
            </el-input>
          </el-form-item>
          <!-- <div>{{ containerListData }}</div> -->
          <el-form-item :label="$t('物料条码')" prop="barcode">
            <!-- <el-input
              v-model="query.barcode"
              filterable
              class="!w-[200px]"
              :placeholder="$t('table.pleaseEnter')"
            > -->
            <el-input
              v-model="query.barcode"
              :placeholder="$t('物料条码')"
              class="!w-[200px]"
              :rows="7"
              type="textarea"
            />
          </el-form-item>

          <!-- <el-form-item :label="$t('table.materialCode')" prop="code">
            <el-input
              v-model="query.code"
              MaterialList
              :placeholder="$t('table.materialCode')"
              clearable
              class="!w-[200px]"
            />
          </el-form-item> -->

          <el-form-item :label="$t('绑定库区')" prop="area">
            <el-select
              v-model="query.area"
              filterable
              class="!w-[200px]"
              :placeholder="$t('table.pleaseSelect')"
            >
              <el-option label="不指定" value="" />
              <el-option
                v-for="item in areaList"
                :key="item.id"
                :label="item.name"
                :value="item.code"
              />
            </el-select>
          </el-form-item>

          <el-form-item :label="$t('绑定容器')" prop="callMode">
            <el-select
              v-model="query.callMode"
              filterable
              class="!w-[200px]"
              :placeholder="$t('table.pleaseSelect')"
            >
              <el-option label="不指定" value="" />
              <el-option
                v-for="item in modelList"
                :key="item.id"
                :label="item.name"
                :value="item.id"
              />
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
</style>
