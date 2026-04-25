<template>
  <div>
    <el-row :gutter="10">
      <!-- 左侧 部分 -->
      <el-col :span="4">
        <el-card class="box-card">
          <template #header>
            <div class="card-header">
              <span>{{ $t("table.tools") }}</span>
            </div>
          </template>
          <el-scrollbar id="boxToolbar">
            <el-menu :default-active="setSelect" @select="toolbarSelect">
              <!-- <el-menu-item index="setSelect"> -->
              <!-- <div class="options" @click="toolbarSelect('setSelect')">
                <div>
                  <el-icon size="25">
                    <yidong />
                  </el-icon>
                </div>

                <div class="T-center">
                  {{ $t("table.select") }}
                </div>
              </div> -->

              <div class="options-div">
                <div
                  class="options"
                  :class="{ selected: selectedId === 'setSelect' }"
                  @click="toolbarSelect('setSelect')"
                >
                  <div class="T-center" style="height: 25px; line-height: 25px">
                    <el-icon size="25">
                      <yidong />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.select") }}
                  </div>
                </div>
              </div>

              <!-- </el-menu-item> -->
              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setNodeRect' }"
                  @click="toolbarSelect('setNodeRect')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-a-tubiaokuzhizuo_zhandianjuxing"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.site-rectangle") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setNodeEllipse' }"
                  @click="toolbarSelect('setNodeEllipse')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-a-tubiaokuzhizuo_zhandianyuanxing"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.site-Round") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setEdgeBeeline' }"
                  @click="toolbarSelect('setEdgeBeeline')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-a-tubiaokuzhizuo_luxianzhixian"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.route-straightLine") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setEdgeArc' }"
                  @click="toolbarSelect('setEdgeArc')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-a-tubiaokuzhizuo_luxianyuanhu"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.route-Arc") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setEdgeQuadratic' }"
                  @click="toolbarSelect('setEdgeQuadratic')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-a-tubiaokuzhizuo_luxianbeisaiererci"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    <div>
                      {{ $t("table.route") }}
                    </div>
                    <div>
                      {{ $t("table.Bessel2") }}
                    </div>
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setEdgeBezier' }"
                  @click="toolbarSelect('setEdgeBezier')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-a-tubiaokuzhizuo_luxianbeisaiersanci"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    <div>
                      {{ $t("table.route") }}
                    </div>
                    <div>
                      {{ $t("table.Bessel3") }}
                    </div>
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setZoneRect' }"
                  @click="toolbarSelect('setZoneRect')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-a-tubiaokuzhizuo_quyujuxing"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.region-Rectangular") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setZoneEllipse' }"
                  @click="toolbarSelect('setZoneEllipse')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-a-tubiaokuzhizuo_quyuyuanxing"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.region-Round") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setTagRect' }"
                  @click="toolbarSelect('setTagRect')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-a-tubiaokuzhizuo_biaoqianjuxing"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.Label-Rectangular") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setTagEllipse' }"
                  @click="toolbarSelect('setTagEllipse')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-a-tubiaokuzhizuo_biaoqianyuanxing"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.Label-Round") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setModeView' }"
                  @click="toolbarSelect('setModeView')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span class="iconfont fz-30 icon-tubiaokuzhizuo_chakan" />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.settingMode-Views") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setModeRead' }"
                  @click="toolbarSelect('setModeRead')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_shezhizhidumoshi"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.settingMode-ReadOnlys") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setModeEdit' }"
                  @click="toolbarSelect('setModeEdit')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_shezhimoshibianji"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.settingMode-Edits") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setSwitchInfo' }"
                  @click="toolbarSelect('setSwitchInfo')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_qiehuanxinxi"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.switchInformation") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setSwitchCarRoute' }"
                  @click="toolbarSelect('setSwitchCarRoute')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_qiehuancheliangguiji"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.switchVehiclePath") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setCheckError' }"
                  @click="toolbarSelect('setCheckError')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_jianchacuowu"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.checkForErrors") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setCopyShape' }"
                  @click="toolbarSelect('setCopyShape')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_chuangjianfuben"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.copyObject") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setCloneSize' }"
                  @click="toolbarSelect('setCloneSize')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_fuzhichicun"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.copySize") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setLeftAlign' }"
                  @click="toolbarSelect('setLeftAlign')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_zuoduiqi"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.alignLeft") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setRightAlign' }"
                  @click="toolbarSelect('setRightAlign')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_youduiqi"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.alignRight") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setTopAlign' }"
                  @click="toolbarSelect('setTopAlign')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_dingduiqi"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.topAlignment") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setBottomAlign' }"
                  @click="toolbarSelect('setBottomAlign')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_diduiqi"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.bottomAlignment") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{
                    selected: selectedId === 'setHorizontalDistribution'
                  }"
                  @click="toolbarSelect('setHorizontalDistribution')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_hengxiangfenbu"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.transverseDistribution") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{
                    selected: selectedId === 'setVerticalDistribution'
                  }"
                  @click="toolbarSelect('setVerticalDistribution')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_zongxiangfenbu"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.longitudinalDistribution") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setMoveUp' }"
                  @click="toolbarSelect('setMoveUp')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_shangyiyiceng"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.moveUpOneLayer") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setMoveDown' }"
                  @click="toolbarSelect('setMoveDown')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_xiayiyiceng"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.moveDownOneLayer") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setToTop' }"
                  @click="toolbarSelect('setToTop')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_zhiyudingceng"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.placingOnTop") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setToBottom' }"
                  @click="toolbarSelect('setToBottom')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_zhiyudiceng"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.placedAtTheBottom") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setAddImage' }"
                  @click="toolbarSelect('setAddImage')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_tianjiaditutupian"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.addMapImage") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setDeleteImage' }"
                  @click="toolbarSelect('setDeleteImage')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_shanchuditutupian"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.deleteMapImage") }}
                  </div>
                </div>
              </div>

              <div class="flex">
                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setReset' }"
                  @click="toolbarSelect('setReset')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_zhongzhiditu"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.resetMap") }}
                  </div>
                </div>

                <div
                  class="w50"
                  :class="{ selected: selectedId === 'setDeleteActive' }"
                  @click="toolbarSelect('setDeleteActive')"
                >
                  <div class="T-center">
                    <el-icon>
                      <span
                        class="iconfont fz-30 icon-tubiaokuzhizuo_qiangzhijieshu"
                      />
                    </el-icon>
                  </div>
                  <div class="T-center">
                    {{ $t("table.deleteObject") }}
                  </div>
                </div>
              </div>
            </el-menu>
          </el-scrollbar>
        </el-card>
      </el-col>
      <!-- header 部分 -->
      <el-col :span="16">
        <el-card class="box-card box-map">
          <template #header>
            <div class="card-header fr-flex-space">
              <div>
                <div>
                  <span>{{ $t("menus.Map") }}</span>
                </div>
                <div>
                  编码/名称
                  <el-input
                    v-model="SearchCode"
                    style="width: 240px"
                    placeholder="编码"
                  />
                  <el-button type="primary" @click="SearchMonitoring()">{{
                    $t("table.search")
                  }}</el-button>
                </div>
                <div class="fr-flex">
                  <div
                    class="fr-btn-map-edit"
                    @click="toolbarSelect('setScaleUp')"
                  >
                    <div class="T-center">
                      <i class="iconfont fr-font icon-tubiaokuzhizuo_fangda" />
                    </div>
                    <div class="T-center">放大</div>
                  </div>
                  <div
                    class="fr-btn-map-edit"
                    @click="toolbarSelect('setScaleDown')"
                  >
                    <div class="T-center">
                      <i class="iconfont fr-font icon-tubiaokuzhizuo_suoxiao" />
                    </div>
                    <div class="T-center">缩小</div>
                  </div>
                  <div
                    class="fr-btn-map-edit"
                    @click="toolbarSelect('setTranslateLeft')"
                  >
                    <div class="T-center">
                      <i
                        class="iconfont fr-font icon-tubiaokuzhizuo_zuopianyi"
                      />
                    </div>
                    <div class="T-center">左偏移</div>
                  </div>
                  <div
                    class="fr-btn-map-edit"
                    @click="toolbarSelect('setTranslateRight')"
                  >
                    <div class="T-center">
                      <i
                        class="iconfont fr-font icon-tubiaokuzhizuo_youpianyi"
                      />
                    </div>
                    <div class="T-center">右偏移</div>
                  </div>
                  <div
                    class="fr-btn-map-edit"
                    @click="toolbarSelect('setTranslateTop')"
                  >
                    <div class="T-center">
                      <i
                        class="iconfont fr-font icon-tubiaokuzhizuo_shangpianyi"
                      />
                    </div>
                    <div class="T-center">上偏移</div>
                  </div>
                  <div
                    class="fr-btn-map-edit"
                    @click="toolbarSelect('setTranslateBottom')"
                  >
                    <div class="T-center">
                      <i
                        class="iconfont fr-font icon-tubiaokuzhizuo_xiapianyi"
                      />
                    </div>
                    <div class="T-center">下偏移</div>
                  </div>
                </div>
              </div>
              <div style="display: flex; align-items: end">
                <div
                  class="fr-btn-map-edit-right"
                  @click="toolbarSelect('setOpenData')"
                >
                  <div class="T-center">
                    <i class="iconfont fr-font icon-tubiaokuzhizuo_dakaiditu" />
                  </div>
                  <div class="T-center">{{ $t("table.openMap") }}</div>
                </div>

                <div
                  class="fr-btn-map-edit-right"
                  @click="toolbarSelect('loadSimpleMap')"
                >
                  <div class="T-center">
                    <i
                      class="iconfont fr-font icon-tubiaokuzhizuo_baocunditu"
                    />
                  </div>
                  <!-- 导入simple地图 -->
                  <div class="T-center">
                    {{ $t("table.loadSimpleMap") }}
                  </div>
                </div>

                <div
                  class="fr-btn-map-edit-right"
                  @click="toolbarSelect('loadPointCloudMap')"
                >
                  <div class="T-center">
                    <i
                      class="iconfont fr-font icon-tubiaokuzhizuo_baocunditu"
                    />
                  </div>

                  <div class="T-center">
                    {{ $t("table.loadPointCloudMap") }}
                  </div>
                </div>

                <div
                  class="fr-btn-map-edit-right"
                  @click="toolbarSelect('setScalingAdapt')"
                >
                  <div class="T-center">
                    <i
                      class="iconfont fr-font icon-tubiaokuzhizuo_dituzishiying"
                    />
                  </div>
                  <div class="T-center">
                    {{ $t("table.mapAdaptive") }}
                  </div>
                </div>

                <div
                  class="fr-btn-map-edit-right"
                  @click="toolbarSelect('setScalingNormal')"
                >
                  <div class="T-center">
                    <i class="iconfont fr-font icon-tubiaokuzhizuo_quanping" />
                  </div>

                  <div class="T-center">
                    {{ $t("table.map100") }}
                  </div>
                </div>

                <div
                  class="fr-btn-map-edit-right"
                  @click="toolbarSelect('setSaveData')"
                >
                  <div class="T-center">
                    <i
                      class="iconfont fr-font icon-tubiaokuzhizuo_baocunditu-118"
                    />
                  </div>
                  <div class="T-center">
                    {{ $t("table.saveMap") }}
                  </div>
                </div>

                <div
                  class="fr-btn-map-edit-right"
                  @click="toolbarSelect('setSaveImage')"
                >
                  <div class="T-center">
                    <i
                      class="iconfont fr-font icon-tubiaokuzhizuo_baocunditu-118"
                    />
                  </div>
                  <div class="T-center">
                    {{ $t("保存地图图片") }}
                  </div>
                </div>
              </div>
            </div>
          </template>
          <el-scrollbar id="boxPanel" style="height: 100%">
            <canvas
              id="canvas"
              style="width: 800px; height: 100%; border: 1px solid #ccc"
              tabindex="0"
              >浏览器不支持canvas</canvas
            >
          </el-scrollbar>
        </el-card>
      </el-col>
      <el-col :span="4">
        <el-card class="box-card">
          <template #header>
            <div class="card-header">
              <span
                >{{ $t("table.property") }} {{ tabCurrent.data.title }}</span
              >
            </div>
          </template>
          <el-scrollbar id="boxProperty">
            <el-tabs v-show="tabShow('tabPanel')">
              <el-tab-pane :label="$t('table.foundation')">
                <el-form
                  :model="panelForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item :label="$t('table.width')">
                    <el-input-number
                      v-model="panelForm.map.size.w"
                      :min="100"
                      :max="9999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item :label="$t('table.height')">
                    <el-input-number
                      v-model="panelForm.map.size.h"
                      :min="100"
                      :max="9999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item :label="$t('table.transparency')">
                    <el-input-number
                      v-model="panelForm.map.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item :label="$t('table.borderWidth')">
                    <el-input-number
                      v-model="panelForm.map.lineWidth"
                      :min="0"
                      :max="99999"
                      :step="1"
                    />
                  </el-form-item>

                  <el-form-item :label="$t('table.borderColor')">
                    <el-color-picker v-model="panelForm.map.strokeStyle" />
                  </el-form-item>

                  <el-form-item :label="$t('table.originalSize')">
                    <el-switch
                      v-model="panelForm.image.origin"
                      inline-prompt
                      :active-text="$t('table.yes')"
                      :inactive-text="$t('table.no')"
                    />
                  </el-form-item>

                  <el-form-item :label="$t('table.HandSize')">
                    <el-switch
                      v-model="panelForm.image.manual"
                      inline-prompt
                      :active-text="$t('table.yes')"
                      :inactive-text="$t('table.no')"
                    />
                  </el-form-item>

                  <el-form-item :label="$t('table.HandSize') + '(w)'">
                    <el-input-number
                      v-model="panelForm.image.manualSize.w"
                      :min="10"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item :label="$t('table.HandSize') + '(h)'">
                    <el-input-number
                      v-model="panelForm.image.manualSize.h"
                      :min="10"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="panelForm.image.manualPoint.x"
                      :min="-999999"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="panelForm.image.manualPoint.y"
                      :min="-999999"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="点云透明度">
                    <el-input-number
                      v-model="panelForm.image.globalAlpha"
                      :min="0"
                      :max="1"
                      :step="0.05"
                    />
                  </el-form-item>

                  <el-form-item :label="$t('table.fillColor')">
                    <el-color-picker v-model="panelForm.map.fillStyle" />
                  </el-form-item>
                  <el-form-item :label="$t('table.isVisible')">
                    <el-switch
                      v-model="panelForm.map.visible"
                      inline-prompt
                      :active-text="$t('table.yes')"
                      :inactive-text="$t('table.no')"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
            <el-tabs v-show="tabShow('tabNodeRect')">
              <el-tab-pane label="基础">
                <el-form
                  :model="nodeRectForm.shape"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="nodeRectForm.shape.base.point.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="nodeRectForm.shape.base.point.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(W)">
                    <el-input-number
                      v-model="nodeRectForm.shape.base.size.w"
                      :min="10"
                      :max="9999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(H)">
                    <el-input-number
                      v-model="nodeRectForm.shape.base.size.h"
                      :min="10"
                      :max="9999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="透明度">
                    <el-input-number
                      v-model="nodeRectForm.shape.base.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="边框宽度">
                    <el-input-number
                      v-model="nodeRectForm.shape.base.lineWidth"
                      :min="0"
                      :max="99999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="边框颜色">
                    <el-color-picker
                      v-model="nodeRectForm.shape.base.strokeStyle"
                    />
                  </el-form-item>
                  <el-form-item label="填充颜色">
                    <el-color-picker
                      v-model="nodeRectForm.shape.base.fillStyle"
                    />
                  </el-form-item>
                  <el-form-item label="是否锁定">
                    <el-switch
                      v-model="nodeRectForm.shape.lock.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示">
                    <el-switch
                      v-model="nodeRectForm.shape.base.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示编码">
                    <el-switch
                      v-model="nodeRectForm.shape.code.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="编码文本">
                    <el-input
                      v-model="nodeRectForm.shape.code.text"
                      placeholder="编码"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码字体">
                    <el-input
                      v-model="nodeRectForm.shape.code.font"
                      placeholder="编码字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码颜色">
                    <el-color-picker
                      v-model="nodeRectForm.shape.code.fillStyle"
                    />
                  </el-form-item>
                  <el-form-item label="编码透明">
                    <el-input-number
                      v-model="nodeRectForm.shape.code.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示名称">
                    <el-switch
                      v-model="nodeRectForm.shape.name.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="名称">
                    <el-input
                      v-model="nodeRectForm.shape.name.text"
                      placeholder="名称"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称字体">
                    <el-input
                      v-model="nodeRectForm.shape.name.font"
                      placeholder="名称字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称颜色">
                    <el-color-picker
                      v-model="nodeRectForm.shape.name.fillStyle"
                    />
                  </el-form-item>
                  <el-form-item label="名称透明">
                    <el-input-number
                      v-model="nodeRectForm.shape.name.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="数据">
                <el-form
                  :model="nodeRectForm?.shape?.fields"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="节点描述">
                    <el-input
                      v-model="nodeRectForm.shape.fields.id"
                      placeholder="id"
                      disabled
                    />
                  </el-form-item>
                  <el-form-item label="站点类型">
                    <el-select
                      v-model="nodeRectForm.shape.fields.type"
                      filterable
                      disabled
                      class="!w-[100%]"
                      :placeholder="$t('table.pleaseSelect')"
                    >
                      <el-option label="充电点" value="charge" />
                      <el-option label="默认" value="default" />
                      <el-option label="待命点" value="standby" />
                    </el-select>
                  </el-form-item>

                  <el-form-item label="X坐标">
                    <el-input
                      v-model="nodeRectForm.shape.fields.x"
                      placeholder="id"
                      disabled
                    />
                  </el-form-item>
                  <el-form-item label="Y坐标">
                    <el-input
                      v-model="nodeRectForm.shape.fields.y"
                      placeholder="id"
                      disabled
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <!-- <el-tab-pane label="数据">
                <el-form
                  :model="nodeRectForm.shape.data"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="节点描述">
                    <el-input
                      v-model="nodeRectForm.shape.data.nodeDescription"
                      placeholder="节点描述"
                      maxlength="99999"
                      :rows="3"
                      type="textarea"
                    />
                  </el-form-item>
                  <el-form-item label="是否释放">
                    <el-switch
                      v-model="nodeRectForm.shape.data.released"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="站点位置">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="nodePositionOpen"
                    />
                  </el-form-item>
                  <el-form-item label="动作">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="actionIndexOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane> -->
              <!-- <el-form-item label="设置必空点">
                <el-popover
                  v-if="isSetMustFree"
                  placement="top-start"
                  title="设置方法！"
                  :width="200"
                  trigger="hover"
                  content="点击后按住Ctrl 键选中必空点"
                >
                  <template #reference>
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="extendedMustFree(false)"
                    />
                  </template>
                </el-popover>
                <el-button
                  v-if="!isSetMustFree"
                  type="primary"
                  :icon="Select"
                  @click="extendedMustFree(true)"
                />
                <el-button
                  v-if="!isSetMustFree"
                  type="info"
                  :icon="Close"
                  @click="extendedMustFree(false)"
                />
              </el-form-item> -->
              <el-tab-pane label="其它">
                <el-form
                  :model="nodeRectForm.shape"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="扩展字段">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="extendedFieldOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
            <el-tabs v-show="tabShow('tabNodeEllipse')">
              <el-tab-pane label="基础">
                <el-form
                  :model="nodeEllipseForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="nodeEllipseForm.base.point.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="nodeEllipseForm.base.point.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(W)">
                    <el-input-number
                      v-model="nodeEllipseForm.base.size.w"
                      :min="10"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(H)">
                    <el-input-number
                      v-model="nodeEllipseForm.base.size.h"
                      :min="10"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="透明度">
                    <el-input-number
                      v-model="nodeEllipseForm.base.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="边框宽度">
                    <el-input-number
                      v-model="nodeEllipseForm.base.lineWidth"
                      :min="0"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="边框颜色">
                    <el-color-picker
                      v-model="nodeEllipseForm.base.strokeStyle"
                    />
                  </el-form-item>
                  <el-form-item label="填充颜色">
                    <el-color-picker v-model="nodeEllipseForm.base.fillStyle" />
                  </el-form-item>
                  <el-form-item label="是否锁定">
                    <el-switch
                      v-model="nodeEllipseForm.lock.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示">
                    <el-switch
                      v-model="nodeEllipseForm.base.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示编码">
                    <el-switch
                      v-model="nodeEllipseForm.code.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="编码文本">
                    <el-input
                      v-model="nodeEllipseForm.code.text"
                      placeholder="编码"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码字体">
                    <el-input
                      v-model="nodeEllipseForm.code.font"
                      placeholder="编码字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码颜色">
                    <el-color-picker v-model="nodeEllipseForm.code.fillStyle" />
                  </el-form-item>
                  <el-form-item label="编码透明">
                    <el-input-number
                      v-model="nodeEllipseForm.code.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示名称">
                    <el-switch
                      v-model="nodeEllipseForm.name.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="名称">
                    <el-input
                      v-model="nodeEllipseForm.name.text"
                      placeholder="名称"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称字体">
                    <el-input
                      v-model="nodeEllipseForm.name.font"
                      placeholder="名称字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称颜色">
                    <el-color-picker v-model="nodeEllipseForm.name.fillStyle" />
                  </el-form-item>
                  <el-form-item label="名称透明">
                    <el-input-number
                      v-model="nodeEllipseForm.name.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="数据">
                <el-form
                  :model="nodeEllipseForm?.fields"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="节点描述">
                    <el-input
                      v-model="nodeEllipseForm.fields.id"
                      placeholder="id"
                      disabled
                    />
                  </el-form-item>
                  <el-form-item label="站点类型">
                    <el-select
                      v-model="nodeEllipseForm.fields.type"
                      filterable
                      disabled
                      class="!w-[100%]"
                      :placeholder="$t('table.pleaseSelect')"
                    >
                      <el-option label="充电点" value="charge" />
                      <el-option label="默认" value="default" />
                      <el-option label="待命点" value="standby" />
                    </el-select>
                  </el-form-item>

                  <el-form-item label="X坐标">
                    <el-input
                      v-model="nodeEllipseForm.fields.x"
                      placeholder="id"
                      disabled
                    />
                  </el-form-item>
                  <el-form-item label="Y坐标">
                    <el-input
                      v-model="nodeEllipseForm.fields.y"
                      placeholder="id"
                      disabled
                    />
                  </el-form-item>
                </el-form>
                <!-- <el-form
                  :model="nodeEllipseForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="节点描述">
                    <el-input
                      v-model="nodeEllipseForm.data.nodeDescription"
                      placeholder="节点描述"
                      maxlength="99999"
                      :rows="3"
                      type="textarea"
                    />
                  </el-form-item>
                  <el-form-item label="是否释放">
                    <el-switch
                      v-model="nodeEllipseForm.data.released"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="站点位置">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="nodePositionOpen"
                    />
                  </el-form-item>
                  <el-form-item label="动作">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="actionIndexOpen"
                    />
                  </el-form-item>
                </el-form> -->
              </el-tab-pane>
              <el-tab-pane label="其它">
                <el-form
                  :model="nodeEllipseForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="扩展字段">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="extendedFieldOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
            <el-tabs v-show="tabShow('tabEdgeBeeline')">
              <el-tab-pane label="基础">
                <el-form
                  :model="edgeBeelineForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="edgeBeelineForm.base.point.x"
                      :min="0"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="edgeBeelineForm.base.point.y"
                      :min="0"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(W)">
                    <el-input-number
                      v-model="edgeBeelineForm.base.size.w"
                      :min="10"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(H)">
                    <el-input-number
                      v-model="edgeBeelineForm.base.size.h"
                      :min="10"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="透明度">
                    <el-input-number
                      v-model="edgeBeelineForm.base.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="边框宽度">
                    <el-input-number
                      v-model="edgeBeelineForm.base.lineWidth"
                      :min="0"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="边框颜色">
                    <el-color-picker
                      v-model="edgeBeelineForm.base.strokeStyle"
                    />
                  </el-form-item>
                  <el-form-item label="填充颜色">
                    <el-color-picker v-model="edgeBeelineForm.base.fillStyle" />
                  </el-form-item>
                  <el-form-item label="是否显示">
                    <el-switch
                      v-model="edgeBeelineForm.base.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否锁定">
                    <el-switch
                      v-model="edgeBeelineForm.lock.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否单行线">
                    <el-switch
                      v-model="edgeBeelineForm.base.isOneway"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="路线宽度">
                    <el-input-number
                      v-model="edgeBeelineForm.base.width"
                      :min="0"
                      :max="9999999"
                      :step="1"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="数据">
                <el-form
                  :model="edgeBeelineForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="路线描述">
                    <el-input
                      v-model="edgeBeelineForm.data.edgeDescription"
                      placeholder="路线描述"
                      maxlength="99999"
                      :rows="3"
                      type="textarea"
                    />
                  </el-form-item>
                  <el-form-item label="是否释放">
                    <el-switch
                      v-model="edgeBeelineForm.data.released"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="最大速度">
                    <el-input-number
                      v-model="edgeBeelineForm.data.maxSpeed"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="最大高度">
                    <el-input-number
                      v-model="edgeBeelineForm.data.maxHeight"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="最小高度">
                    <el-input-number
                      v-model="edgeBeelineForm.data.minHeight"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线方向">
                    <el-input-number
                      v-model="edgeBeelineForm.data.orientation"
                      :min="-180"
                      :max="180"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="方向类型">
                    <el-radio-group
                      v-model="edgeBeelineForm.data.orientationType"
                    >
                      <el-radio-button value="TANGENTIAL">切线</el-radio-button>
                      <el-radio-button value="GLOBAL">全局</el-radio-button>
                    </el-radio-group>
                  </el-form-item>
                  <el-form-item label="交叉方向">
                    <el-input
                      v-model="edgeBeelineForm.data.direction"
                      placeholder="交叉方向"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="是否允许旋转">
                    <el-switch
                      v-model="edgeBeelineForm.data.rotationAllowed"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="最大转向">
                    <el-input-number
                      v-model="edgeBeelineForm.data.maxRotationSpeed"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线长度">
                    <el-input-number
                      v-model="edgeBeelineForm.data.length"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线轨迹">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="trackRouteRefOpen"
                    />
                  </el-form-item>
                  <el-form-item label="执行动作">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="actionIndexOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="其它">
                <el-form
                  :model="edgeBeelineForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="扩展字段">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="extendedFieldOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
            <el-tabs v-show="tabShow('tabEdgeArc')">
              <el-tab-pane label="基础">
                <el-form
                  :model="edgeArcForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="edgeArcForm.base.point.x"
                      :min="0"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="edgeArcForm.base.point.y"
                      :min="0"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(W)">
                    <el-input-number
                      v-model="edgeArcForm.base.size.w"
                      :min="10"
                      :max="9999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(H)">
                    <el-input-number
                      v-model="edgeArcForm.base.size.h"
                      :min="10"
                      :max="999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="透明度">
                    <el-input-number
                      v-model="edgeArcForm.base.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="边框宽度">
                    <el-input-number
                      v-model="edgeArcForm.base.lineWidth"
                      :min="0"
                      :max="99999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="边框颜色">
                    <el-color-picker v-model="edgeArcForm.base.strokeStyle" />
                  </el-form-item>
                  <el-form-item label="填充颜色">
                    <el-color-picker v-model="edgeArcForm.base.fillStyle" />
                  </el-form-item>
                  <el-form-item label="是否显示">
                    <el-switch
                      v-model="edgeArcForm.base.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否锁定">
                    <el-switch
                      v-model="edgeArcForm.lock.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否单行线">
                    <el-switch
                      v-model="edgeArcForm.base.isOneway"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="路线宽度">
                    <el-input-number
                      v-model="edgeArcForm.base.width"
                      :min="0"
                      :max="9999999"
                      :step="1"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="数据">
                <el-form
                  :model="edgeArcForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="路线描述">
                    <el-input
                      v-model="edgeArcForm.data.edgeDescription"
                      placeholder="路线描述"
                      maxlength="99999"
                      :rows="3"
                      type="textarea"
                    />
                  </el-form-item>
                  <el-form-item label="是否释放">
                    <el-switch
                      v-model="edgeArcForm.data.released"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="最大速度">
                    <el-input-number
                      v-model="edgeArcForm.data.maxSpeed"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="最大高度">
                    <el-input-number
                      v-model="edgeArcForm.data.maxHeight"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="最小高度">
                    <el-input-number
                      v-model="edgeArcForm.data.minHeight"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线方向">
                    <el-input-number
                      v-model="edgeArcForm.data.orientation"
                      :min="-180"
                      :max="180"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="方向类型">
                    <el-radio-group v-model="edgeArcForm.data.orientationType">
                      <el-radio-button value="TANGENTIAL">切线</el-radio-button>
                      <el-radio-button value="GLOBAL">全局</el-radio-button>
                    </el-radio-group>
                  </el-form-item>
                  <el-form-item label="交叉方向">
                    <el-input
                      v-model="edgeArcForm.data.direction"
                      placeholder="交叉方向"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="是否允许旋转">
                    <el-switch
                      v-model="edgeArcForm.data.rotationAllowed"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="最大转向">
                    <el-input-number
                      v-model="edgeArcForm.data.maxRotationSpeed"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线长度">
                    <el-input-number
                      v-model="edgeArcForm.data.length"
                      :min="0"
                      :max="999999999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线轨迹">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="trackRouteRefOpen"
                    />
                  </el-form-item>
                  <el-form-item label="执行动作">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="actionIndexOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="其它">
                <el-form
                  :model="edgeArcForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="扩展字段">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="extendedFieldOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
            <el-tabs v-show="tabShow('tabEdgeQuadratic')">
              <el-tab-pane label="基础">
                <el-form
                  :model="edgeQuadraticForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="edgeQuadraticForm.base.point.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="edgeQuadraticForm.base.point.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(W)">
                    <el-input-number
                      v-model="edgeQuadraticForm.base.size.w"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(H)">
                    <el-input-number
                      v-model="edgeQuadraticForm.base.size.h"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="透明度">
                    <el-input-number
                      v-model="edgeQuadraticForm.base.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="边框宽度">
                    <el-input-number
                      v-model="edgeQuadraticForm.base.lineWidth"
                      :min="0"
                      :max="99999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="边框颜色">
                    <el-color-picker
                      v-model="edgeQuadraticForm.base.strokeStyle"
                    />
                  </el-form-item>
                  <el-form-item label="填充颜色">
                    <el-color-picker
                      v-model="edgeQuadraticForm.base.fillStyle"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示">
                    <el-switch
                      v-model="edgeQuadraticForm.base.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否锁定">
                    <el-switch
                      v-model="edgeQuadraticForm.lock.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否单行线">
                    <el-switch
                      v-model="edgeQuadraticForm.base.isOneway"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="路线宽度">
                    <el-input-number
                      v-model="edgeQuadraticForm.base.width"
                      :min="0"
                      :max="9999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="控制点1(X)">
                    <el-input-number
                      v-model="edgeQuadraticForm.base.p1.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="控制点1(Y)">
                    <el-input-number
                      v-model="edgeQuadraticForm.base.p1.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="数据">
                <el-form
                  :model="edgeQuadraticForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="路线描述">
                    <el-input
                      v-model="edgeQuadraticForm.data.edgeDescription"
                      placeholder="路线描述"
                      maxlength="99999"
                      :rows="3"
                      type="textarea"
                    />
                  </el-form-item>
                  <el-form-item label="是否释放">
                    <el-switch
                      v-model="edgeQuadraticForm.data.released"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="最大速度">
                    <el-input-number
                      v-model="edgeQuadraticForm.data.maxSpeed"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="最大高度">
                    <el-input-number
                      v-model="edgeQuadraticForm.data.maxHeight"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="最小高度">
                    <el-input-number
                      v-model="edgeQuadraticForm.data.minHeight"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线方向">
                    <el-input-number
                      v-model="edgeQuadraticForm.data.orientation"
                      :min="-180"
                      :max="180"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="方向类型">
                    <el-radio-group
                      v-model="edgeQuadraticForm.data.orientationType"
                    >
                      <el-radio-button value="TANGENTIAL">切线</el-radio-button>
                      <el-radio-button value="GLOBAL">全局</el-radio-button>
                    </el-radio-group>
                  </el-form-item>
                  <el-form-item label="交叉方向">
                    <el-input
                      v-model="edgeQuadraticForm.data.direction"
                      placeholder="交叉方向"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="是否允许旋转">
                    <el-switch
                      v-model="edgeQuadraticForm.data.rotationAllowed"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="最大转向">
                    <el-input-number
                      v-model="edgeQuadraticForm.data.maxRotationSpeed"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线长度">
                    <el-input-number
                      v-model="edgeQuadraticForm.data.length"
                      :min="0"
                      :max="9999999999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线轨迹">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="trackRouteRefOpen"
                    />
                  </el-form-item>
                  <el-form-item label="执行动作">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="actionIndexOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="其它">
                <el-form
                  :model="edgeQuadraticForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="扩展字段">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="extendedFieldOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
            <el-tabs v-show="tabShow('tabEdgeBezier')">
              <el-tab-pane label="基础">
                <el-form
                  :model="edgeBezierForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="edgeBezierForm.base.point.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="edgeBezierForm.base.point.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(W)">
                    <el-input-number
                      v-model="edgeBezierForm.base.size.w"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(H)">
                    <el-input-number
                      v-model="edgeBezierForm.base.size.h"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="透明度">
                    <el-input-number
                      v-model="edgeBezierForm.base.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="边框宽度">
                    <el-input-number
                      v-model="edgeBezierForm.base.lineWidth"
                      :min="0"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="边框颜色">
                    <el-color-picker
                      v-model="edgeBezierForm.base.strokeStyle"
                    />
                  </el-form-item>
                  <el-form-item label="填充颜色">
                    <el-color-picker v-model="edgeBezierForm.base.fillStyle" />
                  </el-form-item>
                  <el-form-item label="是否显示">
                    <el-switch
                      v-model="edgeBezierForm.base.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否锁定">
                    <el-switch
                      v-model="edgeBezierForm.lock.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否单行线">
                    <el-switch
                      v-model="edgeBezierForm.base.isOneway"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="路线宽度">
                    <el-input-number
                      v-model="edgeBezierForm.base.width"
                      :min="0"
                      :max="9999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="控制点1(X)">
                    <el-input-number
                      v-model="edgeBezierForm.base.p1.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="控制点1(Y)">
                    <el-input-number
                      v-model="edgeBezierForm.base.p1.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="控制点2(X)">
                    <el-input-number
                      v-model="edgeBezierForm.base.p2.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="控制点2(Y)">
                    <el-input-number
                      v-model="edgeBezierForm.base.p2.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="数据">
                <el-form
                  :model="edgeBezierForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="路线描述">
                    <el-input
                      v-model="edgeBezierForm.data.edgeDescription"
                      placeholder="路线描述"
                      maxlength="99999"
                      :rows="3"
                      type="textarea"
                    />
                  </el-form-item>
                  <el-form-item label="是否释放">
                    <el-switch
                      v-model="edgeBezierForm.data.released"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="最大速度">
                    <el-input-number
                      v-model="edgeBezierForm.data.maxSpeed"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="最大高度">
                    <el-input-number
                      v-model="edgeBezierForm.data.maxHeight"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="最小高度">
                    <el-input-number
                      v-model="edgeBezierForm.data.minHeight"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线方向">
                    <el-input-number
                      v-model="edgeBezierForm.data.orientation"
                      :min="-180"
                      :max="180"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="方向类型">
                    <el-radio-group
                      v-model="edgeBezierForm.data.orientationType"
                    >
                      <el-radio-button value="TANGENTIAL">切线</el-radio-button>
                      <el-radio-button value="GLOBAL">全局</el-radio-button>
                    </el-radio-group>
                  </el-form-item>
                  <el-form-item label="交叉方向">
                    <el-input
                      v-model="edgeBezierForm.data.direction"
                      placeholder="交叉方向"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="是否允许旋转">
                    <el-switch
                      v-model="edgeBezierForm.data.rotationAllowed"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="最大转向">
                    <el-input-number
                      v-model="edgeBezierForm.data.maxRotationSpeed"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线长度">
                    <el-input-number
                      v-model="edgeBezierForm.data.length"
                      :min="0"
                      :max="99999"
                      :step="1"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="路线轨迹">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="trackRouteRefOpen"
                    />
                  </el-form-item>
                  <el-form-item label="执行动作">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="actionIndexOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="其它">
                <el-form
                  :model="edgeBezierForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="扩展字段">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="extendedFieldOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
            <el-tabs v-show="tabShow('tabZoneRect')">
              <el-tab-pane label="基础">
                <el-form
                  :model="zoneRectForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="zoneRectForm.base.point.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="zoneRectForm.base.point.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(W)">
                    <el-input-number
                      v-model="zoneRectForm.base.size.w"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(H)">
                    <el-input-number
                      v-model="zoneRectForm.base.size.h"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="透明度">
                    <el-input-number
                      v-model="zoneRectForm.base.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="边框宽度">
                    <el-input-number
                      v-model="zoneRectForm.base.lineWidth"
                      :min="0"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="边框颜色">
                    <el-color-picker v-model="zoneRectForm.base.strokeStyle" />
                  </el-form-item>
                  <el-form-item label="填充颜色">
                    <el-color-picker v-model="zoneRectForm.base.fillStyle" />
                  </el-form-item>
                  <el-form-item label="是否锁定">
                    <el-switch
                      v-model="zoneRectForm.lock.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示">
                    <el-switch
                      v-model="zoneRectForm.base.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示编码">
                    <el-switch
                      v-model="zoneRectForm.code.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="编码">
                    <el-input
                      v-model="zoneRectForm.code.text"
                      placeholder="编码"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码字体">
                    <el-input
                      v-model="zoneRectForm.code.font"
                      placeholder="编码字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码颜色">
                    <el-color-picker v-model="zoneRectForm.code.fillStyle" />
                  </el-form-item>
                  <el-form-item label="编码透明">
                    <el-input-number
                      v-model="zoneRectForm.code.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示名称">
                    <el-switch
                      v-model="zoneRectForm.name.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="名称">
                    <el-input
                      v-model="zoneRectForm.name.text"
                      placeholder="名称"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称字体">
                    <el-input
                      v-model="zoneRectForm.name.font"
                      placeholder="名称字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称颜色">
                    <el-color-picker v-model="zoneRectForm.name.fillStyle" />
                  </el-form-item>
                  <el-form-item label="名称透明">
                    <el-input-number
                      v-model="zoneRectForm.name.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="显示图片">
                    <el-switch
                      v-model="zoneRectForm.image.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="图片透明">
                    <el-input-number
                      v-model="zoneRectForm.image.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="数据">
                <el-form
                  :model="zoneRectForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="最多车辆">
                    <el-input-number
                      v-model="zoneRectForm.data.maxCar"
                      :min="0"
                      :max="99999"
                      :step="1"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="其它">
                <el-form
                  :model="zoneRectForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="扩展字段">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="extendedFieldOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
            <el-tabs v-show="tabShow('tabZoneEllipse')">
              <el-tab-pane label="基础">
                <el-form
                  :model="zoneEllipseForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="zoneEllipseForm.base.point.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="zoneEllipseForm.base.point.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(W)">
                    <el-input-number
                      v-model="zoneEllipseForm.base.size.w"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(H)">
                    <el-input-number
                      v-model="zoneEllipseForm.base.size.h"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="透明度">
                    <el-input-number
                      v-model="zoneEllipseForm.base.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="边框宽度">
                    <el-input-number
                      v-model="zoneEllipseForm.base.lineWidth"
                      :min="0"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="边框颜色">
                    <el-color-picker
                      v-model="zoneEllipseForm.base.strokeStyle"
                    />
                  </el-form-item>
                  <el-form-item label="填充颜色">
                    <el-color-picker v-model="zoneEllipseForm.base.fillStyle" />
                  </el-form-item>
                  <el-form-item label="是否显示">
                    <el-switch
                      v-model="zoneEllipseForm.base.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否锁定">
                    <el-switch
                      v-model="zoneEllipseForm.lock.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示编码">
                    <el-switch
                      v-model="zoneEllipseForm.code.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="编码文本">
                    <el-input
                      v-model="zoneEllipseForm.code.text"
                      placeholder="编码"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码字体">
                    <el-input
                      v-model="zoneEllipseForm.code.font"
                      placeholder="编码字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码颜色">
                    <el-color-picker v-model="zoneEllipseForm.code.fillStyle" />
                  </el-form-item>
                  <el-form-item label="编码透明">
                    <el-input-number
                      v-model="zoneEllipseForm.code.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示名称">
                    <el-switch
                      v-model="zoneEllipseForm.name.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="名称">
                    <el-input
                      v-model="zoneEllipseForm.name.text"
                      placeholder="名称"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称字体">
                    <el-input
                      v-model="zoneEllipseForm.name.font"
                      placeholder="名称字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称颜色">
                    <el-color-picker v-model="zoneEllipseForm.name.fillStyle" />
                  </el-form-item>
                  <el-form-item label="名称透明">
                    <el-input-number
                      v-model="zoneEllipseForm.name.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="数据">
                <el-form
                  :model="zoneEllipseForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="最多车辆">
                    <el-input-number
                      v-model="zoneEllipseForm.data.maxCar"
                      :min="0"
                      :max="99999"
                      :step="1"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
              <el-tab-pane label="其它">
                <el-form
                  :model="zoneEllipseForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="扩展字段">
                    <el-button
                      type="primary"
                      :icon="Edit"
                      @click="extendedFieldOpen"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
            <el-tabs v-show="tabShow('tabTagRect')">
              <el-tab-pane label="基础">
                <el-form
                  :model="tagRectForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="tagRectForm.base.point.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="tagRectForm.base.point.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(W)">
                    <el-input-number
                      v-model="tagRectForm.base.size.w"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(H)">
                    <el-input-number
                      v-model="tagRectForm.base.size.h"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="透明度">
                    <el-input-number
                      v-model="tagRectForm.base.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="边框宽度">
                    <el-input-number
                      v-model="tagRectForm.base.lineWidth"
                      :min="0"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="边框颜色">
                    <el-color-picker v-model="tagRectForm.base.strokeStyle" />
                  </el-form-item>
                  <el-form-item label="填充颜色">
                    <el-color-picker v-model="tagRectForm.base.fillStyle" />
                  </el-form-item>
                  <el-form-item label="是否显示">
                    <el-switch
                      v-model="tagRectForm.base.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否锁定">
                    <el-switch
                      v-model="tagRectForm.lock.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示编码">
                    <el-switch
                      v-model="tagRectForm.code.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="编码文本">
                    <el-input
                      v-model="tagRectForm.code.text"
                      placeholder="编码"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码字体">
                    <el-input
                      v-model="tagRectForm.code.font"
                      placeholder="编码字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码颜色">
                    <el-color-picker v-model="tagRectForm.code.fillStyle" />
                  </el-form-item>
                  <el-form-item label="编码透明">
                    <el-input-number
                      v-model="tagRectForm.code.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示名称">
                    <el-switch
                      v-model="tagRectForm.name.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="名称">
                    <el-input
                      v-model="tagRectForm.name.text"
                      placeholder="名称"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称字体">
                    <el-input
                      v-model="tagRectForm.name.font"
                      placeholder="名称字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称颜色">
                    <el-color-picker v-model="tagRectForm.name.fillStyle" />
                  </el-form-item>
                  <el-form-item label="名称透明">
                    <el-input-number
                      v-model="tagRectForm.name.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
            <el-tabs v-show="tabShow('tabTagEllipse')">
              <el-tab-pane label="基础">
                <el-form
                  :model="tagEllipseForm"
                  :size="size"
                  :label-position="labelPosition"
                  label-width="100px"
                >
                  <el-form-item label="坐标(X)">
                    <el-input-number
                      v-model="tagEllipseForm.base.point.x"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="坐标(Y)">
                    <el-input-number
                      v-model="tagEllipseForm.base.point.y"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(W)">
                    <el-input-number
                      v-model="tagEllipseForm.base.size.w"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="尺寸(H)">
                    <el-input-number
                      v-model="tagEllipseForm.base.size.h"
                      :min="10"
                      :max="9999999999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="透明度">
                    <el-input-number
                      v-model="tagEllipseForm.base.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="边框宽度">
                    <el-input-number
                      v-model="tagEllipseForm.base.lineWidth"
                      :min="0"
                      :max="99999"
                      :step="1"
                    />
                  </el-form-item>
                  <el-form-item label="边框颜色">
                    <el-color-picker
                      v-model="tagEllipseForm.base.strokeStyle"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示">
                    <el-switch
                      v-model="tagEllipseForm.base.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否锁定">
                    <el-switch
                      v-model="tagEllipseForm.lock.enable"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示编码">
                    <el-switch
                      v-model="tagEllipseForm.code.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="编码">
                    <el-input
                      v-model="tagEllipseForm.code.text"
                      placeholder="编码"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码字体">
                    <el-input
                      v-model="tagEllipseForm.code.font"
                      placeholder="编码字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="编码颜色">
                    <el-color-picker v-model="tagEllipseForm.code.fillStyle" />
                  </el-form-item>
                  <el-form-item label="编码透明">
                    <el-input-number
                      v-model="tagEllipseForm.code.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                  <el-form-item label="是否显示名称">
                    <el-switch
                      v-model="tagEllipseForm.name.visible"
                      inline-prompt
                      active-text="是"
                      inactive-text="否"
                    />
                  </el-form-item>
                  <el-form-item label="名称">
                    <el-input
                      v-model="tagEllipseForm.name.text"
                      placeholder="名称"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称字体">
                    <el-input
                      v-model="tagEllipseForm.name.font"
                      placeholder="名称字体"
                      maxlength="50"
                    />
                  </el-form-item>
                  <el-form-item label="名称颜色">
                    <el-color-picker v-model="tagEllipseForm.name.fillStyle" />
                  </el-form-item>
                  <el-form-item label="名称透明">
                    <el-input-number
                      v-model="tagEllipseForm.name.globalAlpha"
                      :min="0.0"
                      :max="1.0"
                      :step="0.01"
                      :precision="2"
                    />
                  </el-form-item>
                </el-form>
              </el-tab-pane>
            </el-tabs>
          </el-scrollbar>
        </el-card>
      </el-col>
    </el-row>
    <NodePosition ref="nodePositionRef" />
    <ActionIndex ref="actionIndexRef" />
    <ExtendedField ref="extendedFieldRef" />
    <TrackRoute ref="trackRouteRef" />

    <div v-if="showImg" class="fixd" style="right: 390px">
      <img src="../../../assets/mapEdit.png" alt="" />
    </div>

    <div class="fixd" @click="showImg = !showImg">
      <mapedit />
    </div>
  </div>
  <el-dialog v-model="showMapSelection" title="选择文件" width="30%">
    <span>请选择要合并的地图文件：</span>
    <!-- <input type="file" accept=".json" @change="handleFileChange" /> -->
    <div @click="triggerFileInput">
      <!-- 自定义按钮 -->
      <el-button type="primary">选择文件</el-button>
      <!-- 隐藏的文件输入框 -->
      <input
        ref="fileInput"
        type="file"
        accept=".json"
        style="display: none"
        @change="handleFileChange"
      />
    </div>
    <p v-if="selectedFileName">选择的文件: {{ selectedFileName }}</p>
    <template v-slot:footer>
      <span class="dialog-footer">
        <el-button
          @click="
            showMapSelection = false;
            selectedFileName = '';
          "
          >取消</el-button
        >
        <el-button type="primary" @click="confirmMerge">确认合并</el-button>
      </span>
    </template>
  </el-dialog>
  <el-dialog
    v-model="showMapDialog"
    title="是否合并迷榖和Simple的地图？"
    width="500"
  >
    <div>
      <div>
        是否显示库位：
        <el-switch
          v-model="LoadPibraryPosition"
          class="ml-2"
          style="--el-switch-on-color: #13ce66; --el-switch-off-color: #ff4949"
        />
      </div>
      <el-button
        type="primary"
        @click="
          showMapSelection = true;
          showMapDialog = false;
        "
        >合并本地地图1</el-button
      >
      <el-button type="primary" @click="confirmMergeAxios()"
        >合并服务器地图</el-button
      >
      <el-button type="primary" @click="loadSimpleMapFn()"
        >仅加载Simple地图</el-button
      >
    </div>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="showMapDialog = false">Cancel</el-button>
      </div>
    </template>
  </el-dialog>
