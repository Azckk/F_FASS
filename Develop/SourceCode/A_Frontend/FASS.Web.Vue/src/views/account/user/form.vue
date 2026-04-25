<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { usePublicHooks } from "../hooks";

const { switchStyle } = usePublicHooks();

const selectGenderList = ref([]);
const selectAvatarList = ref([]);

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    username: "",
    name: "",
    nick: "",
    gender: "",
    birthday: "",
    phone: "",
    mail: "",
    avatar: "",
    isOnline: false,
    isSystem: false,
    isEnable: true,
    remark: ""
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  selectGenderList.value = (await getListToSelect("UserGender")).data;
  selectAvatarList.value = [
    { id: "1", label: "男1", value: "boy1.png" },
    { id: "2", label: "男2", value: "boy2.png" },
    { id: "3", label: "男3", value: "boy3.png" },
    { id: "4", label: "男4", value: "boy4.png" },
    { id: "5", label: "男5", value: "boy5.png" },
    { id: "6", label: "男6", value: "boy6.png" },
    { id: "7", label: "男7", value: "boy7.png" },
    { id: "8", label: "男8", value: "boy8.png" },
    { id: "9", label: "男1", value: "girl1.png" },
    { id: "10", label: "男2", value: "girl2.png" },
    { id: "11", label: "男3", value: "girl3.png" },
    { id: "12", label: "男4", value: "girl4.png" },
    { id: "13", label: "男5", value: "girl5.png" },
    { id: "14", label: "男6", value: "girl6.png" },
    { id: "15", label: "男7", value: "girl7.png" },
    { id: "16", label: "男8", value: "girl8.png" }
  ];
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
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.username')" prop="username">
          <el-input
            v-model="newFormInline.username"
            clearable
            :placeholder="$t('table.username')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.uname')">
          <el-input
            v-model="newFormInline.name"
            clearable
            :placeholder="$t('table.uname')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.nick')">
          <el-input
            v-model="newFormInline.nick"
            clearable
            :placeholder="$t('table.nick')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.gender')">
          <el-select
            v-model="newFormInline.gender"
            :placeholder="$t('table.gender')"
            clearable
            filterable
          >
            <el-option
              v-for="item in selectGenderList"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.birthday')">
          <el-date-picker
            v-model="newFormInline.birthday"
            clearable
            type="date"
            :placeholder="$t('table.birthday')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.phone')">
          <el-input
            v-model="newFormInline.phone"
            clearable
            :placeholder="$t('table.phone')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.mail')">
          <el-input
            v-model="newFormInline.mail"
            clearable
            :placeholder="$t('table.mail')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.avatar')">
          <el-select
            v-model="newFormInline.avatar"
            :placeholder="$t('table.avatar')"
            clearable
            filterable
          >
            <el-option
              v-for="item in selectAvatarList"
              :key="item.id"
              :label="item.label"
              :value="item.value"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.systemUser')">
          <el-switch
            v-model="newFormInline.isSystem"
            inline-prompt
            :active-value="true"
            :inactive-value="false"
            :active-text="$t('table.yes')"
            :inactive-text="$t('table.no')"
            :style="switchStyle"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.isEnable')">
          <el-switch
            v-model="newFormInline.isEnable"
            inline-prompt
            :active-value="true"
            :inactive-value="false"
            active-text="是"
            inactive-text="否"
            :style="switchStyle"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.remark')">
          <el-input
            v-model="newFormInline.remark"
            :placeholder="$t('table.remark')"
            type="textarea"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
