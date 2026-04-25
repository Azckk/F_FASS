import { v4 as uuidv4 } from "uuid";

const normMap = {
  index: 0,
  id: "03c26b2a-2a0d-44d8-9c57-6462e2574593",
  kind: "Map",
  type: "MapRect",
  base: {
    visible: true,
    size: {
      w: 130000,
      h: 90000
    },
    globalAlpha: 1,
    lineWidth: 1,
    lineDash: [0],
    strokeStyle: "#808080",
    fillStyle: "#91caff",
    center: {
      x: 65000,
      y: 45000
    },
    version: "2.0"
  },
  image: {
    visible: true,
    globalAlpha: 1,
    src: ""
  },
  extends: []
};
const normNode = {
  index: 2,
  id: "5dd9824b-3559-422a-8bee-41b5a4422817",
  kind: "Node",
  type: "NodeRect",
  base: {
    visible: true,
    point: {
      x: 260.34375,
      y: 304
    },
    size: {
      w: 40,
      h: 40
    },
    globalAlpha: 1,
    lineWidth: 1,
    lineDash: [0],
    strokeStyle: "#000000",
    fillStyle: "#FFFFFF",
    center: {
      x: 280.34375,
      y: 324
    }
  },
  code: {
    visible: true,
    globalAlpha: 1,
    font: "10px Arial",
    fillStyle: "#000000",
    text: "3"
  },
  name: {
    visible: true,
    globalAlpha: 1,
    font: "10px Arial",
    fillStyle: "#000000",
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
    nodeId: "5dd9824b-3559-422a-8bee-41b5a4422817",
    sequenceId: 0,
    nodeDescription: "",
    released: true,
    nodePosition: {
      x: 280.34375,
      y: 324,
      mapId: "c54379f2-e5fb-4356-9b36-98f6bb8011e3"
    },
    actions: []
  },
  extends: []
};
const normEdge = {
  index: 2,
  id: "06081b89-6941-4f3e-b7ef-e701cc01359d",
  kind: "Edge",
  type: "EdgeBeeline",
  base: {
    visible: true,
    point: {
      x: 208.74375,
      y: 145.1
    },
    size: {
      w: 3,
      h: 3
    },
    globalAlpha: 1,
    lineWidth: 1,
    lineDash: [0],
    strokeStyle: "#000000",
    fillStyle: "#000000",
    center: {
      x: 234.84375,
      y: 151
    },
    isOneway: false,
    width: 0,
    startNode: {
      id: "b2224652-d21c-4bb9-b51f-df9c6340a63d"
    },
    endNode: {
      id: "562787be-c6c0-45e7-9bdd-32c55bc5ba89"
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
    edgeId: "06081b89-6941-4f3e-b7ef-e701cc01359d",
    sequenceId: 0,
    edgeDescription: "",
    released: true,
    startNodeId: "b2224652-d21c-4bb9-b51f-df9c6340a63d",
    endNodeId: "562787be-c6c0-45e7-9bdd-32c55bc5ba89",
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
  },
  extends: []
};

const mapTransform = {
  init: Json => {
    let newJson = {};
    let mapJson = Json;
    mapTransform.changeMap(mapJson, newJson);
    mapTransform.changeSites(mapJson, newJson);
    mapTransform.changeTracks(mapJson, newJson);
    return newJson;
  },
  changeMap: (mapJson, newJson) => {
    newJson.map = { ...normMap, id: uuidv4() }; // 假设传入mapJson有map属性
    console.log("Map changed:", mapJson, newJson);
  },
  changeSites: (mapJson, newJson) => {
    // 提取所有x和y的值
    const xValues = mapJson.sites.map(site => site.x);
    const yValues = mapJson.sites.map(site => -site.y); // 对y取负
    // 获取x的最小值和最大值
    const minX = Math.min(...xValues);
    const maxX = Math.max(...xValues);
    // 获取y取负后的最小值和最大值
    const minY = Math.min(...yValues);
    const maxY = Math.max(...yValues);
    const AbsMinX = Math.abs(minX);
    const AbsMaxX = Math.abs(maxX);
    const AbsMinY = Math.abs(minY);
    const AbsMaxY = Math.abs(maxY);
    newJson.map.base.size.w = AbsMinX + AbsMaxX;
    newJson.map.base.size.h = AbsMinY + AbsMaxY;
    newJson.map.base.center.x = AbsMinX + AbsMaxX / 2;
    newJson.map.base.center.y = AbsMinY + AbsMaxY / 2;
    newJson.nodes = mapJson.sites.map((site, index) => ({
      ...normNode, // 为每个site生成一个新的UUID
      ...{
        index,
        id: site.id,
        base: {
          visible: true,
          point: {
            x: site.x + AbsMinX,
            y: site.y * -1 + AbsMinY
          },
          size: {
            w: 100,
            h: 100
          },
          globalAlpha: 1,
          lineWidth: 1,
          lineDash: [0],
          strokeStyle: "#000000",
          fillStyle: "#FFFFFF",
          center: {
            x: site.x + 50 + AbsMinX,
            y: site.y * -1 + 50 + AbsMinY
          }
        },
        data: {
          nodeId: site.id,
          sequenceId: 0,
          nodeDescription: "",
          released: true,
          nodePosition: {
            x: site.x + 50 + AbsMinX,
            y: site.y * -1 + 50 + AbsMinY,
            mapId: newJson.map.id
          },
          actions: []
        },
        code: {
          visible: true,
          globalAlpha: 1,
          font: "10px Arial",
          fillStyle: "#000000",
          text: site.id
        }
      }
    }));
    console.log("Sites changed:", mapJson, newJson);
  },
  changeTracks: (mapJson, newJson) => {
    newJson.edges = mapJson.tracks.map((track, index) => ({
      ...normEdge,
      ...{
        id: track.id,
        index,
        base: {
          visible: true,
          point: {
            x: 208.74375,
            y: 145.1
          },
          size: {
            w: 3,
            h: 3
          },
          globalAlpha: 1,
          lineWidth: 10,
          lineDash: [0],
          strokeStyle: "#000000",
          fillStyle: "#000000",
          center: {
            x: 234.84375,
            y: 151
          },
          isOneway: false,
          width: 0,
          startNode: {
            id: track.a
          },
          endNode: {
            id: track.b
          }
        },
        code: {
          text: track.a + "-" + track.b
        },
        data: {
          edgeId: track.id,
          sequenceId: 0,
          edgeDescription: "",
          released: true,
          startNodeId: track.a,
          endNodeId: track.b,
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
    }));
    /*
    newJson.edges.push(
      {
        index: 33,
        id: 999,
        kind: "Edge",
        type: "EdgeQuadratic",
        base: {
          visible: true,
          point: {
            x: 2665.85218,
            y: 1689.29
          },
          size: {
            w: 20.1635051258504387,
            h: 20.1635051258504387
          },
          globalAlpha: 1,
          lineWidth: 20,
          lineDash: [0],
          strokeStyle: "#000000",
          fillStyle: "#000000",
          center: {
            x: 2665.85218,
            y: 1689.29
          },
          isOneway: false,
          width: 0,
          startNode: {
            id: 280
          },
          endNode: {
            id: 291
          },
          p1: {
            x1: 2335.85218,
            y1: 1189.29,
            x2: 50,
            y2: 1676.2928100000001,
            x: 2665.85218,
            y: 1689.29
          }
        },
        code: {
          text: "280-291"
        },
        name: {
          text: ""
        },
        lock: {
          enable: false
        },
        data: {
          edgeId: "70d4870a-65b4-4125-af92-e200ca2fe942",
          sequenceId: 0,
          edgeDescription: "",
          released: true,
          startNodeId: "1f43d2d9-4423-4048-92fc-e4e5fe8189c5",
          endNodeId: "d5698203-5574-47d3-83df-46129a319512",
          maxSpeed: 0,
          maxHeight: 0,
          minHeight: 0,
          orientation: 0,
          orientationType: "TANGENTIAL",
          direction: "",
          rotationAllowed: true,
          maxRotationSpeed: 0,
          length: 195.15370107582905,
          trajectory: {
            degree: 1,
            knotVector: [0],
            controlPoints: [
              {
                x: 2665.85218,
                y: 1689.29
              }
            ]
          },
          actions: []
        },
        extends: []
      },
      {
        index: 8,
        id: 1000,
        kind: "Edge",
        type: "EdgeArc",
        base: {
          visible: true,
          point: {
            x: 309.26366742387677,
            y: 578.0794442594703
          },
          size: {
            w: 2.1635051258504387,
            h: 2.1635051258504387
          },
          globalAlpha: 1,
          lineWidth: 1,
          lineDash: [0],
          strokeStyle: "#000000",
          fillStyle: "#000000",
          center: {
            x: 375.94836846583047,
            y: 516.934645019702
          },
          isOneway: false,
          width: 0,
          startNode: {
            id: 301
          },
          endNode: {
            id: 293
          },
          p1: {
            x1: 350.6445031270298,
            y1: 604.3477159455781,
            x2: 393.6445031270298,
            y2: 439.3477159455781,
            x: 350.6445031270298,
            y: 604.3477159455781
          }
        },
        code: {
          text: "5-6"
        },
        name: {
          text: ""
        },
        lock: {
          enable: false
        },
        data: {
          edgeId: "dd6a8bfb-b713-447d-8750-cf7d7e96792d",
          sequenceId: 0,
          edgeDescription: "",
          released: true,
          startNodeId: "d12e9763-41e7-4632-bf74-916515bda710",
          endNodeId: "e3664a84-9f2b-40fe-9137-93022193e8aa",
          maxSpeed: 0,
          maxHeight: 0,
          minHeight: 0,
          orientation: 0,
          orientationType: "TANGENTIAL",
          direction: "",
          rotationAllowed: true,
          maxRotationSpeed: 0,
          length: 282.4202761262705,
          trajectory: {
            degree: 1,
            knotVector: [0],
            controlPoints: [
              {
                x: 350.6445031270298,
                y: 604.3477159455781
              }
            ]
          },
          actions: []
        },
        extends: []
      }
    );
    */
    console.log("Tracks changed:", mapJson, newJson);
  }
};

export { mapTransform };