</template>
<script setup>
import {
  onMounted,
  h,
  reactive,
  ref,
  toRefs,
  watch,
  computed,
  watchEffect
} from "vue";
import { Edit, Select, Close } from "@element-plus/icons-vue";
import NodePosition from "./component/NodePosition.vue";
import ActionIndex from "./component/ActionIndex.vue";
import ExtendedField from "./component/ExtendedField.vue";
import TrackRoute from "./component/TrackRoute.vue";
import { message } from "@/utils/message";
import { IconifyIconOffline } from "@/components/ReIcon";
import CaretRight from "@iconify-icons/ep/caret-right";
import yidong from "@/assets/svg/yidong.svg?component";
import { Save } from "@/api/map/edit";
import { mapTransform } from "@/utils/map/transform.js";
import mapedit from "@/assets/svg/mapedit.svg?component";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { ElMessageBox, ElButton } from "element-plus";
import {
  GetMap,
  GetSimpleMap,
  GetStorageListToSelect,
  GetPointCloudMap
} from "@/api/monitor/runtime";
import { v4 as uuidv4 } from "uuid";
import { stringify, parse } from "flatted";

// import SimpleMap from "./simplemap.json";
defineOptions({
  name: "Map-Edit"
});
import { ActivationCodeKey } from "@/utils/auth";
const activationCode = localStorage.getItem(ActivationCodeKey);
const setSelect = ref("setSelect");

