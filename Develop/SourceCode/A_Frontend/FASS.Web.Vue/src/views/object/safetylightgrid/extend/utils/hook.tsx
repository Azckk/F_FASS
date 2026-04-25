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
// import { GetExtends } from "@/api/base/node/index";

import {
  getPageItem,
  addOrUpdateItem,
  deletesItem
} from "@/api/object/safetylightgrid";

// import { usePublicHooks } from "../../hooks";
import editForm from "../form.vue";
export function useHook(
  tableRef: Ref,
  nodeId: string,
  keyValue?: Ref | undefined
) {
  const { t } = useMyI18n();
  const query = reactive({
    key: "",
    value: ""
  });
  const formRef = ref();
  const nodeList = ref([]);
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
      headerRenderer: () => t("通讯地址位"),
      label: "table.openCloseSignal",
      prop: "openCloseSignal",
      sortable: true
    },
    {
      headerRenderer: () => t("table.siteId"),
      label: "table.station",
      prop: "station",
      sortable: true
    },
    {
      headerRenderer: () => t("描述"),
      label: "table.remark",
      prop: "remark",
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
    const { data } = await getPageItem(keyValue.value, pageParam.value);
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
      // id: row?.id ?? "",
      openCloseSignal: row?.openCloseSignal ?? null,
      remark: row?.remark ?? null,
      station: row?.station ?? null,
      safetyLightGridsId: row?.safetyLightGridsId ?? null
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
      width: "45%",
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
      width: "45%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      // hideFooter: true,
      props: { formInline: handleRow() },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options }) => {
        const formData = options.props.formInline;
        // keyValue
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            formData.safetyLightGridsId = keyValue.value;
            // dataListSave.value.push(formData);
            // dataList.value = dataListSave.value; // 接口确认之后记得删除
            console.log("提交数据", formData);
            await addOrUpdateItem("", formData);
            message("操作成功！", { type: "success" });
            handleSearch();
            done();
          }
        });
      }
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
        await deletesItem(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  function handleUpdate(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    console.log(row);

    addDialog({
      title: "修改",
      width: "45%",
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
            console.log(formData);
            // await addOrUpdateItem("", formData);
            await addOrUpdateItem({ keyValue: row?.id }, formData);
            // await addOrUpdate("", formData);
            message("操作成功！", { type: "success" });
            handleSearch();
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
    nodeList,
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
    handleDelete,
    handleUpdate,
    handleSearchArrList
  };
}
