import { type Ref, h, ref, reactive, onMounted } from "vue";
//  /
import { ElMessageBox } from "element-plus";
// import dayjs from "dayjs";
import { useMyI18n } from "@/plugins/i18n";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { GetActions } from "@/api/base/node";
// import { usePublicHooks } from "../../hooks";
import editForm from "../form.vue";
import operationForm from "../../operation/index.vue";
// import actionTypeData from "./actionType.json";
import { v4 as uuidv4 } from "uuid";
export function useHook(tableRef: Ref, nodeId: string) {
  const { t } = useMyI18n();
  const query = reactive({
    actionType: "",
    blockingType: ""
  });
  const formRef = ref();
  const loading = ref(true);
  const dataList = ref([]);
  const dataListSave = ref([]);
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
      headerRenderer: () => t("table.actionType"),
      label: "table.actionType",
      prop: "actionType",
      sortable: true
      // formatter: ({ actionType }) => {
      //   // 字典中没有该类型，引用自定义数据（根据GetListToSelectByCarId请求回来的）
      //   let dictResult = actionTypeData.filter(x => x.code === actionType);
      //   if (locale.value == "zh") {
      //     return dictResult[0].name;
      //   } else {
      //     return dictResult[0].code;
      //   }
      // }
    },
    {
      headerRenderer: () => t("table.blockageType"),
      label: "table.blockageType",
      prop: "blockingType",
      sortable: true
      // formatter: ({ blockingType }) => {
      //   if (locale.value == "zh") {
      //     if (blockingType == "NONE") {
      //       return "无限制";
      //     } else if (blockingType == "SOFT") {
      //       return "禁止行驶";
      //     } else if (blockingType == "HARD") {
      //       return "禁止行驶且禁止其它动作";
      //     } else {
      //       return blockingType;
      //     }
      //   } else {
      //     return blockingType;
      //   }
      // }
    },
    {
      headerRenderer: () => t("table.sortNumber"),
      label: "table.sortNumber",
      prop: "sortNumber",
      sortable: true
    }
    // {
    //   headerRenderer: () => t("table.sequenceId"),
    //   label: "table.sequenceId",
    //   prop: "sequenceId",
    //   sortable: true
    // }
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
        order: [{ field: "createAt", sequence: "DESC" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }

  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    const { data } = await GetActions(nodeId);
    dataList.value = data?.rows ?? [];
    // dataList.value = [
    //   {
    //     nodeId: null,
    //     actionType: "StopPause",
    //     blockingType: "NONE",
    //     sortNumber: 1
    //   },
    //   {
    //     nodeId: null,
    //     actionType: "StopPause",
    //     blockingType: "NONE",
    //     sortNumber: 2
    //   },
    //   {
    //     nodeId: null,
    //     actionType: "StopPause",
    //     blockingType: "SOFT",
    //     sortNumber: 1
    //   }
    // ];
    dataListSave.value = dataList.value;
    pagination.total = parseInt(data.total);
    loading.value = false;
  }

  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    query.actionType = "";
    query.blockingType = "";
    handleSearchArrList();
  }

  function handleRow(row = undefined) {
    return {
      nodeId: row?.nodeId ?? null,
      sortNumber: row?.sortNumber ?? null,
      blockingType: row?.blockingType ?? null,
      actionType: row?.actionType ?? null,
      operationDescription: row?.operationDescription ?? null
    };
  }

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
      // hideFooter: true,
      props: { formInline: handleRow(), operation: operation },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options }) => {
        const formData = options.props.formInline;
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            formData.uuid = uuidv4();
            dataListSave.value.push(formData);
            dataList.value = dataListSave.value; // 接口确认之后记得删除
            // await addOrUpdate("", formData);
            message("操作成功！", { type: "success" });
            // handleSearch();
            done();
          }
        });
      }
    });
  }
  function handleEdit() {}
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
      // hideFooter: true,
      props: { formInline: handleRow(row), operation: operation },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options }) => {
        const formData = options.props.formInline;
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            for (let i = 0; i < dataList.value.length; i++) {
              if (dataList.value[i].uuid === row.uuid) {
                dataList.value[i] = formData;
              }
            }
            // await addOrUpdate("", formData);
            message("操作成功！", { type: "success" });
            // handleSearch();
            done();
          }
        });
      }
    });
  }
  function handleDelete(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        for (let i = 0; i < dataList.value.length; i++) {
          if (dataList.value[i].uuid === rows[0].uuid) {
            dataList.value.splice(i, 1);
          }
        }
        // await deletes(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        // handleSearch();
      })
      .catch(() => cancel?.());
  }
  function operation() {
    addDialog({
      title: "动作参数",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      hideFooter: true,
      props: { formInline: handleRow() },
      contentRenderer: () => operationForm
    });
  }
  // function handleSearchArrList2() {
  //   dataList.value = dataListSave.value.filter(item => {
  //     if (!query.actionType && !query.blockingType) {
  //       return true;
  //     }
  //     return (
  //       item.actionType === query.actionType ||
  //       item.blockingType === query.blockingType
  //     );
  //   });
  // }
  function handleSearchArrList() {
    dataList.value = dataListSave.value.filter(
      item =>
        (!query.actionType && !query.blockingType) ||
        item.actionType === query.actionType ||
        item.blockingType === query.blockingType
    );
  }

  onMounted(async () => {
    handleSearch();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    pagination,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleDetail,
    handleAdd,
    handleEdit,
    handleDelete,
    handleUpdate,
    handleSearchArrList
  };
}