let selectedId = ref("");
let fileInput = ref(null);

let SearchCode = ref("");
let showMapSelection = ref(false);
let showMapDialog = ref(false);
let selectedFileName = ref(""); // 保存用户选择文件的名称

let LoadPibraryPosition = ref(true);
watchEffect(() => {
  localStorage.setItem("LoadPibraryPosition", LoadPibraryPosition.value);
});

const SearchMonitoring = () => {
  // const search = document.getElementById("textSearch").value;
  if (!SearchCode.value) {
    message("请输入编码/名称！", { type: "warning" });
    return;
  }
  const shape = page.panel.allShapes.find(
    e => e.code.text === SearchCode.value || e.name.text === SearchCode.value
  );
  console.log("SearchCode is", SearchCode.value, "\n", page.panel);
  if (!shape) {
    message("未搜索到对象！", { type: "warning" });
    return;
  }
  page.panel.setViewAndActive(shape);
};

const showImg = ref(false);

// 是否选择了多个属性
let isMultiple = ref(false);

let isSetMustFree = ref(true);
const nodePositionRef = ref(null);
const nodePositionOpen = () => {
  const shape = page.panel.activeShapes[0];
  nodePositionRef.value.open(shape);
};
const actionIndexRef = ref(null);
const actionIndexOpen = () => {
  const shape = page.panel.activeShapes[0];
  actionIndexRef.value.open(shape);
};
const extendedFieldRef = ref(null);
const extendedFieldOpen = () => {
  const shape = page.panel.activeShapes[0];
  extendedFieldRef.value.open(shape);
};
const extendedMustFree = (set = false) => {
  const shape = page.panel.activeShapes[0];
  isSetMustFree.value = !isSetMustFree.value;
  if (set) {
    // 获取activeShapes中的id值集合
    const ids = page.panel.activeShapes.map(activeShape => activeShape.id);
    // 查找是否已经存在key为MustFree的对象
    let mustFreeObj = shape.extends.find(ext => ext.key === "MustFree");
    if (mustFreeObj) {
      // 如果存在，替换value值
      mustFreeObj.value = ids;
    } else {
      // 如果不存在，创建一个新的对象并添加到extends数组中
      mustFreeObj = { key: "MustFree", value: ids };
      shape.extends.push(mustFreeObj);
    }
  }
  console.log("page.panel.activeShapes is", page.panel.activeShapes);
  // extendedFieldRef.value.open(shape);
  // 设置必空点的逻辑
  // 点击之后  必空点设置按钮转化为 确定 与 取消；
  // 将地图设置为选中状态，获取选中的站点code数据；
};
const trackRouteRef = ref(null);
const trackRouteRefOpen = () => {
  const shape = page.panel.activeShapes[0];
  trackRouteRef.value.open(shape);
};
// 控制赋值的时候不改变
let isWatchChange = ref(false);

