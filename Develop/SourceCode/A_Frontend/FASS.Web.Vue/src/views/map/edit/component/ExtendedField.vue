<template>
  <el-dialog
    v-model="show"
    :close-on-press-escape="false"
    :close-on-click-modal="false"
    :destroy-on-close="true"
    draggable
    width="60%"
    append-to-body
    @close="close"
  >
    <template #header>
      <div style="font-size: 16px" class="my-header">
        <h4>扩展属性</h4>
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
            <el-form-item label="键">
              <el-input v-model="where.key" />
            </el-form-item>
            <el-form-item label="值">
              <el-input v-model="where.value" />
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
              <el-button color=" #59c0fa" @click="lookfrom">查看</el-button>
              <el-button color=" #0095e8" @click="addfrom">添加</el-button>
              <el-button color=" #59c0fa" @click="Modify">修改</el-button>
              <el-button color=" #f1416c" @click="delete_from">删除</el-button>
            </el-button-group>
            <el-button-group class="ml-4">
              <el-button
                color="#f4f4f4"
                :dark="false"
                :icon="Refresh"
                @click="SearchTable"
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
                      <el-checkbox v-model="fromShow.data.key" label="键" />
                    </el-dropdown-item>
                    <el-dropdown-item>
                      <el-checkbox v-model="fromShow.data.value" label="值" />
                    </el-dropdown-item>
                  </el-dropdown-menu>
                </template>
              </el-dropdown>
              <!-- <el-dropdown  :hide-on-click="false">
                    <el-button color="#f4f4f4" :icon="Share" >
                        导出数据<el-icon class="el-icon--right"><arrow-down /></el-icon>
                    </el-button>
                    <template #dropdown>
                        <el-dropdown-menu>
                            <el-dropdown-item>MS-Excel</el-dropdown-item>
                            <el-dropdown-item>MS-CSV</el-dropdown-item>
                        </el-dropdown-menu>
                    </template>
                    </el-dropdown> -->
            </el-button-group>
          </div>
          <!-- 表格 -->
          <el-table
            ref="multipleTableRef"
            :data="form.data"
            style="width: 100%"
            @select="selectTable"
            @select-all="handleSelectionChange"
          >
            <el-table-column type="selection" width="55" />
            <el-table-column
              v-if="fromShow.data.key"
              property="key"
              label="键"
            />
            <el-table-column
              v-if="fromShow.data.value"
              property="value"
              label="值"
            />
          </el-table>
        </div>
      </div>
      <el-pagination
        class="pagination"
        v-model:current-page="currentPage"
        v-model:page-size="pageSize"
        :page-sizes="[10, 20, 50, 100]"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
      />
    </el-card>
    <template #footer>
      <span class="dialog-footer">
        <el-button type="primary" @click="close">关闭</el-button>
      </span>
    </template>
  </el-dialog>
  <el-dialog v-model="innerVisible" draggable width="60%" :title="title">
    <template #default>
      <el-form
        label-width="60px"
        :disabled="inner_type == 'look'"
        :inline="true"
        :model="innerFrom.data"
        class="demo-form-inline"
      >
        <el-form-item label="Key">
          <el-input v-model="innerFrom.data.key" />
        </el-form-item>
        <el-form-item label="Value">
          <el-input type="textarea" v-model="innerFrom.data.value" />
        </el-form-item>
      </el-form>
    </template>
    <template #footer>
      <el-button @click="innerVisible = false">取消</el-button>
      <el-button type="success" v-if="inner_type == 'add'" @click="addDictItem"
        >确定</el-button
      >
      <el-button
        type="success"
        v-if="inner_type == 'modify'"
        @click="ModifyDictItem"
        >更新</el-button
      >
    </template>
  </el-dialog>
</template>

<script setup>
import { reactive, ref } from "vue";
import { ElMessage } from "element-plus";
import {
  Delete,
  Search,
  ArrowDown,
  Refresh,
  Memo,
  Share
} from "@element-plus/icons-vue";
const where = reactive({
  key: "",
  value: ""
});
const fromShow = reactive({
  data: {
    key: true,
    value: true
  }
});
//需要一份完整的数据，用于搜索
const completeData = reactive({
  data: []
});
const innerVisible = ref(false);
const formRef = ref(null);
const multipleTableRef = ref(null);
const show = ref(false);
const title = ref("编辑");
const inner_type = ref("编辑");
const size = ref("small");
const labelPosition = ref("left");
const form = reactive({
  data: [
    {
      key: "",
      value: ""
    }
  ]
});
const innerFrom = reactive({
  data: {
    key: "",
    value: ""
  }
});
const multipleSelection = ref(); //被选中的数组
const currentPage = ref(1);
const pageSize = ref(10);
const total = ref(0);
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
const handleSizeChange = val => {};
const handleCurrentChange = val => {};
//清空搜索
const clearSearch = event => {
  event.stopPropagation();
  Object.assign(where, {
    key: "",
    value: ""
  });
  form.data = completeData.data;
};

