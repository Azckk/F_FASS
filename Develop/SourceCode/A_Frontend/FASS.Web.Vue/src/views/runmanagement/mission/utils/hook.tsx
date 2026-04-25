import { ref, onMounted, onUnmounted, h } from "vue";
import { message } from "@/utils/message";
import {
  MissionGetPage,
  GetMissionFields,
  GetMissionStatus,
  GetMissionMethods,
  SetMissionFields,
  DeleteMissionField,
  ExecuteMissionMethod
} from "@/api/runmanagement/mission";
import { useMyI18n } from "@/plugins/i18n";
import { addDialog } from "@/components/ReDialog";
import type { FormItemProps } from "../utils/types";
import editForm from "../form.vue";

export function useHook(tableRef: Ref) {
  const { t } = useMyI18n();
  const formRef = ref();
  const leftList = ref();
  const statusData = ref();
  const fieldData = ref();
  const methodsData = ref();
  const selection = ref([]);

  const activeId = ref();
  let previousLeftList = []; // 用于比较的左侧菜单数据
  let leftMenuTimer = null; // 左侧菜单定时器
  let statusDataTimer = null; // 状态数据定时器

  const methodsColumns: TableColumnList = [
    // { type: "selection", align: "left" },

    {
      headerRenderer: () => t("table.function"),
      label: "功能",
      prop: "ButtonName"
    },
    {
      headerRenderer: () => t("table.description"),
      label: "status",
      prop: "ButtonDescription"
    },
    { label: "操作", slot: "operation", minWidth: 120 }
  ];

  const columns: TableColumnList = [
    // { type: "selection", align: "left" },
    {
      headerRenderer: () => t("table.number"),
      label: "table.number",
      prop: "id",
      hide: true
    },
    {
      headerRenderer: () => t("table.fieldName"),
      label: "字段名称",
      prop: "Key"
    },
    {
      headerRenderer: () => t("table.fieldParameters"),
      label: "字段参数",
      prop: "Vaule"
    },
    { label: "操作", slot: "operation", minWidth: 120 }
  ];

  const statusColumns: TableColumnList = [
    // { type: "selection", align: "left" },

    {
      headerRenderer: () => t("table.name"),
      label: "标记",
      prop: "key"
    },
    {
      headerRenderer: () => t("table.param"),
      label: "status",
      prop: "value",
      formatter: ({ value }) => {
        if (typeof value === "string" || typeof value === "number") {
          const strValue = String(value); // 将值转换为字符串，方便正则匹配
          if (/^\d+\.0$/.test(strValue)) {
            return parseInt(strValue, 10); // 转换为整数
          }
        }
        return value; // 保持其他内容不变
      }
    }
    // { label: "操作", slot: "operation", minWidth: 120 }
  ];

  // function handleAdd() {
  //   console.log("avatar+++", activeId.value);
  // }
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
            await SetMissionFields(
              activeId.value,
              formData.key,
              formData.value
            );
            message("操作成功！", { type: "success" });
            refreshFieldData();
            done();
          }
        });
      }
    });
  }

  async function handleDelete(rows = selection.value) {
    await DeleteMissionField(activeId.value, rows[0].Key);
    message("操作成功！", { type: "success" });
    refreshFieldData();
  }

  function handleSearch() {
    refreshFieldData();
  }

  function handleUpdate(rows = selection.value) {
    if (rows.length === 0) {
      message("请至少选择一项数据再进行操作！", { type: "warning" });
      return;
    }
    const row = rows[0];
    // console.log("row11", row);

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
            await SetMissionFields(
              activeId.value,
              formData.key,
              formData.value
            );
            message("操作成功！", { type: "success" });
            refreshFieldData();
            done();
          }
        });
      }
    });
  }

  function handleSelection(val) {
    selection.value = val;
  }

  function handleRow(row = undefined) {
    return {
      key: row?.Key ?? null,
      value: row?.Vaule ?? null,
      isDisable: row ? true : false
    };
  }
  async function handleExecute(row) {
    const execute = row[0];
    // console.log("avatar", execute);
    if (execute.ParamsList.length > 0) {
      handleExecuteCarMethod(row);
    } else {
      // handleExecuteCarMethod(row);
      await ExecuteMissionMethod(
        activeId.value,
        execute.MethodName,
        execute.ParamsCount
      );
    }

    message("操作成功！", { type: "success" });
  }
  function handleExecuteCarMethod(rows = selection.value) {
    const row = rows[0];
    addDialog({
      title: "查询",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: {
        formInline: { key: row.MethodName, value: "", isDisable: true }
      },
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options, index }) => {
        const formData = options.props.formInline as FormItemProps;
        console.log("formData", formData);
        formRef.value.getRef().validate(async valid => {
          if (valid) {
            await ExecuteMissionMethod(
              activeId.value,
              formData.key,
              formData.value
            );
            message("操作成功！", { type: "success" });
            done();
          }
        });
      }
    });
  }
  function handleSerialization(MethodName, carCode, showForm?) {
    if (!showForm) {
      return {
        carCode,
        method: MethodName,
        param: null
      };
    } else {
      let where = showForm.map(item => {
        return {
          Key: item.Name,
          Value: item.value
        };
      });
      return {
        carCode,
        method: MethodName,
        param: JSON.stringify(where)
      };
    }
  }

  // 手动刷新字段数据
  async function refreshFieldData() {
    if (!activeId.value) return;
    const res = await GetMissionFields(activeId.value);
    fieldData.value = res.data;
  }

  // 手动刷新功能数据
  async function refreshMethodsData() {
    if (!activeId.value) return;
    const res = await GetMissionMethods(activeId.value);
    methodsData.value = res.data;
  }

  // 自动刷新状态数据
  async function refreshStatusData() {
    if (!activeId.value) return;
    const res = await GetMissionStatus(activeId.value);
    statusData.value = res.data;
  }

  function idsAreEqual(list1, list2) {
    const ids1 = list1
      .map(item => item.id)
      .sort()
      .join(",");
    const ids2 = list2
      .map(item => item.id)
      .sort()
      .join(",");
    return ids1 === ids2;
  }

  let isFetchingLeftMenu = false;
  // 定时刷新左侧菜单数据
  async function fetchLeftData() {
    if (isFetchingLeftMenu) {
      console.log("左侧菜单刷新中，跳过本次调用");
      return;
    }
    isFetchingLeftMenu = true; // 上锁
    try {
      const res = await MissionGetPage();

      const newList = res.data;

      leftList.value = newList;
      // 对 newList 排序后再比较，确保一致性
      const sortedNewList = newList.sort((a, b) => a.id - b.id);
      const sortedPrevList = previousLeftList.sort((a, b) => a.id - b.id);
      // 比较新旧数据，如果一致则跳过刷新右侧数据
      if (idsAreEqual(sortedNewList, sortedPrevList)) {
        // console.log("左侧菜单未变化，跳过右侧刷新");
        return;
      }
      previousLeftList = leftList.value;

      // 如果 activeId 无效或已删除，则自动选中第一个菜单项
      if (
        !activeId.value ||
        !newList.some(item => item.id === activeId.value)
      ) {
        activeId.value = newList[0]?.id;
      }

      // 左侧菜单变化时，刷新右侧字段数据和功能数据
      if (activeId.value) {
        await Promise.all([refreshFieldData(), refreshMethodsData()]);
      }
    } catch (error) {
      console.error("获取左侧菜单失败:", error);
    } finally {
      isFetchingLeftMenu = false; // 解锁
    }
  }

  // 切换左侧菜单时
  function changeData(id) {
    // if (activeId.value === id) {
    //   console.log("菜单未切换，跳过刷新");
    //   return;
    // }

    activeId.value = id;
    refreshFieldData();
    refreshMethodsData();
    refreshStatusData(); // 切换时也刷新状态数据
  }

  // 开始定时器
  function startTimers() {
    // 定时刷新左侧菜单（500ms）
    leftMenuTimer = setInterval(fetchLeftData, 500);

    // 定时刷新状态数据（500ms）
    statusDataTimer = setInterval(refreshStatusData, 500);
  }

  // 清理定时器
  function stopTimers() {
    if (leftMenuTimer) clearInterval(leftMenuTimer);
    if (statusDataTimer) clearInterval(statusDataTimer);
  }

  // 生命周期
  onMounted(() => {
    fetchLeftData(); // 初次加载左侧菜单
    refreshStatusData(); // 初次加载状态数据
    startTimers(); // 开启定时器
  });

  onUnmounted(() => {
    stopTimers(); // 清理定时器
  });

  return {
    leftList,
    fieldData,
    statusData,
    methodsData,
    methodsColumns,
    columns,
    statusColumns,
    handleExecute,
    handleAdd,
    changeData,
    activeId,
    handleUpdate,
    handleSelection,
    handleDelete,
    handleSearch
  };
}