onMounted(async () => {
  console.log("window.MapInfo.transform is", window.MapInfo);
  await page.init(); //初始化Canvas

  // OpenMap();
  // createWatcher(nodeRectForm.shape.base, cloneNodeRectForm, 'NodeRect', isWatchChange, findDifferences);
  // createWatcher(nodeEllipseForm.base, cloneNodeEllipseForm, 'NodeEllipse', isWatchChange, findDifferences);
});

const OpenMap = async () => {
  try {
    let { data } = await MapOpenAt({ MapName: "Map" });
    if (data.code === 200) {
      let MapInfo = JSON.parse(data.data.mapContent);
      // console.log("MapInfo", MapInfo);
      page.panel.setData(MapInfo);
    }
  } catch {}
};

const toolbarSelect = index => {
  console.log("index", index);

  selectedId.value = index;

  setSelect.value = index;
  page.setCommand(index);
};

const tabShow = tabId => {
  return tabId === tabCurrent.data.id;
};

const size = ref("small");
const labelPosition = ref("left");
const tabCurrent = reactive({
  data: {
    id: "tabPanel",
    title: "：地图"
  }
});

const panelForm = reactive({
  map: {
    size: { w: 0, h: 0 },
    globalAlpha: 0.0,
    lineWidth: 0,
    strokeStyle: "#FFFFFF",
    fillStyle: "#FFFFFF",
    visible: true
  },
  image: {
    origin: true,
    manual: false,
    manualSize: { w: 0, h: 0 },
    manualPoint: { x: 0, y: 0 },
    globalAlpha: 1,
    offset: false,
    offsetPoint: {
      x: 0,
      y: 0
    }
  }
});

