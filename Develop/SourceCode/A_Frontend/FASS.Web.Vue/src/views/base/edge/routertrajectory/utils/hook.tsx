import { type Ref, h, ref, reactive, onMounted } from "vue";
//  /
// import { ElMessageBox } from "element-plus";
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
import actionTypeData from "./actionType.json";

export function useHook(tableRef: Ref, nodeId: string) {
  const { t, locale } = useMyI18n();
  const query = reactive({
    actionType: "",
    blockageType: ""
  });
  const formRef = ref();
  const loading = ref(true);
  const dataList = ref([]);
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
      sortable: true,
      formatter: ({ actionType }) => {
        // 字典中没有该类型，引用自定义数据（根据GetListToSelectByCarId请求回来的）
        let dictResult = actionTypeData.filter(x => x.code === actionType);
        if (locale.value == "zh") {
          return dictResult[0].name;
        } else {
          return dictResult[0].code;
        }
      }
    },
    {
      headerRenderer: () => t("table.blockageType"),
      label: "table.blockageType",
      prop: "blockageType",
      sortable: true,

      formatter: ({ blockingType }) => {
        if (locale.value == "zh") {
          if (blockingType == "NONE") {
            return "无限制";
          } else if (blockingType == "SOFT") {
            return "禁止行驶";
          } else if (blockingType == "HARD") {
            return "禁止行驶且禁止其它动作";
          } else {
            return blockingType;
          }
        } else {
          return blockingType;
        }
      }
    },
    {
      headerRenderer: () => t("table.sortNumber"),
      label: "table.sortNumber",
      prop: "sortNumber",
      sortable: true
    },
    {
      headerRenderer: () => t("table.sequenceId"),
      label: "table.sequenceId",
      prop: "sequenceId",
      sortable: true
    }
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
    pagination.total = parseInt(data.total);
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
      nodeId: row?.nodeId ?? null
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
      props: { formInline: handleRow(row) },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options }) => {
        const formData = options.props.formInline;
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            alert(`暂无接口${formData}`); // 接口确认之后记得删除
            dataList.value.push(formData); // 接口确认之后记得删除
            // await addOrUpdate("", formData);
            message("操作成功！", { type: "success" });
            // handleSearch();
            done();
          }
        });
      }
    });
  }
  function handleDelete() {}

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
    handleDelete,
    handleUpdate
  };
}
