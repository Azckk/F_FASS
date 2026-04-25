<template>
  <el-dialog
    id="action-dialog"
    v-model="show"
    :close-on-press-escape="false"
    :close-on-click-modal="false"
    :destroy-on-close="true"
    width="60%"
    append-to-body
    @close="close"
  >
    <template #header>
      <div style="font-size: 16px" class="my-header">
        <h4>执行动作</h4>
      </div>
    </template>
    <el-card class="box-card" style="margin: 10px 0 10px">
      <el-collapse>
        <el-collapse-item name="1">
          <template #title>
            <span style="margin-right: 10px">搜索条件</span>
            <el-button :icon="Delete" @click="clearSearch">清空搜索</el-button>
            <el-button type="primary" :icon="Search" @click="SearchTable"
              >搜索</el-button
            >
          </template>
          <el-form
            ref="ruleFormRef"
            :inline="true"
            :model="where"
            class="demo-form-inline"
          >
            <el-form-item label="动作类型">
              <el-input v-model="where.actionType" />
            </el-form-item>
            <el-form-item label="堵塞类型">
              <el-input v-model="where.blockingType" />
            </el-form-item>
          </el-form>
        </el-collapse-item>
      </el-collapse>
    </el-card>
    <el-card class="box-card">
      <div id="user">
        <div class="main">
          <!-- 表格操作框 -->
          <div class="operation">
            <el-button-group class="ml-4">
              <el-button type="primary" @click="lookfrom">查看</el-button>
              <el-button type="primary" @click="addfrom">添加</el-button>
              <el-button type="primary" @click="Modify">修改</el-button>
              <el-button type="primary" @click="delete_from">删除</el-button>
              <!-- <el-button color=" #f1416c"   @click="delete_from">批量禁用</el-button> -->
            </el-button-group>
            <el-button-group class="ml-4">
              <el-button
                color="#f4f4f4"
                :dark="false"
                :icon="Refresh"
                @click=""
              >
                <span>刷新</span>
              </el-button>
              <el-dropdown :hide-on-click="false">
                <el-button color="#f4f4f4" :icon="Memo">
                  列<el-icon class="el-icon--right"><arrow-down /></el-icon>
                </el-button>
                <template #dropdown>
                  <el-dropdown-menu>
                    <el-dropdown-item>
                      <el-checkbox
                        v-model="fromShow.data.actionType"
                        label="动作类型"
                      />
                    </el-dropdown-item>
                    <el-dropdown-item>
                      <el-checkbox
                        v-model="fromShow.data.blockingType"
                        label="堵塞类型"
                      />
                    </el-dropdown-item>
                    <el-dropdown-item>
                      <el-checkbox
                        v-model="fromShow.data.sortNumber"
                        label="排序号"
                      />
                    </el-dropdown-item>
                  </el-dropdown-menu>
                </template>
              </el-dropdown>
            </el-button-group>
          </div>
          <!-- 表格 -->
          <el-table
            ref="multipleTableRef"
            :data="checkData.data"
            style="width: 100%"
            @select="selectTable"
            @select-all="handleSelectionChange"
          >
            <el-table-column type="selection" width="55" />
            <el-table-column
              v-if="fromShow.data.actionType"
              property="actionType"
              label="动作类型"
            />
            <el-table-column
              v-if="fromShow.data.blockingType"
              property="blockingType"
              label="堵塞类型"
            >
              <template #default="scope">
                {{ scope.row.blockingType }}
              </template>
            </el-table-column>
            <el-table-column
              v-if="fromShow.data.sortNumber"
              property="sortNumber"
              label="排序号"
            />
          </el-table>
        </div>
      </div>
      <el-pagination class="pagination" layout="total" :total="total" />
    </el-card>
    <template #footer>
      <span class="dialog-footer">
        <el-button type="primary" @click="close">关闭</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script setup>
import {
  Delete,
  Search,
  ArrowDown,
  Refresh,
  Memo,
  Share,
  InfoFilled
} from "@element-plus/icons-vue";
import { addDialog } from "@/components/ReDialog";
import ActionParameterIndex from "./ActionParameterIndex/index.vue";
import ActionForm from "./actionform/form.vue";
import { h, reactive, ref, computed, onMounted, nextTick, watch } from "vue";
// import {  MotionAdd , MotionSearchToPage} from "@/api/motion"
// import userApi from "../../api/user";
import { ElMessage } from "element-plus";
import { v4 as uuidv4 } from "uuid";
const formRef = ref();
const multipleTableRef = ref();
const fromShow = reactive({
  data: {
    actionType: true,
    blockingType: true,
    sortNumber: true
  }
});
//动作对象
const table = reactive({
  data: [] //获取的所有数据
});
const checkData = reactive({
  data: []
});
const total = ref(0);
//搜索字段
const where = reactive({
  actionType: "", //动作类型
  blockingType: "" //堵塞类型
});
const tableRef = ref(null);
const show = ref(false);
const dialogFormVisible = ref(false);
const title = ref("编辑");
const multipleSelection = ref([]);
let InnerRuleFormRef = ref();

//选择
const selectTable = (value, row) => {
  multipleSelection.value = value;
  // console.log("value, row" , value, "\n" , row)
};
const handleSelectionChange = val => {
  multipleSelection.value = val;
};

