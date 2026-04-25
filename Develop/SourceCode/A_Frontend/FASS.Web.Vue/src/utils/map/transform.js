import { v4 as uuidv4 } from "uuid";
import dayjs from "dayjs";

const normMap = {
  index: 0,
  id: "35481dfc-0f1f-482c-a4e6-6e7bb8b5a374",
  kind: "Map",
  type: "MapRect",
  base: {
    visible: true,
    size: {
      w: 32450,
      h: 22500
    },
    globalAlpha: 1,
    lineWidth: 0,
    lineDash: [0],
    strokeStyle: "#ffffff",
    fillStyle: "#CFD6E0",
    center: {
      x: 16225,
      y: 11250
    },
    version: "1.0"
  },
  image: {
    visible: true,
    globalAlpha: 1,
    src: ""
  },
  extends: []
};
const storage = {
  index: 0,
  id: "e8e6307d-fabe-455f-b656-faa082788848",
  kind: "Node",
  type: "NodeRect",
  base: {
    visible: true,
    point: {
      x: 6292.61667283779,
      y: 3296.4898701731427
    },
    size: {
      w: 1070,
      h: 600
    },
    globalAlpha: 1,
    lineWidth: 1,
    lineDash: [0],
    strokeStyle: "#808080",
    fillStyle: "#808080",
    center: {
      x: 6463.8593794754925,
      y: 3404.8524812105443
    }
  },
  code: {
    visible: true,
    globalAlpha: 1,
    font: "150px Arial",
    fillStyle: "#ffffff",
    text: "1"
  },
  name: {
    visible: true,
    globalAlpha: 1,
    font: "150px Arial",
    fillStyle: "#ffffff",
    text: "A-A0-1"
  },
  image: {
    visible: false,
    globalAlpha: 1,
    src: ""
  },
  lock: {
    enable: false
  },
  data: {
    nodeId: "e8e6307d-fabe-455f-b656-faa082788848",
    sequenceId: 0,
    nodeDescription: "",
    released: true,
    nodePosition: {
      x: 6463.8593794754925,
      y: 3404.8524812105443,
      mapId: "35481dfc-0f1f-482c-a4e6-6e7bb8b5a374"
    },
    actions: []
  },
  extends: []
};
const normNode = {
  index: 1,
  id: "4dfe51a9-1858-4fd3-9139-4c50c8c360fe",
  kind: "Node",
  type: "NodeEllipse",
  base: {
    visible: true,
    point: {
      x: 34061.81030393874,
      y: 10271.804298656136
    },
    size: {
      w: 150,
      h: 150
    },
    globalAlpha: 1,
    lineWidth: 20,
    lineDash: [0],
    strokeStyle: "#880CE0",
    fillStyle: "#FFFFFF",
    center: {
      x: 34111.81030393874,
      y: 10321.804298656136
    }
  },
  code: {
    visible: true,
    globalAlpha: 1,
    font: "50px Arial",
    fillStyle: "#880CE0",
    text: "V288B"
  },
  name: {
    visible: true,
    globalAlpha: 1,
    font: "30px Arial",
    fillStyle: "#880CE0",
    text: "站点"
  },
  image: {
    visible: false,
    globalAlpha: 1,
    src: ""
  },
  lock: {
    enable: false
  },
  data: {
    nodeId: "4dfe51a9-1858-4fd3-9139-4c50c8c360fe",
    sequenceId: 0,
    nodeDescription: "",
    released: true,
    nodePosition: {
      x: 34111.81030393874,
      y: 10321.804298656136,
      mapId: "35481dfc-0f1f-482c-a4e6-6e7bb8b5a374"
    },
    actions: []
  },
  extends: []
};
const normEdge = {
  index: 7,
  id: "6c6968ac-4c9b-4746-9c0b-8d7a7c1383b0",
  kind: "Edge",
  type: "EdgeBeeline",
  base: {
    visible: true,
    point: {
      x: 6984.227373759469,
      y: 3342.3407710697325
    },
    size: {
      w: 103.89902015059555,
      h: 103.89902015059555
    },
    globalAlpha: 1,
    lineWidth: 150,
    lineDash: [0],
    strokeStyle: "#FFFFFF",
    fillStyle: "#FFFFFF",
    center: {
      x: 7189.253839595641,
      y: 3398.5798160833115
    },
    isOneway: false,
    width: 0,
    startNode: {
      id: "d39b07c3-8a6a-4515-a912-d665eb4f708a"
    },
    endNode: {
      id: "0f781533-62bb-44a8-89f4-3b340548ce06"
    }
  },
  code: {
    text: "80-6"
  },
  name: {
    text: ""
  },
  lock: {
    enable: false
  },
  data: {
    edgeId: "6c6968ac-4c9b-4746-9c0b-8d7a7c1383b0",
    sequenceId: 0,
    edgeDescription: "",
    released: true,
    startNodeId: "d39b07c3-8a6a-4515-a912-d665eb4f708a",
    endNodeId: "0f781533-62bb-44a8-89f4-3b340548ce06",
    maxSpeed: 0,
    maxHeight: 0,
    minHeight: 0,
    orientation: 0,
    orientationType: "TANGENTIAL",
    direction: "",
    rotationAllowed: true,
    maxRotationSpeed: 0,
    length: 765.685224080423,
    trajectory: {
      degree: 0,
      knotVector: [0],
      controlPoints: []
    },
    actions: []
  },
  extends: []
};
const normCircularArc = {
  index: 12,
  id: "506ee29c-ef30-46a6-ae59-23a5f6579805",
  kind: "Edge",
  type: "EdgeArc",
  base: {
    visible: true,
    point: {
      x: 5284.8949236887165,
      y: 6265.1488617454725
    },
    size: {
      w: 103.89902015059555,
      h: 103.89902015059555
    },
    globalAlpha: 1,
    lineWidth: 150,
    lineDash: [0],
    strokeStyle: "#FFFFFF",
    fillStyle: "#FFFFFF",
    center: {
      x: 7021.746104194335,
      y: 4017.7871142386084
    },
    isOneway: false,
    width: 0,
    startNode: {
      id: "198eb4a7-9dd6-498d-8bae-6b8f75b92cb0"
    },
    endNode: {
      id: "4df79ee3-bb83-4495-aeb8-75a3aa5a362a"
    },
    p1: {
      x: 7790.056622282211,
      y: 7277.753715329867,
      x1: 7790.056622282211,
      y1: 7277.753715329867,
      x2: 7766.056622282211,
      y2: 4755.753715329867
    }
  },
  code: {
    text: "8-10"
  },
  name: {
    text: ""
  },
  lock: {
    enable: false
  },
  data: {
    edgeId: "506ee29c-ef30-46a6-ae59-23a5f6579805",
    sequenceId: 0,
    edgeDescription: "",
    released: true,
    startNodeId: "198eb4a7-9dd6-498d-8bae-6b8f75b92cb0",
    endNodeId: "4df79ee3-bb83-4495-aeb8-75a3aa5a362a",
    maxSpeed: 0,
    maxHeight: 0,
    minHeight: 0,
    orientation: 0,
    orientationType: "TANGENTIAL",
    direction: "",
    rotationAllowed: true,
    maxRotationSpeed: 0,
    length: 10491.4819771025,
    trajectory: {
      degree: 1,
      knotVector: [0],
      controlPoints: [
        {
          x: 7790.056622282211,
          y: 7277.753715329867
        }
      ]
    },
    actions: []
  },
  extends: []
};
const normEdgeQuadratic = {
  index: 2,
  id: "1bcfecc4-2c85-4a73-b119-f49fa5e4bf59",
  kind: "Edge",
  type: "EdgeQuadratic",
  base: {
    visible: true,
    point: {
      x: 450.2614235012283,
      y: 340.4972472534005
    },
    size: {
      w: 2.686452976210051,
      h: 2.686452976210051
    },
    globalAlpha: 1,
    lineWidth: 150,
    lineDash: [0],
    strokeStyle: "#FFFFFF",
    fillStyle: "#FFFFFF",
    center: {
      x: 516.1926536421905,
      y: 311.4613140335655
    },
    isOneway: false,
    width: 0,
    startNode: {
      id: "381e805c-d95f-462b-a718-8f26d916e73e"
    },
    endNode: {
      id: "59d6e888-1c33-4156-947a-61f695e34319"
    },
    p1: {
      x1: 469.6219174801696,
      y1: 423.14321611641924,
      x2: 556.6219174801696,
      y2: 204.14321611641924,
      x: 469.6219174801696,
      y: 423.14321611641924
    }
  },
  code: {
    text: "1-2"
  },
  name: {
    text: ""
  },
  lock: {
    enable: false
  },
  data: {
    edgeId: "1bcfecc4-2c85-4a73-b119-f49fa5e4bf59",
    sequenceId: 0,
    edgeDescription: "",
    released: true,
    startNodeId: "381e805c-d95f-462b-a718-8f26d916e73e",
    endNodeId: "59d6e888-1c33-4156-947a-61f695e34319",
    maxSpeed: 0,
    maxHeight: 0,
    minHeight: 0,
    orientation: 0,
    orientationType: "TANGENTIAL",
    direction: "",
    rotationAllowed: true,
    maxRotationSpeed: 0,
    length: 275.812439386991,
    trajectory: {
      degree: 1,
      knotVector: [0],
      controlPoints: [
        {
          x: 469.6219174801696,
          y: 423.14321611641924
        }
      ]
    },
    actions: []
  },
  extends: []
};
const normEdgeBezier = {
  index: 5,
  id: "a603781b-c4f0-4346-881a-fda2f4b3ad12",
  kind: "Edge",
  type: "EdgeBezier",
  base: {
    visible: true,
    point: {
      x: 395.1704156693407,
      y: 336.5853906255138
    },
    size: {
      w: 2.686452976210051,
      h: 2.686452976210051
    },
    globalAlpha: 1,
    lineWidth: 150,
    lineDash: [0],
    strokeStyle: "#FFFFFF",
    fillStyle: "#FFFFFF",
    center: {
      x: 419.22160751011256,
      y: 385.2436317427552
    },
    isOneway: false,
    width: 0,
    startNode: {
      id: "381e805c-d95f-462b-a718-8f26d916e73e"
    },
    endNode: {
      id: "99efd013-5156-438d-9993-f841978e0d38"
    },
    p1: {
      x1: 353.6219174801696,
      y1: 335.14321611641924,
      x2: 468.6219174801696,
      y2: 320.14321611641924,
      x: 353.6219174801696,
      y: 335.14321611641924
    },
    p2: {
      x1: 360.22160751011256,
      y1: 444.2436317427552,
      x2: 478.22160751011256,
      y2: 444.2436317427552,
      x: 478.22160751011256,
      y: 444.2436317427552
    }
  },
  code: {
    text: "1-3"
  },
  name: {
    text: ""
  },
  lock: {
    enable: false
  },
  data: {
    edgeId: "a603781b-c4f0-4346-881a-fda2f4b3ad12",
    sequenceId: 0,
    edgeDescription: "",
    released: true,
    startNodeId: "381e805c-d95f-462b-a718-8f26d916e73e",
    endNodeId: "99efd013-5156-438d-9993-f841978e0d38",
    maxSpeed: 0,
    maxHeight: 0,
    minHeight: 0,
    orientation: 0,
    orientationType: "TANGENTIAL",
    direction: "",
    rotationAllowed: true,
    maxRotationSpeed: 0,
    length: 244.3960116826732,
    trajectory: {
      degree: 2,
      knotVector: [0],
      controlPoints: [
        {
          x: 353.6219174801696,
          y: 335.14321611641924
        },
        {
          x: 478.22160751011256,
          y: 444.2436317427552
        }
      ]
    },
    actions: []
  },
  extends: []
};
function findNodeIdByCode(nodes, track, startNode = true) {
  const code =
    track.direction !== "b2a"
      ? startNode
        ? track.a
        : track.b
      : startNode
        ? track.b
        : track.a;
  // const code = startNode ? track.a : track.b;
  const node = nodes.find(node => node.id === String(code));
  return node ? node.id : null;
}