//搜索
const SearchTable = event => {
  form.data = completeData.data;
  event.stopPropagation();
  form.data = form.data.filter(
    item => item.key === where.key || item.value === where.value
  );
  total.value = form.data.length;
};
//查看
const lookfrom = () => {
  if (confirm_is_select() === 1) {
    innerFrom.data.key = "";
    innerFrom.data.value = "";
    innerVisible.value = true;
    inner_type.value = "look";
    title.value = "查看";
    innerFrom.data = multipleSelection.value[0];
  } else {
    ElMessage({
      message: "请选择一条数据",
      type: "warning"
    });
  }
};
//增加
const addfrom = () => {
  inner_type.value = "add";
  innerVisible.value = true;
};
//修改
const Modify = () => {
  if (confirm_is_select() === 1) {
    // console.log("multipleSelection",multipleSelection.value)
    inner_type.value = "modify";
    innerVisible.value = true;
    title.value = "修改";
    innerFrom.data.key = multipleSelection.value[0].key;
    innerFrom.data.value = multipleSelection.value[0].value;
    // console.log("multipleSelection.value[0]" ,multipleSelection.value[0] )
  } else {
    ElMessage({
      message: "请选择一条数据",
      type: "warning"
    });
  }
};
//删除
const delete_from = () => {
  if (confirm_is_select() !== 0) {
    let objectToRemove = multipleSelection.value;
    for (let i = 0; i < objectToRemove.length; i++) {
      const index = form.data.findIndex(
        item =>
          item.key === objectToRemove[i].key &&
          item.value === objectToRemove[i].value
      );
      if (index !== -1) {
        form.data.splice(index, 1); // 删除找到的对象
      }
    }
    completeData.data = JSON.parse(JSON.stringify(form.data));
    total.value = form.data.length;
  } else {
    ElMessage({
      message: "请至少选择一条数据",
      type: "warning"
    });
  }
};
//选择
const selectTable = (value, row) => {
  multipleSelection.value = value;
  // console.log("value, row" , value, "\n" , row)
};
//确定增加
const addDictItem = () => {
  form.data.push({
    key: innerFrom.data.key,
    value: innerFrom.data.value
  });
  completeData.data = JSON.parse(JSON.stringify(form.data));
  Object.assign(innerFrom.data, {
    key: "",
    value: ""
  });
  // console.log("form.data \n",form.data,
  // "innerFrom.data,\n",innerFrom.data)
  innerVisible.value = false;
  total.value = form.data.length;
};
//确定修改
const ModifyDictItem = () => {
  innerVisible.value = false;
  const index = form.data.findIndex(
    item =>
      item.key === multipleSelection.value[0].key &&
      item.value === multipleSelection.value[0].value
  );
  if (index !== -1) {
    form.data[index].key = innerFrom.data.key;
    form.data[index].value = innerFrom.data.value;
    // form.data.splice(index, 1); // 删除找到的对象
  }
  form.data = JSON.parse(JSON.stringify(form.data));
  completeData.data = JSON.parse(JSON.stringify(form.data));
  setTimeout(() => {
    innerFrom.data.key = "";
    innerFrom.data.value = "";
  }, 500);
  multipleTableRef.value.clearSelection();
  multipleSelection.value = [];
};
//点击全选
const handleSelectionChange = value => {
  multipleSelection.value = value;
};
//打开dialog
const open = shape => {
  show.value = true;
  form.data = shape.extends;
  //将完整的数据存储到这里
  completeData.data = JSON.parse(JSON.stringify(shape.extends));
  // console.log("shape is" , shape)
};
//关闭dialog
const close = () => {
  show.value = false;
  form.data = completeData.data;
};

defineExpose({
  open,
  close
});
</script>

<style lang="scss" scoped>
* {
  ::v-deep(.el-dialog__header) {
    border-bottom: 1px solid #ccc;
    font-size: 16px;
  }
  ::v-deep(.el-form-item) {
    width: 45%;
  }
}
.operation {
  display: flex;
  justify-content: space-between;
}
</style>
