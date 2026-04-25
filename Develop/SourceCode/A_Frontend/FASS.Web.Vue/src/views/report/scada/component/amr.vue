<template>
  <div class="car noScrollbar">
    <el-row class="wrap">
      <el-col :span="8" class="amr" v-for="amr of amrs" :key="amr.id">
        <div class="flex">
          <div class="left">
            <img src="../../../../assets/statistics/amr.png" alt="" />
            <!-- <img src="/static/amr.png" alt="" /> -->
          </div>
          <div class="right">
            <el-tooltip
              class="box-item"
              effect="light"
              :content="amr.name"
              placement="top"
            >
              <div class="name" v-if="amr.name.split('').length <= 16">
                <div style="float: left">名称：</div>
                <b>{{ amr.name }}</b>
              </div>
              <div class="name" v-else>
                <div style="float: left">名称：</div>
                <b>{{ amr.name.slice(0, 16) + "..." }}</b>
              </div>
            </el-tooltip>
            <span
              >状态：{{
                amr.state == 0
                  ? "离线"
                  : amr.state == 1
                    ? "空闲"
                    : amr.state == 2
                      ? "任务中"
                      : amr.state == 3
                        ? "充电中"
                        : "异常"
              }}</span
            >
          </div>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<script>
export default {
  name: "Amr",
  props: ["amrs"],
  data() {
    return {};
  },
  methods: {}
};
</script>

<style lang="scss" scoped>
.car {
  overflow-y: scroll;
  height: 400px;
  padding: 0;
  margin: 8px -8px 0px 8px;
  display: flex;
  justify-content: center;
}
b {
  font-weight: normal;
}
.wrap {
  width: 100%;
  height: 400px;
  .amr {
    height: 101px;
    padding-top: 10px;
    background: #f4eeff;
    display: flex;
    border-radius: 5px;
    border: 5px solid #fff;
    .left {
      width: 85px;
      align-items: center; /* 垂直居中 */
      display: flex;
      justify-content: center; /* 水平居中 */
      img {
        width: 73px;
        text-align: center;
        align-items: center;
      }
    }
    .right {
      flex: 1;
      font-size: 14px;
      color: #333333;
      .name {
        margin-top: 5px;
        height: 40px;
        line-height: 20px;
      }
    }
  }
}
</style>
