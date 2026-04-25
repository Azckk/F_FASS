import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { addDialog } from "@/components/ReDialog";
import { userGetPage, userAdd, userDeletes } from "@/api/account/role";
import ActionParameterForm from "../../actionparameterform/form.vue";
import { FormProps } from "../../actionparameterform/form.vue";
// import userSelect from "../../../user/select.vue";
// actionparameterform
import { useMyI18n } from "@/plugins/i18n";

export function useHook(newFormInline:Ref) {
  const { t } = useMyI18n();
  const switchLoadMap = ref({});

  const query = reactive({
    key: "",
    value: "",
  });
  const selectRef = ref();
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
      headerRenderer: () => t("table.key"),
      label: "table.key",
      prop: "key"
    },
    {
      headerRenderer: () => t("table.value"),
      label: "table.value",
      prop: "value"
    },
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
            operator: "Equal",
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
    // pageParam.value = handlePageParam();
    dataList.value = newFormInline.value
    // const { data } = await userGetPage(keyValue.value, pageParam.value);
    // dataList.value = data.rows;
    // pagination.total = data.total;
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
      key: row?.key ?? "",
      value: row?.value ?? "",
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
      contentRenderer: () => h(ActionParameterForm, { ref: selectRef }),
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
      contentRenderer: () => h(ActionParameterForm, { ref: selectRef }),
      beforeSure: async (done, { options, index }) => {
        selectRef.value.getRef().validate(async valid => {
          if (valid) {
            const formData = options.props.formInline;
            newFormInline.value.push(formData)
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
    let row = rows[0];
    addDialog({
      title: "修改",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: handleRow(row) },
      contentRenderer: () => h(ActionParameterForm, { ref: selectRef }),
      beforeSure: async (done, { options, index }) => {
        selectRef.value.getRef().validate(async valid => {
          if (valid) {
            const formData = options.props.formInline;
            row = Object.assign(row,formData)
            handleSearch();
            done();
          }
        });
      }
    });
  }

  // handleUpdate


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
        rows.forEach(row => {
          const index = newFormInline.value.findIndex(
            item => item.key === row.key && item.value === row.value
          );
          if (index !== -1) {
            newFormInline.value.splice(index, 1);
          }
        });
        // await userDeletes(keyValue.value, rows);
        // message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
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
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleUpdate,
    handleSearch,
    handleReset,
    handleDetail,
    handleAdd,
    handleDelete
  };
}
