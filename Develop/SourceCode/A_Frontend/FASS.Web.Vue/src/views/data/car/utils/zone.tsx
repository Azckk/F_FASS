import { h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { type PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { ZoneGetPage, ZoneAdd, ZoneDelete } from "@/api/data/car";
// import storage from "../../../warehouse/area/select.vue";
import storage from "../../../base/zone/index.vue";
import { useMyI18n } from "@/plugins/i18n";
const isEnglish = new RegExp("[A-Za-z]+");
export function useHook(newFormInline: any) {
  const { t } = useMyI18n();
  const query = reactive({
    carTypeId: "",
    code: "",
    name: "",
    zoneCode: ""
  });
  const selectRef = ref();
  const CarControlTypeList = ref();
  const CarProtocolTypeList = ref();
  const CarAvoidTypeList = ref(); //避让类型
  const nodeList = ref(); //站点
  const loading = ref(true);
  const carTypeList = ref([]);
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
      headerRenderer: () => t("table.code"),
      label: "table.code",
      prop: "zoneCode"
    },
    {
      headerRenderer: () => t("table.name"),
      label: "table.name",
      prop: "zoneName"
    },
    {
      headerRenderer: () => t("table.isEnable"),
      label: "table.isEnable",
      prop: "isEnable",
      cellRenderer: ({ row }) => (
        <el-icon color={row.isEnable ? "green" : "red"} class="no-inherit">
          {row.isEnable ? (
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
    console.log(" newFormInline is", newFormInline.value.id);
    const { data } = await ZoneGetPage({
      keyValue: newFormInline.value.id,
      pageParam: pageParam.value.pageParam
    });
    dataList.value = data.rows;
    pagination.total = data.total;
    loading.value = false;
  }
  const formRef = ref();
  function handleAdd() {
    addDialog({
      title: "添加",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      contentRenderer: () => h(storage, { ref: formRef }),
      beforeSure: async done => {
        // const rows = selectRef.value.tableRef.getTableRef().getSelectionRows();
        const rows = formRef.value.getRef().getTableRef().getSelectionRows();
        console.log("rows is", rows);
        await ZoneAdd({ keyValue: newFormInline.value.id }, rows);
        handleSearch();
        done();
      }
      // beforeSure: (done, { options, index }) => {
      //   const formData = options;
      //   console.log("formData is", formData);

      //   formRef.value.getRef().validate(async valid => {
      //     // if (valid) {
      //     //   await addOrUpdate("", formData);
      //     //   message("操作成功！", { type: "success" });
      //     //   handleSearch();
      //     //   done();
      //     // }
      //   });
      // }
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
        await ZoneDelete({ keyValue: newFormInline.value.id }, rows);
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    handleSearch();
  }

  onMounted(async () => {
    // 需要在这里获取数据
    // carTypeList
    handleSearch();
  });

  return {
    query,
    loading,
    columns,
    carTypeList,
    nodeList,
    dataList,
    pagination,
    CarControlTypeList,
    CarProtocolTypeList,
    CarAvoidTypeList,
    handleReset,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleAdd,
    handleDelete
  };
}
