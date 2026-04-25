<template>
  <el-dialog 
    center
    v-model="show"
    :close-on-press-escape="false"
    :close-on-click-modal="false"
    :destroy-on-close="true"
    :title="title"
    width="50%"
    append-to-body
    @close="close"
    >
    <el-form ref="formRef" :model="form" :size="size" :label-position="labelPosition" label-width="100px">
      <el-form-item label="控制点数量">
        <el-input-number v-model="form.data.degree" style="width: 100%;" :min="0" :max="999" :step="1"  />
      </el-form-item>
      <el-form-item label="控制点 NURBS">
        <el-input v-model="form.data.knotVector[0]" placeholder="地图描述" maxlength="99999" :rows="5" type="textarea" />
      </el-form-item>
      <el-form-item label="控制点">
        <el-button type="primary" @click="OpenInnerDialog" :icon="Operation" style="width: 100%;" />
      </el-form-item>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button type="primary" @click="close">关闭</el-button>
      </span>
    </template>
  </el-dialog>
    <el-dialog
        v-model="innerVisible"
        title="坐标参数"
        :modal="false"

      >
        <template #default>
          <el-card class="box-card" style="margin: 10px 0 10px ;">
              <el-collapse v-model="activeNames">
                    <el-collapse-item name="1">
                        <template #title>
                            <span style="margin-right: 10px ;">搜索条件</span>
                            <el-button  :icon="Delete" @click="clearSearch">清空搜索</el-button>
                            <el-button type="primary" :icon="Search" @click="SearchTable" >搜索</el-button>
                        </template>
                        <el-form ref="ruleFormRef" :inline="true"  :model="where" class="demo-form-inline">
                            <el-form-item label="坐标(X)">
                              <el-input-number style="width: 100%;" v-model="where.x"  />
                            </el-form-item>
                            <el-form-item label="坐标(Y)">
                              <el-input v-model="where.y"  />
                            </el-form-item>
                            <el-form-item label="权重">
                              <el-input v-model="where.weight"  />
                            </el-form-item>
                        </el-form>
                    </el-collapse-item>
              </el-collapse>
          </el-card>
          <el-card  class="box-card">
        <div id="user">
            <div class="main">
            <!-- 表格操作框 -->
            <div class="operation">
                <!-- <el-button-group class="ml-4">
                  <el-button color=" #59c0fa"   @click="lookfrom">查看</el-button>
                  <el-button color=" #0095e8"   @click="addfrom">添加</el-button>
                  <el-button color=" #0095e8"   @click="modeify">修改</el-button>
                      <el-popconfirm
                        confirm-button-text="Yes"
                        cancel-button-text="No"
                        :icon="InfoFilled"
                        icon-color="#626AEF"
                        title="确定删除?"
                        @confirm="confirmEvent"
                        @cancel="cancelEvent"
                      >
                        <template #reference>
                          <el-button color=" #59c0fa">删除</el-button>
                        </template>
                      </el-popconfirm>
                </el-button-group> -->
                <el-button-group class="ml-4">
                    <el-button color="#f4f4f4" :dark="false" :icon="Refresh" @click="">
                        <span>刷新</span>
                    </el-button>
                    <el-dropdown  :hide-on-click="false">
                    <el-button color="#f4f4f4" :icon="Memo" >
                        列<el-icon class="el-icon--right"><arrow-down /></el-icon>
                    </el-button>
                    <template #dropdown>
                        <el-dropdown-menu>
                        <el-dropdown-item>
                            <el-checkbox v-model="fromShow.data.x" label="坐标(X)" />
                        </el-dropdown-item>
                        <el-dropdown-item>
                            <el-checkbox v-model="fromShow.data.y" label="坐标(Y)" />
                        </el-dropdown-item>
                        <el-dropdown-item>
                            <el-checkbox v-model="fromShow.data.weight" label="权重" />
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
                :data="table.data"
                style="width: 100%"
                @select="selectTable"
                @select-all ="handleSelectionChange"
                >
                <el-table-column type="selection" width="55" />
                <el-table-column v-if="fromShow.data.x" property="x" label="坐标(X)"  />
                <el-table-column v-if="fromShow.data.y" property="y" label="坐标(Y)"  />
                <el-table-column v-if="fromShow.data.weight" property="weight" label="权重">
                  <template #default="scope">
                    {{scope.row.weight?scope.row.weight:"-"}}
                  </template>
                </el-table-column>
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
        </template>
        <template #footer>
          <el-button type="primary" @click="innerVisible=false">确定</el-button>
          <el-button @click="innerVisible=false">取消</el-button>
        </template>
      </el-dialog>
      <el-dialog 
        center
        v-model="PositionShow"
        :close-on-press-escape="false"
        :close-on-click-modal="false"
        :destroy-on-close="true"
        width="50%"
        align-center
        append-to-body
        @close="closePosition"
        >
        <el-form ref="PositionFormRef" :model="PositionForm" :size="size" :label-position="labelPosition" label-width="100px">
          <el-form-item label="坐标(X)">
            <el-input-number v-model="PositionForm.data.x" style="width: 100%;" :min="0"  :step="1"  />
          </el-form-item>
          <el-form-item label="坐标(Y)">
            <el-input-number v-model="PositionForm.data.y" style="width: 100%;" :min="0"  :step="1"  />
          </el-form-item>
          <el-form-item label="权重">
            <el-input-number v-model="PositionForm.data.weight" style="width: 100%;" :min="0"  :step="1"  />
          </el-form-item>
        </el-form>
        <template #footer>
          <span class="dialog-footer">
            <el-button type="primary" @click="AddPosition">确定</el-button>
            <el-button  @click="closePosition">关闭</el-button>
          </span>
        </template>
      </el-dialog>
