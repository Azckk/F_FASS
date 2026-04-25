import { type Ref, h, ref, reactive, onMounted } from "vue";
import { GetListToSelectByCarTypeCode } from "@/api/data/caraction";
import { GetDictItemInLocal } from "@/utils/auth";
import { addDialog } from "@/components/ReDialog";
// import { getList } from "@/api/account/org";
import { type FormItemProps } from "../utils/types";
import ActionParameterIndex from "../../actionparameterindex/index.vue";
import { useMyI18n } from "@/plugins/i18n";
export function useHook(newFormInline: FormItemProps) {
  const { t } = useMyI18n();
  const ListToSelectByCarTypeCode = ref([]);
  const ListActionBlockingType = ref([]);
  const selectRef = ref();
  // const { switchStyle } = usePublicHooks();
  async function getListToSelectByCarTypeCode() {
    const { data } = await GetListToSelectByCarTypeCode();
    ListToSelectByCarTypeCode.value = [...data];
  }
  async function GetTaskTemplateTypeList() {
    try {
      const data = await GetDictItemInLocal("ActionBlockingType");
      ListActionBlockingType.value = [...data];
    } catch (error) {
      console.error("Error:", error);
    }
  }
  async function openActionParameterIndex() {
    addDialog({
      title: "添加",
      width: "60%",
      alignCenter: true,
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      props: { formInline: newFormInline.actionParameters },
      contentRenderer: () => h(ActionParameterIndex, { ref: selectRef }),
      beforeSure: async (done, { options, index }) => {
        console.log(" options.props.formInline   ", options.props.formInline);
        const formData = options.props.formInline;
        done();
        // console.log(" formData is" , formData , newFormInline)
        // const rows = selectRef.value.tableRef.getTableRef().getSelectionRows();
        // await userAdd(keyValue.value, rows);
        // message("操作成功！", { type: "success" });
        // handleSearch();
        // done();
      }
    });
  }

  onMounted(async () => {
    getListToSelectByCarTypeCode();
    GetTaskTemplateTypeList();
  });

  return {
    ListToSelectByCarTypeCode,
    ListActionBlockingType,
    openActionParameterIndex
  };
}
