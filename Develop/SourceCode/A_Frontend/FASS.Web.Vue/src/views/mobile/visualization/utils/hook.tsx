import { h, ref, reactive, onMounted, onUnmounted } from "vue";
import { ElMessageBox } from "element-plus";
// import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  GetAreaList,
  StorageGetPage,
  GetTagPage,
  AddTag,
  TagDelete,
  GetListToSelect
} from "@/api/warehouse/visualization";
import { GetDictItemInLocal } from "@/utils/auth";

import editForm from "../panel/form.vue";

export function useHook() {
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    code: "",
    name: ""
  });
  const formRef = ref();
  const loading = ref(true);
  const dataList = ref([]);
  const StorageTypeList = ref([]);
  const StorageStateList = ref([]);
  const nodeList = ref([]);
  const WarehouseAreaList = ref([]);
  const selection = ref([]);
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

  function handleSelection(val) {
    selection.value = val;
  }

  function handlePageSize() {
    handleSearch();
  }

  function handlePageCurrent() {
    handleSearch();
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

  async function handleSearch() {
    // loading.value = true;
    // pageParam.value = handlePageParam();
    // const { data } = await getPage(pageParam.value);
    // dataList.value = data.rows;
    // pagination.total = data.total;
    // loading.value = false;
  }
  async function GetTagPageFn() {
    pageParam.value = handlePageParam();
    const { data } = await GetTagPage(pageParam.value);
    return data;
  }
  let keyValue = ref();
  function handleUpdate(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows.tags[0];
    // console.log(" row ", rows);
    keyValue.value = rows.id;
    addDialog({
      title: "修改",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: handleRow(row), id: rows.id },
      contentRenderer: () => h(editForm, { ref: formRef }),
      // beforeSure: async (done, { options, index }) => {
      //   await handleGetList(areaList.value[0].id);
      //   done();
      // }
      beforeSure: async (done, { options, index }) => {
        const formData = formRef.value.getRef();
        let res;
        if (formData == undefined) {
          // 未修改直接关闭弹窗
          done();
        } else if (formData.type == "del") {
          res = await TagDelete(formData.keyValue, [formData]);
        } else if (formData.type == "add") {
          res = await AddTag(formData.keyValue, [formData]);
        } else {
          done();
        }
        if (res && res.code == 200 && res.data == "OK") {
          message("操作成功！", { type: "success" });
          await handleGetList(activeAreaId.value);
          done();
        }
      }
    });
  }
  const colour = ref();
  async function getColourData(id) {
    const { data } = await GetListToSelect();
    // console.log(data, id);
    // console.log(data.find(item => item.id === id));
    return data.find(item => item.id === id);
  }
  async function changeValue(row) {
    console.log(row);
    ElMessageBox.confirm(`是否确定操作？`)
      .then(async () => {
        if (row.tagId == "") {
          // 删除
          // let res = await TagDelete(row, data);
        } else {
          let data = await getColourData(row.tagId);
          console.log(data);
          let res = await AddTag(row.areaId, data);
          console.log(res);
        }
      })
      .catch(() => {
        row.tagId = (() => {
          if (row.tags.length > 0) {
            return row.tags[0]?.id;
          } else {
            return "";
          }
        })();
      });
    // if (typeof row.tagData === "string") {
    //   // 删除tag
    //   if (!row.tags[0]) return;
    //   ElMessageBox.confirm(`确定删除“${row.tags[0].name}”标签？`)
    //     .then(async () => {
    //       let data = [
    //         {
    //           id: row?.tags[0].id,
    //           sortNumber: row?.tags[0].sortNumber ?? 0,
    //           isEnable: true,
    //           isDelete: false,
    //           name: row?.tags[0].name,
    //           value: row?.tags[0].value,
    //           colour: row?.tags[0].colour
    //         }
    //       ];
    //       let res = await TagDelete(row, data);
    //       if (res.code == 200 && res.data == "OK") {
    //         handleGetList(areaList.value[0].id);
    //       }
    //     })
    //     .catch(() => {
    //       // catch error
    //     });
    // } else {
    //   // 添加标签
    //   let data = [
    //     {
    //       id: row.tagData.id,
    //       sortNumber: row.tagData.sortNumber ?? 0,
    //       isEnable: true,
    //       isDelete: false,
    //       name: row.tagData.name,
    //       value: row.tagData.value,
    //       colour: row.tagData.colour
    //     }
    //   ];
    //   let res = await AddTag(row.tagData.keyValue, data);
    //   if (res.code === 200) {
    //     ElMessage({
    //       message: `添加“${row.name}”标签成功`,
    //       type: "success"
    //     });
    //     // colour.value = row.colour;
    //     handleGetList(areaList.value[0].id);
    //   }
    // }
  }
  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    handleSearch();
  }

  function handleRow(row = undefined) {
    return {
      colour: row?.colour ?? "",
      id: row?.id ?? null,
      name: row?.name ?? null,
      isEnable: row?.isEnable ?? true,
      isLock: row?.isLock ?? false,
      sortNumber: row?.sortNumber ?? 0,
      value: row?.value,
      keyValue: keyValue.value
    };
  }
  async function GetStorageTypeList() {
    try {
      const data = await GetDictItemInLocal("StorageType");
      StorageTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  async function GetWarehouseAreaToSelct() {
    // try {
    //   const { data } = await getWarehouseAreaList();
    //   WarehouseAreaList.value = [...data];
    // } catch (error) {
    //   console.error("Error:", error);
    // }
  }
  async function handleSendWorkFn(row) {
    console.log(row);
  }
  async function handleGetList(row) {
    console.log(row);
    activeAreaId.value = row.index ? row.index : row;
    if (activeAreaId.value === "all") {
      activeAreaId.value = "";
    }
    pageParam.value = handlePageParam();
    // console.log("原始的库位状态数据", dataList.value);
    const { data } = await StorageGetPage(activeAreaId.value, pageParam.value);
    data.rows.forEach(element => {
      element.tagId = (() => {
        if (element.tags.length > 0) {
          return element.tags[0]?.id;
        } else {
          return "";
        }
      })();
    });
    dataList.value = data.rows;
    console.log("请求回来的对应库位状态数据 dataList", dataList.value);
  }
  const tagListData = ref();
  async function GetListToSelectFn() {
    const { data } = await GetListToSelect();
    tagListData.value = data;
  }
  let timer = null;
  onMounted(async () => {
    GetStorageTypeList();
    handleSearch();
    GetWarehouseAreaToSelct();
    GetAreaListFn();
    GetListToSelectFn();
    timer = setInterval(() => {
      handleGetList(activeAreaId.value);
    }, 5000);
    // const { data } = await getList();
  });

  onUnmounted(() => {
    clearInterval(timer);
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
    handleSearch,
    handleReset,
    handleSendWorkFn,
    handleGetList,
    GetTagPageFn,
    changeValue,
    handleUpdate
  };
}