//封装的，确认至少有一条选中数据的方法
const confirm_is_select = () => {
  if (!multipleSelection.value) return 0;
  if (multipleSelection.value.length === 1) {
    return 1;
  } else if (multipleSelection.value.length >= 1) {
    return 2;
  } else {
    return 0;
  }
};
//打开dialog
const open = shape => {
  show.value = true;
  checkData.data.length = shape.data.actions.length;
  checkData.data = shape.data.actions;
  table.data = shape.data.actions;
  total.value = checkData.data.length;
  // checkData.data = GetMotionSearchToPage(shape.data.actions);
  // console.log("actions", shape, "\n", table.data, checkData.data);
  // nextTick(() => {
  //   for (let i = 0; i < table.data.length; i++) {
  //     for (let j = 0; j < checkData.data.length; j++) {
  //       if (table.data[i].motionID == checkData.data[j].actionId) {
  //         // multipleTableRef.value.toggleRowSelection(table.data[i],true)
  //         table.data[i].IsSelect = true;
  //       }
  //     }
  //   }
  // });
};

const close = () => {
  show.value = false;
};

watch(
  () => checkData.data.length,
  (newValue, oldValue) => {
    table.data.forEach(element => {
      element.IsSelect = false;
    });
    setTimeout(() => {
      for (let i = 0; i < table.data.length; i++) {
        for (let j = 0; j < checkData.data.length; j++) {
          if (table.data[i].motionID == checkData.data[j].actionId) {
            // multipleTableRef.value.toggleRowSelection(table.data[i],true)
            table.data[i].IsSelect = true;
          }
        }
      }
    }, 100);
  }
);

function handleRow(row = undefined) {
  return {
    actionId: row?.actionId ?? uuidv4(),
    actionParameters: row?.actionParameters ?? [],
    sortNumber: row?.sortNumber ?? null,
    actionType: row?.actionType ?? "",
    actionDescription: row?.actionDescription ?? "",
    blockingType: row?.blockingType ?? ""
  };
}

//增删改查
const lookfrom = () => {
  console.log(" multipleSelection.value ", multipleSelection.value);
  if (confirm_is_select() === 1) {
    addDialog({
      title: "查看",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      hideFooter: true,
      props: { formInline: handleRow(multipleSelection.value[0]) },
      contentRenderer: () => h(ActionForm, { ref: formRef })
    });
  } else {
    ElMessage({
      message: "请选择一条数据",
      type: "warning"
    });
  }
};
const addfrom = () => {
  addDialog({
    title: "添加",
    width: "60%",
    alignCenter: true,
    draggable: true,
    fullscreenIcon: true,
    closeOnClickModal: false,
    props: { formInline: handleRow() },
    contentRenderer: () => h(ActionForm, { ref: formRef }),
    beforeSure: (done, { options, index }) => {
      const formData = options.props.formInline;
      console.log(" formData is", formData);
      checkData.data.push(formData);
      total.value = checkData.data.length;
      done();
    }
  });
};
const Modify = () => {
  if (confirm_is_select() === 1) {
    addDialog({
      title: "修改",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      props: { formInline: handleRow(multipleSelection.value[0]) },
      contentRenderer: () => h(ActionForm, { ref: formRef }),
      beforeSure: (done, { options, index }) => {
        const formData = options.props.formInline;
        for (let j = 0; j < checkData.data.length; j++) {
          let item = checkData.data[j];
          if (item.actionId == formData.actionId) {
            Object.assign(item, formData);
            break; // 更新后跳出循环
          }
        }
        done();
      }
    });
  } else {
    ElMessage({
      message: "请选择一条数据",
      type: "warning"
    });
  }
};
const delete_from = () => {
  if (confirm_is_select() !== 0) {
    for (let i = 0; i < multipleSelection.value.length; i++) {
      for (let j = 0; j < checkData.data.length; j++) {
        if (multipleSelection.value[i].actionId == checkData.data[j].actionId) {
          checkData.data.splice(j, 1);
        }
      }
    }
    multipleTableRef.value.clearSelection();
    multipleSelection.value.length = 0;
    total.value = checkData.data.length;
    // console.log("checkData.data" ,checkData.data)
    ElMessage({
      message: "删除成功",
      type: "success"
    });
  } else {
    ElMessage({
      message: "请至少选择一条数据",
      type: "warning"
    });
  }
};
const GetMotionSearchToPage = actionData => {
  return actionData.filter(action => {
    return (
      (!where.actionType || action.actionType.includes(where.actionType)) &&
      (!where.blockingType || action.blockingType.includes(where.blockingType))
    );
  });
};

//清空搜索
const clearSearch = event => {
  event.stopPropagation();
  Object.assign(where, {
    actionType: "", //动作类型
    blockingType: "" //堵塞类型
  });
  checkData.data = GetMotionSearchToPage(table.data);
  total.value = checkData.data.length;
};
//搜索
const SearchTable = event => {
  checkData.data = GetMotionSearchToPage(table.data);
  total.value = checkData.data.length;
  event.stopPropagation();
};

onMounted(() => {
  // GetMotionSearchToPage();
});

//框中框逻辑
const dialogType = ref("");
const activeNames = ref([1]);
const rules = reactive({
  motionDescription: [
    { required: true, message: "请输入动作描述", trigger: "change" }
  ],
  motionType: [
    {
      required: true,
      message: "Please select Activity zone",
      trigger: "change"
    }
  ],
  motionID: [
    {
      required: true,
      message: "Please select Activity count",
      trigger: "change"
    }
  ],
  motionParameter: [
    {
      required: true,
      message: "请输入动作边界",
      trigger: "change"
    }
  ]
});

defineExpose({
  open,
  close
});
</script>

<style scoped>
::v-deep(.el-form-item:not(.special)) {
  width: 44%;
}
::v-deep(.el-dialog) {
  margin-top: 28vh;
}
#action-dialog {
  margin-top: 28vh;
}
.operation {
  display: flex;
  justify-content: space-between;
}
</style>
