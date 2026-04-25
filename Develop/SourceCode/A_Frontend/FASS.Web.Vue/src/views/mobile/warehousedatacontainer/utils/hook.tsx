import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { type PaginationProps } from "@pureadmin/table";
import { allowMouseEvent, deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { getPage, Update } from "@/api/mobile/warehousedatacontainer";
import { getPage as getCarListApi } from "@/api/data/car";
import { GetDictItemInLocal } from "@/utils/auth";
import { GetListToSelect as GetNodeListToSelect } from "@/api/base/node";
import editForm from "../form.vue";
// import locationFrom from "../locationFrom.vue";
// import actionFrom from "../actionFrom.vue";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(tableRef?: Ref) {
  const isEnglish = new RegExp("[A-Za-z]+");
  const { t } = useMyI18n();
  const switchLoadMap = ref({});
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    code: "",
    name: "",
    kind: "",
    areaCode: ""
  });
  const formRef = ref();
  const loading = ref(true);
  const dataList = ref([]);
  const carList = ref();
  const nodeList = ref();
  const StorageStateList = ref([]);
  const selection = ref([]);
  const pageParam = ref({});
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true
  });
  const columns: TableColumnList = [
    { type: "selection", align: "left" },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.region"),
      label: "table.region",
      prop: "areaCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.code"),
      label: "table.code",
      prop: "code",
      sortable: true
    },
    {
      headerRenderer: () => t("table.status"),
      label: "table.status",
      prop: "state",
      sortable: true,
      formatter: ({ state }) => {
        if (isEnglish.test(t("table.status"))) {
          return state;
        } else {
          if (state == "EmptyMaterial") {
            return "空物料";
          } else if (state == "FullMaterial") {
            return "满物料";
          }
        }
        // return "未知";
      }
    },
    {
      headerRenderer: () => t("table.barcode"),
      label: "table.barcode",
      prop: "barcode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.isLock"),
      label: "table.isLock",
      prop: "isLock",
      sortable: true,
      cellRenderer: ({ row, props }) => (
        <el-icon color={row.isLock ? "green" : "red"} class="no-inherit">
          {row.isLock ? (
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
    }
    // {
    //   headerRenderer: () => t("table.operation"),
    //   label: "table.operation",
    //   slot: "operation",
    //   minWidth: 120
    // }
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
        order: [{ field: "createAt", sequence: "DESC" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }

  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    const { data } = await getPage(pageParam.value);
    dataList.value = data.rows;
    pagination.total = data.total;
    loading.value = false;
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
      name: row?.name ?? null,
      code: row?.code ?? null,
      isEnable: row?.isEnable ?? true,
      isLock: row?.isLock ?? false,
      areaCode: row?.areaCode ?? null,
      state: row?.state ?? null,
      nodeCode: row?.nodeCode ?? null,
      id: row?.id ?? null,
      sortNumber: row?.sortNumber ?? null,
      isDelete: row?.isDelete ?? null,
      createAt: row?.createAt ?? null,
      createBy: row?.createBy ?? null,
      updateAt: row?.updateAt ?? null,
      updateBy: row?.updateBy ?? null,
      remark: row?.remark ?? null,
      extend: row?.extend ?? null,
      areaId: row?.areaId ?? null,
      nodeId: row?.nodeId ?? null,
      type: row?.type ?? null,
      barcode: row?.barcode ?? null
    };
  }

  // 添加
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
        const formData = options.props.formInline;
        console.log(formData);
        let data = {
          carId: formData.carId,
          targetNodeId: formData.targetNodeId
        };
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            let res = await Update(data);
            if (res.code == 200) {
              message("操作成功！", { type: "success" });
              handleSearch();
              done();
            } else {
              message("操作失败！", { type: "warning" });
            }
          }
        });
      }
    });
  }

  async function getCarListFn(row?) {
    pageParam.value = handlePageParam();
    let res = await getCarListApi(pageParam.value);
    carList.value = res.data.rows;
  }
  async function GetNodeListToSelectFn(row?) {
    pageParam.value = handlePageParam();
    let res = await GetNodeListToSelect(pageParam.value);
    nodeList.value = res.data;
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
      props: {
        formInline: handleRow(row),
        StorageStateList: StorageStateList.value
      },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options, index }) => {
        const formData = options.props.formInline;
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            let res = await Update({ keyValue: row?.id }, formData);
            if (res.code == 200) {
              message("操作成功！", { type: "success" });
              handleSearch();
              done();
            } else {
              message("操作失败！", { type: "warning" });
            }
          }
        });
      }
    });
  }

  async function GetStorageStateList() {
    try {
      const data = await GetDictItemInLocal("ContainerState");
      // for (var i = 0; i < data.length; i++) {
      //   var code = data[i].code;
      //   switch (code) {
      //     case "NoneContainer":
      //       data[i].code = "无容器";
      //       break;
      //     case "FullContainer":
      //       data[i].code = "满容器";
      //       break;
      //     case "EmptyContainer":
      //       data[i].code = "空容器";
      //       break;
      //     default:
      //       break;
      //   }
      // }
      StorageStateList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  onMounted(async () => {
    handleSearch();
    getCarListFn();
    GetNodeListToSelectFn();
    GetStorageStateList();
    // const { data } = await getList();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    pagination,
    carList,
    nodeList,
    StorageStateList,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    // handleAdd,
    handleUpdate
  };
}
