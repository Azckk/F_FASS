import { type Ref, h, ref, reactive, onMounted } from "vue";
import { ElMessageBox } from "element-plus";
import dayjs from "dayjs";
import { message } from "@/utils/message";
import { handleTree } from "@/utils/tree";
import { cloneDeep, isAllEmpty } from "@pureadmin/utils";
import { addDialog } from "@/components/ReDialog";
import { getList, addOrUpdate, deletes } from "@/api/account/org";
import { type FormItemProps } from "../utils/types";
import { usePublicHooks } from "../../hooks";
import editForm from "../form.vue";
import { useMyI18n } from "@/plugins/i18n";

export function useHook(tableRef: Ref) {
  const { t } = useMyI18n();
  const { tagStyle } = usePublicHooks();

  const query = reactive({
    code: "",
    name: ""
  });
  const formRef = ref();
  const loading = ref(true);
  const dataList = ref([]);
  const selection = ref([]);
  const columns: TableColumnList = [
    { type: "selection", align: "left" },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.name"),
      label: "table.name",
      prop: "name"
    },
    {
      headerRenderer: () => t("table.code"),
      label: "table.code",
      prop: "code"
    },
    {
      headerRenderer: () => t("table.sort"),
      label: "table.sortNumber",
      prop: "sortNumber"
    },
    {
      headerRenderer: () => t("table.createAt"),
      label: "table.createAt",
      prop: "createAt",
      formatter: ({ createAt }) => dayjs(createAt).format("YYYY-MM-DD HH:mm:ss")
    },
    // { headerRenderer: () => t("table.operation"), slot: "operation" }
  ];

  function handleSelection(val) {
    selection.value = val;
  }

  async function handleSearch() {
    loading.value = true;
    const { data } = await getList();
    let newData = data;
    if (!isAllEmpty(query.code)) {
      newData = newData.filter(item => item.code.includes(query.code));
    }
    if (!isAllEmpty(query.name)) {
      newData = newData.filter(item => item.name.includes(query.name));
    }
    dataList.value = handleTree(newData);
    loading.value = false;
  }

  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    handleSearch();
  }

  function handleParents(treeList) {
    if (!treeList || !treeList.length) {
      return;
    }
    const newTreeList = [];
    for (let i = 0; i < treeList.length; i++) {
      treeList[i].disabled = !treeList[i].isEnable;
      handleParents(treeList[i].children);
      newTreeList.push(treeList[i]);
    }
    return newTreeList;
  }

  function handleRow(row = undefined) {
    const parents = handleParents(cloneDeep(dataList.value));
    return {
      parents: parents,
      parentId: row?.parentId ?? parents[0]?.id,
      code: row?.code ?? null,
      name: row?.name ?? null,
      sortNumber: row?.sortNumber ?? 0,
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
            await addOrUpdate("", formData);
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
            await addOrUpdate(row?.id, formData);
            message("操作成功！", { type: "success" });
            handleSearch();
            done();
          }
        });
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
        await deletes(rows.map(e => e.id));
        message("操作成功！", { type: "success" });
        handleSearch();
      })
      .catch(() => cancel?.());
  }

  onMounted(() => {
    handleSearch();
  });

  return {
    query,
    loading,
    columns,
    dataList,
    handleSelection,
    handleSearch,
    handleReset,
    handleDetail,
    handleAdd,
    handleUpdate,
    handleDelete
  };
}