let AbsMinX;
let AbsMaxX;
let AbsMinY;
let AbsMaxY;
let MapPaddingLeft;
let MapPaddingTop;
let normNodeW = normNode.base.size.w / 2;
let normNodeH = normNode.base.size.h / 2;
// let now = dayjs();
// let formattedDate = now.format("YYYYMMDDHHmmss");
let formattedDate = "";
// console.log(formattedDate); // 格式: "YYYY-MM-DDTHH:mm:ss.sss"
let dydwd;
const mapTransform = {
  init: (Json, storages) => {
    let newJson = {};
    let mapJson = Json;
    mapTransform.changeMap(mapJson, newJson);
    mapTransform.changeSites(mapJson, newJson, storages);
    mapTransform.changeTracks(mapJson, newJson);
    mapTransform.changeCircularArcTracks(mapJson, newJson);
    mapTransform.changeBezierTracks(mapJson, newJson);
    // console.log("newJson is", mapJson, newJson);
    return newJson;
  },
  changeMap: (mapJson, newJson) => {
    newJson.map = { ...normMap, id: uuidv4() }; // 假设传入mapJson有map属性
    // newJson.map = { ...normMap, id: formattedDate }; // 假设传入mapJson有map属性
    // console.log("Map changed:", mapJson, newJson);
  },
  changeSites: (mapJson, newJson, storages) => {
    // 提取所有x和y的值
    const xValues = mapJson.sites.map(site => site.x);
    const yValues = mapJson.sites.map(site => -site.y); // 对y取负
    // 获取x的最小值和最大值
    const minX = Math.min(...xValues);
    const maxX = Math.max(...xValues);
    // 获取y取负后的最小值和最大值
    const minY = Math.min(...yValues);
    const maxY = Math.max(...yValues);
    AbsMinX = Math.abs(minX);
    AbsMaxX = Math.abs(maxX);
    AbsMinY = Math.abs(minY);
    AbsMaxY = Math.abs(maxY);
    MapPaddingLeft = (AbsMinX + AbsMaxX) * 0.1;
    MapPaddingTop = (AbsMinY + AbsMaxY) * 0.1;
    newJson.map.base.size.w =
      (AbsMinX + AbsMaxX) * 1.2 + 2 * normNode.base.size.w;
    newJson.map.base.size.h =
      (AbsMinY + AbsMaxY) * 1.2 + 2 * normNode.base.size.h;
    newJson.map.base.center.x = AbsMinX + AbsMaxX / 2;
    newJson.map.base.center.y = AbsMinY + AbsMaxY / 2;
    let AbsExtend = {
      AbsMinX: AbsMinX + MapPaddingLeft,
      AbsMaxX: AbsMaxX,
      AbsMinY: AbsMinY + MapPaddingTop,
      AbsMaxY: AbsMaxY,
      // normNodeW: normNode.base.size.w / 2,
      // normNodeH: normNode.base.size.h / 2
      normNodeW: 0,
      normNodeH: 0
    };
    newJson.map.extends[0] = {
      key: "Abs",
      value: JSON.stringify(AbsExtend)
    };
    // newJson.map.extends[1] = {
    //   key: "AbsMaxX",
    //   value: AbsMaxX + ""
    // };
    // newJson.map.extends[2] = {
    //   key: "AbsMinY",
    //   value: AbsMinY + ""
    // };
    // newJson.map.extends[3] = {
    //   key: "AbsMaxY",
    //   value: AbsMaxY + ""
    // };
    // newJson.map.extends.push(AbsMinX, AbsMaxX, AbsMinY, AbsMaxY);
    const storageMap = new Map(storages.map(item => [item.nodeId, item]));
    // console.log("storages is", storages);
    // 2. 遍历 mapJson.nodes，更新匹配的节点
    // mapJson.nodes.forEach(node => {
    //   if (storageMap.has(node.id)) {
    //   }
    // });

    newJson.nodes = mapJson.sites.map((site, index) => {
      // let id = uuidv4();
      let id = formattedDate + site.id;
      if (
        localStorage.getItem("LoadPibraryPosition") == "true" &&
        (storageMap.has(id) || site?.fields?.shelf === "true")
      ) {
        const storageItem = storageMap.get(id);
        let node = {
          ...storage, // 为每个site生成一个新的UUID
          ...{
            index,
            id: site.id + "",
            base: {
              visible: true,
              point: {
                x: site.x + AbsMinX + MapPaddingLeft - storage.base.size.w / 2,
                y:
                  site.y * -1 +
                  AbsMinY +
                  MapPaddingTop -
                  storage.base.size.h / 2
              },
              size: {
                w: storage.base.size.w,
                h: storage.base.size.h
              },
              globalAlpha: storage.base.globalAlpha,
              lineWidth: storage.base.lineWidth,
              lineDash: [0],
              strokeStyle: storage.base.strokeStyle,
              fillStyle: storage.base.fillStyle,
              center: {
                x: site.x + 100 + AbsMinX + MapPaddingLeft,
                y: site.y * -1 + 100 + AbsMinY + MapPaddingTop
              }
            },
            data: {
              nodeId: site.id + "",
              sequenceId: 0,
              nodeDescription: "",
              released: true,
              nodePosition: {
                x: site.x + 100 + AbsMinX + MapPaddingLeft,
                y: site.y * -1 + AbsMinY + MapPaddingTop,
                mapId: newJson.map.id
              },
              actions: []
            },
            code: {
              visible: true,
              globalAlpha: 1,
              font: storage.code.font,
              fillStyle: storage.code.fillStyle,
              text: site.id + ""
            },
            name: {
              visible: true,
              globalAlpha: 1,
              font: "150px Arial",
              fillStyle: "#FFFFFF",
              text: storageItem?.name || site?.name
            },
            fields: {
              type: site.type,
              x: site.x,
              y: site.y,
              id: site.id
            }
          }
        };
        // console.log("node is", node);
        return node;
      } else {
        if (site?.fields?.charge === "true") {
          return {
            ...normNode, // 为每个site生成一个新的UUID
            ...{
              index,
              id,
              base: {
                visible: true,
                point: {
                  x: site.x + AbsMinX + MapPaddingLeft - normNodeW,
                  y: site.y * -1 + AbsMinY + MapPaddingTop - normNodeH
                },
                size: {
                  w: normNode.base.size.w,
                  h: normNode.base.size.h
                },
                globalAlpha: normNode.base.globalAlpha,
                lineWidth: normNode.base.lineWidth,
                lineDash: [0],
                strokeStyle: "#25942C",
                fillStyle: "#4ED908",
                center: {
                  x: site.x + 100 + AbsMinX + MapPaddingLeft,
                  y: site.y * -1 + 100 + AbsMinY + MapPaddingTop
                }
              },
              data: {
                nodeId: id,
                sequenceId: 0,
                nodeDescription: "",
                released: true,
                nodePosition: {
                  x: site.x + 100 + AbsMinX + MapPaddingLeft,
                  y: site.y * -1 + AbsMinY + MapPaddingTop,
                  mapId: newJson.map.id
                },
                actions: []
              },
              code: {
                visible: false,
                globalAlpha: 1,
                font: "50px Arial",
                fillStyle: "#FFFFFF",
                text: id + ""
              },
              name: {
                visible: true,
                globalAlpha: 1,
                font: "30px Arial",
                fillStyle: "#FFFFFF",
                text: "充电站"
              },
              fields: {
                type: site.type,
                x: site.x,
                y: site.y,
                id: site.id
              }
            }
          };
        } else if (site?.fields?.standby === "true") {
          return {
            ...normNode, // 为每个site生成一个新的UUID
            ...{
              index,
              id,
              base: {
                visible: true,
                point: {
                  x: site.x + AbsMinX + MapPaddingLeft - normNodeW,
                  y: site.y * -1 + AbsMinY + MapPaddingTop - normNodeH
                },
                size: {
                  w: normNode.base.size.w,
                  h: normNode.base.size.h
                },
                globalAlpha: normNode.base.globalAlpha,
                lineWidth: normNode.base.lineWidth,
                lineDash: [0],
                strokeStyle: "#0D94BB",
                fillStyle: "#0CB0E0",
                center: {
                  x: site.x + 100 + AbsMinX + MapPaddingLeft,
                  y: site.y * -1 + 100 + AbsMinY + MapPaddingTop
                }
              },
              data: {
                nodeId: id,
                sequenceId: 0,
                nodeDescription: "",
                released: true,
                nodePosition: {
                  x: site.x + 100 + AbsMinX + MapPaddingLeft,
                  y: site.y * -1 + AbsMinY + MapPaddingTop,
                  mapId: newJson.map.id
                },
                actions: []
              },
              code: {
                visible: true,
                globalAlpha: 1,
                font: "50px Arial",
                fillStyle: "#FFFFFF",
                text: site.id + ""
              },
              name: {
                visible: true,
                globalAlpha: 1,
                font: "30px Arial",
                fillStyle: "#FFFFFF",
                text: "待命点"
              },
              fields: {
                type: site.type,
                x: site.x,
                y: site.y,
                id: site.id
              }
            }
          };
        } else {
          // 定位点 虚拟生成simple00点，用于点云定位
          dydwd = {
            ...normNode, // 为每个site生成一个新的UUID
            ...{
              index,
              id,
              base: {
                visible: false,
                point: {
                  x: 0 + AbsMinX + MapPaddingLeft - normNodeW,
                  y: 0 * -1 + AbsMinY + MapPaddingTop - normNodeH
                },
                size: {
                  w: 1,
                  h: 1
                },
                globalAlpha: 0,
                center: {
                  x: site.x + 100 + AbsMinX + MapPaddingLeft,
                  y: site.y * -1 + 100 + AbsMinY + MapPaddingTop
                }
              },
              data: {
                nodeId: id,
                sequenceId: 0,
                nodeDescription: "",
                released: true,
                nodePosition: {
                  x: site.x + 100 + AbsMinX + MapPaddingLeft,
                  y: site.y * -1 + AbsMinY + MapPaddingTop,
                  mapId: newJson.map.id
                },
                actions: []
              },
              code: {
                visible: false,
                globalAlpha: 1,
                font: normNode.code.font,
                fillStyle: normNode.code.fillStyle,
                text: ""
              },
              fields: {
                type: site.type,
                x: site.x,
                y: site.y,
                id: site.id
              },
              name: {
                visible: false,
                globalAlpha: 1,
                font: normNode.name.font,
                fillStyle: normNode.name.fillStyle,
                text: ""
              }
            }
          };
          return {
            ...normNode, // 为每个site生成一个新的UUID
            ...{
              index,
              id,
              base: {
                visible: true,
                point: {
                  x: site.x + AbsMinX + MapPaddingLeft - normNodeW,
                  y: site.y * -1 + AbsMinY + MapPaddingTop - normNodeH
                },
                size: {
                  w: normNode.base.size.w,
                  h: normNode.base.size.h
                },
                globalAlpha: normNode.base.globalAlpha,
                lineWidth: normNode.base.lineWidth,
                lineDash: [0],
                strokeStyle: normNode.base.strokeStyle,
                fillStyle: normNode.base.fillStyle,
                center: {
                  x: site.x + 100 + AbsMinX + MapPaddingLeft,
                  y: site.y * -1 + 100 + AbsMinY + MapPaddingTop
                }
              },
              data: {
                nodeId: id,
                sequenceId: 0,
                nodeDescription: "",
                released: true,
                nodePosition: {
                  x: site.x + 100 + AbsMinX + MapPaddingLeft,
                  y: site.y * -1 + AbsMinY + MapPaddingTop,
                  mapId: newJson.map.id
                },
                actions: []
              },
              code: {
                visible: true,
                globalAlpha: 1,
                font: normNode.code.font,
                fillStyle: normNode.code.fillStyle,
                text: site.id + ""
              },
              fields: {
                type: site.type,
                x: site.x,
                y: site.y,
                id: site.id
              }
            }
          };
        }
      }
    });

    newJson.dy = dydwd;
    // console.log("Sites changed:", mapJson, newJson);
  },
  changeTracks: (mapJson, newJson) => {
    newJson.edges = mapJson.tracks.map((track, index) => {
      // let id = uuidv4();
      let id = formattedDate + track.id;
      let startNodeId = findNodeIdByCode(newJson.nodes, track, true);
      let endNodeId = findNodeIdByCode(newJson.nodes, track, false);
      return {
        ...normEdge,
        ...{
          id: id,
          index,
          base: {
            visible: true,
            point: {
              x: 208.74375,
              y: 145.1
            },
            size: {
              w: normEdge.base.size.w,
              h: normEdge.base.size.h
            },
            globalAlpha: normEdge.base.globalAlpha,
            lineWidth: normEdge.base.lineWidth,
            lineDash: [0],
            strokeStyle: normEdge.base.strokeStyle,
            fillStyle: normEdge.base.fillStyle,
            center: {
              x: 234.84375,
              y: 151
            },
            isOneway: track.direction === "both" ? false : true,
            // isb2a: track.direction === "b2a",
            width: normEdge.base.width,
            startNode: {
              id: startNodeId
            },
            endNode: {
              id: endNodeId
            }
          },
          code: {
            text: track.a + "-" + track.b
          },
          data: {
            edgeId: id,
            sequenceId: 0,
            edgeDescription: "",
            released: true,
            startNodeId: startNodeId,
            endNodeId: endNodeId,
            maxSpeed: 0,
            maxHeight: 0,
            minHeight: 0,
            orientation: 0,
            orientationType: "TANGENTIAL",
            direction: "",
            rotationAllowed: true,
            maxRotationSpeed: 0,
            length: 124.95199078045935,
            trajectory: {
              degree: 0,
              knotVector: [0],
              controlPoints: []
            },
            actions: []
          }
        } // 为每个track生成一个新的UUID
      };
    });
  },
  changeCircularArcTracks: (mapJson, newJson) => {
    let length = newJson.edges.length;
    // console.log("length changed:", length, newJson);
    let circularArcTracks = mapJson.circularArcTracks.map((ArcTrack, index) => {
      // let id = uuidv4();
      let id = formattedDate + ArcTrack.id;
      let startNodeId = findNodeIdByCode(newJson.nodes, ArcTrack, true);
      let endNodeId = findNodeIdByCode(newJson.nodes, ArcTrack, false);
      return {
        index: length + index,
        id,
        kind: "Edge",
        type: "EdgeArc",
        base: {
          visible: true,
          // point: { x: ArcTrack.centerX, y: ArcTrack.centerY },
          point: {
            x: ArcTrack.centerX + AbsMinX + MapPaddingLeft,
            y: ArcTrack.centerY * -1 + AbsMinY + MapPaddingTop
          },
          size: {
            w: normCircularArc.base.size.w,
            h: normCircularArc.base.size.h
          },
          globalAlpha: normCircularArc.base.globalAlpha,
          lineWidth: normCircularArc.base.lineWidth,
          lineDash: [0],
          strokeStyle: normCircularArc.base.strokeStyle,
          fillStyle: normCircularArc.base.fillStyle,
          // center: { x: ArcTrack.centerX, y: ArcTrack.centerY },
          center: {
            x: ArcTrack.controlPointsX + AbsMinX + MapPaddingLeft,
            y: ArcTrack.controlPointsY * -1 + AbsMinY + MapPaddingTop
          },
          isOneway: ArcTrack.direction === "both" ? false : true,
          width: normCircularArc.base.width,
          // isb2a: track.direction === "b2a",
          startNode: { id: startNodeId },
          endNode: { id: endNodeId },
          p1: {
            x: ArcTrack.controlPointsX + AbsMinX + MapPaddingLeft,
            y: ArcTrack.controlPointsY * -1 + AbsMinY + MapPaddingTop
          }
        },
        code: { text: "8-10" },
        name: { text: "" },
        lock: { enable: false },
        data: {
          edgeId: id,
          sequenceId: 0,
          edgeDescription: "",
          released: true,
          startNodeId: startNodeId,
          endNodeId: endNodeId,
          maxSpeed: 0,
          maxHeight: 0,
          minHeight: 0,
          orientation: 0,
          orientationType: "TANGENTIAL",
          direction: "",
          rotationAllowed: true,
          maxRotationSpeed: 0,
          length: 1607.0559660218178,
          trajectory: {
            degree: 1,
            knotVector: [0],
            controlPoints: [
              // { x: ArcTrack.controlPointsX, y: ArcTrack.controlPointsY }
              {
                x: ArcTrack.controlPointsX + AbsMinX + MapPaddingLeft,
                y: ArcTrack.controlPointsY * -1 + AbsMinY + MapPaddingTop
              }
            ]
          },
          actions: []
        },
        extends: []
      };
    });
    newJson.edges = [...newJson.edges, ...circularArcTracks];
  },
  changeBezierTracks: (mapJson, newJson) => {
    let length = newJson.edges.length;
    let circularArcTracks = mapJson.bezierTracks.map((Track, index) => {
      if (Track.controlPoints.length > 1) {
        return changeBezierTrack(Track, length, index, newJson.nodes);
      } else {
        return changeQuadraticTrack(Track, length, index, newJson.nodes);
      }
    });
    newJson.edges = [...newJson.edges, ...circularArcTracks];
  },
  TransformSite: (mapJson, storages) => {
    // 1. 创建以 id 为键的映射表
    const storageMap = new Map(storages.map(item => [item.id, item]));
    // 2. 遍历 mapJson.nodes，更新匹配的节点
    mapJson.nodes.forEach(node => {
      if (storageMap.has(node.id)) {
        const storageItem = storageMap.get(node.id);
        node.base.point.x;
        node = {
          ...storage, // 为每个site生成一个新的UUID
          ...{
            index,
            id: node.id,
            base: {
              visible: true,
              point: {
                x: site.x + AbsMinX + MapPaddingLeft - storage.base.size.w / 2,
                y:
                  site.y * -1 +
                  AbsMinY +
                  MapPaddingTop -
                  storage.base.size.h / 2
              },
              size: {
                w: storage.base.size.w,
                h: storage.base.size.h
              },
              globalAlpha: storage.base.globalAlpha,
              lineWidth: storage.base.lineWidth,
              lineDash: [0],
              strokeStyle: storage.base.strokeStyle,
              fillStyle: storage.base.fillStyle,
              center: {
                x: site.x + 100 + AbsMinX + MapPaddingLeft,
                y: site.y * -1 + 100 + AbsMinY + MapPaddingTop
              }
            },
            data: {
              nodeId: node.id,
              sequenceId: 0,
              nodeDescription: "",
              released: true,
              nodePosition: {
                x: site.x + 100 + AbsMinX + MapPaddingLeft,
                y: site.y * -1 + AbsMinY + MapPaddingTop,
                mapId: newJson.map.id
              },
              actions: []
            },
            code: {
              visible: true,
              globalAlpha: 1,
              font: storage.code.font,
              fillStyle: storage.code.fillStyle,
              text: node.id + ""
            },
            name: {
              visible: true,
              globalAlpha: 1,
              font: "50px Arial",
              fillStyle: "#ffffff",
              text: storageItem.name
            }
          }
        };
      }
    });
    // return mapJson;
    // console.log(mapJson, storages);
  }
};

