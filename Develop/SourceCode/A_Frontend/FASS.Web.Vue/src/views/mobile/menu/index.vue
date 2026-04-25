<script setup lang="ts">
import { ref } from "vue";
import { useRouter, useRoute } from "vue-router";
const router = useRouter();
import { useHook } from "./utils/hook";
const { handleRouter } = useHook();
</script>

<template>
  <div class="bg-white main-content">
    <div class="title">
      <span>任务下发</span>
    </div>
    <el-row :gutter="20">
      <el-col :span="8">
        <div
          class="grid-content ep-bg-purple"
          @click="handleRouter('/mobile/runtime/index')"
        >
          <img src="@/assets/mobile/menu2/运行监控.png" alt="" />
          <div>运行监控</div>
        </div>
      </el-col>
      <el-col :span="8"
        ><div
          class="grid-content ep-bg-purple"
          @click="handleRouter('/mobile/visualization/index')"
        >
          <img src="@/assets/mobile/menu2/库位可视化.png" alt="" />
          <div>库位可视化</div>
        </div>
      </el-col>
      <el-col :span="8"
        ><div
          class="grid-content ep-bg-purple"
          @click="handleRouter('/mobile/storagecall/index')"
        >
          <img src="@/assets/mobile/menu2/取料呼叫.png" alt="" />
          <div>缺料呼叫</div>
        </div>
      </el-col>
    </el-row>
    <el-row :gutter="24">
      <el-col :span="8">
        <div
          class="grid-content ep-bg-purple"
          @click="handleRouter('/mobile/flowcartasksingle/index')"
        >
          <img src="@/assets/mobile/menu2/单点任务.png" alt="" />
          <div>单点任务</div>
        </div>
      </el-col>
      <el-col :span="8"
        ><div
          class="grid-content ep-bg-purple"
          @click="handleRouter('/mobile/flowcartaskdouble/index')"
        >
          <img src="@/assets/mobile/menu2/双点任务.png" alt="" />
          <div>双点任务</div>
        </div>
      </el-col>
      <el-col :span="8"
        ><div
          class="grid-content ep-bg-purple"
          @click="handleRouter('/mobile/storagecalllist/index')"
        >
          <img src="@/assets/mobile/menu2/缺料呼叫历史.png" alt="" />
          <div>缺料呼叫历史</div>
        </div>
      </el-col>
    </el-row>
    <div class="title">
      <span>基础信息维护</span>
    </div>
    <el-row :gutter="24">
      <el-col :span="6">
        <div
          class="grid-content ep-bg-purple"
          @click="handleRouter('/mobile/warehousedatacontainer/index')"
        >
          <img src="@/assets/mobile/menu2/容器数据.png" alt="" />
          <div>容器数据</div>
        </div>
      </el-col>
      <el-col :span="6"
        ><div
          class="grid-content ep-bg-purple"
          @click="handleRouter('/mobile/warehousedatamaterial/index')"
        >
          <img src="@/assets/mobile/menu2/物料数据.png" alt="" />
          <div>物料数据</div>
        </div>
      </el-col>
      <el-col :span="6"
        ><div
          class="grid-content ep-bg-purple"
          @click="handleRouter('/mobile/warehousedatastorage/index')"
        >
          <img src="@/assets/mobile/menu2/储位数据.png" alt="" />
          <div>储位数据</div>
        </div>
      </el-col>
      <el-col :span="6"
        ><div
          class="grid-content ep-bg-purple"
          @click="handleRouter('/mobile/materialadd/index')"
        >
          <img src="@/assets/mobile/menu2/储位数据.png" alt="" />
          <div>物料登记</div>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<style scoped>
.bg-white {
  height: 100%;
  background: #ebeef4;
}
.main-content {
  padding: 10px;
  margin-right: 10px;
}
.el-row {
  margin-bottom: 20px;
}
.el-row:last-child {
  margin-bottom: 0;
}
.el-col {
  border-radius: 4px;
}

.grid-content {
  border-radius: 4px;
  width: calc(256px / 2);
  height: calc(226px / 2);
  background-color: #fff;
  text-align: center;
  font-weight: bold;
  font-size: 12px;
  color: #262626;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  img {
    width: calc(136px / 2);
    height: calc(115px / 2);
  }
}
.title {
  font-size: 16px;
  font-weight: bold;
  margin-bottom: 10px;
}
</style>
