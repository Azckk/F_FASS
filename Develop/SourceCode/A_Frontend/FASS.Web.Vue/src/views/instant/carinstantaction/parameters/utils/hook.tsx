import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  DeleteParameters,
  deletes,
  enable,
  disable,
  GetListToSelectByCarId,
  GetParametersPage,
  addOrUpdateParameters
} from "@/api/instant/carinstantaction";
// import { getList } from "@/api/account/org";
import { type FormItemProps } from "../utils/types";
// import { usePublicHooks } from "../../hooks";
import editForm from "../form.vue";
import { useMyI18n } from "@/plugins/i18n";
import { GetDictItemInLocal } from "@/utils/auth";
export function useHook(parametersId) {
  const isEnglish = new RegExp("[A-Za-z]+");
  const { t } = useMyI18n();
  const switchLoadMap = ref({});
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    key: "",
    value: ""
  });
  const formRef = ref();
  const loading = ref(true);
  const carList = ref([]);
  const stateList = ref([]);
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
    { type: "selection", align: "left", sortable: true },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
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
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      sortable: true,
      formatter: ({ createAt }) => dayjs(createAt).format("YYYY-MM-DD HH:mm:ss")
    }
    // {
    //   headerRenderer: () => t("table.operation"),
    //   label: "table.operation",
    //   slot: "operation",
    //   minWidth: 120,
    //   sortable: true
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
        order: [{ field: "sortNumber", sequence: "asc" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }
  const queryCar = reactive({
    carTypeId: "",
    code: "",
    name: ""
  });
  function handkleCarPageParam() {
    return {
      pageParam: JSON.stringify({
        where: Object.keys(queryCar)
          .filter(e => queryCar[e])
          .map(e => ({
            logic: "And",
            field: e,
            operator: "Contains",
            value: queryCar[e]
          })),
        order: [{ field: "sortNumber", sequence: "asc" }],
        number: pagination.currentPage,
        size: pagination.pageSize
      })
    };
  }

  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    const { data } = await GetParametersPage({
      carInstantActionId: parametersId.id,
      pageParam: pageParam.value.pageParam
      // parametersId, pageParam.value
    });
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
    console.log("parametersId.id is", parametersId);
    return {
      key: row?.key ?? null,
      value: row?.value ?? null,
      actionId: row?.actionId ?? parametersId.id
      // key: row?.key ?? null,
      // value: row?.value ?? null,
      // sortNumber: row?.sortNumber ?? 0,
      // isEnable: row?.isEnable !== undefined ? row.isEnable : true,
      // isDelete: row?.isDelete !== undefined ? row.isDelete : false,
      // actionId: row?.actionId ?? parametersId
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
      closeOnClickModal: false,
      props: { formInline: handleRow() },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options, index }) => {
        const formData = options.props.formInline as FormItemProps;
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            await addOrUpdateParameters("", formData);
            message("操作成功！", { type: "success" });
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
    const row = rows[0];
    addDialog({
      title: "修改",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: handleRow(row) },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options, index }) => {
        const formData = options.props.formInline as FormItemProps;
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            console.log(formData);
            await addOrUpdateParameters(row.id, formData);
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
        await DeleteParameters(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleEnable(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await enable(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function handleDisable(rows = selection.value, cancel = undefined) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("是否确认操作？", "提示", {
      type: "warning",
      draggable: true
    })
      .then(async () => {
        await disable(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  async function TypeListToSelect() {
    // const { data } = await GetTypeListToSelect();
    // carTypeList.value = [...data];
  }
  async function getStateList() {
    let res = await GetDictItemInLocal("WorkState");
    stateList.value = res;
  }

  let operationTypeList = ref([]);
  async function getListToSelect() {
    let res = await GetListToSelectByCarId();
    operationTypeList.value = res;
  }

  onMounted(async () => {
    handleSearch();
    TypeListToSelect();
    // getStateList();
    // getCarList();
    // const { data } = await getList();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    carList,
    pagination,
    stateList,
    operationTypeList,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleDetail,
    handleAdd,
    handleUpdate,
    handleDelete,
    handleEnable,
    handleDisable,
    getListToSelect
  };
}