</template>

<script setup>
import { reactive, ref ,computed , watch} from 'vue';
import { Operation , Delete ,Search ,InfoFilled , Share ,ArrowDown ,Memo,Refresh} from '@element-plus/icons-vue'
const formRef = ref(null);
const show = ref(false);
const title = ref('路线轨迹');
const size = ref('small');
const labelPosition = ref('left');

const form = reactive({
  data: {
    "degree": 1,
    "knotVector": [
        0
    ],
    "controlPoints": [
   
    ]
  }
});

const open = (shape) => {
  show.value = true;
  form.data = JSON.parse(JSON.stringify( shape.trajectory));
  table.data = JSON.parse(JSON.stringify( form.data.controlPoints))
};

const close = () => {
  show.value = false;
};
const name = computed(() => {
  return firstName.value + '---' + lastName.value
})

//框内数据
const innerVisible = ref(false);
const PositionShow = ref(false);
const PositionFormRef = ref();
const dialogType = ref("");
const activeNames = ref(["1"]);
const currentPage = ref(1)
const pageSize = ref(10)
const multipleSelection = ref()
const total = ref(0)
const PositionForm = reactive(
  {data:{
    x:null,
    y:null,
    weight:null
  }}
)
const fromShow = reactive({
    data:{
      x:true,
      y:true,
      weight:true
    }
  })
//搜索字段
const where = reactive({
    x: null, //
    y: null,
    where: null

  })
  const table = reactive({
    data: []
  })
const OpenInnerDialog = (shape) => {
  innerVisible.value = true;
};

watch(()=>table.data.length,(newValue,oldValue)=>{
  form.data.controlPoints.length = 0;
  table.data.forEach((item,index)=>{
    form.data.controlPoints[index] =  
     {
      x:Number(item.x),
      y:Number(item.y),
      weight:Number(item.weight),
    }
  })
  console.log("checkData.data" ,form.data)
})
  //选择
const selectTable = (value, row)=>{
  multipleSelection.value = value
}
const handleSelectionChange = (val) => {
  multipleSelection.value = val
};
//清空搜索
const clearSearch = (event)=>{
  event.stopPropagation();
  Object.assign(where, {
    x: null, //
    y: null,
    where: null,
    });
    table.data = JSON.parse(JSON.stringify( form.data.controlPoints))
}
//搜索
const SearchTable = (event)=>{
  event.stopPropagation();
  table.data = table.data.filter(item => item.x === where.x || item.y === where.y);   
}
const handleSizeChange = (val) => {
}
const handleCurrentChange = (val) => {
}
//数据的增删改查
//封装的，确认至少有一条选中数据的方法
const confirm_is_select = ()=>{
  if(!multipleSelection.value)  return 0;
        if(multipleSelection.value.length===1){
            return 1;
        }else if(multipleSelection.value.length>=1){
            return 2;
        }else{
            return 0;
        }
}
//增删改查
const lookfrom = ()=>{
  if(confirm_is_select()===1){
      dialogType.value = "look";
    }else{
        ElMessage({
            message: '请选择一条数据',
            type: 'warning',
        })
    }
}
const addfrom = ()=>{
  dialogType.value = "add";
  PositionShow.value = true;
}

const modeify = ()=>{
  dialogType.value = "modeify";
}
const confirmEvent = () => {
}
const cancelEvent = () => {
}

const closePosition = ()=>{
  PositionShow.value = false
  Object.assign(PositionForm.data,{
      x:null,
      y:null,
      weight:null
  })
}
const AddPosition = ()=>{
  PositionShow.value = false
  table.data.push( JSON.parse(JSON.stringify(PositionForm.data)))
  // console.log(" table.data" ,  table.data)
  Object.assign(PositionForm.data,{
      x:null,
      y:null,
      weight:null
  })
}

defineExpose({
  open,
  close
});
</script>

<style lang="scss" scoped>
*{
  :deep(.box-card > .el-form-item){
        width: 28%;
    }
}
 
    .operation{
        display: flex;
        justify-content: space-between;
      }
</style>