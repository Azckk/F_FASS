import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { addDialog } from "@/components/ReDialog";
import { userGetPage, userAdd, userDeletes } from "@/api/account/role";
import { usePublicHooks } from "../../../hooks";
import editForm from "../../../user/form.vue";
import userSelect from "../../../user/select.vue";
import { useMyI18n } from "@/plugins/i18n";

export function useHook(tableRef: Ref, keyValue: Ref) {
  const { t } = useMyI18n();
  const switchLoadMap = ref({});
  const { switchStyle } = usePublicHooks();

  const query = reactive({
    username: "",
    name: "",
    phone: "",
    mail: ""
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
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.username"),
      label: "table.username",
      prop: "username"
    },
    {
      headerRenderer: () => t("table.name"),
      label: "table.name",
      prop: "name"
    },
    {
      headerRenderer: () => t("table.gender"),
      label: "table.gender",
      prop: "gender",
      cellRenderer: ({ row, props }) => (
        <el-tag
          size={props.size}
          type={
            row.gender === "Unknown"
              ? ""
              : row.gender === "Male"
                ? "info"
                : "danger"
          }
          effect="plain"
        >
          {row.gender === "Unknown"
            ? "未知"
            : row.gender === "Male"
              ? "男"
              : "女"}
        </el-tag>
      )
    },
    {
      headerRenderer: () => t("table.phone"),
      label: "table.phone",
      prop: "phone"
    },
    {
      headerRenderer: () => t("table.mail"),
      label: "table.mail",
      prop: "mail"
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      minWidth: 120,
      formatter: ({ createAt }) => dayjs(createAt).format("YYYY-MM-DD HH:mm:ss")
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
    pageParam.value = handlePageParam();
    const { data } = await userGetPage(keyValue.value, pageParam.value);
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
      username: row?.username ?? null,
      name: row?.name ?? null,
      code: row?.code ?? null,
      nick: row?.nick ?? null,
      gender: row?.gender ?? null,
      birthday: row?.birthday ?? null,
      phone: row?.phone ?? null,
      mail: row?.mail ?? null,
      avatar: row?.avatar ?? null,
      isSystem: row?.isSystem ?? false,
      isEnable: row?.isEnable ?? true,
      remark: row?.remark ?? null
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
      width: "80%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: handleRow() },
      contentRenderer: () => h(userSelect, { ref: selectRef }),
      beforeSure: async (done, { options, index }) => {
        const rows = selectRef.value.tableRef.getTableRef().getSelectionRows();
        let idList = rows.map(e => e.id);
        await userAdd(keyValue.value, idList);
        message("操作成功！", { type: "success" });
        handleSearch();
        done();
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
        let idList = rows.map(e => e.id);
        console.log(idList);
        await userDeletes(keyValue.value, idList);
        message("操作成功！", { type: "success" });
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
    handleSearch,
    handleReset,
    handleDetail,
    handleAdd,
    handleDelete
  };
}
