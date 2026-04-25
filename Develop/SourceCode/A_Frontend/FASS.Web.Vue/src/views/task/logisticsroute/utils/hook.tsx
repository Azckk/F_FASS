import { type Ref, h, ref, reactive, onMounted } from "vue";
//@ts-ignore
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import {
  GetTypeListToSelect,
  getPage,
  addOrUpdate,
  deletes,
  GetAreaListToSelect
} from "@/api/task/logisticsroute";
import { type FormItemProps } from "../utils/types";
import editForm from "../form.vue";
import { GetDictItemInLocal } from "@/utils/auth";
import process from "../../tasktemplateprocess/index.vue";
import { useMyI18n } from "@/plugins/i18n";

export function useHook(tableRef?: Ref | undefined) {
  const { t } = useMyI18n();
  const query = reactive({
    code: "",
    name: ""
  });
  const formRef = ref();
  const TaskTemplateTypeList = ref([]);
  const nodeList = ref(); //站点
  const loading = ref(true);
  const carTypeList = ref([]);
  const dataList = ref([]);
  const selection = ref([]);
  const pageParam = ref({});
  const areaCode = ref();
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
      headerRenderer: () => t("table.index"),
      label: "table.index",
      type: "index",
      width: 60
    },
    {
      headerRenderer: () => t("table.code"),
      label: "table.code",
      prop: "code",
      sortable: true
    },
    {
      headerRenderer: () => t("table.lineName"),
      label: "table.lineName",
      prop: "name",
      sortable: true
    },
    {
      headerRenderer: () => t("table.startingPointReservoirArea"),
      label: "table.startingPointReservoirArea",
      prop: "srcName",
      sortable: true
    },
    {
      headerRenderer: () => t("table.terminalReservoirArea"),
      label: "table.terminalReservoirArea",
      prop: "destName",
      sortable: true
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
    // 遍历路由数组  修改起点/终点库区的id为name
    try {
      dataList.value.forEach(route => {
        const srcArea = areaCode.value.find(
          area => area.code === route.srcAreaCode
        );
        if (srcArea) {
          route.srcName = srcArea.name;
        }
        const destArea = areaCode.value.find(
          area => area.code === route.destAreaCode
        );
        if (destArea) {
          route.destName = destArea.name;
        }
      });
    } catch (error) {}

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
    console.log(row);
    return {
      code: row?.code ?? `dx_${dayjs().format("YYMMDDHHmmss")}`,
      name: row?.name ?? "",
      // destAreaCode: row?.destAreaCode ?? "",
      // srcAreaCode: row?.srcAreaCode ?? "",
      // sortNumber: row?.sortNumber ?? 0,
      destAreaId: row?.destAreaId ?? "",
      srcAreaId: row?.srcAreaId ?? ""
      // isEnable: row?.isEnable ?? true,
      // isDelete: row?.isDelete ?? false,
      // "createBy": row?.createBy ??"",
      // "updateBy": row?.updateBy ??"",
      // "remark": row?.remark ??"",
      // "extend": row?.extend ??"",
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
            console.log("formData", formData);
            await addOrUpdate("", formData);
            message("操作成功！", { type: "success" });
            handleSearch();
            done();
          }
        });
      }
    });
  }

  async function TypeListToSelect() {
    const { data } = await GetTypeListToSelect();
    carTypeList.value = [...data];
  }

  async function GetTaskTemplateTypeList() {
    try {
      const data = await GetDictItemInLocal("TaskTemplateType");
      TaskTemplateTypeList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }

  async function getZoneDataListFn() {
    pageParam.value = handlePageParam();
    const { data } = await GetAreaListToSelect();
    areaCode.value = data.rows;
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
            console.log(" row ", row.id);
            await addOrUpdate(row?.id, formData);
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
        await deletes(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  onMounted(async () => {
    // 需要在这里获取数据
    getZoneDataListFn();
    TypeListToSelect();
    handleSearch();
    GetTaskTemplateTypeList();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    pagination,
    areaCode,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleDetail,
    handleAdd,
    handleUpdate,
    handleDelete,
    handleReset
  };
}
