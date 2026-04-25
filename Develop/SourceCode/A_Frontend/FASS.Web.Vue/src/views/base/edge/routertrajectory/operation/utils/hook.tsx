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

export function useHook(tableRef?: Ref, nodeId?: string) {
  const { t } = useMyI18n();
  const query = reactive({
    coordinatesX: "",
    coordinatesY: "",
    weight: ""
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
      headerRenderer: () => t("table.coordinatesX"),
      label: "table.coordinatesX",
      prop: "coordinatesX",
      sortable: true
    },
    {
      headerRenderer: () => t("table.coordinatesY"),
      label: "table.coordinatesY",
      prop: "coordinatesY",
      sortable: true
    },
    {
      headerRenderer: () => t("table.weight"),
      label: "table.weight",
      prop: "weight",
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
    const { data } = await GetActions(nodeId); // 暂无接口
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
            // alert(`暂无接口${formData}`); // 接口确认之后记得删除
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
  function handleEdit() {}
  function handleDelete() {}
  function handleDisable() {}
  function handleUpdate() {}
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
    handleUpdate
  };
}
