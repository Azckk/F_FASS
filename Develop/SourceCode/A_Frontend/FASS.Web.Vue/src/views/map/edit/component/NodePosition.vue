<template>
  <el-dialog
    v-model="show"
    :close-on-press-escape="false"
    :close-on-click-modal="false"
    :destroy-on-close="true"
    :title="title"
    width="60%"
    append-to-body
    @close="close"
  >
    <el-form
      ref="formRef"
      :model="form"
      :size="size"
      :label-position="labelPosition"
      label-width="100px"
    >
      <el-form-item label="坐标(X)">
        <el-input-number
          v-model="form.data.x"
          :min="0"
          :max="99999"
          :step="1"
          disabled="disabled"
        />
      </el-form-item>
      <el-form-item label="坐标(Y)">
        <el-input-number
          v-model="form.data.y"
          :min="0"
          :max="99999"
          :step="1"
          disabled="disabled"
        />
      </el-form-item>
      <el-form-item label="角度">
        <el-input-number
          v-model="form.data.theta"
          :min="-180"
          :max="180"
          :step="1"
        />
      </el-form-item>
      <el-form-item label="精度坐标">
        <el-input-number
          v-model="form.data.allowedDeviationXY"
          :min="0"
          :max="99999"
          :step="1"
        />
      </el-form-item>
      <el-form-item label="精度角度">
        <el-input-number
          v-model="form.data.allowedDeviationTheta"
          :min="0"
          :max="99999"
          :step="1"
        />
      </el-form-item>
      <el-form-item label="地图编号">
        <el-input
          v-model="form.data.mapId"
          placeholder="地图编号"
          maxlength="50"
          disabled="disabled"
        />
      </el-form-item>
      <el-form-item label="地图描述">
        <el-input
          v-model="form.data.mapDescription"
          placeholder="地图描述"
          maxlength="99999"
          :rows="3"
          type="textarea"
        />
      </el-form-item>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button type="primary" @click="close">关闭</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script setup>
import { reactive, ref } from "vue";

const formRef = ref(null);
const show = ref(false);
const title = ref("编辑");

const size = ref("small");
const labelPosition = ref("left");

const form = reactive({
  data: {
    x: 0,
    y: 0,
    theta: 0,
    allowedDeviationXY: 0,
    allowedDeviationTheta: 0,
    mapId: "",
    mapDescription: ""
  }
});

const open = shape => {
  show.value = true;
  form.data = shape.data.nodePosition;
};

const close = () => {
  show.value = false;
};

defineExpose({
  open,
  close
});
</script>

<style scoped></style>
