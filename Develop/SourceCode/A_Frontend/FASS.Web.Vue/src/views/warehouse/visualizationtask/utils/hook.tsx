import { h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
// import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { GetAreaList, GetListToSelect } from "@/api/warehouse/visualization";
import {
  StorageGetPage,
  AddWork,
  ContainerAddMaterial
} from "@/api/warehouse/visualizationTask";
import { GetDictItemInLocal } from "@/utils/auth";

import select from "../sendwork/select.vue";
import userSelect from "../material/select.vue";
import container from "../panel/container/index.vue";
import material from "../panel/material/index.vue";

export function useHook() {
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    code: "",
    name: ""
  });
  const formRef = ref();
  const loading = ref(true);
  const dataList = ref([]);
  const handleSendWorkFns = ref("123");
  const StorageTypeList = ref([]);
  const StorageStateList = ref([]);
  const nodeList = ref([]);
  const WarehouseAreaList = ref([]);
  const selection = ref([]);
  const selectRef = ref();
  const pageParam = ref({});
  const areaList = ref([]);
  const activeAreaId = ref();
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 99999,
    currentPage: 1,
    background: true
  });
  const columns: TableColumnList = [
    { type: "selection", align: "left" },
    { label: "编号", prop: "id", hide: true }

    // { label: "操作", slot: "operation", minWidth: 120 }
  ];

  // function settingDestination() {
  //   console.log("设置终点");
  // }
  // function settingStart() {
  //   console.log("设置起点");
  // }

  function handleSelection(val) {
    selection.value = val;
  }

  function handlePageSize() {
    handleGetList("all");
  }

  function handlePageCurrent() {
    handleGetList("all");
  }

  function handlePageParam() {
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
        order: [{ field: "code", sequence: "ASC" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }

  async function GetAreaListFn() {
    const { data } = await GetAreaList();
    areaList.value = data;
    // console.log(areaList.value);
    if (areaList.value.length) {
      await handleGetList("all");
      // await handleGetList(areaList.value[0].id);
    }
  }

  function handleContainer(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows;

    addDialog({
      title: "容器",
      props: { roleId: row?.id ?? null },
      // props: { roleId: rows.id },
      width: "80%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      contentRenderer: () => container,
      closeCallBack: () => handleGetList("all")
    });
  }
  function handleMaterial(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    if (!rows?.containers || rows.containers.length === 0) {
      message("请先设置容器状态", { type: "error" });
      return;
    }
    const row = rows;
    const containersId = rows?.containers[0].id; //获取容器ID
    // console.log(rows);

    if (rows.state == "EmptyContainer" || rows.materials.length === 0) {
      addDialog({
        title: "添加",
        width: "80%",
        alignCenter: true,
        draggable: true,
        fullscreenIcon: true,
        closeOnClickModal: false,
        contentRenderer: () => h(userSelect, { ref: selectRef }),
        beforeSure: async (done, { options, index }) => {
          const rows = selectRef.value.tableRef
            .getTableRef()
            .getSelectionRows();
          if (rows?.length != 0) {
            await ContainerAddMaterial(containersId, rows);
            message("操作成功！", { type: "success" });
          }

          // handleSearch();
          // handleGetList("all");
          done();
        },
        closeCallBack: () => handleGetList("all")
      });

      return;
    }

    // console.log(containersId);
    addDialog({
      title: "物料",
      props: { roleId: row?.id ?? null, containersId: containersId },
      width: "80%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      contentRenderer: () => material,
      closeCallBack: () => handleGetList("all")
    });
  }

  function handleSetStartPoin(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows;
    addDialog({
      title: "任务站点",
      props: {
        roleId: row?.id ?? null,
        handleSendWorkFns: handleSendWorkFns.value
      },
      width: "30%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      contentRenderer: () => select,
      closeCallBack: () => handleGetList("all")
    });
  }

  // async function handleSendWorkFn(row) {
  //   console.log(row + "触发了");
  // }

  // async function handleSearch() {
  //   loading.value = true;
  //   pageParam.value = handlePageParam();
  //   console.log("搜索", pageParam.value);

  //   // const { data } = await StorageGetPage(pageParam.value);
  //   // const { data } = await StorageGetPage(activeAreaId.value, pageParam.value);
  //   dataList.value = data.rows;
  //   pagination.total = data.total;
  //   loading.value = false;
  // }
  // async function GetTagPageFn() {
  //   pageParam.value = handlePageParam();
  //   const { data } = await GetTagPage(pageParam.value);
  //   return data;
  // }
  let keyValue = ref();

  const colour = ref();
  // async function getColourData(id) {
  //   const { data } = await GetListToSelect();
  //   // console.log(data, id);
  //   // console.log(data.find(item => item.id === id));
  //   return data.find(item => item.id === id);
  // }
  async function changeValue(row) { }
  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    // handleSearch();
    handleGetList("all");
  }

  // function handleRow(row = undefined) {
  //   return {
  //     colour: row?.colour ?? "",
  //     id: row?.id ?? null,
  //     name: row?.name ?? null,
  //     isEnable: row?.isEnable ?? true,
  //     isLock: row?.isLock ?? false,
  //     sortNumber: row?.sortNumber ?? 0,
  //     value: row?.value,
  //     keyValue: keyValue.value
  //   };
  // }
  async function GetStorageTypeList() {
    try {
      const data = await GetDictItemInLocal("StorageType");
      StorageTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }
  async function handleGetList(row) {
    activeAreaId.value = row.index ? row.index : row;
    if (activeAreaId.value === "all") {
      activeAreaId.value = "";
    }
    pageParam.value = handlePageParam();
    const { data } = await StorageGetPage(activeAreaId.value, pageParam.value);
    dataList.value = data.rows;
  }
  const tagListData = ref();

  onMounted(async () => {
    GetStorageTypeList();
    // handleSearch();
    // GetAreaListFn();
    // GetListToSelectFn();
    // const { data } = await getList();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    nodeList,
    areaList,
    WarehouseAreaList,
    pagination,
    StorageTypeList,
    StorageStateList,
    colour,
    tagListData,
    handlePageParam,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleReset,
    handleSendWorkFns,
    // handleSendWorkFn,
    handleGetList,
    changeValue,
    handleContainer,
    handleMaterial,
    handleSetStartPoin
  };
}