// storage

function changeBezierTrack(Track, length, index, newJsonNodes) {
  // let id = uuidv4();
  let id = formattedDate + Track.id;
  let startNodeId = findNodeIdByCode(newJsonNodes, Track, true);
  let endNodeId = findNodeIdByCode(newJsonNodes, Track, false);
  return {
    ...normEdgeBezier,
    ...{
      index: index + length,
      id,
      base: {
        visible: true,
        point: {
          x: 395.1704156693407,
          y: 336.5853906255138
        },
        size: {
          w: 105.58514718667779,
          h: 105.58514718667779
        },
        globalAlpha: normEdgeBezier.base.globalAlpha,
        lineWidth: normEdgeBezier.base.lineWidth,
        lineDash: [0],
        strokeStyle: normEdgeBezier.base.strokeStyle,
        fillStyle: normEdgeBezier.base.fillStyle,
        center: {
          x: 419.22160751011256,
          y: 385.2436317427552
        },
        isOneway: Track.direction === "both" ? false : true,
        width: 0,
        // isb2a: track.direction === "b2a",
        startNode: {
          // id: Track.a + ""
          id: startNodeId
        },
        endNode: {
          // id: Track.b + ""
          id: endNodeId
        },
        p1: {
          x: Track.controlPoints[0].x + AbsMinX + MapPaddingLeft,
          y: Track.controlPoints[0].y * -1 + AbsMinY + MapPaddingTop
        },
        p2: {
          x: Track.controlPoints[1].x + AbsMinX + MapPaddingLeft,
          y: Track.controlPoints[1].y * -1 + AbsMinY + MapPaddingTop
        }
      },
      code: {
        text: Track.a + "" + Track.b
      },
      data: {
        edgeId: id,
        sequenceId: 0,
        edgeDescription: "",
        released: true,
        // startNodeId: Track.a + "",
        // endNodeId: Track.b + "",
        startNodeId: startNodeId,
        endNodeId: endNodeId,
        maxSpeed: 0,
        maxHeight: 0,
        minHeight: 0,
        orientation: 0,
        orientationType: "TANGENTIAL",
        direction: "",
        rotationAllowed: true,
        maxRotationSpeed: 0,
        length: 244.3960116826732,
        trajectory: {
          degree: 2,
          knotVector: [0],
          controlPoints: [
            {
              x:
                Track.direction !== "b2a"
                  ? Track.controlPoints[0].x + AbsMinX + MapPaddingLeft
                  : Track.controlPoints[1].x + AbsMinX + MapPaddingLeft,
              y:
                Track.direction !== "b2a"
                  ? Track.controlPoints[0].y * -1 + AbsMinY + MapPaddingTop
                  : Track.controlPoints[1].y * -1 + AbsMinY + MapPaddingTop
            },
            {
              x:
                Track.direction !== "b2a"
                  ? Track.controlPoints[1].x + AbsMinX + MapPaddingLeft
                  : Track.controlPoints[0].x + AbsMinX + MapPaddingLeft,
              y:
                Track.direction !== "b2a"
                  ? Track.controlPoints[1].y * -1 + AbsMinY + MapPaddingTop
                  : Track.controlPoints[0].y * -1 + AbsMinY + MapPaddingTop
            }
          ]
        },
        actions: []
      },
      extends: []
    }
  };
}
function changeQuadraticTrack(Track, length, index, newJsonNodes) {
  // let id = uuidv4();
  let id = formattedDate + Track.id;
  let startNodeId = findNodeIdByCode(newJsonNodes, Track, true);
  let endNodeId = findNodeIdByCode(newJsonNodes, Track, false);
  return {
    ...normEdgeQuadratic,
    ...{
      index: index + length,
      id,
      base: {
        visible: true,
        point: {
          x: 450.2614235012283,
          y: 340.4972472534005
        },
        size: {
          w: 105.58514718667779,
          h: 105.58514718667779
        },
        globalAlpha: normEdgeQuadratic.base.globalAlpha,
        lineWidth: normEdgeQuadratic.base.lineWidth,
        lineDash: [0],
        strokeStyle: normEdgeQuadratic.base.strokeStyle,
        fillStyle: normEdgeQuadratic.base.fillStyle,
        center: {
          x: Track.controlPoints[0].x + AbsMinX + MapPaddingLeft,
          y: Track.controlPoints[0].y * -1 + AbsMinY + MapPaddingTop
        },
        isOneway: Track.direction === "both" ? false : true,
        width: normEdgeQuadratic.base.width,
        // isb2a: track.direction === "b2a",
        startNode: {
          id: startNodeId
        },
        endNode: {
          id: endNodeId
        },
        p1: {
          x: Track.controlPoints[0].x + AbsMinX + MapPaddingLeft,
          y: Track.controlPoints[0].y * -1 + AbsMinY + MapPaddingTop
        }
      },
      code: {
        text: Track.a + "" + Track.b
      },
      name: {
        text: ""
      },
      lock: {
        enable: false
      },
      data: {
        edgeId: id,
        sequenceId: 0,
        edgeDescription: "",
        released: true,
        startNodeId: startNodeId,
        endNodeId: endNodeId,
        maxSpeed: 0,
        maxHeight: 0,
        minHeight: 0,
        orientation: 0,
        orientationType: "TANGENTIAL",
        direction: "",
        rotationAllowed: true,
        maxRotationSpeed: 0,
        length: 275.812439386991,
        trajectory: {
          degree: 1,
          knotVector: [0],
          controlPoints: [
            {
              x: Track.controlPoints[0].x + AbsMinX + MapPaddingLeft,
              y: Track.controlPoints[0].y * -1 + AbsMinY + MapPaddingTop
            }
          ]
        },
        actions: []
      },
      extends: []
    }
  };
}

export { mapTransform };
