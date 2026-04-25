import { type Ref, h, ref, reactive, onMounted } from "vue";
//  /
import { ElMessageBox } from "element-plus";
// import dayjs from "dayjs";
import { useMyI18n } from "@/plugins/i18n";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
// import { GetActions } from "@/api/base/node";
import { GetExtends } from "@/api/base/node/index";

// import { usePublicHooks } from "../../hooks";
import editForm from "../form.vue";
import { v4 as uuidv4 } from "uuid";

export function useHook(tableRef: Ref, nodeId: string) {
  const { t } = useMyI18n();
  const query = reactive({
    key: "",
    value: ""
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
      headerRenderer: () => t("table.key"),
      label: "table.key",
      prop: "key",
      sortable: true
    },
    {
      headerRenderer: () => t("table.value"),
      label: "table.value",
      prop: "value",
      sortable: true
    }
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
    const { data } = await GetExtends(nodeId);
    dataList.value = data?.rows ?? [];
    pagination.total = parseInt(data.total);
    loading.value = false;
  }
  function handleReset(form) {
    if (!form) {
      return;
    }
    query.key = "";
    query.value = "";
    form.resetFields();
    // handleSearch();
    handleSearchArrList();
  }

  function handleRow(row = undefined) {
    return {
      nodeId: row?.nodeId ?? null,
      key: row?.key ?? null,
      value: row?.value ?? null
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
      props: { formInline: handleRow() },
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
  function handleDisable() {}

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
  function handleSearchArrList() {
    dataList.value = dataListSave.value.filter(
      item =>
        (!query.key && !query.value) ||
        item.key === query.key ||
        item.value === query.value
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
    handleDisable,
    handleUpdate,
    handleSearchArrList
  };
}
