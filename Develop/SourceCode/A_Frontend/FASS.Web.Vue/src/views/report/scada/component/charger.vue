<template>
  <div class="wrap noScrollbar">
    <div v-if="chargers.length === 0" class="noData">
      <div class="charger">{{ $t("table.noChargingData") }}</div>
    </div>
    <div class="charger" v-for="charger of chargers" :key="charger.id">
      <span>{{ $t("table.name") }}：{{ charger.name }}</span
      ><br />
      <span
        >{{ $t("table.mode") }}：{{
          charger.isOnline ? $t("table.online") : $t("table.offline")
        }}</span
      ><br />
      <span
        >{{ $t("table.state") }}：{{
          charger.isCharging ? $t("table.charging") : $t("table.inIdle")
        }}</span
      ><br />
      <span>{{ $t("table.voltage2") }}：{{ charger.voltage }}V</span><br />
      <span>>{{ $t("table.current2") }}：{{ charger.current }}A</span>
    </div>
  </div>
</template>

<script>
export default {
  name: "Charger",
  props: ["chargers"],
  data() {
    return {};
  },
  methods: {}
};
</script>

<style lang="scss" scoped>
.wrap {
  height: 100%;
  width: 100%;
  display: flex;
  flex-wrap: wrap;
  // overflow-y: scroll;
  padding: 0;
  border: 1px solid #ebeef5;
}
.noData {
  height: 100%;
  width: 100%;
  text-align: center;
  line-height: 400px;
  color: #999;
}
.charger {
  width: calc(33.33%);
  height: 174px;
  background-image: url("../../../../assets/statistics/8SCADA.png");
  background-size: 56px 153px;
  background-repeat: no-repeat;
  background-position: 10% 50%; /* 水平25%，垂直75% */
  background-color: #f4eeff;
  // margin: 5px;
  border: 5px solid #fff;
  padding: 10px 8px 8px 100px;
  font-size: 14px;
  line-height: 32px;
}
</style>
