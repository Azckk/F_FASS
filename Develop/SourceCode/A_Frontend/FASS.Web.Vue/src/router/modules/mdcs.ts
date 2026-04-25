import { $t } from "@/plugins/i18n";

export default {
  path: "/mdcs",
  redirect: "/mdcs/simpleMap",
  meta: {
    icon: "ri:information-line",
    showLink: false,
    // showLink: true,
    title: $t("menus.pureAbnormal"),
    rank: 9
  },
  children: [
    {
      path: "/mdcs/simpleMap",
      name: "Simple地图(test)",
      component: () => import("@/views/mdcs/monitor/index.vue"),
      meta: {
        // title: $t("menus.pureFive")
        title: "Simple地图"
      }
    }
  ]
} satisfies RouteConfigsTable;
