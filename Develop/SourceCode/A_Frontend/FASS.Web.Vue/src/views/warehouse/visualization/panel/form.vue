<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { FormProps, FormInline } from "./utils/types";

import { useHook } from "./utils/hook";
const props = withDefaults(defineProps<FormProps>(), {
  formInline: () =>
    ({
      colour: "",
      id: "",
      name: "",
      isEnable: true,
      isLock: false,
      sortNumber: 0,
      value: "",
      keyValue: "",
      type: ""
    }) as FormInline
});
const { tagData, selectedItemId, newFormInline, addTagFn, TagDeleteFn } =
  useHook(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return newFormInline.value;
}

defineExpose({ getRef });

const sortedTagData = computed(() => {
  if (!selectedItemId.value) {
    return tagData.value;
  }
  const selectedItem = tagData.value.find(
    item => item.id === selectedItemId.value
  );
  const otherItems = tagData.value.filter(
    item => item.id !== selectedItemId.value
  );
  return [selectedItem, ...otherItems];
});
</script>

<template>
  <el-form ref="ruleFormRef" label-width="100px">
    <el-row :gutter="30">
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.tags')">
          <div
            v-for="(item, index) in sortedTagData"
            :key="index"
            class="tag-item"
            :class="{ 'highlight-background': item.id === selectedItemId }"
            @click="addTagFn(item, 'add')"
          >
            {{ item.name }}
            <el-button
              v-if="item.id === selectedItemId"
              class="delBtn"
              text
              @click.stop="TagDeleteFn(item, 'del')"
              >X</el-button
            >
          </div>
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>

<style scoped lang="scss">
.tag-item {
  margin: 10px;
  padding: 0 10px;
  height: 40px;
  width: auto;
  border-radius: 4px;
  background-color: #eee;
  position: relative;
}
.tag-item:hover {
  background-color: #999;
}
.highlight-background {
  background-color: rgb(0, 183, 255);
}
.delBtn {
  position: absolute;
  right: -15px;
  top: -10px;
  width: 10px;
  height: 10px;
}
</style>
