import { type Ref, h, ref, reactive, onMounted } from "vue";
import { message } from "@/utils/message";
import type { PaginationProps } from "@pureadmin/table";
import { deviceDetection } from "@pureadmin/utils";
import { GetDictItemInLocal } from "@/utils/auth";
import { addDialog } from "@/components/ReDialog";
import { getPage, Add } from "@/api/mobile/flowcartasksingle";
import { TaskTemplateListToSelect } from "@/api/flow/taskinstance";
import editForm from "../form.vue";
// import locationFrom from "../locationFrom.vue";
import { useMyI18n } from "@/plugins/i18n";

export function useHook(tableRef?: Ref) {
  const isEnglish = new RegExp("[A-Za-z]+");
  const { t } = useMyI18n();
  // const { switchStyle } = usePublicHooks();
  const query = reactive({
    code: "",
    name: "",
    kind: "",
    type: "",
    state: "",
    taskTemplateId: "Single"
  });
  const formRef = ref();
  const loading = ref(true);
  const dataList = ref([]);
  const carList = ref();
  const nodeList = ref();
  const selection = ref([]);
  const pageParam = ref({});
  const DictItemInLocalList = ref();
  const TaskTemplateList = ref([]); //模板选择
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
      headerRenderer: () => t("table.template"),
      label: "table.template",
      prop: "taskTemplateCode",
      sortable: true,
      formatter: ({ taskTemplateCode }) => {
        // if (taskTemplateCode) {
        //   return taskTemplateCode;
        // }
        // return "--未指定--";
        let dictResult = TaskTemplateList.value.filter(
          x => x.code === taskTemplateCode
        );
        if (dictResult && dictResult[0]) {
          if (!isEnglish.test(t("table.type"))) {
            return dictResult[0].name;
          } else {
            return dictResult[0].code;
          }
        }
        return "--未指定--";
      }
    },
    {
      headerRenderer: () => t("table.car"),
      label: "table.car",
      prop: "carCode",
      sortable: true
    },
    {
      headerRenderer: () => t("table.code"),
      label: "table.code",
      prop: "code",
      sortable: true
    },
    {
      headerRenderer: () => t("table.status"),
      label: "table.status",
      prop: "state",
      sortable: true,
      formatter: ({ state }) => {
        if (DictItemInLocalList.value) {
          let dictResult = DictItemInLocalList.value.filter(
            x => x.code === state
          );
          console.log(dictResult);

          if (isEnglish.test(t("table.status"))) {
            return dictResult[0].code;
          } else {
            return dictResult[0].name;
          }
        }
      }
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
    const { data } = await getPage(pageParam.value);
    dataList.value = data.rows;
    pagination.total = data.total;
    loading.value = false;
  }
  async function GetTaskInstanceStateType() {
    try {
      const data = await GetDictItemInLocal("TaskInstanceState");
      DictItemInLocalList.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }
  async function GetTaskTemplateListToSelect() {
    const { data } = await TaskTemplateListToSelect();
    TaskTemplateList.value = [...data];
    // console.log("   TaskTemplateList.value is" ,  TaskTemplateList.value   )
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
      name: row?.name ?? null,
      code: row?.code ?? null,
      isEnable: row?.isEnable ?? true,
      isLock: row?.isLock ?? true,
      sequenceId: row?.sequenceId ?? null,
      nodeDescription: row?.nodeDescription ?? null,
      kind: row?.kind ?? null,
      type: row?.type ?? null,
      x: row?.x ?? null,
      y: row?.y ?? null,
      theta: row?.theta ?? null,
      allowedDeviationXY: row?.allowedDeviationXY ?? null,
      allowedDeviationTheta: row?.allowedDeviationTheta ?? null,
      mapId: row?.mapId ?? null
    };
  }

  // 添加
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
        const formData = options.props.formInline;
        console.log(formData);
        let data = {
          carId: formData.carId,
          targetNodeId: formData.targetNodeId
        };
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            let res = await Add(data);
            if (res.code == 200) {
              message("操作成功！", { type: "success" });
              handleSearch();
              done();
            } else {
              message("操作失败！", { type: "warning" });
            }
          }
        });
      }
    });
  }

  onMounted(async () => {
    handleSearch();
    // getCarListFn();
    // GetNodeListToSelectFn();
    GetTaskTemplateListToSelect();
    GetTaskInstanceStateType();
    // const { data } = await getList();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    pagination,
    carList,
    nodeList,
    deviceDetection,
    handleSelection,
    handlePageSize,
    handlePageCurrent,
    handleSearch,
    handleReset,
    handleAdd,
    DictItemInLocalList
  };
}
