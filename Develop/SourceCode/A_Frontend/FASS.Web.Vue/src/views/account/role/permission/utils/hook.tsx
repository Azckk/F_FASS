import { type Ref, ref, onMounted } from "vue";
import { permissionGetTree, permissionUpdate } from "@/api/account/role";

export function useHook(treeRef: Ref, keyValue: Ref) {
  const query = ref();
  const loading = ref(true);
  const treeIds = ref([]);
  const treeData = ref([]);
  const treeProps = {
    value: "id",
    label: "name",
    children: "children"
  };

  function handleSearch(value: string = undefined) {
    loading.value = true;
    treeRef.value!.filter(value);
    loading.value = false;
  }

  function handleFilter(value: string, node) {
    return node.name.includes(value);
  }

  async function handleUpdate() {
    const data = treeRef.value!.getCheckedNodes(false, true);
    // const halfCheckedNodes = treeRef.value.getHalfCheckedNodes(); // 获取部分选中的父节点
    // if (halfCheckedNodes.length > 0) {
    // data.push(...halfCheckedNodes);
    // }
    const ids = data.map(item => item.id);
    // console.log(ids);
    await permissionUpdate(keyValue.value, ids);
  }

  const TreeForEach = (treeData, func) => {
    treeData.forEach(item => {
      func(item);
      if (item.children && item.children.length > 0) {
        TreeForEach(item.children, func);
      }
    });
  };

  onMounted(async () => {
    const { data } = await permissionGetTree(keyValue.value);
    treeData.value = data;
    TreeForEach(data, node => {
      if (node.checked === true && node.children <= 0) {
        treeIds.value.push(node.id);
      }
    });
    treeRef.value.setCheckedKeys(treeIds.value);
  });

  return {
    query,
    treeData,
    treeProps,
    handleSearch,
    handleFilter,
    handleUpdate
  };
}
