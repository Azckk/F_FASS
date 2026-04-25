import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
// import {
//   // getPage,
//   // addOrUpdate,
//   // deletes,
//   GetListToSelect,
//   setData,
//   getData
// } from "@/api/data/chargingstation";

import { getPage, addOrUpdate, deletes } from "@/api/object/trafficlight";

// import { GetListToSelect as gatChargeNodePage } from "@/api/data/charge";
// import { GetTypeListToSelect, getPage as gatCarPage } from "@/api/data/car";
import type { FormItemProps } from "../utils/types";
import { GetDictItemInLocal } from "@/utils/auth";
import editForm from "../form.vue";
import address from "../extend/index.vue";

// import strategyForm from "../strategy.vue";
import { useMyI18n } from "@/plugins/i18n";

export function useHook(tableRef?: Ref | undefined) {
  const { t } = useMyI18n();
  const isEnglish = new RegExp("[A-Za-z]+");
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    code: "",
    name: "",
    carTypeId: "",
    chargeId: ""
  });
  const formRef = ref();
  const loading = ref(true);
  const nodeList = ref([]);
  const carList = ref([]);
  const dataList = ref([]);
  const ChargingMode = ref();
  const ChargingProtocol = ref([]);
  const selection = ref([]);
  const pageParam = ref({});
  const carTypeList = ref([]);
  // const current = ref([
  //   { value: "10", label: "10A" },
  //   { value: "20", label: "20A" },
  //   { value: "40", label: "40A" }
  // ]);
  // const voltage = ref([
  //   { value: "12", label: "12A" },
  //   { value: "24", label: "24A" },
  //   { value: "48", label: "48A" },
  //   { value: "60", label: "60A" },
  //   { value: "72", label: "72A" }
  // ]);
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true
  });

  const columns: TableColumnList = [
    { type: "selection", align: "left", sortable: true },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.name"),
      label: "table.name",
      prop: "name",
      sortable: true
    },
    {
      headerRenderer: () => t("table.communicationMode"),
      label: "table.protocolType",
      prop: "protocolType",
      sortable: true,
      formatter: ({ protocolType }) => {
        try {
          let dictResult = ChargingProtocol.value.filter(
            x => x.id === protocolType
          );
          if (dictResult && dictResult[0]) {
            return dictResult[0].name;
          }
        } catch (error) {
          return "未知";
        }
      }
    },

    {
      headerRenderer: () => t("IP"),
      label: "table.chargingPileIP",
      prop: "ip",
      sortable: true
    },
    {
      headerRenderer: () => t("table.port"),
      label: "table.port",
      prop: "port",
      sortable: true
    },
    {
      headerRenderer: () => t("table.isEnable"),
      label: "table.isEnable",
      prop: "isEnable",
      sortable: true,
      cellRenderer: ({ row, props }) => (
        <el-icon color={row.isEnable ? "green" : "red"} class="no-inherit">
          {row.isEnable ? (
            <div style="display: flex; align-items: center">
              <iconify-icon-online icon="ep:check" />
              <span style="margin-left: 10px">{row.date}</span>
            </div>
          ) : (
            <div style="display: flex; align-items: center">
              <iconify-icon-online icon="ep:close" />
              <span style="margin-left: 10px">{row.date}</span>
            </div>
          )}
        </el-icon>
      )
      // cellRenderer: ({ row, props }) => (
      //   <el-icon color={row.isEnable ? "green" : "red"} class="no-inherit">
      //     {row.isEnable ? (
      //       <div style="display: flex; align-items: center">
      //         <iconify-icon-online icon="ep:check" />
      //         <span style="margin-left: 10px">{row.date}</span>
      //       </div>
      //     ) : (
      //       <div style="display: flex; align-items: center">
      //         <iconify-icon-online icon="ep:close" />
      //         <span style="margin-left: 10px">{row.date}</span>
      //       </div>
      //     )}
      //   </el-icon>
      // )
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      minWidth: 120,
      sortable: true,
      formatter: ({ createAt }) => dayjs(createAt).format("YYYY-MM-DD HH:mm:ss")
    }
    // {
    //   headerRenderer: () => t("table.state"),
    //   label: "table.state",
    //   prop: "state",
    //   sortable: true
    // }
    // { label: "操作", slot: "operation", minWidth: 120 ,sortable: true }
  ];

  function handleSelection(val) {
    selection.value = val;
  }

  function handlePageSize() {
    handleSearch();
  }

  function handlePageCurrent() {
    handleSearch();
  }

  function handlePageParam(id?: string) {
    query.carTypeId = id;
    return {
      pageParam: JSON.stringify({
        where: Object.keys(query)
          .filter(e => query[e])
          .map(e => ({
            logic: "And",
            field: e,
            operator: "Contains",
            value: query[e]
          })),
        order: [{ field: "createAt", sequence: "DESC" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }
  // StandbyState
  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    const { data } = await getPage(pageParam.value);
    dataList.value = data.rows;
    // console.log(data);
    pagination.total = data.total;
    loading.value = false;
  }
  // async function handleSearchCar(id?: string) {
  //   loading.value = true;
  //   pageParam.value = handlePageParam(id);
  //   const { data } = await gatCarPage(pageParam.value);
  //   carList.value = data.rows;
  //   pagination.total = data.total;
  //   loading.value = false;
  // }
  // async function changeCarType(id) {
  //   handleSearchCar(id);
  // }
  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    handleSearch();
  }
  function handleRow(row = undefined) {
    return {
      // nodeId: row?.nodeId ?? "",
      // id: row?.id ?? "",
      code: row?.code ?? "",
      ip: row?.ip ?? "",
      name: row?.name ?? "",
      protocolType: row?.protocolType ?? "",
      // state: row?.state ?? "",
      // voltage: row?.voltage ?? "",
      // chargingMode: row?.ChargingMode ?? "",
      // sortNumber: row?.sortNumber ?? 0,
      isEnable: row?.isEnable ?? true,
      isDelete: row?.isDelete ?? false,
      // chargeId: row?.chargeId ?? "",
      // chargeCode: row?.chargeCode ?? "",
      port: row?.port ?? ""
      // protocol: row?.protocol ?? "",
      // mode: row?.mode ?? "",
      // current: row?.current ?? "",
      // isOccupied: row?.isOccupied ?? false,
      // occupiedCarId: row?.occupiedCarId ?? ""
      // "id":row?.id ?? "",
      // "createAt":row?.createAt ?? "",
      // "createBy":row?.createBy ?? "",
      // "updateAt":row?.updateAt ?? "",
      // "updateBy":row?.updateBy ?? "",
      // "remark": row?.remark ?? "",
      // "extend": row?.extend ?? "",
    };
  }
  // // 获取字典充电模式
  // async function GetChargModeList() {
  //   try {
  //     // const data = await GetDictItemInLocal("ChargingMode");
  //     const data = await GetListToSelect("ChargingMode");
  //     ChargingMode.value = [...data.data];
  //   } catch (error) {
  //     console.error("Error:", error);
  //   }
  // }

  function handleDetail(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "查看",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      hideFooter: true,
      props: { formInline: handleRow(row) },
      contentRenderer: () => editForm
    });
  }

  function handleAdd() {
    addDialog({
      title: "添加",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: handleRow() },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options, index }) => {
        const formData = options.props.formInline as FormItemProps;
        formData.code = new Date().getTime().toString();
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            console.log("提交数据", formData);
            await addOrUpdate("", formData);
            message("操作成功！", { type: "success" });
            handleSearch();
            done();
          }
        });
      }
    });
  }

  function handleUpdate(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "修改",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: handleRow(row) },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options, index }) => {
        const formData = options.props.formInline as FormItemProps;
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            await addOrUpdate({ keyValue: row?.id }, formData);
            message("操作成功！", { type: "success" });
            handleSearch();
            done();
          }
        });
      }
    });
  }
  async function handleStrategy(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    addDialog({
      title: "地址位",
      width: "45%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: row },
      contentRenderer: () => h(address, { ref: formRef }),
      closeCallBack: () => { }
      // beforeSure: (done, { options }) => {
      //   const formData = options.props.formInline as FormItemProps;
      //   formRef.value.getRef().validate(async valid => {
      //     if (valid) {
      //       await setData(formData);
      //       message("操作成功！", { type: "success" });
      //       handleSearch();
      //       done();
      //     }
      //   });
      // }
    });
  }

  async function handleDelete(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await deletes(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleEnable(rows = selection.value, cancel = undefined) { }

  async function handleDisable(rows = selection.value, cancel = undefined) { }

  // async function getNodeListToSelect() {
  //   pageParam.value = handlePageParam();
  //   const { data } = await gatChargeNodePage();
  //   nodeList.value = data;
  // }
  async function GetChargingProtocol() {
    try {
      const data = await GetDictItemInLocal("ProtocolType");
      ChargingProtocol.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }
  // async function TypeListToSelect() {
  //   const { data } = await GetTypeListToSelect();
  //   carTypeList.value = [...data];
  // }

  onMounted(async () => {
    handleSearch();
    GetChargingProtocol();
    // getNodeListToSelect();
    // GetChargModeList();
    // TypeListToSelect();
  });

  return {
    query,
    loading,
    columns,
    ChargingMode,
    carList,
    dataList,
    nodeList,
    pagination,
    // current,
    // voltage,
    carTypeList,
    ChargingProtocol,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleDetail,
    handleAdd,
    handleUpdate,
    handleDelete,
    handleEnable,
    handleDisable,
    // changeCarType,
    handleStrategy
  };
}