const nodeRectForm = reactive({
  shape: {
    base: {
      enable: true,
      visible: true,
      point: {
        x: 10,
        y: 10
      },
      size: {
        w: 40,
        h: 40
      },
      globalAlpha: 0,
      font: "10px Arial",
      strokeStyle: "#000000",
      fillStyle: "#000000",
      pointOffset: {
        x: 0,
        y: 0.8333333333333334
      },
      text: ""
    },
    name: {
      enable: true,
      visible: true,
      point: {
        x: 0,
        y: 0
      },
      globalAlpha: 0,
      font: "10px Arial",
      strokeStyle: "#000000",
      fillStyle: "#000000",
      pointOffset: {
        x: 0,
        y: 0
      },
      text: ""
    },
    code: {
      enable: true,
      visible: true,
      point: {
        x: 0,
        y: 0
      },
      globalAlpha: 0,
      font: "10px Arial",
      strokeStyle: "#000000",
      fillStyle: "#000000",
      pointOffset: {
        x: 0,
        y: 0
      },
      text: ""
    },
    lock: { enable: false },
    data: {
      nodeDescription: "",
      released: false
    },
    fields: {
      type: "",
      x: "",
      y: "",
      id: ""
    }
  }
});

const nodeEllipseForm = reactive({
  lock: {
    enable: false
  },
  base: {
    enable: true,
    visible: true,
    point: {
      x: 10,
      y: 10
    },
    size: {
      w: 40,
      h: 40
    },
    globalAlpha: 0,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0.8333333333333334
    },
    text: ""
  },
  name: {
    enable: true,
    visible: true,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 0,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  code: {
    enable: true,
    visible: true,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 0,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  data: {
    nodeDescription: "",
    released: false
  },
  fields: {
    type: "",
    x: "",
    y: "",
    id: ""
  }
});

const edgeBeelineForm = reactive({
  base: {
    enable: true,
    visible: true,
    point: {
      x: 10,
      y: 10
    },
    size: {
      w: 40,
      h: 40
    },
    globalAlpha: 1,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0.8333333333333334
    },
    text: "",
    length: 0,
    lineWidth: 1,
    width: 0,
    isOneway: false
  },
  name: {
    enable: false,
    visible: false,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 1,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  code: {
    enable: true,
    visible: true,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 1,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  data: {
    sequenceId: 0,
    edgeDescription: "",
    released: true,
    startNodeId: "",
    endNodeId: "",
    maxSpeed: 0,
    maxHeight: 0,
    minHeight: 0,
    orientation: 0,
    orientationType: "TANGENTIAL",
    direction: "",
    rotationAllowed: true,
    maxRotationSpeed: 0,
    trajectory: {
      degree: 0,
      knotVector: [0],
      controlPoints: []
    },
    actions: []
  },
  lock: {
    enable: false
  }
});

const edgeArcForm = reactive({
  base: {
    enable: true,
    visible: true,
    point: {
      x: 10,
      y: 10
    },
    size: {
      w: 40,
      h: 40
    },
    globalAlpha: 0,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0.8333333333333334
    },
    text: "",
    length: 0,
    lineWidth: 1,
    width: 0,
    isOneway: false
  },
  name: {
    enable: false,
    visible: false,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 1,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  code: {
    enable: true,
    visible: true,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 0,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  data: {
    sequenceId: 0,
    edgeDescription: "",
    released: true,
    startNodeId: "",
    endNodeId: "",
    maxSpeed: 0,
    maxHeight: 0,
    minHeight: 0,
    orientation: 0,
    orientationType: "TANGENTIAL",
    direction: "",
    rotationAllowed: true,
    maxRotationSpeed: 0,
    length: 0,
    trajectory: {
      degree: 0,
      knotVector: [0],
      controlPoints: []
    },
    actions: []
  },
  lock: {
    enable: false
  }
});

const edgeQuadraticForm = reactive({
  base: {
    enable: true,
    visible: true,
    point: {
      x: 10,
      y: 10
    },
    size: {
      w: 10,
      h: 10
    },
    globalAlpha: 1,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0.8333333333333334
    },
    text: "",
    p1: { x: 10, y: 10 },
    length: 0,
    lineWidth: 1,
    width: 0,
    isOneway: false
  },
  name: {
    enable: false,
    visible: false,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 1,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  code: {
    enable: true,
    visible: true,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 1,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  data: {
    edgeId: "",
    sequenceId: 0,
    edgeDescription: "",
    released: true,
    startNodeId: "",
    endNodeId: "",
    maxSpeed: 0,
    maxHeight: 0,
    minHeight: 0,
    orientation: 0,
    orientationType: "TANGENTIAL",
    direction: "",
    rotationAllowed: true,
    maxRotationSpeed: 0,
    length: 0,
    trajectory: {
      degree: 0,
      knotVector: [0],
      controlPoints: []
    },
    actions: []
  },
  lock: {
    enable: false
  }
});

const edgeBezierForm = reactive({
  base: {
    enable: true,
    visible: true,
    point: {
      x: 10,
      y: 10
    },
    size: {
      w: 10,
      h: 10
    },
    globalAlpha: 1,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0.8333333333333334
    },
    text: "",
    p1: { x: 0, y: 0 },
    p2: { x: 0, y: 0 },
    length: 0,
    lineWidth: 1,
    width: 0,
    isOneway: false
  },
  name: {
    enable: false,
    visible: false,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 1,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  code: {
    enable: true,
    visible: true,
    point: {
      x: 0,
      y: 0
    },
    globalAlpha: 0,
    font: "10px Arial",
    strokeStyle: "#000000",
    fillStyle: "#000000",
    pointOffset: {
      x: 0,
      y: 0
    },
    text: ""
  },
  data: {
    edgeId: "",
    sequenceId: 0,
    edgeDescription: "",
    released: true,
    startNodeId: "",
    endNodeId: "",
    maxSpeed: 0,
    maxHeight: 0,
    minHeight: 0,
    orientation: 0,
    orientationType: "TANGENTIAL",
    direction: "",
    rotationAllowed: true,
    maxRotationSpeed: 0,
    length: 0,
    trajectory: {
      degree: 0,
      knotVector: [0],
      controlPoints: []
    },
    actions: []
  },
  lock: {
    enable: false
  }
});

const zoneRectForm = reactive({
  base: {
    visible: true,
    point: {
      x: 176.171875,
      y: 228
    },
    size: {
      w: 200,
      h: 200
    },
    globalAlpha: 0.2,
    lineWidth: 1,
    lineDash: [0],
    strokeStyle: "#000000",
    fillStyle: "#FFFFFF",
    center: {
      x: 276.171875,
      y: 328
    }
  },
  code: {
    visible: true,
    globalAlpha: 1,
    font: "10px Arial",
    fillStyle: "#000000",
    text: "1"
  },
  name: {
    visible: true,
    globalAlpha: 1,
    font: "10px Arial",
    fillStyle: "#000000",
    text: "区域"
  },
  lock: {
    enable: false
  },
  data: {
    maxCar: 1,
    nodeIds: []
  },
  image: {
    visible: false,
    globalAlpha: 1,
    src: ""
  },
  extends: []
});

const zoneEllipseForm = reactive({
  base: {
    visible: true,
    point: {
      x: 566.171875,
      y: 264
    },
    size: {
      w: 200,
      h: 200
    },
    globalAlpha: 0.2,
    lineWidth: 1,
    lineDash: [0],
    strokeStyle: "#000000",
    fillStyle: "#FFFFFF",
    center: {
      x: 666.171875,
      y: 364
    }
  },
  code: {
    visible: true,
    globalAlpha: 1,
    font: "10px Arial",
    fillStyle: "#000000",
    text: "2"
  },
  name: {
    visible: true,
    globalAlpha: 1,
    font: "10px Arial",
    fillStyle: "#000000",
    text: "区域"
  },
  lock: {
    enable: false
  },
  image: {
    visible: false,
    globalAlpha: 1,
    src: ""
  },
  data: {
    maxCar: 1,
    nodeIds: []
  },
  extends: []
});

const tagRectForm = reactive({
  base: {
    visible: true,
    point: {
      x: 174.171875,
      y: 560
    },
    size: {
      w: 50,
      h: 50
    },
    globalAlpha: 1,
    lineWidth: 1,
    lineDash: [0],
    strokeStyle: "#000000",
    fillStyle: "#FFFFFF",
    center: {
      x: 199.171875,
      y: 585
    }
  },
  code: {
    visible: true,
    globalAlpha: 1,
    font: "10px Arial",
    fillStyle: "#000000",
    text: "1"
  },
  name: {
    visible: true,
    globalAlpha: 1,
    font: "10px Arial",
    fillStyle: "#000000",
    text: "标签"
  },
  image: {
    visible: false,
    globalAlpha: 1,
    src: ""
  },
  lock: {
    enable: false
  },
  extends: []
});

const tagEllipseForm = reactive({
  base: {
    visible: true,
    point: {
      x: 617.171875,
      y: 591
    },
    size: {
      w: 50,
      h: 50
    },
    globalAlpha: 1,
    lineWidth: 1,
    lineDash: [0],
    strokeStyle: "#000000",
    fillStyle: "#FFFFFF",
    center: {
      x: 642.171875,
      y: 616
    }
  },
  code: {
    visible: true,
    globalAlpha: 1,
    font: "10px Arial",
    fillStyle: "#000000",
    text: "2"
  },
  name: {
    visible: true,
    globalAlpha: 1,
    font: "10px Arial",
    fillStyle: "#000000",
    text: "标签"
  },
  image: {
    visible: false,
    globalAlpha: 1,
    src: ""
  },
  lock: {
    enable: false
  },
  extends: []
});
let cloneNodeRectForm = stringify(nodeRectForm.shape);
let cloneNodeEllipseForm = stringify(nodeEllipseForm);
let cloneEdgeBeelineForm = stringify(edgeBeelineForm);
let cloneEdgeArcForm = stringify(edgeArcForm);
let cloneEdgeQuadraticForm = stringify(edgeQuadraticForm);
let cloneEdgeBezierForm = stringify(edgeBezierForm);
let cloneZoneRectForm = stringify(zoneRectForm);
let cloneZoneEllipseForm = stringify(zoneEllipseForm);
let cloneTagRectForm = stringify(tagRectForm);
let cloneTagEllipseForm = stringify(tagEllipseForm);

let paddingList = [];
let sideWH = [];
let coordinates = {};
let nodeList = [];

const page = {
  timerId: null,
  canvas: {},
  panel: {},
  init: async () => {
    // page.box = document.getElementById("boxPanel");
    page.canvas = document.getElementById("canvas");
    page.panel = await new Panel(page.canvas, {
      map: { base: { size: { w: 1300, h: 900 } } },
      info: { visible: true },
      zoom: { enable: false }, //关闭画板缩放渲染
      mode: "Edit",
      license: { activationCode: activationCode }
    }).init();
    // page.panel = await new Panel(page.box, page.canvas, {
    //   map: { base: { size: { w: 1300, h: 900 } } },
    //   mode: "Edit",
    //   part: false,
    //   license: { activationCode }
    // }).init();
    page.bind();
  },
  bind: () => {
    page.panel.panelPointerDownAction = event =>
      page.setPointerDownAction(event);
    page.panel.panelActiveAction = panel => page.setPanelForm(panel);
    page.panel.nodeRectActiveEnterAction = (shape, shapes) =>
      page.setNodeRectForm(shape, shapes);
    page.panel.nodeEllipseActiveEnterAction = (shape, shapes) =>
      page.setNodeEllipseForm(shape, shapes);
    page.panel.edgeBeelineActiveEnterAction = (shape, shapes) =>
      page.setEdgeBeelineForm(shape);
    page.panel.edgeArcActiveEnterAction = (shape, shapes) =>
      page.setEdgeArcForm(shape);
    page.panel.edgeQuadraticActiveEnterAction = (shape, shapes) =>
      page.setEdgeQuadraticForm(shape);
    page.panel.edgeBezierActiveEnterAction = (shape, shapes) =>
      page.setEdgeBezierForm(shape);
    page.panel.tagRectActiveEnterAction = (shape, shapes) =>
      page.setTagRectForm(shape);
    page.panel.tagEllipseActiveEnterAction = (shape, shapes) =>
      page.setTagEllipseForm(shape);
    page.panel.zoneRectActiveEnterAction = (shape, shapes) =>
      page.setZoneRectForm(shape);
    page.panel.zoneEllipseActiveEnterAction = (shape, shapes) =>
      page.setZoneEllipseForm(shape);
    page.panel.panelMouseUpAction = e => page.setSelect();
    window.addEventListener("resize", event => page.panel.setResize());
    page.panel.loadSimpleMap = e => page.loadSimpleMap();
    page.panel.loadPointCloudMap = e => page.loadPointCloudMap(); //新增自定义方法
  },

  setCommand: async command => {
    console.log("command", command);
    if (command === "setSaveData") {
      page.panel[command]() && page.saveData(page.panel.getData());
    } else if (command === "loadSimpleMap") {
      // 加载Simple地图 并打开
      await page.panel["setReset"]();
      page.panel[command]();
      // page.panel[command]() && page.saveData(page.panel.getData());
    } else if (command === "loadPointCloudMap") {
      // page.panel["setInit"]();
      page.panel[command]();
    } else {
      await page.panel[command]();
    }
  },
  setPointerDownAction: e => {
    if (e.button === 2) {
      // console.log("右键");
      setSelect.value = "setSelect";
      page.setTabActive("setSelect");
      page.panel.setSelect();
    }
  },
  setPanelForm: panel => {
    page.setTabActive("tabPanel", "：地图");
    panelForm.map = toRefs(reactive(panel.map.base));
    panelForm.image = toRefs(reactive(panel.map.image));
  },
  setNodeRectForm: (shape, shapes) => {
    console.log("shape", shapes);
    nodeList = shapes;
    page.setTabActive("tabNodeRect", "：站点(矩形)");
    isWatchChange.value = false;
    nodeRectForm.shape.base = reactive(shape.base);
    nodeRectForm.shape.code = reactive(shape.code);
    nodeRectForm.shape.name = reactive(shape.name);
    nodeRectForm.shape.name = reactive(shape.name);
    nodeRectForm.shape.lock.enable = shape.lock.enable;
    nodeRectForm.shape.data.nodeDescription = shape.data.nodeDescription;
    nodeRectForm.shape.data.released = shape.data.released;
    if (shape?.fields) {
      nodeRectForm.shape.fields = shape?.fields;
    }
    cloneNodeRectForm = stringify(nodeRectForm.shape);
    // cloneNodeRectForm = JSON.stringify(nodeRectForm.shape.base);
    setTimeout(() => {
      isWatchChange.value = true;
    });
  },
  setNodeEllipseForm: (shape, shapes) => {
    // isSetMustFree.value = true;
    page.setTabActive("tabNodeEllipse", "：站点(圆形)");
    nodeEllipseForm.base = shape.base;
    nodeEllipseForm.code = shape.code;
    nodeEllipseForm.name = shape.name;
    nodeEllipseForm.data.nodeDescription = shape.data.nodeDescription;
    nodeEllipseForm.lock.enable = shape.lock.enable;
    nodeEllipseForm.data.released = shape.data.released;
    if (shape?.fields) {
      nodeEllipseForm.fields = shape?.fields;
    }
    isWatchChange.value = false;
    cloneNodeEllipseForm = stringify(nodeEllipseForm);
    setTimeout(() => {
      isWatchChange.value = true;
    });
  },
  setEdgeBeelineForm: shape => {
    console.log("shape is", shape);
    page.setTabActive("tabEdgeBeeline", "：路线(直线)");
    deepCopy(edgeBeelineForm, shape);
    isWatchChange.value = false;
    cloneEdgeBeelineForm = stringify(edgeBeelineForm);
    setTimeout(() => {
      isWatchChange.value = true;
    });
    // edgeBeelineForm.shape = toRefs(reactive(shape));
  },
  loadSimpleMap: async () => {
    showMapDialog.value = true;
  },
  loadPointCloudMap: async () => {
    let base64Src = await GetPointCloudMap(); //获取点云地图背景
    // let base64Src = url.map;
    let newImage = new Image();
    newImage.src = base64Src.data.lidarMapsBase64;
    let w = null;
    let h = null;
    newImage.onload = async () => {
      // 输出图片信息 比如可以获取图片宽高
      w = newImage.width;
      h = newImage.height;
      try {
        // console.log(w);
        // console.log(w * base64Src.data.ratio);
        // console.log("图片宽高", w, h);
        // console.log(
        //   "迷毂宽高/2",
        //   page.panel.map.base.size.w / 2,
        //   page.panel.map.base.size.h / 2
        // );
        // console.log("base64Src.data.distanceX", base64Src.data.distanceX);
        // console.log("base64Src.data.distanceY", base64Src.data.distanceY);
        page.panel.map.image.src = base64Src.data.lidarMapsBase64; //设置背景
        page.panel.map.image.origin = false;
        page.panel.map.image.manual = true;
        page.panel.map.image.manualSize.w = w * base64Src.data.ratio; //9000 * 10; // shanghaiMap 38000
        page.panel.map.image.manualSize.h = h * base64Src.data.ratio; //9000 * 10;
        page.panel.map.image.globalAlpha = 1;

        // page.panel.map.image.manualPoint.x = -550; //-13000 * 7.15; //-93000 // shanghaiMap -4550
        // page.panel.map.image.manualPoint.y =
        //   page.panel.map.base.size.h - h * base64Src.data.ratio; // 迷毂地图高 - 点云地图高 - 550
        /**  左下角
         * 把img定位到simple(0,0)点，然后+上偏移量
         * x轴 = 1/2*simple宽 + 点云地图偏移量
         * y轴 = 1/2*simple高 - 点云地图高 - 点云地图偏移量
         * */
        // page.panel.map.image.manualPoint.x =
        //   page.panel.map.base.size.w / 2 + base64Src.data.distanceX;
        // page.panel.map.image.manualPoint.y =
        //   page.panel.map.base.size.h / 2 -
        //   h * base64Src.data.ratio -
        //   base64Src.data.distanceY; //simple地图高/2 + 偏移值
        /**  左上角
         * 把点云图(0,0)定位到迷毂中心点，然后+偏移量 - 1/2站点宽高
         * */
        // page.panel.map.image.manualPoint.x =
        //   (page.panel.map.base.size.w + paddingList[0] + paddingList[2]) / 2 +
        //   base64Src.data.distanceX -
        //   (sideWH[0] / 2 + sideWH[2] / 2);
        // page.panel.map.image.manualPoint.y =
        //   (page.panel.map.base.size.h + paddingList[1] + paddingList[3]) / 2 -
        //   base64Src.data.distanceY -
        //   (sideWH[1] / 2 + sideWH[3] / 2);
        // simple中心点
        page.panel.map.image.manualPoint.x =
          coordinates.x + base64Src.data.distanceX;
        page.panel.map.image.manualPoint.y =
          coordinates.y - base64Src.data.distanceY;

        // page.panel.map.image.src = base64Src;
      } catch {}
    };
  },
  // TODO
  /**
   * 多选的逻辑：
   * 先用shapes拿到所有的数据，然后取第一个的数据显示，监听是否又改变，有改变使用mapjs中方法传递
   */
  setEdgeArcForm: shape => {
    page.setTabActive("tabEdgeArc", "：路线(圆弧)");
    deepCopy(edgeArcForm, shape);
    isWatchChange.value = false;
    cloneEdgeArcForm = stringify(edgeArcForm);
    setTimeout(() => {
      isWatchChange.value = true;
    });
  },
  setEdgeQuadraticForm: shape => {
    page.setTabActive("tabEdgeQuadratic", "：路线(贝塞尔二次)");
    deepCopy(edgeQuadraticForm, shape);
    isWatchChange.value = false;
    cloneEdgeQuadraticForm = stringify(edgeQuadraticForm);
    setTimeout(() => {
      isWatchChange.value = true;
    });
  },
  setEdgeBezierForm: shape => {
    page.setTabActive("tabEdgeBezier", "：路线(贝塞尔三次)");
    deepCopy(edgeBezierForm, shape);
    isWatchChange.value = false;
    cloneEdgeBezierForm = stringify(edgeBezierForm);
    setTimeout(() => {
      isWatchChange.value = true;
    });
  },
  setZoneRectForm: shape => {
    page.setTabActive("tabZoneRect", "：区域(矩形)");
    isWatchChange.value = false;
    deepCopy(zoneRectForm, shape);
    isWatchChange.value = false;
    cloneZoneRectForm = stringify(zoneRectForm);
    setTimeout(() => {
      isWatchChange.value = true;
    });
  },
  setZoneEllipseForm: shape => {
    page.setTabActive("tabZoneEllipse", "：区域(圆形)");
    isWatchChange.value = false;
    deepCopy(zoneEllipseForm, shape);
    isWatchChange.value = false;
    cloneZoneEllipseForm = stringify(zoneEllipseForm);
    setTimeout(() => {
      isWatchChange.value = true;
    });
  },
  setTagRectForm: shape => {
    page.setTabActive("tabTagRect", "：标签(矩形)");
    isWatchChange.value = false;
    deepCopy(tagRectForm, shape);
    isWatchChange.value = false;
    cloneTagRectForm = stringify(tagRectForm);
    setTimeout(() => {
      isWatchChange.value = true;
    });
  },
  setTagEllipseForm: shape => {
    page.setTabActive("tabTagEllipse", "：标签(圆形)");
    isWatchChange.value = false;
    deepCopy(tagEllipseForm, shape);
    isWatchChange.value = false;
    cloneTagEllipseForm = stringify(tagEllipseForm);
    setTimeout(() => {
      isWatchChange.value = true;
    });
    // tagEllipseForm.shape = toRefs(reactive(shape));
  },
  setTabActive: (tabId, tabTitle) => {
    tabCurrent.data = { id: tabId, title: tabTitle };
  },
  saveData: async data => {
    await Save(data);
    message("操作成功！", { type: "success" });
  },
  setNodePosition: shapeType => {},
  setTrajectory: shapeType => {},
  setActions: shapeType => {},
  setExtends: shapeType => {}
};
function triggerFileInput() {
  fileInput.value.click(); // 触发文件选择
}
let selectedFile = ref(null);
const handleFileChange = event => {
  const file = event.target.files[0];
  if (file) {
    const reader = new FileReader();
    reader.onload = e => {
      try {
        const fileContent = e.target.result; // 文件的内容是文本
        const jsonData = JSON.parse(fileContent); // 将文件内容解析为 JSON
        selectedFileName.value = file.name;
        selectedFile.value = jsonData;
        console.log("selectedFileName", selectedFileName.value);
      } catch (error) {
        // console.error("文件解析失败:", error);
        message("文件解析失败，请选择正确的文件格式！", { type: "error" });
      }
    };

    reader.readAsText(file); // 读取文件内容为文本
  }
};
const confirmMergeAxios = async () => {
  showMapDialog.value = false;
  const { data } = await GetMap();
  let res = data.map.fileContent;
  res = JSON.parse(res);
  console.log("res", res);
  loadSimpleMapFn(res);
};
const confirmMerge = () => {
  if (selectedFile.value && selectedFileName.value) {
    loadSimpleMapFn(selectedFile.value); // 合并地图，传递文件
    showMapSelection.value = false;
    selectedFileName.value = "";
  } else {
    selectedFileName.value = "";
    message("请选择一个文件进行合并", { type: "warning" });
  }
};

async function loadSimpleMapFn(file = null) {
  try {
    showMapDialog.value = false;
    let data = await GetSimpleMap();
    let res = await GetStorageListToSelect();
    // let data = {
    //   code:200,
    //   data:SimpleMap
    // };
    // console.log("data is", data);

    if (data.code === 200) {
      if (!Object.keys(data.data).length) {
        message("请打开Simple地图再进行操作！", { type: "warning" });
        return false;
      }
      // let MapInfo = JSON.parse(data.data)
      // console.log("res", "\n", res);
      let newMap = mapTransform.init(data.data, res.data);
      newMap.map.image.src = "";
      // console.log(newMap);
      let minXPoint = 0;
      let minYPoint = 0;
      let maxXPoint = 0;
      let maxYPoint = 0;
      let minXSize = 0;
      let minYSize = 0;
      let maxXSize = 0;
      let maxYSize = 0;
      coordinates = newMap.dy.base.point;
      // 循环遍历数据
      minXPoint = newMap.nodes[0].base.point;
      minYPoint = newMap.nodes[0].base.point;
      maxXPoint = newMap.nodes[0].base.point;
      maxYPoint = newMap.nodes[0].base.point;
      newMap.nodes.forEach(item => {
        const point = item.base.point;
        const size = item.base.size ? item.base.size : { w: 0, h: 0 };
        if (point.x < minXPoint.x) {
          // 左
          minXPoint = point;
          minXSize = size;
        }
        if (point.y < minYPoint.y) {
          // 上
          minYPoint = point;
          minYSize = size;
        }
        if (point.x >= maxXPoint.x) {
          // 右
          maxXPoint = point;
          maxXSize = size;
        }
        if (point.y >= maxYPoint.y) {
          // 下
          maxYPoint = point;
          maxYSize = size;
        }
      });

      paddingList = [
        minXPoint.x,
        minYPoint.y,
        newMap.map.base.size.w - maxXPoint.x,
        newMap.map.base.size.h - maxYPoint.y
      ];
      sideWH = [minXSize.w, minYSize.h, maxXSize.w, maxYSize.h];
      // console.log("paddingList", newMap);
      //** newMap.nodes.push()  newMap.edges.push() 原先的数据进去 */
      // newMap = {
      //   "dy": {}
      //   "nodes": []
      //   "map": {}
      //   "edges": []
      //}

      if (file) {
        newMap = mergeMap(newMap, file);
      }
      console.log("newMap", newMap);
      page.panel.setData(newMap);
    }
  } catch {
    message("加载地图失败！", { type: "error" });
  }
}
const mergeMap = (newMap, fileMap) => {
  // console.log("fileMap", fileMap);
  // // 合并 dy
  // newMap.dy = {
  //   ...newMap.dy,
  //   ...fileMap.dy
  // };

  // // 合并 map
  // newMap.map = {
  //   ...newMap.map,
  //   ...fileMap.map
  // };

  // 合并 nodes 和 edges
  newMap.nodes = mergeArrayById(newMap.nodes, fileMap.nodes, "id");
  newMap.edges = mergeArrayById(newMap.edges, fileMap.edges, "id");
  if (fileMap.map.image.src) {
    newMap.map.image.src = fileMap.map.image.src;
  }
  // 可选字段的合并处理
  if (fileMap.tags && fileMap.tags.length > 0) {
    newMap.tags = mergeOptionalFields(newMap.tags, fileMap.tags);
  }
  if (fileMap.zones && fileMap.zones.length > 0) {
    newMap.zones = mergeOptionalFields(newMap.zones, fileMap.zones);
  }

  return newMap;
};

// 合并可选字段的辅助函数
const mergeOptionalFields = (field1, field2) => {
  // 如果 field1 是 null 或 undefined，直接返回 field2
  if (!field1) {
    return field2;
  }

  // 如果 field2 是 null 或 undefined，直接返回 field1
  if (!field2) {
    return field1;
  }

  // 如果 field1 和 field2 都是数组，基于 id 合并
  if (Array.isArray(field1) && Array.isArray(field2)) {
    return mergeArrayById(field1, field2, "id");
  }

  // 如果不是数组，直接返回合并后的结果（可以根据需求调整）
  return [...field1, ...field2]; // 合并数组或对象
};

// 根据唯一键（如 id）合并数组
const mergeArrayById = (array1, array2, key = "id") => {
  const map = new Map();

  // 将 array1 中的元素存入 Map 中
  array1.forEach(item => map.set(item[key], item));

  // 遍历 array2，合并或追加
  array2.forEach(item => {
    if (map.has(item[key])) {
      const existingItem = map.get(item[key]);
      map.set(item[key], { ...existingItem, ...item });
    } else {
      map.set(item[key], item);
    }
  });

  // 返回合并后的数组
  return Array.from(map.values());
};
function findDifferences(newObj, oldObj, prefix = "", whitelist = []) {
  let changes = [];
  for (const key in newObj) {
    if (newObj.hasOwnProperty(key)) {
      const newPath = prefix ? `${prefix}.${key}` : key;

      // 检查路径是否包含白名单中的字符串
      const isWhitelisted = whitelist.some(whitelistedPath =>
        newPath.includes(whitelistedPath)
      );
      if (isWhitelisted) {
        continue; // 跳过白名单中的路径
      }

      if (typeof newObj[key] === "object" && newObj[key] !== null) {
        if (oldObj[key] === undefined || oldObj[key] === null) {
          // 如果旧对象中不存在这个键，说明是新增的属性
          changes.push({ path: newPath, value: newObj[key] });
        } else {
          changes = changes.concat(
            findDifferences(newObj[key], oldObj[key], newPath, whitelist)
          );
        }
      } else if (newObj[key] !== oldObj[key]) {
        changes.push({ path: newPath, value: newObj[key] });
      }
    }
  }
  return changes;
}
// 辅助函数，用于深拷贝
const deepCopy = (target, source) => {
  for (const key in target) {
    if (target.hasOwnProperty(key) && source.hasOwnProperty(key)) {
      if (typeof target[key] === "object" && target[key] !== null) {
        deepCopy(target[key], source[key]);
      } else {
        target[key] = source[key];
      }
    }
  }
};
/**
 * 思路：使用watch方法，赋值的时候给一个isChange的值，复制完成之后变回来，确保赋值的时候，不触发改变
 * path 方法增加值，填充第一项，另外解决path不正确问题
 *
 */
function offsetXYFn(oldVal, changes, type, isMultiple = false) {
  // if (isMultiple) return;
  let pointPrevX = oldVal.base.pointPrev.x;
  let pointPrevW = oldVal.base.sizePrev.w;
  let pointPrevY = oldVal.base.pointPrev.y;
  let pointPrevH = oldVal.base.sizePrev.h;
  if (!isMultiple) {
    console.log("nodeList", nodeList);
    nodeList.forEach((item, index) => {
      console.log(item.base.point);
      for (let i = 0; i < changes.length; i++) {
        page.panel.setActiveShapeParam(changes[i].path, changes[i].value, type);
        if (changes[i].path == "base.size.w") {
          page.panel.setActiveShapeParam(
            "base.point.x",
            pointPrevX + (pointPrevW - changes[i].value) / 2,
            type
          );
        } else if (changes[i].path == "base.size.h") {
          page.panel.setActiveShapeParam(
            "base.point.y",
            pointPrevY + (pointPrevH - changes[i].value) / 2,
            type
          );
        }
        //
      }
    });
  } else {
    changes.forEach(({ path, value }) => {
      page.panel.setActiveShapeParam(path, value, "EdgeArc");
    });
  }
}
watch(
  () => nodeRectForm.shape,
  (newVal, oldVal) => {
    if (!isWatchChange.value) return;
    let oldObj = parse(cloneNodeRectForm);
    const changes = findDifferences(newVal, oldObj);
    let w = false;
    if (nodeList >= 2) w = true;
    // offsetXYFn(oldVal, changes, "NodeRect", w);
    changes.forEach(({ path, value }) => {
      page.panel.setActiveShapeParam(path, value, "NodeRect");
    });
    cloneNodeRectForm = stringify(newVal);
  },
  { deep: true } // 深度监听对象的属性变化
);
// 监听站点
watch(
  () => nodeEllipseForm,
  // () => nodeRectForm.shape.base,
  (newVal, oldVal) => {
    if (!isWatchChange.value) return;
    let oldObj = parse(cloneNodeEllipseForm);
    const changes = findDifferences(newVal, oldObj);
    // offsetXYFn(oldVal, changes, "NodeEllipse");
    changes.forEach(({ path, value }) => {
      page.panel.setActiveShapeParam(path, value, "NodeEllipse");
    });
    cloneNodeEllipseForm = stringify(newVal);
  },
  { deep: true } // 深度监听对象的属性变化
);
watch(
  () => edgeBeelineForm,
  (newVal, oldVal) => {
    if (!isWatchChange.value) return;
    let oldObj = parse(cloneEdgeBeelineForm);
    const changes = findDifferences(newVal, oldObj);
    changes.forEach(({ path, value }) => {
      page.panel.setActiveShapeParam(path, value, "EdgeBeeline");
    });
    console.log("直线属性调整", newVal);
    cloneEdgeBeelineForm = stringify(newVal);
  },
  { deep: true } // 深度监听对象的属性变化
);
watch(
  () => edgeArcForm,
  (newVal, oldVal) => {
    if (!isWatchChange.value) return;
    let oldObj = parse(cloneEdgeArcForm);
    const changes = findDifferences(newVal, oldObj);
    changes.forEach(({ path, value }) => {
      page.panel.setActiveShapeParam(path, value, "EdgeArc");
    });
    cloneEdgeArcForm = stringify(newVal);
  },
  { deep: true } // 深度监听对象的属性变化
);
watch(
  // 二次贝塞尔
  () => edgeQuadraticForm,
  (newVal, oldVal) => {
    if (!isWatchChange.value) return;
    // console.log("二次贝塞尔属性调整", newVal);
    let oldObj = parse(cloneEdgeQuadraticForm);
    const changes = findDifferences(newVal, oldObj, "", ["base.p1", "base.p2"]);
    changes.forEach(({ path, value }) => {
      page.panel.setActiveShapeParam(path, value, "EdgeQuadratic");
    });
    cloneEdgeQuadraticForm = stringify(newVal);
  },
  { deep: true }
);
watch(
  () => edgeBezierForm,
  (newVal, oldVal) => {
    if (!isWatchChange.value) return;
    let oldObj = parse(cloneEdgeBezierForm);
    const changes = findDifferences(newVal, oldObj, "", ["base.p1", "base.p2"]);
    changes.forEach(({ path, value }) => {
      page.panel.setActiveShapeParam(path, value, "EdgeBezier");
    });
    // console.log("贝塞尔三次直线属性调整", newVal);
    cloneEdgeBezierForm = stringify(newVal);
  },
  { deep: true } // 深度监听对象的属性变化
);
watch(
  () => zoneRectForm,
  (newVal, oldVal) => {
    if (!isWatchChange.value) return;
    let oldObj = parse(cloneZoneRectForm);
    const changes = findDifferences(newVal, oldObj, "", ["base.p1", "base.p2"]);
    changes.forEach(({ path, value }) => {
      console.log("Path:", path, "\nValue:", value, "\nShapeType:", "NodeRect");
      page.panel.setActiveShapeParam(path, value, "ZoneRect");
    });
    cloneZoneRectForm = stringify(newVal);
  },
  { deep: true } // 深度监听对象的属性变化
);
watch(
  () => zoneEllipseForm,
  (newVal, oldVal) => {
    if (!isWatchChange.value) return;
    let oldObj = parse(cloneZoneEllipseForm);
    const changes = findDifferences(newVal, oldObj, "", ["base.p1", "base.p2"]);
    changes.forEach(({ path, value }) => {
      page.panel.setActiveShapeParam(path, value, "ZoneEllipse");
    });
    cloneZoneEllipseForm = stringify(newVal);
  },
  { deep: true } // 深度监听对象的属性变化
);
watch(
  () => tagRectForm,
  (newVal, oldVal) => {
    if (!isWatchChange.value) return;
    let oldObj = parse(cloneTagRectForm);
    const changes = findDifferences(newVal, oldObj, "", ["base.p1", "base.p2"]);
    changes.forEach(({ path, value }) => {
      page.panel.setActiveShapeParam(path, value, "TagRect");
    });
    cloneTagRectForm = stringify(newVal);
  },
  { deep: true } // 深度监听对象的属性变化
);
watch(
  () => tagEllipseForm,
  (newVal, oldVal) => {
    if (!isWatchChange.value) return;
    let oldObj = parse(cloneTagEllipseForm);
    const changes = findDifferences(newVal, oldObj, "", ["base.p1", "base.p2"]);
    changes.forEach(({ path, value }) => {
      page.panel.setActiveShapeParam(path, value, "TagEllipse");
    });
    cloneTagEllipseForm = stringify(newVal);
  },
  { deep: true } // 深度监听对象的属性变化
);
</script>
<style scoped>
@import "./index.scss";

.fixd {
  position: fixed;
  bottom: 20px;
  right: 350px;
}

.el-scrollbar {
  height: 79vh;
}
.options {
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}
/* .options-div :hover {
  background-color: #c6e4f1;
} */
.flex {
  font-size: 14px;
  display: flex;
  margin-top: 3px;
  cursor: pointer;
}
.flex :hover {
  /* background-color: #c6e4f1; */
}
/* 选中样式 */
.selected {
  background-color: #c6e4f1;
}
.fz-30 {
  font-size: 20px;
}
.w50 {
  width: 50%;
  /* padding: 5px; */
}
.T-center {
  text-align: center;
}

#boxPanel {
  height: 100%;
}

::v-deep(.el-menu-item) {
  height: 30px !important;
  color: #000 !important;
  background-color: #fff !important;
}

::v-deep(#boxToolbar .el-menu--vertical .el-menu-item:hover) {
  color: rgb(187, 187, 187) !important;
}

::v-deep(#boxToolbar .el-menu--vertical .el-menu-item.is-active) {
  color: rgb(114, 46, 209) !important;
}

/* 覆盖 .el-menu-item.is-active 的悬停样式 */
::v-deep(#boxToolbar .el-menu--vertical .el-menu-item.is-active:hover) {
  color: rgb(114, 46, 209) !important;
}

.box-map {
  height: 100%;
}

::v-deep(.box-map .el-card__body) {
  height: calc(100% - 150px);
}

::v-deep(.box-map .el-scrollbar__view) {
  height: 100%;
}
</style>
